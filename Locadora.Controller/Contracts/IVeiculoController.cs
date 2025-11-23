using Locadora.Models;
using Locadora.Models.Enums;

namespace Locadora.Controller.Contracts;

public interface IVeiculoController
{
    public Task AdicionarVeiculo(Veiculo veiculo);

    public Task AtualizarStatusVeiculo(EStatusVeiculo statusVeiculo, string placa);

    public Task<List<Veiculo>> ListarVeiculos();

    public Task<Veiculo> BuscarVeiculoPlaca(string placa);

    public Task ExcluirVeiculo(int idVeiculo);
}