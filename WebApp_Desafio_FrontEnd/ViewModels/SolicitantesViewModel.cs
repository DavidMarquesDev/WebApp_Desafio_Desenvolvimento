using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Globalization;

namespace WebApp_Desafio_FrontEnd.ViewModels
{
    [DataContract]
    public class SolicitantesViewModel
    {
        private CultureInfo ptBR = new CultureInfo("pt-BR");

        [Display(Name = "ID")]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Solicitante")]
        [DataMember(Name = "Solicitante")]
        [MaxLength(30, ErrorMessage = "O campo Solicitante não pode ter mais de 30 caracteres.")]
        public string Solicitante { get; set; }

        [Display(Name = "CPF")]
        [DataMember(Name = "CPF")]
        [MaxLength(14, ErrorMessage = "O campo CPF não pode ter mais de 14 caracteres.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato 123.456.789-00.")]
        public string CPF { get; set; }

        [Display(Name = "DataCriacao")]
        [DataMember(Name = "DataCriacao")]
        public DateTime DataCriacao { get; set; }

        [DataMember(Name = "DataCriacaoWrapper")]
        public string DataCriacaoWrapper
        {
            get
            {
                return DataCriacao.ToString("dd/MM/yyyy", ptBR); // Modifiquei o formato para "dd/MM/yyyy"
            }
            set
            {
                DataCriacao = DateTime.ParseExact(value, "dd/MM/yyyy", ptBR);
            }
        }
    }
}
