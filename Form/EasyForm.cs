using EasyControlWeb.Form.Controls;
using EasyControlWeb.Form.Estilo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static EasyControlWeb.EasyUtilitario;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form
{
     [ 
        AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("ClassName"),
        ParseChildren(true, "ClassName"),
        ToolboxData("<{0}:EasyForm runat=server></{0}:EasyForm")
      ]
    [Serializable]
    public class EasyForm : CompositeControl
    {
        #region variables
        string ToolBarFcnCliente = "";
        #endregion
        #region variable Globales
        string ScriptSetValue = "";
        #endregion
        EasyDropdownList ddl;

        #region Propiedades

        HtmlGenericControl _Contenedor;
        HtmlGenericControl CardBase;
        HtmlGenericControl _Form;
        EasyToolBarButtons oEasyToolBarButtons;
        HtmlButton btnPostBack;//se utiliza al momento de cambio de valor de un control
        List<LiteralControl> BloqueScript =  new List<LiteralControl>();
        //HtmlButton obtnAccion;
        HtmlGenericControl omsgValida;
        EasyMessageBox oEasyMessageBox;

        string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;


        public delegate void Commit(EasyButton oEasyButton, EasyFormDataBE oEasyFormDataBE);
        public event Commit CommitTransaccion;

        //Eventos de formulario desencadenado por el objeto
        //public delegate void Select_Change(EasyFormDataBE oEasyFormDataBE);
        public delegate void ChangeValue(string IdControl);
        public event ChangeValue Change;


        bool showButtonsOk_Cancel;
        public bool ShowButtonsOk_Cancel { get { return showButtonsOk_Cancel; }set { showButtonsOk_Cancel = value; } }

        string cssClass;
        public override string CssClass
        {
            get
            {
                if (cssClass == null)
                {
                    cssClass = "row g-3";
                }
                return cssClass;
            }
            set
            {
                cssClass = value;
            }




        }

       // public string Titulo { get; set; }

        private string className;
        public string ClassName
        {
            get
            {
                if (className == null)
                {
                    className = "form-row";
                }
                return className;
            }
            set
            {
                className = value;
            }
        }


        /*Header del forms*/

        EasyFormHeader oEasyFormHeader = new EasyFormHeader();
        [TypeConverter(typeof(Type_Header))]
        [Category("Editor"),
        Description("Detalle de la clase usuario."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public EasyFormHeader Cabecera
        {
            get
            {
                return oEasyFormHeader;
            }
            set
            {
                oEasyFormHeader = (EasyFormHeader)value;
            }
        }


        [Browsable(true)]
        private List<EasyFormSeccion> arrSeccion;
        [
        Category("Behavior"),
        Description("Coleccion de Seccion de grupos de formularios"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(EasyFormCollectionEditor.EasyFormoCollectionEditorSeccion), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public List<EasyFormSeccion> Secciones
        {
            get
            {
                if (arrSeccion == null) {
                    arrSeccion = new List<EasyFormSeccion>();
                }
                return arrSeccion;
            }
        }
       
        #endregion


        #region Eventos Propios
        protected override void OnInit(EventArgs e){}
        protected override void CreateChildControls()
        {
            string IdCtrl = "";
            string ScriptObtenerDatos = "var " + this.ClientID +"={};\n" + this.ClientID + ".GetAllData = function(){\n";
            ScriptObtenerDatos += "             var DataColletion=new Array();\n";
            _Contenedor = CrearContenedor("container");
             //Crea la Cabecera del formulario
            _Contenedor.Controls.Add(this.Cabecera);
             //Creacion del formuario de contrles
            _Form = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("form", this.CssClass);
           // _Form.Controls.Add(Enter());
            int IdSeccion = 0;
            if ((this.Secciones != null) && (this.Secciones.Count > 0))
            {
                foreach (EasyFormSeccion oFormSeccion in this.Secciones)
                {

                    //HtmlGenericControl SeccionTitulo = Helper.HtmlControlsDesign.CrearControl("h2", null, null, oFormSeccion.Titulo);
                    HtmlGenericControl SeccionTitulo = Helper.HtmlControlsDesign.CrearControl("h2");

                        HtmlGenericControl _imgShowHide = Helper.HtmlControlsDesign.CrearControl("img");
                        _imgShowHide.Attributes["src"] = ((oFormSeccion.Visible == false) ? EasyUtilitario.Constantes.ImgDataURL.IconoFijada: EasyUtilitario.Constantes.ImgDataURL.IconoDesFijar);  
                        _imgShowHide.Attributes["close"] = EasyUtilitario.Constantes.ImgDataURL.IconoFijada;
                        _imgShowHide.Attributes["show"] = EasyUtilitario.Constantes.ImgDataURL.IconoDesFijar;
                        _imgShowHide.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = "javascript:" + this.ClientID + "_ShowHide(this,'" + ((oFormSeccion.ID == null) ? IdSeccion.ToString() : oFormSeccion.ID) + "');";
                        _imgShowHide.Attributes["style"] = ((oFormSeccion.Colapsable == true) ? "display:block;" : "display:none;") + " cursor: pointer;";


                    HtmlTable tbl = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(1, 2);
                            tbl.Attributes["style"] = "width:100%;" + ((oFormSeccion.Colapsable == true) ? "border: 1px  dotted #696666;" : ""); ;
                            tbl.Rows[0].Cells[0].InnerText = oFormSeccion.Titulo;
                            tbl.Rows[0].Cells[0].Attributes["style"] = "width:95%;";
                            tbl.Rows[0].Cells[1].Controls.Add(_imgShowHide);
                            tbl.Rows[0].Cells[1].Attributes["style"] = "width:5%;";
                            tbl.Rows[0].Cells[1].Attributes["aling"] = "right";

                    //SeccionTitulo.Controls.Add(_imgShowHide);
                    SeccionTitulo.Controls.Add(tbl);

                    _Form.Controls.Add(SeccionTitulo);
                    // _Form.Controls.Add(Enter());
                    //Creacion de seccion de grupos
                    HtmlGenericControl _grp = Helper.HtmlControlsDesign.CrearControl("div", this.ClassName + " FrmGrp");
                    _grp.ID = "Secc" + ((oFormSeccion.ID ==null)? IdSeccion.ToString() : oFormSeccion.ID);
                    _grp.Attributes["ShowHide"] = ((oFormSeccion.Visible == false) ? "0" : "1");

                    // _grp.ID = this.ClientID;
                    //Listar los Items del grupo 
                    foreach (Control ctrl in oFormSeccion.ItemsCtrl) {
                        ScriptObtenerDatos += "             DataColletion[DataColletion.length] = new Array();\n";
                        if (ctrl is EasyNumericBox)
                        {
                            EasyNumericBox oNB = (EasyNumericBox)ctrl;
                            //_grp.Controls.Add(CrearControlBase(oNB, oNB.Etiqueta, oNB.Requerido, oNB.MensajeValida, oNB.EasyStyle));
                            _grp.Controls.Add(EasyUtilitario.Helper.HtmlControlsDesign.CrearControlBase(oNB, oNB.Etiqueta, oNB.Requerido, oNB.MensajeValida, oNB.EasyStyle));
                            if (!IsDesign())
                            {
                                if (oNB.EnableOnChange)
                                {
                                    string NomCtrl = ClientID + "_" + oNB.ID;
                                    string ScriptKeyDow = "$(" + cmll + "#" + NomCtrl + cmll + @").keydown(function(e){
                                                                                    if (e.keyCode == 13){
                                                                                     __doPostBack('" + this.ClientID + @"$btnAction', '" + oNB.ID + @"');
                                                                                    }
                                                                                });";
                                    BloqueScript.Add(new LiteralControl("<script>\n" + ScriptKeyDow + "</script>"));
                                }

                                IdCtrl=this.ClientID + "_" + oNB.ID;
                                ScriptObtenerDatos += "             DataColletion[" + cmll + IdCtrl + cmll + "] = " + IdCtrl + ".GetValue();\n";
                            }
                        }
                        else if (ctrl is EasyDatepicker)
                        {
                            EasyDatepicker oDP = (EasyDatepicker)ctrl;
                            _grp.Controls.Add(EasyUtilitario.Helper.HtmlControlsDesign.CrearControlBase(oDP, oDP.Etiqueta, oDP.Requerido, oDP.MensajeValida, oDP.EasyStyle));
                            if (!IsDesign())
                            {
                                    string _doPost = (((oDP.EnableOnChange)&&(Change != null)) ? "__doPostBack('" + this.ClientID + @"$btnAction', '" + oDP.ID + @"');" : "");

                                    string defFNC = oDP.fncSelectDate;
                                    if (defFNC != null)
                                    {
                                        defFNC = defFNC + "(value);";
                                    }
                                    string NombreFCAC = oDP.ID + "_OnSelectDate";
                                    oDP.fncSelectDate = NombreFCAC;
                                    string ScriptSelectDate = "function " + NombreFCAC + @"(value){ 
                                                " + defFNC + @"
                                                " + _doPost + @"
                                           }";
                                    BloqueScript.Add(new LiteralControl("<script>\n" + ScriptSelectDate + "</script>"));

                                //Datos script
                                IdCtrl = this.ClientID + "_" + oDP.ID;
                                ScriptObtenerDatos += "             DataColletion[" + cmll + IdCtrl + cmll + "] = " + IdCtrl + ".GetValue();\n";
                            }
                        }
                        else if (ctrl is EasyTimepicker)
                        {
                            EasyTimepicker oTP = (EasyTimepicker)ctrl;

                            _grp.Controls.Add(
                                EasyUtilitario.Helper.HtmlControlsDesign.CrearControlBase(
                                    oTP,
                                    oTP.Etiqueta,
                                    oTP.Requerido,
                                    oTP.MensajeValida,
                                    oTP.EasyStyle
                                )
                            );

                            if (!IsDesign())
                            {
                                string _doPost = (((oTP.EnableOnChange) && (Change != null))
                                    ? "__doPostBack('" + this.ClientID + @"$btnAction', '" + oTP.ID + @"');"
                                    : "");

                                string defFNC = oTP.fncSelectTime;
                                if (defFNC != null)
                                {
                                    defFNC = defFNC + "(value);";
                                }

                                string NombreFCAC = oTP.ID + "_OnSelectTime";
                                oTP.fncSelectTime = NombreFCAC;

                                string ScriptSelectTime = "function " + NombreFCAC + @"(value){ 
                        " + defFNC + @"
                        " + _doPost + @"
                   }";

                                BloqueScript.Add(
                                    new LiteralControl("<script>\n" + ScriptSelectTime + "</script>")
                                );

                                // Datos script
                                IdCtrl = this.ClientID + "_" + oTP.ID;
                                ScriptObtenerDatos += "             DataColletion[" + cmll + IdCtrl + cmll + "] = " + IdCtrl + ".GetValue();\n";
                            }
                        }
                        else if (ctrl is EasyAutocompletar)
                        {
                            EasyAutocompletar oDP = (EasyAutocompletar)ctrl;
                            _grp.Controls.Add(EasyUtilitario.Helper.HtmlControlsDesign.CrearControlBase(oDP, oDP.Etiqueta, oDP.Requerido, oDP.MensajeValida, oDP.EasyStyle));
                            if (!IsDesign())
                            {
                                    string FncDefault = ((oDP.fnOnSelected != null) ? oDP.fnOnSelected + "(value,ItemBE);" : "");
                                    string _doPost = (((oDP.EnableOnChange)&&(Change != null)) ? "__doPostBack('" + this.ClientID + @"$btnAction', '" + oDP.ID + @"');" : "");

                                    string NombreFCAC = oDP.ID + "_OnSelect";
                                    oDP.fnOnSelected = NombreFCAC;
                                    string ScriptAutocomplete = "function " + NombreFCAC + @"(value, ItemBE){ 
                                                                    " + FncDefault + @"
                                                                    " + _doPost + @"
                                                                 }";
                                    BloqueScript.Add(new LiteralControl("<script>\n" + ScriptAutocomplete + "</script>"));

                                //Datos script
                                IdCtrl = this.ClientID + "_" + oDP.ID;
                                if (ctrl.GetType() == typeof(EasyAutocompletar))
                                {
                                    ScriptObtenerDatos += "             DataColletion[" + cmll + IdCtrl + cmll + "] = " + IdCtrl + ".GetValue();\n";
                                }
                                else
                                {
                                    ScriptObtenerDatos += "             DataColletion[" + cmll + IdCtrl + cmll + "] = " + IdCtrl + ".GetListValue();\n";
                                }
                            }
                        }
                        else if (ctrl is EasyUpLoad)
                        {
                            EasyUpLoad oDP = (EasyUpLoad)ctrl;
                            _grp.Controls.Add(EasyUtilitario.Helper.HtmlControlsDesign.CrearControlBase(oDP, oDP.Titulo, oDP.Requerido,"No se ha cargadoa archivos a ser enviados..", oDP.EasyStyle));

                        }
                        else if (ctrl is EasyDropdownList)
                        {
                            // EasyDropdownList ddl = (EasyDropdownList)ctrl;
                            ddl = new EasyDropdownList();
                            ddl = (EasyDropdownList)ctrl;
                            _grp.Controls.Add(EasyUtilitario.Helper.HtmlControlsDesign.CrearControlBase(ddl, ddl.Etiqueta, ddl.Requerido, ddl.MensajeValida, ddl.EasyStyle));
                           

                            if (!IsDesign())
                            {
                                ddl.LoadData();//Carga los datos segun la definicion del DataInterconec
                                if (ddl.AutoPostBack) {
                                    ddl.SelectedIndexChanged += new System.EventHandler(EasyDropdownList_SelectedIndexChanged);
                                }
                                //Datos script
                                IdCtrl = this.ClientID + "_" + ddl.ID;
                                ScriptObtenerDatos += "             DataColletion[" + cmll + IdCtrl + cmll + "] = " + IdCtrl + ".GetValue();\n";

                            }


                        }
                        else if (ctrl is EasyTextBox) {
                            EasyTextBox txt = (EasyTextBox)ctrl;
                            _grp.Controls.Add(EasyUtilitario.Helper.HtmlControlsDesign.CrearControlBase(txt, txt.Etiqueta, txt.Requerido, txt.MensajeValida, txt.EasyStyle));
                            IdCtrl = this.ClientID + "_" + txt.ID;
                            ScriptObtenerDatos += "             DataColletion[" + cmll + IdCtrl + cmll + "] = " + IdCtrl + ".GetValue();\n";
                        }
                        else if (ctrl is EasyCheckBox)
                        {
                            EasyCheckBox cb = (EasyCheckBox)ctrl;
                            _grp.Controls.Add(EasyUtilitario.Helper.HtmlControlsDesign.CrearControlBase(cb,null,false, null, cb.EasyStyle));
                            IdCtrl = this.ClientID + "_" + cb.ID;
                            ScriptObtenerDatos += "             DataColletion[" + cmll + IdCtrl + cmll + "] = " + IdCtrl + ".GetValue();\n";
                        }
                    }

                    _Form.Controls.Add(_grp);
                    //Script para control de las secciones
                    
                    IdSeccion++;
                }
                ScriptObtenerDatos += "             return DataColletion;\n";
                ScriptObtenerDatos += "}\n";

                BloqueScript.Add(new LiteralControl("<script>\n" + ScriptObtenerDatos + "</script>"));//Cierra la funcion de obtencion de datos
                //Fin del formulario y secciones
            }
            //seccion de botones

           if (ShowButtonsOk_Cancel == true)
            {
                ToolBarFcnCliente = this.ClientID + "_Accion";
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



               // oEasyToolBarButtons.fnToolBarButtonClick = ToolBarFcnCliente; en caso sea necesario tulizarlo en el cliente
                oEasyToolBarButtons.onClick += EasyToolBarButtons_onClick;

                // _Form.Controls.Add(oEasyToolBarButtons);
                //, this.ClassName
                HtmlGenericControl _grp_ToolBar = Helper.HtmlControlsDesign.CrearControl("div");
                _grp_ToolBar.Style["border-color"] = "red";
                _grp_ToolBar.Controls.Add(oEasyToolBarButtons);
                _Form.Controls.Add(_grp_ToolBar);




                /*obtnAccion = new HtmlButton();
                obtnAccion.InnerText = "Aceptar";
                obtnAccion.ID = "btnApcetarUnder";
                obtnAccion.ServerClick += btn_onClick;
                _Form.Controls.Add(obtnAccion);*/
                //Crear el script para la funcion que permitira desencader postback



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

            CardBase = CardContenedor(_Contenedor);
            this.Controls.Add(CardBase);

            Control ct = this.NamingContainer;
            if (ct.ClientID == "_Page")
            {
                ScriptSetValue = "function " + this.ID + @"_val(NameControl,valor){
                                        var strNombreCtrl = '" + this.ID + "_' + " + @"NameControl;
                                            document.getElementById(strNombreCtrl).value=valor;
                                }
                    ";
            }
            else
            {
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
            if (IsDesign())
            {
                (new LiteralControl("Modo de diseño")).RenderControl(writer);
            }
            else {//EN EJECUCION
                CardBase.RenderControl(writer);

                string srcSecc = @" function " + this.ClientID + @"_ShowSeccion(IdSeccion){
                                                                          var NomId = '" + this.ClientID + @"_Secc'+IdSeccion;
                                                                          $('#'+ NomId).show();
                                                                   }
                                    function " + this.ClientID + @"_HideSeccion(IdSeccion){
                                                                            var NomId = '" + this.ClientID + @"_Secc'+IdSeccion;
                                                                            $('#'+ NomId).hide();
                                                                    };

                                    function " + this.ClientID + @"_ShowHide(e,idSecc){
                                        var _Img = jNet.get(e); 
                                        if(_Img.attr('src')==_Img.attr('close')){
                                            _Img.attr('src',_Img.attr('show') );
                                            " + this.ClientID + @"_ShowSeccion(idSecc);                                         }
                                        else{
                                            _Img.attr('src',_Img.attr('close'));
                                             " + this.ClientID + @"_HideSeccion(idSecc);     
                                        }
                                    }
                                     Manager.Task.Excecute(function () {
                                                                        var FrmGrupos = document.querySelectorAll('.FrmGrp');
                                                                        Array.prototype.forEach.call(FrmGrupos, function(itemGrp, index) {
                                                                            var CtrlGrp = jNet.get(itemGrp);
                                                                                if(CtrlGrp.attr('ShowHide')=='0'){;
                                                                                    $('#'+ CtrlGrp.attr('id')).hide();
                                                                                }
                                                                        });
                                                            }, 500,true);
                                   ";

                BloqueScript.Add(new LiteralControl("<script>\n" + srcSecc + "</script>"));//Cierra la funcion de obtencion de datos

                foreach (LiteralControl LCScript in BloqueScript)
                {
                    LCScript.RenderControl(writer);
                }
            }
        }
        #endregion

        #region Eventos Externos
        //evento de los controles ddl
        protected void EasyDropdownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            EasyForm_PostBack(sender, e);
        }

        protected void btn_onClick(object sender, EventArgs e)
        {
            string Argument = ((System.Web.UI.Page)HttpContext.Current.Handler).Request["__EVENTARGUMENT"];
            if (CommitTransaccion != null)//verifica si esta asociado en la pagina con el evento
            {
                EasyControlWeb.Form.Controls.EasyButton oEasyButton = new EasyButton();
                oEasyButton.Texto = Argument;

                if (Argument == "btnAceptar")
                {
                    if (Validar())
                    {
                        CommitTransaccion?.Invoke(oEasyButton, getAllData());
                    }
                }
                else
                {
                    CommitTransaccion?.Invoke(oEasyButton, null);
                }
            }

        }

        protected void EasyToolBarButtons_onClick(EasyControlWeb.Form.Controls.EasyButton oEasyButton)
        {
            if (CommitTransaccion != null)//verifica si esta asociado en la pagina con el evento
            {
                if (oEasyButton.Texto == "Aceptar")
                {
                    if (Validar())
                    {
                        CommitTransaccion?.Invoke(oEasyButton, getAllData());
                    }
                    else {

                        oEasyMessageBox = new EasyMessageBox();
                        oEasyMessageBox.ID = "msgb";
                        oEasyMessageBox.Titulo = "Campos obligatorios";
                        oEasyMessageBox.Contenido = "Por favor completar los datos en los campos obligatorios del formulario..!!";
                        oEasyMessageBox.Tipo = EasyUtilitario.Enumerados.MessageBox.Tipo.AlertType;
                        oEasyMessageBox.AlertStyle = EasyUtilitario.Enumerados.MessageBox.AlertStyle.modern;
                        _Form.Controls.Add(oEasyMessageBox);
                    }
                }
                else {
                    CommitTransaccion?.Invoke(oEasyButton, null);
                }
            }
        }

        protected virtual void EasyForm_PostBack(object sender, EventArgs e)
        {
           
            string Argument = ((System.Web.UI.Page)HttpContext.Current.Handler).Request["__EVENTARGUMENT"];//Obtine el argumento como nombre del objeto que desencadena el evento
            if (Change != null)
            {
                Change?.Invoke(Argument);
            }
        }
        #endregion

        /*****************************************************************************************************/
        #region Metodos Propios
        public void SetMsgRequerido(string Id,string className) {
            string NombreFeedCtrl = "feed_" + Id;
            Control ctrl = this.FindControl(NombreFeedCtrl);
            ((HtmlGenericControl)ctrl).Attributes["class"] = className;
        }


        bool  Validar() {
            bool go=true;
            foreach (EasyFormSeccion oeasyFormSeccion in Secciones)
            {
                foreach (Control ctrl in oeasyFormSeccion.ItemsCtrl)
                {
                    PropertyInfo property = ctrl.GetType().GetProperty("Requerido");
                    bool Req = (bool)property.GetValue(ctrl, null);
                    if (Req == true) {
                        SetMsgRequerido(ctrl.ID, "invalid-feedback");//Inicializa para luego aplicar la validacion 

                        string valor = (string)GetValue(ctrl.ID);
                        if (ctrl is EasyDropdownList)
                        {
                            valor = (string)GetValue(ctrl.ID);
                            if (valor == "-1")
                            {
                                go = false;
                                SetMsgRequerido(ctrl.ID, "was-validated");
                            }
                        }
                        else
                        {
                            if (valor.Length == 0)
                            {
                                go = false;
                                SetMsgRequerido(ctrl.ID, "was-validated");
                            }
                        }
                    }
                }
            }
            return go;
        }

        public EasyFormDataBE getAllData()
        {
            return getAllData(true);
        }
        public EasyFormDataBE getAllData(bool validar)
        {
            if (Validar() == false) { return null; }//Ejecuta la validacion previo  a la entrega de resultados

            EasyFormDataBE oEasyFormDataBE = new EasyFormDataBE();
            foreach (EasyFormSeccion oeasyFormSeccion in Secciones) {
                foreach (Control ctrl in oeasyFormSeccion.ItemsCtrl)
                {
                    if (ctrl is EasyNumericBox)
                    {
                        oEasyFormDataBE.Data.Add(ctrl.ID, ((EasyNumericBox)ctrl).Text);
                    }
                    else if (ctrl is EasyDatepicker)
                    {
                        oEasyFormDataBE.Data.Add(ctrl.ID, ((EasyDatepicker)ctrl).Text);
                    }
                    else if (ctrl is EasyAutocompletar)
                    {
                        oEasyFormDataBE.Data.Add(ctrl.ID, ((EasyAutocompletar)ctrl).GetValue());
                    }
                    else if (ctrl is EasyUpLoad)
                    {
                        EasyUpLoad oEasyUpLoad = ((EasyUpLoad)ctrl);
                        string LstFiles = ""; int idx = 0;
                        List<EasyFileInfo> allFiles = oEasyUpLoad.getAllFiles(true);
                        if (allFiles != null)
                        {
                            foreach (EasyFileInfo oEasyFileInfo in allFiles)
                            {
                                if (oEasyFileInfo.Temporal == true)//Esta en la carpeta temporal listo para ser pasado a la carpeta final
                                {
                                    if (oEasyFileInfo.IdEstado == 1)
                                    {
                                        if (oEasyFileInfo.Nombre.Length > 0)
                                        {
                                            LstFiles += ((idx == 0) ? "" : "@") + oEasyFileInfo.Nombre;
                                            /*Copia a la carpeta final*/
                                            if (System.IO.File.Exists(oEasyUpLoad.PathLocalyWeb.CarpetaFinal + oEasyFileInfo.Nombre) == false)
                                            {
                                                System.IO.File.Move(oEasyUpLoad.PathLocalyWeb.CarpetaTemporal + oEasyFileInfo.Nombre, oEasyUpLoad.PathLocalyWeb.CarpetaFinal + oEasyFileInfo.Nombre);
                                                oEasyFileInfo.Temporal = false;
                                            }
                                            else
                                            {
                                                System.IO.File.Delete(oEasyUpLoad.PathLocalyWeb.CarpetaFinal + oEasyFileInfo.Nombre);
                                                System.IO.File.Copy(oEasyUpLoad.PathLocalyWeb.CarpetaTemporal + oEasyFileInfo.Nombre, oEasyUpLoad.PathLocalyWeb.CarpetaFinal + oEasyFileInfo.Nombre);
                                                oEasyFileInfo.Temporal = false;

                                            }
                                        }
                                    }
                                    else
                                    { //Elimiar de la carpeta temporal
                                        System.IO.File.Delete(oEasyUpLoad.PathLocalyWeb.CarpetaTemporal + oEasyFileInfo.Nombre);
                                    }
                                }
                                else
                                { //archivos que se encuentran en la carpeta fina
                                    if (oEasyFileInfo.IdEstado == 1)
                                    {
                                        LstFiles += ((idx == 0) ? "" : "@") + oEasyFileInfo.Nombre + ";" + oEasyFileInfo.Temporal;
                                    }
                                    else
                                    {
                                        System.IO.File.Delete(oEasyUpLoad.PathLocalyWeb.CarpetaFinal + oEasyFileInfo.Nombre);
                                    }
                                }
                                idx++;
                            }
                        }
                        oEasyFormDataBE.Data.Add(ctrl.ID, LstFiles);
                    }
                    else if (ctrl is EasyDropdownList)
                    {
                        oEasyFormDataBE.Data.Add(ctrl.ID, ((EasyDropdownList)ctrl).SelectedValue);
                    }
                    else if (ctrl is EasyTextBox)
                    {
                        EasyTextBox txt = (EasyTextBox)ctrl;
                        oEasyFormDataBE.Data.Add(ctrl.ID, txt.Text);
                    }
                    else if (ctrl is EasyCheckBox)
                    {
                        EasyCheckBox chkb = (EasyCheckBox)ctrl;
                        oEasyFormDataBE.Data.Add(ctrl.ID,chkb.Checked.ToString());
                    }
                }
            }

            return oEasyFormDataBE;
        }
        public void SetValue(string Id, string Valor)
        {
            SetValue(Id, "", Valor);
        }
        public void SetValue(string Id, bool Valor)
        {
            SetValue(Id, "", Valor.ToString());
        }
        public void SetValue(string Id, string Text, string Valor)
        {
            Control  ctrl = this.FindControl(Id);
            if (ctrl != null)
            {
                if (ctrl is EasyTextBox)
                {
                    ((EasyTextBox)ctrl).Text = Valor;
                }
                else if (ctrl is EasyNumericBox)
                {
                    ((EasyNumericBox)ctrl).Text = Valor;
                }
                else if (ctrl is EasyDatepicker)
                {
                    ((EasyDatepicker)ctrl).Text = Valor;
                }
                else if (ctrl is EasyAutocompletar)
                {
                    ((EasyAutocompletar)(ctrl)).SetValue(Text, Valor);
                }
                else if (ctrl is EasyDropdownList)
                {
                    ListItem itm = ((EasyDropdownList)(ctrl)).Items.FindByValue(Valor);
                    if (itm != null) {
                        itm.Selected = true;
                    }
                }
                else if (ctrl is EasyCheckBox)
                {
                    ((EasyCheckBox)(ctrl)).Checked = Convert.ToBoolean(Convert.ToInt32(Valor));
                    
                }
                else if (ctrl is EasyTimepicker)
                {
                    ((EasyTimepicker)ctrl).Text = Valor;
                }
            }
        }
        public void SetValue(string Id, List<EasyFileInfo> allEasyFileInfo) {
            Control ctrl = this.FindControl(Id);
            if (ctrl is EasyUpLoad)
            {
                 ((EasyUpLoad)(ctrl)).setAllFiles(allEasyFileInfo);
                ((EasyUpLoad)(ctrl)).getAllFiles(true);
            }
        }
       
        public void SetDataRow(string Id, Dictionary<string, string> RecordSet)
        {
            Control ctrl = this.FindControl(Id);
            if (ctrl != null)
            {
                ((EasyAutocompletar)(ctrl)).SetData(RecordSet);
            }
        }



       

        public object GetValue(string Id)
        {
            Control ctrl = this.FindControl(Id);
            return GetValue(ctrl);
        }

        public object GetValue(Control ctrl)
        {
            object objReturn = null;
            if (ctrl != null)
            {
                if (ctrl is EasyTextBox)
                {
                    objReturn = ((EasyTextBox)(ctrl)).Text;
                }
                else if (ctrl is EasyDatepicker)
                {
                    objReturn = ((EasyDatepicker)(ctrl)).Text;
                }
                else if (ctrl is EasyTimepicker)
                {
                    objReturn = ((EasyTimepicker)(ctrl)).Text;
                }
                else if (ctrl is EasyNumericBox)
                {
                    objReturn = ((EasyNumericBox)(ctrl)).Text;
                }
                else if (ctrl is EasyAutocompletar)
                {
                    objReturn = ((EasyAutocompletar)(ctrl)).GetValue();
                }
                else if (ctrl is EasyDropdownList)
                {
                    objReturn = ((EasyDropdownList)ctrl).SelectedValue;
                }
                else if (ctrl is EasyUpLoad)
                {
                    EasyUpLoad oEasyUpLoad = ((EasyUpLoad)ctrl);
                    string LstFiles = ""; int idx = 0;
                    List<EasyFileInfo> allFiles = oEasyUpLoad.getAllFiles(true);
                    if (allFiles != null)
                    {
                        foreach (EasyFileInfo oEasyFileInfo in allFiles)
                        {
                            LstFiles += ((idx == 0) ? "" : "@") + oEasyFileInfo.Nombre + ";" + oEasyFileInfo.Temporal;
                            idx++;
                        }
                    }
                    objReturn = LstFiles;
                }
                else if (ctrl is EasyCheckBox)
                {
                    objReturn = ((EasyCheckBox)ctrl).Checked;
                }
            }
            return objReturn;
        }




        public void SetReadOnly(string Id)
        {
            Control ctrl = this.FindControl(Id);
            if (ctrl != null)
            {
                if (ctrl is EasyTextBox)
                {
                    ((EasyTextBox)ctrl).ReadOnly =true;
                }
                else if (ctrl is EasyNumericBox)
                {
                    ((EasyNumericBox)ctrl).ReadOnly = true;
                }
                else if (ctrl is EasyDatepicker)
                {
                    ((EasyDatepicker)ctrl).ReadOnly = true;
                }
                else if (ctrl is EasyAutocompletar)
                {
                    ((EasyAutocompletar)(ctrl)).SetReadOnly();
                }
                else if (ctrl is EasyDropdownList)
                {
                     ((EasyDropdownList)(ctrl)).SetReadOnly();
                }
            }
        }






        public object GetListItems(string Id)
        {
            Control ctrl = this.FindControl(Id);
            return GetListItems(ctrl);
        }
        public object GetListItems(Control ctrl)
        {
            object objReturn = null;
            if (ctrl != null)
            {
                if (ctrl is EasyUpLoad)
                {
                    EasyUpLoad oEasyUpLoad = ((EasyUpLoad)ctrl);
                    List<EasyFileInfo> allFiles = oEasyUpLoad.getAllFiles(true);
                    objReturn = allFiles;
                }
            }
            return objReturn;
        }

        /*public object GetValue(string Id, bool OnlyArray)
        {
            Control ctrl = this.FindControl(Id);
            return GetValue(ctrl, OnlyArray);
        }
        public object GetValue(Control ctrl,bool OnlyArray)
        {
            object objReturn = null;
            if (ctrl != null)
            {
                if (ctrl is EasyUpLoad)
                {
                    EasyUpLoad oEasyUpLoad = ((EasyUpLoad)ctrl);
                    List<EasyFileInfo> allFiles = oEasyUpLoad.getAllFiles(true);
                    objReturn = allFiles;
                }
            }
            return objReturn;
        }
        */

        public object GetDataRow(string Id)
        {
            Control ctrl = this.FindControl(Id);
            object objReturn = null;
            if (ctrl != null)
            {
                objReturn = ((EasyAutocompletar)(ctrl)).GetData();
            }
            return objReturn;
        }



        public void ItemSetDataBoud(string Id, DataTable dtSource) {
            Control ctrl = this.FindControl(Id);
            if (ctrl != null)
            {
                if (ctrl is EasyDropdownList)
                {
                    EasyDropdownList ddl = (EasyDropdownList)ctrl;
                    ddl.DataSource = dtSource;
                    ddl.DataBind();
                    ddl.Items.Insert(0,new ListItem("[Seleccionar]", "-1"));
                }
            }
        }


        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        HtmlGenericControl CrearContenedor(string ClassName)
        {
            return EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", ClassName);
        }
        HtmlGenericControl CardContenedor(HtmlGenericControl ofrm)
        {
            HtmlGenericControl oCard1 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div");
            oCard1.Attributes.Add("style", "width: 98%;");
            HtmlGenericControl oCard2 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div","card shadow mb-4");
            HtmlGenericControl oCard3 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card-body shadow-sm p-5");

            oCard3.Controls.Add(ofrm);
            oCard2.Controls.Add(oCard3);
            oCard1.Controls.Add(oCard2);
            return oCard1;

        }
        Control Enter()
        {
            return new LiteralControl("\n");
        }

      
        #endregion

        /*****************************************************************************************************/


    }
}
