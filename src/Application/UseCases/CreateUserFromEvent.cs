using src.Domain.Repositories;
using src.Domain.Entities;
using System.Threading.Tasks;
using src.Domain.Events;

namespace src.Application.UseCases
{
    public class CreateUserFromEvent
    {
        private readonly IUserRepository _userRepository;

        public CreateUserFromEvent(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(UserCreatedEvent userEvent)
        {
            var user = new User
            {
                Id = userEvent.UserId,
                Name = userEvent.Name,
                FirstLastName = userEvent.FirstLastName,
                SecondLastName = userEvent.SecondLastName,
                RUT = userEvent.RUT,
                Email = userEvent.Email
            };

            // Inserta el usuario en la base de datos
            await _userRepository.CreateAsync(user);
            Console.WriteLine($"User with ID {user.Id} created in database.");
        }
    }
}
