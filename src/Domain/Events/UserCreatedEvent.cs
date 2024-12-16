using System;
using System.Collections.Generic;

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
        public List<string> SubjectIds { get; set; } = new List<string>();

        public UserCreatedEvent(Guid userId, string name, string firstLastName, string secondLastName, string rut, string email, List<string>? subjectIds = null)
        {
            UserId = userId;
            Name = name;
            FirstLastName = firstLastName;
            SecondLastName = secondLastName;
            RUT = rut;
            Email = email;
            SubjectIds = subjectIds ?? new List<string>();
        }
    }
}
