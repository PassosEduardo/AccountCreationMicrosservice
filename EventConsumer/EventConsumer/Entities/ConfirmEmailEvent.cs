﻿namespace EventConsumer.Entities;

public class ConfirmEmailEvent
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string EmailToken { get; set; }
}