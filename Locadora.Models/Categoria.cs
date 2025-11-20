namespace Locadora.Models;

public class Categoria(
    string nome, 
    string descricao, 
    decimal diaria
    )
{
    public int CategoriaId { get; private set; }
    public string Nome { get; private set; } = nome;
    public string Descricao { get; private set; } = descricao;
    public decimal Diaria { get; private set; } = diaria;
    
    public static readonly string INSERT_CATEGORIA =
        "INSERT INTO tblCategorias (Nome, Descricao, Diaria) VALUES (@Nome, @Descricao, @Diaria); SELECT SCOPE_IDENTITY();";
    
    public static readonly string UPDATE_CATEGORIA =
        "UPDATE tblCategorias SET Nome = @Nome, Descricao = @Descricao, Diaria = @Diaria WHERE CategoriaId = @CategoriaId;";
    
    public static readonly string SELECT_CATEGORIA_NOME =
        "SELECT CategoriaID FROM tblCategorias WHERE Nome = @Nome;";

    public static readonly string SELECT_ALL_CATEGORIA =
        "SELECT Nome, Descricao, Diaria FROM tblCategorias";
    
    public static readonly string DELETE_CATEGORIA =
        "DELETE FROM tblCategorias WHERE CategoriaID = @CategoriaId;";
    
    public void SetCategoriaId(int categoriaId) 
        =>  CategoriaId = categoriaId;
    
    public override string ToString()
    {
        return $"Nome: {Nome}\nDescricao: {Descricao}\nDiaria: {Diaria}\n";
    }
}