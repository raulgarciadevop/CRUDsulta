using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEscuela
{
    public partial class Form1 : Form
    {
        private bool IsNuevo = false;
        private bool IsEditar = false;
        EscuelaEntities bd;

        public Form1()
        {
            InitializeComponent();
        }
    
        private void Form1_Load(object sender, EventArgs e)
        {
            Habilitar(false);
            botones();
            Limpiar();
            ListarAlumnos();
        }

        private void Habilitar(bool valor)
        {
          
                txtNombre.ReadOnly = !valor;
                txtApeP.ReadOnly = !valor;
                txtAPM.ReadOnly = !valor;
                numEdad.ReadOnly = !valor;
           
           
        }

        private void Limpiar()
        {
            txtID.Clear();
            txtNombre.Clear();
            txtApeP.Clear();
            txtAPM.Clear();
            numEdad.Value = 0;
        }

        private void botones()
        {
            if (IsNuevo|| IsEditar)
            {
                btnNuevo.Enabled = false;
                btnEditar.Enabled = false;
                btnGuardar.Enabled = true;
                btnCancelar.Enabled = true;
            }
            else
            {
                btnNuevo.Enabled = true;
                btnEditar.Enabled = false;
                btnGuardar.Enabled = false;
                btnCancelar.Enabled = false;
            }
           
        }

        private void ListarAlumnos()
        {
            using(bd = new EscuelaEntities())
            {
                List<T_Alumnos> Lalumnos = new List<T_Alumnos>();
                Lalumnos = bd.T_Alumnos.ToList();
                gridAlumnos.DataSource = null;
                gridAlumnos.DataSource = Lalumnos;
            }
        }

        private void BuscarAlumno(string TexoBuscar)
       {
            using (bd = new EscuelaEntities())
            {
                List<T_Alumnos> Lalumnos = new List<T_Alumnos>();
                //Lalumnos = bd.T_Alumnos.Where(x => x.Nombre.Contains(TexoBuscar) || x.ApellidoPaterno.Contains(TexoBuscar) || x.ApellidoMaterno.Contains(TexoBuscar)).ToList();
                //gridAlumnos.DataSource = null;
                //gridAlumnos.DataSource = Lalumnos;
                var Result = from A in bd.T_Alumnos
                             where A.Nombre.Contains(TexoBuscar)
                             select A;
                Lalumnos = Result.ToList();
                gridAlumnos.DataSource = null;
                gridAlumnos.DataSource = Lalumnos;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {

            IsNuevo = true;
            IsEditar = false;
            Habilitar(true);
            botones();
            Limpiar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
          
            IsNuevo = false;
            IsEditar = true;
            Habilitar(true);
            botones();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            IsNuevo = false;
            IsEditar = false;
            botones();
            Habilitar(false);
            Limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsNuevo)
                {
                    if (txtNombre.Text == "")
                    {
                        MessageBox.Show("El nombre es requerido");

                    }
                    else
                    {
                        using(bd= new EscuelaEntities())
                        {
                            T_Alumnos Alumno = new T_Alumnos();
                            Alumno.Nombre = txtNombre.Text.ToUpper().Trim();
                            Alumno.ApellidoPaterno = txtApeP.Text.ToUpper().Trim();
                            Alumno.ApellidoMaterno = txtAPM.Text.ToUpper().Trim();
                            Alumno.edad =(int)numEdad.Value;

                            bd.T_Alumnos.Add(Alumno);
                            int rpta = 0;
                            rpta = bd.SaveChanges();
                            if (rpta != 0)
                            {
                                MessageBox.Show("Alumno registrado");
                                Limpiar();
                                IsNuevo = false;
                                IsEditar = false;
                                botones();
                                Habilitar(false);
                                ListarAlumnos();
                            }
                        }
                    }

                }
                else
                {
                    using (bd = new EscuelaEntities())
                    {
                        T_Alumnos Alumno = new T_Alumnos();
                        int id = 0;
                        id = Convert.ToInt32(txtID.Text);
                        Alumno = bd.T_Alumnos.Where(x => x.idEstudiante == id).FirstOrDefault();
                        Alumno.Nombre = txtNombre.Text.ToUpper().Trim();
                        Alumno.ApellidoPaterno = txtApeP.Text.ToUpper().Trim();
                        Alumno.ApellidoMaterno = txtAPM.Text.ToUpper().Trim();
                        Alumno.edad = (int)numEdad.Value;

                       
                        int rpta = 0;
                        rpta = bd.SaveChanges();
                        if (rpta != 0)
                        {
                            MessageBox.Show("Alumno editado");
                            Limpiar();
                            IsNuevo = false;
                            IsEditar = false;
                            botones();
                            Habilitar(false);
                            ListarAlumnos();
                        }
                    }
                }

            }catch(Exception ex)
            {

            }
        }

        private void gridAlumnos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = 0;
            id = Convert.ToInt32(gridAlumnos.CurrentRow.Cells[0].Value);
            using (bd = new EscuelaEntities())
            {
                T_Alumnos alumno = new T_Alumnos();
                alumno = bd.T_Alumnos.Where(x => x.idEstudiante == id).FirstOrDefault();
                txtID.Text = alumno.idEstudiante.ToString();
                txtNombre.Text = alumno.Nombre;
                txtApeP.Text = alumno.ApellidoPaterno;
                txtAPM.Text = alumno.ApellidoMaterno;
                numEdad.Value = Convert.ToDecimal(alumno.edad);
            }
            btnEditar.Enabled = true;

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarAlumno(txtBuscar.Text);
        }
    }
}
