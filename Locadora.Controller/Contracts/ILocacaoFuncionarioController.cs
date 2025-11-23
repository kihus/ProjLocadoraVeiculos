using Locadora.Models;

namespace Locadora.Controller.Contracts;

public interface ILocacaoFuncionarioController
{
    public Task AdicionarLocacaoFuncionario(LocacaoFuncionario relacionamento);
    public Task RemoverLocacaoFuncionario(int locacaoFuncionarioId);
    public Task<List<LocacaoFuncionario>> ListarFuncionariosDaLocacao(Guid locacaoId);
    public Task<List<LocacaoFuncionario>> ListarLocacoesDoFuncionario(int funcionarioId);
}