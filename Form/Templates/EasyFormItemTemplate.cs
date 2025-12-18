using EasyControlWeb.Filtro;
using EasyControlWeb.InterConecion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using static EasyControlWeb.EasyUtilitario.Constantes;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;
/*
* Ejemplo de Propiedad multicontrols desde un dropdownlist
* http://www.ozcandegirmenci.com/something-about-collectioneditor/
*/
 
namespace EasyControlWeb.Form.Templates
{

    [
        AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("TemplateType"),
        ParseChildren(true, "TemplateType"),
        ToolboxData("<{0}:EasyFormItemTemplate runat=server></{0}:EasyFormItemTemplate")
    ]

    [TypeConverter(typeof(Type_FormItemTemplate))]
    [Serializable]

    public class EasyFormItemTemplate
    {



        public EasyFormItemTemplate() {
            
        }


        private string text;
        [Browsable(true)]
        public string Text { get { return text; } set { text = value; } }

        private int maxLength;
        [Browsable(true)]
        public int MaxLength { get { return maxLength; } set { maxLength = value; } }

        string templateType;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TemplateType { get { return templateType; } set { templateType = value; } }


        #region Atributos en comun [EasyITemplateDatepicker,EasyITemplateNumericBox]
        /* string pathImagenDefault;
             [Browsable(true)]
             [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
             public string PathImagenDefault { get { return pathImagenDefault; } set { pathImagenDefault = value; } }*/
        #endregion

        #region EasyITemplateDatepicker
        private string formatInPut;
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string FormatInPut
        {
            get{return formatInPut; }
            set{ formatInPut = value;}
        }
        private string formatOutPut;
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string FormatOutPut
        {
            get { return formatOutPut; }
            set { formatOutPut = value; }
        }
        #endregion


        #region Attr Clone de EasyITemplateNumericBox
        private int nroDecimales;
            [Browsable(true)]
            public int NroDecimales { get { return nroDecimales; } set { nroDecimales = value; } }
        #endregion



        #region Atributos Comunes para  AutoComplete y Dropdownlist
        private string textField;
        [Browsable(true)]
        [Description("Campo que representa el Texto del item seleccionado"), DefaultValue("")]
        public string TextField
        {
            get { return textField; }
            set { textField = value; }
        }

        private string valueField;
        [Browsable(true)]
        [Description("Campo que representa el Valor del item seleccionado"), DefaultValue("")]
        public string ValueField
        {
            get { return valueField; }
            set { valueField = value; }
        }


        /*Utilizado por EasyListAutocompletar*/
        private string lstValueField;
        [Browsable(true)]
        [Description("Campo que representa el Valor de los items seleccionados"), DefaultValue("")]
        public string LstValueField { get; set; }

        #endregion

        #region Attr AutoComplete
        private int nroCarIni;
            [Browsable(true)]
            [Description("Nro de carácter Ini Find"), DefaultValue("")]
            public int NroCarIni { get { return nroCarIni; } set { nroCarIni=value; } }
        
            [Browsable(true)]
            [Category("Display"), Description("Plantilla base para los items como resultado"), DefaultValue("")]
            public string fncTempaleCustom { get; set; }

        #endregion

        public string GetTipo() {
            string[] strTipo = this.GetType().ToString().Split('.');
            return strTipo[strTipo.GetUpperBound(0)].ToString();
        }

    }
}
