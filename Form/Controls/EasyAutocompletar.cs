using EasyControlWeb.Filtro;
using EasyControlWeb.Form.Base;
using EasyControlWeb.Form.Estilo;
using EasyControlWeb.InterConeccion;
using System;
using System.Collections;
using System.Collections.Generic; 
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

//Referencia
//https://stackoverflow.com/questions/15664964/jquery-autocomplete-renderitem

namespace EasyControlWeb.Form.Controls
{
    [DefaultProperty("SelectValue")]
    [ToolboxData("<{0}:EasyAutocompletar runat=server></{0}:EasyAutocompletar>")]
    [Serializable]
    public class EasyAutocompletar: CompositeControl 
    { 
        string txtID="";
        public TextBox txtText;
        TextBox txtVal;
        TextBox txtRecordSelected;

        public EasyAutocompletar() : base()
        {
            if (txtVal == null) { txtVal = new TextBox(); }
            if (txtText == null) { txtText = new TextBox();}
            if (txtRecordSelected == null) { txtRecordSelected = new TextBox(); }
            
        }
        public EasyAutocompletar(string Formato) { }

        
        private bool enableOnChange;
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public bool EnableOnChange { get { return enableOnChange; } set { enableOnChange = value; } }

        private bool requerido;
        [Category("Validación"), Description("Activa la validacion de campos obligatorios")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public bool Requerido { get { return requerido; } set { requerido = value; } }

        private string mensajeValida;
        [Category("Validación"), Description("Descripcion que se debe mostrar si el campo es requerido u obligatorio")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string MensajeValida { get { return mensajeValida; } set { mensajeValida = value; } }

        [Category("Mensaje"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Placeholder { get; set; }

       

        [Category("Datos"), Description("Campo que representa el Valor del item seleccionado"), DefaultValue("")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string ValueField { get; set; }

        [Category("Datos"), Description("Campo que representa el Texto del item seleccionado"), DefaultValue("")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string DisplayText { get; set; }
        [Category("Datos"), Description("Cantidad de caracteres que desecandena la búsqueda"), DefaultValue("")]

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public int NroCarIni { get; set; }

        [Category("FuncionesJScript"), Description("Contructor de Plantillas"), DefaultValue("")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncTempaleCustom { get; set; }

        [Category("FuncionesJScript"), Description("function javascript para enlazar al evento de seleccion define 1 parametro que es la clase que contiene el registro seleccionado"), DefaultValue("")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fnOnSelected { get; set; }

        [Category("FuncionesJScript"), Description("function javascript que permite detectar la acción del mouse al dar click "), DefaultValue("")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fnOnClick { get; set; }


        

        [Browsable(false)]
        public void SetValue(string Text, string Value)
        {
            txtText.Text = Text;
            txtVal.Text = Value;
        }

        [Browsable(false)]
        public void SetReadOnly()
        {
            txtText.ReadOnly = true;
        }

        Bootstrap oBootstrap = new Bootstrap();
        [Category("Apariencia")]
        [TypeConverter(typeof(Type_Style))]
        [Description("Define estilo vigente para cada control"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public Bootstrap EasyStyle
        {
            get { return oBootstrap; }
            set { oBootstrap = (Bootstrap)value; }
        }

        private string etiqueta;
        [Category("Varios"), Description(""), DefaultValue("")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Etiqueta
        {
            get { return etiqueta; }
            set { etiqueta = value; }
        }

      
        EasyDataInterConect oEasyDataInterConect = new EasyDataInterConect();
        [TypeConverter(typeof(Type_DataInterConect))]
        [Category("Editor"),
           Description("Detalle de la clase usuario."),
           DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public EasyDataInterConect DataInterconect
        {
            get { return oEasyDataInterConect; }
            set { oEasyDataInterConect = value; }
        }



        /*Propiedad de collecion de cadenas*/
        [Browsable(true)]
        private List<EasyControlBE> easyCtrlDepend;
        [Category("Behavior"),
        Description("Lista de Nombre de Controles relacionados"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(EasyCollectionNomControls), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyControlBE> EasyCtrlDepend
        {
            get
            {
                if (easyCtrlDepend == null)
                {
                    easyCtrlDepend = new List<EasyControlBE>();
                }
                return easyCtrlDepend;
            }
        }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Attributes["disabled"] = ((this.Enabled) ? null : "disabled");
            if (this.Enabled == false)
            {
                this.Style["border-color"] = "#C0C0C0";
                this.Style["color"] = "#0000";
            }
             
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            txtID =  this.ID;
            txtText.ID = "Text";
            txtText.Attributes.Add("class", "form-control autocomplete");
            txtText.Attributes.Add("placeholder", ((this.Placeholder==string.Empty)?"Ingrese texto a localizar":this.Placeholder) );
            txtText.Attributes.Add("autocomplete", "off");
            txtText.Attributes.Add("Type", "COMPLETE");
            txtText.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), this.fnOnClick + "(this)");

            if (this.Attributes["required"] != null)
            {
                txtText.Attributes["required"] = "";
            }


            this.Controls.Add(txtText);
            if (!IsDesign())
            {
                txtVal.ID = "Value";
                txtVal.Style.Add("display", "none");
                this.Controls.Add(txtVal);
                //Contendra toda la data del registro seleccionado
                txtRecordSelected.ID = "txtRecordSelected";
                txtRecordSelected.Style.Add("display", "none");
                this.Controls.Add(txtRecordSelected);

            }
        }
        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        public string GetText() { return txtText.Text; }

        public string GetValue() { return (((txtText.Text.Length>0)?txtVal.Text:"")); }


        public void SetData(Dictionary<string, string> RecordSet)
        {
            string strData = "";int i = 0;
            foreach (KeyValuePair<string, string> entry in RecordSet)
            {
                strData += ((i == 0) ? "" : EasyUtilitario.Constantes.Caracteres.Coma.ToString()) + entry.Key + EasyUtilitario.Constantes.Caracteres.DosPuntos + entry.Value;
                i++;
            }
            this.txtRecordSelected.Text="{"+ strData +"}";
        }


        public Dictionary<string,string> GetData() {
            Dictionary<string,string> DataResult= new Dictionary<string,string>();  
            string strData = this.txtRecordSelected.Text.Replace("{","").Replace("}", "");
            if(strData.Length> 0 ) {
                string []arrData = strData.Split(EasyUtilitario.Constantes.Caracteres.Coma.ToCharArray());
                foreach( string strField in arrData ) {
                    string[] FieldData = strField.Split(EasyUtilitario.Constantes.Caracteres.DosPuntos.ToCharArray());
                    DataResult[FieldData[0].ToString()] = FieldData[1].ToString().Replace(EasyUtilitario.Constantes.Caracteres.ComillaDoble,"");
                }
            }
                return DataResult; 
        }

        protected override void Render(HtmlTextWriter writer)
        {
            txtText.BackColor = this.BackColor;
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string AttrParamsCollection = "";
            // base.Render(writer);
            txtText.RenderControl(writer);

           // txtText.Style.Add("display", "none");
            txtVal.Style.Add("display", "none");
            txtRecordSelected.Style.Add("display", "none");



            if (!IsDesign())
            {
                txtVal.RenderControl(writer);
                txtRecordSelected.RenderControl(writer);

                //Agrega el parametro pordefecto del la configuracion del atrubto DisplayText como parte del parametro que debe de estar en primer lugar en el metodo
                EasyFiltroParamURLws oParamMethod = new EasyFiltroParamURLws();
                oParamMethod.ParamName = this.DisplayText;
                oParamMethod.ObtenerValor = EasyFiltroParamURLws.TipoObtenerValor.CriterioInput;
                this.DataInterconect.UrlWebServicieParams.Insert(0, oParamMethod);


                if ((this.DataInterconect.UrlWebServicieParams != null) && (this.DataInterconect.UrlWebServicieParams.Count != 0))
                {
                    foreach (EasyFiltroParamURLws oEasyFiltroParamURLws in this.DataInterconect.UrlWebServicieParams)
                    {
                        string Valor = "";
                        if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.DinamicoPorURL)
                        {
                            Valor = ((System.Web.UI.Page)HttpContext.Current.Handler).Request.Params[oEasyFiltroParamURLws.ParamName].ToString();
                        }
                        else if ((oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.Fijo)|| (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.FunctionScript))
                        {
                            Valor = oEasyFiltroParamURLws.Paramvalue;
                        }
                        else if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.Session)
                        {
                            Valor = ((System.Web.UI.Page)HttpContext.Current.Handler).Session[oEasyFiltroParamURLws.Paramvalue.ToString()].ToString();
                        }
                        else if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.FormControl)
                        {
                            string NomCtrlContext = this.Attributes["CtrlContext"];
                            string NomCtrl = oEasyFiltroParamURLws.Paramvalue;

                            Valor = NomCtrl;
                        }
                        //AttrParamsCollection += "{Name:" + cmll +  oEasyFiltroParamURLws.ParamName + cmll +"," + "Value:" + cmll + Valor + cmll + "," + "TipoParam:" + cmll + oEasyFiltroParamURLws.ObtenerValor.ToString() + cmll + "},";
                        AttrParamsCollection += "{Name:" + cmll + oEasyFiltroParamURLws.ParamName + cmll + "," + "Value:" + cmll + Valor + cmll + "," + "ObtenerValor:" + cmll + oEasyFiltroParamURLws.ObtenerValor.ToString() + cmll + ",Tipo:" + cmll + oEasyFiltroParamURLws.TipodeDato + cmll + "},";
                    }
                    AttrParamsCollection = "[" + AttrParamsCollection.Substring(0, AttrParamsCollection.Length - 1) + "]";
                    txtText.Attributes.Add("ParamCollection", AttrParamsCollection );
                    //txtText.Attributes.Add("ParamCollection", "[" + AttrParamsCollection + "]");
                }


                //txtText.RenderControl(writer);

                string TemplateCustom = ((this.fncTempaleCustom != null) && (this.fncTempaleCustom.Length > 0)) ? "return " + this.fncTempaleCustom + "(ul,item);" : "return $( '<li></li>').data('item.autocomplete', item).append('<a>' + item." + this.DisplayText + @"+ '<br>' + item." + this.ValueField + @" + '</a>').appendTo(ul);";

                    //string fnSelect = ((fnOnSelected != null) && (fnOnSelected.ToString().Length > 0) ? fnOnSelected + "(ui.item." + this.ValueField + ", ui.item)" : "");
                    string fnSelectExterna = ((fnOnSelected != null) && (fnOnSelected.ToString().Length > 0) ? fnOnSelected + "(ItemValue,ItemBE);" : "");
                    string fnSelect = this.ClientID + "_OnSelected(ui.item." + this.ValueField + ", ui.item);";

                    string findOnSelected = "";
                    if ((this.EasyCtrlDepend != null) && (this.EasyCtrlDepend.Count > 0))
                    {
                        string lstCtrl = "";
                        int post = 0;
                        foreach (EasyControlBE oEasyControlBE in this.EasyCtrlDepend)
                        {
                            lstCtrl += ((post == 0) ? "" : "; ") + oEasyControlBE.Nombre;
                            post++;
                        }

                        findOnSelected = @" function " + this.ClientID + @"_OnSelected(ItemValue, ItemBE){
                                                 jNet.get('" + this.txtRecordSelected.ClientID + @"').value = ''.toString().Serialized(ItemBE);
                                                 " + fnSelectExterna + @"
                                                var arrCtrl = '" + lstCtrl + @"'.split(';');
                                                    arrCtrl.forEach(function(str,i){
                                                            var objDep = jNet.get(str);
                                                            var TipoObj = objDep.tagName;
                                                                switch(TipoObj){
                                                                    case 'SELECT':
                                                                        objDep.LoadData();
                                                                        break;
                                                                    default:
                                                                        Valor = obj.Value;
                                                                }
                                                    });
                                    }

                                ";
                    }
                    else
                    {
                        findOnSelected = @" function " + this.ClientID + @"_OnSelected(ItemValue, ItemBE){
                                                    jNet.get('" + this.txtRecordSelected.ClientID + @"').value = ''.toString().Serialized(ItemBE);
                                                 " + fnSelectExterna + @"
                                        }
                                ";
                    }

                 (new LiteralControl("\n <script>\n" + findOnSelected + "\n" + "</script>\n")).RenderControl(writer);

                 
                /* string GetItemSelected = this.ClientID + @".GetItemSelected=function(){
                                                            var strBE =  jNet.get('" + this.txtRecordSelected.ClientID + @"').value;
                                                            return  strBE;
                                                        }";

                (new LiteralControl("\n <script>\n" + GetItemSelected + "\n" + "</script>\n")).RenderControl(writer);*/


                string CadenaConexion = "";
                    EasyDataInterConect.MetododeConexion MetodoConexionSelect = EasyDataInterConect.MetododeConexion.WebServiceInterno;
                    string PageWebService = "";
                    string WSMetoodo = "";
                    if (this.DataInterconect != null)
                    {
                        MetodoConexionSelect = this.DataInterconect.MetodoConexion;
                        PageWebService = this.DataInterconect.UrlWebService;
                        WSMetoodo = this.DataInterconect.Metodo;
                        if (this.DataInterconect.MetodoConexion == EasyDataInterConect.MetododeConexion.WebServiceInterno)
                        {
                            CadenaConexion = EasyUtilitario.Helper.Pagina.PathSite() + this.DataInterconect.UrlWebService + "/" + this.DataInterconect.Metodo;//aqui hay un error
                        }
                        else
                        {
                            CadenaConexion = EasyUtilitario.Helper.Pagina.PathSite() + this.DataInterconect.UrlWebService;//aqui hay un error
                        }

                    }
                //// var CollectionParams" + this.ClientID + @" = eval(jNet.get(" + this.ClientID + @").attr(" + cmll + "ParamCollection" + cmll + @"));
                string JavaScriptCode = @"  
                                            var " + this.ClientID + @"= $(" + cmll + "#" + txtText.ClientID + cmll + @");
                                            var CollectionParams" + this.ClientID + @" = eval('" + AttrParamsCollection + @"');
                                            " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + cmll + " + SIMA.Utilitario.Constantes.ImgDataURL.IconFind + " + cmll + "') right center no-repeat " + cmll + @"); 
                                            " + this.ClientID + @".autocomplete({
                                                                        minLength: " + NroCarIni.ToString() + @",
                                                                        Text: '" + this.DisplayText + @"',//atributos adicionales para el control de display y seleccion
                                                                        Value: '" + this.ValueField + @"',
                                                                        TextSearch: '',
                                                                        DataLoad: null,
                                                                        source: function(request, response) {
                                                                                if(CollectionParams" + this.ClientID + @"==undefined){
                                                                                    " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + cmll + " + SIMA.Utilitario.Constantes.ImgDataURL.IconFind + " + cmll + "') right center no-repeat " + cmll + @"); 
                                                                                    $(" + cmll + "#" + txtText.ClientID + cmll + @").val('Error no se ha definido parámetro de búsqueda');
                                                                                }
                                                                                var cmll = " + cmll + "'" + cmll + @";
                                                                                var rowsData = new Array();
                                                                                var _SeletectValue = this.options.Value;
                                                                                var _SeletectText = this.options.Text;
                                                                                var oParam = null;
                                                                                var oParamCollections = new SIMA.ParamCollections();
                                                                                //CollectionParams se obtiene del atributo assignado al control su formato es {Campo:valor}
                                                                                  CollectionParams" + this.ClientID + @".forEach(function(Param){
                                                                                                                var Valor = Param.Value;
                                                                                                                    if(Param.ObtenerValor=='FormControl'){
                                                                                                                        var objControl = jNet.get(Param.Value);
                                                                                                                        if(objControl.tagName=='SELECT'){//Posible lista de controles soportados
                                                                                                                            var ListItem = objControl.options[objControl.selectedIndex];
                                                                                                                            Valor  = ListItem.value;
                                                                                                                        }
                                                                                                                        else if(objControl.tagName=='INPUT'){
                                                                                                                            Valor = objControl.value;   
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else if(Param.ObtenerValor=='CriterioInput'){
                                                                                                                        Valor = request.term;//Valor a ser localizado
                                                                                                                    }
                                                                                                                    else if(Param.ObtenerValor=='FunctionScript'){
                                                                                                                        eval('Valor=' + Param.Value);
                                                                                                                    }

                                                                                                                oParam = new SIMA.Param(Param.Name,Valor,Param.Tipo); 
                                                                                                                oParamCollections.Add(oParam);
                                                                                                           }); 
                                                                                  //Establece el valor actual en un atributo del control
                                                                                  jNet.get('" + this.txtText.ClientID + @"').value = request.term;
                                                                                if (((this.options.TextSearch != request.term) && (this.options.TextSearch.indexOf(request.term) == -1)) || (request.term.length > this.options.TextSearch.length)){
                                                                                    this.options.TextSearch = request.term;

                                                                                    try{
                                                                                              //Prepara la cadena de conexion  por el objeto interconect
                                                                                              var oEasyDataInterConect = new EasyDataInterConect();
                                                                                                oEasyDataInterConect.MetododeConexion = ModoInterConect." + MetodoConexionSelect.ToString() + @";
                                                                                                oEasyDataInterConect.UrlWebService =  ((" + this.ClientID + ".UrlWebService==null)?'" + PageWebService + @"':" + this.ClientID + ".UrlWebService" + @");
                                                                                                oEasyDataInterConect.Metodo = '" + WSMetoodo + @"';
                                                                                                oEasyDataInterConect.ParamsCollection = oParamCollections;

                                                                                             //Se conecta y Obtiene los datos en formato DataTable
                                                                                             var oEasyDataResult = new EasyDataResult(oEasyDataInterConect);

                                                                                             var oDataTable = new SIMA.Data.DataTable('tbl');
                                                                                                 oDataTable = oEasyDataResult.getDataTable('EasyAutocompletar');
                                                                                                 if(oDataTable!=null){  
                                                                                                         oDataTable.Rows.forEach(function(oDataRow,idr) {
                                                                                                            var strEntity = " + cmll + cmll + @";
                                                                                                            var EntityRst=null;
                                                                                                            oDataRow.Columns.forEach(function(oDataColumn,idc) {
                                                                                                                strEntity += oDataColumn.Name + " + cmll + ":" + cmll + " + cmll + oDataRow[oDataColumn.Name].toString() + cmll + " + cmll + "," + cmll + @";
                                                                                                            });
                                                                                                            strEntity = strEntity.substring(0, strEntity.length - 1);
                                                                                                        
                                                                                                            strEntity = " + cmll + "{" + cmll + " + " + cmll + "label: " + cmll + " + cmll + oDataRow[_SeletectText] + cmll +" + cmll + "," + cmll + " + strEntity + " + cmll + "}" + cmll + @";
                                                                                                            eval(" + cmll + "EntityRst= " + cmll + @" + strEntity);
                                                                                                       
                                                                                                            rowsData[rowsData.length] = new Array();
                                                                                                            rowsData[rowsData.length - 1] = EntityRst;
                                                                                                        });
                                                                                                        this.options.DataLoad = rowsData;
                                                                                                 }
                                                                                    }
                                                                                    catch(oException){
                                                                                        var strexception =''
                                                                                         if (oException instanceof SIMA.WebServiceException){
                                                                                            var msgConfig = { Titulo: 'Error', Descripcion:'Origen:'+  oException.Point + '\n' + 'Mensaje:' + oException.Message};
                                                                                            var oMsg = new SIMA.MessageBox(msgConfig);
                                                                                            oMsg.Alert();
                                                                                         }
                                                                                    }

                                                                                }
                                                                                else
                                                                                {//en caso el criterio sea diferente a lo ya cargado
                                                                                    rowsData = this.options.DataLoad;
                                                                                }

                                                                                if(rowsData.length==0){
                                                                                      " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + cmll + " + SIMA.Utilitario.Constantes.ImgDataURL.IconWriter + " + cmll + "') right center no-repeat " + cmll + @");
                                                                                      jNet.get('" + txtVal.ClientID + @"').value = '';
                                                                                }

                                                                                var retorno=false;
                                                                                response(
                                                                                            $.grep(rowsData, function(item) {
                                                                                                                    return true;
                                                                                                            })
                                                                                         );



                                                                               /* debera de usuarse cuando la data no sea obtenida usando llamadas al servidos y esta sea contruida array JS por el desarrollador
                                                                                response(
                                                                                            $.grep(rowsData, function(item) {
                                                                                                                        var TextFieldResultValue = item." + this.DisplayText + @".toUpperCase();
                                                                                                                        retorno = ((TextFieldResultValue.indexOf(request.term.toUpperCase()) == -1) ? false : true);
                                                                                                                    return retorno;
                                                                                                        })
                                                                                         );*/
                                                                            },
                                                                            search:function(){
                                                                                 " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + cmll + " + SIMA.Utilitario.Constantes.ImgDataURL.IconWriter + " + cmll + "') right center no-repeat " + cmll + @"); 
                                                                            },
                                                                            close: function(event, ui){
                                                                                " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + cmll + " + SIMA.Utilitario.Constantes.ImgDataURL.IconFind + " + cmll + "') right center no-repeat " + cmll + @"); 
                                                                            },
                                                                            open: function() {
                                                                                " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + cmll + " + SIMA.Utilitario.Constantes.ImgDataURL.IconFind + " + cmll + "') right center no-repeat " + cmll + @"); 
                                                                               
                                                                            },
                                                                            focus: function(event, ui) {
                                                                                event.preventDefault();
                                                                                if(ui.item." + this.ValueField + @"==undefined){
                                                                                     var x='Cambiar de color para indicar error';
                                                                                }

                                                                                $(" + cmll + "#" + txtText.ClientID + cmll + @").val(ui.item." + this.DisplayText + @");
                                                                                $(" + cmll + "#" + txtVal.ClientID + cmll + @").val(ui.item." + this.ValueField + @");
                                                                                return false;
                                                                             },
                                                                            select: function (event, ui) {
                                                                                event.preventDefault();

                                                                                if(ui.item." + this.ValueField + @"==undefined){
                                                                                        var msgConfig = { Titulo: 'ValueField', Descripcion: 'El nombre del campo:= [" + this.ValueField + @"] de la propiedad ValueField, no existe en el resultado de la consulta\n por favor modifique o cambie por un nombre valido'};
                                                                                        var oMsg = new SIMA.MessageBox(msgConfig);
                                                                                        oMsg.Alert();
                                                                                }


                                                                                $(" + cmll + "#" + txtText.ClientID + cmll + @").val(ui.item.label);
                                                                                $(" + cmll + "#" + txtVal.ClientID + cmll + @").val(ui.item." + this.ValueField + @");
                                                                                " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + cmll + " + SIMA.Utilitario.Constantes.ImgDataURL.IconFind + " + cmll + "') right center no-repeat " + cmll + @");
                                                                               // try{
                                                                                    " + fnSelect + @";
                                                                                    " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + cmll + " + SIMA.Utilitario.Constantes.ImgDataURL.IconFind + " + cmll + "') right center no-repeat " + cmll + @"); 
                                                                                /*}
                                                                                catch(ex){
                                                                                    alert(ex.message);
                                                                                }*/
                                                                    }
                                                                });
                                                                " + this.ClientID + @".data('autocomplete')._renderItem = function(ul,item){ " + TemplateCustom + @"};
                                                                
                                                                ";

                    (new LiteralControl("\n<script>\n" + JavaScriptCode + "\n" + "</script>\n")).RenderControl(writer);
                    //functiones de lectura
                    string scriptGet = this.ClientID + @".GetValue=function(){
                                                                return jNet.get('" + txtVal.ClientID + @"').value;
                                                            }
                                    " + this.ClientID + @".GetText=function(){
                                                                   return jNet.get('" + txtText.ClientID + @"').value;
                                                            }

                                    " + this.ClientID + @".GetValueOld=function(){
                                                                   return '" + txtVal.Text + @"';
                                                            }

                                    " + this.ClientID + @".GetTextOld=function(){
                                                                   return '" + txtText.Text + @"';
                                                            }

                                    " + this.ClientID + @".SetValue=function(value,text){
                                                                   jNet.get('" + txtText.ClientID + @"').value=text;
                                                                   jNet.get('" + txtVal.ClientID + @"').value = value;
                                                            }
                                    " + this.ClientID + @".Clear=function(){
                                                                   jNet.get('" + txtText.ClientID + @"').value='';
                                                                   jNet.get('" + txtVal.ClientID + @"').value = '';
                                                            }
                                    " + this.ClientID + @".CustomTemplateBE=function(_Ul,_Item,_BodyTemplate){
                                        this.ul =_Ul;
                                        this.item=_Item;
                                        this.BodyTemplate = _BodyTemplate;
                                    }
                                    " + this.ClientID + @".UrlWebService =null;
                                    
                                    " + this.ClientID + @".SetCustomTemplate = function(CustomTemplateBE){
                                                                                         return $('<li></li>').data('item.autocomplete', CustomTemplateBE.item)
                                                                                                                   .append(CustomTemplateBE.BodyTemplate)
                                                                                                                   .appendTo(CustomTemplateBE.ul);
                                                                                   }                                                    

                                    " + this.ClientID + @".GetItemSelected=function(){
                                                                                 var strBE =  jNet.get('" + this.txtRecordSelected.ClientID + @"').value;
                                                                                 var oItemBE = strBE.toString().SerializedToObject();
                                                                                 return  oItemBE;
                                                                             }                                                                                  
                                    ";

                    (new LiteralControl("\n <script>\n" + scriptGet + "\n" + "</script>\n")).RenderControl(writer);



            }

        }

    }



}
