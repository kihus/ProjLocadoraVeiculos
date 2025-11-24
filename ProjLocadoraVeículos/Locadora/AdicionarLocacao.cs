using Locadora.Controller;
using Locadora.Models;
using Utils;

namespace Locadora.View.Locadora;

public class AdicionarLocacao
{
    private VeiculoController _veiculoController =  new();
    private CategoriaController _categoriaController =  new();
    private FuncionarioController _funcionarioController =  new();
    
    public async Task CriarLocacao(LocacaoController locacaoController)
    {
        try
        {
            Console.Clear();

            Console.WriteLine("======= CRIAR LOCAÇÃO =======");
            Console.Write("Digite o CPF do funcionário: ");
            var cpf = Console.ReadLine() ?? "";
            var funcionarioId = _funcionarioController.BuscarFuncionarioPorCPF(cpf).Result.FuncionarioId;
            
            Console.Write("Digite o id do cliente: ");
            if (!int.TryParse(Console.ReadLine(), out var idCliente))
                throw new Exception("Digite um numero correto");
            
            Console.Write("Digite a placa do veículo: ");
            var placa = Console.ReadLine();
            var veiculo = _veiculoController.BuscarVeiculoPlaca(placa).Result;
                    
            var veiculoId = veiculo.VeiculoId;
            var valorDiaria = _categoriaController.BuscarDiariaCategoriaPorId(veiculo.CategoriaId).Result;

            Console.Write("Quantos dias vai alugar o carro? ");
            if (!int.TryParse(Console.ReadLine(), out var dias))
                throw new Exception("Digite um numero correto");
            

            var locacao = new Models.Locacao(
                idCliente,
                veiculoId,
                dias,
                valorDiaria
            );
            
            locacao.SetVeiculoNome(new VeiculoController().BuscarVeiculoNome(veiculoId).Result);
            locacao.SetClienteNome(new ClienteController().BuscarClienteId(idCliente).Result);

            Console.WriteLine("\n" + locacao);

            await locacaoController.AdicionarLocacao(locacao, funcionarioId);
            Console.WriteLine("Locacao adicionada com sucesso!");
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