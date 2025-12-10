-- ============================================================================
-- SIMOP - Sistema de Monitoreo de Proyectos Sociales
-- ============================================================================
-- Descripción: Base de datos para gestión y seguimiento de proyectos sociales
-- Autor: Ever Enrique Castellon Mazariego
-- Fecha: Diciembre 2024
-- Tecnologías: SQL Server, ASP.NET Core 8.0 Web API, Blazor Server (net 8.0)
-- ============================================================================

-- Crear base de datos
CREATE DATABASE SIMOP;
GO

USE SIMOP;
GO

-- ============================================================================
-- 1. TABLA: cat_Categorias
-- Descripción: Almacena las categorías de proyectos sociales
-- Ejemplos: Infraestructura, Salud, Educación, Asistencia Social
-- ============================================================================
CREATE TABLE cat_Categorias (
    cat_codigo INT IDENTITY PRIMARY KEY,
    cat_nombre NVARCHAR(255) NOT NULL
);
GO

-- ============================================================================
-- 2. TABLA: pro_Proyectos
-- Descripción: Proyectos sociales con información general, presupuesto y fechas
-- Estado y Porcentaje: Calculados dinámicamente en el frontend (Blazor)
-- ============================================================================
CREATE TABLE pro_Proyectos (
    pro_codigo INT IDENTITY PRIMARY KEY,
    pro_nombre NVARCHAR(255) NOT NULL,
    pro_descripcion NVARCHAR(1000),
    pro_codCategoria INT NOT NULL,
    pro_ubicacion NVARCHAR(200),
    pro_presupuestoTotal DECIMAL(18,2) NOT NULL,
    pro_fechaInicio DATE,
    pro_fechaFinEstimada DATE,
    pro_fechaCreacion DATETIME DEFAULT GETDATE(),
    pro_fechaActualizacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (pro_codCategoria) REFERENCES cat_Categorias(cat_codigo)
);
GO

-- ============================================================================
-- 3. TABLA: phi_proyectoHitos
-- Descripción: Hitos o puntos de control que marcan el avance de cada proyecto
-- Ejemplos: "Diseño Arquitectónico", "Construcción Fase 1", "Entrega Final"
-- Estado y Porcentaje: Calculados dinámicamente según tareas completadas
-- ============================================================================
CREATE TABLE phi_proyectoHitos (
    phi_codigo INT IDENTITY PRIMARY KEY,
    phi_codproyecto INT NOT NULL,
    phi_nombre NVARCHAR(200) NOT NULL,
    phi_fechaInicio DATE NULL,
    phi_fechaFin DATE NULL,
    FOREIGN KEY (phi_codproyecto) REFERENCES pro_Proyectos(pro_codigo)
);
GO

-- ============================================================================
-- 4. TABLA: pta_proyectoTareas
-- Descripción: Tareas específicas asociadas a proyectos y/o hitos
-- Estados: Pendiente, En Progreso, Completada
-- Ejemplos: "Capacitación docentes", "Visita técnica", "Revisión de ingeniería"
-- ============================================================================
CREATE TABLE pta_proyectoTareas (
    pta_codigo INT IDENTITY PRIMARY KEY,
    pta_codproyecto INT NOT NULL,
    pta_codHito INT NULL,
    pta_Descripcion NVARCHAR(300) NOT NULL,
    pta_Estado VARCHAR(20) NOT NULL DEFAULT 'Pendiente',
    pta_FechaInicio DATE NULL,
    pta_FechaFin DATE NULL,
    FOREIGN KEY (pta_codproyecto) REFERENCES pro_Proyectos(pro_codigo),
    FOREIGN KEY (pta_codHito) REFERENCES phi_proyectoHitos(phi_codigo)
);
GO

-- ============================================================================
-- 5. TABLA: ppre_proyectoPresupuesto
-- Descripción: Registro de movimientos de presupuesto por proyecto
-- Permite hacer seguimiento detallado de gastos y costos
-- ============================================================================
CREATE TABLE ppre_proyectoPresupuesto (
    ppre_codigo INT IDENTITY PRIMARY KEY,
    ppre_codproyecto INT NOT NULL,
    ppre_monto DECIMAL(18,2) NOT NULL,
    ppre_descripcion NVARCHAR(300),
    ppre_fecha DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ppre_codproyecto) REFERENCES pro_Proyectos(pro_codigo)
);
GO

-- ============================================================================
-- DATOS DE EJEMPLO
-- ============================================================================

-- Categorías
-- select * from cat_Categorias
INSERT INTO cat_Categorias (cat_nombre)
VALUES 
    ('Infraestructura'),
    ('Salud'),
    ('Educación'),
    ('Asistencia Social');
