using ClinicaVeterinaria;

ApplicationConfiguration.Initialize();
Conexion.CrearTablas();
Application.Run(new MenuForm());
