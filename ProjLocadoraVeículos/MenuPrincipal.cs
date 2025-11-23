using Locadora.View.Clientes;
using Locadora.View.Categorias;
using Utils;
using Locadora.View.Veiculos;
using Locadora.View.Funcionarios;

namespace Locadora.View
{
    public class MenuPrincipal
    {
        private void ExibirMenu()
        {
            Console.Clear();
            Console.WriteLine("=============== LOCADORA DE VEÍCULOS ===============");
            Console.WriteLine("1 -> Menu gerenciamento de Clientes");
            Console.WriteLine("2 -> Menu gerenciamento de Veículos e Categorias");
            Console.WriteLine("3 -> Menu gerenciamento de Funcionários");
            Console.WriteLine("4 -> Menu gerenciamento de Locações");
            Console.WriteLine("0 -> Encerrar programa");
            Console.WriteLine("====================================================");
            Console.Write("-> ");
        }

        private async Task AcessarMenuDeVeiculosCategorias()
        {
            var opcao = "";
            var trabalhando = true;
            do
            {
                Console.Clear();
                Console.WriteLine("Qual dos menus deseja acessar?");
                Console.WriteLine("1 - Veiculos");
                Console.WriteLine("2 - Categorias");
                Console.WriteLine("0 - Retornar");
                
                opcao = Console.ReadLine() ?? "";

                switch (opcao)
                {
                    case "1":
                        var veiculoMenu = new VeiculoMenu();
                        await veiculoMenu.MenuDeVeiculos();
                        trabalhando = false;
                        break;
                    
                    case "2":
                        var categoriaMenu = new CategoriasMenu();
                        await categoriaMenu.MenuDeCategorias();
                        trabalhando = false;
                        break;
                    
                    case "0":
                        trabalhando = false;
                        break;
                    
                    default:
                        Console.WriteLine("Opção inválida! Tente uma das opções do menu!");
                        Helpers.PressionerEnterParaContinuar();
                        break;
                }
            }
            while (trabalhando);
        }

        public async Task Menu()
        {
            var opcao = "";
            var trabalhando = true;
            do
            {
                ExibirMenu();
                opcao = Console.ReadLine() ?? "";
                
                switch (opcao)
                {
                    case "1":
                        var menuCliente = new ClienteMenu();
                        await menuCliente.MenuDoCliente();
                        break;
                    
                    case "2":
                        await AcessarMenuDeVeiculosCategorias();
                        break;
                    
                    case "3":
                        var menuFuncionario = new FuncionarioMenu();
                        await menuFuncionario.MenuDeFuncionarios();
                        break;
                    
                    case "4":
                        break;
                    
                    case "0":
                        trabalhando = false;
                        break;
                    
                    default:
                        Console.WriteLine("Opção inválida! Selecione uma das opções do menu!");
                        Helpers.PressionerEnterParaContinuar();
                        break;
                }
            }
            while (trabalhando);

            Console.WriteLine("Sistema encerrado com sucesso!");
        }
    }
}
