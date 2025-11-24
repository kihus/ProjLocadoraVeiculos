namespace Locadora.Models;

public class LocacaoFuncionario
{
    public int LocacaoFuncionarioID { get; private set; }
    public Guid LocacaoId { get; private set; }
    public int FuncionarioID { get; private set; }

    public Locacao Locacao { get; private set; }
    public Funcionario Funcionario { get; private set; }

    public readonly static string INSERT = @"
            INSERT INTO tblLocacaoFuncionarios(LocacaoID, FuncionarioID)
            VALUES (@LocacaoID, @FuncionarioID);";

    public readonly static string DELETE = @"
            DELETE FROM tblLocacaoFuncionarios
            WHERE LocacaoID =  @idLocacao";

    public readonly static string SELECT_BY_LOCACAO = @"
            SELECT lf.LocacaoFuncionarioID, lf.LocacaoID, lf.FuncionarioID, f.Nome, f.Email, f.Salario
            FROM tblLocacaoFuncionarios lf
            JOIN tblFuncionarios f ON lf.FuncionarioID = f.FuncionarioID
            WHERE lf.LocacaoID = @LocacaoID;";

    public readonly static string SELECT_BY_FUNCIONARIO = @"
            SELECT lf.LocacaoFuncionarioID, lf.LocacaoID, lf.FuncionarioID, l.DataLocacao, l.ValorTotal
            FROM tblLocacaoFuncionarios lf
            JOIN tblLocacoes l ON lf.LocacaoID = l.LocacaoID
            WHERE lf.FuncionarioID = @FuncionarioID;";

    public LocacaoFuncionario(Guid locacaoId, int funcionarioId)
    {
        LocacaoId = locacaoId;
        FuncionarioID = funcionarioId;
    }

    public void SetLocacaoFuncionarioID(int id) 
            => LocacaoFuncionarioID = id;
    
    public void SetLocacao(Locacao locacao) 
            => Locacao = locacao;
    
    public void SetFuncionario(Funcionario funcionario) 
            => Funcionario = funcionario;
}