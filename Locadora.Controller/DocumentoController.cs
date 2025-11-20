using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller;

public class DocumentoController
{
    public void AdicionarDocumento(Documento documento)
    {
        using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        {
            try
            {
                connection.Open();
                var command = new SqlCommand(Documento.INSERT_DOCUMENTO, connection);

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Erro ao adicionar o Documento: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro inesperado ao adicionar documento: " + ex.Message);
            }
        }
        
    }
}