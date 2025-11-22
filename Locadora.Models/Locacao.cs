using Locadora.Models.Enums;

namespace Locadora.Models;

public class Locacao(
    int clienteId, 
    int veiculoId,
    int diasLocacao,
    decimal valorDiaria
    )
{
    public Guid LocacaoId { get; private set; }
    public int ClienteId { get; private set; } = clienteId;
    public string ClienteNome { get; private set; }
    public int VeiculoId { get; private set; } = veiculoId;
    public string VeiculoNome { get; private set; }
    public DateTime DataLocacao { get; private set; }
    public DateTime DataDevolucaoPrevista { get; private set; } = DateTime.Now.AddDays(diasLocacao);
    public DateTime? DataDevolucaoReal { get; private set; }
    public decimal ValorDiaria { get; private set; } = valorDiaria;
    public decimal ValorTotal { get; private set; } = valorDiaria * diasLocacao;
    public decimal? Multa { get; private set; } = 0.00m;
    public EStatus Status { get; private set; } = EStatus.Ativa;

    public readonly static string sp_AdicionarLocacao =
        "EXEC sp_AdicionarLocacao @ClienteId, @VeiculoId, @DataDevolucaoPrevista, @DataDevolucaoReal, @ValorDiaria, @ValorTotal, @Multa, @Status; SELECT SCOPE_IDENTITY();";

    public readonly static string sp_AtualizarLocacao =
        "EXEC sp_AtualizarLocacao @idLocacao, @DataDevolucaoReal, @SWtatus, @Multa";

    public readonly static string sp_BuscarLocacaoId =
        "EXEC sp_BuscarLocacaoId @idLocacao";
    
     public readonly static string sp_BuscarLocacao =
            "EXEC sp_BuscarLocacao";
     
     public readonly static string sp_CancelarLocacao =
         "EXEC sp_CancelarLocacao @idLocacao, @Status";
     
    
    public void SetVeiculoNome(string nome)
        => VeiculoNome = nome; 

    public void SetClienteNome(string nome)
        =>  ClienteNome = nome;
    
    public void SetMulta(decimal multa)
        =>  Multa = multa;

    public void SetStatus(EStatus status)
        => Status = status;
    
    public void SetDataLocacao(DateTime dataLocacao)
        => DataLocacao = dataLocacao;
    
    public void SetDataDevolucaoReal(DateTime dataDevolucaoReal)
        => DataDevolucaoReal = dataDevolucaoReal;
    
    public override string ToString()
    {
        return $"Cliente: {ClienteNome}\n" +
               $"Veiculo: {VeiculoNome}\n" +
               $"Data locacao: {DataLocacao}\n" +
               $"Data prevista: {DataDevolucaoPrevista}\n" +
               $"{(DataDevolucaoReal is null ? "" : $"Data devolucao real: {DataDevolucaoReal}\n")}" +
               $"{(Multa is null ? "" : $"Multa: {Multa}\n")}" +
               $"Valor diaria: {ValorDiaria}\n" +
               $"Valor total: {ValorTotal}\n" +
               $"Status: {Status}\n";
    }
}