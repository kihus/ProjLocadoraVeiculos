using Locadora.Models.Enums;

namespace Locadora.Models;

public class Veiculo(
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
    public string? Categoria { get; private set; } 
    public EStatusVeiculo StatusVeiculo { get; private set; } = statusVeiculo;
    
    public readonly static string INSERT_VEICULO =
        "INSERT INTO tblVeiculos (CategoriaId, Placa, Marca, Modelo, Ano, StatusVeiculo) VALUES (@CategoriaId, @Placa, @Marca, @Modelo, @Ano, @StatusVeiculo);";
    
    public readonly static string SELECT_VEICULOS =
        "SELECT CategoriaId, Placa, Marca, Modelo, Ano, StatusVeiculo FROM tblVeiculos";

    public readonly static string SELECT_VEICULO_PLACA =
        "SELECT VeiculoId, CategoriaId, Placa, Marca, Modelo, Ano, StatusVeiculo FROM tblVeiculos WHERE Placa = @Placa";
    
    public readonly static string SELECT_VEICULO_ID = 
        "SELECT VeiculoId FROM tblVeiculos WHERE VeiculoId = @VeiculoId";

    public readonly static string SELECT_VEICULO_NOME =
        "EXEC sp_BuscarVeiculoId @idVeiculo;";
    
    public readonly static string UPDATE_STATUS_VEICULO =
        "UPDATE tblVeiculos SET StatusVeiculo = @StatusVeiculo WHERE Placa = @Placa;";
    
    public readonly static string DELETE_VEICULO =
        "DELETE FROM tblVeiculos WHERE VeiculoId = @VeiculoId;";

    public void SetVeiculoId(int veiculoId) 
        => VeiculoId = veiculoId;

    public void SetCategoriaId(int categoriaId) 
        => CategoriaId = categoriaId;
    
    public void SetCategoria(string categoria) 
        => Categoria = categoria;
    
    public override string ToString()
    {
        return $"Marca: {Marca}\nModelo: {Modelo}\nPlaca: {Placa}\nAno: {Ano}\nCategoria: {Categoria}\nStatus do veiculo: {StatusVeiculo.ToString()}\n";
    }
}