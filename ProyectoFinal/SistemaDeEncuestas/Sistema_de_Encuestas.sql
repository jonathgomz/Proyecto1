
USE master;
GO

-- Eliminar base de datos si existe
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'Sistema_de_Encuestas')
BEGIN
    ALTER DATABASE Sistema_de_Encuestas SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Sistema_de_Encuestas;
END
GO

-- Crear base de datos
CREATE DATABASE Sistema_de_Encuestas;
GO

USE Sistema_de_Encuestas;
GO

-- Almacena las encuestas creadas
CREATE TABLE Encuesta(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(200) NOT NULL,
    Descripcion VARCHAR(MAX),
    Creador VARCHAR(100) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Almacena las preguntas de cada encuesta
-- TipoPregunta: 'Unica' (radio) o 'Multiple' (checkbox)
CREATE TABLE Pregunta(
    Id INT PRIMARY KEY IDENTITY(1,1),
    EncuestaId INT NOT NULL,
    Texto VARCHAR(MAX) NOT NULL,
    TipoPregunta VARCHAR(20) NOT NULL DEFAULT 'Unica',
    Orden INT NOT NULL DEFAULT 1,
    FOREIGN KEY(EncuestaId) REFERENCES Encuesta(Id) ON DELETE CASCADE,
    CHECK (TipoPregunta IN ('Unica', 'Multiple'))
);
GO

-- Almacena las opciones de respuesta de cada pregunta
CREATE TABLE OpcionPregunta(
    Id INT PRIMARY KEY IDENTITY(1,1),
    PreguntaId INT NOT NULL,
    TextoOpcion VARCHAR(500) NOT NULL,
    Orden INT NOT NULL DEFAULT 1,
    FOREIGN KEY(PreguntaId) REFERENCES Pregunta(Id) ON DELETE CASCADE
);
GO

-- Almacena la información de cada usuario que responde
CREATE TABLE RespuestaUsuario(
    Id INT PRIMARY KEY IDENTITY(1,1),
    EncuestaId INT NOT NULL,
    Usuario VARCHAR(100) NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    TiempoSegundos INT NOT NULL,
    FOREIGN KEY(EncuestaId) REFERENCES Encuesta(Id) ON DELETE CASCADE
);
GO

-- Almacena cada respuesta individual a cada pregunta
CREATE TABLE RespuestaDetalle(
    Id INT PRIMARY KEY IDENTITY(1,1),
    RespuestaUsuarioId INT NOT NULL,
    PreguntaId INT NOT NULL,
    OpcionId INT NOT NULL,
    FOREIGN KEY(RespuestaUsuarioId) REFERENCES RespuestaUsuario(Id) ON DELETE CASCADE,
    FOREIGN KEY(PreguntaId) REFERENCES Pregunta(Id),
    FOREIGN KEY(OpcionId) REFERENCES OpcionPregunta(Id)
);
GO

-- DATOS DE PRUEBA 

-- Encuesta de ejemplo
INSERT INTO Encuesta (Titulo, Descripcion, Creador, FechaCreacion)
VALUES ('Satisfacción del Servicio', 'Encuesta para medir la satisfacción de nuestros clientes', 'Admin', GETDATE());

-- Pregunta 1: Opción única
INSERT INTO Pregunta (EncuestaId, Texto, TipoPregunta, Orden)
VALUES (1, '¿Cómo calificarías nuestro servicio?', 'Unica', 1);

INSERT INTO OpcionPregunta (PreguntaId, TextoOpcion, Orden)
VALUES 
    (1, 'Excelente', 1),
    (1, 'Bueno', 2),
    (1, 'Regular', 3),
    (1, 'Malo', 4);

-- Pregunta 2: Opción múltiple
INSERT INTO Pregunta (EncuestaId, Texto, TipoPregunta, Orden)
VALUES (1, '¿Qué aspectos te gustaron? (Puedes seleccionar varios)', 'Multiple', 2);

INSERT INTO OpcionPregunta (PreguntaId, TextoOpcion, Orden)
VALUES 
    (2, 'Atención rápida', 1),
    (2, 'Buen precio', 2),
    (2, 'Calidad del producto', 3),
    (2, 'Facilidad de uso', 4);

GO

-- VISTAS ÚTILES PARA REPORTES

-- Vista: Resumen de encuestas
CREATE VIEW vw_ResumenEncuestas AS
SELECT 
    e.Id,
    e.Titulo,
    e.Descripcion,
    e.Creador,
    e.FechaCreacion,
    COUNT(DISTINCT ru.Id) AS TotalRespuestas,
    COUNT(DISTINCT p.Id) AS TotalPreguntas
FROM Encuesta e
LEFT JOIN RespuestaUsuario ru ON e.Id = ru.EncuestaId
LEFT JOIN Pregunta p ON e.Id = p.EncuestaId
GROUP BY e.Id, e.Titulo, e.Descripcion, e.Creador, e.FechaCreacion;
GO

-- Vista: Detalle de respuestas por encuesta
CREATE VIEW vw_DetalleRespuestas AS
SELECT 
    e.Titulo AS Encuesta,
    ru.Usuario,
    ru.Fecha,
    ru.TiempoSegundos,
    p.Texto AS Pregunta,
    op.TextoOpcion AS Respuesta
FROM RespuestaUsuario ru
INNER JOIN Encuesta e ON ru.EncuestaId = e.Id
INNER JOIN RespuestaDetalle rd ON ru.Id = rd.RespuestaUsuarioId
INNER JOIN Pregunta p ON rd.PreguntaId = p.Id
INNER JOIN OpcionPregunta op ON rd.OpcionId = op.Id;
GO


--final--
PRINT 'Base de datos creada exitosamente';
SELECT * FROM vw_ResumenEncuestas;

USE Sistema_de_Encuestas;
GO

-- ============================================================
-- ENCUESTA 1: Preferencias de Café
-- ============================================================
INSERT INTO Encuesta (Titulo, Descripcion, Creador, FechaCreacion) 
VALUES ('Preferencias de Café', 'Ayúdanos a conocer tus gustos sobre el café', 'Juan Pérez', '2024-11-15 09:30:00');
DECLARE @EncuestaId1 INT = SCOPE_IDENTITY();

INSERT INTO Pregunta (EncuestaId, Texto, TipoPregunta, Orden) VALUES
(@EncuestaId1, '¿Cómo prefieres tu café?', 'Unica', 1),
(@EncuestaId1, '¿Qué complementos te gustan? (puedes elegir varios)', 'Multiple', 2),
(@EncuestaId1, '¿En qué momento del día tomas café?', 'Unica', 3);

DECLARE @P1 INT = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId1 AND Orden = 1);
DECLARE @P2 INT = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId1 AND Orden = 2);
DECLARE @P3 INT = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId1 AND Orden = 3);

