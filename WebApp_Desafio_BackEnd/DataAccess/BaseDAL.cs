using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public abstract class BaseDAL
    {
        //protected static string CONNECTION_STRING = $"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}Dados\\DesafioDB.db\";Version=3;";
        protected static string CONNECTION_STRING = $"Data Source=\"G:\\Downloads\\Geral\\WebApp_Desafio_Desenvolvimento\\WebApp_Desafio_Desenvolvimento\\DesafioDB.db\";Version=3;";
    }
}
