using Locadora.Controller;
using Utils;

namespace Locadora.View.Clientes
{
    public class ListarClientes
    {
        public void ListarTodosClientes(ClienteController clienteController)
        {
            try
            {
                Console.Clear();
                var clientes = clienteController.ListarTodosClientes().Result;
                if (clientes.Count is 0)
                    throw new Exception("Não há clientes registrados no sistema!");

                Console.WriteLine("======= LISTA DE CLIENTE =======");
                foreach (var cliente in clientes)
                {
                    Console.WriteLine(cliente);
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