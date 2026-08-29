using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UvvConsultas.Models
{
    public class Consulta
    {
        [Key]
        public int IdConsulta { get; set; }

        [Required]
        public Especialidade Especialidade { get; set; }

        [Required(ErrorMessage = "A data e hora da consulta é obrigatória")]
        [DataType(DataType.DateTime, ErrorMessage = "A data e hora da consulta deve ser uma data e hora válida")]
        public DateTime DataHora { get; set; }

        [StringLength(500)]
        public string Descricao { get; set; }

        [ForeignKey("UsuarioId")]

        public int UsuarioId { get; set; }
    }
}
