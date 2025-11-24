-- Criação do Banco de Dados
IF NOT EXISTS (SELECT name
               FROM master.dbo.sysdatabases
               WHERE name = N'LocadoraBD')
    BEGIN
        CREATE DATABASE LocadoraBD;
    END;
GO

USE LocadoraBD;
GO

-- 1. Tabela tblClientes (Parte do 1:1 com tblDocumentos e 1:N com tblLocacoes)
CREATE TABLE tblClientes
(
    ClienteID INT PRIMARY KEY IDENTITY (1,1),
    Nome      VARCHAR(100)        NOT NULL,
    Email     VARCHAR(100) UNIQUE NOT NULL,
    Telefone  VARCHAR(20)
);

-- 2. Tabela tblDocumentos (Relacionamento 1:1 com tblClientes)
-- ClienteID é FK e UNIQUE, garantindo que cada cliente tenha apenas um documento e cada documento pertença a um único cliente.
CREATE TABLE tblDocumentos
(
    DocumentoID   INT PRIMARY KEY IDENTITY (1,1),
    ClienteID     INT UNIQUE         NOT NULL, -- Garante o 1:1
    TipoDocumento VARCHAR(10)        NOT NULL, -- Ex: 'CPF', 'CNPJ'
    Numero        VARCHAR(20) UNIQUE NOT NULL,
    DataEmissao   DATE,
    DataValidade  DATE,
    CONSTRAINT FK_Documentos_Clientes FOREIGN KEY (ClienteID) REFERENCES tblClientes (ClienteID) ON DELETE CASCADE
);

-- 3. Tabela tblCategorias (Parte do 1:N com tblVeiculos)
CREATE TABLE tblCategorias
(
    CategoriaID INT PRIMARY KEY IDENTITY (1,1),
    Nome        VARCHAR(50) UNIQUE NOT NULL,
    Descricao   VARCHAR(255),
    Diaria      DECIMAL(10, 2)     NOT NULL
);

-- 4. Tabela tblVeiculos (Relacionamento 1:N com tblCategorias e 1:N com tblLocacoes)
CREATE TABLE tblVeiculos
(
    VeiculoID     INT PRIMARY KEY IDENTITY (1,1),
    CategoriaID   INT                NOT NULL,
    Placa         VARCHAR(10) UNIQUE NOT NULL,
    Marca         VARCHAR(50)        NOT NULL,
    Modelo        VARCHAR(50)        NOT NULL,
    Ano           INT                NOT NULL,
    StatusVeiculo VARCHAR(20)        NOT NULL DEFAULT '0', -- 'Disponível', 'Alugado', 'Manutenção'
    CONSTRAINT FK_Veiculos_Categorias FOREIGN KEY (CategoriaID) REFERENCES tblCategorias (CategoriaID)
);

-- 5. Tabela tblFuncionarios (Parte do N:M com tblLocacoes via tblLocacaoFuncionarios)
CREATE TABLE tblFuncionarios
(
    FuncionarioID INT PRIMARY KEY IDENTITY (1,1),
    Nome          VARCHAR(100)        NOT NULL,
    CPF           VARCHAR(14) UNIQUE  NOT NULL,
    Email         VARCHAR(100) UNIQUE NOT NULL,
    Salario       DECIMAL(10, 2)
);

-- 6. Tabela tblLocacoes (Relacionamento 1:N com tblClientes e tblVeiculos, e N:M com tblFuncionarios)
CREATE TABLE tblLocacoes
(
    LocacaoID             UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ClienteID             INT            NOT NULL,
    VeiculoID             INT            NOT NULL,
    DataLocacao           DATETIME       NOT NULL      DEFAULT GETDATE(),
    DataDevolucaoPrevista DATETIME       NOT NULL,
    DataDevolucaoReal     DATETIME,                                     -- Pode ser NULL se a locação estiver ativa
    ValorDiaria           DECIMAL(10, 2) NOT NULL,
    ValorTotal            DECIMAL(10, 2)               DEFAULT 0.00,
    Multa                 DECIMAL(10, 2)               DEFAULT 0.00,
    Status                VARCHAR(20)    NOT NULL      DEFAULT 'Ativa', -- 'Ativa', 'Finalizada', 'Cancelada'
    CONSTRAINT FK_Locacoes_Clientes FOREIGN KEY (ClienteID) REFERENCES tblClientes (ClienteID),
    CONSTRAINT FK_Locacoes_Veiculos FOREIGN KEY (VeiculoID) REFERENCES tblVeiculos (VeiculoID)
);

