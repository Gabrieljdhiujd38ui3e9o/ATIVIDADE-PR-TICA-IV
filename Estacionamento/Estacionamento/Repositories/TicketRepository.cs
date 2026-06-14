using Estacionamento.Data;
using Estacionamento.Models;
using Estacionamento.Enums;
using Microsoft.Data.SqlClient;
using System;

namespace Estacionamento.Repositories
{
    public class TicketRepository
    {
        public void CriarTicket(Ticket ticket)
        {
            Conexao db = new Conexao();
            SqlConnection conexao = db.PegarConexao();
            conexao.Open();

            string sql = "INSERT INTO Tickets (VeiculoId, Vaga, Entrada, Status) VALUES (@VeiculoId, @Vaga, @Entrada, @Status); SELECT SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql, conexao);

            cmd.Parameters.AddWithValue("@VeiculoId", ticket.Veiculo.Id);
            cmd.Parameters.AddWithValue("@Vaga", ticket.Vaga);
            cmd.Parameters.AddWithValue("@Entrada", ticket.Entrada);
            cmd.Parameters.AddWithValue("@Status", ticket.Status);

            ticket.Id = Convert.ToInt32(cmd.ExecuteScalar());

            conexao.Close();
        }

        public Ticket BuscarTicketAberto(string placa)
        {
            Conexao db = new Conexao();
            SqlConnection conexao = db.PegarConexao();
            conexao.Open();

            string sql = "SELECT t.Id as TicketId, t.Vaga, t.Entrada, v.Id as VeiculoId, v.Placa, v.Modelo, v.Cor, v.Tipo " +
                         "FROM Tickets t " +
                         "INNER JOIN Veiculos v ON t.VeiculoId = v.Id " +
                         "WHERE v.Placa = @Placa AND t.Status = 'Aberto'";

            SqlCommand cmd = new SqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@Placa", placa);

            SqlDataReader leitor = cmd.ExecuteReader();

            Ticket ticketEncontrado = null;

            if (leitor.Read())
            {
                int tipoNum = Convert.ToInt32(leitor["Tipo"]);
                Veiculo veiculo;

                // Instancia o veiculo correto dependendo do banco
                if (tipoNum == 1) veiculo = new Carro();
                else if (tipoNum == 2) veiculo = new Moto();
                else veiculo = new Caminhao();

                veiculo.Id = Convert.ToInt32(leitor["VeiculoId"]);
                veiculo.Placa = leitor["Placa"].ToString();
                veiculo.Modelo = leitor["Modelo"].ToString();
                veiculo.Cor = leitor["Cor"].ToString();

                ticketEncontrado = new Ticket();
                ticketEncontrado.Id = Convert.ToInt32(leitor["TicketId"]);
                ticketEncontrado.Vaga = leitor["Vaga"].ToString();
                ticketEncontrado.Entrada = Convert.ToDateTime(leitor["Entrada"]);
                ticketEncontrado.Veiculo = veiculo;
                ticketEncontrado.Status = "Aberto";
            }

            leitor.Close();
            conexao.Close();
            return ticketEncontrado;
        }

        public void AtualizarSaida(Ticket ticket, Pagamento pagamento)
        {
            Conexao db = new Conexao();
            SqlConnection conexao = db.PegarConexao();
            conexao.Open();

            // Atualiza o ticket
            string sqlTicket = "UPDATE Tickets SET Saida = @Saida, Status = 'Pago' WHERE Id = @Id";
            SqlCommand cmdTicket = new SqlCommand(sqlTicket, conexao);
            cmdTicket.Parameters.AddWithValue("@Saida", ticket.Saida);
            cmdTicket.Parameters.AddWithValue("@Id", ticket.Id);
            cmdTicket.ExecuteNonQuery();

            // Insere o pagamento
            string sqlPag = "INSERT INTO Pagamentos (TicketId, TipoPagamento, ValorPago, DataHora) VALUES (@TicketId, @TipoPagamento, @ValorPago, @DataHora)";
            SqlCommand cmdPag = new SqlCommand(sqlPag, conexao);
            cmdPag.Parameters.AddWithValue("@TicketId", pagamento.TicketId);
            cmdPag.Parameters.AddWithValue("@TipoPagamento", (int)pagamento.Tipo);
            cmdPag.Parameters.AddWithValue("@ValorPago", pagamento.ValorPago);
            cmdPag.Parameters.AddWithValue("@DataHora", pagamento.DataHora);
            cmdPag.ExecuteNonQuery();

            conexao.Close();
        }
    }
}