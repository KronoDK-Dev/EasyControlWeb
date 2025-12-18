using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Security.Permissions;
using System.ComponentModel.DataAnnotations;
using System.Web.UI;
using System.Drawing.Design;
using EasyControlWeb.Filtro;
using System.Collections.Generic;
using EasyControlWeb.Form.Controls;
using EasyControlWeb.Form.Templates;
using System.Linq;
using EasyControlWeb.Test;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;
using EasyControlWeb.Form.Estilo;

namespace EasyControlWeb.Form
{  
    [
       AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
       AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
       DefaultProperty("Etiqueta"),
       ParseChildren(true, "Etiqueta"),
       ToolboxData("<{0}:EasyFomItem runat=server></{0}:EasyFomItem")
   ]
    [Serializable]
    public class EasyFomItem
    {
        #region Variables
            string etiqueta;
        #endregion
        public enum TipoControl {
            TextBox,
            TextBoxMultiline,
            NumericBox,
            Email,
            Password,
            DDLlist,
            TextBoxFind,
            Datepicker,
            GridVIew,
            AutoComplete,
            UpLoad,
        }
      

        const string ETQ_CLASS_TEXTBOX = "ClassTextBox";
        const string ETQ_CLASS_TEXTBOX_LABEL = "ClassTextBoxLabel";

       
        [Display(Name = "Id")]
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Category("Identificacion"), Description("Identificacion del control"), DefaultValue("")]
        public string Id { get; set; }


        [Category("Identificacion"), Description("Identificacion del control"), DefaultValue("")]
        [Display(Name = "Etiqueta")]
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Etiqueta {
            get
            {
                if (etiqueta == null)
                {
                    etiqueta = " ";
                }
                return etiqueta;
            }
            set
            {
                etiqueta = value;
            }
        }
       
        [Category("Identificacion"), Description("Identificacion del control"), DefaultValue("")]
        //public string SubEtiqueta { get; set; }
        public string Placeholder { get; set; }

        [Category("Lectura y Escritura de data"), Description("Lectura de datos"), DefaultValue("")]
        public string Valor { get; set; }

        [Category("Validacion"), Description("Validaciones de campos"), DefaultValue("")]
        public bool Requerido { get; set; }
        [Category("Validacion"), Description("Validaciones de campos"), DefaultValue("")]
        public string MsgValidacion { get; set; }
        [Category("Validacion"), Description("Validaciones de campos"), DefaultValue("")]
        public bool Readonly { get; set; }//class="form-control-plaintext" 

     
        [Category("ACCION"), Description("Inica si el control luego de editado realiza un PostBack"), DefaultValue(false)]
        public bool AutoPostBack { get; set; }

        
        #region para el tipo de autocompletar
        /*
        [Category("ZAutoComplete"), Description("Imagen icono representativo del control"), DefaultValue("http://")]
            [Browsable(true)]
            [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
            public string PathImagenDefault { get; set; }
            [Category("ZAutoComplete"), Description("Imagen gif que indica progresso de búsqueda"), DefaultValue("http://")]
            [Browsable(true)]
            [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
            public string PathImagenLoanding { get; set; }

            [Category("ZAutoComplete-Datos"), Description("Url del web serivice que sirve para obtener la información"), DefaultValue("http://")]
            public string UrlWebServiceMetodo { get; set; }

            [Category("ZAutoComplete-Datos"), Description("Campo que representa el Valor del item seleccionado"), DefaultValue("")]
            public string FieldValue { get; set; }
            [Category("ZAutoComplete-Datos"), Description("Campo que representa el Texto del item seleccionado"), DefaultValue("")]
            public string FieldText { get; set; }
            [Category("ZAutoComplete-Datos"), Description("Nro de carácter Ini Find"), DefaultValue("")]
            public int NroCarIni { get; set; }

            [Browsable(true)]
            private List<EasyFiltroParamURLws> easyParams;
            [
            Category("ZAutoComplete"),
            Description("Parametros que utilizara el WebService en la propiedad UrlAsmxMetodo, Mínimo un parámetro"),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
            Editor(typeof(EasyFiltroCollectionParams), typeof(UITypeEditor)),
            PersistenceMode(PersistenceMode.InnerProperty)
            ]
            public List<EasyFiltroParamURLws> EasyFiltroParamsURLws { get { return easyParams; } }

            */
            //style
        Bootstrap oBootstrap = new Bootstrap();
        [TypeConverter(typeof(Type_Style))]
        [Category("Editor"),
        Description("Detalle de la clase usuario."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]

        public Bootstrap EasyStyle
        {
                get {
                        return oBootstrap;
                    }
                set {
                        oBootstrap = (Bootstrap)value;                
                    }
        }




        [Category("Identificacion"), Description("Identificacion del control"), DefaultValue("")]

        TipoControl tipo;
        public TipoControl Tipo
        {
            get { return tipo; }
            set{tipo = value;}
        }


        #endregion



        /*---------------------------------------------new--------------------------------------------*/

        #region Itemscontrols personalizadas
        /*
        EasyFormItemTemplate oEasyFormItemTemplate = new EasyFormItemTemplate();

        [TypeConverter(typeof(Type_FormItemTemplate))]
        [Category("Editor"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public EasyFormItemTemplate EasyControlTemplate
        {
            get
            {
                if (oEasyFormItemTemplate.TemplateType == "EasyITemplateDatepicker")
                {
                    oEasyFormItemTemplate = new EasyITemplateDatepicker(oEasyFormItemTemplate);
                }
                else if (oEasyFormItemTemplate.TemplateType == "EasyITemplateNumericBox")
                {
                    oEasyFormItemTemplate = new EasyITemplateNumericBox(oEasyFormItemTemplate);
                }
                else if (oEasyFormItemTemplate.TemplateType == "EasyITemplateTextMultiline")
                {
                    oEasyFormItemTemplate = new EasyITemplateTextMultiline(oEasyFormItemTemplate);
                }
                else if (oEasyFormItemTemplate.TemplateType == "EasyITemplateUpLoad")
                {
                    oEasyFormItemTemplate = new EasyITemplateUpLoad(oEasyFormItemTemplate);
                }
                else if (oEasyFormItemTemplate.TemplateType == "EasyITemplateAutocompletar")
                {
                    oEasyFormItemTemplate = new EasyITemplateAutocompletar(oEasyFormItemTemplate);
                }
                
                return oEasyFormItemTemplate;
            }
            set
            {
                oEasyFormItemTemplate = value;
            }
        } */
        #endregion
       



        public EasyFomItem(): this(String.Empty){}
        public EasyFomItem(string _Id)
        {
            Id = _Id;
           //easyParams = new List<EasyFiltroParamURLws>();
        }


    }
}