INSERT INTO OpcionPregunta (PreguntaId, TextoOpcion, Orden) VALUES
(@P1, 'Negro', 1), (@P1, 'Con leche', 2), (@P1, 'Capuchino', 3), (@P1, 'Espresso', 4),
(@P2, 'Azúcar', 1), (@P2, 'Crema', 2), (@P2, 'Canela', 3), (@P2, 'Miel', 4),
(@P3, 'Mañana', 1), (@P3, 'Tarde', 2), (@P3, 'Noche', 3), (@P3, 'Todo el día', 4);

-- Respuestas
INSERT INTO RespuestaUsuario (EncuestaId, Usuario, Fecha, TiempoSegundos) VALUES
(@EncuestaId1, 'María García', '2024-11-15 10:00:00', 45),
(@EncuestaId1, 'Carlos Ruiz', '2024-11-15 11:30:00', 38),
(@EncuestaId1, 'Ana López', '2024-11-15 14:20:00', 52),
(@EncuestaId1, 'Pedro Sánchez', '2024-11-16 08:15:00', 41),
(@EncuestaId1, 'Laura Martínez', '2024-11-16 09:45:00', 48);

INSERT INTO RespuestaDetalle (RespuestaUsuarioId, PreguntaId, OpcionId) VALUES
-- María: Con leche, Azúcar+Crema, Mañana
(1, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 2)),
(1, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(1, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2)),
(1, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 1)),
-- Carlos: Espresso, Azúcar, Todo el día
(2, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 4)),
(2, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(2, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 4)),
-- Ana: Capuchino, Canela, Mañana
(3, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 3)),
(3, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 3)),
(3, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 1)),
-- Pedro: Negro, Sin complementos (ninguno), Mañana
(4, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 1)),
(4, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 1)),
-- Laura: Con leche, Azúcar+Miel, Tarde
(5, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 2)),
(5, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(5, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 4)),
(5, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 2));

-- ============================================================
-- ENCUESTA 2: Hábitos de Ejercicio
-- ============================================================
INSERT INTO Encuesta (Titulo, Descripcion, Creador, FechaCreacion) 
VALUES ('Hábitos de Ejercicio', 'Encuesta sobre rutinas de ejercicio físico', 'Gimnasio FitLife', '2024-11-10 07:00:00');
DECLARE @EncuestaId2 INT = SCOPE_IDENTITY();

