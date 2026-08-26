using System.ComponentModel.DataAnnotations;

namespace UvvConsultas.Models.ViewModels
{

    // ViewModel para o registro de usuários que serve para validar os dados de entrada do usuário no formulário de registro
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        [DataType(DataType.Password)]
        public string ConfirmarSenha { get; set; }
    }
}
