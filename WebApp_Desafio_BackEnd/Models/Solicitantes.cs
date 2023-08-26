using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_Desafio_BackEnd.Models
{
    [Serializable]
    public class Solicitantes
    {
        public static readonly Solicitantes Empty;

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "A Descricao é obrigatória")]
        public string Solicitante { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        public string Cpf { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
