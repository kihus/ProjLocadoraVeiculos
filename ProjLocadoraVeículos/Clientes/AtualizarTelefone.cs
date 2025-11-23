using Locadora.Controller;
using Utils;

namespace Locadora.View.Clientes
{
    public class AtualizarTelefone
    {
        public async Task AtualizarTelefoneCliente(ClienteController clienteController)
        {
            try
            {
                Console.Clear();
                if (clienteController.ListarTodosClientes().Result.Count is 0)
                {
                    throw new Exception("Não há clientes registrados no sistema");
                }

                new ListarClientes().ListarTodosClientes(clienteController);

                Console.WriteLine("Digite o email do cliente que deseja alterar o telefone:");
                var emailCliente = Console.ReadLine();

                Console.WriteLine("\nDigite o novo telefone do cliente:");
                var telefoneCliente = Console.ReadLine();

                await clienteController.AtualizarTelefoneCliente(telefoneCliente, emailCliente);
                Console.WriteLine("Telefone do cliente atualizado com sucesso!");
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