using System.Data;
using Locadora.Controller.Contracts;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller;

public class VeiculoController : IVeiculoController
{
    public async Task AdicionarVeiculo(Veiculo veiculo)
    {
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(Veiculo.INSERT_VEICULO, connection, transaction);
                    command.Parameters.AddWithValue("@CategoriaId", veiculo.CategoriaId);
                    command.Parameters.AddWithValue("@Placa", veiculo.Placa);
                    command.Parameters.AddWithValue("@Marca", veiculo.Marca);
                    command.Parameters.AddWithValue("@Modelo", veiculo.Modelo);
                    command.Parameters.AddWithValue("@Ano", veiculo.Ano);
                    command.Parameters.AddWithValue("@StatusVeiculo", veiculo.StatusVeiculo);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine("Veiculo adicionado com sucesso!");
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao adicionar veiculo: " + ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao adicionar veiculo: " + ex.Message);
                }
            }
        }
    }

    public async Task AtualizarStatusVeiculo(EStatusVeiculo statusVeiculo, string placa)
    {
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(Veiculo.UPDATE_STATUS_VEICULO, connection, transaction);
                    command.Parameters.AddWithValue("@StatusVeiculo", statusVeiculo);
                    command.Parameters.AddWithValue("@Placa", placa);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine("Veiculo atualizado");
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao atualizar veiculo: " + ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao atualizar veiculo: " + ex.Message);
                }
            }
        }
    }

    public async Task<List<Veiculo>> ListarVeiculos()
    {
        var veiculos = new List<Veiculo>();
        var categoriaController = new CategoriaController();
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
           
            try
            {
                await connection.OpenAsync();

                var command = new SqlCommand(Veiculo.SELECT_VEICULOS, connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    veiculos.Add(new Veiculo(
                            Convert.ToInt32(reader["CategoriaID"]),
                            reader["Placa"].ToString(),
                            reader["Marca"].ToString(),
                            reader["Modelo"].ToString(),
                            Convert.ToInt16(reader["Ano"]),
                            Enum.Parse<EStatusVeiculo>(reader["StatusVeiculo"].ToString())
                        )
                    );

                    var categoria = categoriaController.BuscarCategoriaNome(reader.GetInt32((0))).Result;
                    veiculos.Last().SetCategoria(categoria);
                }

                return veiculos;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar os veiculos: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao listar os veiculos: " + ex.Message);
            }
        }
    }

    public async Task<Veiculo> BuscarVeiculoPlaca(string placa)
    {
        Veiculo veiculo = null;
        var categoriaController = new CategoriaController();
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            try
            {
                var command = new SqlCommand(Veiculo.SELECT_VEICULO_PLACA, connection);
                command.Parameters.AddWithValue("@Placa", placa);

                var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    veiculo = new Veiculo(
                        Convert.ToInt32(reader["CategoriaId"]),
                        reader["Placa"].ToString(),
                        reader["Marca"].ToString(),
                        reader["Modelo"].ToString(),
                        Convert.ToInt16(reader["Ano"]),
                        Enum.Parse<EStatusVeiculo>(reader.GetValue(6).ToString())
                    );

                    var categoria = categoriaController.BuscarCategoriaNome(reader.GetInt32(1)).Result;
                    veiculo.SetCategoria(categoria);
                    veiculo.SetVeiculoId(reader.GetInt32(0));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao encontrar o veiculo: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao encontrar o veiculo: " + ex.Message);
            }

            return veiculo ?? throw new Exception("Carro nao foi encontrado");
        }
    }
    
    private async Task<int> BuscarVeiculoId(int idVeiculo)
    {
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            try
            {
                await connection.OpenAsync();

                var command = new SqlCommand(Veiculo.SELECT_VEICULO_ID, connection);
                command.Parameters.AddWithValue("@VeiculoId", idVeiculo);

                var reader = command.ExecuteReader();

                var id = 0;

                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
                
                return id;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao selecionar o veiculo: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao selecionar o veiculo: " + ex.Message);
            }
        }
    }

    public async Task<string> BuscarVeiculoNome(int idVeiculo)
    {
        string modeloVeiculo = null;
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            try
            {
                await connection.OpenAsync();

                var command = new SqlCommand(Veiculo.SELECT_VEICULO_NOME, connection);
                command.Parameters.AddWithValue("@idVeiculo", idVeiculo);

                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    modeloVeiculo = reader.GetString(0);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Erro ao buscar veiculo: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro inesperado ao buscar veiculo: " + ex.Message);
            }
        }

        return modeloVeiculo ?? throw new Exception("Não foi encontrado nenhum veiculo com esse id!");
    }

    public async Task ExcluirVeiculo(int idVeiculo)
    {

        if (BuscarVeiculoId(idVeiculo).Result is 0)
            throw new Exception("Nao foi encontrado nenhum veiculo com esse id!");
        
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();
        
            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(Veiculo.DELETE_VEICULO, connection, transaction);
                    command.Parameters.AddWithValue("@VeiculoId", idVeiculo);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine("Veiculo deletado com sucesso!");
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao deletar veiculo: " + ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao deletar veiculo: " + ex.Message);
                }
            }
        }
    }
}