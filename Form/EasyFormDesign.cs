using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Security.Permissions;
using System.Collections;
using System.Drawing.Design;
using System.Web.UI.HtmlControls;
using EasyControlWeb.Form.Controls;
using EasyControlWeb.Test;
using EasyControlWeb.Filtro;
using EasyControlWeb.Form.Templates;
using EasyControlWeb.Form.Estilo;

namespace EasyControlWeb.Form
{
    /*[
        Designer(typeof(SimpleContainerControlDesigner)),
        ParseChildren(false)
    ]*/
  
    [
      AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
      AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
      DefaultProperty("Titulo"),
      ParseChildren(true, "Titulo"),
      ToolboxData("<{0}:EasyFormDesign runat=server></{0}:EasyFormDesign")
    ]
    [Serializable]
    // public class EasyFormDesign : CompositeControl, INamingContainer
    public class EasyFormDesign : CompositeControl
    {
        public enum FormSeccion
        {
            form,
            div,
            label,
            small,
            legend,
            p,
            hr,
            h2
        }
        #region Constantes
        const string vs_CONTROL_GRP = "ControlesGrupo";
        #endregion

        #region Declaracion de Objetos
        HtmlButton btnPostBack;
        EasyToolBarButtons oEasyToolBarButtons;


        HtmlGenericControl _Contenedor;
        HtmlGenericControl _Form;
        HtmlGenericControl CardBase;

        TextBox _Texto;
        EasyNumericBox _EasyNumericBox;
        EasyDatepicker _EasyDatepicker;
        EasyAutocompletar _EasyAutocompletar;
        EasyUpLoad _EasyUpLoad;
        Button btn;
        #endregion


        #region EVentos Externos

        public delegate void CompletarOperacion(EasyButton oEasyButton, EasyFormDataBE oEasyFormDataBE);
        public event CompletarOperacion ProcesoCompletado;

        //Eventos de formulario desencadenado por el objeto
        //public delegate void Select_Change(EasyFormDataBE oEasyFormDataBE);
        public delegate void Select_Change(string IdControl);
        public event Select_Change SelectAndChange;


        #endregion
        #region Cosntantes
        const string ClaseContenedora = "container";
        #endregion


        #region Objetos de Colleccion
            List<LiteralControl> BloqueScript = new List<LiteralControl>();
        #endregion
        #region variable Globales
        string ScriptSetValue = "";
        #endregion

        #region Propiedades Simples



        private ITemplate __captionTemplate = null;
        [TemplateContainer(typeof(ChartTemplateContainer))] 
        public ITemplate CaptionTemplate { get { return __captionTemplate; } set { __captionTemplate = value; } }



