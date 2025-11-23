using Locadora.Controller.Contracts;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller;

public class FuncionarioController : IFuncionarioController
{
    public async Task AdicionarFuncionario(Funcionario funcionario)
    {
        await using (var connection = new SqlConnection(ConnectionDB.GetConnectionString()))
        {
            await connection.OpenAsync();
            await using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var command = new SqlCommand(Funcionario.INSERTFUNCIONARIO, connection, transaction);

                    command.Parameters.AddWithValue("@Nome", funcionario.Nome);
                    command.Parameters.AddWithValue("@CPF", funcionario.Cpf);
                    command.Parameters.AddWithValue("@Email", funcionario.Email);
                    command.Parameters.AddWithValue("@Salario",
                        funcionario.Salario == 0 ? null : funcionario.Salario);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao inserir funcionário no banco de dados: " + ex.Message);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao inserir funcionário: " + e.Message);
                }
            }
        }
    }


    public async Task AtualizarFuncionarioPorCPF(decimal salario, string cpf)
    {
        var funcionario = BuscarFuncionarioPorCPF(cpf).Result 
                          ?? throw new Exception("Funcionário não encontrado!");

        await using (var connection = new SqlConnection(ConnectionDB.GetConnectionString()))
        {
            await connection.OpenAsync();
            await using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var command = new SqlCommand(Funcionario.UPDATEFUNCIONARIOPORCPF, connection, transaction);
                    
                        command.Parameters.AddWithValue("@Salario", salario);
                        command.Parameters.AddWithValue("@idFuncionario", funcionario.FuncionarioId);

                        await command.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao atualziar funcionário: " + ex.Message);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao atualziar funcionário: " + e.Message);
                }
            }
        }
    }

    public async Task<Funcionario> BuscarFuncionarioPorCPF(string cpf)
    {
        Funcionario funcionario = null;

        await using (var connection = new SqlConnection(ConnectionDB.GetConnectionString()))
        {
            await connection.OpenAsync();
            try
            {
                await using (var command = new SqlCommand(Funcionario.SELECTFUNCIONARIOPORCPF, connection))
                {
                    command.Parameters.AddWithValue("@CPF", cpf);

                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            funcionario = new Funcionario(
                                reader["Nome"].ToString(),
                                reader["CPF"].ToString(),
                                reader["Email"].ToString(),
                                reader["Salario"] != DBNull.Value ? decimal.Parse(reader["Salario"].ToString()) : 0
                            );
                            funcionario.SetFuncionarioId((int)reader["FuncionarioID"]);
                        }

                        return funcionario ?? throw new Exception("Funcionario nao encontrado!");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar funcionário: " + ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Erro inesperado ao buscar funcionário: " + e.Message);
            }
        }
    }

    public async Task DeletarFuncionarioPorCPF(string cpf)
    {
        var funcionario = BuscarFuncionarioPorCPF(cpf).Result ?? throw new Exception("Funcionário não encontrado.");

        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(Funcionario.DELETEFUNCIONARIOPORCPF, connection, transaction);

                    command.Parameters.AddWithValue("@IdFuncionario", funcionario.FuncionarioId);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao deletar funcionário: " + ex.Message);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao deletar funcionário: " + e.Message);
                }
            }
        }
    }

    public async Task<List<Funcionario>> ListarTodosFuncionarios()
    {
        await using (var connection = new SqlConnection(ConnectionDB.GetConnectionString()))
        {
            await connection.OpenAsync();
            try
            {
                await using var command = new SqlCommand(Funcionario.SELECTTODOSFUNCIONARIOS, connection);
                {
                    var funcionarios = new List<Funcionario>();

                    await using var reader = command.ExecuteReader();
                    {
                        while (reader.Read())
                        {
                            funcionarios.Add(new Funcionario(
                                    reader["Nome"].ToString(),
                                    reader["CPF"].ToString(),
                                    reader["Email"].ToString(),
                                    reader.GetDecimal(4)
                                )
                            );
                        }

                        return funcionarios;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar funcionários: " + ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Erro inesperado ao listar funcionários: " + e.Message);
            }
        }
    }
}