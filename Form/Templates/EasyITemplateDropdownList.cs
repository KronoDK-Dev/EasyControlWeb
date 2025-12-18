using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Templates
{
    [TypeConverter(typeof(Type_FormItemTemplate))]
    [Serializable]
    public class EasyITemplateDropdownList:EasyFormItemTemplate
    {

     
        public EasyITemplateDropdownList() { }
        public EasyITemplateDropdownList(EasyFormItemTemplate oEasyFormItemTemplate)
        {
            this.TextField = oEasyFormItemTemplate.TextField;
            this.ValueField = oEasyFormItemTemplate.ValueField;
            this.TemplateType = oEasyFormItemTemplate.TemplateType;
        }

        public string ToString(bool InfoProp)
        {
            return this.TextField + EasyUtilitario.Constantes.Caracteres.Espacio + this.ValueField;
        }



        /*Ocultar Otros Atributos*/

        /*******************************************************HIDDEN*************************************************/

        #region Ocultar Attr EasyITemplateDatePick and EasyITemplateNumericBox
            [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
            public new string FormatOutPut { get; set; }

            [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
            public new string FormatInPut { get; set; }
        #endregion


        #region Ocultar Attr EasyITemplateNumericBox
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new int NroDecimales { get; set; }
        #endregion


        #region Attr AutoComlete
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string PathImagenLoanding { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new int NroCarIni { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string fncTempaleCustom { get; set; }

        #endregion

        #region Ocultar Attr EasyITemplateTextBox
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string Text { get; set; }
        #endregion
    }
}
