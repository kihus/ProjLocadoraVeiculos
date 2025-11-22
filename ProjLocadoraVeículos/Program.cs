using Locadora.Models;
using Locadora.Controller;
using Locadora.Models.Enums;


// CRUD Cliente
// var clienteController = new ClienteController();
// var cliente = new Cliente("Orochimaru", "Orochimaru@konoha.com");
// var documento = new Documento("cpf", "77799977700", DateOnly.Parse("04/21/2023"), DateOnly.Parse("04/22/2030"));
//
// try
// {
//     await clienteController.AdicionarCliente(cliente, documento);
//
//     await clienteController.AtualizarTelefoneCliente("1199993333", "n@d.com");
//
//     await clienteController.AtualizarDocumentoCliente("Orochimaru@konoha.com", documento);
//
//     await clienteController.ExcluirCliente("Orochimaru@konoha.com");
//
//     clienteController
//         .ListarTodosCliente()
//         .Result
//         .OrderBy(x => x.Nome)
//         .ToList()
//         .ForEach(x => Console.WriteLine(x));
//     
// }
// catch (Exception ex)
// {
//     Console.WriteLine(ex.Message);
// }

// CRUD Categoria
// var categoria = new Categoria("Kombi", 38.99m);
// var categoriaController = new CategoriaController();
//
// try
// {
//     await categoriaController.AdicionarCategoria(categoria);
//
//     await categoriaController.AtualizarCategoria(categoria);
//
//     await categoriaController.ExcluirCategoria(categoria.Nome);
//
//     categoriaController
//         .ListarTodasCategorias()
//         .Result
//         .ToList()
//         .ForEach(x => Console.WriteLine(x));
// }
// catch (Exception ex)
// {
//     Console.WriteLine(ex.Message);
// }


// CRUD Veiculo
// var veiculo = new Veiculo(1, "ABC1D234", "Renault", "Kwid", 2025, EStatusVeiculo.Alugado);
// var veiculoController = new VeiculoController();
//
// try
// {
//     await veiculoController.AdicionarVeiculo(veiculo);
//     
//     Console.WriteLine(await veiculoController.BuscarVeiculoPlaca(veiculo.Placa));
//     
//     await veiculoController.UpdateVeiculo(EStatusVeiculo.Reservado, "MNO7890");
//     
//     await veiculoController.ExcluirVeiculo(8);
//     
//     veiculoController
//         .ListarVeiculos()
//         .Result
//         .ForEach(x => Console.WriteLine(x));
// }
// catch (Exception ex)
// {
//     Console.WriteLine(ex.Message);
// }

// CRUD Locacao
var locacao = new Locacao(1, 1, 7, 189.99m);
var locacaoController = new  LocacaoController();

try
{
    //await locacaoController.AdicionarLocacao(locacao);
    await locacaoController.AtualizarLocacao(7, DateTime.Parse("11/30/2025"), EStatus.Concluida);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}