namespace src.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string FirstLastName { get; set; }
        public required string SecondLastName { get; set; }
        public required string RUT { get; set; }
        public required string Email { get; set; }
    }
}