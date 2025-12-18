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
    /*EasyITemplateNumericBox*/
    /***********************************************************************************************************/
    [TypeConverter(typeof(Type_FormItemTemplate))]
    [Serializable]
    public class EasyITemplateNumericBox: EasyFormItemTemplate
    {
        public EasyITemplateNumericBox() { }

        public EasyITemplateNumericBox(EasyFormItemTemplate oEasyFormItemTemplate) {
            //this.PathImagenDefault = oEasyFormItemTemplate.PathImagenDefault;
            this.NroDecimales = oEasyFormItemTemplate.NroDecimales;
            this.TemplateType = oEasyFormItemTemplate.TemplateType;
        }


        

            #region Seccion para ocultar atributos que no se deben de utilizar por este tipo

            #endregion

            public string ToString(bool InfoProp)
            {
                //return  this.NroDecimales.ToString() + EasyUtilitario.Constantes.Caracteres.Espacio +  this.PathImagenDefault;
                return this.NroDecimales.ToString() ;
            }

        #region Ocultar Attr EasyITemplateDatePick and EasyITemplateNumericBox
            [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
            public new string FormatOutPut { get; set; }

            [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
            public new string FormatInPut { get; set; }
        #endregion

        #region Attr AutoComlete
        /*[Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
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

        #region Ocultar Attr EasyITemplateTextBox
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string Text { get; set; }
        #endregion
    }

}
