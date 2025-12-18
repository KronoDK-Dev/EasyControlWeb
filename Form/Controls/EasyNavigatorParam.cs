using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Form.Controls
{
    [Serializable]
    public  class EasyNavigatorParam
    {
        [Category("Params")]
        [Browsable(true)]
        [Description("Nombre de parámetro")] 
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Nombre { get; set; }

        [Category("Params")]
        [Browsable(true)]
        [Description("Valor de parámetro")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Valor { get; set; }


        public EasyNavigatorParam() : this(string.Empty, string.Empty) { }
        public EasyNavigatorParam(string _Nombre, string _Valor)
        {
            this.Nombre = _Nombre;
            this.Valor = _Valor;
        }
        public override string ToString()
        {
            return this.Nombre + EasyUtilitario.Constantes.Caracteres.SignoIgual + this.Valor;
        }

    }
}
