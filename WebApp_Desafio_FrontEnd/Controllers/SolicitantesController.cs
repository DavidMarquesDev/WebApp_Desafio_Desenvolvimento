using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApp_Desafio_FrontEnd.ApiClients.Desafio_API;
using WebApp_Desafio_FrontEnd.ViewModels;
using WebApp_Desafio_FrontEnd.ViewModels.Enums;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Newtonsoft.Json.Linq;

namespace WebApp_Desafio_FrontEnd.Controllers
{
    public class SolicitantesController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment;

        public SolicitantesController(IHostingEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]
        public IActionResult Listar()
        {
            // Busca de dados está na Action Datatable()
            return View();
        }

        [HttpGet]
        public IActionResult Datatable(int draw, int start, int length, string search)
        {
            try
            {
                var solicitantesApiClient = new SolicitantesApiClient();
                var lstSolicitantes = solicitantesApiClient.SolicitantesListar();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    lstSolicitantes = lstSolicitantes
                        .Where(s => s.Solicitante.ToLower().Contains(search))
                        .ToList();
                }

                var paginatedData = lstSolicitantes.Skip(start).Take(length).ToList();

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    draw = draw,
                    recordsTotal = lstSolicitantes.Count,
                    recordsFiltered = lstSolicitantes.Count, 
                    data = paginatedData
                };

                return Ok(dataTableVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }


        [HttpGet]
        public IActionResult Cadastrar()
        {
            var solicitantesVM = new SolicitantesViewModel()
            {
                DataCriacao = DateTime.Now
            };
            ViewData["Title"] = "Cadastrar Novo Solicitante";

            try
            {
                var solicitantesApiClient = new SolicitantesApiClient();

                ViewData["ListaSolicitantes"] = solicitantesApiClient.SolicitantesListar();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            return View("Cadastrar", solicitantesVM);
        }

        [HttpPost]
        public IActionResult Cadastrar(SolicitantesViewModel solicitantesVM)
        {
            ViewData["Title"] = "Cadastrar Novo Chamado";
            try
            {
                var solicitantesApiClient = new SolicitantesApiClient();

                var lstSolicitantes = solicitantesApiClient.SolicitantesListar();
                var cpfExistente = lstSolicitantes.Any(s => s.CPF == solicitantesVM.CPF);

                if (cpfExistente)
                {
                    return Ok(new ResponseViewModel(
                                $"Já existe esse CPF cadastrado!",
                                AlertTypes.error,
                                this.RouteData.Values["controller"].ToString(),
                                nameof(this.Cadastrar)));
                }

                var realizadoComSucesso = solicitantesApiClient.SolicitantesGravar(solicitantesVM);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Solicitante gravado com sucesso!",
                                AlertTypes.success,
                                this.RouteData.Values["controller"].ToString(),
                                nameof(this.Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o Solicitante.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }




        [HttpGet]
        public IActionResult Editar([FromRoute] int id)
        {
            ViewData["Title"] = "Editar o Solicitante";
            try
            {
                var solicitantesApiClient = new SolicitantesApiClient();
                var solicitantesVM = solicitantesApiClient.SolicitantesObter(id);

                return View("Editar", solicitantesVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpPut]
        public IActionResult Editar(SolicitantesViewModel solicitantesVM)
        {
            try
            {
                var solicitantesApiClient = new SolicitantesApiClient();
                var realizadoComSucesso = solicitantesApiClient.SolicitantesEditar(solicitantesVM);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Solicitante gravado com sucesso!",
                                AlertTypes.success,
                                this.RouteData.Values["controller"].ToString(),
                                nameof(this.Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o Chamado.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpDelete]
        public IActionResult Excluir([FromRoute] int id)
        {
            try
            {
                var solicitantesApiClient = new SolicitantesApiClient();
                var realizadoComSucesso = solicitantesApiClient.SolicitantesExcluir(id);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Solicitacao {id} excluído com sucesso!",
                                AlertTypes.success,
                                "Chamados",
                                nameof(Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o Chamado {id}.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpPost]
        public IActionResult PesquisarSolicitantes(string solicitante)
        {
            try
            {
                var solicitantesApiClient = new SolicitantesApiClient();
                var lstSolicitantes = solicitantesApiClient.SolicitantesListar();

                if (!string.IsNullOrEmpty(solicitante))
                {
                    solicitante = solicitante.ToLower(); 
                    lstSolicitantes = lstSolicitantes
                        .Where(c => c.Solicitante.ToLower().Contains(solicitante))
                        .ToList();
                }

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    draw = 1,
                    recordsTotal = lstSolicitantes.Count,
                    recordsFiltered = lstSolicitantes.Count, 
                    data = lstSolicitantes
                };

                return Ok(dataTableVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }



        [HttpGet]
        public IActionResult Report()
        {
            string mimeType = string.Empty;
            int extension = 1;
            string contentRootPath = _hostEnvironment.ContentRootPath;
            string path = Path.Combine(contentRootPath, "wwwroot", "reports", "rptSolicitantes.rdlc");
            
            LocalReport localReport = new LocalReport(path);

            var solicitantesApiClient = new SolicitantesApiClient();
            var lstSolicitantes = solicitantesApiClient.SolicitantesListar();

            localReport.AddDataSource("dsSolicitantes", lstSolicitantes);

            // Renderiza o relatório em PDF
            ReportResult reportResult = localReport.Execute(RenderType.Pdf);

            //return File(reportResult.MainStream, "application/pdf");
            return File(reportResult.MainStream, "application/octet-stream", "rptSolicitantes.pdf");
        }

    }
}
