using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using EasyControlWeb.Filtro;
using EasyControlWeb.InterConecion;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Templates
{
    /***********************************************************************************************************/
    /*EasyITemplateDatepicker*/ /*https://www.codeproject.com/Articles/5372/How-to-Edit-and-Persist-Collections-with-Collectio*/
    /***********************************************************************************************************/
    [TypeConverter(typeof(Type_FormItemTemplate))]
    [Serializable]
    public class EasyITemplateDatepicker: EasyFormItemTemplate
    { 
        public EasyITemplateDatepicker() { }
        public EasyITemplateDatepicker(EasyFormItemTemplate oEasyFormItemTemplate) {
            this.FormatOutPut = oEasyFormItemTemplate.FormatOutPut;
            this.FormatInPut = oEasyFormItemTemplate.FormatInPut;
            this.TemplateType = oEasyFormItemTemplate.TemplateType;
        }

        public  string ToString(bool InfoProp)
        {
            return "";//this.PathImagenDefault;
        }

        /*******************************************************HIDDEN*************************************************/
        #region Ocultar Attr EasyITemplateNumericBox
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new int NroDecimales { get; set; }
        #endregion


        #region Attr AutoComlete
       /* [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string PathImagenLoanding { get; set; }*/

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string ValueField { get; set; }
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string TextField { get; set; }
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new int NroCarIni { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string fncTempaleCustom { get; set; }

        #endregion

        /*******************************************************HIDDEN*************************************************/
        #region Ocultar Attr EasyITemplateTextBox
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string Text { get; set; }
        #endregion
    }


}
