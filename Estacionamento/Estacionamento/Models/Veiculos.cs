using System;
using Estacionamento.Enums;

namespace Estacionamento.Models
{
    
    public abstract class Veiculo
    {
        public int Id { get; set; }
        public string? Placa { get; set; }
        public string? Modelo { get; set; }
        public string? Cor { get; set; }
        public TipoVeiculo Tipo { get; set; }

        public Veiculo(TipoVeiculo tipo)
        {
            this.Tipo = tipo;
        }

        public abstract decimal CalcularValorCobranca(TimeSpan permanencia);

        public int CalcularHoras(TimeSpan permanencia)
        {
            int minutosTotais = (int)permanencia.TotalMinutes;

            if (minutosTotais <= 15) return 0;

            int horas = minutosTotais / 60;
            int minutosRestantes = minutosTotais % 60;

            if (horas == 0) horas = 1;
            else if (minutosRestantes > 30) horas = horas + 1;

            return horas;
        }
    }

    // 2. A CLASSE FILHA: CARRO
    public class Carro : Veiculo
    {
        public Carro() : base(TipoVeiculo.Carro) { }

        public override decimal CalcularValorCobranca(TimeSpan permanencia)
        {
            int horas = CalcularHoras(permanencia);
            return horas * 10.00m;
        }
    }

    // 3. A CLASSE FILHA: MOTO
    public class Moto : Veiculo
    {
        public Moto() : base(TipoVeiculo.Moto) { }

        public override decimal CalcularValorCobranca(TimeSpan permanencia)
        {
            int horas = CalcularHoras(permanencia);
            return horas * 5.00m;
        }
    }

    // 4. A classe CAMINHÃO
    public class Caminhao : Veiculo
    {
        public Caminhao() : base(TipoVeiculo.Caminhao) { }

        public override decimal CalcularValorCobranca(TimeSpan permanencia)
        {
            int horas = CalcularHoras(permanencia);

            if (horas == 0) return 0;

            decimal valorHoras = horas * 18.00m;
            decimal taxaAdicional = 20.00m;

            return valorHoras + taxaAdicional;
        }
    }
}
