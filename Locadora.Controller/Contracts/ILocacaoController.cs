using Locadora.Models;
using Locadora.Models.Enums;

namespace Locadora.Controller.Contracts;

public interface ILocacaoController
{
    public Task AdicionarLocacao(Locacao locacao, int idLocacao);
    
    public Task FinalizarLocacao(Guid idLocacao, DateTime dataDevolucaoReal, EStatus status);
    
    public Task<Locacao> BuscarLocacaoId(Guid locacaoId);
    
    public Task<List<Locacao>> ListarLocacao();
    
    public Task CancelarLocacao(Guid locacaoId);
}