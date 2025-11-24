using Locadora.Models;
using Microsoft.Data.SqlClient;

namespace Locadora.Controller.Contracts;

public interface ILocacaoFuncionarioController
{
    public Task<List<LocacaoFuncionario>> ListarFuncionariosDaLocacao(Guid locacaoId);
    public Task<List<LocacaoFuncionario>> ListarLocacoesDoFuncionario(int funcionarioId);
}