-- 7. Tabela tblLocacaoFuncionarios (Tabela de Junção para o relacionamento N:M entre tblLocacoes e tblFuncionarios)
CREATE TABLE tblLocacaoFuncionarios
(
    LocacaoFuncionarioID INT PRIMARY KEY IDENTITY (1,1),
    LocacaoID            UNIQUEIDENTIFIER NOT NULL,
    FuncionarioID        INT              NOT NULL,
    CONSTRAINT FK_LocFunc_Locacoes FOREIGN KEY (LocacaoID) REFERENCES tblLocacoes (LocacaoID) ON DELETE CASCADE,
    CONSTRAINT FK_LocFunc_Funcionarios FOREIGN KEY (FuncionarioID) REFERENCES tblFuncionarios (FuncionarioID),
    CONSTRAINT UQ_Locacao_Funcionario UNIQUE (LocacaoID, FuncionarioID) -- Garante que um funcionário não seja associado duas vezes à mesma locação
);

-- Inserção de Dados Iniciais
-- Clientes e Documentos (1:1)
INSERT INTO tblClientes (Nome, Email, Telefone)
VALUES ('João Silva', 'joao.silva@email.com', '11987654321'),
       ('Maria Souza', 'maria.souza@email.com', '21998765432'),
       ('Carlos Pereira', 'carlos.pereira@email.com', '31976543210');

INSERT INTO tblDocumentos (ClienteID, TipoDocumento, Numero, DataEmissao, DataValidade)
VALUES (1, 'CPF', '12345678900', '2000-01-15', '2030-01-15'),
       (2, 'CPF', '98765432111', '2005-03-20', '2035-03-20'),
       (3, 'CPF', '11122233344', '2010-07-01', '2040-07-01');

-- Categorias (1:N)
INSERT INTO tblCategorias (Nome, Descricao, Diaria)
VALUES ('Compacto', 'Carros pequenos para cidade', 80.00),
       ('Sedan', 'Carros médios, conforto', 120.00),
       ('SUV', 'Carros grandes, espaço', 180.00);

-- Veículos (1:N com Categorias)
INSERT INTO tblVeiculos (CategoriaID, Placa, Marca, Modelo, Ano, StatusVeiculo)
VALUES (1, 'ABC1234', 'Fiat', 'Mobi', 2020, '0'),
       (1, 'DEF5678', 'Renault', 'Kwid', 2021, '0'),
       (2, 'GHI9012', 'Chevrolet', 'Onix Plus', 2022, '0'),
       (2, 'JKL3456', 'Hyundai', 'HB20S', 2023, '0'),
       (3, 'MNO7890', 'Jeep', 'Renegade', 2022, '0');

-- Funcionários (N:M)
INSERT INTO tblFuncionarios (Nome, CPF, Email, Salario)
VALUES ('Ana Costa', '44455566677', 'ana.costa@locadora.com', 2500.00),
       ('Pedro Santos', '77788899900', 'pedro.santos@locadora.com', 2800.00);

-- Locações (para teste, sem funcionários associados ainda)
DECLARE @LocacaoID1 UNIQUEIDENTIFIER;
DECLARE @LocacaoID2 UNIQUEIDENTIFIER;
DECLARE @LocacaoID3 UNIQUEIDENTIFIER;
-- Locação 1: João aluga Mobi
INSERT INTO tblLocacoes (ClienteID, VeiculoID, DataLocacao, DataDevolucaoPrevista, ValorDiaria, Status)
VALUES (1, 1, GETDATE(), DATEADD(day, 5, GETDATE()), 80.00, '0');
UPDATE tblVeiculos
SET StatusVeiculo = 'Alugado'
WHERE VeiculoID = 1;

SET @LocacaoID1 = SCOPE_IDENTITY();

-- Locação 2: Maria aluga Onix Plus
INSERT INTO tblLocacoes (ClienteID, VeiculoID, DataLocacao, DataDevolucaoPrevista, ValorDiaria, Status)
VALUES (2, 3, GETDATE(), DATEADD(day, 7, GETDATE()), 120.00, '0');
UPDATE tblVeiculos
SET StatusVeiculo = 'Alugado'
WHERE VeiculoID = 3;

SET @LocacaoID2 =  SCOPE_IDENTITY();

-- Locação 3: Carlos aluga Renegade (já finalizada)
INSERT INTO tblLocacoes (ClienteID, VeiculoID, DataLocacao, DataDevolucaoPrevista, DataDevolucaoReal, ValorDiaria,
                         ValorTotal, Status)
VALUES (3, 5, DATEADD(day, -10, GETDATE()), DATEADD(day, -5, GETDATE()), DATEADD(day, -5, GETDATE()), 180.00, 900.00,
        '1');
-- Veículo 5 deve estar disponível novamente

-- LocaçãoFuncionarios (N:M)
-- Ana e Pedro envolvidos na Locação 1
INSERT INTO tblLocacaoFuncionarios (LocacaoID, FuncionarioID)
VALUES (@LocacaoID1, 1),
       (@LocacaoID2, 2),
       (@LocacaoID3, 2);
