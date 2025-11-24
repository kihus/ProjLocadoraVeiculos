using Locadora.Controller;
using Locadora.Models;
using Locadora.Models.Enums;
using Locadora.View.Categorias;
using Utils;

namespace Locadora.View.Veiculos
{
    public class AdicionarVeiculo
    {
        public async Task FormAddVeiculo(CategoriaController categoriaController, VeiculoController veiculoController)
        {
            try
            {
                Console.Clear();
                if (categoriaController.ListarTodasCategorias().Result.Count is 0)
                {
                    Console.WriteLine("Não há categorias para adicionar um veículo");
                }
                else
                {
                    new ListarCategorias().ListarTodasCategorias(categoriaController);

                    Console.Clear();
                    Console.WriteLine("======= SELECIONE UMA CATEGORIA =======");
                    Console.WriteLine("Digite o nome da categoria que deseja cadastrar o veículo:");
                    var nomeCategoria = Console.ReadLine() ?? "";
                    var categoria = categoriaController.BuscarCategoria(nomeCategoria);

                    Console.Clear();
                    Console.WriteLine("======= DADOS DO VEÍCULO =======");
                    Console.Write("Digite a placa do veículo: ");
                    var placa = SolicitarPlaca();

                    Console.Write("\nDigite a marca do veículo: ");
                    var marca = Console.ReadLine();

                    Console.Write("\nDigite o modelo do veículo: ");
                    var modelo = Console.ReadLine();

                    Console.WriteLine("\nDigite o ano do veículo: ");

                    if (!int.TryParse(Console.ReadLine(), out var ano))
                        throw new Exception("Digite o ano corretamente");

                    var veiculo = new Veiculo(
                        categoria.Id,
                        placa,
                        marca,
                        modelo,
                        ano,
                        EStatusVeiculo.Disponivel
                    );

                    await veiculoController.AdicionarVeiculo(veiculo);
                    Console.WriteLine("\nVeículo adicionado com sucesso!");
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
        private string SolicitarPlaca()
        {
            var placa = "";
            var regexAntigo = @"^[A-Z]{3}[0-9]{4}$";
            var regexMercosul = @"^[A-Z]{3}[0-9][A-Z][0-9]{2}$";

            do
            {
                Console.Write("Digite a placa do veículo (ex: ABC1234 ou ABC1D23): ");
                placa = Console.ReadLine()?.ToUpper().Replace("-", "").Trim();

                if (System.Text.RegularExpressions.Regex.IsMatch(placa, regexAntigo) ||
                    System.Text.RegularExpressions.Regex.IsMatch(placa, regexMercosul))
                {
                    return placa;
                }
                Console.WriteLine("Placa inválida! Tente novamente.");
            } while (true);
        }
    }
}