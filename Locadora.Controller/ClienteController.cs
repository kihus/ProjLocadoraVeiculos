using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller;

public class ClienteController
{
	public async Task AdicionarCliente(Cliente cliente, Documento documento)
	{
		await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
	
		{
			await connection.OpenAsync();

			await using var transaction = connection.BeginTransaction();
			
			{
				try
				{
					var command = new SqlCommand(Cliente.INSERT_CLIENTE, connection, transaction);

					command.Parameters.AddWithValue("@Nome", cliente.Nome);
					command.Parameters.AddWithValue("@Email", cliente.Email);
					command.Parameters.AddWithValue("@Telefone", cliente.Telefone ?? null);

					cliente.SetClienteId(Convert.ToInt32(await command.ExecuteScalarAsync()));

					documento.SetClienteId(cliente.ClienteId);

					var documentoController = new DocumentoController();
					await documentoController.AdicionarDocumento(documento, connection, transaction);
					
					await transaction.CommitAsync();
					Console.WriteLine("Deu certo");
				}
				catch (SqlException ex)
				{
					await transaction.RollbackAsync();
					throw new Exception("Erro ao adicionar cliente: " + ex.Message);
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					throw new Exception("Erro inesperado ao adicionar cliente: " + ex.Message);
				}
			}
		}
	}
	
	public async Task<List<Cliente>> ListarTodosCliente()
	{
		await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
		
		{
			try
			{
				var clientes = new List<Cliente>();
				
				await connection.OpenAsync();
				var command = new SqlCommand(Cliente.SELECT_CLIENTE, connection);

				var reader = await command.ExecuteReaderAsync() ?? throw new Exception("Cliente não encontrado");

				while (reader.Read())
				{
					clientes.Add(new Cliente(
						reader.GetString(0),
						reader.GetString(1),
						reader.GetValue(2).ToString() ?? null
						)
					);

					var documento = new Documento(
						reader.GetString(3),
						reader.GetString(4),
						DateOnly.FromDateTime(reader.GetDateTime(5)),
						DateOnly.FromDateTime(reader.GetDateTime(6))
					);
					
					var cliente = clientes.Last();
					cliente.SetDocumento(documento);
				}
				return clientes;
			}
			catch (SqlException ex)
			{
				throw new Exception("Erro ao listar clientes: " + ex.Message);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro inesperado aconteceu ao listar clientes: " + ex.Message);
			}
		}
	}
	
	public async Task<Cliente> BuscaClientePorEmail(string email)
	{
		await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
		
		{ 
			try
			{
				await connection.OpenAsync();

				var command = new SqlCommand(Cliente.SELECT_CLIENTE_EMAIL, connection);
				command.Parameters.AddWithValue("@Email", email);

				var reader = await command.ExecuteReaderAsync() ?? throw new Exception("Não foi encontrado nenhum cliente no banco de dados!");
				
				var clientes = new List<Cliente>();

				while (reader.Read())
				{
					clientes.Add(new Cliente(
						reader.GetString(1),
						reader.GetString(2),
						reader.GetValue(3).ToString() ?? null
						)
					);
					
					var documento = new Documento(
						reader.GetString(4),
						reader.GetString(5),
						DateOnly.FromDateTime(reader.GetDateTime(6)),
						DateOnly.FromDateTime(reader.GetDateTime(7))
					);
					
					var cliente = clientes.Last();
					cliente.SetClienteId(Convert.ToInt32(reader["ClienteID"]));
					cliente.SetDocumento(documento);
				}

				return clientes.Find(x => x.Email == email) ?? throw new Exception();
			}
			catch (SqlException ex)
			{
				throw new Exception("Erro ao encontrar cliente: " + ex.Message);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro inesperado ao encontrar cliente: " + ex.Message);
			}
		}
	}
	
	public async Task<Cliente> AtualizarTelefoneCliente(string telefone, string email)
	{
		await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
	
		{
			await connection.OpenAsync();
			
			await using var transaction = connection.BeginTransaction();
			
			{
				try
				{
					var cliente = BuscaClientePorEmail(email).Result ?? 
					              throw new Exception("Cliente não encontrado");
				
					cliente.SetTelefone(telefone);

					var command = new SqlCommand(Cliente.UPDATE_CLIENTE, connection);

					command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
					command.Parameters.AddWithValue("@idCliente", cliente.ClienteId);

					await command.ExecuteNonQueryAsync();
					return cliente;
				}
				catch (SqlException ex)
				{
					throw new Exception("Erro ao atualizar cliente: " + ex.Message);
				}
				catch (Exception ex)
				{
					throw new Exception("Erro inesperado ao atualizar cliente: " + ex.Message);
				}
			}
		}

	}
	
	public async Task AtualizarDocumentoCliente(string email, Documento documento)
	{
		var cliente = BuscaClientePorEmail(email).Result ??
		              throw new Exception("Cliente nao encontrado");
		
		await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
	
		{
			await connection.OpenAsync();
			
			await using var transaction = connection.BeginTransaction();
			
			{
				try
				{
					var documentoController = new DocumentoController();
					documento.SetClienteId(cliente.ClienteId);
					await documentoController.AtualizarDocumento(documento, connection, transaction);
					
					await transaction.CommitAsync();
				}
				catch (SqlException ex)
				{
					await transaction.RollbackAsync();
					throw new Exception("Erro ao atualizar cliente: " + ex.Message);
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					throw new Exception("Erro inesperado ao atualizar cliente: " + ex.Message);
				}
			}
		}
	} 
	
	public async Task ExcluirCliente(string email)
	{
		await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
		
		{
			await connection.OpenAsync();
			await using var transaction = connection.BeginTransaction();
			
			{
				try
				{
					var cliente = BuscaClientePorEmail(email).Result ?? throw new Exception("Cliente nao encontrado!");
				
					var command = new SqlCommand(Cliente.DELETE_CLIENTE, connection, transaction);

					
					command.Parameters.AddWithValue("@idCliente", cliente.ClienteId);
					await command.ExecuteNonQueryAsync();
					
					await transaction.CommitAsync();
					
					Console.WriteLine("Cliente excluido com sucesso!");
				}
				catch (SqlException ex)
				{
					throw new Exception("Erro ao excluir cliente: " + ex.Message);
				}
				catch (Exception ex)
				{
					throw new Exception("Erro inesperado ao excluir cliente: " + ex.Message);
				}
			}
		}
	}
}