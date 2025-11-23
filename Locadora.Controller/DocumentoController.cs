using Locadora.Models;
using Microsoft.Data.SqlClient;

namespace Locadora.Controller;

public class DocumentoController
{
    public async Task AdicionarDocumento(Documento documento, SqlConnection connection, SqlTransaction transaction)
    {
        try
        {
            var command = new SqlCommand(Documento.INSERT_DOCUMENTO, connection, transaction);

            command.Parameters.AddWithValue("@ClienteId", documento.ClienteId);
            command.Parameters.AddWithValue("@TipoDocumento", documento.TipoDocumento);
            command.Parameters.AddWithValue("@Numero", documento.Numero);
            command.Parameters.AddWithValue("@DataEmissao", documento.DataEmissao);
            command.Parameters.AddWithValue("@DataValidade", documento.DataValidade);

            await command.ExecuteNonQueryAsync();
            Console.WriteLine("Documento adicionado com sucesso");
        }
        catch (SqlException ex)
        {
            throw new Exception("Erro ao adicionar o Documento: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao adicionar documento: " + ex.Message);
        }
    }

    public async Task AtualizarDocumento(Documento documento, SqlConnection connection, SqlTransaction transaction)
    {
        try
        {
            var command = new SqlCommand(Documento.UPDATE_DOCUMENTO, connection, transaction);

            command.Parameters.AddWithValue("@ClienteId", documento.ClienteId);
            command.Parameters.AddWithValue("@TipoDocumento", documento.TipoDocumento);
            command.Parameters.AddWithValue("@Numero", documento.Numero);
            command.Parameters.AddWithValue("@DataEmissao", documento.DataEmissao);
            command.Parameters.AddWithValue("@DataValidade", documento.DataValidade);

            await command.ExecuteNonQueryAsync();
            Console.WriteLine("Documento atualizado com sucesso");
        }
        catch (SqlException ex)
        {
            throw new Exception("Erro ao adicionar o Documento: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao adicionar documento: " + ex.Message);
        }
    }
}