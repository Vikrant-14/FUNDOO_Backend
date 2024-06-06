using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly ApplicationDbContext _context;

        public UserRL(ApplicationDbContext context)
        {
            _context = context;
        }   

        public UserEntity AddUsers(UserML model)
        {
            UserEntity userEntity = new UserEntity();

            userEntity.Name = model.Name;
            userEntity.Email = model.Email;
            userEntity.Password = model.Password;
            userEntity.PhoneNumber = model.PhoneNumber;

            if( userEntity.PhoneNumber == null)
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

       
    }
}
