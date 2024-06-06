using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
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


        public UserEntity LoginUser(LoginML model)
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
    }
}
