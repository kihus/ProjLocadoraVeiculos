using Locadora.Models;
using Locadora.Controller;



// CRUD Cliente
var clienteController = new ClienteController();
var cliente = new Cliente("Orochimaru", "Orochimaru@konoha.com");
var documento = new Documento("cpf", "77799977700", DateOnly.Parse("04/21/2023"), DateOnly.Parse("04/22/2030"));

try
{
    await clienteController.AdicionarCliente(cliente, documento);

    await clienteController.AtualizarTelefoneCliente("1199993333", "n@d.com");
    
    await clienteController.AtualizarDocumentoCliente("Orochimaru@konoha.com", documento);
    
    await clienteController.ExcluirCliente("Orochimaru@konoha.com");

    var clientes = clienteController.ListarTodosCliente().Result.OrderBy(x => x.Nome).ToList();
    clientes.ForEach(x => Console.WriteLine(x));
}
catch (Exception ex)
{
    Console.WriteLine("Erro: " + ex.Message, ex.Source);
}

// CRUD Categoria
var categoria = new Categoria("Jeep", "Carro grande demais", 298.99m);
var categoriaController = new CategoriaController();

try
{
    await categoriaController.AdicionarCategoria(categoria);

    await categoriaController.AtualizarCategoria(categoria);

    await categoriaController.ExcluirCategoria(categoria.Nome);

    var categorias = categoriaController.ListarTodasCategorias().Result.ToList();
    categorias.ForEach(x => Console.WriteLine(x));
}
catch (Exception ex)
{
    throw new Exception("Erro: " + ex.Message);
}
