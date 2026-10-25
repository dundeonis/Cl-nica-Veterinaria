using Microsoft.Data.Sqlite;
using System.Data;

namespace ClinicaVeterinaria;

public static class Conexion
{
    private static string cadena = "Data Source=clinica.db";

    public static SqliteConnection Abrir()
    {
        var con = new SqliteConnection(cadena);
        con.Open();
        var cmd = con.CreateCommand();
        cmd.CommandText = "PRAGMA foreign_keys = ON;";
        cmd.ExecuteNonQuery();
        return con;
    }

    public static void CrearTablas()
    {
        using var con = Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Propietario (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                Telefono TEXT,
                Direccion TEXT
            );
            CREATE TABLE IF NOT EXISTS Mascota (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                Especie TEXT NOT NULL,
                Raza TEXT,
                Edad INTEGER,
                PropietarioId INTEGER NOT NULL,
                FOREIGN KEY (PropietarioId) REFERENCES Propietario(Id) ON DELETE CASCADE
            );
            CREATE TABLE IF NOT EXISTS Cita (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Fecha TEXT NOT NULL,
                Motivo TEXT NOT NULL,
                MascotaId INTEGER NOT NULL,
                FOREIGN KEY (MascotaId) REFERENCES Mascota(Id) ON DELETE CASCADE
            );";
        cmd.ExecuteNonQuery();
    }
}

public static class PropietarioRepo
{
    public static void Agregar(Propietario p)
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = "INSERT INTO Propietario (Nombre, Telefono, Direccion) VALUES ($n, $t, $d)";
        cmd.Parameters.AddWithValue("$n", p.Nombre);
        cmd.Parameters.AddWithValue("$t", p.Telefono);
        cmd.Parameters.AddWithValue("$d", p.Direccion);
        cmd.ExecuteNonQuery();
    }

    public static DataTable Listar()
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT Id, Nombre, Telefono, Direccion FROM Propietario ORDER BY Nombre";
        var tabla = new DataTable();
        using var lector = cmd.ExecuteReader();
        tabla.Load(lector);
        return tabla;
    }

    public static void Modificar(Propietario p)
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = "UPDATE Propietario SET Nombre = $n, Telefono = $t, Direccion = $d WHERE Id = $id";
        cmd.Parameters.AddWithValue("$n", p.Nombre);
        cmd.Parameters.AddWithValue("$t", p.Telefono);
        cmd.Parameters.AddWithValue("$d", p.Direccion);
        cmd.Parameters.AddWithValue("$id", p.Id);
        cmd.ExecuteNonQuery();
    }

    public static void Eliminar(int id)
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = "DELETE FROM Propietario WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }
}

public static class MascotaRepo
{
    public static void Agregar(Mascota m)
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = "INSERT INTO Mascota (Nombre, Especie, Raza, Edad, PropietarioId) VALUES ($n, $e, $r, $ed, $p)";
        cmd.Parameters.AddWithValue("$n", m.Nombre);
        cmd.Parameters.AddWithValue("$e", m.Especie);
        cmd.Parameters.AddWithValue("$r", m.Raza);
        cmd.Parameters.AddWithValue("$ed", m.Edad);
        cmd.Parameters.AddWithValue("$p", m.PropietarioId);
        cmd.ExecuteNonQuery();
    }

    public static DataTable Listar()
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = @"SELECT m.Id, m.Nombre, m.Especie, m.Raza, m.Edad, m.PropietarioId, p.Nombre AS Propietario
                            FROM Mascota m
                            INNER JOIN Propietario p ON p.Id = m.PropietarioId
                            ORDER BY m.Nombre";
        var tabla = new DataTable();
        using var lector = cmd.ExecuteReader();
        tabla.Load(lector);
        return tabla;
    }

    public static void Modificar(Mascota m)
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = "UPDATE Mascota SET Nombre = $n, Especie = $e, Raza = $r, Edad = $ed, PropietarioId = $p WHERE Id = $id";
        cmd.Parameters.AddWithValue("$n", m.Nombre);
        cmd.Parameters.AddWithValue("$e", m.Especie);
        cmd.Parameters.AddWithValue("$r", m.Raza);
        cmd.Parameters.AddWithValue("$ed", m.Edad);
        cmd.Parameters.AddWithValue("$p", m.PropietarioId);
        cmd.Parameters.AddWithValue("$id", m.Id);
        cmd.ExecuteNonQuery();
    }

    public static void Eliminar(int id)
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = "DELETE FROM Mascota WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }
}

public static class CitaRepo
{
    public static void Agregar(Cita c)
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = "INSERT INTO Cita (Fecha, Motivo, MascotaId) VALUES ($f, $m, $ma)";
        cmd.Parameters.AddWithValue("$f", c.Fecha);
        cmd.Parameters.AddWithValue("$m", c.Motivo);
        cmd.Parameters.AddWithValue("$ma", c.MascotaId);
        cmd.ExecuteNonQuery();
    }

    public static DataTable Listar()
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = @"SELECT c.Id, c.Fecha, c.Motivo, c.MascotaId, m.Nombre AS Mascota
                            FROM Cita c
                            INNER JOIN Mascota m ON m.Id = c.MascotaId
                            ORDER BY c.Fecha";
        var tabla = new DataTable();
        using var lector = cmd.ExecuteReader();
        tabla.Load(lector);
        return tabla;
    }

    public static void Modificar(Cita c)
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = "UPDATE Cita SET Fecha = $f, Motivo = $m, MascotaId = $ma WHERE Id = $id";
        cmd.Parameters.AddWithValue("$f", c.Fecha);
        cmd.Parameters.AddWithValue("$m", c.Motivo);
        cmd.Parameters.AddWithValue("$ma", c.MascotaId);
        cmd.Parameters.AddWithValue("$id", c.Id);
        cmd.ExecuteNonQuery();
    }

    public static void Eliminar(int id)
    {
        using var con = Conexion.Abrir();
        var cmd = con.CreateCommand();
        cmd.CommandText = "DELETE FROM Cita WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }
}
