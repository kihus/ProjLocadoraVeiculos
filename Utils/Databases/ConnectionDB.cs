namespace Utils.Databases;

public class ConnectionDB
{
	private static readonly string _connectionString = "Data Source=localhost;Initial Catalog=LocadoraBD;Integrated Security=True;Trust Server Certificate=True";

	public static string GetConnectionString()
	{
		return _connectionString;
	}
}
