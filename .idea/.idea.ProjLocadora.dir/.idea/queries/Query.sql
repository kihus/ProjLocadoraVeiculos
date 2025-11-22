DELETE
FROM tblClientes
WHERE ClienteID = 54

CREATE OR ALTER PROCEDURE sp_AdicionarCategoria @NomeCategoria VARCHAR(50),
                                                @DescricaoCategoria VARCHAR(255) NULL,
                                                @DiariaCategoria DECIMAL(10, 2)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO tblCategorias (Nome, Descricao, Diaria)
        VALUES (@NomeCategoria, @DescricaoCategoria, @DiariaCategoria);

        PRINT 'Categoria adicionada!'
    END TRY
    BEGIN CATCH
        print 'Um erro aconteceu ao adicionar o cliente: ' + ERROR_MESSAGE()
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_AtualizarCategoria @NomeCategoria VARCHAR(50),
                                                @DescricaoCategoria VARCHAR(255) NULL,
                                                @DiariaCategoria DECIMAL(10, 2),
                                                @CategoriaId INTEGER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE tblCategorias
        SET Nome      = @NomeCategoria,
            Descricao = @DescricaoCategoria,
            Diaria    = @DiariaCategoria
        WHERE CategoriaId = @CategoriaId;

        PRINT 'Categoria atualizada!'
    END TRY
    BEGIN CATCH
        print 'Um erro aconteceu ao adicionar o cliente: ' + ERROR_MESSAGE()
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_AtualizarCategoria @NomeCategoria VARCHAR(50),
                                                @DescricaoCategoria VARCHAR(255) NULL,
                                                @DiariaCategoria DECIMAL(10, 2),
                                                @CategoriaId INTEGER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE tblCategorias
        SET Nome      = @NomeCategoria,
            Descricao = @DescricaoCategoria,
            Diaria    = @DiariaCategoria
        WHERE CategoriaId = @CategoriaId;

        PRINT 'Categoria atualizada!'
    END TRY
    BEGIN CATCH
        print 'Um erro aconteceu ao atualizar o cliente: ' + ERROR_MESSAGE()
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_ExcluirCategoria @CategoriaId INTEGER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE
        FROM tblCategorias
        WHERE CategoriaID = @CategoriaId;

        PRINT 'Categoria excluida!'
    END TRY
    BEGIN CATCH
        PRINT 'Um erro aconteceu ao adicionar o cliente: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_AdicionarLocacao @ClienteIdLocacao INT,
                                              @VeiculoIdLocacao INT,
                                              @DataDevolucaoPrevistaLocacao DATETIME,
                                              @DataDevolucaoRealLocacao DATETIME NULL,
                                              @ValorDiariaLocacao DECIMAL(10, 2),
                                              @ValorTotalLocacao DECIMAL(10, 2),
                                              @MultaLocacao DECIMAL(10, 2) NULL,
                                              @StatusLocacao VARCHAR(20) NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO tblLocacoes (ClienteID, VeiculoID, DataDevolucaoPrevista, DataDevolucaoReal, ValorDiaria,
                                 ValorTotal, Multa, Status)
        VALUES (@ClienteIdLocacao, @VeiculoIdLocacao, @DataDevolucaoPrevistaLocacao,
                @DataDevolucaoRealLocacao, @ValorDiariaLocacao,
                @ValorTotalLocacao, @MultaLocacao, @StatusLocacao);

        PRINT 'Locacao adicionada com sucesso!'
    END TRY
    BEGIN CATCH
        PRINT 'Erro ao adicionar locacao' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_AtualizarLocacao @idLocacao INT,
                                              @DataDevolucaoReal DATETIME,
                                              @Status VARCHAR(20),
                                              @Multa DECIMAL(10, 2)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE tblLocacoes
        SET DataDevolucaoReal = @DataDevolucaoReal,
            Status            = @Status,
            Multa             = @Multa
        WHERE LocacaoID = @idLocacao;

        PRINT 'Locacao Atualizada com sucesso!'
    END TRY
    BEGIN CATCH
        PRINT 'Erro ao atualizar localizacao: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_BuscarLocacaoId @idLocacao INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT ClienteID,
               VeiculoID,
               DataLocacao,
               DataDevolucaoPrevista,
               DataDevolucaoReal,
               ValorDiaria,
               ValorTotal,
               Multa,
               Status
        FROM tblLocacoes
        WHERE LocacaoID = @idLocacao;
    END TRY
    BEGIN CATCH
        PRINT 'Erro ao encontrar locacao: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_BuscarLocacao
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT ClienteID,
               VeiculoID,
               DataLocacao,
               DataDevolucaoPrevista,
               DataDevolucaoReal,
               ValorDiaria,
               ValorTotal,
               Multa,
               Status
        FROM tblLocacoes
    END TRY
    BEGIN CATCH
        PRINT 'Erro ao encontrar locacao: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_CancelarLocacao @idLocacao INT,
                                             @Status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE tblLocacoes
        SET Status = @Status
        WHERE LocacaoID = @idLocacao;
        
        PRINT 'Locacao Atualizada';
    END TRY
    BEGIN CATCH
        PRINT 'Erro ao atualizar locacao: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END


