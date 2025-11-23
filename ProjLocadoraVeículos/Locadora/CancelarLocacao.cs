using Locadora.Controller;
using Utils;

namespace Locadora.View.Locadora;

public class CancelarLocacao
{
    public async Task ExcluirLocacao(LocacaoController locacaoController)
    {
        try
        {
            Console.Clear();
            if (locacaoController.ListarLocacao().Result.Count is 0)
                throw new Exception("Não há funcionários registrados no sistema!");
            
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