using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class SolicitanteDAL : BaseDAL
    {
        private const string ANSI_DATE_FORMAT = "yyyy-MM-dd";
        public IEnumerable<Solicitantes> ListarSolicitantes()
        {
            IList<Solicitantes> lstSolicitantes = new List<Solicitantes>();

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {

                    dbCommand.CommandText = "SELECT ID, Solicitante, Cpf,DataCriacao FROM Solicitantes ";

                    dbConnection.Open();

                    using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        var solicitante = Solicitantes.Empty;

                        while (dataReader.Read())
                        {
                            solicitante = new Solicitantes();

                            if (!dataReader.IsDBNull(0))
                                solicitante.ID = dataReader.GetInt32(0);
                            if (!dataReader.IsDBNull(1))
                                solicitante.Solicitante = dataReader.GetString(1);
                            if (!dataReader.IsDBNull(2))
                                solicitante.Cpf = dataReader.GetString(2);
                            if (!dataReader.IsDBNull(3))
                                solicitante.DataCriacao = dataReader.GetDateTime(3);

                            lstSolicitantes.Add(solicitante);
                        }
                        dataReader.Close();
                    }
                    dbConnection.Close();
                }

            }

            return lstSolicitantes;
        }

        public Solicitantes ObterSolicitantes(int idSolicitantes)
        {
            var solicitante = Solicitantes.Empty;

            DataTable dtSolicitantes = new DataTable();

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText =
                        "SELECT Solicitantes.ID, " +
                        "       Solicitantes.Solicitante, " +
                        "       Solicitantes.Cpf, " +
                        "       Solicitantes.DataCriacao " +
                        "FROM Solicitantes " +
                        $"WHERE Solicitantes.ID = {idSolicitantes}";

                    dbConnection.Open();

                    using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            solicitante = new Solicitantes();

                            if (!dataReader.IsDBNull(0))
                                solicitante.ID = dataReader.GetInt32(0);
                            if (!dataReader.IsDBNull(1))
                                solicitante.Solicitante = dataReader.GetString(1);
                            if (!dataReader.IsDBNull(2))
                                solicitante.Cpf = dataReader.GetString(2);
                            if (!dataReader.IsDBNull(3))
                                solicitante.DataCriacao = dataReader.GetDateTime(3);
                        }
                        dataReader.Close();
                    }
                    dbConnection.Close();
                }

            }

            return solicitante;
        }
        public bool GravarSolicitantes(string Solicitante, string CpfSolicitante)
        {
            int regsAfetados = -1;

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText =
                        "INSERT INTO Solicitantes (Solicitante, Cpf, DataCriacao)" +
                        "VALUES (@Solicitante, @Cpf, DATETIME('now'))";

                    dbCommand.Parameters.AddWithValue("@Solicitante", Solicitante);
                    dbCommand.Parameters.AddWithValue("@Cpf", CpfSolicitante);

                    dbConnection.Open();
                    regsAfetados = dbCommand.ExecuteNonQuery();
                    dbConnection.Close();
                }
            }

            return (regsAfetados > 0);
        }


        public bool EditarSolicitantes(int ID, string Solicitante, string CpfSolicitante)
        {
            int regsAfetados = -1;

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText =
                        "UPDATE Solicitantes " +
                        "SET Solicitante=@Solicitante, Cpf=@Cpf " +
                        "WHERE ID=@ID ";

                    dbCommand.Parameters.AddWithValue("@Solicitante", Solicitante);
                    dbCommand.Parameters.AddWithValue("@Cpf", CpfSolicitante);
                    dbCommand.Parameters.AddWithValue("@ID", ID);

                    dbConnection.Open();
                    regsAfetados = dbCommand.ExecuteNonQuery();
                    dbConnection.Close();
                }
            }

            return (regsAfetados > 0);
        }

        public bool ExcluirSolicitantes(int idSolicitante)
        {
            int regsAfetados = -1;

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = $"DELETE FROM Solicitantes WHERE ID = {idSolicitante}";

                    dbConnection.Open();
                    regsAfetados = dbCommand.ExecuteNonQuery();
                    dbConnection.Close();
                }
            }

            return (regsAfetados > 0);
        }

    }
}