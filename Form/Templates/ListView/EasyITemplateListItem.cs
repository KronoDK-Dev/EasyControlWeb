using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Web.UI;
using System.Text;
using System.Web;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;
using EasyControlWeb.Form.Templates.ListView;
namespace EasyControlWeb.Form.Templates.ListView
{


    [TypeConverter(typeof(Type_LisItemTemplate))]
    [Serializable]

    public class EasyITemplateListItem: EasyListViewTemplate
    {
        public EasyITemplateListItem() { }
        public EasyITemplateListItem(EasyListViewTemplate oEasyListViewTemplate)
        {
            this.TemplateType = oEasyListViewTemplate.TemplateType;
            this.Url = oEasyListViewTemplate.Url;
            this.fncTempaleCustom = oEasyListViewTemplate.fncTempaleCustom;
            this.Key = oEasyListViewTemplate.Key;
            this.Text = oEasyListViewTemplate.Text;
            this.Value = oEasyListViewTemplate.Value;
            this.IdEstado = oEasyListViewTemplate.IdEstado;

    }

    #region Ocultar Attr ListItemImg
    [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string Src { get; set; }
        #endregion


    }
}
