using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Templates.ListView
{


    [TypeConverter(typeof(Type_LisItemTemplate))]
    [Serializable]
    public class EasyITemplateListItemImg: EasyListViewTemplate
    {
        
        public EasyITemplateListItemImg() { }
        public EasyITemplateListItemImg(EasyListViewTemplate oEasyListViewTemplate)
        {
            this.TemplateType = oEasyListViewTemplate.TemplateType;
            this.Src = oEasyListViewTemplate.Src;
            this.fncTempaleCustom = oEasyListViewTemplate.fncTempaleCustom;
            this.Key = oEasyListViewTemplate.Key;
            this.Text = oEasyListViewTemplate.Text;
            this.Value = oEasyListViewTemplate.Value;
            this.IdEstado = oEasyListViewTemplate.IdEstado;
        }

        #region Ocultar Attr ListItem
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string  Url { get; set; }
        #endregion

    }
}
