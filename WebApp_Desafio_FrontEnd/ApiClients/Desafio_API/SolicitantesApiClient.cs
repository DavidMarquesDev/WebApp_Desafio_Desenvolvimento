using Newtonsoft.Json;
using System.Collections.Generic;
using WebApp_Desafio_FrontEnd.ViewModels;

namespace WebApp_Desafio_FrontEnd.ApiClients.Desafio_API
{
    public class SolicitantesApiClient : BaseClient
    {
        private const string tokenAutenticacao = "AEEFC184-9F62-4B3E-BB93-BE42BF0FFA36";

        private const string solicitantesListUrl = "api/Solicitantes/Listar";
        private const string solicitantesListUrlObterUrl = "api/Solicitantes/Obter";
        private const string solicitantesListUrlGravarUrl = "api/Solicitantes/Gravar";
        private const string solicitantesListUrlEditarUrl = "api/Solicitantes/Editar";
        private const string solicitantesListUrlExcluirUrl = "api/Solicitantes/Excluir";

        private string desafioApiUrl = "https://localhost:44334/"; // Endereço API IIS-Express

        public SolicitantesApiClient() : base()
        {
            //TODO
        }

        public List<SolicitantesViewModel> SolicitantesListar()
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = default(Dictionary<string, object>); // Não há parâmetros para essa chamada

            var response = base.Get($"{desafioApiUrl}{solicitantesListUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<List<SolicitantesViewModel>>(json);
        }

        public SolicitantesViewModel SolicitantesObter(int idSolicitante)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = new Dictionary<string, object>()
            {
                { "idSolicitante", idSolicitante }
            };

            var response = base.Get($"{desafioApiUrl}{solicitantesListUrlObterUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<SolicitantesViewModel>(json);
        }

        public bool SolicitantesGravar(SolicitantesViewModel solicitante)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var response = base.Post($"{desafioApiUrl}{solicitantesListUrlGravarUrl}", solicitante, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<bool>(json);
        }

        public bool SolicitantesEditar(SolicitantesViewModel solicitante)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var response = base.Put($"{desafioApiUrl}{solicitantesListUrlEditarUrl}", solicitante, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<bool>(json);
        }

        public bool SolicitantesExcluir(int idSolicitante)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = new Dictionary<string, object>()
            {
                { "idSolicitante", idSolicitante }
            };

            var response = base.Delete($"{desafioApiUrl}{solicitantesListUrlExcluirUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<bool>(json);
        }

    }
}
