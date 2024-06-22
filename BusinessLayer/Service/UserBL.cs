using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRL;

        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }


        public UserEntity AddUsers(UserML model)
        {
            try
            {
                return userRL.AddUsers(model);

            }
            catch (UserException) {
                throw;
            }
        }

        public UserEntity RegisterNewUser(UserML model)
        {
            try
            {
                return userRL.RegisterNewUser(model);
            }
            catch (UserException)
            {
                throw;
            }
        }


        public string LoginUser(LoginML model)
        {
            try
            {
                return userRL.LoginUser(model);
            }
            catch (UserException)
            {
                throw;
            }
        }

        public Role AddRole(Role role)
        {
            try
            {
                return userRL.AddRole(role);
            }
            catch (UserException)
            {
                throw;
            }
        }

        public bool AssignRoleToUser(AddUserRoleML obj)
        {
            try
            {
                return userRL.AssignRoleToUser(obj);
            }
            catch (UserException)
            {
                throw;
            }
        }

        public void ForgetPassword(string email)
        {
            try
            {
                userRL.ForgetPassword(email);
            }
            catch (UserException)
            {
                throw;
            }
        }

        public void ResetPassword(string email, string password)
        {
            try
            {
                userRL.ResetPassword(email,password);
            }
            catch (UserException)
            {
                throw;
            }
        }

        public UserEntity GetUserById(int id)
        {
            try
            {
                return userRL.GetUserById(id);
            }
            catch (UserException)
            {
                throw;
            }
        }
    }
}
