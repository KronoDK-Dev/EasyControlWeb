using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using static EasyControlWeb.EasyUtilitario.Enumerados;

namespace EasyControlWeb.Filtro
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class EasyFiltroItem : EventArgs
    {
        private string id;
        private string idPadre;
        private string operador;
        private string campo;
        private string campoDescripcion;
        private EasyUtilitario.Enumerados.TiposdeDatos tipodeDatos;
        private string criterio;
        private string criterioDescripcion;
        private string valueField;
        private string textField;
        private int nroHijos;
        private string oTemplateType;
        private bool definitivo;

        public EasyFiltroItem()
            : this(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, 0)
        {
        }

        public EasyFiltroItem(string Id, string IdPadre, string Operador, string Campo, string Criterio, string _ValueField, string _TextField, int NroHijos)
        {
            id = Id;
            idPadre = IdPadre;
            operador = Operador;
            campo = Campo;
            criterio = Criterio;
            TextField = _TextField;
            ValueField = _ValueField;
            nroHijos = NroHijos;
            TipodeDatos = tipodeDatos;
        }

        public override string ToString()
        {
            return "EasyFiltroItem";
        }

        public string ToString(bool outClienteBE)
        {
            // string cmll = "\"";
            string cmll = "'";
            string strBaseBE = "{"
                               + "Id" + ":" + cmll + this.Id + cmll + ","
                               + "IdPadre" + ":" + cmll + this.IdPadre + cmll + ","
                               + "Operador" + ":" + cmll + this.Operador + cmll + ","
                               + "Campo" + ":" + cmll + this.Campo + cmll + ","
                               + "CampoDescripcion" + ":" + cmll + this.CampoDescripcion + cmll + ","
                               + "Criterio" + ":" + cmll + this.Criterio + cmll + ","
                               + "CriterioDescripcion" + ":" + cmll + this.CriterioDescripcion + cmll + ","
                               + "ValueField" + ":" + cmll + this.ValueField + cmll + ","
                               + "TextField" + ":" + cmll + this.TextField + cmll + ","
                               + "NroHijos" + ":" + cmll + this.NroHijos + cmll + "}";

            return strBaseBE.ToString();
        }

        public enum btnTipo
        { //Utilizdo para saber si el filtr es principal o secundarios
            Valor,
            Campo,
            CampoyValor,
        }

        public HtmlGenericControl Render(btnTipo obtnTipo, string ClientIDGestor, string MetodoDelete)
        {
            string IdFilter = "BtnFilter_" + this.id;
            HtmlGenericControl btnFiltItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "form-multi-select-tag");
            btnFiltItem.Attributes["id"] = IdFilter;
            btnFiltItem.Attributes.Add("data-value", "1");


            string Criterio = "  <span  style = 'color: #0000FF'>" + this.CriterioDescripcion + "</span>  ";
            string CampoDesc = "<span  style = 'font-weight: bold;'> " + this.CampoDescripcion + " </span>";
            if (obtnTipo == btnTipo.Campo)
            {
                btnFiltItem.Controls.Add(new LiteralControl(CampoDesc + " "));
            }
            else if (obtnTipo == btnTipo.Valor)
            {
                btnFiltItem.Controls.Add(new LiteralControl(Criterio + " " + this.TextField + " "));
            }
            else if (obtnTipo == btnTipo.CampoyValor)
            {
                //TextBox txtIdDelet =  (TextBox)((System.Web.UI.Page)HttpContext.Current.Handler).FindControl(btnFiltItem.ClientID);
                HtmlGenericControl PItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("p");
                PItem.Controls.Add(new LiteralControl(CampoDesc + Criterio + this.TextField + " "));

                PItem.Attributes.CssStyle.Add("display", "inline");
                PItem.Attributes.CssStyle.Add("margin-right", "5px");
                PItem.Attributes.CssStyle.Add("margin-left", "10px");
                PItem.Attributes.CssStyle.Add("cursor", "pointer");
                var ScriptItemApply = @"var itemDFiltro = " + this.ToString(true) + ";" + ClientIDGestor + @"ApplyItemSelect(itemDFiltro);";
                //PItem.Attributes.Add("onclick", ScriptItemApply);
                PItem.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = ScriptItemApply;
                btnFiltItem.Controls.Add(PItem);
                //btnFiltItem.Controls.Add(new LiteralControl(CampoDesc + Criterio + this.TextField + " "));

            }

            HtmlGenericControl btnCmd = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("button", "form-multi-select-tag-delete text-medium-emphasis");
            btnCmd.Attributes.Add("aria-label", "Close");
            btnCmd.Attributes.Add("type", "button");
            btnCmd.Attributes.CssStyle.Add("cursor", "pointer");


            HtmlGenericControl btnDel = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span");
            HtmlImage oimg = EasyUtilitario.Helper.HtmlControlsDesign.CrearImagen(EasyUtilitario.Constantes.ImgDataURL.IconDeItem);
            oimg.Attributes.Add("aria-hidden", "true");
            var ScriptItemFiltroData = @"javascript: var itemDFiltro = " + this.ToString(true) + ";" + MetodoDelete + @"(itemDFiltro);";
            string onSeleted = ScriptItemFiltroData;
            oimg.Attributes.Add("onclick", onSeleted);
            btnDel.Controls.Add(oimg);


            btnCmd.Controls.Add(btnDel);
            btnFiltItem.Controls.Add(btnCmd);
            return btnFiltItem;
        }

        [
            Category("Behavior"),
            DefaultValue(""),
            Description("Valor Generado al momento de ser elaborado"),
            NotifyParentProperty(true),
        ]
        public string Id { get { return id; } set { this.id = value; } }

        [
            Category("Behavior"), DefaultValue(""),
            Description("Id del iten de Criterio padre"),
            NotifyParentProperty(true),
        ]
        public string IdPadre { get { return idPadre; } set { this.idPadre = value; } }

        [
            Category("Behavior"), DefaultValue(""),
            Description("Valor del Operador Logico AND o OR"),
            NotifyParentProperty(true),
        ]
        public string Operador { get { return operador; } set { this.operador = value; } }

        [
            Category("Behavior"), DefaultValue(""),
            Description("Nombre de Campos al que se le aplica el criterio"),
            NotifyParentProperty(true),
        ]
        public string Campo { get { return campo; } set { this.campo = value; } }

        [
            Category("Behavior"), DefaultValue(""),
            Description("Descripcionde Nombre de Campos al que se le aplica el criterio"),
            NotifyParentProperty(true),
        ]
        public string CampoDescripcion { get { return campoDescripcion; } set { this.campoDescripcion = value; } }

        [
            Category("Behavior"), DefaultValue(""),
            Description("Tipo de dato del campo"),
            NotifyParentProperty(true),
        ]
        public EasyUtilitario.Enumerados.TiposdeDatos TipodeDatos { get { return tipodeDatos; } set { this.tipodeDatos = value; } }

        [
            Category("Behavior"), DefaultValue(""),
            Description("Tipo de criterio a aplicar"),
            NotifyParentProperty(true),
        ]
        public string Criterio { get { return criterio; } set { this.criterio = value; } }

        [
            Category("Behavior"), DefaultValue(""),
            Description("Descripcion del Tipo de criterio a aplicar"),
            NotifyParentProperty(true),
        ]
        public string CriterioDescripcion { get { return criterioDescripcion; } set { this.criterioDescripcion = value; } }

        [
            Category("Behavior"), DefaultValue(""),
            Description("Nro de Hijos que desprenden del campo principal"),
            NotifyParentProperty(true),
        ]
        public int NroHijos { get { return nroHijos; } set { this.nroHijos = value; } }

        [
            Category("Behavior"), DefaultValue(""),
            Description("Valor"),
            NotifyParentProperty(true),
        ]
        public string ValueField { get { return valueField; } set { valueField = value; } }

        [
            Category("Behavior"), DefaultValue(""),
            Description("descripcion del Valor"),
            NotifyParentProperty(true),
        ]
        public string TextField { get { return textField; } set { textField = value; } }

        public string TemplateType
        {
            get { return oTemplateType; }
            set { oTemplateType = value; }
        }

        public bool Definitivo { get { return definitivo; } set { definitivo = value; } }
    }
}
