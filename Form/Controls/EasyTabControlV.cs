using EasyControlWeb.Filtro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Resources;

namespace EasyControlWeb.Form.Controls
{
    /*
     * texto vertical :https://stackoverflow.com/questions/4264527/vertical-text-direction
     */
    public class EasyTabControlV : CompositeControl
    {
        #region Declaracion de variables y Objetos de uso interno 
            public HtmlGenericControl ossTab;
            public HtmlGenericControl oTabContent;

            public HtmlGenericControl oGrantTab;

            private string TabDefault = "";
            string TabsCollections = "";
        #endregion

        #region Propiedades

        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncTabOnClick { get; set; }


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
            public EasyTabControlV() : this(String.Empty) { }

            public EasyTabControlV(string Formato)
            {
                oEasyTabItem = new List<EasyTabItem>();
            }
        #endregion

       
       


        HtmlGenericControl HmtlTabControlBaseUbicacion()
        {
            oGrantTab = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row");
            //oGrantTab.Style.Add("border", "5px dotted red");
            oGrantTab.Style.Add("padding", "0");
            oGrantTab.Style.Add("height", "800px");


            oGrantTab.ID = this.ClientID + "_Grant";
            // oGrantTab.Attributes.Add("role", "tablist");
            HtmlGenericControl divColsm10 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "col-sm-12");
           // divColsm10.Style.Add("border", "5px dotted blue");
            divColsm10.Style.Add("padding", "0px");
            divColsm10.Style.Add("margin", "0px");
            divColsm10.Style.Add("width", "100%");
            divColsm10.Style.Add("height", "100%");

            divColsm10.Controls.Add(HtmlTabControlHeader());
            divColsm10.Controls.Add(HtmlTabControlBody());

            oGrantTab.Controls.Add(divColsm10);
            return oGrantTab;
        }
        HtmlGenericControl HtmlTabControlHeader()
        {
            //Contenedor de los tabs
            HtmlGenericControl divColxs3 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "col-xs-2");
            //divColxs3.Style.Add("border-style", "dotted");
           // divColxs3.Style.Add("border", "2px dotted violet");
            divColxs3.Style.Add("height", "100%");
            divColxs3.Style.Add("padding", "0px");
            divColxs3.Style.Add("margin", "0px");
            divColxs3.Style.Add("left", "0px");
            divColxs3.Style.Add("width", "100px");

            HtmlGenericControl TabHeader = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("ul", "nav nav-tabs tabs-left vertical-text");
            //TabHeader.Style.Add("border", "2px dotted red");
          //  TabHeader.Style.Add("padding", "0px");
          //  TabHeader.Style.Add("margin", "0px");
            TabHeader.Style.Add("left", "0px");
            TabHeader.Style.Add("width", "120px");

