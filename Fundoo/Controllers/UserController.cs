using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ModelLayer;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Fundoo.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        private readonly ResponseML responseML;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
            responseML = new ResponseML();
        }

        [HttpPost("adduser")]
        public IActionResult AddUser(UserML model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Any())
                                .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                    return BadRequest(new ResponseML { Success = false, Message = "Validation Occured in {Phone number}", Data = null });
                }

                var result = userBL.AddUsers(model);
                responseML.Success = true;
                responseML.Message = "User Added Successfully";
                responseML.Data = result;

                if (result == null)
                {
                    responseML.Success = false;
                    responseML.Message = "Error  Ocurred while adding user";
                    return StatusCode(500, responseML);
                }
            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(500, responseML);
            }

            return StatusCode(200, responseML);
        }


        [HttpPost("register")]
        public IActionResult RegisterNewUser(UserML model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, "Invalid Format");
                }

                var result = userBL.RegisterNewUser(model);

                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "User Registered Successfully";
                    responseML.Data = result;
                }

            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
            catch(SqlException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }

            return StatusCode(201, responseML);
        }


        [HttpPost("login")]
        public IActionResult LoginUser(LoginML model)
        {
            try
            {
                var token = userBL.LoginUser(model);

                responseML.Success = true;
                responseML.Message = "User Login Successfully";
                responseML.Data = token;

                return StatusCode(200, responseML);
            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
            catch (SqlException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
        }

        [HttpPost("assignrole")]
        public IActionResult AssignRoleToUser([FromBody] AddUserRoleML model)
        {
            try
            {
                var addedUserRole = userBL.AssignRoleToUser(model);

                return Ok(addedUserRole);
            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpPost("addrole")]
        [Authorize(Roles ="Admin")]
        public IActionResult AddRole([FromBody] Role role)
        {
            try
            {
                var addedRole = userBL.AddRole(role);

                return Ok(addedRole);
            }
            catch(UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpGet("forget-password/{email}")]
        public IActionResult ForgetPassword(string email)
        {
            try
            {
                userBL.ForgetPassword(email);

                responseML.Success = true;
                responseML.Message = "Email Sent Successfully";

                return Ok(responseML);
            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpPost("reset-password/{token}")]
        public IActionResult ResetPassword(string token, [FromBody] ResetPasswordML model)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;

                Console.WriteLine(emailClaim);

                userBL.ResetPassword(emailClaim, model.Password);

                responseML.Success = true;
                responseML.Message = "Password Changed Successfully";

                return StatusCode(200,responseML);
            }
            catch(UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }
    }
}