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

using System.Data.SqlTypes;


namespace presentacion
{
    public partial class frmDetalleArticulo : Form
    {
        public frmDetalleArticulo()
        {
            InitializeComponent();
        }

        private Articulos articuloNull = null;

        public frmDetalleArticulo(Articulos articulo)
        {
            InitializeComponent();

            this.articuloNull = articulo;

            Text = "Detalle del Artículo";

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmDetalleArticulo_Load(object sender, EventArgs e)
        {
            MarcasNegocio marcasNegocio = new MarcasNegocio();
            CategoriasNegocio categoriasNegocio = new CategoriasNegocio();

            try
            {
                if (articuloNull != null)
                {
                    txtCodigoDetalle.Text = articuloNull.Codigo;
                    txtNombreDetalle.Text = articuloNull.Nombre;
                    txtDescripcionDetalle.Text = articuloNull.Descripcion;
                    txtPrecioDetalle.Text = articuloNull.Precio.ToString();

                    cargarImagen(articuloNull.ImagenUrl);

                    txtMarcaDetalle.Text = articuloNull.Marca.Descripcion;
                    txtCategoriaDetalle.Text = articuloNull.Categoria.Descripcion;
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
                pbxArticuloDetalle.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxArticuloDetalle.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
        }
    }
}
