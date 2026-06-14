using Microsoft.Data.SqlClient;

namespace Estacionamento.Data
{
    public class Conexao
    {
        public SqlConnection PegarConexao()
        {
            
            string strConexao = "Server=.\\SQLEXPRESS;Database=EstacionamentoDB;Trusted_Connection=True;TrustServerCertificate=True;";
            SqlConnection conexao = new SqlConnection(strConexao);
            return conexao;
        }
    }
}
