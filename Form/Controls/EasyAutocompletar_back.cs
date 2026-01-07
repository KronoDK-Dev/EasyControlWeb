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
        TextBox txtText;
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

        

        [Browsable(false)]
        public void SetValue(string Text, string Value)
        {
            txtText.Text = Text;
            txtVal.Text = Value;
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
            //txtText.ID = txtID + "Text";
            txtText.ID = "Text";
            txtText.Attributes.Add("class", "form-control autocomplete");
            txtText.Attributes.Add("placeholder", ((this.Placeholder==string.Empty)?"Ingrese texto a localizar":this.Placeholder) );
            txtText.Attributes.Add("autocomplete", "off");
           
            this.Controls.Add(txtText);
            if (!IsDesign())
            {
                //txtVal.ID = txtID + "Value";
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

        public string GetValue() { return txtVal.Text; }


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
            // base.Render(writer);
            if (!IsDesign())
            {
                txtVal.RenderControl(writer);
                txtRecordSelected.RenderControl(writer);
            }
            txtText.BackColor = this.BackColor;
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string AttrParamsCollection = "";
            if (!IsDesign())
            { //Agrega el parametro pordefecto del la configuracion del atrubto DisplayText como parte del parametro que debe de estar en primer lugar en el metodo
                EasyFiltroParamURLws oParamMethod = new EasyFiltroParamURLws();
                oParamMethod.ParamName = this.DisplayText;
                oParamMethod.ObtenerValor = EasyFiltroParamURLws.TipoObtenerValor.CriterioInput;
                this.DataInterconect.UrlWebServicieParams.Insert(0,oParamMethod);
            }

            if ((this.DataInterconect.UrlWebServicieParams != null) && (this.DataInterconect.UrlWebServicieParams.Count != 0))
            {
                    foreach (EasyFiltroParamURLws oEasyFiltroParamURLws in this.DataInterconect.UrlWebServicieParams)
                    {
                        string Valor = "";
                        if (oEasyFiltroParamURLws.ObtenerValor== EasyFiltroParamURLws.TipoObtenerValor.DinamicoPorURL)
                        {
                            Valor = ((System.Web.UI.Page)HttpContext.Current.Handler).Request.Params[oEasyFiltroParamURLws.ParamName].ToString();
                        }
                        else if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.Fijo)
                        {
                            Valor = oEasyFiltroParamURLws.Paramvalue;
                        }
                        else if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.FormControl)
                        {
                           // string NomContext = "";
                            string NomCtrlContext = this.Attributes["CtrlContext"];
                            string NomCtrl = oEasyFiltroParamURLws.Paramvalue;
                        /*  if ((NomCtrlContext != "")&&(NomCtrlContext!=null))
                          {
                              NomContext = NomCtrlContext +"_"+ NomCtrl +"Text";
                          }
                          else
                          {
                              NomContext = this.ClientID.Replace(this.ID, "") + NomCtrl;
                          }
                          Valor = NomContext;*/
                            Valor = NomCtrl;
                        }
                    //AttrParamsCollection += "{Name:" + cmll +  oEasyFiltroParamURLws.ParamName + cmll +"," + "Value:" + cmll + Valor + cmll + "," + "TipoParam:" + cmll + oEasyFiltroParamURLws.ObtenerValor.ToString() + cmll + "},";
                    AttrParamsCollection += "{Name:" + cmll + oEasyFiltroParamURLws.ParamName + cmll + "," + "Value:" + cmll + Valor + cmll + "," + "ObtenerValor:" + cmll + oEasyFiltroParamURLws.ObtenerValor.ToString() + cmll + "},";
                }
                    AttrParamsCollection = AttrParamsCollection.Substring(0, AttrParamsCollection.Length - 1);
                    txtText.Attributes.Add("ParamCollection", "[" + AttrParamsCollection + "]");
                }
            txtText.RenderControl(writer);

            if (!IsDesign())
            {
                string TemplateCustom = ((this.fncTempaleCustom!=null)&&(this.fncTempaleCustom.Length > 0)) ? "return " + this.fncTempaleCustom + "(ul,item);" : "return $( '<li></li>').data('item.autocomplete', item).append('<a>' + item." + this.DisplayText + @"+ '<br>' + item." + this.ValueField + @" + '</a>').appendTo(ul);";

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
                                                var strTranslate = '';
                                                    jNet.get('" + this.txtRecordSelected.ClientID + @"').value = strTranslate.Serialized(ItemBE);
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
                                                    var strTranslate = '';
                                                    jNet.get('" + this.txtRecordSelected.ClientID + @"').value = strTranslate.Serialized(ItemBE);
                                                 " + fnSelectExterna + @"
                                    }
                                ";
                }

             (new LiteralControl("\n <script>\n" + findOnSelected + "\n" + "</script>\n")).RenderControl(writer);





                string CadenaConexion = "";
                if (this.DataInterconect.MetodoConexion == EasyDataInterConect.MetododeConexion.WebServiceeLocal)
                {
                    CadenaConexion = EasyUtilitario.Helper.Pagina.PathSite() + this.DataInterconect.UrlWebService + "/" + this.DataInterconect.Metodo;//aqui hay un error
                }
                else
                {
                    CadenaConexion = EasyUtilitario.Helper.Pagina.PathSite() + this.DataInterconect.UrlWebService;//aqui hay un error
                }

                //string CadenaConexion = EasyUtilitario.Helper.Pagina.PathSite() + this.DataInterconect.UrlWebService + "/" + this.DataInterconect.Metodo;//aqui hay un error
                string JavaScriptCode = @"  var " + this.ClientID + @"= $(" + cmll + "#" + txtText.ClientID + cmll + @");
                                            var CollectionParams" + this.ClientID + @" = eval(" + this.ClientID + @".attr(" + cmll + "ParamCollection" + cmll + @"));
                                          //imagen base
                                            " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconFind + "') right center no-repeat " + cmll + @"); 
                                            " + this.ClientID + @".autocomplete({
                                                                        minLength: " + NroCarIni.ToString() + @",
                                                                        Text: '" + this.DisplayText + @"',//atributos adicionales para el control de display y seleccion
                                                                        Value: '" + this.ValueField + @"',
                                                                        TextSearch: '',
                                                                        DataLoad: null,
                                                                        source: function(request, response) {
                                                                                if(CollectionParams" + this.ClientID + @"==undefined){
                                                                                    " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconFind + "') right center no-repeat " + cmll + @"); 
                                                                                    $(" + cmll + "#" + txtText.ClientID + cmll + @").val('Error no se ha definido parámetro de búsqueda');
                                                                                }
                                                                                var cmll = " + cmll + "'" + cmll + @";
                                                                                var rowsData = new Array();
                                                                                var _SeletectValue = this.options.Value;
                                                                                var _SeletectText = this.options.Text;
                                                                                var oParam = null;
                                                                                var oParamCollections = new SIMA.Data.OleDB.ParamCollections();
                                                                                //Adiciona a la Collection el parametro de busqueda ingresada
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
                                                                                                                oParam = new SIMA.Data.OleDB.Param(Param.Name,Valor);
                                                                                                                oParamCollections.Add(oParam);
                                                                                                           });
                                                                                if (((this.options.TextSearch != request.term) && (this.options.TextSearch.indexOf(request.term) == -1)) || (request.term.length > this.options.TextSearch.length))
                                                                                {//para evitar volver a cargar del lado del servidor
                                                                                    this.options.TextSearch = request.term;//asigna para luego comparar en la llamada al response()
                                                                                    var OleDBCommand = new SIMA.Data.OleDB.Command();
                                                                                    OleDBCommand.CadenadeConexion = '" + CadenaConexion + @"';
                                                                                    var oDataTable = new SIMA.Data.DataTable('tbl');
                                                                                    oDataTable = OleDBCommand.ExecuteDataSet(oParamCollections);
                                                                                    if(oDataTable!=null){
                                                                                        oDataTable.Rows.forEach(function(oDataRow,idr) {
                                                                                            var strEntity = " + cmll + cmll + @";
                                                                                            oDataRow.Columns.forEach(function(oDataColumn,idc) {
                                                                                                strEntity += oDataColumn.Name + " + cmll + ":" + cmll + " + cmll + oDataRow[oDataColumn.Name].toString() + cmll + " + cmll + "," + cmll + @";
                                                                                            });
                                                                                            strEntity = strEntity.substring(0, strEntity.length - 1);
                                                                                            strEntity = " + cmll + "{" + cmll + " + " + cmll + "label: " + cmll + " + cmll + oDataRow[_SeletectText] + cmll +" + cmll + "," + cmll + " + strEntity + " + cmll + "}" + cmll + @";
                                                                                            eval(" + cmll + "var Entity= " + cmll + @" + strEntity);
                                                                                            rowsData[rowsData.length] = new Array();
                                                                                            rowsData[rowsData.length - 1] = Entity;
                                                                                        });
                                                                                        this.options.DataLoad = rowsData;
                                                                                    }
                                                                                    else{
                                                                                        this.options.DataLoad = null;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {//en caso el criterio sea diferente a lo ya cargado
                                                                                    rowsData = this.options.DataLoad;
                                                                                }

                                                                                if(rowsData.length==0){
                                                                                      " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconWriter + "') right center no-repeat " + cmll + @");
                                                                                      jNet.get('" + txtVal.ClientID + @"').value = '';
                                                                                }

                                                                                var retorno=false;
                                                                                response(
                                                                                            $.grep(rowsData, function(item) {
                                                                                                                        var TextFieldResultValue = item." + this.DisplayText + @".toUpperCase();
                                                                                                                        retorno = ((TextFieldResultValue.indexOf(request.term.toUpperCase()) == -1) ? false : true);
                                                                                                                    return retorno;
                                                                                                        })
                                                                                         );
                                                                            },
                                                                            search:function(){
                                                                                 " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + EasyUtilitario.Constantes.ImgDataURL.ImgLoandig + "') right center no-repeat " + cmll + @"); 
                                                                            },
                                                                            close: function(event, ui){
                                                                                " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconFind + "') right center no-repeat " + cmll + @"); 
                                                                            },
                                                                            open: function() {
                                                                                " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconFind + "') right center no-repeat " + cmll + @"); 
                                                                               /* 
                                                                                $('.ui-widget-content').css('background', '" + ColorTranslator.ToHtml(this.BackColor) + @"');*/
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
                                                                                " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconFind + "') right center no-repeat " + cmll + @");
                                                                                try{
                                                                                    " + fnSelect + @";
                                                                                    " + this.ClientID + @".css(" + cmll + "background" + cmll + "," + cmll + "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconFind + "') right center no-repeat " + cmll + @"); 
                                                                                }
                                                                                catch(ex){
                                                                                    alert(ex.message);
                                                                                }
                                                                    }
                                                                });
                                                                " + this.ClientID + @".data('autocomplete')._renderItem = function(ul,item){ " + TemplateCustom + @"};
                                                                
                                                                ";



                /*
                 * .append('<a>' + item." + this.DisplayText + @"+ '<br>' + item." + this.ValueField + @" + '</a>')
                 


                   obj" + this.ClientID + @"Searh.data('autocomplete')._renderItem = function(ul,item){
                                                                                                                                         return $( '<li></li>').data('item.autocomplete', item)
                                                                                                                                                                .append('<a><table><tr><td><a><img  src=  " + cmll + cmll + @"></a></td>'
                                                                                                                                                                           +     '<td>' + item." + this.DisplayText + @"+ '<br>' + item." + this.ValueField + @" + '</td>'
                                                                                                                                                                           +     '<td><label>Qty:</label>'
                                                                                                                                                                           +         '<button  type=" + cmll +"button" + cmll + " class=" + cmll + "primary" + cmll + @">Update Count</button>'
                                                                                                                                                                           +     '</td>'
                                                                                                                                                                           + '</tr>'
                                                                                                                                                                          +'</table></a>')
                                                                                                                                                              .appendTo(ul);
                                                                                                                                                };



                 */



                (new LiteralControl("\n<script>\n" + JavaScriptCode + "\n" + "</script>\n")).RenderControl(writer);
                //functiones de lectura
                string scriptGet =  this.ClientID + @".GetValue=function(){
                                                                return jNet.get('" + txtVal.ClientID + @"').value;
                                                            }
                                    " + this.ClientID + @".GetText=function(){
                                                                   return jNet.get('" + txtText.ClientID + @"').value;
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
                                    " + this.ClientID + @".SetCustomTemplate = function(CustomTemplateBE){
                                                                                         return $('<li></li>').data('item.autocomplete', CustomTemplateBE.item)
                                                                                                                   .append(CustomTemplateBE.BodyTemplate)
                                                                                                                   .appendTo(CustomTemplateBE.ul);
                                                                                   }                                                    
                                    ";
                (new LiteralControl("\n <script>\n" + scriptGet + "\n" + "</script>\n")).RenderControl(writer);
            }
        }

    }



}
