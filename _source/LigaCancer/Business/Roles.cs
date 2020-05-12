// <copyright file="Roles.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

namespace RVCC.Business
{
    public static class Roles
    {
        public const string Admin = "Admin";

        public const string User = "User";

        public const string SocialAssistance = "SocialAssistance";

        public const string AdminUserAuthorize = "Admin, User";

        public const string AdminUserSocialAssistanceAuthorize = "Admin, User, SocialAssistance";

        public static string GetRoleName(string role)
        {
            string userRoleName = role switch
            {
                Admin => "Administrador",
                User => "Usuário",
                SocialAssistance => "Assistente Social",
                _ => string.Empty
            };

            return userRoleName;
        }
    }
}