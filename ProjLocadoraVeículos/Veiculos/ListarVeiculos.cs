using Locadora.Controller;
using Utils;

namespace Locadora.View.Veiculos
{
    public class ListarVeiculos
    {
        public void ListarTodosVeiculos(VeiculoController veiculoController)
        {
            try
            {
                Console.Clear();
                
                var veiculos = veiculoController.ListarVeiculos().Result;
                
                if (veiculos.Count <= 0)
                    throw new Exception("Não há veículos registrados no sistema!");

                Console.WriteLine("======= LISTA DE VEÍCULOS =======");
                foreach (var veiculo in veiculos.OrderBy(x => x.Modelo))
                {
                    Console.WriteLine(veiculo);
                }
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