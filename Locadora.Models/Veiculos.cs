using Locadora.Models.Enums;

namespace Locadora.Models;

public class Veiculos(
    int categoriaId, 
    string placa, 
    string marca, 
    string modelo, 
    int ano, 
    EStatusVeiculo statusVeiculo
    )
{
    public int VeiculoId { get; private set; }
    public int CategoriaId  { get; private set; } = categoriaId;
    public string Placa  { get; private set; } = placa;
    public string Marca { get; private set; } = marca;
    public string Modelo { get; private set; } = modelo;
    public int Ano { get; private set; } = ano;
    public EStatusVeiculo StatusVeiculo { get; private set; } = statusVeiculo;

    public void SetVeiculoId(int veiculoId) 
        => VeiculoId = veiculoId;

    public void SetCategoriaId(int categoriaId) 
        => CategoriaId = categoriaId;
    
    public override string ToString()
    {
        return $"Marca: {Marca}\nModelo: {Modelo}\nPlaca: {Placa}\nAno: {Ano}\nStatus do veiculo: {StatusVeiculo.ToString()}";
    }
}