GO


-- Pedro envolvido na Locação 2
INSERT INTO tblLocacaoFuncionarios (LocacaoID, FuncionarioID)
VALUES (2, 2);
GO

CREATE OR ALTER PROCEDURE sp_INSERIRCATEGORIA @NomeCategoria VARCHAR(50),
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
        -- Texto corrigido de 'cliente' para 'categoria'
        PRINT 'Um erro aconteceu ao adicionar a categoria: ' + ERROR_MESSAGE()
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
                                              @StatusLocacao VARCHAR(20) NULL,
                                              @idFuncionario INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TabelaTemp TABLE (IdGerado UNIQUEIDENTIFIER);
    DECLARE @idLocacao UNIQUEIDENTIFIER;
    
    BEGIN TRY
        INSERT INTO tblLocacoes (ClienteID, 
                                 VeiculoID, 
                                 DataDevolucaoPrevista, 
                                 DataDevolucaoReal, 
                                 ValorDiaria,
                                 ValorTotal, 
                                 Multa, 
                                 Status)
        OUTPUT inserted.LocacaoID INTO @TabelaTemp(IdGerado)
        VALUES (@ClienteIdLocacao, 
                @VeiculoIdLocacao, 
                @DataDevolucaoPrevistaLocacao,
                @DataDevolucaoRealLocacao, 
                @ValorDiariaLocacao,
                @ValorTotalLocacao, 
                @MultaLocacao, 
                @StatusLocacao);
        
        SELECT @idLocacao = IdGerado FROM @TabelaTemp;
        
        INSERT INTO tblLocacaoFuncionarios (LocacaoID, FuncionarioID) 
        VALUES (@idLocacao, @idFuncionario);

        PRINT 'Locacao adicionada com sucesso!'
    END TRY
    BEGIN CATCH
        PRINT 'Erro ao adicionar locacao' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_AtualizarLocacao @idLocacao UNIQUEIDENTIFIER,
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

        PRINT 'Locacao atualizada com sucesso!'
    END TRY
    BEGIN CATCH
        PRINT 'Erro ao atualizar localizacao: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_BuscarLocacaoId @idLocacao UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT LocacaoID,
               ClienteID,
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
        PRINT 'Erro ao buscar locacao: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_BuscarLocacao
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT LocacaoID,
               ClienteID,
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

CREATE OR ALTER PROCEDURE sp_BuscarVeiculoId @idVeiculo INT
AS
BEGIN
    BEGIN TRY
        SELECT Modelo FROM tblVeiculos WHERE VeiculoID = @idVeiculo;
    END TRY
    BEGIN CATCH
        PRINT 'Erro ao buscar veiculo ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_AdicionarCategoria @NomeCategoria VARCHAR(50),
                                                @DescricaoCategoria VARCHAR(255) NULL,
                                                @DiariaCategoria DECIMAL(10, 2)
AS
BEGIN
    SET
        NOCOUNT ON;

    BEGIN TRY
        INSERT INTO tblCategorias (Nome, Descricao, Diaria)
        VALUES (@NomeCategoria, @DescricaoCategoria, @DiariaCategoria);

        PRINT
            'Categoria adicionada!'
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
    SET
        NOCOUNT ON;

    BEGIN TRY
        UPDATE tblCategorias
        SET Nome      = @NomeCategoria,
            Descricao = @DescricaoCategoria,
            Diaria    = @DiariaCategoria
        WHERE CategoriaId = @CategoriaId;

        PRINT
            'Categoria atualizada!'
    END TRY
    BEGIN CATCH
        print 'Um erro aconteceu ao atualizar o cliente: ' + ERROR_MESSAGE()
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_ExcluirCategoria @CategoriaId INTEGER
AS
BEGIN
    SET
        NOCOUNT ON;

    BEGIN TRY
        DELETE
        FROM tblCategorias
        WHERE CategoriaID = @CategoriaId;

        PRINT
            'Categoria excluida!'
    END TRY
    BEGIN CATCH
        PRINT 'Um erro aconteceu ao adicionar o cliente: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_CancelarLocacao @idLocacao INT,
                                             @Status VARCHAR(20)
AS
BEGIN
    SET
        NOCOUNT ON;
    BEGIN TRY
        UPDATE tblLocacoes
        SET Status = @Status
        WHERE LocacaoID = @idLocacao;

        PRINT
            'Locacao Atualizada';
    END TRY
    BEGIN CATCH
        PRINT 'Erro ao atualizar locacao: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_BuscarClienteId @idCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT Nome
        FROM tblClientes
        WHERE ClienteID = @idCliente;
    END TRY
    BEGIN CATCH
        PRINT 'Erro ao buscar cliente ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
GO
