using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace WebApp_Desafio_FrontEnd.ViewModels
{
    [DataContract]
    public class DepartamentoViewModel
    {
        [Display(Name = "ID")]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Descricao")]
        [DataMember(Name = "Descricao")]
        [MaxLength(30, ErrorMessage = "O campo Descrição não pode ter mais de 30 caracteres.")]
        public string Descricao { get; set; }

    }
}
