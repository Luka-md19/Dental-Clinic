﻿using DentalClinic.API.Data;
using Microsoft.AspNetCore.Identity;

namespace DentalClinic.Web.Services.Identity
{
    public class PasswordValidatorService : IPasswordValidator<ApiUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApiUser> manager, ApiUser user, string password)
        {
            var errors = new List<IdentityError>();

            if (password.Contains("Company Name"))
            {
                errors.Add(new IdentityError
                {
                    Code = "Company Name",
                    Description = "Password should not contain name of company"
                });
            }

            if (password.Contains(user.FirstName) || password.Contains(user.LastName))
            {
                errors.Add(new IdentityError
                {
                    Code = "Weak Password",
                    Description = "Password should not contain your name"
                });
            }

            if (password.Contains(user.UserName) || password.Contains(user.Email))
            {
                errors.Add(new IdentityError
                {
                    Code = "Weak Password",
                    Description = "Password should not contain username or email address"
                });
            }

            if (errors.Count > 0)
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}