        [Category("Botones"), Description("Botones de Aceptar y cancelar + ubicación")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public bool ShowButtonsOk_Cancel {
            get {
                if (ViewState["VerBtnMant"] == null)
                {
                    ViewState["VerBtnMant"] = false;
                }
                return (bool)ViewState["VerBtnMant"];
                    
            }
            set { ViewState["VerBtnMant"] = value; }
         }

        [Category("Botones"), Description("Botones de Aceptar y cancelar + ubicación")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public EasyUtilitario.Enumerados.Ubicacion UbicacionOK_Cancel { get; set; }


        [Category("Header"),Description("Titulo del formulario")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public string Titulo { get; set; }
        [Category("Header"), Description("Descripción de la actividad ")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public string Descripcion { get; set; }




        private string className;
        public string ClassName
        {
            get
            {
                if (className == null)
                {
                    className = "row g-3";
                }
                return className;
            }
            set
            {
                className = value;
            }
        }
        #endregion

        List<EasyFormGrupo> grp;

        #region Propiedades de colleccion
        [Browsable(true)]
        private List<EasyFormGrupo> arrSeccionGrupo;
        [
        Category("Behavior"),
        Description("Coleccion de Seccion de grupos de formularios"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(EasyFormCollectionEditor.EasyFormoCollectionEditorGrupo), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public List<EasyFormGrupo> SeccionGrupo
        {
            get
            {
                InicializaColecciones();
                return arrSeccionGrupo;
            }
        }


       
        private List<Control> arrControl;
        [
        Category("Behavior"),
        Description("Coleccion de Seccion de grupos de formularios"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(EasyFormCollectionEditor.EasyFormCollectionEditorControls), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public List<Control> GrpControl
        {
            get
            {
                InicializaColecciones();
                return arrControl;
            }
        }
       


        void InicializaColecciones()
        {
            if (arrSeccionGrupo == null)
            {
                arrSeccionGrupo = new List<EasyFormGrupo>();
            }
            if (arrControl == null)
            {
                arrControl = new List<Control>();
            }
        }




        [Browsable(false)]
        List<Control> _MultipleItems;

        // custom editor attribute
        [Editor(typeof(CustomCollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contains  Button and  CheckBox items in a List collection")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public List<Control> MultipleItems
        {
            get
            {
                if (_MultipleItems == null)
                {
                    _MultipleItems = new List<Control>();
                }
                return _MultipleItems;
            }
        }




        #endregion


        HtmlGenericControl BaseObject(FormSeccion fSeccion)
        {
            return BaseObject(fSeccion,"");
        }
        HtmlGenericControl BaseObject(FormSeccion fSeccion, string ClaseStyle) {
            HtmlGenericControl miBase = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl(fSeccion.ToString(), ClaseStyle);
            return miBase;
        }
        HtmlGenericControl CrearContenedor(string ClassName)
        {
            return BaseObject(FormSeccion.div, ClassName);
        }

        HtmlGenericControl CrearForm(string ClassName)
        {
            return BaseObject(FormSeccion.form, ClassName);
        }

      
        HtmlGenericControl CrearFormColum(string ClaseStyle)
        {
            return BaseObject(FormSeccion.div, ClaseStyle);
        }
       HtmlGenericControl CrearFormEtiqueta(string Texto, string idCtrlRelacionado,string ClassName)
        {
            HtmlGenericControl lbl = BaseObject(FormSeccion.label, ClassName);
            lbl.InnerText = Texto;
            lbl.Attributes.Add("for", idCtrlRelacionado);
            return lbl;
        }
 
        HtmlGenericControl CrearParrafo(string Texto)
        {
            HtmlGenericControl _Parrafo = BaseObject(FormSeccion.h2);
            _Parrafo.InnerText = Texto;
            return _Parrafo;
        }

        HtmlGenericControl CrearGrupo() {
            HtmlGenericControl grp = BaseObject(FormSeccion.div, "form-row");
            return grp;
        }

        HtmlGenericControl CrearTextBox(EasyFomItem oEasyFomItem)
        {
            _Texto = new TextBox();
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string TipoO = ((oEasyFomItem.Tipo == EasyFomItem.TipoControl.TextBox) ? "Text" : oEasyFomItem.Tipo.ToString());
            _Texto.Attributes.Add("Type", TipoO);
            _Texto.Attributes.Add("class", ((Bootstrap)oEasyFomItem.EasyStyle).ClassName);

           _Texto.Attributes.Add("autocomplete", "off");
            _Texto.Attributes.Add("placeholder", oEasyFomItem.Placeholder);
            _Texto.Text = (string)oEasyFomItem.Valor;
            if(oEasyFomItem.EasyControlTemplate.TemplateType == "EasyITemplateTextMultiline")
            {
                _Texto.TextMode = TextBoxMode.MultiLine;
                _Texto.Rows = oEasyFomItem.EasyControlTemplate.NroLineas;
            }
            _Texto.Attributes.Add("data-validate", "true");
            _Texto.Style.Add("background", "white url('" + oEasyFomItem.EasyControlTemplate.PathImagenDefault + "') right center no-repeat; padding-right:5px;");

            if (oEasyFomItem.AutoPostBack)
            {

                string NomCtrl = ClientID + "_" + oEasyFomItem.Id;
                string ScriptKeyDow = "$(" + cmll + "#" + NomCtrl + cmll + @").keydown(function(e){
                                                                                    if (e.keyCode == 13){
                                                                                     __doPostBack('" + this.ClientID + @"$btnAction', '" + oEasyFomItem.Id + @"');
                                                                                    }
                                                                                });";
                BloqueScript.Add(new LiteralControl("<script>\n" + ScriptKeyDow + "</script>"));
            }
            if (oEasyFomItem.Requerido == true)
            {
                _Texto.Attributes.Add("required", "");
            }

            return CrearControlBase(oEasyFomItem, _Texto);
        }
        HtmlGenericControl CrearNumericBox(EasyFomItem oEasyFomItem)
        {
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            _EasyNumericBox = new EasyNumericBox();
            _EasyNumericBox.ID = oEasyFomItem.Id;
            _EasyNumericBox.Attributes.Add("class", ((Bootstrap)oEasyFomItem.EasyStyle).ClassName);
            _EasyNumericBox.Attributes.Add("autocomplete", "off");
            _EasyNumericBox.Placeholder = oEasyFomItem.Placeholder;
            _EasyNumericBox.Text = (string)oEasyFomItem.Valor;
            _EasyNumericBox.Attributes.Add("data-validate", "true");
            _EasyNumericBox.PathImagenDefault = oEasyFomItem.EasyControlTemplate.PathImagenDefault;

            _EasyNumericBox.NroDecimales = oEasyFomItem.EasyControlTemplate.NroDecimales; //oEasyFomItem.NroDecimales;
            if (oEasyFomItem.AutoPostBack)
            {
                string NomCtrl = ClientID + "_" + oEasyFomItem.Id;
                string ScriptKeyDow = "$(" + cmll + "#" + NomCtrl + cmll + @").keydown(function(e){
                                                                                    if (e.keyCode == 13){
                                                                                     __doPostBack('" + this.ClientID + @"$btnAction', '" + oEasyFomItem.Id + @"');
                                                                                    }
                                                                                });";
                BloqueScript.Add(new LiteralControl("<script>\n" + ScriptKeyDow + "</script>"));
            }
            if (oEasyFomItem.Requerido == true)
            {
                _EasyNumericBox.Attributes.Add("required", "");
            }
            return CrearControlBase(oEasyFomItem, _EasyNumericBox);
        }
       HtmlGenericControl CrearDatePick(EasyFomItem oEasyFomItem)
        {
            _EasyDatepicker = new EasyDatepicker();
            //_EasyDatepicker.Attributes.Add("class", oEasyFomItem.ClassName);
             _EasyDatepicker.Attributes.Add("class", ((Bootstrap)oEasyFomItem.EasyStyle).ClassName);

            _EasyDatepicker.Attributes.Add("autocomplete", "off");
            _EasyDatepicker.Placeholder= oEasyFomItem.Placeholder;
            _EasyDatepicker.Text = (string)oEasyFomItem.Valor;
            _EasyDatepicker.Attributes.Add("data-validate", "true");
            _EasyDatepicker.PathImagenDefault = oEasyFomItem.EasyControlTemplate.PathImagenDefault;

             if (oEasyFomItem.AutoPostBack)
             {
                string NombreFCAC = oEasyFomItem.Id + "_OnSelectDate";
                _EasyDatepicker.fncSelectDate = NombreFCAC;
                string ScriptSelectDate = "function " + NombreFCAC + @"(value){ 
                                                __doPostBack('" + this.ClientID + @"$btnAction', '" + oEasyFomItem.Id + @"');
                                           }";
                BloqueScript.Add(new LiteralControl("<script>\n" + ScriptSelectDate + "</script>"));
             }

            if (oEasyFomItem.Requerido == true)
            {
                _EasyDatepicker.Attributes.Add("required", "");
            }
            return CrearControlBase(oEasyFomItem, _EasyDatepicker);
        }
        HtmlGenericControl CrearAutoComplete(EasyFomItem oEasyFomItem)
        {
            _EasyAutocompletar = new EasyAutocompletar();
            _EasyAutocompletar.ID = oEasyFomItem.Id;
            _EasyAutocompletar.Attributes.Add("class", ((Bootstrap)oEasyFomItem.EasyStyle).ClassName);
            _EasyAutocompletar.Attributes.Add("autocomplete", "off");
            _EasyAutocompletar.Placeholder =  oEasyFomItem.Placeholder;
            //_EasyAutocompletar.tet = (string)oEasyFomItem.Valor;
            _EasyAutocompletar.Attributes.Add("data-validate", "true");
            _EasyAutocompletar.FieldText = oEasyFomItem.EasyControlTemplate.FieldText;
            _EasyAutocompletar.FieldValue = oEasyFomItem.EasyControlTemplate.FieldValue;
            _EasyAutocompletar.PathImagenDefault = oEasyFomItem.EasyControlTemplate.PathImagenDefault;
            _EasyAutocompletar.PathImagenLoanding = oEasyFomItem.EasyControlTemplate.PathImagenLoanding;
            _EasyAutocompletar.UrlWebServiceMetodo = oEasyFomItem.EasyControlTemplate.UrlWebServiceMetodo;
           
            /*Crea el bloque script rlacionado al control*/
            if (oEasyFomItem.AutoPostBack)
            {
                string NombreFCAC = oEasyFomItem.Id + "_OnSelect";
                _EasyAutocompletar.fnOnSelected = NombreFCAC;
                string ScriptAutocomplete = "function " + NombreFCAC + @"(value, ItemBE){ 
                                                __doPostBack('" + this.ClientID + @"$btnAction', '" + oEasyFomItem.Id + @"');
                                            }";
                BloqueScript.Add(new LiteralControl("<script>\n" + ScriptAutocomplete + "</script>"));
            }

            if ((oEasyFomItem.EasyControlTemplate.wsParams != null) && (oEasyFomItem.EasyControlTemplate.wsParams.Count > 0))
            {
                foreach (EasyFiltroParamURLws oParam in oEasyFomItem.EasyControlTemplate.wsParams)
                {
                    _EasyAutocompletar.UrlWebServiceParams.Add(oParam);
                }

            }

            if (oEasyFomItem.Requerido == true)
            {
                _EasyDatepicker.Attributes.Add("required", "");
            }
            return CrearControlBase(oEasyFomItem, _EasyAutocompletar);
        }

        HtmlGenericControl CrearControlBase(EasyFomItem oEasyFomItem,Control objCtrl)
        {
            HtmlGenericControl htmCampoGrupo = BaseObject(FormSeccion.div, "form-group");

            Bootstrap oBootstrap = (Bootstrap)oEasyFomItem.EasyStyle;
            HtmlGenericControl _Col = CrearFormColum("col-" + oBootstrap .TipoTalla + "-" + Convert.ToInt32(oBootstrap.Ancho).ToString());//Este valor tiene que ser atribto del control
            HtmlGenericControl _Label = CrearFormEtiqueta(oEasyFomItem.Etiqueta, oEasyFomItem.Id, oBootstrap.ClassLabel);
          
            objCtrl.ID = oEasyFomItem.Id;
          
            htmCampoGrupo.Controls.Add(_Label);
            htmCampoGrupo.Controls.Add(Enter());
            htmCampoGrupo.Controls.Add(objCtrl);
            if (oEasyFomItem.Requerido == true){
                HtmlGenericControl omsgValida = new HtmlGenericControl("div");
                omsgValida.Attributes.Add("class", "invalid-feedback");
                omsgValida.InnerText = oEasyFomItem.MsgValidacion;
                htmCampoGrupo.Controls.Add(omsgValida);
            }
            _Col.Controls.Add(htmCampoGrupo);
            return _Col;
        }


        HtmlGenericControl CrearUpload(EasyFomItem oEasyFomItem) {
            try
            {
                _EasyUpLoad = new EasyUpLoad();
                _EasyUpLoad.DisplayButtons = true;
                _EasyUpLoad.Titulo = oEasyFomItem.Etiqueta;// "Lista de archivos";
                _EasyUpLoad.fncScriptAceptar = this.ClientID + "_ConfirmarUp";
                _EasyUpLoad.PaginaProceso = ((oEasyFomItem.EasyControlTemplate.PaginaProceso==null)?"": oEasyFomItem.EasyControlTemplate.PaginaProceso);
                _EasyUpLoad.RutaLocalTmp = ((oEasyFomItem.EasyControlTemplate.RutaLocalTmp == null) ? @"c:\tmp\" : oEasyFomItem.EasyControlTemplate.RutaLocalTmp);
                _EasyUpLoad.HttpTmp = oEasyFomItem.EasyControlTemplate.HttpTmp;
                _EasyUpLoad.RutaLocalFinal = oEasyFomItem.EasyControlTemplate.RutaLocalFinal;
                _EasyUpLoad.HttpFinal = oEasyFomItem.EasyControlTemplate.HttpFinal;
                _EasyUpLoad.ImgDelete = oEasyFomItem.EasyControlTemplate.ImgDelete;
                _EasyUpLoad.CargarArchivo += EasyForm_CompletarCarga;
                return CrearControlBase(oEasyFomItem, _EasyUpLoad);
            }
            catch (Exception ex) {
                HtmlGenericControl Ctrl = new HtmlGenericControl("div");
                Ctrl.InnerText = ex.Message;
                return Ctrl;
            }
        }

        Control Enter() {
            return new LiteralControl("\n");
        }

        Control RayaHorizontal() {
            return BaseObject(FormSeccion.hr);
        }

        public void SetValue(string Id, List<EasyFileInfo> allEasyFileInfo)
        {
            object obj = this.FindControl(Id);
            if (obj != null)
            {
                if (obj.GetType() == typeof(EasyUpLoad))
                {
                    //List<EasyFileInfo> allEasyFileInfo = (List<EasyFileInfo>)LstObjetos.Cast<EasyFileInfo>();
                    ((EasyUpLoad)(obj)).setAllFiles(allEasyFileInfo);
                }

            }
         }
        public void SetValue(string Id, string Text,string Valor)
        {
            object obj = this.FindControl(Id);
            if (obj != null)
            {
                if (obj.GetType() == typeof(TextBox))
                {
                    ((TextBox)(obj)).Text = Valor;
                }
                else if (obj.GetType() == typeof(EasyNumericBox))
                {
                    ((EasyNumericBox)(obj)).Text = Valor;
                }
                else if (obj.GetType() == typeof(EasyDatepicker))
                {
                    ((EasyDatepicker)(obj)).Text = Valor;
                }

                else if (obj.GetType() == typeof(EasyAutocompletar))
                {
                    ((EasyAutocompletar)(obj)).SetValue(Text, Valor);
                }
            }

        }
        public void SetValue(string Id, string Valor) {
            SetValue(Id, "", Valor);
        }
       


       public object GetValue(string Id)
        {
            object objReturn = null; 
            object obj = this.FindControl(Id);
            if (obj != null)
            {
                if (obj.GetType() == typeof(TextBox))
                {
                    objReturn= ((TextBox)(obj)).Text ;
                }
                else if (obj.GetType() == typeof(EasyDatepicker))
                {
                    objReturn = ((EasyDatepicker)(obj)).Text;
                }
                else if (obj.GetType() == typeof(EasyNumericBox))
                {
                    objReturn = ((EasyNumericBox)(obj)).Text;
                }
                else if (obj.GetType() == typeof(EasyAutocompletar))
                {
                    objReturn = ((EasyAutocompletar)(obj)).getValue();
                }


            }
            return objReturn;
        }


        private Dictionary<int, string> Bootstrap_Style_Col = new Dictionary<int, string>();
        public void DefineClass_bootstrap_col()
        {
            Bootstrap_Style_Col.Add(1, "");

        
        }

        #region Controles internos
        HtmlGenericControl CardContenedor(HtmlGenericControl ofrm) {
            /*HtmlGenericControl oCard1 = BaseObject(FormSeccion.div,"col-lg-9 mx-auto");
            HtmlGenericControl oCard2 = BaseObject(FormSeccion.div, "card shadow mb-4");
            HtmlGenericControl oCard3 = BaseObject(FormSeccion.div, "card-body shadow-sm p-5");*/

            HtmlGenericControl oCard1 = BaseObject(FormSeccion.div, "");
            oCard1.Attributes.Add("style", "width: 98%;");
            HtmlGenericControl oCard2 = BaseObject(FormSeccion.div, "card shadow mb-4");
            HtmlGenericControl oCard3 = BaseObject(FormSeccion.div, "card-body shadow-sm p-5");

            oCard3.Controls.Add(ofrm);
            oCard2.Controls.Add(oCard3);
            oCard1.Controls.Add(oCard2);
            return oCard1;

        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            if (this.DesignMode == true) {
                this.EnsureChildControls();
            }
        }
        protected override void CreateChildControls()
        {
            Controls.Clear();
            this.ChildControlsCreated = true;

            List<EasyFormGrupo> efGrupoMemo = (List<EasyFormGrupo>)ViewState[vs_CONTROL_GRP];
            if (efGrupoMemo == null) { efGrupoMemo = new List<EasyFormGrupo>(); }
            List<EasyFormGrupo> efGrupo = this.SeccionGrupo;
            ViewState[vs_CONTROL_GRP] = ((efGrupo.Count > 0) ?  ((efGrupo.Count> efGrupoMemo.Count)? efGrupo: efGrupoMemo): ViewState[vs_CONTROL_GRP]);
            efGrupo = (List<EasyFormGrupo>)ViewState[vs_CONTROL_GRP];
            //Aqi debe de ir otro contenedor
            _Contenedor = CrearContenedor("container");
            //Crear la cabecera del formulario
            EasyFormHeader oEasyFormHeader = new EasyFormHeader();
                oEasyFormHeader.Titulo = this.Titulo;
                oEasyFormHeader.Descripcion = this.Descripcion;
            _Contenedor.Controls.Add(oEasyFormHeader);


            _Form = CrearForm(this.ClassName);
            if ((efGrupo != null)&& (efGrupo.Count > 0)){
                
              _Form.Controls.Add(Enter());
              foreach (EasyFormGrupo formGrupo in efGrupo) {
                    _Form.Controls.Add(CrearParrafo(formGrupo.Titulo));
                    _Form.Controls.Add(Enter());
                    HtmlGenericControl _grp = CrearGrupo();
                    #region Ver Ante
                     foreach (EasyFomItem fomItem in formGrupo.FormItems)
                      {
                          if ((fomItem.Tipo == EasyFomItem.TipoControl.TextBox) || (fomItem.Tipo == EasyFomItem.TipoControl.TextBoxMultiline) || (fomItem.Tipo == EasyFomItem.TipoControl.Email) || (fomItem.Tipo == EasyFomItem.TipoControl.Password))
                          {
                              _grp.Controls.Add(CrearTextBox(fomItem));
                          }
                          else if (fomItem.Tipo == EasyFomItem.TipoControl.NumericBox)
                          {
                              _grp.Controls.Add(CrearNumericBox(fomItem));
                          }
                          else if (fomItem.Tipo == EasyFomItem.TipoControl.Datepicker)
                          {
                              _grp.Controls.Add(CrearDatePick(fomItem));
                          }
                          else if (fomItem.Tipo == EasyFomItem.TipoControl.AutoComplete)
                          {
                              _grp.Controls.Add(CrearAutoComplete(fomItem)); 
                          }
                          else if (fomItem.Tipo == EasyFomItem.TipoControl.UpLoad)
                          {
                              _grp.Controls.Add(CrearUpload(fomItem));

                          }
                      }
                    #endregion
                 /*   foreach (Control ctrl in formGrupo.MultipleItems) {
                        if (ctrl is Button) {
                            _Texto = new TextBox();
                            //btn = (Button)ctrl;
                            _grp.Controls.Add(_Texto);
                        }
                    }*/

                  /*  foreach (EasyFomItem fomItem in formGrupo.FormItems)
                    {
                        if (fomItem.EasyControlTemplate.TemplateType is "EasyITemplateTextMultiline")
                        {
                            _grp.Controls.Add(CrearTextBox(fomItem));
                        }
                        else if (fomItem.EasyControlTemplate.TemplateType is "EasyITemplateDatepicker")
                        {
                            _grp.Controls.Add(CrearDatePick(fomItem));
                        }
                        else if (fomItem.EasyControlTemplate.TemplateType is "EasyITemplateNumericBox")
                        {
                            _grp.Controls.Add(CrearNumericBox(fomItem));
                        }
                        else if (fomItem.EasyControlTemplate.TemplateType is "EasyITemplateUpLoad")
                        {
                            _grp.Controls.Add(CrearUpload(fomItem));
                        }
                        else if (fomItem.EasyControlTemplate.TemplateType is "EasyITemplateAutocompletar")
                        {
                            _grp.Controls.Add(CrearAutoComplete(fomItem) );
                        }

                    }
                    */

                    _Form.Controls.Add(_grp);
                   // _Form.Controls.Add(RayaHorizontal());
                }
               
            }

            ViewState[vs_CONTROL_GRP]= efGrupo;

            if (ShowButtonsOk_Cancel == true)
            {
                oEasyToolBarButtons = new EasyToolBarButtons();

               EasyButton oEasyButtonA = new EasyButton();
                oEasyButtonA.Id = "btnAceptar";
                oEasyButtonA.ClassName = "btn btn-primary";
                oEasyButtonA.Texto = "Aceptar";
                oEasyButtonA.RunAtServer = true;
                oEasyButtonA.Ubicacion = EasyUtilitario.Enumerados.Ubicacion.Derecha;

                oEasyToolBarButtons.EasyButtons.Add(oEasyButtonA);

                EasyButton oEasyButtonC = new EasyButton();
                oEasyButtonC.Id = "btnCancelar";
                oEasyButtonC.ClassName = "btn btn-secondary";
                oEasyButtonC.Texto = "Cancelar";
                oEasyButtonC.RunAtServer = true;
                oEasyButtonC.Ubicacion = EasyUtilitario.Enumerados.Ubicacion.Derecha;

                oEasyToolBarButtons.EasyButtons.Add(oEasyButtonC);
                oEasyToolBarButtons.onClick += EasyToolBarButtons_onClick;

                _Form.Controls.Add(oEasyToolBarButtons);

            }
            //Autoposback
            btnPostBack = new HtmlButton();
            btnPostBack.ID = "btnAction";
            btnPostBack.InnerText = "Evento de lado del server";
            btnPostBack.Attributes.Add("runat", "server");
            btnPostBack.Style.Add("display", "none");
            btnPostBack.ServerClick += new System.EventHandler(EasyForm_PostBack);
            _Form.Controls.Add(btnPostBack);


            _Contenedor.Controls.Add(_Form);
            //this.Controls.Add(_Contenedor);

            CardBase = CardContenedor(_Contenedor);
            this.Controls.Add(CardBase);

            //Crear El Script para la Asignacion de valores mediante el siguiente metodo

            ViewState[vs_CONTROL_GRP] = efGrupo;
            Control ct = this.NamingContainer;
            if (ct.ClientID == "_Page")
            {
                ScriptSetValue= "function " + this.ID + @"_val(NameControl,valor){
                                        var strNombreCtrl = '" + this.ID + "_' + " + @"NameControl;
                                            document.getElementById(strNombreCtrl).value=valor;
                                }
                    ";
            }
            else {
                ScriptSetValue = "function " + ct.ClientID + @"_val(NameControl,valor){
                                        var strNombreCtrl = '" + this.ClientID + "_' + " + @"NameControl;
                                        document.getElementById(strNombreCtrl).value=valor;
                                }
                    ";
            }
            BloqueScript.Add(new LiteralControl("<script>\n" + ScriptSetValue + "</script>"));

        }

        protected override void Render(HtmlTextWriter writer)
        {
            CardBase.RenderControl(writer);
            //Anula el el evento KeyDown del body de la pagina
            string AnulaKeyPostback = @"$('body').keydown(function () {
                                                            if (event.keyCode == 13) {return false; }
                                                        });";
            BloqueScript.Add(new LiteralControl("<script>\n" + AnulaKeyPostback + "</script>"));
            //Registros de Script Generados en la pagina           
            foreach (LiteralControl LCScript in BloqueScript){
                LCScript.RenderControl(writer);
            }

            if (IsDesign())
            {
                (new LiteralControl("Modo de diseño")).RenderControl(writer);
            }
            
        }


        protected virtual void EasyForm_PostBack(object sender, EventArgs e)
        {
            string Argument = ((System.Web.UI.Page)HttpContext.Current.Handler).Request["__EVENTARGUMENT"];//Obtine el argumento como nombre del objeto que desencadena el evento
            if (SelectAndChange != null)
            {
                SelectAndChange?.Invoke(Argument);
            }
        }

        static  void EasyForm_CompletarCarga(List<EasyFileInfo> oLstEasyFileInfo) {
            int i = 0;
          
        }
        /*Eventos propios*/
        protected void EasyToolBarButtons_onClick(EasyControlWeb.Form.Controls.EasyButton oEasyButton)
        {
            if (ProcesoCompletado != null)//verifica si esta asociado en la pagina con el evento
            {
                ProcesoCompletado?.Invoke(oEasyButton,getMetaData());

            }
        }
        
        public EasyFormDataBE getMetaData() {
            List<EasyFormGrupo> efGrupoControls = (List<EasyFormGrupo>)ViewState[vs_CONTROL_GRP];

            EasyFormDataBE oEasyFormDataBE = new EasyFormDataBE();
            foreach (EasyFormGrupo oFormGrupo in efGrupoControls)
            {
                foreach (EasyFomItem oEasyFomItem in oFormGrupo.FormItems)
                {
                    if ((oEasyFomItem.Tipo == EasyFomItem.TipoControl.TextBox) || (oEasyFomItem.Tipo == EasyFomItem.TipoControl.Password) || (oEasyFomItem.Tipo == EasyFomItem.TipoControl.Email) || (oEasyFomItem.Tipo == EasyFomItem.TipoControl.Datepicker))
                    {
                        TextBox MiTxt = (TextBox)this.FindControl(oEasyFomItem.Id);
                        oEasyFormDataBE.Data.Add(oEasyFomItem.Id, MiTxt.Text);
                    }
                    else if (oEasyFomItem.Tipo == EasyFomItem.TipoControl.NumericBox)
                    {
                        EasyNumericBox MiNB = (EasyNumericBox)this.FindControl(oEasyFomItem.Id);
                        oEasyFormDataBE.Data.Add(oEasyFomItem.Id, MiNB.Text);
                    }
                    else if (oEasyFomItem.Tipo == EasyFomItem.TipoControl.Datepicker)
                    {
                        EasyDatepicker MiDP = (EasyDatepicker)this.FindControl(oEasyFomItem.Id);
                        oEasyFormDataBE.Data.Add(oEasyFomItem.Id, MiDP.Text);
                    }
                    else if (oEasyFomItem.Tipo == EasyFomItem.TipoControl.AutoComplete)
                    {
                        EasyAutocompletar MiACPLT = (EasyAutocompletar)this.FindControl(oEasyFomItem.Id);
                        oEasyFormDataBE.Data.Add(oEasyFomItem.Id, MiACPLT.getValue());
                    }
                    else if (oEasyFomItem.Tipo == EasyFomItem.TipoControl.UpLoad)
                    {
                        EasyUpLoad MiUpLoad = (EasyUpLoad)this.FindControl(oEasyFomItem.Id);
                        string LstArchivo = "";
                        List<EasyFileInfo> allEasyFileInfo = MiUpLoad.getAllFiles();
                        if (allEasyFileInfo != null)
                        {
                            foreach (EasyFileInfo oEasyFileInfo in allEasyFileInfo)
                            {
                                LstArchivo += oEasyFileInfo.ToString(true);
                            }
                            oEasyFormDataBE.Data.Add(oEasyFomItem.Id, LstArchivo);
                        }
                    }
                }
            }
            return oEasyFormDataBE;
        }


        protected override void RenderContents(HtmlTextWriter output)
        {
            int i = 0;
        }
        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
    }

    internal class ChartTemplateContainer
    {
    }


    //FindControls(((System.Web.UI.Page)HttpContext.Current.Handler).Page, Id, Valor);
    /* public void FindControls(Control Page, string Id, string Valor)
     {
         foreach (Control ctrl in Page.Controls)
         {
             if (ctrl is TextBox)
             {
                 _Texto = ((TextBox)(ctrl));
                 if (_Texto.ID == Id)
                 {
                     _Texto.Text = Valor;
                 }
                 //((TextBox)(ctrl)).ReadOnly = true;

             }
             else if (ctrl.HasControls())
             {
                 FindControls(ctrl, Id, Valor);
             }
         }
     }*/




    /*
    public class MiContenedor : ContainerControlDesigner
    {
        private Style _style = null;

        // Define the caption text for the frame in the design surface.
        public override string FrameCaption
        {
            get
            {
                return "Form Contenedor";
            }
        }

        // Define the style of the frame around the control in the design surface.
        public override Style FrameStyle
        {
            get
            {
                if (_style == null)
                {
                    _style = new Style();
                    _style.Font.Name = "Verdana";
                    _style.Font.Size = new FontUnit("XSmall");
                    _style.BackColor = Color.LavenderBlush;
                    _style.ForeColor = Color.DarkBlue;
                }

                return _style;
            }
        }
    }
    */

}
