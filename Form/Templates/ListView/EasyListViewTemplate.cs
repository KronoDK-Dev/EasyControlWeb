using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Templates.ListView
{
    [
          AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
          AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
          DefaultProperty("TemplateType"),
          ParseChildren(true, "TemplateType"),
          ToolboxData("<{0}:EasyListViewTemplate runat=server></{0}:EasyListViewTemplate")
      ]

    [TypeConverter(typeof(Type_LisItemTemplate))]
    [Serializable]

    public class EasyListViewTemplate
    {
        string templateType;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TemplateType { get { return templateType; } set { templateType = value; } }



        #region Attr ListviewItemImg
        [Browsable(true)]
        [Description("Ruta de la imagen"), DefaultValue("")]
        private string src;
        public string Src { get { return src; } set { src = value; } }


        private string url;
        [Browsable(true)]
        [Description("Ruta de la pagina vinculante"), DefaultValue("")]
        public string Url { get { return url; } set { url = value; } }

        private string _fncTempaleCustom;
        [Browsable(true)]
        [Category("Display"), Description("Plantilla base para los items como resultado"), DefaultValue("")]
        public string fncTempaleCustom { get { return _fncTempaleCustom; } set { _fncTempaleCustom = value; } }

        public string key;
        [Browsable(true)]
        [Description(""), DefaultValue("")]
        public string Key { get { return key; } set { key = value; } }

        public string text;
        [Browsable(true)]
        [Description(""), DefaultValue("")]
        public string Text { get { return text; } set { text = value; } }


        private string _value;
        [Browsable(true)]
        [Description(""), DefaultValue("")]
        public string Value { get { return _value; } set { _value = value; } }

        private int idEstado;
        [Browsable(true)]
        [Description(""), DefaultValue("")]
        public int IdEstado { get { return idEstado; } set { idEstado = value; } }


        #endregion
        public EasyListViewTemplate() { }
        
        public EasyListViewTemplate(string _url,string _key,string _text,string _value,int _idEstado) {
            this.Url = _url;
            this.Key = _key;
            this.Text = _text;
            this.Value = _value;
            this.IdEstado = _idEstado;
        }

    }
}
