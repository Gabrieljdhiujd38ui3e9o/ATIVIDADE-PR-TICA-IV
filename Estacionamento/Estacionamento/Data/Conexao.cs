using Microsoft.Data.SqlClient;

namespace Estacionamento.Data
{
    public class Conexao
    {
        public SqlConnection PegarConexao()
        {
            // Substitua localhost pelo nome do seu banco se precisar (ex: .\\SQLEXPRESS)
            string strConexao = "Server=.\\SQLEXPRESS;Database=EstacionamentoDB;Trusted_Connection=True;TrustServerCertificate=True;";
            SqlConnection conexao = new SqlConnection(strConexao);
            return conexao;
        }
    }
}