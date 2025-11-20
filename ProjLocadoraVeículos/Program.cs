using Locadora.Models;
using Locadora.Controller;
using Microsoft.Data.SqlClient;

var cliente = new Cliente("Orochimaru", "Orochimaru@konoha.com");
var documento = new Documento("cpf", "77799977700", DateOnly.Parse("04/21/2021"), DateOnly.Parse("04/22/2030"));

//Console.WriteLine(cliente);

var clienteController = new ClienteController();

//clienteController.AdicionarCliente(cliente, documento);


//clienteController.AtualizarTelefoneCliente("1199993333", "n@d.com");

//clienteController.ExcluirCliente("Orochimaru@konoha.com");

await clienteController.AtualizarDocumentoCliente("Orochimaru@konoha.com", documento);

// try
// {
// 	
// 	var clientes = clienteController.ListarTodosCliente().Result.OrderBy(x => x.Nome).ToList();
// 	clientes.ForEach(x => Console.WriteLine(x));
// }
// catch (Exception ex)
// {
// 	Console.WriteLine("Erro: " + ex.Message, ex.Source);
// }


	
	
