CREATE DATABASE EstacionamentoDB;
GO
USE EstacionamentoDB;
GO

CREATE TABLE Veiculos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Placa VARCHAR(10) NOT NULL UNIQUE,
    Modelo VARCHAR(50) NOT NULL,
    Cor VARCHAR(30) NOT NULL,
    Tipo INT NOT NULL 
);

CREATE TABLE Tickets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VeiculoId INT NOT NULL,
    Vaga VARCHAR(10) NOT NULL,
    Entrada DATETIME NOT NULL,
    Saida DATETIME NULL,
    Status VARCHAR(20) DEFAULT 'Aberto',
    CONSTRAINT FK_Tickets_Veiculos FOREIGN KEY (VeiculoId) REFERENCES Veiculos(Id)
);

CREATE TABLE Pagamentos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TicketId INT NOT NULL,
    TipoPagamento INT NOT NULL, 
    ValorPago DECIMAL(10,2) NOT NULL,
    DataHora DATETIME NOT NULL,
    CONSTRAINT FK_Pagamentos_Tickets FOREIGN KEY (TicketId) REFERENCES Tickets(Id)
);
GO