using System;
using System.Collections.Generic;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.Business
{
    public class SolicitantesBLL
    {
        private SolicitanteDAL dal = new SolicitanteDAL();

        public IEnumerable<Solicitantes> ListarSolicitantes()
        {
            return dal.ListarSolicitantes();
        }

        public Solicitantes ObterSolicitantes(int idSolicitante)
        {
            return dal.ObterSolicitantes(idSolicitante);
        }

        public bool GravarSolicitantes(string Solicitante, string CpfSolicitante)
        {
            return dal.GravarSolicitantes(Solicitante, CpfSolicitante);
        }

        public bool EditarSolicitantes(int ID, string Solicitante, string CpfSolicitante)
        {
            return dal.EditarSolicitantes(ID, Solicitante, CpfSolicitante);
        }

        public bool ExcluirSolicitantes(int idSolicitante)
        {
            return dal.ExcluirSolicitantes(idSolicitante);
        }
    }
}
