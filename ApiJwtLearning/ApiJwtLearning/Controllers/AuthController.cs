using ApiJwtLearning.Model;
using ApiJwtLearning.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using NuGet.Protocol;

namespace ApiJwtLearning.Controllers
{
    [Route("Api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {

            _authService = authService;

        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RejesterAsync(model);
            if(!result.IsAuthenticated)
                return BadRequest(result.meassage);

            //return Ok(result);
            // we can return anonmys object as to our situation the message will be null so here we go
            return Ok(new { Tooken = result.Token, Email = result.Eamil, UserName = result.UserName, ExpireOne = result.ExpiresOn });

        }


        [HttpPost("Token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokkenRequistModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokkenAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.meassage);

           return Ok(new { Tooken = result.Token, Email = result.Eamil, UserName = result.UserName, ExpireOne = result.ExpiresOn });

        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            return Ok(result);


        }
    }
}
