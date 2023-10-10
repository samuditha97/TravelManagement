using System;
using TravelManagement.Models;

namespace TravelManagement.Interfaces
{
	public interface IUsersService
	{
        Task<UsersClass> RegisterAsyn(UsersClass user);
        Task<UsersClass> AuthenticateAsync(string username, string password);
        Task<IEnumerable<UsersClass>> GetAllUsers();
        Task<UsersClass> GetUserById(string userId);
        Task<bool> UpdateUserAsync(string userId, UsersClass user);
        Task<bool> DeleteUserAsync(string userId);
    }
}

