using Locadora.Controller;
using Utils;

namespace Locadora.View.Categorias
{
    public class ExcluirCategoria
    {
        public async Task ExcluirUmaCategoria(CategoriaController categoriaController)
        {
            try
            {
                Console.Clear();
                if (categoriaController.ListarTodasCategorias().Result.Count is 0)
                {
                    Console.WriteLine("Não há categorias registrados no sistema");
                }
                else
                {
                    new ListarCategorias().ListarTodasCategorias(categoriaController);

                    Console.WriteLine("Digite o nome da categoria que deseja excluir: ");
                    var nomeCategoria = Console.ReadLine() ?? "";

                    await categoriaController.ExcluirCategoria(nomeCategoria);
                    Console.WriteLine("Categoria deletada com sucesso!");
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
