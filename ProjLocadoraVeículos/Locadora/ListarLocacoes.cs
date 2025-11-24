using Locadora.Controller;
using Utils;

namespace Locadora.View.Locadora;

public class ListarLocacoes
{
    public void ListarLocacao(LocacaoController locacaoController)
    {
        try
        {
            Console.Clear();

            var locacoes = locacaoController.ListarLocacao().Result;

            if (locacoes.Count is 0)
                throw new Exception("Não há locações registradas no sistema!");

            Console.WriteLine("======= LISTA DE LOCAÇÕES =======");
            foreach (var locacao in locacoes.OrderBy(x => x.Status))
            {
                Console.WriteLine(locacao);
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