INSERT INTO Pregunta (EncuestaId, Texto, TipoPregunta, Orden) VALUES
(@EncuestaId2, '¿Cuántos días a la semana haces ejercicio?', 'Unica', 1),
(@EncuestaId2, '¿Qué tipo de ejercicio prefieres?', 'Multiple', 2);

SET @P1 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId2 AND Orden = 1);
SET @P2 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId2 AND Orden = 2);

INSERT INTO OpcionPregunta (PreguntaId, TextoOpcion, Orden) VALUES
(@P1, 'No hago ejercicio', 1), (@P1, '1-2 días', 2), (@P1, '3-4 días', 3), (@P1, '5 o más días', 4),
(@P2, 'Cardio', 1), (@P2, 'Pesas', 2), (@P2, 'Yoga', 3), (@P2, 'Deportes', 4);

INSERT INTO RespuestaUsuario (EncuestaId, Usuario, Fecha, TiempoSegundos) VALUES
(@EncuestaId2, 'Roberto Díaz', '2024-11-10 08:30:00', 35),
(@EncuestaId2, 'Sofia Torres', '2024-11-11 19:00:00', 42),
(@EncuestaId2, 'Miguel Ángel', '2024-11-12 07:15:00', 28),
(@EncuestaId2, 'Lucía Fernández', '2024-11-12 18:30:00', 51),
(@EncuestaId2, 'Diego Castro', '2024-11-13 06:45:00', 33),
(@EncuestaId2, 'Valentina Rojas', '2024-11-13 20:00:00', 39),
(@EncuestaId2, 'Andrés Morales', '2024-11-14 07:30:00', 44);

INSERT INTO RespuestaDetalle (RespuestaUsuarioId, PreguntaId, OpcionId) VALUES
(6, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 3)),
(6, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(6, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2)),
(7, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 4)),
(7, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 3)),
(8, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 2)),
(8, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 4)),
(9, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 3)),
(9, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(9, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 3)),
(10, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 4)),
(10, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2)),
(11, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 3)),
(11, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(11, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 4)),
(12, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 1)),
(12, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1));

-- ============================================================
-- ENCUESTA 3: Solicitud para Invadir India
-- ============================================================
INSERT INTO Encuesta (Titulo, Descripcion, Creador, FechaCreacion) 
VALUES ('Solicitud para Invadir India', 'Encuesta estratégica sobre la viabilidad de una operación militar en el subcontinente indio', 'General Napoleón Bonaparte', '2024-12-01 00:00:00');
DECLARE @EncuestaId3 INT = SCOPE_IDENTITY();

INSERT INTO Pregunta (EncuestaId, Texto, TipoPregunta, Orden) VALUES
(@EncuestaId3, '¿Consideras que deberíamos invadir India?', 'Unica', 1),
(@EncuestaId3, '¿Qué recursos necesitaríamos? (selecciona todos los aplicables)', 'Multiple', 2),
(@EncuestaId3, '¿Cuál es tu mayor preocupación?', 'Unica', 3),
(@EncuestaId3, '¿Qué estrategia militar prefieres?', 'Unica', 4);

SET @P1 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId3 AND Orden = 1);
SET @P2 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId3 AND Orden = 2);
SET @P3 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId3 AND Orden = 3);
DECLARE @P4 INT = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId3 AND Orden = 4);

INSERT INTO OpcionPregunta (PreguntaId, TextoOpcion, Orden) VALUES
(@P1, 'Sí, definitivamente', 1), (@P1, 'Tal vez, necesitamos más información', 2), (@P1, 'No, es mala idea', 3), (@P1, 'Prefiero invadir otro lugar', 4),
(@P2, '1 millón de soldados', 1), (@P2, '500 elefantes de guerra', 2), (@P2, 'Curry ilimitado para las tropas', 3), (@P2, 'Mapas actualizados', 4), (@P2, 'Traductor Hindi-Español', 5),
(@P3, 'El clima monzónico', 1), (@P3, 'La comida muy picante', 2), (@P3, 'Las películas de Bollywood', 3), (@P3, 'Los 1,400 millones de habitantes', 4),
(@P4, 'Ataque frontal masivo', 1), (@P4, 'Infiltración silenciosa', 2), (@P4, 'Negociación diplomática (¿invasión pacífica?)', 3), (@P4, 'Esperar a que nos inviten', 4);

