using System.Threading.Tasks;
using src.Domain.Entities;

namespace src.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
    }
}