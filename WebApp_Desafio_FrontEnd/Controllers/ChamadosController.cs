﻿using Microsoft.AspNetCore.Mvc;
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

namespace WebApp_Desafio_FrontEnd.Controllers
{
    public class ChamadosController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment;

        public ChamadosController(IHostingEnvironment hostEnvironment)
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
                var chamadosApiClient = new ChamadosApiClient();
                var lstChamados = chamadosApiClient.ChamadosListar();

                // Aplicar pesquisa do filtro
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    lstChamados = lstChamados
                        .Where(s => s.Solicitante.ToLower().Contains(search))
                        .ToList();
                }

                // Implementar lógica de paginação
                var paginatedData = lstChamados.Skip(start).Take(length).ToList();

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    draw = draw,
                    recordsTotal = lstChamados.Count,
                    recordsFiltered = lstChamados.Count, 
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
            var chamadoVM = new ChamadoViewModel()
            {
                DataAbertura = DateTime.Now
            };
            ViewData["Title"] = "Cadastrar Novo Chamado";

            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var solicitantesApiClient = new SolicitantesApiClient();

                ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();
                ViewData["ListaSolicitantes"] = solicitantesApiClient.SolicitantesListar();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            return View("Cadastrar", chamadoVM);
        }

        [HttpPost]
        public IActionResult Cadastrar(ChamadoViewModel chamadoVM)
        {
            ViewData["Title"] = "Cadastrar Novo Chamado";
            try
            {
                var chamadosApiClient = new ChamadosApiClient();
                var realizadoComSucesso = chamadosApiClient.ChamadoGravar(chamadoVM);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Chamado gravado com sucesso!",
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

        [HttpGet]
        public IActionResult Editar([FromRoute] int id)
        {
            ViewData["Title"] = "Editar o Chamado";
            try
            {
                var chamadosApiClient = new ChamadosApiClient();
                var chamadoVM = chamadosApiClient.ChamadoObter(id);

                var departamentosApiClient = new DepartamentosApiClient();
                var solicitantesApiClient = new SolicitantesApiClient();

                ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();
                ViewData["ListaSolicitantes"] = solicitantesApiClient.SolicitantesListar();

                return View("Editar", chamadoVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpPut]
        public IActionResult Editar(ChamadoViewModel chamadoVM)
        {
            try
            {
                var chamadosApiClient = new ChamadosApiClient();
                var realizadoComSucesso = chamadosApiClient.ChamadoEditar(chamadoVM);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Chamado gravado com sucesso!",
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
                var chamadosApiClient = new ChamadosApiClient();
                var realizadoComSucesso = chamadosApiClient.ChamadoExcluir(id);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Chamado {id} excluído com sucesso!",
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
        public IActionResult PesquisarChamados(string solicitante)
        {
            try
            {
                var chamadosApiClient = new ChamadosApiClient();
                var lstChamados = chamadosApiClient.ChamadosListar(); if (!string.IsNullOrEmpty(solicitante))
                {
                    solicitante = solicitante.ToLower(); // Converter o termo de pesquisa para minúsculas
                    lstChamados = lstChamados
                        .Where(c => c.Solicitante.ToLower().Contains(solicitante))
                        .ToList();
                }

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    draw = 1,
                    recordsTotal = lstChamados.Count,
                    recordsFiltered = lstChamados.Count, 
                    data = lstChamados
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
            string path = Path.Combine(contentRootPath, "wwwroot", "reports", "rptChamados.rdlc");
            
            LocalReport localReport = new LocalReport(path);

            // Carrega os dados que serão apresentados no relatório
            var chamadosApiClient = new ChamadosApiClient();
            var lstChamados = chamadosApiClient.ChamadosListar();

            localReport.AddDataSource("dsChamados", lstChamados);

            // Renderiza o relatório em PDF
            ReportResult reportResult = localReport.Execute(RenderType.Pdf);

            //return File(reportResult.MainStream, "application/pdf");
            return File(reportResult.MainStream, "application/octet-stream", "rptChamados.pdf");
        }

    }
}
