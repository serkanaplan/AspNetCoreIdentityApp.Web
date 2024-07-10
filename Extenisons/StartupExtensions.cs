﻿using AspNetCoreIdentityApp.Web.CustomValidations;
using AspNetCoreIdentityApp.Web.Localizations;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Extenisons;

public static class StartupExtensions
{
    public static void AddIdentityWithExt(this IServiceCollection services)
    {
        // şifre sıfırlama için gönderilecek token ın süresini belirler
        services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2));

        services.AddIdentity<AppUser, AppRole>(options =>
            {

              options.User.RequireUniqueEmail = true;
              options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_";

              options.Password.RequiredLength = 6;
              options.Password.RequireNonAlphanumeric = false;
              options.Password.RequireLowercase = true;
              options.Password.RequireUppercase = false;
              options.Password.RequireDigit = false;

        
              options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
              options.Lockout.MaxFailedAccessAttempts = 3;

            }).AddPasswordValidator<PasswordValidator>()
            .AddUserValidator<UserValidator>()
            .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();
    }
}
