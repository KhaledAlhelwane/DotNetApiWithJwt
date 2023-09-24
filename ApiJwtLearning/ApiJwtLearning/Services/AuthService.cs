using ApiJwtLearning.Helpers;
using ApiJwtLearning.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiJwtLearning.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtMapingValue _jwt;
        public AuthService(UserManager<ApplicationUser> userManager,IOptions<JwtMapingValue> JWT, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwt = JWT.Value;
            _roleManager = roleManager;
        }

      

        public async Task<AuthModel> RejesterAsync(RegisterModel model)
        {
            if ( await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel { meassage = "Email is already registerd" };
            }
            if (await _userManager.FindByNameAsync(model.UserName) is not null)
            {
                return new AuthModel { meassage = "the UserName is already registerd" };
            }
            var user=new ApplicationUser {
            UserName = model.UserName,
            Email=model.Email,
            FirstName=model.firstName,
            SecoundName=model.secoundName,            
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = String.Empty;
                foreach(var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }

                return new AuthModel { meassage = errors };
            }
            await _userManager.AddToRoleAsync(user, "User");
            var JwtSecurityTokken= await CreateJwtTokenAsync(user);
            return new AuthModel
            {
                Eamil = user.Email,
                IsAuthenticated = true,
                UserName = user.UserName,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityTokken),
                ExpiresOn = JwtSecurityTokken.ValidTo
            };

           

        }

     public async Task<AuthModel> GetTokkenAsync(TokkenRequistModel model)
        {
            var Authobj = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null )
            {
                Authobj.IsAuthenticated = false;
                Authobj.meassage = "Error in log in or in password";
               return Authobj;
            }
         if (!await _userManager.CheckPasswordAsync(user, model.PassWord))
            {
                Authobj.IsAuthenticated = false;
                Authobj.meassage = "Error in log in or in password";
                return Authobj;
            }
           var JwtSecurityTokken=await CreateJwtTokenAsync(user);
            Authobj.Token= new JwtSecurityTokenHandler().WriteToken(JwtSecurityTokken);
            Authobj.UserName = user.UserName;
            Authobj.ExpiresOn = JwtSecurityTokken.ValidTo; 
            Authobj.Eamil= user.Email;  
            Authobj.IsAuthenticated = true;


            return Authobj;


        }
        private async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issure,
                audience: _jwt.Audiance,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        //to add role to a specific User
        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return "the user Id is not found!!!";
            }
            if(!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return "there is no Role with this name"+ model.RoleName;
            }
            if(await _userManager.IsInRoleAsync(user, model.RoleName))
            {
                return "user is already has this role";
            }

        var result= await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return "Succeeded";
            }
            else
            {
                return "Somthing went wrong";
            }
        }

    }
}
