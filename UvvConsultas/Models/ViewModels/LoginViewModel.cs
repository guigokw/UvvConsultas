using System.ComponentModel.DataAnnotations;

namespace UvvConsultas.Models.ViewModels
{

    // ViewModel para o login de usuários que serve para validar os dados de entrada do usuário no formulário de login
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
    }
}
