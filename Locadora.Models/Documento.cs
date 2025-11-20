namespace Locadora.Models;

public class Documento(
	string tipoDocumento,
	string numero,
	DateOnly dataEmissao,
	DateOnly dataValidade
	)
{
	public int DocumentoId { get; private set; }
	public int ClienteId { get; private set; }
	public string TipoDocumento { get; private set; } = tipoDocumento;
	public string Numero { get; private set; } = numero;
	public DateOnly DataEmissao { get; private set; } = dataEmissao;
	public DateOnly DataValidade { get; private set; } = dataValidade;

	public static readonly string INSERT_DOCUMENTO = 
		"INSERT INTO tblDocumentos (ClienteID, TipoDocumento, Numero, DataEmissao, DataValidade) VALUES (@ClienteId, @TipoDocumento, @Numero, @DataEmissao, @DataValidade)";

	public static readonly string UPDATE_DOCUMENTO =
		"UPDATE tblDocumentos SET TipoDocumento = @TipoDocumento, Numero = @Numero, DataEmissao = @DataEmissao, DataValidade = @DataValidade WHERE ClienteId = @ClienteId";

	public override string ToString()
	{
		return $"Documento: {TipoDocumento}\nNumero: {Numero}\nData de emissão: {DataEmissao}\nData de validade: {DataValidade}\n";
	}

	public void SetClienteId(int clienteId) => ClienteId = clienteId;

}

