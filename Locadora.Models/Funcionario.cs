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