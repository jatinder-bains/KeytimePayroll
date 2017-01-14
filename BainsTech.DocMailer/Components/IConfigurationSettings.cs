﻿namespace BainsTech.DocMailer.Components
{
    internal interface IConfigurationSettings
    {
        string DocumentsLocation { get; }
        string DocumentExtension { get; }

        string GetEmailForCompany(string companyName);
    }


}