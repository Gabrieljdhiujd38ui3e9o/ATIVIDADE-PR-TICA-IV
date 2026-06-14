using Estacionamento.Models;
using System;

namespace Estacionamento.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Veiculo Veiculo { get; set; }
        public string Vaga { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime? Saida { get; set; }
        public string Status { get; set; }

        public Ticket()
        {
            Status = "Aberto";
        }

        public TimeSpan CalcularTempo()
        {
            if (Saida == null)
            {
                throw new Exception("O ticket ainda não foi fechado.");
            }

            TimeSpan tempo = Saida.Value.Subtract(Entrada);
            return tempo;
        }
    }
}