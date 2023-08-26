using System;

namespace WebApp_Desafio_API.ViewModels
{
    /// <summary>
    /// Solicitação da chamada
    /// </summary>
    public class SolicitantesRequest
    {
        /// <summary>
        /// ID do Chamado
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Assunto do Chamado
        /// </summary>
        public string solicitante { get; set; }

        /// <summary>
        /// Solicitante do Chamado
        /// </summary>
        public string cpf { get; set; }

        /// <summary>
        /// Data de Abertura do Chamado
        /// </summary>
        public DateTime dataCriacao { get; set; }
    }
}
