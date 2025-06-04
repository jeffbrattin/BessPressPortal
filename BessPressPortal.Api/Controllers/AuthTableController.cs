using BessPressPortal.Api.Services;
using global::BessPressPortal.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BessPressPortal.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthTableController : Controller
    {
        private readonly AuthenticationTableService _authenticationTableService;
        public AuthTableController(AuthenticationTableService authenticationTableService)
        {
            _authenticationTableService = authenticationTableService;
        }

        [HttpPost("adduser")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<bool>>> AddUser([FromBody] CreateUserDto createUserDto)
        {

            Console.WriteLine("AddUser endpoint hit");


            try
            {
                var result = await _authenticationTableService.AddUserAsync(createUserDto);

                if (!result.Success)
                    return Conflict(result);

                return Created(string.Empty, result);
            }
            catch (Exception ex)
            {
                // Consider logging here
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}",
                    Data = false
                });
            }
        }


    }
}
