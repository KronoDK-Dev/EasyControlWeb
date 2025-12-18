using EasyControlWeb.Form.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Test
{
   
    public class DosBE 
    {
        string segundo;
        public string Segundo
        {
            get { return segundo; }
            set { segundo = value; }
        }

        string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        //Anular las propiedades del la clase uno
        [Browsable(false)]
        [Bindable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("", true)]
        public new string Primero{get;set;}



        public DosBE() { }
        public DosBE(string _segundo,string _nombre)
        {
            segundo = _segundo;
            nombre = _nombre;
        }
        public string ToString(bool InfoProp)
        {

            return this.Segundo;
        }
    }
}
