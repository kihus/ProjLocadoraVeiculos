using Locadora.Controller;
using Locadora.Models;
using Utils;

namespace Locadora.View.Clientes
{
    public class AdicionarCliente
    {
        public async Task FormAddCliente(ClienteController clienteController)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("======= DADOS DO CLIENTE =======");
                Console.Write("Digite o nome do cliente: ");
                var nomeCliente = Console.ReadLine() ?? "";

                Console.Write("\nDigite o email do cliente: ");
                var emailCliente = Console.ReadLine() ?? "";
                
                var telefoneCliente = Helpers.SolicitarTelefone();

                Console.Clear();
                Console.WriteLine("======= DOCUMENTO DO CLIENTE =======");
                Console.Write("Digite o tipo de documento do cliente: ");
                var tipoDocumento = Console.ReadLine().ToUpper() ?? "";
                var numeroDocumento = Helpers.SolicitarNumeroDocumento(tipoDocumento);
                
                Console.Write("\nDigite a data de emissão do documento: ");
                if(!DateOnly.TryParse(Console.ReadLine(), out var dataEmissao))
                    throw new Exception("Digite a data corretamente");
                
                Console.Write("\nDigite a data de validade do documento: ");
                if(!DateOnly.TryParse(Console.ReadLine(), out var dataValidade))
                    throw new Exception("Digite a data corretamente");

                var cliente = new Cliente(
                    nomeCliente,
                    emailCliente,
                    telefoneCliente
                );

                var documento = new Documento(
                    tipoDocumento,
                    numeroDocumento,
                    dataEmissao,
                    dataValidade
                );

                await clienteController.AdicionarCliente(cliente, documento);
                Console.WriteLine("Cliente adicionado com sucesso!");
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
