using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyControlWeb.Form.Base
{
    public enum EasyButtonType{
        button,
        Separador

    }
    public  class EasyButtonBase
    {
        public EasyButtonBase() { }
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Icono { get; set; }
        public EasyButtonType TipoBoton { get; set; }
    }
}