GO

-- Proyectos
-- select * from pro_Proyectos
INSERT INTO pro_Proyectos (pro_nombre, pro_descripcion, pro_codCategoria, pro_ubicacion, pro_presupuestoTotal, pro_fechaInicio, pro_fechaFinEstimada)
VALUES
    ('Centro Comunitario La Esperanza', 
     'Construcción y habilitación de un centro comunitario para actividades sociales y culturales.', 
     1, 'Zona Norte', 75000, '2025-01-10', '2025-12-30'),
    
    ('Programa Alimentación Escolar', 
     'Distribución de alimentos nutritivos a estudiantes de 15 escuelas en situación de vulnerabilidad.', 
     2, '15 Escuelas', 50000, '2025-02-01', '2025-06-30'),
    
    ('Becas Universitarias 2025', 
     'Otorgamiento de becas universitarias a estudiantes destacados de bajos recursos.', 
     3, 'Nacional', 200000, '2025-01-15', '2025-11-30'),
    
    ('Clínica Móvil Zona Rural', 
     'Atención médica móvil en zonas rurales sin acceso a servicios de salud.', 
     2, '8 Comunidades', 65000, '2025-03-01', '2025-07-15');
GO

-- Hitos (Ejemplo: Proyecto Alimentación Escolar)
-- select * from phi_proyectoHitos
INSERT INTO phi_proyectoHitos (phi_codproyecto, phi_nombre, phi_fechaInicio, phi_fechaFin)
VALUES
    (2, 'Evaluación Nutricional', '2025-02-01', '2025-02-20'),
    (2, 'Distribución Fase 1', '2025-02-25', '2025-03-10'),
    (2, 'Capacitación Docentes', '2025-03-15', '2025-04-05');
GO

-- Tareas (Ejemplo: Proyecto Centro Comunitario)
-- select * from pta_proyectoTareas
INSERT INTO pta_proyectoTareas (pta_codproyecto, pta_codHito, pta_Descripcion, pta_Estado, pta_FechaInicio, pta_FechaFin)
VALUES (1, NULL, 'Nivelación del terreno', 'Completada', '2025-04-01', '2025-04-10'),
    (1, NULL, 'Marcado de cimientos', 'Completada', '2025-04-11', '2025-04-15'),
    (1, NULL, 'Colocación de fundación', 'En Progreso', '2025-04-16', NULL),
    (1, NULL, 'Revisión de ingeniería', 'Pendiente', NULL, NULL);
GO

-- Movimientos de Presupuesto (Ejemplo: Centro Comunitario)
-- select * from ppre_proyectoPresupuesto
INSERT INTO ppre_proyectoPresupuesto (ppre_codproyecto, ppre_monto, ppre_descripcion)
VALUES 
    (1, 15000, 'Pago diseño arquitectónico'),
    (1, 12000, 'Compra de materiales de construcción iniciales'),
    (1, 21000, 'Avance construcción fase 1 - Mano de obra');
GO

-- ============================================================================
-- CONSULTAS ÚTILES
-- ============================================================================

-- Obtener porcentaje de progreso de hitos según tareas completadas
/*
SELECT 
    H.phi_codigo,
    H.phi_nombre,
    COUNT(T.pta_codigo) AS TotalTareas,
    SUM(CASE WHEN T.pta_Estado = 'Completada' THEN 1 ELSE 0 END) AS TareasCompletadas,
    (CAST(SUM(CASE WHEN T.pta_Estado = 'Completada' THEN 1 ELSE 0 END) AS FLOAT) 
     / NULLIF(COUNT(T.pta_codigo), 0)) * 100 AS PorcentajeAvance
FROM phi_proyectoHitos H
LEFT JOIN pta_proyectoTareas T ON H.phi_codigo = T.pta_codHito
WHERE H.phi_codproyecto = 1
GROUP BY H.phi_codigo, H.phi_nombre;
*/

-- Obtener presupuesto gastado por proyecto
/*
SELECT 
    P.pro_codigo,
    P.pro_nombre,
    P.pro_presupuestoTotal,
    ISNULL(SUM(PR.ppre_monto), 0) AS PresupuestoGastado,
    P.pro_presupuestoTotal - ISNULL(SUM(PR.ppre_monto), 0) AS PresupuestoDisponible,
    (ISNULL(SUM(PR.ppre_monto), 0) / P.pro_presupuestoTotal) * 100 AS PorcentajeUsado
FROM pro_Proyectos P
LEFT JOIN ppre_proyectoPresupuesto PR ON P.pro_codigo = PR.ppre_codproyecto
GROUP BY P.pro_codigo, P.pro_nombre, P.pro_presupuestoTotal;
*/
