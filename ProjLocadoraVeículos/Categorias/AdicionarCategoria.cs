using Locadora.Controller;
using Locadora.Models;
using Utils;

namespace Locadora.View.Categorias
{
    public class AdicionarCategoria
    {
        public async Task FormAddCategoria(CategoriaController categoriaController)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("======= DADOS DA CATEGORIA =======");
                Console.Write("Digite o nome da categoria: ");
                var nomeCategoria = Console.ReadLine() ?? "";

                Console.Write("\nDigite a descrição da categoria: ");
                var descricaoCategoria = Console.ReadLine();

                var valorDiaria = Helpers.LerDecimal("Digite o valor da diária: ");

                var categoria = new Categoria(
                    nomeCategoria,
                    descricaoCategoria,
                    valorDiaria
                );

                await categoriaController.AdicionarCategoria(categoria);
                Console.WriteLine("Categoria adicionada com sucesso!");
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
