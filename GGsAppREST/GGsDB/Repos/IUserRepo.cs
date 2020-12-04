using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Repos
{
    public interface IUserRepo
    {
        void AddUser(User user);
        void UpdateUser(User user);
        List<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByEmail(string email);
        void DeleteUser(int id);
    }
}