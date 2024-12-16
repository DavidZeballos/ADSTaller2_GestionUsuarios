using src.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Application.UseCases
{
    public class GetUserProgress
    {
        private readonly IUserRepository _userRepository;

        public GetUserProgress(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<string>> ExecuteAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} was not found.");

            return user.SubjectIds?.ToList() ?? new List<string>();
        }
    }
}
