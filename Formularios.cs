namespace ClinicaVeterinaria;

public static class Ayuda
{
    public static void Etiqueta(Form f, string texto, int x, int y)
    {
        var l = new Label();
        l.Text = texto;
        l.Location = new Point(x, y);
        l.AutoSize = true;
        f.Controls.Add(l);
    }

    public static void Poner(Form f, Control c, int x, int y, int ancho)
    {
        c.Location = new Point(x, y);
        c.Width = ancho;
        f.Controls.Add(c);
    }

    public static void Boton(Form f, Button b, string texto, int x, int y)
    {
        b.Text = texto;
        b.Location = new Point(x, y);
        b.Width = 100;
        f.Controls.Add(b);
    }

    public static void ConfigurarGrid(Form f, DataGridView g, int x, int y, int ancho, int alto)
    {
        g.Location = new Point(x, y);
        g.Size = new Size(ancho, alto);
        g.ReadOnly = true;
        g.AllowUserToAddRows = false;
        g.MultiSelect = false;
        g.RowHeadersVisible = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        f.Controls.Add(g);
    }
}

public class MenuForm : Form
{
    Button btnPropietarios = new Button();
    Button btnMascotas = new Button();
    Button btnCitas = new Button();
    Button btnSalir = new Button();

    public MenuForm()
    {
        Text = "Clínica Veterinaria";
        Size = new Size(360, 320);
        StartPosition = FormStartPosition.CenterScreen;

        Ayuda.Boton(this, btnPropietarios, "Propietarios", 100, 40);
        Ayuda.Boton(this, btnMascotas, "Mascotas", 100, 100);
        Ayuda.Boton(this, btnCitas, "Citas", 100, 160);
        Ayuda.Boton(this, btnSalir, "Salir", 100, 220);

        btnPropietarios.Width = 140;
        btnMascotas.Width = 140;
        btnCitas.Width = 140;
        btnSalir.Width = 140;

        btnPropietarios.Click += AbrirPropietarios;
        btnMascotas.Click += AbrirMascotas;
        btnCitas.Click += AbrirCitas;
        btnSalir.Click += Salir;
    }

    void AbrirPropietarios(object sender, EventArgs e)
    {
        new PropietarioForm().ShowDialog();
    }

    void AbrirMascotas(object sender, EventArgs e)
    {
        new MascotaForm().ShowDialog();
    }

    void AbrirCitas(object sender, EventArgs e)
    {
        new CitaForm().ShowDialog();
    }

    void Salir(object sender, EventArgs e)
    {
        Close();
    }
}

public class PropietarioForm : Form
{
    TextBox txtNombre = new TextBox();
    TextBox txtTelefono = new TextBox();
    TextBox txtDireccion = new TextBox();
    Button btnAgregar = new Button();
    Button btnModificar = new Button();
    Button btnEliminar = new Button();
    Button btnLimpiar = new Button();
    DataGridView grid = new DataGridView();
    int idSeleccionado = 0;

    public PropietarioForm()
    {
        Text = "Propietarios";
        Size = new Size(720, 520);
        StartPosition = FormStartPosition.CenterScreen;

        Ayuda.Etiqueta(this, "Nombre", 20, 20);
        Ayuda.Poner(this, txtNombre, 100, 17, 200);
        Ayuda.Etiqueta(this, "Teléfono", 20, 55);
        Ayuda.Poner(this, txtTelefono, 100, 52, 200);
        Ayuda.Etiqueta(this, "Dirección", 20, 90);
        Ayuda.Poner(this, txtDireccion, 100, 87, 200);

        Ayuda.Boton(this, btnAgregar, "Agregar", 330, 15);
        Ayuda.Boton(this, btnModificar, "Modificar", 440, 15);
        Ayuda.Boton(this, btnEliminar, "Eliminar", 330, 50);
        Ayuda.Boton(this, btnLimpiar, "Limpiar", 440, 50);

        Ayuda.ConfigurarGrid(this, grid, 20, 130, 660, 330);

        btnAgregar.Click += Agregar;
        btnModificar.Click += Modificar;
        btnEliminar.Click += Eliminar;
        btnLimpiar.Click += Limpiar;
        grid.CellClick += SeleccionarFila;

        Cargar();
    }

    void Cargar()
    {
        grid.DataSource = PropietarioRepo.Listar();
    }

