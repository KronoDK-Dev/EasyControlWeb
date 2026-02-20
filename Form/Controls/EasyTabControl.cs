using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Security.Permissions;
using System.Web;
using static EasyControlWeb.EasyUtilitario.Helper;
using EasyControlWeb.Filtro;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows;

namespace EasyControlWeb.Form.Controls
{
    public enum EasyTabUbicacion { 
                                    Top,
                                    left,
                                    Right
                                   }

    [
           AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
           AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
           DefaultProperty("fncTabOnClick"),
           ParseChildren(true, "fncTabOnClick"),
           ToolboxData("<{0}:EasyTabControl runat=server></{0}:EasyTabControl")
       ]
    public  class EasyTabControl :CompositeControl
    {
        //rEFERENCIA : https://bootsnipp.com/fullscreen/nP8E7

        #region Declaracion de variables y Objetos de uso interno 
        public HtmlGenericControl ossTab;
        public HtmlGenericControl oTabContent;



        public HtmlGenericControl oGrantTab;
        #endregion


        #region Propiedades

        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncTabOnClick{ get; set; }



        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncTabOnRefresh { get; set; }


        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public EasyTabUbicacion Ubicacion { get; set; } = EasyTabUbicacion.Top;

        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public bool LoadSilent{ get; set; } = false;

        [Browsable(false)]
        List<EasyTabItem> oEasyTabItem;

