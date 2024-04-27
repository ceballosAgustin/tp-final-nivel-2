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

using System.Configuration;
using System.Data.SqlTypes;

namespace presentacion
{
    public partial class frmAltaArticulos : Form
    {
        public frmAltaArticulos()
        {
            InitializeComponent();
        }

        private Articulos articuloNull = null;

        public frmAltaArticulos(Articulos articulo)
        {
            InitializeComponent();

            this.articuloNull = articulo;

            Text = "Modificar Artículo";

        }

        private void frmAltaArticulos_Load(object sender, EventArgs e)
        {
            MarcasNegocio marcasNegocio = new MarcasNegocio();
            CategoriasNegocio categoriasNegocio = new CategoriasNegocio();

            try
            {
                cboMarca.DataSource = marcasNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                cboCategoria.DataSource = categoriasNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if(articuloNull != null)
                {
                    txtCodigo.Text = articuloNull.Codigo;
                    txtNombre.Text = articuloNull.Nombre;
                    txtDescripcion.Text = articuloNull.Descripcion;
                    txtPrecio.Text = articuloNull.Precio.ToString();

                    txtImagenUrl.Text = articuloNull.ImagenUrl;
                    cargarImagen(articuloNull.ImagenUrl);

                    cboMarca.SelectedValue = articuloNull.Marca.Id;
                    cboCategoria.SelectedValue = articuloNull.Categoria.Id;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxArticulo.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            CatalogoNegocio negocio = new CatalogoNegocio();

            try
            {
                if (!validarAgregado())
                {
                    return;
                }
                
                if(articuloNull == null)
                {
                    articuloNull = new Articulos();
                }

                articuloNull.Codigo = txtCodigo.Text;
                articuloNull.Nombre = txtNombre.Text;
                articuloNull.Descripcion = txtDescripcion.Text;
                articuloNull.Precio = decimal.Parse(txtPrecio.Text);
                articuloNull.ImagenUrl = txtImagenUrl.Text;

                articuloNull.Marca = (Marcas)cboMarca.SelectedItem;
                articuloNull.Categoria = (Categorias)cboCategoria.SelectedItem;

                if(articuloNull.Id != 0)
                {
                    negocio.modificar(articuloNull);
                    MessageBox.Show("¡Articulo modificado exitosamente!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    negocio.agregar(articuloNull);
                    MessageBox.Show("¡Articulo agregado exitosamente!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool validarAgregado()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text)
                || string.IsNullOrEmpty(txtNombre.Text)
                    || string.IsNullOrEmpty(txtDescripcion.Text)
                        || string.IsNullOrEmpty(txtPrecio.Text))
            {
                MessageBox.Show("Complete los campos faltantes para cargar el Artículo...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!(soloNumeros(txtPrecio.Text)))
            {
                MessageBox.Show("Ingrese solo números en Precio para cargar el Artículo...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool soloNumeros(string cadena)
        {
            int contadorComas = 0;
            
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)) && caracter != ',')
                {
                    return false;
                }

                if(caracter == ',')
                {
                    contadorComas++;
                }
            }

            return contadorComas <= 1;
        }

    }
}
