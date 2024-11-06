using System;
using System.Threading.Tasks;
using src.Domain.Entities;
using src.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace src.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            return user ?? throw new KeyNotFoundException($"User with ID {id} was not found.");
        }

        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            Console.WriteLine($"User with ID {user.Id} successfully saved to database.");
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}