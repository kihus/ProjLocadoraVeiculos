using Locadora.Controller;
using Utils;

namespace Locadora.View.Veiculos
{
    public class ExcluirVeiculo
    {
        public async Task DeletarUmVeiculo(VeiculoController veiculoController)
        {
            Console.Clear();
            try
            {
                if (veiculoController.ListarVeiculos().Result.Count is 0)
                    throw new Exception("Não há veículos registrados no sistema!");

                Console.WriteLine("====== SELECIONE A PLACA DO VEÍCULO QUE DESEJA EXCLUIR ======");
                new ListarVeiculos().ListarTodosVeiculos(veiculoController);

                Console.WriteLine("Insira a placa do veículo que deseja excluir:");
                var placa = Console.ReadLine() ?? "";

                var veiculo = veiculoController.BuscarVeiculoPlaca(placa).Result;
                await veiculoController.ExcluirVeiculo(veiculo.VeiculoId);
                
                Console.WriteLine("\nVeículo deletado com sucesso!");
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