    void LimpiarCampos()
    {
        txtNombre.Text = "";
        txtTelefono.Text = "";
        txtDireccion.Text = "";
        idSeleccionado = 0;
    }

    void Agregar(object sender, EventArgs e)
    {
        if (txtNombre.Text.Trim() == "")
        {
            MessageBox.Show("Ingrese el nombre");
            return;
        }

        var p = new Propietario();
        p.Nombre = txtNombre.Text.Trim();
        p.Telefono = txtTelefono.Text.Trim();
        p.Direccion = txtDireccion.Text.Trim();

        PropietarioRepo.Agregar(p);
        MessageBox.Show("Propietario agregado");
        LimpiarCampos();
        Cargar();
    }

    void Modificar(object sender, EventArgs e)
    {
        if (idSeleccionado == 0)
        {
            MessageBox.Show("Seleccione un registro");
            return;
        }

        if (txtNombre.Text.Trim() == "")
        {
            MessageBox.Show("Ingrese el nombre");
            return;
        }

        var p = new Propietario();
        p.Id = idSeleccionado;
        p.Nombre = txtNombre.Text.Trim();
        p.Telefono = txtTelefono.Text.Trim();
        p.Direccion = txtDireccion.Text.Trim();

        PropietarioRepo.Modificar(p);
        MessageBox.Show("Propietario modificado");
        LimpiarCampos();
        Cargar();
    }

    void Eliminar(object sender, EventArgs e)
    {
        if (idSeleccionado == 0)
        {
            MessageBox.Show("Seleccione un registro");
            return;
        }

        var respuesta = MessageBox.Show("Se eliminarán también sus mascotas y citas. ¿Desea continuar?", "Confirmar", MessageBoxButtons.YesNo);
        if (respuesta == DialogResult.Yes)
        {
            PropietarioRepo.Eliminar(idSeleccionado);
            MessageBox.Show("Propietario eliminado");
            LimpiarCampos();
            Cargar();
        }
    }

    void Limpiar(object sender, EventArgs e)
    {
        LimpiarCampos();
    }

    void SeleccionarFila(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        var fila = grid.Rows[e.RowIndex];
        idSeleccionado = Convert.ToInt32(fila.Cells["Id"].Value);
        txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
        txtTelefono.Text = fila.Cells["Telefono"].Value.ToString();
        txtDireccion.Text = fila.Cells["Direccion"].Value.ToString();
    }
}

public class MascotaForm : Form
{
    TextBox txtNombre = new TextBox();
    TextBox txtEspecie = new TextBox();
    TextBox txtRaza = new TextBox();
    NumericUpDown numEdad = new NumericUpDown();
    ComboBox cmbPropietario = new ComboBox();
    Button btnAgregar = new Button();
    Button btnModificar = new Button();
    Button btnEliminar = new Button();
    Button btnLimpiar = new Button();
    DataGridView grid = new DataGridView();
    int idSeleccionado = 0;

    public MascotaForm()
    {
        Text = "Mascotas";
        Size = new Size(720, 580);
        StartPosition = FormStartPosition.CenterScreen;

        Ayuda.Etiqueta(this, "Nombre", 20, 20);
        Ayuda.Poner(this, txtNombre, 110, 17, 200);
        Ayuda.Etiqueta(this, "Especie", 20, 55);
        Ayuda.Poner(this, txtEspecie, 110, 52, 200);
        Ayuda.Etiqueta(this, "Raza", 20, 90);
        Ayuda.Poner(this, txtRaza, 110, 87, 200);
        Ayuda.Etiqueta(this, "Edad", 20, 125);
        Ayuda.Poner(this, numEdad, 110, 122, 80);
        Ayuda.Etiqueta(this, "Propietario", 20, 160);
        Ayuda.Poner(this, cmbPropietario, 110, 157, 200);

        cmbPropietario.DropDownStyle = ComboBoxStyle.DropDownList;
        numEdad.Maximum = 50;

        Ayuda.Boton(this, btnAgregar, "Agregar", 350, 15);
        Ayuda.Boton(this, btnModificar, "Modificar", 460, 15);
        Ayuda.Boton(this, btnEliminar, "Eliminar", 350, 50);
        Ayuda.Boton(this, btnLimpiar, "Limpiar", 460, 50);

        Ayuda.ConfigurarGrid(this, grid, 20, 200, 660, 330);

        btnAgregar.Click += Agregar;
        btnModificar.Click += Modificar;
        btnEliminar.Click += Eliminar;
        btnLimpiar.Click += Limpiar;
        grid.CellClick += SeleccionarFila;

        CargarPropietarios();
        Cargar();
    }

