using Locadora.Models;
using Locadora.Controller;

var cliente = new Cliente("Naruto Uzucrack", "n@d.com", "11944983683");
//var documento = new Documento(1, "cpf", "11122233300", DateOnly.Parse("04/21/2025"), DateOnly.Parse("04/22/2025"));

Console.WriteLine(cliente);

var clienteController = new ClienteController();

//clienteController.AdicionarCliente(cliente);


//clienteController.AtualizarTelefoneCliente("1199993333", "n@d.com");
clienteController.ExcluirCliente("n@d.com");

if (clienteController.ListarTodosCliente() is null)
{
	Console.WriteLine("Lista de clientes vazia");
}
else
{
	var clientes = clienteController.ListarTodosCliente().OrderBy(x => x.Nome).ToList();
	clientes.ForEach(x => Console.WriteLine(x));
}