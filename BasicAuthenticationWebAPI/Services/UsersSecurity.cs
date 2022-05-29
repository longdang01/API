using BasicAuthenticationWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicAuthenticationWebAPI.Services
{
    public class UsersSecurity
    {
        public static bool Login(string username, string password)
        {
            using (EmployeeManagerEntities db = new EmployeeManagerEntities())
            {
                try
                {
                    return db.Users.Any(item => item.UserName == username
                    && item.Password == password);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static User GetUsersDetail(string username, string password)
        {
            using (EmployeeManagerEntities db = new EmployeeManagerEntities())
            {
                try
                {
                    return db.Users.SingleOrDefault(item =>
                    item.UserName == username && item.Password == password);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}