using System;

namespace WebApp_Desafio_API.ViewModels
{
    /// <summary>
    /// Resposta do departamento
    /// </summary>
    public class DepartamentoResponse
    {
        /// <summary>
        /// ID do Departamento
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Descrição do Departamento
        /// </summary>
        public string descricao { get; set; }
    }
}
