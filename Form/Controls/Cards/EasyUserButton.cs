using EasyControlWeb.Form.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyControlWeb.Form.Controls.Cards
{
    public  class EasyUserButton: EasyButtonBase
    {
        public string Login { get; set; }
        public string ApellidosyNombres { get; set; }
        public string Especialidad { get; set; }

        public string PathImagen { get; set; }
    }
}
