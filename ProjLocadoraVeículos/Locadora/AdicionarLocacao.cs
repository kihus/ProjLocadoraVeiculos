using Locadora.Controller;
using Locadora.Models;
using Utils;

namespace Locadora.View.Locadora;

public class AdicionarLocacao
{
    public async Task CriarLocacao(LocacaoController locacaoController)
    {
        try
        {
            Console.Clear();
            
            Console.WriteLine("======= CRIAR LOCAÇÃO =======");
            Console.Write("Digite o id do cliente: ");
            if (!int.TryParse(Console.ReadLine(), out var idCliente))
                throw new Exception("Digite um numero correto");
            
            Console.Write("Digite o id do veiculo: ");
            if (!int.TryParse(Console.ReadLine(), out var idVeiculo))
                throw new Exception("Digite um numero correto");

            Console.Write("Quantos dias vai alugar o carro? ");
            if (!int.TryParse(Console.ReadLine(), out var dias))
                throw new Exception("Digite um numero correto");

            Console.Write("Qual o valor da diária? ");
            if (!decimal.TryParse(Console.ReadLine(), out var valorDiaria))
                throw new Exception("Digite o valor correto");

            var locacao = new Locacao(
                idCliente,
                idVeiculo,
                dias,
                valorDiaria
            );
            
            Console.WriteLine(locacao);
            
            await locacaoController.AdicionarLocacao(locacao);
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