using DotnetAPI.DTOs;
using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public interface IUserRepository
    {
        bool SaveChanges();
        void AddEntity<T>(T entityToAdd);
        void RemoveEntity<T>(T entityToRemove);
        void UpdateEntity<T>(T entityToUpdate);
        IEnumerable<User> GetUsers();
        User GetSingleUser(int userId);
        UserJobInfo GetUserJobInfo(int userId);
        UserSalary GetUserSalary(int userId);
    }
}