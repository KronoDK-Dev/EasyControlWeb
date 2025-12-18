using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Web.UI.HtmlControls;
using static EasyControlWeb.EasyUtilitario;
using System.Drawing;

namespace EasyControlWeb.Form.Controls
{

    public enum PathStyle { 
            Basico
            ,Tradicional
    }

    [
           AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
           AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
           DefaultProperty("fncPathOnClick"),
           ParseChildren(true, "fncPathOnClick"),
           ToolboxData("<{0}:EasyPathHistory runat=server></{0}:EasyPathHistory")
       ]


    public  class EasyPathHistory : CompositeControl
    {
        #region Variables
            public HtmlGenericControl ULContentPath;
          
        #endregion


        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncPathOnClick { get; set; }


        [Category("Configuracion"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public PathStyle TipoPath{ get; set; }


        [Category("Configuracion"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public bool PathHome { get; set; }
        
        [Browsable(false)]
        List<EasyPathItem> oEasyPathItems;

        [Editor(typeof(EasyControlCollection.EasyFormCollectionPathsItemEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contiene los items de Paths definidos")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyPathItem> PathCollections
        {
            get
            {
                if (oEasyPathItems == null)
                {
                    oEasyPathItems = new List<EasyPathItem>();
                }
                return oEasyPathItems;
            }
        }


        HtmlGenericControl HtmlPathContent()
        {
            
            ULContentPath = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("ul", (((this.CssClass == null) || (this.CssClass.Length == 0)) ? "PathHistory" : this.CssClass));
            try
            {
                if ((this.oEasyPathItems != null) && (this.oEasyPathItems.Count != 0))
                {
                    //ULContentPath = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("ul", (((this.CssClass == null) || (this.CssClass.Length == 0)) ? "PathHistory" : this.CssClass));
                    ULContentPath.ID = this.ClientID;
                    if (this.PathHome)
                    {
                        EasyPathItem oPathHome = new EasyPathItem();
                        oPathHome.Id = this.ClientID + "_Home";
                        oPathHome.ClassName = "fa fa-home";
                        ULContentPath.Controls.Add(HtmlPathItem(oPathHome));
                    }
                    foreach (EasyPathItem item in this.oEasyPathItems)
                    {
                        ULContentPath.Controls.Add(HtmlPathItem(item));
                    }

                }
            }
            catch(Exception ex){
                EasyPathItem oPathHome = new EasyPathItem();
                oPathHome.Id = this.ClientID + "_Home";
                oPathHome.ClassName = "fa fa-home";
                ULContentPath.Controls.Add(HtmlPathItem(oPathHome));
            }
            return ULContentPath;
        }
        HtmlGenericControl HtmlPathItem(EasyPathItem oEasyPathItem) {

            HtmlGenericControl oText = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a");
            oText.Attributes.Add("href", "#");
            oText.Attributes.Add("Descripcion", oEasyPathItem.Descripcion);
            HtmlGenericControl oSpan = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", oEasyPathItem.ClassName);
            oText.Controls.Add(oSpan);
            oText.Controls.Add(new LiteralControl(oEasyPathItem.Titulo));
            
            HtmlGenericControl oLiPath = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("li");
            oLiPath.Controls.Add(oText);

            return oLiPath;
        }

        /*Tipo Modelo 2*/
        HtmlGenericControl htmlPathContenV2() {
            HtmlGenericControl HtmlContainer = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "container ScrollPath");
            HtmlContainer.Attributes["style"] = "width: " + this.Width + "px;";
                HtmlGenericControl HtmlDivRow = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row");
                    HtmlDivRow.Controls.Add(HtmlPathItemCollectionV2());
            HtmlContainer.Controls.Add(HtmlDivRow);
            return HtmlContainer;
        }
        string strLstPathItems = "";
        HtmlGenericControl HtmlPathItemCollectionV2() {
        
            HtmlGenericControl HtmlDivGroup = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "btn-group btn-breadcrumb");
            HtmlDivGroup.ID = this.ClientID + "_" + "BarPath";
            if (this.PathHome) {
                     HtmlGenericControl HtmlBtnHome = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "btn btn-primary");
                     HtmlBtnHome.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(),"var oPathItem=  new " + this.ClientID + ".PathItem('" + this.ClientID + @"_Home','','','btn btn-primary');    "+ this.fncPathOnClick+ "(oPathItem);");
                     HtmlBtnHome.Controls.Add(new LiteralControl("<i class='fa fa-home'>"));
                    strLstPathItems = @"oItem = new " + this.ClientID + ".PathItem('" + this.ClientID + @"_Home','','','btn btn-primary');
                                       " + this.ClientID + @".ColletionPath.Add(oItem);
                                    ";
                 HtmlDivGroup.Controls.Add(HtmlBtnHome);
            }
            if (this.oEasyPathItems != null)
            {
                foreach (EasyPathItem item in this.oEasyPathItems)
                {
                    HtmlDivGroup.Controls.Add(HtmlPathItemV2(item));
                    strLstPathItems += @" oItem = new " + this.ClientID + ".PathItem('" + this.ClientID + item.Id + "','" + item.Titulo + "','" + item.Descripcion + "','" + "btn btn-default" + @"');
                                       " + this.ClientID + @".ColletionPath.Add(oItem);
                                    ";
                }
            }
            return HtmlDivGroup;
        }

        HtmlGenericControl HtmlPathItemV2(EasyPathItem oItemPath) {
            HtmlGenericControl HtmlBtn = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "btn btn-default");
            HtmlBtn.InnerHtml = oItemPath.Titulo;
            return HtmlBtn;
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
                                    
            protected override void Render(HtmlTextWriter writer)
            {
                if (!IsDesign())
                {
                    if (this.TipoPath == PathStyle.Basico)
                    {
                        this.HtmlPathContent().RenderControl(writer);
                    }
                    else { 
                        this.htmlPathContenV2().RenderControl(writer);
                    string EventClickPath = ((fncPathOnClick != null) && (fncPathOnClick.Length > 0) ? fncPathOnClick + "(PathBE);" : "");

                    string scrV2 = @"<script>
                                            var oItem=null;
                                            var " + this.ClientID + @"={};
                                            " + this.ClientID + @".ColletionPath=new Array();
                                            " + this.ClientID + @".TipoItem={Home:'btn btn-primary',Default:'btn btn-default'}
                                            " + this.ClientID + @".PathItem=function(_ID,_TITULO,_DESCRIPCION,_TIPOPATH){
                                                    this.Id = _ID;
                                                    this.Titulo=_TITULO;
                                                    this.Descripcion=_DESCRIPCION;
                                                    this.ClassName=_TIPOPATH;
                                                }

                                             " + strLstPathItems + @" 
                                             " + this.ClientID + @"_PathContent =  jNet.get('" + this.ClientID + @"_BarPath');
                                             " + this.ClientID + @".Clear=function(){
                                                  " + this.ClientID + @"_PathContent.clear();
                                                  " + this.ClientID + @".ColletionPath.Clear();
                                             }
                                             " + this.ClientID + @".Add=function(oPathItem){
                                                        if(" + this.ClientID + @".Find(oPathItem)==false){
                                                            " + this.ClientID + @".ColletionPath.Add(oPathItem);
                                                        }
                                             }
                                             " + this.ClientID + @".Binding=function(oPathItem){
                                                        var htmlPathItem = jNet.create('div');
                                                            var objPath=''.toString().BaseSerialized(oPathItem);
                                                            htmlPathItem.attr('Data',objPath);
                                                            htmlPathItem.addEvent('click', function () {
                                                                    var YoBtn = jNet.get(this);
                                                                    var PathBE =  YoBtn.attr('Data').toString().SerializedToObject();
                                                                    " + EventClickPath + @"
                                                             });
                                                                htmlPathItem.attr('class',oPathItem.ClassName);
                                                                if(oPathItem.ClassName==" + this.ClientID + @".TipoItem.Home){
                                                                    var oI = jNet.create('div');
                                                                        oI.attr('class','fa fa-home');
                                                                    htmlPathItem.insert(oI);
                                                                }
                                                                else{
                                                                    htmlPathItem.innerHTML=oPathItem.Titulo;
                                                                }
                                                            " + this.ClientID + @"_PathContent.insert(htmlPathItem);
                                             }
                                             " + this.ClientID + @".Find=function(oPathItem){
                                                     var Existe=false;
                                                     " + this.ClientID + @".ColletionPath.forEach(function(PathItem,i){
                                                                           if(oPathItem.Id == PathItem.Id){
                                                                                Existe=true;                   
                                                                           }
                                                       });

                                                    return Existe;
                                             }

                                             " + this.ClientID + @".Render=function(){

                                                     " + this.ClientID + @".ColletionPath.forEach(function(PathItem,i){
                                                                           " + this.ClientID + @".Add(PathItem);
                                                        });
                                             }
                                              " + this.ClientID + @".Paint=function(){
                                                     " + this.ClientID + @".ColletionPath.forEach(function(PathItem,i){
                                                                           " + this.ClientID + @".Binding(PathItem);
                                                        });
                                             }

                                    </script>";

                    (new LiteralControl(scrV2)).RenderControl(writer);


                }
                }
                else
                {
                    (new LiteralControl("PathHistoryControl")).RenderControl(writer);
                }
            }

        #endregion

        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }


    }
}
