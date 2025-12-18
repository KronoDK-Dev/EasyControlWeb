using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyControlWeb.Form.Templates
{
    public  class EasyITemplateListAutocompletar: EasyFormItemTemplate
    { 
        public EasyITemplateListAutocompletar() { }
        public EasyITemplateListAutocompletar(EasyFormItemTemplate oEasyFormItemTemplate)
        {
            this.TextField = oEasyFormItemTemplate.TextField;
            this.ValueField = oEasyFormItemTemplate.ValueField;
            this.LstValueField = oEasyFormItemTemplate.ValueField;
            this.TemplateType = oEasyFormItemTemplate.TemplateType;
            this.fncTempaleCustom = oEasyFormItemTemplate.fncTempaleCustom;
        }

        public string ToString(bool InfoProp)
        {
            return this.LstValueField;
        }


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
