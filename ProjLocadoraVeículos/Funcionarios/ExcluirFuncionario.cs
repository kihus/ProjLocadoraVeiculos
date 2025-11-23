using Locadora.Controller;
using Utils;

namespace Locadora.View.Funcionarios
{
    public class ExcluirFuncionario
    {
        public async Task ExcluirUmFuncionario(FuncionarioController funcionarioController)
        {
            try
            {
                Console.Clear();
                if (funcionarioController.ListarTodosFuncionarios().Result.Count == 0)
                {
                    Console.WriteLine("Não há funcionários registrados no sistema!");
                }
                else
                {
                    new ListarFuncionarios().ListarTodosFuncionarios(funcionarioController);

                    Console.WriteLine("Insira o CPF do funcionário que deseja excluir:");
                    var cpf = Console.ReadLine();   

                    await funcionarioController.DeletarFuncionarioPorCPF(cpf);
                    Console.WriteLine("\nFuncionário excluído com sucesso!");
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
