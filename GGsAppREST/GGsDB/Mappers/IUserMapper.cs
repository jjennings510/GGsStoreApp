using GGsDB.Entities;
using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Mappers
{
    public interface IUserMapper
    {
        User ParseUser(Users user);
        Users ParseUser(User user);
        List<User> ParseUser(ICollection<Users> users);
        ICollection<Users> ParseUser(List<User> users);
    }
}