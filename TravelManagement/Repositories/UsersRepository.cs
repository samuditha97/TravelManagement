using System;
using MongoDB.Bson;
using MongoDB.Driver;
using TravelManagement.Interfaces;
using TravelManagement.Models;

namespace TravelManagement.Repositories
{
	public class UsersRepository: IUsersService
	{

        private readonly IMongoCollection<UsersClass> _usersCollection;

        public UsersRepository(IMongoDatabase database)
        {
            _usersCollection = database.GetCollection<UsersClass>("Users");
        }

        public async Task<UsersClass> AuthenticateAsync(string username, string password)
        {
            // Implement user authentication logic using MongoDB.
            var user = await _usersCollection.Find(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            // Implement user deletion logic using MongoDB.
            var result = await _usersCollection.DeleteOneAsync(u => u.Id == ObjectId.Parse(userId));
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<UsersClass>> GetAllUsers()
        {
            // Implement logic to retrieve all users from the MongoDB collection.
            var users = await _usersCollection.Find(_ => true).ToListAsync();
            return users;
        }

        public async Task<UsersClass> GetUserById(string userId)
        {
            // Implement logic to retrieve a user by their ID from the MongoDB collection.
            var user = await _usersCollection.Find(u => u.Id == ObjectId.Parse(userId)).FirstOrDefaultAsync();
            return user;
        }

        public async Task<UsersClass> RegisterAsyn(UsersClass user)
        {
            // Implement user registration logic using MongoDB.
            await _usersCollection.InsertOneAsync(user);
            return user;
        }


        public async Task<bool> UpdateUserAsync(string userId, UsersClass user)
        {
            // Implement user update logic using MongoDB.
            var result = await _usersCollection.ReplaceOneAsync(u => u.Id == ObjectId.Parse(userId), user);
            return result.ModifiedCount > 0;
        }
    }
}