    void CargarPropietarios()
    {
        cmbPropietario.DisplayMember = "Nombre";
        cmbPropietario.ValueMember = "Id";
        cmbPropietario.DataSource = PropietarioRepo.Listar();
    }

    void Cargar()
    {
        grid.DataSource = MascotaRepo.Listar();
        grid.Columns["PropietarioId"].Visible = false;
    }

    void LimpiarCampos()
    {
        txtNombre.Text = "";
        txtEspecie.Text = "";
        txtRaza.Text = "";
        numEdad.Value = 0;
        idSeleccionado = 0;
    }

    bool DatosCompletos()
    {
        if (cmbPropietario.SelectedValue == null)
        {
            MessageBox.Show("Registre un propietario primero");
            return false;
        }

        if (txtNombre.Text.Trim() == "" || txtEspecie.Text.Trim() == "")
        {
            MessageBox.Show("Ingrese nombre y especie");
            return false;
        }

        return true;
    }

    void Agregar(object sender, EventArgs e)
    {
        if (!DatosCompletos())
        {
            return;
        }

        var m = new Mascota();
        m.Nombre = txtNombre.Text.Trim();
        m.Especie = txtEspecie.Text.Trim();
        m.Raza = txtRaza.Text.Trim();
        m.Edad = Convert.ToInt32(numEdad.Value);
        m.PropietarioId = Convert.ToInt32(cmbPropietario.SelectedValue);

        MascotaRepo.Agregar(m);
        MessageBox.Show("Mascota agregada");
        LimpiarCampos();
        Cargar();
    }

    void Modificar(object sender, EventArgs e)
    {
        if (idSeleccionado == 0)
        {
            MessageBox.Show("Seleccione un registro");
            return;
        }

        if (!DatosCompletos())
        {
            return;
        }

        var m = new Mascota();
        m.Id = idSeleccionado;
        m.Nombre = txtNombre.Text.Trim();
        m.Especie = txtEspecie.Text.Trim();
        m.Raza = txtRaza.Text.Trim();
        m.Edad = Convert.ToInt32(numEdad.Value);
        m.PropietarioId = Convert.ToInt32(cmbPropietario.SelectedValue);

        MascotaRepo.Modificar(m);
        MessageBox.Show("Mascota modificada");
        LimpiarCampos();
        Cargar();
    }

    void Eliminar(object sender, EventArgs e)
    {
        if (idSeleccionado == 0)
        {
            MessageBox.Show("Seleccione un registro");
            return;
        }

        var respuesta = MessageBox.Show("Se eliminarán también sus citas. ¿Desea continuar?", "Confirmar", MessageBoxButtons.YesNo);
        if (respuesta == DialogResult.Yes)
        {
            MascotaRepo.Eliminar(idSeleccionado);
            MessageBox.Show("Mascota eliminada");
            LimpiarCampos();
            Cargar();
        }
    }

    void Limpiar(object sender, EventArgs e)
    {
        LimpiarCampos();
    }

    void SeleccionarFila(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        var fila = grid.Rows[e.RowIndex];
        idSeleccionado = Convert.ToInt32(fila.Cells["Id"].Value);
        txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
        txtEspecie.Text = fila.Cells["Especie"].Value.ToString();
        txtRaza.Text = fila.Cells["Raza"].Value.ToString();
        numEdad.Value = Convert.ToInt32(fila.Cells["Edad"].Value);
        cmbPropietario.SelectedValue = Convert.ToInt64(fila.Cells["PropietarioId"].Value);
    }
}

public class CitaForm : Form
{
    DateTimePicker dtpFecha = new DateTimePicker();
    TextBox txtMotivo = new TextBox();
    ComboBox cmbMascota = new ComboBox();
    Button btnAgregar = new Button();
    Button btnModificar = new Button();
    Button btnEliminar = new Button();
    Button btnLimpiar = new Button();
    DataGridView grid = new DataGridView();
    int idSeleccionado = 0;

