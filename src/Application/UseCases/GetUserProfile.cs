using System;
using System.Threading.Tasks;
using src.Domain.Repositories;
using src.Application.DTOs;

namespace src.Application.UseCases
{
    public class GetUserProfile
    {
        private readonly IUserRepository _userRepository;

        public GetUserProfile(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> ExecuteAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found");

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                FirstLastName = user.FirstLastName,
                SecondLastName = user.SecondLastName,
                RUT = user.RUT,
                Email = user.Email
            };
        }
    }
}