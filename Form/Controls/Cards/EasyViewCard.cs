using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Documents;

namespace EasyControlWeb.Form.Controls.Cards
{
    public  class EasyViewCard : CompositeControl
    {
        #region Variables
        public HtmlGenericControl vContainerLog;
        #endregion



        #region Propiedades
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
        List<EasyViewCardGroup> oEasyCardGroupCollection;

        [Editor(typeof(EasyControlCollection.EasyFormCollectionGroupLogItemEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contiene los Grupos de items Log Generados")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyViewCardGroup> CardGroupCollections
        {
            get
            {
                if (oEasyCardGroupCollection == null)
                {
                    oEasyCardGroupCollection = new List<EasyViewCardGroup>();
                }
                return oEasyCardGroupCollection;
            }
        }

       

        #endregion


        #region Constructor
        public EasyViewCard() : this(String.Empty) { }

        public EasyViewCard(string Formato)
        {
           

            oEasyCardGroupCollection = new List<EasyViewCardGroup>();

        }
        #endregion


        #region Eventos Internos

        HtmlGenericControl HtmlCardContent()
        {
            vContainerLog = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "container py-5");
            vContainerLog.ID = this.ClientID;
            vContainerLog.Controls.Add(HtmlHeadeView());
            HtmlGenericControl vLogRow = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row justify-content-center");
            foreach (EasyViewCardGroup oEasyViewCardGroup in this.oEasyCardGroupCollection)
            {
                vLogRow.Controls.Add(oEasyViewCardGroup);
            }
            vContainerLog.Controls.Add(vLogRow);
            return vContainerLog;
        }

        HtmlGenericControl HtmlHeadeView()
        {
            int NroItems = 0;
            foreach (var item in oEasyCardGroupCollection)
            {
                NroItems += item.CardItems.Count;
            }
            HtmlGenericControl htmlRowHeader = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row align-items-center");

            HtmlGenericControl htmlColTitle = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "col-md-6");
            HtmlGenericControl htmlmb3 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "mb-3");
            HtmlGenericControl h5 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("h5", "card-title");
            h5.Controls.Add(new LiteralControl("<span class='text-muted fw-normal ms-2'>" + this.Titulo + " (" + NroItems.ToString() + ")</span>"));
            htmlmb3.Controls.Add(h5);
            htmlColTitle.Controls.Add(htmlmb3);
            htmlRowHeader.Controls.Add(htmlColTitle);

            HtmlGenericControl htmlColm6 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "col-md-6");
            HtmlGenericControl Flex = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "d-flex flex-wrap align-items-center justify-content-end gap-2 mb-3");
            HtmlGenericControl div = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div");
            div.Controls.Add(new LiteralControl("<a id='" + this.ClientID + "_new' href='#' data-bs-toggle='modal' data-bs-target='.add-new' class='btn btn-primary' " + EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString() + "='" + this.ClientID + ".ToolBarOnClick(this);" + "'><i class='fa fa-plus me-1'></i> Nuevo</a>"));
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
                string OnItemDelete = ((fncItemOnDelete == null) ? "" : fncItemOnDelete + "(obtn);");

                string fcToolbar = ((fcToolBarOnClick == null) ? "" : fcToolBarOnClick + "(obtn);");

                                var scriptViewLogTool = @"<script>
                                            var " + this.ClientID + @"={};
                                                 " + this.ClientID + @".DeleteItem=function(IdItem){
                                                                                        var Item = jNet.get(IdItem);
                                                                                        var Contenedor = jNet.get(Item.attr('IdGrp'));
                                                                                        Contenedor.remove(IdItem);
                                                                                   }
                                                 " + this.ClientID + @".AddItem=function(oEasyItemLog){
                                                                                    var ViewContent = jNet.get('" + this.ClientID + "_ContentPosition" + @"');

                                                                                    }
                                                 " + this.ClientID + @".EditItem=function(oEasyItemLog){
                                                                                var oItemLogTitle = jNet.get('" + this.ClientID + "_Title_" + @"'+ oEasyItemLog.Id);
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
                //Registrar Las Entidades como Controles existtan
                EasyCard oEasyCard = new EasyCard();
                string EasyCardBE = EasyUtilitario.Helper.Data.SeriaizedObjetoToClient(oEasyCard);

                EasyUserButton oEasyUserButton= new EasyUserButton();
                string EasyUserButtonBE = EasyUtilitario.Helper.Data.SeriaizedObjetoToClient(oEasyUserButton);

                string ScriptItemLogBE = @" <script>
                                           " + EasyCardBE + @"
                                           " + EasyUserButtonBE + @"
                                            </script>            
                                        ";
                (new LiteralControl(ScriptItemLogBE)).RenderControl(writer);


                //Renderiza el Control
                HtmlCardContent().RenderControl(writer);
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

