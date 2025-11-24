using Locadora.Controller;
using Utils;

namespace Locadora.View.Locadora;

public class MenuLocacao
{
    private void ExibirMenu()
    {
        Console.Clear();
        Console.WriteLine("=============== MENU DE FUNCIONÁRIOS ===============");
        Console.WriteLine("1 -> Adicionar Locação");
        Console.WriteLine("2 -> Listar Locações");
        Console.WriteLine("3 -> Atualizar Locação");
        Console.WriteLine("0 -> Retornar ao menu anterior");
        Console.WriteLine("================================================");
        Console.Write("-> ");
    }

    public async Task MenuDeLocacao()
    {
        var locacaoController = new LocacaoController();
        var opcao = "";
        var repetirMenu = true;

        do
        {
            ExibirMenu();
            opcao = Console.ReadLine() ?? "";
            try
            {
                switch (opcao)
                {
                    case "1":
                        var addLocacao = new AdicionarLocacao();
                        await addLocacao.CriarLocacao(locacaoController);
                        break;

                    case "2":
                        var listarLocacoes = new ListarLocacoes();
                        listarLocacoes.ListarLocacao(locacaoController);
                        break;

                    case "3":
                        var finalizarLocacao = new AtualizarLocacao();
                        await finalizarLocacao.FinalizarLocacao(locacaoController);
                        break;

                    case "0":
                        repetirMenu = false;
                        break;

                    default:
                        Console.WriteLine("Opção inválida! Selecione uma das opções do menu!");
                        Helpers.PressionerEnterParaContinuar();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Helpers.PressionerEnterParaContinuar();
            }
        } while (repetirMenu);
    }
}