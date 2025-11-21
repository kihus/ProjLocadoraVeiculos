namespace Locadora.Models;

public class Categoria(
    string nome, 
    decimal diaria
    )
{
    public int CategoriaId { get; private set; }
    public string Nome { get; private set; } = nome;
    public string? Descricao { get; private set; } = string.Empty;
    public decimal Diaria { get; private set; } = diaria;
    
    public static readonly string INSERT_CATEGORIA =
        "EXEC sp_AdicionarCategoria @Nome, @Descricao, @Diaria;";
    
    public static readonly string UPDATE_CATEGORIA =
        "EXEC sp_AtualizarCategoria @Nome, @Descricao, @Diaria, @CategoriaId;";
    
    public static readonly string SELECT_CATEGORIA_NOME =
        "SELECT CategoriaID FROM tblCategorias WHERE Nome = @Nome;";

    public static readonly string SELECT_CATEGORIA_ID = 
        "SELECT Nome FROM tblCategorias WHERE CategoriaID = @CategoriaId;";
    
    public static readonly string SELECT_ALL_CATEGORIA =
        "SELECT Nome, Descricao, Diaria FROM tblCategorias";
    
    public static readonly string DELETE_CATEGORIA =
        "EXEC sp_ExcluirCategoria @CategoriaId";

    public Categoria(string nome, string? descricao, decimal diaria) : this(nome, diaria)
    {
        Descricao = descricao;
    }
    
    public void SetCategoriaId(int categoriaId) 
        =>  CategoriaId = categoriaId;
    
    public override string ToString()
    {
        return $"Nome: {Nome}\nDescricao: {Descricao}\nDiaria: {Diaria}\n";
    }
}