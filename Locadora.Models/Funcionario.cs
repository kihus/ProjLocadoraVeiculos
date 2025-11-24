namespace Locadora.Models;

public class Funcionario(
    string nome, 
    string cpf, 
    string email
    )
{
    public int FuncionarioId { get; private set; }
    public string Nome { get; private set; } = nome;
    public string Cpf { get; private set; } = cpf;
    public string Email { get; private set; } = email;
    public decimal? Salario { get; private set; }
    
    public readonly static string INSERTFUNCIONARIO = 
        @"INSERT INTO tblFuncionarios (Nome,CPF,Email,Salario) VALUES (@Nome,@CPF,@Email,@Salario);";
    
    public readonly static string SELECTFUNCIONARIOPORCPF = 
        @"SELECT FuncionarioID,Nome,CPF,Email,Salario FROM tblFuncionarios WHERE FuncionarioID = @idFuncionario;";
    
    public readonly static string SELECTTODOSFUNCIONARIOS = 
        @"SELECT Nome, CPF,Email,Salario FROM tblFuncionarios;";
    
    public readonly static string UPDATEFUNCIONARIOPORCPF = 
        @"UPDATE tblFuncionarios SET Salario = @Salario WHERE FuncionarioID = @idFuncionario;";
    
    public readonly static string DELETEFUNCIONARIOPORCPF = 
        @"DELETE FROM tblFuncionarios WHERE FuncionarioID = @idFuncionario;";

    public Funcionario(
        string nome, 
        string cpf, 
        string email, 
        decimal? salario
        ) 
        : this(
            nome, 
            cpf, 
            email
            )
    {
        Salario = salario;
    }
    
    public void SetFuncionarioId(int funcionarioId)
        => FuncionarioId = funcionarioId;

    public override string ToString()
    {
        return $"Nome: {Nome}\nCPF: {Cpf}\nEmail: {Email}\n{(Salario is null ? "" : "Salario: " +Salario)}\n";
    }
}