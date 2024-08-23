CREATE PROCEDURE dbo.CreateUser
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(255),
    @Email NVARCHAR(255),
    @Logo VARBINARY(MAX) = NULL,
    @PasswordHash NVARCHAR(255),
    @Salt NVARCHAR(255),
    @Role NVARCHAR(50)
AS
BEGIN
    -- Verificar se o email já existe
    IF EXISTS (SELECT 1 FROM dbo.Users WHERE Email = @Email)
    BEGIN
        -- Lançar um erro se o email já existir
        RAISERROR('Email já cadastrado.', 16, 1)
        RETURN
    END

    -- Inserir o novo usuário na tabela Users
    INSERT INTO dbo.Users (Id, Name, Email, Logo, PasswordHash, Salt, Role)
    VALUES (@Id, @Name, @Email, @Logo, @PasswordHash, @Salt, @Role)
END
GO

CREATE PROCEDURE dbo.UpdateUser
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(255),
    @Logo VARBINARY(MAX) = NULL
AS
BEGIN
    -- Verificar se o usuário existe
    IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Id = @Id)
    BEGIN
        -- Lançar um erro se o usuário não existir
        RAISERROR('Usuário não encontrado.', 16, 1)
        RETURN
    END

    -- Atualizar os dados do usuário
    UPDATE dbo.Users
    SET 
        Name = @Name,
        Logo = @Logo
    WHERE Id = @Id
END
GO

CREATE PROCEDURE dbo.DeleteUser
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    -- Verificar se o usuário existe
    IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Id = @Id)
    BEGIN
        -- Lançar um erro se o usuário não existir
        RAISERROR('Usuário não encontrado.', 16, 1)
        RETURN
    END

    -- Excluir o usuário da tabela Users
    DELETE FROM dbo.Users WHERE Id = @Id
END
GO

CREATE PROCEDURE dbo.CreateAddress
    @Id UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @Street NVARCHAR(255),
    @Number NVARCHAR(50),
    @Neighborhood NVARCHAR(255),
    @City NVARCHAR(255),
    @PostalCode NVARCHAR(20),
    @State NVARCHAR(50),
    @Complement NVARCHAR(255) = NULL
AS
BEGIN
    INSERT INTO dbo.Address (Id, UserId, Street, Number, Neighborhood, City, PostalCode, State, Complement)
    VALUES (@Id, @UserId, @Street, @Number, @Neighborhood, @City, @PostalCode, @State, @Complement)
END
GO

CREATE PROCEDURE dbo.UpdateAddress
    @Id UNIQUEIDENTIFIER,
    @Street NVARCHAR(255),
    @Number NVARCHAR(50),
    @Neighborhood NVARCHAR(255),
    @City NVARCHAR(255),
    @PostalCode NVARCHAR(20),
    @State NVARCHAR(50),
    @Complement NVARCHAR(255) = NULL
AS
BEGIN
    UPDATE dbo.Address
    SET 
        Street = @Street,
        Number = @Number,
        Neighborhood = @Neighborhood,
        City = @City,
        PostalCode = @PostalCode,
        State = @State,
        Complement = @Complement
    WHERE Id = @Id
END
GO

CREATE PROCEDURE dbo.DeleteAddress
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM dbo.Address WHERE Id = @Id
END
GO