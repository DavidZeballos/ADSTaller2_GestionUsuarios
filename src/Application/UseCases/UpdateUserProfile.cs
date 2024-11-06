using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Domain.Repositories;

namespace src.Application.UseCases
{
    public class UpdateUserProfile
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserProfile(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(Guid userId, string name, string firstLastName, string secondLastName)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                        ?? throw new KeyNotFoundException("User not found");

            user.Name = name;
            user.FirstLastName = firstLastName;
            user.SecondLastName = secondLastName;

            await _userRepository.UpdateAsync(user);
        }
    }
}