using System;

namespace WebApp_Desafio_API.ViewModels
{
    /// <summary>
    /// Solicitação do departamento
    /// </summary>
    public class DepartamentoRequest
    {
        /// <summary>
        /// ID do departamento
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Descricao do Departamento
        /// </summary>
        public string descricao { get; set; }

    }
}
