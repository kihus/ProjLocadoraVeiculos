using Locadora.Controller;
using Utils;

namespace Locadora.View.Veiculos
{
    public class VeiculoMenu
    {
        private void ExibirMenu()
        {
            Console.Clear();
            Console.WriteLine("=============== MENU DE VEÍCULOS ===============");
            Console.WriteLine("1 -> Adicionar Veículo");
            Console.WriteLine("2 -> Listar todos Veículos");
            Console.WriteLine("3 -> Atualizar Veículo");
            Console.WriteLine("4 -> Excluir Veículo");
            Console.WriteLine("0 -> Retornar ao menu anterior");
            Console.WriteLine("================================================");
            Console.Write("-> ");
        }

        public async Task MenuDeVeiculos()
        {
            var categoriaController = new CategoriaController();
            var veiculoController = new VeiculoController();
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
                            var addVeiculo = new AdicionarVeiculo();
                            await addVeiculo.FormAddVeiculo(categoriaController, veiculoController);
                            break;

                        case "2":
                            var listarVeiculos = new ListarVeiculos();
                            listarVeiculos.ListarTodosVeiculos(veiculoController);
                            break;

                        case "3":
                            var attVeiculo = new AtualizarVeiculo();
                            await attVeiculo.AtualizarUmVeiculo(veiculoController);
                            break;

                        case "4":
                            var excluirVeiculo = new ExcluirVeiculo();
                            await excluirVeiculo.DeletarUmVeiculo(veiculoController);
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
}