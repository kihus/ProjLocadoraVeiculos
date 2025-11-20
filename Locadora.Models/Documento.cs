namespace Locadora.Models;

public class Documento(
	int clienteId,
	string tipoDocumento,
	string numero,
	DateOnly dataEmissao,
	DateOnly dataValidade
	)
{
	public int DocumentoId { get; private set; }
	public int ClienteId { get; private set; } = clienteId;
	public string TipoDocumento { get; private set; } = tipoDocumento;
	public string Numero { get; private set; } = numero;
	public DateOnly DataEmissao { get; private set; } = dataEmissao;
	public DateOnly DataValidade { get; private set; } = dataValidade;

	public static readonly string INSERT_DOCUMENTO = $"INSERT INTO Documento (ClienteID, TipoDocumento, Numero, DataEmissao, DataValidade) " +
	                                                 $"VALUES (@ClienteId, @TipoDocumento, @Numero, @DataEmissao, @DataValidade)";

	public override string ToString()
	{
		return $"Tipo Documento: {TipoDocumento}\nNumero: {Numero}\nData de emissão: {DataEmissao}\nData de validade: {DataValidade}\n";
	}
}

