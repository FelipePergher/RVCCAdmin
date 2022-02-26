// <copyright file="UserResolverService.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace RVCC.Business.Services
{
    public class UserResolverService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<UserResolverService> _logger;

        public UserResolverService(IHttpContextAccessor context, ILogger<UserResolverService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public string GetUserName()
        {
            try
            {
                return this._context.HttpContext?.User.Identity?.Name;
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "Failed to get user.");
                throw;
            }
        }
    }
}
