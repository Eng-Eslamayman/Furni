﻿namespace Furni.Web.Services
{
    public interface IEmailBodyBuilder
    {
        string GetEmailBody(string template, Dictionary<string, string> placeHolders);
    }
}
