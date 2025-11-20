using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller;

public class ClienteController
{
	public void AdicionarCliente(Cliente cliente)
	{
		using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
		{
			connection.Open();

			using SqlTransaction transaction = connection.BeginTransaction();
			{
				try
				{
					var command = new SqlCommand(Cliente.INSERT_CLIENTE, connection, transaction);

					command.Parameters.AddWithValue("@Nome", cliente.Nome);
					command.Parameters.AddWithValue("@Email", cliente.Email);
					command.Parameters.AddWithValue("@Telefone", cliente.Telefone ?? (object)DBNull.Value);

					cliente.SetClienteId(Convert.ToInt32(command.ExecuteScalar()));
					transaction.Commit();

					Console.WriteLine("Deu certo");
				}
				catch (SqlException ex)
				{
					Console.WriteLine("Erro ao adicionar cliente: " + ex.Message);
					transaction.Rollback();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Erro inesperado ao adicionar cliente: " + ex.Message);
					transaction.Rollback();
				}
			}
		}

	}
	public async Task<List<Cliente>> ListarTodosCliente()
	{
		using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
		{
			var clientes = new List<Cliente>();

			try
			{
				await connection.OpenAsync();
				var command = new SqlCommand(Cliente.SELECT_CLIENTE, connection);

				var reader = await command.ExecuteReaderAsync();

				if(reader is null)
				{
					throw new Exception("Cliente não encontrado");
				}

				while (await reader.ReadAsync())
				{
					clientes.Add(new Cliente(
						reader.GetString(1),
						reader.GetString(2),
						reader.GetValue(3).ToString() ?? null
						)
					);

					clientes.Last().SetClienteId(reader.GetInt32(0));
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
		using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
		{ 
			try
			{
				await connection.OpenAsync();

				var command = new SqlCommand(Cliente.SELECT_CLIENTE_EMAIL, connection);
				command.Parameters.AddWithValue("@Email", email);

				var reader = await command.ExecuteReaderAsync() ?? throw new Exception("Não foi encontrado nenhum cliente no banco de dados!");
				
				var clientes = new List<Cliente>();

				while (await reader.ReadAsync())
				{
					clientes.Add(new Cliente(
						reader.GetString(1),
						reader.GetString(2),
						reader.GetValue(3).ToString() ?? null
						)
					);

					clientes.Last().SetClienteId(reader.GetInt32(0));
				}

				return clientes.Find(x => x.Email == email) ?? throw new Exception("Não foi possível encontrar esse cliente");

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
		using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
		{
			try
			{
				connection.Open();

				var cliente = BuscaClientePorEmail(email).Result;

				if (cliente is null)
				{
					throw new Exception("Cliente não encontrado");
				}


				cliente.SetTelefone(telefone);

				var command = new SqlCommand(Cliente.UPDATE_CLIENTE, connection);

				command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
				command.Parameters.AddWithValue("@idCliente", cliente.ClienteId);

				command.ExecuteNonQuery();
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
			finally
			{
				connection.Close();
			}

		}

	}
	public void ExcluirCliente(string email)
	{
		using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
		{
			try
			{
				var cliente = BuscaClientePorEmail(email).Result;

				if(cliente is null)
				{
					Console.WriteLine("Cliente nao encontrado!");
					return;
				}

				var command = new SqlCommand(Cliente.DELETE_CLIENTE, connection);

				connection.Open();
				command.Parameters.AddWithValue("@idCliente", cliente.ClienteId);
				command.ExecuteNonQuery();
				Console.WriteLine("Cliente excluido com sucesso!");
			}
			catch (SqlException ex)
			{
				Console.WriteLine("Erro ao excluir cliente: " + ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Erro inesperado ao excluir cliente: " + ex.Message);
			}
			finally
			{
				connection.Close();
			}
		}
	}
}