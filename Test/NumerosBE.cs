using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Test
{
    [TypeConverter(typeof(DataConfig_TypeC))]
    [Serializable]
    public class NumerosBE
    {
        string nombre;
        public string Nombre {
            get { return nombre; }
            set { nombre = value; }
        }
        public NumerosBE() { }
        public NumerosBE(string _nombre) {
            nombre = _nombre;
        }

    }
}