            foreach (EasyTabItem item in oEasyTabItem)
            {
                HtmlGenericControl oTab = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("li");
                //oTab.Style.Add("padding-Left", "5px");
               // oTab.Style.Add("margin-top", "0px");
               // oTab.Style.Add("margin-left", "0px");
                oTab.Style.Add("left", "-40px");
                //oTab.Style.Add("top", "50px");


                HtmlGenericControl oText = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a");
                oText.ID = this.ClientID + "_" + item.Id;
                oText.Attributes.Add("href", "#" + "C_" + this.ClientID + "_" + item.Id);
                oText.Attributes.Add("data-toggle", "tab");
                oText.Attributes.Add("Data", item.DataCollection);
                oText.Attributes.Add("Enabled", ((item.Enabled == true) ? "1" : "0"));
                oText.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), this.ClientID + ".onTabClick(this)");

                TabsCollections += this.ClientID + @".TabsCollections.Add('" + oText.ID + "');";
                if (item.Selected == true)
                {
                    oTab.Attributes.Add("class", "active");
                    TabDefault = oText.ID;
                }

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

                    tb.Rows[0].Cells[1].Style.Add("padding-left", "5px");
                    tb.Rows[0].Cells[1].Controls.Add(oimg);
                    oText.Controls.Add(tb);
                }
                else
                {
                    oText.InnerHtml = item.Text;
                }
                oText.Attributes.Add("Loading", "false");
                oText.Attributes.Add("TipoTab", item.TipoDisplay.ToString());
                oText.Attributes.Add("Valor", item.Value);
                oText.Attributes.Add("Params", getParamsForTabs(item));

                oTab.Controls.Add(oText);



                TabHeader.Controls.Add(oTab);
            }
            divColxs3.Controls.Add(TabHeader);
            return divColxs3;
        }


        HtmlGenericControl HtmlTabControlBody()//Continua mañana
        {
            HtmlGenericControl divColxs9 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "col-xs-12");//Aqui se tiene que considerar para mostrar los card ordenados
            //divColxs9.Style.Add("border", "2px dotted #000");
            divColxs9.Style.Add("margin", "0");
            divColxs9.Style.Add("padding", "0px");
            divColxs9.Style.Add("left", "50px");
            divColxs9.Style.Add("top", "-800px");
            //divColxs9.Style.Add("width", "700px");
            //divColxs9.Style.Add("width", "100%");
            //divColxs9.Style.Add("height", "100%");



            HtmlGenericControl TabBody = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "tab-content");
           // TabBody.Style.Add("border", "2px dotted orange");

            foreach (EasyTabItem item in oEasyTabItem)
            {
                string Class = ((item.Selected == true) ? "tab-pane active" : "tab-pane");

                HtmlGenericControl oTabContent = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", Class);
                oTabContent.ID = "C_" + this.ClientID + '_'  + item.Id;
                //oTabContent.InnerText = item.Text;
                // oTabContent.Style.Add("border-style","dotted");

                TabBody.Controls.Add(oTabContent);
            }
            divColxs9.Controls.Add(TabBody);
            return divColxs9;
        }



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

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
                HmtlTabControlBaseUbicacion().RenderControl(writer);
              
                string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble.ToString();
                string TabOnclick = ((this.fncTabOnClick != null) ? this.fncTabOnClick + "(oTab);" : "");
                string ScriptTabOnclick = @"<script>
                                               var " + this.ClientID + @"={};
                                               " + this.ClientID + @".TabsCollections=new Array();
                                               " + this.ClientID + @".TabActivo =null;
                                               " + this.ClientID + @".onTabClick=function(e){
                                                                                       // event.preventDefault();
                                                                                        var oTab = jNet.get(e);
                                                                                        //Establece el tab activo
                                                                                        " + this.ClientID + @".TabActivo = oTab;
                                                                                        " + this.ClientID + @".onTabSelected(oTab);

                                                                                        var LstParams = oTab.attr('Params');
                                                                                        oTab.Params=FormParams(LstParams);
                                                                                        var oTipoTab = oTab.attr('TipoTab');
                                                                                        var oValor = oTab.attr('Valor');
                                                                                        var NomTabContent='C_' + oTab.attr('id');
                                                                                        //var TabContent=jNet.get(NomTabContent);
                                                                                        //TabContent.clear();

                                                                                        

                                                                                         if(oTab.attr('Loading')=='false'){
                                                                                                //Parametros para invocar a
                                                                                                var oColletionParams = new SIMA.ParamCollections();
                                                                                                var oParam = null;
                                                                                                
                                                                                                if(LstParams.length>0){
                                                                                                         for (var [key, value] of Object.entries(oTab.Params)) {
                                                                                                                oParam = new SIMA.Param(key, value);
                                                                                                                oColletionParams.Add(oParam);    
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
                                                                                                                                                                                    //fnOnComplete: function () {}
                                                                                                                                                                                   };
                                                                                                                                                                SIMA.Utilitario.Helper.LoadPageInCtrl(oLoadConfig);    
                                                                                                                                                            });

                                                                                                            break;
                                                                                                        case 'Texto':
                                                                                                            jNet.get(NomTabContent).innerHTML = oValor;
                                                                                                            break;
                                                                                                        case 'ContentCtrl':
                                                                                                            var obj= jNet.get(oValor);
                                                                                                            jNet.get(NomTabContent).insert(obj);
                                                                                                            break;
                                                                                                    }

                                                                                             oTab.attr('Loading','true');
                                                                                             //Ejecuta la funcion Script relacionada al control
                                                                                             " + TabOnclick + @"
                                                                                        }



                                               }
                                           </script>
                                           ";

                (new LiteralControl(ScriptTabOnclick)).RenderControl(writer);

                string ScriptTabsCollecion = @"  <script>
                                                    " + TabsCollections + @"
                                                    " + this.ClientID + @".TabsCollections.forEach(function(IdTab,i){
                                                                                    //jNet.get('C'+IdTab).css('display','none');
                                                                                });
                                            </script>";

                (new LiteralControl(ScriptTabsCollecion)).RenderControl(writer);

                string ScriptTabActive = @"<script>
                                                " + this.ClientID + @".onTabSelected = function(oTab) {
                                                                                            var IdTab = oTab.attr('id');
                                                                                            " + this.ClientID + @".TabsCollections.forEach(function(_IdTab,i){
                                                                                                                            var oTabLI = jNet.get(jNet.get(_IdTab).parentNode);
                                                                                                                            
                                                                                                                            if(_IdTab==IdTab){
                                                                                                                                oTabLI.attr('class','active');
                                                                                                                            }
                                                                                                                            else{
                                                                                                                                oTabLI.removeAttr('class');
                                                                                                                            }                                                                                                                            
                                                                                                                        });
                                                                                           localStorage.setItem(UsuarioBE.UserName + '" + this.ClientID + @"', IdTab);
                                                                                        }
                                           </script>";

                (new LiteralControl(ScriptTabActive)).RenderControl(writer);
                
                string ScriptTabDefault = @"<script>
                                                Manager.Task.Excecute(function () {
                                                                var IdSelectInLog = localStorage.getItem(UsuarioBE.UserName +'" + this.ClientID + @"');
                                                                var cmlls =" + cmll +  EasyUtilitario.Constantes.Caracteres.ComillaSimple.ToString() + cmll + @";
                                                                cmlls ='';
                                                                var Idtab= ((IdSelectInLog==null)?'" + TabDefault + @"':IdSelectInLog);
                                                                //try{
                                                                    var oTabSelected =jNet.get(Idtab);
                                                                    " + this.ClientID + @".onTabClick(oTabSelected);
                                                                        $('.nav-tabs a[href=" + cmll + "#" + "' + Idtab + '" + cmll + @"]').tab('show');

                                                                /*}
                                                                catch(err){
                                                                    alert(err);
                                                                }*/
                                                        }, 1000,true);
                                         </script>";
                (new LiteralControl(ScriptTabDefault)).RenderControl(writer);

            }
            else
            {
                (new LiteralControl("TabControlV")).RenderControl(writer);
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
