﻿using System;
using System.Security.Cryptography;
using System.Text;

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

        public static string Derypt(this string text)
        {
            return Encoding.Unicode.GetString(
                ProtectedData.Unprotect(
                    Convert.FromBase64String(text), null, DataProtectionScope.LocalMachine));
        }
    }
}