using Estacionamento.Enums;
using System;

namespace Estacionamento.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public TipoPagamento Tipo { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataHora { get; set; }

        public void Validar(decimal valorCobrado)
        {
            if (ValorPago < valorCobrado)
            {
                throw new Exception("O valor pago é menor do que o valor cobrado!");
            }
        }
    }
}