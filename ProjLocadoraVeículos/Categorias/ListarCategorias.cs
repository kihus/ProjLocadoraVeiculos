using Locadora.Controller;
using Utils;

namespace Locadora.View.Categorias
{
    public class ListarCategorias
    {
        public void ListarTodasCategorias(CategoriaController categoriaController)
        {
            try
            {
                Console.Clear();
                var categorias = categoriaController.ListarTodasCategorias().Result;
                if (categorias.Count > 0)
                    throw new Exception("Não há categorias registradas no sistema!");

                Console.WriteLine("======= LISTA DE CATEGORIAS =======");
                foreach (var categoria in categorias.OrderBy(c => c.Nome))
                {
                    Console.WriteLine(categoria);
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