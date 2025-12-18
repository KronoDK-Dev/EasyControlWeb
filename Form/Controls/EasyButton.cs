using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace EasyControlWeb.Form.Controls
{ 
    public class EasyButton
    {

        public EasyButton() : this(String.Empty, String.Empty, String.Empty, String.Empty, false, EasyUtilitario.Enumerados.Ubicacion.Centro) { }
        public EasyButton(string _Id, string _Texto, string _Descripcion, string _Icono, bool _RunAtServer,  EasyUtilitario.Enumerados.Ubicacion _Ubicacion)
        {
            this.Id = _Id;
            this.Texto = _Texto;
            this.Descripcion = _Descripcion;
            this.Icono = _Icono;
            this.RunAtServer = _RunAtServer;
            this.Ubicacion = _Ubicacion;
        }

        public string Id { get; set; }
        public string Texto { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; }
        public bool RunAtServer { get; set; }
        public string ClassName { get; set; }
        public EasyUtilitario.Enumerados.Ubicacion Ubicacion { get; set; }
       

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

        public HtmlGenericControl Materialized(string _ClassName, string TextColor)
        {
                HtmlGenericControl btnItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a", ((this.ClassName!=null&&(this.ClassName.Length>0))?this.ClassName: _ClassName));
                btnItem.Attributes.Add("href", "#");
                HtmlGenericControl _I = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("i", this.Icono);
                _I.Style.Add("color", TextColor);
                _I.Style.Add("margin-right", "2px");
                btnItem.Controls.Add(_I);
                if ((this.Texto != null))
                {
                    btnItem.Controls.Add(new LiteralControl("<span style='text-transform:capitalize; font-size:12px'>" + this.Texto + "</span>"));
                }
                btnItem.Style.Add("margin-top", "5px");
                btnItem.Style.Add("margin-right", "5px");
                return btnItem;
           
        }
    }

   
}
