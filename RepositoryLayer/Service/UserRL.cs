using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Utility;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRL(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }   

        public UserEntity AddUsers(UserML model)
        {
            UserEntity userEntity = new UserEntity();

            userEntity.Name = model.Name;
            userEntity.Email = model.Email;
            userEntity.Password = model.Password;
            userEntity.PhoneNumber = model.PhoneNumber;

            if( userEntity.PhoneNumber == null )
            {
                throw new UserException("Phone Number Required");
            }

            _context.Users.Add(userEntity);
            _context.SaveChanges();

            

            return userEntity;
        }

        public UserEntity RegisterNewUser(UserML model)
        {

            var findUser = _context.Users.Where(u => u.Email == model.Email).FirstOrDefault();

            UserEntity userEntity;

            if (findUser == null)
            {
                //var hashedPassword = PasswordHasher.HashPassword(model.Password);
                //model.Password = hashedPassword;

                model.Password = PasswordService.HashPassword(model.Password);

                userEntity = new UserEntity()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber
                };
                

                _context.Users.Add(userEntity);
                _context.SaveChanges();
            }
            else
            {
                throw new UserException("User Already Exists");
            }

            return userEntity;
        }


        public string LoginUser(LoginML model)
        {
            var result = _context.Users.Where(u => u.Email == model.Email).FirstOrDefault();

            if (result == null)
            {
                throw new UserException("Invalid Email/Password");
            }

            //if(PasswordHasher.VerifyPassword(result.Password, model.Password))
            //{
            //    return result;
            //}

            if(PasswordService.VerifyPassword(model.Password, result.Password))
            {
                //return result; 
                
                //jwt token return 

                var claims = new List<Claim> 
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWT:Subject"]),
                    new Claim("Id", result.Id.ToString()),
                    new Claim("Email", result.Email)
                };

                var userRoles = _context.UserRoles.Where(u => u.UserId == result.Id).ToList();
                var roleIds = userRoles.Select(s => s.RoleId).ToList();
                var roles = _context.Roles.Where(r => roleIds.Contains(r.Id)).ToList();

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                        _configuration["JWT:Issuer"],
                        _configuration["JWT:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn
                    );

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                return jwtToken;
            }
            else
            {
                throw new UserException("Invalid Email/Password");
            }


            //return result;
        }

        public Role AddRole(Role role)
        {
            var findRole = _context.Roles.Where(r => r.Name == role.Name).FirstOrDefault();

            if(findRole == null)
            {
                var addRole = _context.Roles.Add(role);
                _context.SaveChanges();

                return addRole.Entity;
            }
            else
            {
                throw new UserException("Role Already Exists");
            }
        }

        public bool AssignRoleToUser(AddUserRoleML obj)
        {
            var user = _context.Users.SingleOrDefault(s => s.Id == obj.UserId);

            if(user == null)
            {
                throw new UserException("User not valid");
            }

            var addRoles = new List<UserRole>();

            foreach (int role in obj.RoleIds)
            {
                var userRole = new UserRole();
                userRole.RoleId = role;
                userRole.UserId = user.Id;

                addRoles.Add(userRole);
            }

            _context.UserRoles.AddRange(addRoles);
            _context.SaveChanges();

            return true;
        }
    }
}
