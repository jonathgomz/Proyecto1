CREATE DATABASE CalculadoraDB;

USE CalculadoraDB;

CREATE TABLE HistorialCalculos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Operacion NVARCHAR(100) NOT NULL,
    Resultado NVARCHAR(50) NOT NULL,
    FechaHora DATETIME DEFAULT GETDATE()
);

USE CalculadoraDB
SELECT * FROM HistorialCalculos
DELETE FROM HistorialCalculos

SELECT TOP 10 * FROM HistorialCalculos ORDER BY FechaHora ASC;