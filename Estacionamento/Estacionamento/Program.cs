
using Estacionamento.Enums;
using Estacionamento.Models;
using Estacionamento.Repositories;
using System;

namespace Estacionamento
{
    class Program
    {
        static void Main(string[] args)
        {
            VeiculoRepository repoVeiculo = new VeiculoRepository();
            TicketRepository repoTicket = new TicketRepository();

            int opcao = 0;

            while (opcao != 3)
            {
                Console.Clear();
                Console.WriteLine("=== ESTACIONAMENTO ===");
                Console.WriteLine("1 - Entrada de Veículo");
                Console.WriteLine("2 - Saída de Veículo");
                Console.WriteLine("3 - Sair do Programa");
                Console.Write("Digite a opção: ");

                try
                {
                    opcao = Convert.ToInt32(Console.ReadLine());

                    if (opcao == 1)
                    {
                        FazerEntrada(repoVeiculo, repoTicket);
                    }
                    else if (opcao == 2)
                    {
                        FazerSaida(repoTicket);
                    }
                }
                catch (Exception erro)
                {
                    Console.WriteLine("Ocorreu um erro: " + erro.Message);
                }

                if (opcao != 3)
                {
                    Console.WriteLine("\nAperte ENTER para voltar ao menu...");
                    Console.ReadLine();
                }
            }
        }

        static void FazerEntrada(VeiculoRepository repoVeiculo, TicketRepository repoTicket)
        {
            Console.WriteLine("\n--- NOVA ENTRADA ---");
            Console.Write("Digite a placa: ");
            string placa = Console.ReadLine();

            Console.Write("Digite o modelo: ");
            string modelo = Console.ReadLine();

            Console.Write("Digite a cor: ");
            string cor = Console.ReadLine();

            Console.WriteLine("Tipo (1 para Carro, 2 para Moto, 3 para Caminhão): ");
            int tipoDigitado = Convert.ToInt32(Console.ReadLine());

            Veiculo novoVeiculo = null;

            // Criando o veículo baseado na escolha (sem design patterns complexos)
            switch (tipoDigitado)
            {
                case 1:
                    novoVeiculo = new Carro();
                    break;
                case 2:
                    novoVeiculo = new Moto();
                    break;
                case 3:
                    novoVeiculo = new Caminhao();
                    break;
                default:
                    throw new Exception("Tipo de veículo não existe!");
            }

            novoVeiculo.Placa = placa;
            novoVeiculo.Modelo = modelo;
            novoVeiculo.Cor = cor;

            // Cadastra no banco ou pega o que já existe
            novoVeiculo = repoVeiculo.CadastrarOuBuscar(novoVeiculo);

            Console.Write("Qual a vaga? ");
            string vaga = Console.ReadLine();

            Ticket novoTicket = new Ticket();
            novoTicket.Veiculo = novoVeiculo;
            novoTicket.Vaga = vaga;
            novoTicket.Entrada = DateTime.Now;

            repoTicket.CriarTicket(novoTicket);

            Console.WriteLine("Entrada registrada com sucesso! Número do ticket: " + novoTicket.Id);
        }

        static void FazerSaida(TicketRepository repoTicket)
        {
            Console.WriteLine("\n--- SAÍDA ---");
            Console.Write("Digite a placa do veículo que vai sair: ");
            string placa = Console.ReadLine();

            Ticket ticketAberto = repoTicket.BuscarTicketAberto(placa);

            if (ticketAberto == null)
            {
                Console.WriteLine("Não tem nenhum veículo com essa placa no estacionamento.");
                return;
            }

            ticketAberto.Saida = DateTime.Now;

            // Para testar a cobrança alterando o tempo de entrada (apague isso depois de testar)
            // ticketAberto.Entrada = DateTime.Now.AddHours(-1).AddMinutes(-45);

            TimeSpan tempoFicado = ticketAberto.CalcularTempo();

            // Polimorfismo acontece aqui:
            decimal valorCobrado = ticketAberto.Veiculo.CalcularValorCobranca(tempoFicado);

            Console.WriteLine("Veículo: " + ticketAberto.Veiculo.Modelo);
            Console.WriteLine("Tempo: " + tempoFicado.Hours + " horas e " + tempoFicado.Minutes + " minutos.");
            Console.WriteLine("Valor total a pagar: R$ " + valorCobrado);

            if (valorCobrado > 0)
            {
                Console.WriteLine("Forma de pagamento (1 - Dinheiro, 2 - Cartão): ");
                int tipoPag = Convert.ToInt32(Console.ReadLine());

                Console.Write("Valor que o cliente deu: R$ ");
                decimal valorPago = Convert.ToDecimal(Console.ReadLine());

                Pagamento novoPagamento = new Pagamento();
                novoPagamento.TicketId = ticketAberto.Id;
                novoPagamento.DataHora = DateTime.Now;
                novoPagamento.ValorPago = valorPago;

                if (tipoPag == 1) novoPagamento.Tipo = TipoPagamento.Dinheiro;
                else novoPagamento.Tipo = TipoPagamento.Cartao;

                // Chama o método para testar se ele pagou certo
                novoPagamento.Validar(valorCobrado);

                repoTicket.AtualizarSaida(ticketAberto, novoPagamento);

                decimal troco = valorPago - valorCobrado;
                Console.WriteLine("Pagamento finalizado! Troco: R$ " + troco);
            }
            else
            {
                // Caiu na tolerância de 15 minutos
                Pagamento pagTolerancia = new Pagamento();
                pagTolerancia.TicketId = ticketAberto.Id;
                pagTolerancia.DataHora = DateTime.Now;
                pagTolerancia.ValorPago = 0;
                pagTolerancia.Tipo = TipoPagamento.Dinheiro;

                repoTicket.AtualizarSaida(ticketAberto, pagTolerancia);
                Console.WriteLine("Veículo saiu na tolerância. Não precisa pagar.");
            }
        }
    }
}