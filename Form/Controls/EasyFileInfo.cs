using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Form.Controls
{
    [Serializable]
    public class EasyFileInfo 
    {

        public EasyFileInfo() : this(String.Empty, String.Empty, String.Empty, String.Empty,false) { }
        public EasyFileInfo(string _IdFile, string _Nombre,string _Tipo,string _Sise,bool _Temporal)
        {
            this.IdFile = _IdFile;
            this.Nombre = _Nombre;
            this.Tipo = _Tipo;
            this.Size = _Sise;
            this.Temporal = _Temporal;
            this.IdEstado = 1;
            this.Enviado = false;
            this.Existe = false;
        }

        public EasyFileInfo(string structScriptBE)
        {
            string []arrData = structScriptBE.Replace("{","").Replace("}","").Split(',');
            this.IdFile = arrData[1];
            this.Nombre = arrData[0];
        }


        public string IdFile { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Size { get; set; }
        public bool Temporal { get; set; }
        public int IdEstado { get; set; }
        public bool Enviado { get; set; }
        public bool Existe { get; set; }
        public string ToString(bool ver)
        {

            string Structura = "";
            foreach (var propertyInfo in this.GetType().GetProperties())
            {
                Structura += propertyInfo.Name + ":" + EasyUtilitario.Constantes.Caracteres.ComillaDoble + propertyInfo.GetValue(this, propertyInfo.GetIndexParameters()) + EasyUtilitario.Constantes.Caracteres.ComillaDoble + EasyUtilitario.Constantes.Caracteres.Coma;
            }
            Structura = Structura.Substring(0, Structura.Length - 1);
            return "{" + Structura + "}";
        }

      
    }

}
