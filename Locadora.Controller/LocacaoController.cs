using Locadora.Controller.Contracts;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller;

public class LocacaoController : ILocacaoController
{
    public async Task AdicionarLocacao(Locacao locacao)
    {
        //@ClienteId, @VeiculoId, @DataDevolucaoPrevista, @DataDevolucaoReal, @ValorDiaria, @ValorTotal, @Multa, @Status
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(Locacao.sp_AdicionarLocacao, connection, transaction);

                    command.Parameters.AddWithValue("@ClienteId", locacao.ClienteId);
                    command.Parameters.AddWithValue("@VeiculoId", locacao.VeiculoId);
                    command.Parameters.AddWithValue("@DataDevolucaoPrevista", locacao.DataDevolucaoPrevista);
                    command.Parameters.AddWithValue("@DataDevolucaoReal",
                        locacao.DataDevolucaoReal ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ValorDiaria", locacao.ValorDiaria);
                    command.Parameters.AddWithValue("@ValorTotal", locacao.ValorTotal);
                    command.Parameters.AddWithValue("@Multa", locacao.Multa);
                    command.Parameters.AddWithValue("@Status", locacao.Status);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine("Locacao adicionada com sucesso!");
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

    public async Task AtualizarLocacao(int idLocacao, DateTime dataDevolucaoReal, EStatus status)
    {
        var locacao = LocalizarLocacaoId(idLocacao).Result;
        var dataDevolucao = dataDevolucaoReal.Day - locacao.DataDevolucaoPrevista.Day;
        
        if(dataDevolucao > 0)
            locacao.SetMulta(dataDevolucao * locacao.ValorDiaria);
        
        locacao.SetStatus(status);
        
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(Locacao.sp_AtualizarLocacao, connection, transaction);
                    command.Parameters.AddWithValue("@idLocacao", idLocacao);
                    command.Parameters.AddWithValue("@DataDevolucaoReal", dataDevolucaoReal);
                    command.Parameters.AddWithValue("@Status", locacao.Status);
                    command.Parameters.AddWithValue("@Multa", locacao.Multa);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine("Locacao atualizada com sucesso!");
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao atualizar locacao: " + ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao atualizar a locacao: " + ex.Message);
                }
            }
        }
    }

    public async Task<Locacao> LocalizarLocacaoId(int locacaoId)
    {
        Locacao locacao = null;
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();
            try
            {
                var command = new SqlCommand(Locacao.sp_BuscarLocacaoId, connection);
                command.Parameters.AddWithValue("@idLocacao", locacaoId);
                var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    var dataPrevista = reader.GetDateTime(3).Day - reader.GetDateTime(2).Day;

                    locacao = new Locacao(
                        reader.GetInt32(0),
                        reader.GetInt32(1),
                        dataPrevista,
                        reader.GetDecimal(5)
                    );
                    
                    if(!reader.IsDBNull(4))
                        locacao.SetDataDevolucaoReal((DateTime) reader.GetValue(4));
                    
                    locacao.SetDataLocacao(reader.GetDateTime(2));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao localizar locacao: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao localizar locacao: " + ex.Message);
            }
        }
        return locacao ?? throw new Exception("Nao foi possivel localizar a locacao!");
    }

    public Task<List<Locacao>> ListarLocacao()
    {
        throw new NotImplementedException();
    }

    public Task ExcluirLocacao(int locacaoId)
    {
        throw new NotImplementedException();
    }
}