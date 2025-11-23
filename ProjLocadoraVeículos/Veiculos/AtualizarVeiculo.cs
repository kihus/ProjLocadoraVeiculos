using Locadora.Controller;
using Locadora.Models.Enums;
using Utils;

namespace Locadora.View.Veiculos
{
    public class AtualizarVeiculo
    {
        private EStatusVeiculo SelecionarStatus()
        {
            var status = "";
            do
            {
                Console.Clear();
                Console.WriteLine("===== SELECIONE O STATUS DO VEÍCULO =====");
                Console.WriteLine("D - Disponível");
                Console.WriteLine("A - Alugado");
                Console.WriteLine("M - Manutenção");
                Console.WriteLine("R - Reservado");
                status = Console.ReadLine().ToUpper();

                switch (status)
                {
                    case "D":
                        return EStatusVeiculo.Disponivel;
                    case "A":
                        return EStatusVeiculo.Alugado;
                    case "M":
                        return EStatusVeiculo.Manutencao;
                    case "R":
                        return EStatusVeiculo.Reservado;
                    default:
                        Console.WriteLine("Selecione um dos status acima!");
                        break;
                }
            } while (true);
        }

        public async Task AtualizarUmVeiculo(VeiculoController veiculoController)
        {
            Console.Clear();
            try
            {
                if (veiculoController.ListarVeiculos().Result.Count is 0)
                    throw new Exception("Não veículos registrados no sistema!");

                new ListarVeiculos().ListarTodosVeiculos(veiculoController);

                Console.WriteLine("Digite a placa do veículo que deseja atualizar:");
                var placa = Console.ReadLine() ?? "";

                var status = SelecionarStatus();
                await veiculoController.AtualizarStatusVeiculo(status, placa);

                Console.WriteLine("\nVeículo atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Helpers.PressionerEnterParaContinuar();
            }
        }
    }
}