        [Editor(typeof(EasyControlCollection.EasyFormCollectionTabsItemEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contiene los items de tabs definidos")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyTabItem> TabCollections
        {
            get
            {
                if (oEasyTabItem == null)
                {
                    oEasyTabItem = new List<EasyTabItem>();
                }
                return oEasyTabItem;
            }
        }

        #endregion



        #region Constructor
        public EasyTabControl() : this(String.Empty) { }

        public EasyTabControl(string Formato)
        {
            oEasyTabItem = new List<EasyTabItem>();
        }
        #endregion

        string TabDefault = "";
        string TabsCollections = "";


        #region Fragmento de Control TOP
            HtmlGenericControl HtmlTabControlTop() {
                try
                {
                    if (oEasyTabItem != null)
                    {
                        ossTab = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("ul", "nav nav-tabs");
                        ossTab.ID =  this.ClientID;
                        ossTab.Attributes.Add("role","tablist");

                    
                        foreach (EasyTabItem item in oEasyTabItem)
                        {
                            string classEnebled = ((item.Enabled == true) ? "nav-link" : "nav-link disabled");
                            HtmlGenericControl oText = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a", classEnebled);
                            oText.ID = this.ClientID + "_" + item.Id;
                            //Script que concentra la coleccion de tabs
                            TabsCollections += this.ClientID + @".TabsCollections.Add('" + oText.ID + "');";
                            /************************************************************************************************/
                            oText.Attributes.Add("data-toggle","tab");
                            // oText.Attributes.Add("href", "#C" + oText.ID);
                            oText.Attributes.Add("href", "#" + oText.ID);
                            oText.Attributes.Add("role", "tab");
                            oText.Attributes.Add("Data", item.DataCollection);
                            oText.Attributes.Add("Enabled", ((item.Enabled == true) ?"1":"0"));
                            oText.Attributes.Add("aria-controls", "#C" + this.ClientID + "_" + item.Id);
                            if (item.Selected == true)
                            {
                                oText.Attributes.Add("aria-selected", "true");
                            }
                            if (item.Selected) { TabDefault = oText.ID; }//Tab Seleccionado
                                                                     
                            if (item.AccionRefresh)
                            {
                                HtmlTable tb = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(1, 2);
                                tb.Rows[0].Cells[0].InnerHtml = item.Text;
                                HtmlImage oimg = EasyUtilitario.Helper.HtmlControlsDesign.CrearImagen(EasyUtilitario.Constantes.ImgDataURL.IconRefresh);
                                oimg.Style.Add("width", "16px");
                                oimg.Style.Add("height", "16px");
                                if (item.Enabled == true)
                                {
                                    oimg.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = this.ClientID + ".RefreshTabSelect('" + oText.ID + "')";
                                }

                                tb.Rows[0].Cells[1].Style.Add("padding-left","5px");
                                tb.Rows[0].Cells[1].Controls.Add(oimg);
                                oText.Controls.Add(tb);
                            }
                            else {
                                oText.InnerHtml= item.Text;
                            }
                            oText.Attributes.Add("Loading", "false");
                            oText.Attributes.Add("TipoTab", item.TipoDisplay.ToString());
                            oText.Attributes.Add("Valor", item.Value);
                            oText.Attributes.Add("Params", getParamsForTabs(item));

                            HtmlGenericControl TabItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("li", "nav-item");
                            TabItem.Attributes.Add("role", "presentation");
                            TabItem.Controls.Add(oText);
                        

                            ossTab.Controls.Add(TabItem);   

                        }
                    }
                }
                catch(Exception ex) {
                    return ossTab;
                }
                return ossTab;
            }
            HtmlGenericControl HtmlTabContentTop()
            {
                try
                {
                    if (oEasyTabItem != null)
                    {
                        oTabContent = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "tab-content");
                        oTabContent.ID = "C" + this.ClientID;
                        foreach (EasyTabItem item in oEasyTabItem)
                        {
                            HtmlGenericControl oDivContent = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div");
                            oDivContent.ID = "C" + this.ClientID +"_" + item.Id;
                            if (item.Selected)
                            {
                                oDivContent.Attributes.Add("class", "tab-pane fade show active");
                            }
                            else {
                                oDivContent.Attributes.Add("class", "tab-pane");
                            }
                            oDivContent.Attributes.Add("role","tabpanel");
                            oDivContent.Attributes.Add("aria-labelledby", this.ClientID + "_" + item.Id);
                            //oDivContent.Style.Add("border-style", "ridge");
                            oDivContent.Style.Add("margin", "10px");
                            oDivContent.Style.Add("display", "none");
                            // <div class="tab-pane fade show active" id="home" role="tabpanel" aria-labelledby="home-tab">home</div>
                            // <div class="tab-pane" id="profile" role="tabpanel" aria-labelledby="profile-tab">profile</div>

                            oTabContent.Controls.Add(oDivContent);

                        }
                    }
                }
                catch (Exception ex)
                {
                    oTabContent = new HtmlGenericControl();
                    oTabContent.InnerText = ex.Message;
                    return oTabContent;
                }
                return oTabContent;
            }
        #endregion


        string getParamsForTabs(EasyTabItem _item)
        {
            string ParamURL = "";
            int pos = 0;
            foreach (EasyFiltroParamURLws oParamsUrl in _item.UrlParams)
            {
                string Valor = "";
                if (oParamsUrl.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.DinamicoPorURL)
                {
                    Valor = ((System.Web.UI.Page)HttpContext.Current.Handler).Request.Params[oParamsUrl.ParamName].ToString();
                }
                else if (oParamsUrl.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.FormControl)
                {
                    System.Web.UI.Control oCtrl = ((System.Web.UI.Page)HttpContext.Current.Handler).FindControl(oParamsUrl.Paramvalue);
                    if (oCtrl is EasyNumericBox)
                    {
                        EasyNumericBox onb = (EasyNumericBox)oCtrl;
                        Valor = onb.Text;
                    }
                    if (oCtrl is EasyTextBox)
                    {
                        EasyTextBox otb = (EasyTextBox)oCtrl;
                        Valor = otb.Text;
                    }
                    else if (oCtrl is EasyDatepicker)
                    {
                        EasyDatepicker dpk = (EasyDatepicker)oCtrl;
                        Valor = dpk.Text;
                    }
                    else if (oCtrl is EasyAutocompletar)
                    {
                        EasyAutocompletar AC = (EasyAutocompletar)oCtrl;
                        Valor = AC.GetValue();

                    }
                    else if (oCtrl is EasyDropdownList)
                    {
                        EasyDropdownList ddl = (EasyDropdownList)oCtrl;
                        Valor = ddl.getValue();
                    }
                }
                else if (oParamsUrl.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.Fijo)
                {
                    Valor = oParamsUrl.Paramvalue;
                }
                else if (oParamsUrl.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.Session)
                {
                    string NSesion = oParamsUrl.Paramvalue.ToString().Trim();
                    switch (NSesion)
                    {
                        case "IdUsuario":
                            Valor = ((EasyUsuario)EasyUtilitario.Helper.Sessiones.Usuario.get()).IdUsuario.ToString();
                            break;
                        case "UserName":
                            Valor = ((EasyUsuario)EasyUtilitario.Helper.Sessiones.Usuario.get()).Login;
                            break;
                        case "IdCentro":
                            Valor = ((EasyUsuario)EasyUtilitario.Helper.Sessiones.Usuario.get()).IdCentroOperativo.ToString();
                            break;
                        default:
                            Valor = ((System.Web.UI.Page)HttpContext.Current.Handler).Session[NSesion].ToString();
                            break;
                    }


                }

                else if (oParamsUrl.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.FormControl)
                {
                    string NomCtrlContext = this.Attributes["CtrlContext"];
                    string NomCtrl = oParamsUrl.Paramvalue;
                    Valor = NomCtrl;
                }

                ParamURL += ((pos == 0) ? "" : EasyUtilitario.Constantes.Caracteres.SignoAmperson.ToString()) + oParamsUrl.ParamName + EasyUtilitario.Constantes.Caracteres.SignoIgual.ToString() + oParamsUrl.Paramvalue;
                pos++;
            }
            return ParamURL;
        }

        #region Eventos Propios
        protected override void OnInit(EventArgs e)
        {
           // base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
        }
      
        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
              
               HtmlTabControlTop().RenderControl(writer);
               HtmlTabContentTop().RenderControl(writer);

                string OnRefreshIni = "";
                string OnRefreshFin = "";
                if ((this.fncTabOnRefresh != null) && (this.fncTabOnRefresh.Length > 0)) {
                    OnRefreshIni = this.fncTabOnRefresh + "('Ini');";
                    OnRefreshFin = this.fncTabOnRefresh + "('Fin');";
                }

                string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble.ToString();
                string TabOnclick = ((this.fncTabOnClick != null) ? this.fncTabOnClick + "(oTab);" : "");
                string ScriptTabOnclick = @"<script>
                                                    var " + this.ClientID + @"={};
                                                        " + this.ClientID + @".TabsCollections=new Array();
                                                    var " + this.ClientID + @"_TabSelect = '" + TabDefault + @"'; 
                                                        " + this.ClientID + @".TabActivo =null;
                                                    " + this.ClientID + @".RefreshTabSelect=function(IdTabSelected){
                                                    " + OnRefreshIni + @"
                                                           if (IdTabSelected==undefined){
                                                                IdTabSelected=" + this.ClientID + @"_TabSelect ;
                                                            }
                                                            var oTab = jNet.get(IdTabSelected);
                                                                if(oTab.attr('Enabled')=='0') {return false;}//abandona por estar deshabilitado

                                                                oTab.attr('Loading','false');
                                                                " + this.ClientID + @".OnTab(IdTabSelected);
                                                        }
                                                        " + this.ClientID + @".OnTab=function(IdTabSelected){
                                                                    var oTab = jNet.get(IdTabSelected);
                                                                    if(oTab.attr('Enabled')=='0') {return false;}//abandona por estar deshabilitado

                                                                    " + this.ClientID + @".TabActivo = oTab;
                                                                    var TabContent=jNet.get('C' + IdTabSelected);
                                                                    TabContent.css('display','block');
                                                                    " + this.ClientID + @"_TabSelect = oTab.attr('id');
                                                                    if(oTab.attr('Loading')=='false'){
                                                                            " + OnRefreshIni + @"
                                                                            TabContent.clear();
                                                                            var oColletionParams = new SIMA.ParamCollections();
                                                                            var oParam = null;
                                                                            var NomTabContent = 'C' + oTab.attr('id');
                                                                            var oTipoTab = oTab.attr('TipoTab');
                                                                            var oValor = oTab.attr('Valor');
                                                                            var LstParams = oTab.attr('Params');
                                                                            if(LstParams.length>0){//Verifica si existen parametros
                                                                                oTab.Params=FormParams(LstParams);
                                                                                if(LstParams.length>0){
                                                                                    for (var [key, value] of Object.entries(oTab.Params)) {
                                                                                            oParam = new SIMA.Param(key, value);
                                                                                            oColletionParams.Add(oParam);    
                                                                                    }
                                                                                }      
                                                                            }

                                                                            switch(oTipoTab){
                                                                                case 'UrlLocal':
                                                                                case 'UrlExterna':
                                                                                   SIMA.Utilitario.Helper.Wait('Extendiendo',1000,function(){
                                                                                                                                        var urlPag = ((oTipoTab=='UrlLocal')? Page.Request.ApplicationPath + oValor:oValor);
                                                                                                                                        var oLoadConfig = {
                                                                                                                                                            CtrlName: NomTabContent,
                                                                                                                                                            UrlPage: urlPag,
                                                                                                                                                            ColletionParams: oColletionParams,
                                                                                                                                                            //fnTemplate:function () {},
                                                                                                                                                            fnOnComplete: function () {
                                                                                                                                                                " + OnRefreshFin + @"
                                                                                                                                                            }
                                                                                                                                                           };
                                                                                                                                        SIMA.Utilitario.Helper.LoadPageInCtrl(oLoadConfig);    
                                                                                                                                    });

                                                                                    break;
                                                                                case 'Texto':
                                                                                    jNet.get(NomTabContent).innerHTML = oValor;
                                                                                    break;
                                                                                case 'ContentCtrl':
                                                                                    if(oTab.attr('Loading')=='false'){
                                                                                        var obj= jNet.get(oValor);
                                                                                        obj.css('visibility','visible');
                                                                                        var TbContext = jNet.get(NomTabContent);
                                                                                        //TbContext.css('border-style','dotted');
                                                                                        TbContext.insert(obj);
                                                                                    }
                                                                                    break;
                                                                            }
                                                                        //Tab Cargado
                                                                        oTab.attr('Loading','true');
                                                                    }
                                                                    //Ejecuta la funcion Script relacionada al control
                                                                    " + TabOnclick + @"
                                                                 
                                                    }
                                            </script>
                                            <script>
                                                    " + TabsCollections + @"
                                                    " + this.ClientID + @".TabsCollections.forEach(function(IdTab,i){
                                                                                   // " + this.ClientID + @".OnTab(IdTab);
                                                                                    jNet.get('C'+IdTab).css('display','none');
                                                                                });
                                            </script>
                                            <script>
                                              
                                                $('#" + this.ClientID + @" a').on('click', function (e) {
                                                                                            e.preventDefault();
                                                                                            var _idTab = jNet.get(this).attr('id');
                                                                                            " + this.ClientID + @".TabsCollections.forEach(function(IdTab,i){
                                                                                                                            if(_idTab==IdTab){
                                                                                                                                jNet.get('C'+IdTab).css('display','block');
                                                                                                                            }
                                                                                                                            else{
                                                                                                                                jNet.get('C'+IdTab).css('display','none');
                                                                                                                            }                                                                                                                            
                                                                                                                        });
                                                                                           " + this.ClientID + @".OnTab(_idTab);
                                                                                           $(this).tab('show');
                                                                                           //Graba en el log el tab seleccionado
                                                                                           localStorage.setItem(UsuarioBE.UserName + '" + this.ClientID + @"', _idTab);
                                                                                   });

                                                    Manager.Task.Excecute(function () {
                                                                                var idSelectLog = localStorage.getItem(UsuarioBE.UserName +'" + this.ClientID +  @"');
                                                                                var cmlls =" + cmll +  EasyUtilitario.Constantes.Caracteres.ComillaSimple.ToString() + cmll + @";
                                                                                cmlls ='';
                                                                                var Idtab= ((idSelectLog==null)?  " + this.ClientID + @"_TabSelect:idSelectLog);
                                                                                try{
                                                                                    " + this.ClientID + @".OnTab(Idtab);
                                                                                      $('.nav-tabs a[href=" + cmll + "#" + "' + Idtab + '" + cmll + @"]').tab('show');

                                                                                }
                                                                                catch(err){
                                                                                   // alert(err);
                                                                                }
                                                                        }, 1000,true);
                                              
                                           </script>
                                           ";

                (new LiteralControl(ScriptTabOnclick)).RenderControl(writer);

            }
            else
            {
                (new LiteralControl("TabControl")).RenderControl(writer);
            }
        }



        #region Metodos de Apoyo
        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        #endregion

    }
}
