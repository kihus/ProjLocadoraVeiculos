namespace Utils.Databases;

public class ConnectionDB
{
	private static readonly string _connectionString = "Data Source=localhost;Initial Catalog=LocadoraBD;Persist Security Info=True;User ID=sa;Password=SqlServer@2022;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;";

	public static string GetConnectionString()
	{
		return _connectionString;
	}
}
