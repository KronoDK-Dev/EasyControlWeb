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
    public class EasyITemplateAutoCompletar:EasyFormItemTemplate
    {


     

        public EasyITemplateAutoCompletar() { }
        public EasyITemplateAutoCompletar(EasyFormItemTemplate oEasyFormItemTemplate)
        {
            this.TextField = oEasyFormItemTemplate.TextField;
            this.ValueField = oEasyFormItemTemplate.ValueField;
            this.TemplateType = oEasyFormItemTemplate.TemplateType;
            this.fncTempaleCustom = oEasyFormItemTemplate.fncTempaleCustom;
        }

        public string ToString(bool InfoProp)
        {
            //return this.TextField + EasyUtilitario.Constantes.Caracteres.Espacio + this.ValueField + EasyUtilitario.Constantes.Caracteres.Espacio + this.PathImagenLoanding;
            // return this.TextField + EasyUtilitario.Constantes.Caracteres.Espacio + this.ValueField ;
            return this.ValueField;
        }

        /*Ocultar Caampos*/

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

        #region Ocultar Attr EasyITemplateTextBox
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string Text { get; set; }
        #endregion

    }
}
