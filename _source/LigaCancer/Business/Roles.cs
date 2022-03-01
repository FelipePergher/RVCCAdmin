// <copyright file="Roles.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Business
{
    public static class Roles
    {
        public const string Admin = "Admin";

        public const string Secretary = "Secretary";

        public const string SocialAssistance = "SocialAssistance";

        public const string AdminSecretaryAuthorize = "Admin, Secretary";

        public const string AdminSecretarySocialAssistanceAuthorize = "Admin, Secretary, SocialAssistance";

        public static string GetRoleName(string role)
        {
            string userRoleName = role switch
            {
                Secretary => "Secretária",
                SocialAssistance => "Assistente Social",
                Admin => "Administrador",
                _ => string.Empty
            };

            return userRoleName;
        }
    }
}