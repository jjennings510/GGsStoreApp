using GGsDB.Models;
using GGsDB.Repos;
using System;
using System.Collections.Generic;

namespace GGsLib
{
    public class UserService : IUserService
    {
        private IUserRepo repo;
        public UserService(IRepo repo)
        {
            this.repo = repo;
        }
        public void AddUser(User user)
        {
            List<User> allUsers = repo.GetAllUsers();
            foreach (var u in allUsers)
            {
                if (u.email.Equals(user.email))
                    throw new Exception("This email already exists.");
            }
            repo.AddUser(user);
        }
        public void DeleteUser(int id)
        {
            //List<User> allUsers = repo.GetAllUsers();
            //if (!allUsers.Contains(user))
            //    throw new Exception("This user does not exist and thus cannot be deleted.");
            //User user = GetUserById(id);

            repo.DeleteUser(id);
        }
        public List<User> GetAllUsers()
        {
            return repo.GetAllUsers();
        }
        public User GetUserByEmail(string email)
        {
            return repo.GetUserByEmail(email);
        }
        public User GetUserById(int id)
        {
            return repo.GetUserById(id);
        }
        public void UpdateUser(User user)
        {
            repo.UpdateUser(user);
        }
    }
}
