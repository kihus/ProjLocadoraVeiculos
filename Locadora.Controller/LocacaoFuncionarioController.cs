using Locadora.Controller.Contracts;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller;

public class LocacaoFuncionarioController : ILocacaoFuncionarioController
{
    public async Task AdicionarLocacaoFuncionario(LocacaoFuncionario relacionamento)
    {
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();
            
            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(LocacaoFuncionario.INSERT, connection);
                    command.Parameters.AddWithValue("@LocacaoID", relacionamento.LocacaoId);
                    command.Parameters.AddWithValue("@FuncionarioID", relacionamento.FuncionarioID);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao adicionar locacao e funcionario: " + ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inseperado ao adicionar locacao e funcionario: " + ex.Message);
                }
            }
            
        }
        
    }

    public async Task RemoverLocacaoFuncionario(int locacaoFuncionarioId)
    {
        using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            connection.Open();
            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(LocacaoFuncionario.DELETE, connection);
                    command.Parameters.AddWithValue("@LocacaoFuncionarioID", locacaoFuncionarioId);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao remover locacao e funcionario: " + ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao remover locacao e funcionario: " + ex.Message);
                }
            }
           
        }
        
    }

    public async Task<List<LocacaoFuncionario>> ListarFuncionariosDaLocacao(Guid locacaoId)
    {
        var locacaoFuncionario = new List<LocacaoFuncionario>();
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();
            var command = new SqlCommand(LocacaoFuncionario.SELECT_BY_LOCACAO, connection);
            command.Parameters.AddWithValue("@LocacaoID", locacaoId);

            var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                locacaoFuncionario.Add(new LocacaoFuncionario(
                    (Guid)reader["LocacaoID"],
                    (int)reader["FuncionarioID"]
                    )
                );
                
                locacaoFuncionario.Last().SetLocacaoFuncionarioID((int)reader["LocacaoFuncionarioID"]);
            }
        }
        

        return  locacaoFuncionario;
    }

    public async Task<List<LocacaoFuncionario>> ListarLocacoesDoFuncionario(int funcionarioId)
    {
        var locacaoFuncioario = new List<LocacaoFuncionario>();
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();
            try
            {
                var command = new SqlCommand(LocacaoFuncionario.SELECT_BY_FUNCIONARIO, connection);
                command.Parameters.AddWithValue("@FuncionarioID", funcionarioId);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //inserir locacao
                    locacaoFuncioario.Add(new LocacaoFuncionario(
                            (Guid)reader["LocacaoID"],
                            funcionarioId
                        )
                    );

                    locacaoFuncioario.Last().SetLocacaoFuncionarioID((int)reader["LocacaoFuncionarioID"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar locacao e funcionario: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao listar locacao e funcionario: " + ex.Message);
            }
        }
        
        return locacaoFuncioario ?? throw new Exception("Lista de locacao e funcionario nao foi encontrada!");
    }
}