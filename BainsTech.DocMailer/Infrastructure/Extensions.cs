using System;
using System.Security.Cryptography;
using System.Text;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Infrastructure
{
    internal static class Extensions
    {
        public static string Encrypt(this string text)
        {

            return Convert.ToBase64String(
                ProtectedData.Protect(
                    Encoding.Unicode.GetBytes(text), null, DataProtectionScope.LocalMachine));
        }

        public static string Decrypt(this string text)
        {
            return Encoding.Unicode.GetString(
                ProtectedData.Unprotect(
                    Convert.FromBase64String(text), null, DataProtectionScope.LocalMachine));
        }


        public static string ToDisplayString(this DocumentStatus documentStatus)
        {
            switch (documentStatus)
            {
                case DocumentStatus.ReadyToSend:
                    return "Ready to send";
                case DocumentStatus.IncompatibleFileName:
                    return "Incompatible file name";
                case DocumentStatus.NoMappedEmail:
                    return "No mapped email";
                default:
                    return "Unknown ?";
            }
        }
    }
}