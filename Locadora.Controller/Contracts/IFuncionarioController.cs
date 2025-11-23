using Locadora.Models;

namespace Locadora.Controller.Contracts;

public interface IFuncionarioController
{
    public Task AdicionarFuncionario(Funcionario funcionario);
    public Task AtualizarFuncionarioPorCPF(decimal salario, string cpf);
    public Task<Funcionario> BuscarFuncionarioPorCPF(string cpf);
    public Task DeletarFuncionarioPorCPF(string cpf);
    public Task<List<Funcionario>> ListarTodosFuncionarios();
}