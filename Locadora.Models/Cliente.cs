namespace Locadora.Models;

public class Cliente (
	string nome,
	string email
	)
{
	public static readonly string INSERT_CLIENTE = 
		"INSERT INTO tblClientes VALUES (@Nome, @Email, @Telefone); SELECT SCOPE_IDENTITY()";
	public static readonly string SELECT_CLIENTE = 
		"SELECT c.Nome, c.Email, c.Telefone,d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade FROM tblClientes c JOIN tblDocumentos d ON c.ClienteID = d.ClienteID";
	public static readonly string SELECT_CLIENTE_EMAIL = 
		"SELECT c.ClienteID, c.Nome, c.Email, c.Telefone, d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade FROM tblClientes c JOIN tblDocumentos d ON c.ClienteID = d.ClienteID WHERE Email = @Email";
	public static readonly string UPDATE_CLIENTE = 
		"UPDATE tblClientes SET Telefone = @Telefone WHERE ClienteID = @idCliente";
	public static readonly string DELETE_CLIENTE = 
		"DELETE FROM tblClientes WHERE ClienteID = @idCliente";

	public int ClienteId { get; private set; }
	public string Nome { get; private set; } = nome;
	public string Email { get; private set; } = email;
	public string? Telefone { get; private set; } = string.Empty;
	public Documento Documento { get; private set; } 

	public Cliente(string nome, string email, string? telefone) : this (nome, email) => Telefone = telefone;

	public void SetClienteId(int clienteId)
	{
		ClienteId = clienteId;
	}

	public void SetDocumento(Documento documento)
	{
		Documento = documento;
	}
	
	public void SetTelefone(string telefone)
	{
		Telefone = telefone;
	}

	public override string ToString()
	{
		return $"Nome: {Nome}\nEmail: {Email}\nTelefone: {(string.IsNullOrEmpty(Telefone) ? "S/ Telefone" : Telefone)}\n{Documento}";
	}
}