INSERT INTO RespuestaUsuario (EncuestaId, Usuario, Fecha, TiempoSegundos) VALUES
(@EncuestaId3, 'Alejandro Magno', '2024-12-01 10:00:00', 120),
(@EncuestaId3, 'Genghis Khan', '2024-12-01 11:30:00', 95),
(@EncuestaId3, 'Julio César', '2024-12-01 14:15:00', 180),
(@EncuestaId3, 'Sun Tzu', '2024-12-01 15:45:00', 240),
(@EncuestaId3, 'Mahatma Gandhi', '2024-12-01 16:00:00', 60),
(@EncuestaId3, 'Joan of Arc', '2024-12-02 09:00:00', 110);

INSERT INTO RespuestaDetalle (RespuestaUsuarioId, PreguntaId, OpcionId) VALUES
-- Alejandro Magno: Sí, Soldados+Elefantes+Mapas, Habitantes, Ataque frontal
(13, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 1)),
(13, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(13, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2)),
(13, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 4)),
(13, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 4)),
(13, @P4, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P4 AND Orden = 1)),
-- Genghis Khan: Sí, Soldados+Elefantes+Curry, Clima, Ataque frontal
(14, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 1)),
(14, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(14, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2)),
(14, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 3)),
(14, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 1)),
(14, @P4, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P4 AND Orden = 1)),
-- Julio César: Tal vez, Todos los recursos, Comida picante, Negociación
(15, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 2)),
(15, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(15, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2)),
(15, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 3)),
(15, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 4)),
(15, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 5)),
(15, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 2)),
(15, @P4, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P4 AND Orden = 3)),
-- Sun Tzu: Tal vez, Mapas+Traductor, Habitantes, Infiltración
(16, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 2)),
(16, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 4)),
(16, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 5)),
(16, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 4)),
(16, @P4, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P4 AND Orden = 2)),
-- Gandhi: No, Ninguno, Bollywood, Esperar invitación
(17, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 3)),
(17, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 3)),
(17, @P4, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P4 AND Orden = 4)),
-- Joan of Arc: Prefiero otro lugar, Soldados, Clima, Negociación
(18, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 4)),
(18, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(18, @P3, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P3 AND Orden = 1)),
(18, @P4, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P4 AND Orden = 3));

-- ============================================================
-- ENCUESTA 4: Plataformas de Streaming
-- ============================================================
INSERT INTO Encuesta (Titulo, Descripcion, Creador, FechaCreacion) 
VALUES ('Plataformas de Streaming Favoritas', 'Conoce qué servicios prefiere la gente', 'TechMedia Inc', '2024-11-20 12:00:00');
DECLARE @EncuestaId4 INT = SCOPE_IDENTITY();

INSERT INTO Pregunta (EncuestaId, Texto, TipoPregunta, Orden) VALUES
(@EncuestaId4, '¿Cuál es tu plataforma principal?', 'Unica', 1),
(@EncuestaId4, '¿Qué géneros ves más? (elige varios)', 'Multiple', 2);

SET @P1 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId4 AND Orden = 1);
SET @P2 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId4 AND Orden = 2);

INSERT INTO OpcionPregunta (PreguntaId, TextoOpcion, Orden) VALUES
(@P1, 'Netflix', 1), (@P1, 'Disney+', 2), (@P1, 'Amazon Prime', 3), (@P1, 'HBO Max', 4),
(@P2, 'Acción', 1), (@P2, 'Comedia', 2), (@P2, 'Drama', 3), (@P2, 'Ciencia Ficción', 4), (@P2, 'Documentales', 5);

INSERT INTO RespuestaUsuario (EncuestaId, Usuario, Fecha, TiempoSegundos) VALUES
(@EncuestaId4, 'Camila Herrera', '2024-11-20 14:30:00', 55),
(@EncuestaId4, 'Fernando Gutiérrez', '2024-11-20 18:45:00', 47),
(@EncuestaId4, 'Isabella Romero', '2024-11-21 10:15:00', 62),
(@EncuestaId4, 'Santiago Vargas', '2024-11-21 20:00:00', 38);

INSERT INTO RespuestaDetalle (RespuestaUsuarioId, PreguntaId, OpcionId) VALUES
(19, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 1)),
(19, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2)),
(19, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 3)),
(20, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 3)),
(20, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(20, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 4)),
(21, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 2)),
(21, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2)),
(22, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 4)),
(22, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 3)),
(22, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 5));

