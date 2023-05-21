using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Service
{
    public class UserService
    {
        private readonly UserDAO userDAO;
        public UserService(UserDAO userDAO)
        {
            this.userDAO = userDAO;
        }

        public bool IsAuth(User user)
        {
            bool result = false;

            userDAO.FindAll().ForEach(us =>
            {
                if (BCrypt.Net.BCrypt.Verify(user.Password, us.Password))
                {
                    result = true;
                }
            });

            return result;
        }

        public User Create(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            return userDAO.Create(user);
        }
    }
}