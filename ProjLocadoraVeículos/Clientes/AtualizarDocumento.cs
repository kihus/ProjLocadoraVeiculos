using Locadora.Controller;
using Locadora.Models;
using Utils;

namespace Locadora.View.Clientes
{
    public class AtualizarDocumento
    {
        public async Task AtualizarDocumentoCliente(ClienteController clienteController)
        {
            try
            {
                Console.Clear();
                if (clienteController.ListarTodosClientes().Result.Count == 0)
                {
                    Console.WriteLine("Não há clientes registrados no sistema");
                }
                else
                {
                    new ListarClientes().ListarTodosClientes(clienteController);

                    Console.WriteLine("Digite o email do cliente que deseja alterar o telefone:");
                    var emailCliente = Console.ReadLine() ?? "";

                    Console.WriteLine("\nDigite o novo tipo de documento do cliente:");
                    var tipoDocumento = Console.ReadLine() ?? "";

                    Console.WriteLine("\nDigite o novo número de documento do cliente:");
                    var numeroDocumento = Console.ReadLine() ?? "";

                    Console.WriteLine("\nDigite a data de emissão do documento:");
                    if (DateOnly.TryParse(Console.ReadLine(), out var dataEmissao))
                        throw new Exception("Digite a data corretamente!");

                    Console.WriteLine("\nDigite a data de validade do documento:");
                    if (DateOnly.TryParse(Console.ReadLine(), out var dataValidade))
                        throw new Exception("Digite a data corretamente!");

                    var documento = new Documento(
                        tipoDocumento,
                        numeroDocumento,
                        dataEmissao,
                        dataValidade
                    );

                    await clienteController.AtualizarDocumentoCliente(emailCliente, documento);
                    Console.WriteLine("Documento do cliente atualizado com sucesso!");
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