    public CitaForm()
    {
        Text = "Citas";
        Size = new Size(720, 520);
        StartPosition = FormStartPosition.CenterScreen;

        Ayuda.Etiqueta(this, "Mascota", 20, 20);
        Ayuda.Poner(this, cmbMascota, 100, 17, 200);
        Ayuda.Etiqueta(this, "Fecha", 20, 55);
        Ayuda.Poner(this, dtpFecha, 100, 52, 200);
        Ayuda.Etiqueta(this, "Motivo", 20, 90);
        Ayuda.Poner(this, txtMotivo, 100, 87, 200);

        cmbMascota.DropDownStyle = ComboBoxStyle.DropDownList;
        dtpFecha.Format = DateTimePickerFormat.Custom;
        dtpFecha.CustomFormat = "yyyy-MM-dd HH:mm";

        Ayuda.Boton(this, btnAgregar, "Agregar", 330, 15);
        Ayuda.Boton(this, btnModificar, "Modificar", 440, 15);
        Ayuda.Boton(this, btnEliminar, "Eliminar", 330, 50);
        Ayuda.Boton(this, btnLimpiar, "Limpiar", 440, 50);

        Ayuda.ConfigurarGrid(this, grid, 20, 130, 660, 330);

        btnAgregar.Click += Agregar;
        btnModificar.Click += Modificar;
        btnEliminar.Click += Eliminar;
        btnLimpiar.Click += Limpiar;
        grid.CellClick += SeleccionarFila;

        CargarMascotas();
        Cargar();
    }

    void CargarMascotas()
    {
        cmbMascota.DisplayMember = "Nombre";
        cmbMascota.ValueMember = "Id";
        cmbMascota.DataSource = MascotaRepo.Listar();
    }

    void Cargar()
    {
        grid.DataSource = CitaRepo.Listar();
        grid.Columns["MascotaId"].Visible = false;
    }

    void LimpiarCampos()
    {
        txtMotivo.Text = "";
        dtpFecha.Value = DateTime.Now;
        idSeleccionado = 0;
    }

    bool DatosCompletos()
    {
        if (cmbMascota.SelectedValue == null)
        {
            MessageBox.Show("Registre una mascota primero");
            return false;
        }

        if (txtMotivo.Text.Trim() == "")
        {
            MessageBox.Show("Ingrese el motivo");
            return false;
        }

        return true;
    }

    void Agregar(object sender, EventArgs e)
    {
        if (!DatosCompletos())
        {
            return;
        }

        var c = new Cita();
        c.Fecha = dtpFecha.Value.ToString("yyyy-MM-dd HH:mm");
        c.Motivo = txtMotivo.Text.Trim();
        c.MascotaId = Convert.ToInt32(cmbMascota.SelectedValue);

        CitaRepo.Agregar(c);
        MessageBox.Show("Cita agendada");
        LimpiarCampos();
        Cargar();
    }

    void Modificar(object sender, EventArgs e)
    {
        if (idSeleccionado == 0)
        {
            MessageBox.Show("Seleccione un registro");
            return;
        }

        if (!DatosCompletos())
        {
            return;
        }

        var c = new Cita();
        c.Id = idSeleccionado;
        c.Fecha = dtpFecha.Value.ToString("yyyy-MM-dd HH:mm");
        c.Motivo = txtMotivo.Text.Trim();
        c.MascotaId = Convert.ToInt32(cmbMascota.SelectedValue);

        CitaRepo.Modificar(c);
        MessageBox.Show("Cita modificada");
        LimpiarCampos();
        Cargar();
    }

    void Eliminar(object sender, EventArgs e)
    {
        if (idSeleccionado == 0)
        {
            MessageBox.Show("Seleccione un registro");
            return;
        }

        var respuesta = MessageBox.Show("¿Desea eliminar la cita?", "Confirmar", MessageBoxButtons.YesNo);
        if (respuesta == DialogResult.Yes)
        {
            CitaRepo.Eliminar(idSeleccionado);
            MessageBox.Show("Cita eliminada");
            LimpiarCampos();
            Cargar();
        }
    }

    void Limpiar(object sender, EventArgs e)
    {
        LimpiarCampos();
    }

    void SeleccionarFila(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        var fila = grid.Rows[e.RowIndex];
        idSeleccionado = Convert.ToInt32(fila.Cells["Id"].Value);
        dtpFecha.Value = DateTime.Parse(fila.Cells["Fecha"].Value.ToString());
        txtMotivo.Text = fila.Cells["Motivo"].Value.ToString();
        cmbMascota.SelectedValue = Convert.ToInt64(fila.Cells["MascotaId"].Value);
    }
}
