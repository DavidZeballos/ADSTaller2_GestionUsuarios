using System;

namespace src.Domain.Events
{
    public class UserCreatedEvent
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public string RUT { get; set; }
        public string Email { get; set; }

        public UserCreatedEvent(Guid userId, string name, string firstLastName, string secondLastName, string rut, string email)
        {
            UserId = userId;
            Name = name;
            FirstLastName = firstLastName;
            SecondLastName = secondLastName;
            RUT = rut;
            Email = email;
        }
    }
}
