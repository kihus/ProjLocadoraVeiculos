using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller;

public class CategoriaController
{
    public async Task AdicionarCategoria(Categoria categoria)
    {
        try
        {
            await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            {
                await connection.OpenAsync();

                await using var transaction = connection.BeginTransaction();
                {
                    var command = new SqlCommand(Categoria.INSERT_CATEGORIA, connection, transaction);
                    command.Parameters.AddWithValue("@Nome", categoria.Nome);
                    command.Parameters.AddWithValue("@Descricao", categoria.Descricao);
                    command.Parameters.AddWithValue("@Diaria", categoria.Diaria);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                    Console.WriteLine("Categoria adicionada com sucesso!");
                }
            }
        }
        catch (SqlException ex)
        {
            throw new Exception("Erro ao adicionar categoria: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao adicionar  categoria: " + ex.Message);
        }
    }

    public async Task AtualizarCategoria(Categoria categoria)
    {
        categoria.SetCategoriaId(BuscarCategoria(categoria.Nome).Result);

        if (categoria.CategoriaId == 0)
            throw new Exception("Categoria não encontrada!");

        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(Categoria.UPDATE_CATEGORIA, connection, transaction);

                    command.Parameters.AddWithValue("@Nome", categoria.Nome);
                    command.Parameters.AddWithValue("@Descricao", categoria.Descricao);
                    command.Parameters.AddWithValue("@Diaria", categoria.Diaria);
                    command.Parameters.AddWithValue("@CategoriaId", categoria.CategoriaId);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine("Categoria atualizada com sucesso!");
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao atualizar categoria: " + ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao atualizar categoria: " + ex.Message);
                }
            }
        }
    }

    private async Task<int> BuscarCategoria(string nome)
    {
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            try
            {
                var command = new SqlCommand(Categoria.SELECT_CATEGORIA_NOME, connection);
                command.Parameters.AddWithValue("@Nome", nome);
                var reader = await command.ExecuteReaderAsync();

                var idCategoria = 0;

                while (reader.Read())
                {
                    idCategoria = Convert.ToInt32(reader["CategoriaID"]);
                }

                return idCategoria;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar categoria: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar categoria: " + ex.Message);
            }
        }
    }

    public async Task<string> BuscarCategoriaNome(int categoriaId)
    {
        string nomeCategoria = null;
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            try
            {
                await connection.OpenAsync();

                var command = new SqlCommand(Categoria.SELECT_CATEGORIA_ID, connection);
                command.Parameters.AddWithValue("@CategoriaId", categoriaId);
                var reader = await command.ExecuteReaderAsync();
                
                while (reader.Read())
                {
                    nomeCategoria = reader.GetString(0);
                }

                return nomeCategoria ?? throw new Exception("Categoria nao encontrada!");
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar a categoria: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar categoria: " + ex.Message);
            }
        }
    }

    public async Task<List<Categoria>> ListarTodasCategorias()
    {
        var categorias = new List<Categoria>();

        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            try
            {
                var command = new SqlCommand(Categoria.SELECT_ALL_CATEGORIA, connection);
                var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    categorias.Add(new Categoria(
                            reader["Nome"].ToString(),
                            reader["Descricao"].ToString(),
                            (decimal)reader["Diaria"]
                        )
                    );
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar as categorias: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao listar as categorias: " + ex.Message);
            }
        }
        return categorias ?? throw new Exception("Nenhuma categoria foi encontrada!");
    }

    public async Task ExcluirCategoria(string nome)
    {
        var categoriaId = BuscarCategoria(nome).Result;

        if (categoriaId is 0)
            throw new Exception("Categoria nao encontrada!");

        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(Categoria.DELETE_CATEGORIA, connection, transaction);
                    command.Parameters.AddWithValue("@CategoriaId", categoriaId);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine("Categoria deletada com sucesso!");
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao deletar categoria: " + ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao deletar categoria: " + ex.Message);
                }
            }
        }
    }
}