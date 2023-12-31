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
    public class DepartamentosController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment;

        public DepartamentosController(IHostingEnvironment hostEnvironment)
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
                var departamentosApiClient = new DepartamentosApiClient();
                var lstDepartamentos = departamentosApiClient.DepartamentosListar();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    lstDepartamentos = lstDepartamentos
                        .Where(s => s.Descricao.ToLower().Contains(search))
                        .ToList();
                }

                var paginatedData = lstDepartamentos.Skip(start).Take(length).ToList();

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    draw = draw,
                    recordsTotal = lstDepartamentos.Count,
                    recordsFiltered = lstDepartamentos.Count, 
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
            
            ViewData["Title"] = "Cadastrar Novo Departamento";

            try
            {
                var departamentosApiClient = new DepartamentosApiClient();

                ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            return View("Cadastrar");
        }

        [HttpPost]
        public IActionResult Cadastrar(DepartamentoViewModel DepartamentoVM)
        {
            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var realizadoComSucesso = departamentosApiClient.DepartamentosGravar(DepartamentoVM);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Departamento gravado com sucesso!",
                                AlertTypes.success,
                                this.RouteData.Values["controller"].ToString(),
                                nameof(this.Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o Departamento.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Editar([FromRoute] int id)
        {
            ViewData["Title"] = "Cadastrar Novo Departamento";

            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var departamentoVM = departamentosApiClient.DepartamentosObter(id);


                return View("Editar", departamentoVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }
        [HttpPut]
        public IActionResult Editar(DepartamentoViewModel departamentoVM)
        {
            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var realizadoComSucesso = departamentosApiClient.DepartamentosEditar(departamentoVM);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Departamento gravado com sucesso!",
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
                var departamentosApiClient = new DepartamentosApiClient();
                var realizadoComSucesso = departamentosApiClient.DepartamentosExcluir(id);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Departamento {id} excluído com sucesso!",
                                AlertTypes.success,
                                "Departamento",
                                nameof(Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o Departamento {id}.");
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
            string path = Path.Combine(contentRootPath, "wwwroot", "reports", "rptDepartamentos.rdlc");
            
            LocalReport localReport = new LocalReport(path);

            var departamentosApiClient = new DepartamentosApiClient();
            var lstDepartamentos = departamentosApiClient.DepartamentosListar();

            localReport.AddDataSource("dsDepartamentos", lstDepartamentos);

            // Renderiza o relatório em PDF
            ReportResult reportResult = localReport.Execute(RenderType.Pdf);

            //return File(reportResult.MainStream, "application/pdf");
            return File(reportResult.MainStream, "application/octet-stream", "rptDepartamentos.pdf");
        }
    }
}
