using Locadora.Controller;
using Locadora.Models;
using Utils;

namespace Locadora.View.Categorias
{
    public class AtualizarCategoia
    {
        public async Task AtualizarUmaCategoria(CategoriaController categoriaController)
        {
            try
            {
                Console.Clear();
                if (categoriaController.ListarTodasCategorias().Result.Count == 0)
                {
                    Console.WriteLine("Não há categorias registrados no sistema");
                }
                else
                {
                    new ListarCategorias().ListarTodasCategorias(categoriaController);

                    Console.Write("Digite o nome da categoria que deseja atualizar: ");
                    var nomeCategoria = Console.ReadLine() ?? "";

                    Console.Write("\nDigite a nova descrição da categoria: ");
                    var descricaoCategoria = Console.ReadLine();

                    Console.Write("\nDigite o novo valor da diária: ");
                    var valorDiaria = decimal.Parse(Console.ReadLine());

                    var categoria = new Categoria(
                        nomeCategoria,
                        descricaoCategoria,
                        valorDiaria
                        
                    );

                    await categoriaController.AtualizarCategoria(categoria);
                    Console.WriteLine("Categoria atualizada com sucesso!");
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
}
