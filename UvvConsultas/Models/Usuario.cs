using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UvvConsultas.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3)]

        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres e no máximo 20 caracteres.")]

        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public ICollection<Consulta> Consultas { get; set; }
    }
}
