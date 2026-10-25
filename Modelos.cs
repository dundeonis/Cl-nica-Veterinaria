namespace ClinicaVeterinaria;

public class Propietario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Direccion { get; set; }
}

public class Mascota
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Especie { get; set; }
    public string Raza { get; set; }
    public int Edad { get; set; }
    public int PropietarioId { get; set; }
}

public class Cita
{
    public int Id { get; set; }
    public string Fecha { get; set; }
    public string Motivo { get; set; }
    public int MascotaId { get; set; }
}
