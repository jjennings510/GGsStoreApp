using GGsDB.Models;

namespace GGsLib
{
    public interface IUserService
    {
        void AddUser(User user);
        void DeleteUser(int id);
        User GetUserByEmail(string email);
        User GetUserById(int id);
        void UpdateUser(User user);
    }
}