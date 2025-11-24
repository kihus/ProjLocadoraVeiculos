using System.Data.SqlTypes;
using Locadora.Controller.Contracts;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller;

public class LocacaoController : ILocacaoController
{
    private ClienteController _clienteController = new();
    private VeiculoController _veiculoController = new();
    
    public async Task AdicionarLocacao(Locacao locacao, int idFuncionario)
    {
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
                        locacao.DataDevolucaoReal ?? (object) DBNull.Value);
                    command.Parameters.AddWithValue("@ValorDiaria", locacao.ValorDiaria);
                    command.Parameters.AddWithValue("@ValorTotal", locacao.ValorTotal);
                    command.Parameters.AddWithValue("@Multa", locacao.Multa);
                    command.Parameters.AddWithValue("@Status", locacao.Status);
                    command.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                    
                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao adicionar locacao: " + ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao adicionar locacao: " + ex.Message);
                }
            }
        }
    }

    public async Task FinalizarLocacao(Guid idLocacao, DateTime dataDevolucaoReal, EStatus status)
    {
        var locacao = BuscarLocacaoId(idLocacao).Result;
        var dataDevolucao = dataDevolucaoReal.Day - locacao.DataDevolucaoPrevista.Day;

        if (dataDevolucao > 0)
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

    public async Task<Locacao> BuscarLocacaoId(Guid locacaoId)
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
                    var dataPrevista = reader.GetDateTime(4).Day - reader.GetDateTime(3).Day;

                    locacao = new Locacao(
                        reader.GetInt32(1),
                        reader.GetInt32(2),
                        dataPrevista,
                        reader.GetDecimal(6)
                    );

                    if (!reader.IsDBNull(5))
                        locacao.SetDataDevolucaoReal((DateTime)reader.GetValue(5));
            
                    var nome = _clienteController.BuscarClienteId(reader.GetInt32(1)).Result;
                    var modelo = _veiculoController.BuscarVeiculoNome(reader.GetInt32(2)).Result;
                    
                    locacao.SetClienteNome(nome);
                    locacao.SetVeiculoNome(modelo);

                    locacao.SetStatus(Enum.Parse<EStatus>(reader.GetString(9)));
                    locacao.SetDataLocacao(reader.GetDateTime(3));
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

    public async Task<List<Locacao>> ListarLocacao()
    {
        var locacoes = new List<Locacao>();
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            try
            {
                await connection.OpenAsync();

                var command = new SqlCommand(Locacao.sp_BuscarLocacao, connection);
                var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    var dataPrevista = reader.GetDateTime(4).Day - reader.GetDateTime(3).Day;

                    var locacao = new Locacao(
                        reader.GetInt32(1),
                        reader.GetInt32(2),
                        dataPrevista,
                        reader.GetDecimal(6)
                    );

                    if (!reader.IsDBNull(5))
                    {
                        locacao.SetDataDevolucaoReal((DateTime)reader.GetValue(5));
                        var devolucaoReal = (DateTime)reader.GetValue(5);
                        var dataDevolucao = devolucaoReal.Day - locacao.DataDevolucaoPrevista.Day;

                        if (dataDevolucao > 0)
                            locacao.SetMulta(dataDevolucao * locacao.ValorDiaria);
                    }
                    var nome = _clienteController.BuscarClienteId(reader.GetInt32(1)).Result;
                    var modelo = _veiculoController.BuscarVeiculoNome(reader.GetInt32(2)).Result;

                    locacao.SetLocacaoId(reader.GetGuid(0));
                    locacao.SetClienteNome(nome);
                    locacao.SetVeiculoNome(modelo);
                    
                    locacao.SetDataLocacao(reader.GetDateTime(3));
                    locacao.SetStatus(Enum.Parse<EStatus>(reader.GetString(9)));
                    locacoes.Add(locacao);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar locacoes: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao listar locacoes: " + ex.Message);
            }

            return locacoes ?? throw new Exception("Nao foi possivel localizar a locacao!");
        }
    }

    public async Task CancelarLocacao(Guid locacaoId)
    {
        var locacao = BuscarLocacaoId(locacaoId).Result 
                      ?? throw new Exception("Locacao nao foi encontrada");

        if (locacao.Status == EStatus.Cancelada)
            throw new Exception("Locacao ja esta cancelada");
        
        await using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            await connection.OpenAsync();

            await using var transaction = connection.BeginTransaction();
            {
                try
                {
                    var command = new SqlCommand(Locacao.sp_CancelarLocacao, connection, transaction);
                    command.Parameters.AddWithValue("@idLocacao", locacaoId);
                    command.Parameters.AddWithValue("@Status", EStatus.Cancelada);

                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine("Locacao cancelada com sucesso!");
                }
                catch (SqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao Cancelar Locacao: " + ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro inesperado ao Cancelar Locacao: " + ex.Message);
                }
            }
        }
    }
}