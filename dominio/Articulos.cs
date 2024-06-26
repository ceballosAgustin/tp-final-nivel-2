﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Articulos
    {
        public int Id { get; set; }

        [DisplayName("Código")]
        public string Codigo { get; set; }

        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        public string ImagenUrl { get; set; }

        public decimal Precio { get; set; }

        // Descripción de la clase Marcas
        public Marcas Marca { get; set; }

        // Descripción de la clase Categorias
        [DisplayName("Categoría")]
        public Categorias Categoria { get; set; }

    }
}
