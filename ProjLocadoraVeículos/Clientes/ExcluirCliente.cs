using Locadora.Controller;
using Utils;

namespace Locadora.View.Clientes
{
    public class ExcluirCliente
    {
        public async Task ExcluirUmCliente(ClienteController clienteController)
        {
            try
            {
                Console.Clear();
                if (clienteController.ListarTodosClientes().Result.Count is 0)
                    throw new Exception("Nenhum cliente encontrado!");

                new ListarClientes().ListarTodosClientes(clienteController);

                Console.WriteLine("Digite o email do cliente que deseja alterar o excluir do sistema:");
                var emailCliente = Console.ReadLine();

                await clienteController.ExcluirCliente(emailCliente);
                Console.WriteLine("Cliente excluído com sucesso!");
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