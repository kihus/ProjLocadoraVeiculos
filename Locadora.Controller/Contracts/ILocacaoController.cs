using Locadora.Models;
using Locadora.Models.Enums;

namespace Locadora.Controller.Contracts;

public interface ILocacaoController
{
    public Task AdicionarLocacao(Locacao locacao);
    
    public Task AtualizarLocacao(int idLocacao, DateTime dataDevolucaoReal, EStatus status);
    
    public Task<Locacao> BuscarLocacaoId(int locacaoId);
    
    public Task<List<Locacao>> ListarLocacao();
    
    public Task CancelarLocacao(int locacaoId);
}