-- ============================================================
-- ENCUESTA 5: Comida Favorita
-- ============================================================
INSERT INTO Encuesta (Titulo, Descripcion, Creador, FechaCreacion) 
VALUES ('Tu Comida Favorita', 'Descubre las preferencias culinarias', 'Restaurante El Sabor', '2024-11-18 08:00:00');
DECLARE @EncuestaId5 INT = SCOPE_IDENTITY();

INSERT INTO Pregunta (EncuestaId, Texto, TipoPregunta, Orden) VALUES
(@EncuestaId5, '¿Cuál es tu tipo de comida favorita?', 'Unica', 1),
(@EncuestaId5, '¿Qué platos te gustan?', 'Multiple', 2);

SET @P1 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId5 AND Orden = 1);
SET @P2 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId5 AND Orden = 2);

INSERT INTO OpcionPregunta (PreguntaId, TextoOpcion, Orden) VALUES
(@P1, 'Italiana', 1), (@P1, 'Mexicana', 2), (@P1, 'China', 3), (@P1, 'Japonesa', 4),
(@P2, 'Pizza', 1), (@P2, 'Tacos', 2), (@P2, 'Sushi', 3), (@P2, 'Hamburguesas', 4);

INSERT INTO RespuestaUsuario (EncuestaId, Usuario, Fecha, TiempoSegundos) VALUES
(@EncuestaId5, 'Gabriela Mendoza', '2024-11-18 12:00:00', 42),
(@EncuestaId5, 'Ricardo Paredes', '2024-11-18 13:30:00', 35),
(@EncuestaId5, 'Natalia Cruz', '2024-11-18 19:00:00', 50),
(@EncuestaId5, 'Javier Ortiz', '2024-11-19 12:15:00', 44),
(@EncuestaId5, 'Elena Silva', '2024-11-19 20:30:00', 38),
(@EncuestaId5, 'Mateo Ramos', '2024-11-20 13:00:00', 41);

INSERT INTO RespuestaDetalle (RespuestaUsuarioId, PreguntaId, OpcionId) VALUES
(23, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 1)),
(23, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(24, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 2)),
(24, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2)),
(24, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 4)),
(25, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 4)),
(25, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 3)),
(26, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 1)),
(26, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(26, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 4)),
(27, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 3)),
(27, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 3)),
(28, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 2)),
(28, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2));

-- ============================================================
-- ENCUESTA 6: Videojuegos
-- ============================================================
INSERT INTO Encuesta (Titulo, Descripcion, Creador, FechaCreacion) 
VALUES ('Preferencias de Videojuegos', 'Encuesta para gamers', 'Gaming Zone', '2024-11-12 16:00:00');
DECLARE @EncuestaId6 INT = SCOPE_IDENTITY();

INSERT INTO Pregunta (EncuestaId, Texto, TipoPregunta, Orden) VALUES
(@EncuestaId6, '¿Qué plataforma usas principalmente?', 'Unica', 1),
(@EncuestaId6, '¿Qué géneros juegas? (varios)', 'Multiple', 2);

SET @P1 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId6 AND Orden = 1);
SET @P2 = (SELECT Id FROM Pregunta WHERE EncuestaId = @EncuestaId6 AND Orden = 2);

INSERT INTO OpcionPregunta (PreguntaId, TextoOpcion, Orden) VALUES
(@P1, 'PC', 1), (@P1, 'PlayStation', 2), (@P1, 'Xbox', 3), (@P1, 'Nintendo Switch', 4),
(@P2, 'Shooter', 1), (@P2, 'RPG', 2), (@P2, 'Estrategia', 3), (@P2, 'Deportes', 4);

INSERT INTO RespuestaUsuario (EncuestaId, Usuario, Fecha, TiempoSegundos) VALUES
(@EncuestaId6, 'Daniel Vega', '2024-11-12 18:00:00', 52),
(@EncuestaId6, 'Paula Navarro', '2024-11-13 10:30:00', 48),
(@EncuestaId6, 'Emilio Cortés', '2024-11-13 15:45:00', 55);

INSERT INTO RespuestaDetalle (RespuestaUsuarioId, PreguntaId, OpcionId) VALUES
(29, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 1)),
(29, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(29, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 3)),
(30, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 4)),
(30, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2)),
(31, @P1, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P1 AND Orden = 2)),
(31, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 1)),
(31, @P2, (SELECT Id FROM OpcionPregunta WHERE PreguntaId = @P2 AND Orden = 2));





