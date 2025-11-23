using Locadora.Controller;
using Locadora.Models.Enums;
using Utils;

namespace Locadora.View.Locadora;

public class FinalizarLocacao
{
    public async Task AtualizarLocacao(LocacaoController locacaoController)
    {
        try
        {
            Console.Clear();
            if (locacaoController.ListarLocacao().Result.Count is 0)
                throw new Exception("Não há locações registrados no sistema!");

            Console.Write("Digite o id da locacao: ");
            if (!Guid.TryParse(Console.ReadLine(), out var idLocacao))
                throw new Exception("Digite um id correto!");
            
            Console.Write("Digite a data da devolucao: ");
            if (!DateTime.TryParse(Console.ReadLine(), out var dataDevolucaoReal))
                throw new Exception("Digite a data da devolucao valida!");

            Console.Write("Gostaria de concluir a locação ou finalizar? (1 - cancelar | 2 - concluir) ");
            var status = Console.ReadLine() ?? "";
            
            while (status is not ("1" or "2"))
            {
                Console.WriteLine("Digite a opcao corretamente!");
                Console.Write("Gostaria de concluir a locação ou finalizar? (1 - cancelar | 2 - concluir) ");
                status = Console.ReadLine() ?? "";
            }
            
            if (status is "1")
            {
                Console.Write("Deseja mesmo cancelar? (s/n) ");
                var resultado = Console.ReadLine() ?? "".ToLower();
                while (resultado is "n" or "s")
                {
                    Console.WriteLine("Digite corretamente!");
                    Console.WriteLine("Deseja mesmo criar locação? (s/n) ");
                    resultado = Console.ReadLine() ?? "".ToLower();
                }

                if (status is "n")
                {
                    Console.WriteLine("Cancelando a operação...");
                    return;
                }
                
                await locacaoController.FinalizarLocacao(idLocacao, dataDevolucaoReal, EStatus.Cancelada);
                Console.WriteLine("Locação cancelada com sucesso!");
            }
            else
            {
                Console.Write("Deseja mesmo concluir? (s/n) ");
                var resultado = Console.ReadLine() ?? "".ToLower();
                while (resultado is "n" or "s")
                {
                    Console.WriteLine("Digite corretamente!");
                    Console.WriteLine("Deseja mesmo criar locação? (s/n) ");
                    resultado = Console.ReadLine() ?? "".ToLower();
                }
                
                if (status is "n")
                {
                    Console.WriteLine("Cancelando a operação...");
                    return;
                }
                
                await locacaoController.FinalizarLocacao(idLocacao, dataDevolucaoReal, EStatus.Concluida);
                Console.WriteLine("Locação concluída com sucesso!");
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