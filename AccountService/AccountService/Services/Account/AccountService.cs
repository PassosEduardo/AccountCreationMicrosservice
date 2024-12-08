using AccountService.Entities;
using AccountService.Helpers;
using AccountService.Infrastructure.MongoInfrastrcuture;
using AccountService.Requests;
using AccountService.Services.Kafka;
using FluentResults;
using MongoDB.Driver;
using System.Text.Json;

namespace AccountService.Services.Account;

public class AccountService : IAccountService
{
    private readonly IMongoCollection<AccountEntity> _mongoCollection;
    private readonly IKafkaService _kafkaService;

    public AccountService(IMongoRepository mongoRepository, IKafkaService kafkaService)
    {
        _mongoCollection = mongoRepository.Database.GetCollection<AccountEntity>(nameof(AccountEntity));
        _kafkaService = kafkaService;
    }

    public async Task<Result<bool>> ConfirmEmailAsync(string accountId, string confirmationToken)
    {
        var findResult = await _mongoCollection.FindAsync(x =>
            string.Equals(x.Id, accountId) && string.Equals(x.EmailToken, confirmationToken));

        var result = findResult.FirstOrDefault();

        if (result is null)
            return Result.Fail("Either the email or the token is incorrect");

        result.EmailIsConfirmed = true;

        await _mongoCollection.ReplaceOneAsync(x =>
            string.Equals(x.Id, accountId),
            result);

        return true;
    }

    public async Task<Result<bool>> CreateAccountAsync(CreateAccountRequest request)
    {
        var findResult = await _mongoCollection.Find(x => string.Equals(x.Email, request.Email)).FirstOrDefaultAsync();

        if (findResult is not null)
            return Result.Fail("E-mail already in use");

        var userCredentials = SaltHandler.CreateUserCredentials(request.Password);

        var account = new AccountEntity
        {
            Email = request.Email,
            Password = userCredentials.EncryptedPassword,
            Salt = userCredentials.EncryptedSalt,
            EmailToken = Guid.NewGuid().ToString(),
            EmailIsConfirmed = false
        };

        await _mongoCollection.InsertOneAsync(account);

        await _kafkaService.PublishEmailConfirmationEventAsync(JsonSerializer.Serialize(new
        {
            Email = account.Email,
            EmailToken = account.EmailToken,
            Id = account.Id
        }));

        return true;
    }

    public async Task<Result<bool>> ReSendEmailConfirmationToken(string email)
    {
        var findResult = await _mongoCollection.Find(x =>
            string.Equals(x.Email, email) && !x.EmailIsConfirmed).FirstOrDefaultAsync();

        if (findResult is null)
            return Result.Fail("E-mail already confirmmed or not found");

        var newEmailToken = Guid.NewGuid().ToString();

        UpdateDefinition<AccountEntity> updateDefinition = new UpdateDefinitionBuilder<AccountEntity>()
            .Set(x => x.EmailToken, newEmailToken);

        FilterDefinition<AccountEntity> filterDefinition = new FilterDefinitionBuilder<AccountEntity>()
            .Where(x => x.Email == email);

        var updateResult = await _mongoCollection.FindOneAndUpdateAsync(filterDefinition, updateDefinition);

        await _kafkaService.PublishEmailConfirmationEventAsync(JsonSerializer.Serialize(new
        {
            Email = updateResult.Email,
            EmailToken = newEmailToken,
            Id = updateResult.Id
        }));

        return true;
    }

    public async Task<Result<bool>> SendEmailForPassowordReset(string email)
    {
        var findResult = await _mongoCollection.Find(x =>
            string.Equals(x.Email, email)).FirstOrDefaultAsync();

        if (findResult is null)
            return Result.Fail("E-mail already confirmmed or not found");

        UpdateDefinition<AccountEntity> updateDefinition = new UpdateDefinitionBuilder<AccountEntity>()
            .Set(x => x.PassowordResetKey, Guid.NewGuid().ToString())
            .Set(x => x.PasswordResetDateTime, DateTime.Now);

        FilterDefinition<AccountEntity> filterDefinition = new FilterDefinitionBuilder<AccountEntity>()
            .Where(x => x.Email == email);

        var updateResult = await _mongoCollection.FindOneAndUpdateAsync(filterDefinition, updateDefinition);

        await _kafkaService.PublishPasswordResetEventAsync(JsonSerializer.Serialize(new
        {
            Email = updateResult.Email,
            PassowordResetKey = updateResult.PassowordResetKey,
        }));

        return true;
    }
}
