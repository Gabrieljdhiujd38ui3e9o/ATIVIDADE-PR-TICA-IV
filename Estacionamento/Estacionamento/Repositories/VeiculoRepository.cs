using System;
using Estacionamento.Data;
using Estacionamento.Models;
using Estacionamento.Enums;
using Microsoft.Data.SqlClient;

namespace Estacionamento.Repositories
{
    public class VeiculoRepository
    {
        public Veiculo CadastrarOuBuscar(Veiculo veiculo)
        {
            Conexao db = new Conexao();
            SqlConnection conexao = db.PegarConexao();
            conexao.Open();

            // Primeiro verifica se o veículo já existe
            string queryBusca = "SELECT Id FROM Veiculos WHERE Placa = @Placa";
            SqlCommand cmdBusca = new SqlCommand(queryBusca, conexao);
            cmdBusca.Parameters.AddWithValue("@Placa", veiculo.Placa);

            SqlDataReader leitor = cmdBusca.ExecuteReader();

            if (leitor.Read())
            {
                // Se existir, só pega o ID e retorna
                veiculo.Id = Convert.ToInt32(leitor["Id"]);
                leitor.Close();
                conexao.Close();
                return veiculo;
            }

            leitor.Close();

          
            string queryInsert = "INSERT INTO Veiculos (Placa, Modelo, Cor, Tipo) VALUES (@Placa, @Modelo, @Cor, @Tipo); SELECT SCOPE_IDENTITY();";
            SqlCommand cmdInsert = new SqlCommand(queryInsert, conexao);
            cmdInsert.Parameters.AddWithValue("@Placa", veiculo.Placa);
            cmdInsert.Parameters.AddWithValue("@Modelo", veiculo.Modelo);
            cmdInsert.Parameters.AddWithValue("@Cor", veiculo.Cor);
            cmdInsert.Parameters.AddWithValue("@Tipo", (int)veiculo.Tipo);

            // Pega o ID gerado pelo banco
            veiculo.Id = Convert.ToInt32(cmdInsert.ExecuteScalar());

            conexao.Close();
            return veiculo;
        }
    }
}