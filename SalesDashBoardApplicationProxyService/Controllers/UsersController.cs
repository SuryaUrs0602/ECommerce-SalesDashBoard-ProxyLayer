using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesDashBoardApplication;
using SalesDashBoardApplicationProxyService.Models;

namespace SalesDashBoardApplicationProxyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SalesDashBoardClient _salesDashBoardClient;
        private readonly ILogger<UsersController> _logger;


        public UsersController(SalesDashBoardClient salesDashBoardClient, ILogger<UsersController> logger)
        {
            _salesDashBoardClient = salesDashBoardClient;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            try
            {
                _logger.LogInformation("registering a new user");
                await _salesDashBoardClient.RegisterAsync(userRegisterDto);
                return Ok();
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Registering the user");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            try
            {
                _logger.LogInformation("Logging a user");
                var user = await _salesDashBoardClient.LoginAsync(userLoginDto);

                _logger.LogInformation($"jwt token : {user.Token}");

                if (user == null)
                {
                    return Unauthorized("Invalid Credentials");
                }

                

                return Ok(new { userId = user.UserId, userName = user.UserName, userEmail = user.UserEmail, userRole = user.UserRole, token = user.Token });
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while logging the user");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }




        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                _logger.LogInformation("Getting all Users data");

                var users = await _salesDashBoardClient.UsersAllAsync();
                return Ok(users);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while fetching all the users data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                _logger.LogInformation("Getting user data by id");
                var user = await _salesDashBoardClient.UsersGETAsync(id);
                return Ok(user);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while fetching user data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePasswordOfUser(int id, UserPasswordChangeDto userPasswordChangeDto)
        {
            try
            {
                _logger.LogInformation("Changing the user password");
                await _salesDashBoardClient.ChangePasswordAsync(id, userPasswordChangeDto);
                return NoContent();
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while changing the user password");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserDetails(int id, JsonPatchDocument<UserUpdateDto> patchDocument)
        {
            try
            {
                _logger.LogInformation("Updating the user details");
                var operations = patchDocument.Operations
                    .Select(op => new SalesDashBoardApplication.Operation
                    {
                        Path = op.path,
                        Op = op.op,
                        From = op.from,
                        Value = op.value
                    });
                await _salesDashBoardClient.UsersPATCHAsync(id, operations);
                return NoContent();
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while changing the user details");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }
    }
}
