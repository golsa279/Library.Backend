using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Library.Backend.API.DTOs.Security;
using Library.Backend.API.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Library.Backend.API.Controllers
{
    [ApiController]
    [Route("api/security")]
    public class SecurityApiController : ControllerBase
    {
        private readonly IServiceProvider _sp;

        public SecurityApiController(IServiceProvider sp)
        {
            _sp = sp;
        }

        [HttpPost("login")]
        public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login(LoginRequestDto loginRequest)
        {
            
        
            var signInManager = _sp.GetRequiredService<SignInManager<LibraryUser>>();

            signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

            var result = await signInManager.PasswordSignInAsync( loginRequest.Username, loginRequest.Password, false, false);
 
            
            if (!result.Succeeded)
            {
                
                return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status200OK);
            }

            // The signInManager already produced the needed response in the form of a cookie or bearer token.
            return TypedResults.Empty;
        }
    }
}