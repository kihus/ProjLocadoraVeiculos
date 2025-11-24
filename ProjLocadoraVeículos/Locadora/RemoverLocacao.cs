using Locadora.Controller;
using Locadora.Models.Enums;
using Utils;

namespace Locadora.View.Locadora;

public class RemoverLocacao
{
    public async Task ExcluirLocacao(LocacaoController locacaoController)
    {
        try
        {
            Console.Clear();
            if (locacaoController.ListarLocacao().Result.Count is 0)
                throw new Exception("Não há locações registrados no sistema!");

            Console.Write("Digite o id da locacao: ");
            if (!Guid.TryParse(Console.ReadLine(), out var idLocacao))
                throw new Exception("Digite um id correto!");


            Console.Write("Deseja mesmo remover a locacao? (s/n) ");
            var resultado = Console.ReadLine() ?? "".ToLower();
            while (resultado is not ("n" or "s"))
            {
                Console.WriteLine("Digite corretamente!");
                Console.WriteLine("Deseja mesmo criar locação? (s/n) ");
                resultado = Console.ReadLine() ?? "".ToLower();
            }

            if (resultado is "n")
      
                throw new Exception("Cancelando a operação...");
            

            await locacaoController.FinalizarLocacao(idLocacao, dataDevolucaoReal, EStatus.Cancelada);
            Console.WriteLine("Locação cancelada com sucesso!");

            await locacaoController.FinalizarLocacao(idLocacao, dataDevolucaoReal, EStatus.Concluida);
            Console.WriteLine("Locação concluída com sucesso!");
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