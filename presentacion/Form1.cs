using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using dominio;
using negocio;

namespace presentacion
{
    public partial class frmCatalogo : Form
    {
        private List<Articulos> listaArticulos;

        public frmCatalogo()
        {
            InitializeComponent();
        }

        private void frmCatalogo_Load(object sender, EventArgs e)
        {
            cargar();

            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Precio");
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;

                cargarImagen(seleccionado.ImagenUrl);
            }
        }

        private void cargar()
        {
            CatalogoNegocio negocio = new CatalogoNegocio();

            try
            {
                listaArticulos = negocio.listar();

                dgvArticulos.DataSource = listaArticulos;

                ocultarColumnas();

                cargarImagen(listaArticulos[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulos.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxArticulos.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulos alta = new frmAltaArticulos();

            alta.ShowDialog();

            cargar();

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Si la Celda seleccionada es igual a 0, o sea que no hay ninguna seleccionada
            if (dgvArticulos.SelectedCells.Count == 0)
            {
                // Mensaje de aviso
                MessageBox.Show("Por favor, seleccione un artículo para modificar.", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Que muestre la lista completa
                dgvArticulos.DataSource = listaArticulos;

                return;
            }

            Articulos seleccionado;

            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;

            frmAltaArticulos modificar = new frmAltaArticulos(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void eliminar()
        {
            if (dgvArticulos.SelectedCells.Count == 0)
            {
                MessageBox.Show("No hay ningún Artículo seleccionado para eliminar.", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvArticulos.DataSource = listaArticulos;
                return;
            }


            CatalogoNegocio negocio = new CatalogoNegocio();

            Articulos seleccionado;

            try
            {
                DialogResult respuesta = MessageBox.Show("¿Estás seguro de eliminar el Artículo?", "Eliminar Artículo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;

                    negocio.eliminar(seleccionado.Id);

                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulos> listaFiltrada;

            string filtro = txtFiltro.Text;

            if (filtro.Length >= 2)
            {
                listaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper())
                    || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper())
                        || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }

            dgvArticulos.DataSource = null;

            dgvArticulos.DataSource = listaFiltrada;

            ocultarColumnas();
        }
        
        private bool validarFiltro()
        {
            if(cboCampo.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione el campo por el que quiera filtrar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            if(cboCriterio.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione el criterio por el que quiera filtrar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

        // -------------------------------------------------------------------------------------------

            if(cboCampo.SelectedItem.ToString() == "Nombre")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Ingrese el texto por el que quiera filtrar...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }

            if(cboCampo.SelectedItem.ToString() == "Marca")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Ingrese el texto por el que quiera filtrar...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
  
            if(cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Ingrese un número en el filtro númerico...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }

                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Ingrese solo números para filtrar en el campo númerico...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }

            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }

            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            CatalogoNegocio negocio = new CatalogoNegocio();

            try
            {
                if (validarFiltro())
                {
                    return;
                }

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            // Si es de tipo númerico
            if(opcion == "Precio")
            {
                cboCriterio.Items.Clear();

                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            // Si no, entonces tipo texto
            else 
            {
                cboCriterio.Items.Clear();

                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            if(dgvArticulos.SelectedCells.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un artículo para ver en detalle.", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvArticulos.DataSource = listaArticulos;
                return;
            }

            Articulos seleccionado;

            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;

            frmDetalleArticulo detalle = new frmDetalleArticulo(seleccionado);
            detalle.ShowDialog();
            cargar();
            
        }

        private void btnTraerArticulos_Click(object sender, EventArgs e)
        {
            dgvArticulos.DataSource = listaArticulos;

            ocultarColumnas();
        }

    }
}
