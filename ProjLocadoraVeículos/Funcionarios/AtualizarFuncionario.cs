using Locadora.Controller;
using Utils;

namespace Locadora.View.Funcionarios
{
    public class AtualizarFuncionario
    {
        public async Task AtualizarSalarioFuncionario(FuncionarioController funcionarioController)
        {
            try
            {
                Console.Clear();
                if (funcionarioController.ListarTodosFuncionarios().Result.Count is 0)
                {
                    Console.WriteLine("Não há funcionários registrados no sistema!");
                }
                else
                {
                    new ListarFuncionarios().ListarTodosFuncionarios(funcionarioController);

                    Console.WriteLine("Insira o CPF do funcionário que deseja atualizar o salário:");
                    var cpf = Console.ReadLine();

                    Console.Write("\nDigite o novo salário do funcionário:");
                    var salarioEmString = Console.ReadLine();

                    var salario = string.IsNullOrEmpty(salarioEmString) ?
                        0.0m : decimal.Parse(salarioEmString);

                    await funcionarioController.AtualizarFuncionarioPorCPF(salario, cpf);
                    
                    Console.Clear();
                    Console.WriteLine("Funcionário atualizado com sucesso!");
                    Console.WriteLine(funcionarioController.BuscarFuncionarioPorCPF(cpf));
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
