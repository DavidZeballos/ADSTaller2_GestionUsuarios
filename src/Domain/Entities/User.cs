using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace src.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string FirstLastName { get; set; }
        public required string SecondLastName { get; set; }
        public required string RUT { get; set; }
        public required string Email { get; set; }
        // Guardar la lista como JSON en la base de datos
        public string SubjectIdsJson { get; set; } = "[]";

        // Propiedad no mapeada para trabajar con la lista en memoria
        [NotMapped]
        public List<string> SubjectIds
        {
            get => JsonSerializer.Deserialize<List<string>>(SubjectIdsJson) ?? new List<string>();
            set => SubjectIdsJson = JsonSerializer.Serialize(value);
        }
    }
}