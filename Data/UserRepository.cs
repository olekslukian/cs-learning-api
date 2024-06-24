using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class UserRepository(IConfiguration config) : IUserRepository
    {
        private readonly DataContextEF _contextEF = new(config);

        public bool SaveChanges()
        {
            return _contextEF.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _contextEF.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _contextEF.Remove(entityToRemove);
            }
        }

        public void UpdateEntity<T>(T entityToUpdate)
        {
            if (entityToUpdate != null)
            {
                _contextEF.Update(entityToUpdate);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = [.. _contextEF.Users];

            return users;
        }

        public User GetSingleUser(int userId)
        {
            User user = _contextEF.Users.Where(u => u.UserId == userId)
                .ToList()
                .FirstOrDefault() ?? throw new Exception("Failed to Get User");

            return user;
        }

        public UserJobInfo GetUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfo = _contextEF.UserJobInfo
                .Where(u => u.UserId == userId)
                .ToList()
                .FirstOrDefault() ?? throw new Exception("Failed loading user job info");

            return userJobInfo;
        }
        public UserSalary GetUserSalary(int userId)
        {
            UserSalary? userSalary = _contextEF.UserSalary
               .Where(u => u.UserId == userId)
               .ToList()
               .FirstOrDefault() ?? throw new Exception("Failed loading user salary");

            return userSalary;
        }

    }
}