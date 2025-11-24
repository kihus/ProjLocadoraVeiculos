using Locadora.Controller;
using Locadora.Models;
using Utils;

namespace Locadora.View.Funcionarios
{
    public class AdicionarFuncionario
    {
        public async Task AdicionarUmFuncionario(FuncionarioController funcionarioController)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("======= DADOS DO FUNCIONÁRIO =======");
                Console.WriteLine("Digite o nome do funcionário:");
                var nome = Console.ReadLine();

                Console.WriteLine("\nDigite o CPF do funcionário:");
                var cpf = Console.ReadLine();

                Console.WriteLine("\nDigite o Email do funcionário:");
                var email = Console.ReadLine();

                Console.Write("\nDigite o salário do funcionário:");
                var salarioEmString = Console.ReadLine();

                var salario = string.IsNullOrEmpty(salarioEmString) ? 0.0m : decimal.Parse(salarioEmString);

                var funcionario = new Funcionario(
                    nome,
                    cpf,
                    email,
                    salario
                );

                await funcionarioController.AdicionarFuncionario(funcionario);
                Console.WriteLine("\nFuncionário adicionado com sucesso!");
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