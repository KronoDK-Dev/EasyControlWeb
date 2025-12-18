using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Controls
{
    public enum TipoItemLog
    {
        [Description("fw-bold text-success")]
        success,
        [Description("fw-bold text-danger")]
        danger,
        [Description("fw-bold text-warning")]
        warning,
    }
    /*Como usarlo
     * MyEnum myLocal = MyEnum.V1;
        print(myLocal.ToDescriptionString());
     */



    [
           AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
           AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
           DefaultProperty("fncItembtnOnClick"),
           ParseChildren(true, "fncItembtnOnClick"),
           ToolboxData("<{0}:EasyViewLog runat=server></{0}:EasyViewLog")
    ]

    public class EasyViewLog : CompositeControl
    {
        #region Variables
            public HtmlGenericControl vContainerLog;
        #endregion




        #region Propiedades
            [Category("Scripts"), Description("")]
            [Browsable(true)]
            [RefreshProperties(RefreshProperties.All)]
            [NotifyParentProperty(true)]
            public string fncItembtnOnClick { get; set; }

            [Category("Scripts"), Description("")]
            [Browsable(true)]
            [RefreshProperties(RefreshProperties.All)]
            [NotifyParentProperty(true)]
            public string fncItemOnDelete { get; set; }

            [Category("Scripts"), Description("")]
            [Browsable(true)]
            [RefreshProperties(RefreshProperties.All)]
            [NotifyParentProperty(true)]
            public string fcToolBarOnClick { get; set; }



        [Category("Datos"), Description("")]
            [Browsable(true)]
            [RefreshProperties(RefreshProperties.All)]
            [NotifyParentProperty(true)]
            public string Titulo { get; set; }


        [Browsable(false)]
            List<EasyViewLogGroup> oEasyGroupCollection;

            [Editor(typeof(EasyControlCollection.EasyFormCollectionGroupLogItemEditor), typeof(UITypeEditor))]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            [Description("Contiene los Grupos de items Log Generados")]
            [Category("Behaviour"),
             PersistenceMode(PersistenceMode.InnerProperty)
            ]

            [RefreshProperties(RefreshProperties.All)]
            [NotifyParentProperty(true)]
            public List<EasyViewLogGroup> LogGroupCollections
            {
                get
                {
                    if (oEasyGroupCollection == null)
                    {
                        oEasyGroupCollection = new List<EasyViewLogGroup>();
                    }
                    return oEasyGroupCollection;
                }
            }

            /*Para la barra de herramientas en el footer*/
            [Browsable(false)]
            List<EasyViewLogButtom> oEasyLogButton;

            [Editor(typeof(EasyControlCollection.EasyFormCollectionLogItemButtomEditor), typeof(UITypeEditor))]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            [Description("Contiene los botones de cada item log")]
            [Category("Behaviour"),
             PersistenceMode(PersistenceMode.InnerProperty)
            ]

            [RefreshProperties(RefreshProperties.All)]
            [NotifyParentProperty(true)]
            public List<EasyViewLogButtom> ButtonCollections
            {
                get
                {
                    if (oEasyLogButton == null)
                    {
                        oEasyLogButton = new List<EasyViewLogButtom>();
                    }
                    return oEasyLogButton;
                }
            }


        #endregion


        #region Constructor
        public EasyViewLog() : this(String.Empty) { }

        public EasyViewLog(string Formato)
        {
            oEasyLogButton = new List<EasyViewLogButtom>();
            oEasyGroupCollection = new List<EasyViewLogGroup>();

        }
        #endregion


        #region Eventos Internos
        HtmlGenericControl btnEdit(EasyItemLog oEasyItemLog) {
            //Clase de boton de edición
            EasyViewLogButtom oEasyViewLogButtom = new EasyViewLogButtom();
                oEasyViewLogButtom.Id = "Edit";
                oEasyViewLogButtom.Icono = "";

            HtmlGenericControl htmlDiv = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "media-body");
                HtmlGenericControl htmla = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a", "card-link text-primary read-more");
                htmla.Attributes["href"] = "#";
                htmla.Attributes["IdBtn"] = oEasyViewLogButtom.Id;
                htmla.Attributes["Btn"] = new JavaScriptSerializer().Serialize(oEasyViewLogButtom).Replace(EasyUtilitario.Constantes.Caracteres.ComillaDoble, EasyUtilitario.Constantes.Caracteres.ComillaSimple);
                htmla.Attributes["ItemLog"] = new JavaScriptSerializer().Serialize(oEasyItemLog).Replace(EasyUtilitario.Constantes.Caracteres.ComillaDoble, EasyUtilitario.Constantes.Caracteres.ComillaSimple);
                htmla.Controls.Add(new LiteralControl("Editar"));

            htmla.ID = this.ClientID + "_" + oEasyItemLog.Id + "_" + oEasyViewLogButtom.Id;
                htmla.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = this.ClientID + ".ViewLogOnclick(this);";
            htmlDiv.Controls.Add(htmla);
            return htmlDiv;
        }
        HtmlGenericControl htmlToolBar(EasyItemLog oEasyItemLog) {
            HtmlGenericControl htmlTool =  EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card-footer");
            HtmlGenericControl htmlubicaCenter= EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "media align-items-center");
            htmlTool.Controls.Add(htmlubicaCenter);
            htmlubicaCenter.Controls.Add(btnEdit(oEasyItemLog));

            HtmlGenericControl htmlubicaRight = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "footerright");
            foreach (EasyViewLogButtom obtn in this.ButtonCollections) {
                    HtmlGenericControl htmtnlink3 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "tnlink3");
                    HtmlGenericControl htmI = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("i");
                        htmI.Attributes["class"] = obtn.Icono;
                        htmI.Attributes["aria-hidden"] = "true";
                        htmI.Style.Add("cursor", "pointer");
                        htmI.Attributes["Btn"] = new JavaScriptSerializer().Serialize(obtn).Replace(EasyUtilitario.Constantes.Caracteres.ComillaDoble, EasyUtilitario.Constantes.Caracteres.ComillaSimple); 
                        htmI.Attributes["ItemLog"] = new JavaScriptSerializer().Serialize(oEasyItemLog).Replace(EasyUtilitario.Constantes.Caracteres.ComillaDoble, EasyUtilitario.Constantes.Caracteres.ComillaSimple);
                        htmI.ID = this.ClientID + "_"+ oEasyItemLog.Id + "_" + obtn.Id;
                        htmI.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = this.ClientID +".ViewLogOnclick(this)";
                htmtnlink3.Controls.Add(htmI);
                htmlubicaRight.Controls.Add(htmtnlink3);
            }
            htmlubicaCenter.Controls.Add(htmlubicaRight);
            return htmlTool;
        }
        HtmlGenericControl HtmlItemLog(EasyItemLog oEasyItemLog)
        {
            HtmlGenericControl vItemLog = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "changelog-item mb-3");
            vItemLog.ID = this.ClientID + "_" + oEasyItemLog.Id;
            HtmlGenericControl vLineTime = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "timeline-line");
            vItemLog.Controls.Add(vLineTime);
            HtmlGenericControl vTitle = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("h6", oEasyItemLog.Tipo.ToDescription());
            vTitle.Controls.Add(new LiteralControl(oEasyItemLog.Titulo));
            vTitle.ID = this.ClientID + "_Title_" + oEasyItemLog.Id;
            vItemLog.Controls.Add(vTitle);

            HtmlGenericControl vParrafo = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("p");
            vParrafo.Controls.Add(new LiteralControl(oEasyItemLog.Descripcion));
            vParrafo.ID = this.ClientID + "_Parrafo_" + oEasyItemLog.Id;

            vItemLog.Controls.Add(vParrafo);
            //Agreagr la Barra de herramientas
            vItemLog.Controls.Add(htmlToolBar(oEasyItemLog));
            return vItemLog;
        }

        HtmlGenericControl HtmlGrupoLog(EasyViewLogGroup oEasyViewLogGroup)
        {
            // <div class= style="width:100%">
            HtmlGenericControl vLogGroupCardShadow = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card shadow-sm mb-4");
                vLogGroupCardShadow.Style.Add("width", "100%");
                HtmlGenericControl vLogCardBody = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card-body");
                if (oEasyViewLogGroup.PathHistory != null)
                {
                    vLogCardBody.Controls.Add(oEasyViewLogGroup.PathHistory);
                }
                HtmlGenericControl vPosition = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "position-relative");
                               vPosition.ID = this.ClientID + "_ContentPosition";
           
                    foreach (EasyItemLog oEasyItemLog in oEasyViewLogGroup.LogItems)
                    {
                        vPosition.Controls.Add(HtmlItemLog(oEasyItemLog));
                    }
            vLogCardBody.Controls.Add(vPosition);
            vLogGroupCardShadow.Controls.Add(vLogCardBody);

            return vLogGroupCardShadow;
        }
        HtmlGenericControl HtmlLogContent()
        {
            vContainerLog = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "container py-5");
            vContainerLog.ID = this.ClientID;
            vContainerLog.Controls.Add(HtmlHeadeView());
                HtmlGenericControl vLogRow = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row justify-content-center");
                foreach (EasyViewLogGroup oEasyViewLogGroup in this.oEasyGroupCollection) {
                    vLogRow.Controls.Add(HtmlGrupoLog(oEasyViewLogGroup));
                }
                vContainerLog.Controls.Add(vLogRow);
            return vContainerLog;
        }

        HtmlGenericControl HtmlHeadeView() {
            int NroItems = 0;
            foreach (var item in LogGroupCollections)
            {
                NroItems += item.LogItems.Count;
            }
            HtmlGenericControl htmlRowHeader = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row align-items-center");

                    HtmlGenericControl htmlColTitle = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "col-md-6");
                        HtmlGenericControl htmlmb3 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "mb-3");
                            HtmlGenericControl h5 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("h5", "card-title");
                                    h5.Controls.Add(new LiteralControl("<span class='text-muted fw-normal ms-2'>"+ this.Titulo + " ("+ NroItems.ToString() + ")</span>"));
                        htmlmb3.Controls.Add(h5);
                    htmlColTitle.Controls.Add(htmlmb3);
                htmlRowHeader.Controls.Add(htmlColTitle);

                    HtmlGenericControl htmlColm6 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "col-md-6");
                        HtmlGenericControl Flex = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "d-flex flex-wrap align-items-center justify-content-end gap-2 mb-3");
                            HtmlGenericControl div = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div");
                                                div.Controls.Add(new LiteralControl("<a id='" + this.ClientID + "_new' href='#' data-bs-toggle='modal' data-bs-target='.add-new' class='btn btn-primary' "+ EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString() +"='" + this.ClientID +".ToolBarOnClick(this);" + "'><i class='fa fa-plus me-1'></i> Nuevo</a>"));
                        Flex.Controls.Add(div);
                    htmlColm6.Controls.Add(Flex);

                htmlRowHeader.Controls.Add(htmlColm6);

            return htmlRowHeader;
        }

        #endregion

        #region eventos naturales del control
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
                //string ToolbtnItemLog = ((fncItembtnOnClick == null) ? "" : fncItembtnOnClick + "(IdBtn,oEasyItemLog);");
                string ToolbtnItemLog = ((fncItembtnOnClick == null) ? "" : fncItembtnOnClick + "(oBtnBE,oEasyItemLog);");
                string OnItemDelete = ((fncItemOnDelete == null) ? "" : fncItemOnDelete + "(obtn);");

                string fcToolbar = ((fcToolBarOnClick == null) ? "" : fcToolBarOnClick + "(obtn);");

                var scriptViewLogTool = @"<script>
                                            var " + this.ClientID + @"={};
                                                " + this.ClientID + @".ViewLogOnclick=function(e){
                                                                                         event.preventDefault()
                                                                                          var obtn = jNet.get(e);
                                                                                         // var IdBtn=obtn.attr('IdBtn');
                                                                                          var oBtnBE = obtn.attr('Btn').toString().SerializedToObject();
                                                                                          var oEasyItemLog = obtn.attr('ItemLog').toString().SerializedToObject();
                                                                                          " + ToolbtnItemLog + @"
                                                                                        }
                                                 " + this.ClientID + @".DeleteItem=function(IdItemLog){
                                                                                        var ViewContent = jNet.get('" + this.ClientID + "_ContentPosition" + @"');
                                                                                        var ItemLog = jNet.get('" + this.ClientID + @"_' + IdItemLog);
                                                                                        ViewContent.remove(ItemLog.attr('id'));
                                                                                     }
                                                 " + this.ClientID + @".AddItem=function(oEasyItemLog){
                                                                                    var ViewContent = jNet.get('" + this.ClientID + "_ContentPosition" + @"');

                                                                                    }
                                                 " + this.ClientID + @".EditItem=function(oEasyItemLog){
                                                                                var oItemLogTitle = jNet.get('" + this.ClientID +"_Title_" + @"'+ oEasyItemLog.Id);
                                                                                    oItemLogTitle.innerHTML = oEasyItemLog.Titulo;
                                                                                var oItemLogParrafo = jNet.get('" + this.ClientID + "_Parrafo_" + @"'+ oEasyItemLog.Id);
                                                                                    oItemLogParrafo.innerHTML = oEasyItemLog.Descripcion;
                                                                                }
                                                " + this.ClientID + @".ToolBarOnClick=function(e){
                                                                                            var obtn=jNet.get(e);
                                                                                            " + fcToolbar + @"
                                                                                        }

                                          </script>";
                //Renderiza el script
                (new LiteralControl(scriptViewLogTool)).RenderControl(writer);

                EasyItemLog oEasyItemLog = new EasyItemLog();
                string ItemLogBE = EasyUtilitario.Helper.Data.SeriaizedObjetoToClient(oEasyItemLog);
                string ScriptItemLogBE = @" <script>
                                           " + ItemLogBE + @"
                                            </script>            
                                        ";
                (new LiteralControl(ScriptItemLogBE)).RenderControl(writer);

                //Renderiza el Control
                HtmlLogContent().RenderControl(writer);
            }
        }

        #endregion
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
