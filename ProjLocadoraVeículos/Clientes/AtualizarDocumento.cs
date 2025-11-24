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
                    throw new Exception("Não há clientes registrados no sistema");

                new ListarClientes().ListarTodosClientes(clienteController);

                Console.Write("Digite o email do cliente que deseja alterar o telefone: ");
                var emailCliente = Console.ReadLine() ?? "";
                
                var cliente = clienteController.BuscaClientePorEmail(emailCliente).Result 
                              ?? throw new Exception("");

                Console.Write("\nDigite o novo tipo de documento do cliente: ");
                var tipoDocumento = Console.ReadLine() ?? "";
                
                var numeroDocumento= Helpers.SolicitarNumeroDocumento(tipoDocumento);

                Console.Write("\nDigite a data de emissão do documento: ");
                if (DateOnly.TryParse(Console.ReadLine(), out var dataEmissao))
                    throw new Exception("Digite a data corretamente!");

                Console.Write("\nDigite a data de validade do documento: ");
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