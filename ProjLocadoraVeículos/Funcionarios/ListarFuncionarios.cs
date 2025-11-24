using Locadora.Controller;
using Utils;

namespace Locadora.View.Funcionarios
{
    public class ListarFuncionarios
    {
        public void ListarTodosFuncionarios(FuncionarioController funcionarioController)
        {
            try
            {
                Console.Clear();
                
                var funcionarios = funcionarioController.ListarTodosFuncionarios().Result;
                
                if (funcionarios.Count <= 0)
                    throw new Exception("Não há funcionários registrados no sistema!");

                Console.WriteLine("======= LISTA DE FUNCIONÁRIOS =======");
                foreach (var funcionario in funcionarios.OrderBy(x => x.Nome))
                {
                    Console.WriteLine(funcionario);
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