using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public UserEntity AddUsers(UserML model);
        public UserEntity RegisterNewUser(UserML model);
        public string LoginUser(LoginML model);
        public Role AddRole(Role role);
        public bool AssignRoleToUser(AddUserRoleML obj);
        public void ForgetPassword(string email);
        public void ResetPassword(string email, string password);
        public UserEntity GetUserById(int id);
    }
}
