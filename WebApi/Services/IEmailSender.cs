﻿namespace WebApi.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string messeage);
    }
}
