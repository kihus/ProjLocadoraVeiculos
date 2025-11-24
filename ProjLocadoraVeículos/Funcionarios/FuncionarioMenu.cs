using Locadora.Controller;
using Utils;

namespace Locadora.View.Funcionarios
{
    public class FuncionarioMenu
    {
        private void ExibirMenu()
        {
            Console.Clear();
            Console.WriteLine("=============== MENU DE FUNCIONÁRIOS ===============");
            Console.WriteLine("1 -> Adicionar Funcionário");
            Console.WriteLine("2 -> Listar todos Funcionários");
            Console.WriteLine("3 -> Atualizar Funcionário");
            Console.WriteLine("4 -> Excluir Funcionário");
            Console.WriteLine("0 -> Retornar ao menu anterior");
            Console.WriteLine("================================================");
            Console.Write("-> ");
        }

        public async Task MenuDeFuncionarios()
        {
            var funcionarioController = new FuncionarioController();
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
                            var addFuncionario = new AdicionarFuncionario();
                            await addFuncionario.AdicionarUmFuncionario(funcionarioController);
                            break;

                        case "2":
                            var listarFuncionarios = new ListarFuncionarios();
                            listarFuncionarios.ListarTodosFuncionarios(funcionarioController);
                            break;

                        case "3":
                            var attFuncionario = new AtualizarFuncionario();
                            await attFuncionario.AtualizarSalarioFuncionario(funcionarioController);
                            break;

                        case "4":
                            var excluirFuncionario = new ExcluirFuncionario();
                            await excluirFuncionario.ExcluirUmFuncionario(funcionarioController);
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
                
            }
            while (repetirMenu);
        }
    }
}
