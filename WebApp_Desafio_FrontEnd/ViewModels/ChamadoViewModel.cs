using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace WebApp_Desafio_FrontEnd.ViewModels
{
    [DataContract]
    public class ChamadoViewModel
    {
        private CultureInfo ptBR = new CultureInfo("pt-BR");

        [Display(Name = "ID")]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Assunto")]
        [DataMember(Name = "Assunto")]
        [MaxLength(30, ErrorMessage = "O campo Assunto não pode ter mais de 30 caracteres entendido.")]
        public string Assunto { get; set; }

        [Display(Name = "Solicitante")]
        [DataMember(Name = "Solicitante")]
        [MaxLength(30, ErrorMessage = "O campo Solicitante não pode ter mais de 30 caracteres.")]
        public string Solicitante { get; set; }

        [Display(Name = "IdDepartamento")]
        [DataMember(Name = "IdDepartamento")]
        public int IdDepartamento { get; set; }

        [Display(Name = "Departamento")]
        [DataMember(Name = "Departamento")]
        public string Departamento { get; set; }

        [Display(Name = "DataAbertura")]
        [DataMember(Name = "DataAbertura")]
        public DateTime DataAbertura { get; set; }
        
        [DataMember(Name = "DataAberturaWrapper")]
        public string DataAberturaWrapper
        {
            get
            {
                return DataAbertura.ToString("dd/MM/yyyy", ptBR);
            }
            set
            {
                DataAbertura = DateTime.ParseExact(value, "dd/MM/yyyy", ptBR);
            }
        }
    }
}
