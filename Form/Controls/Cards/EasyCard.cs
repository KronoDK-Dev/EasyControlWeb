using EasyControlWeb.Form.Base;
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

namespace EasyControlWeb.Form.Controls.Cards
{
    public class EasyCard : EasyCardBase
    {
        //variables
        HtmlGenericControl oCardContainer;
        public EasyCard()
        {
            oCollectionsContenido = new List<EasyCardBaseContenido>();
        }
        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fcOnEventCard{ get; set; }



        [Category("Datos"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string JsonData{ get; set; }


        public EasyPanelBase Panel { get; set; }

        public EasyCardToolBar ToolBar { get; set; }



        [Browsable(false)]
        List<EasyCardBaseContenido> oCollectionsContenido;

        [Editor(typeof(EasyControlCollection.EasyFormCollectionCardContenidoEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contiene las lineas de comentarios de un item Card")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyCardBaseContenido> Contenido
        {
            get
            {
                if (oCollectionsContenido == null)
                {
                    oCollectionsContenido = new List<EasyCardBaseContenido>();
                }
                return oCollectionsContenido;
            }
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
        HtmlGenericControl HtmlTitle(string Titulo)
        {
            HtmlGenericControl oRow = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row");
            oRow.Controls.Add(new LiteralControl("<div class='col-12 '><h4 class='card-title '><b>" + Titulo + "</b></h4></div>"));
            return oRow;
        }
        HtmlGenericControl HtmlRowComentarios()
        {
            HtmlGenericControl oRow = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row");
            HtmlGenericControl oCol = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "col");
            foreach (EasyCardBaseContenido oEasyCardBaseContenido in this.Contenido)
            {
                oEasyCardBaseContenido.Attributes["IdGrp"] = this.Attributes["IdGrp"];//Contenedor de los cards 
                oEasyCardBaseContenido.Attributes["IdCard"] = this.ID;
                oEasyCardBaseContenido.Attributes["ObjBE"] = this.Attributes["ObjBE"];
               


                oCol.Controls.Add(oEasyCardBaseContenido);
            }
            oRow.Controls.Add(oCol);
            return oRow;
        }

        HtmlGenericControl HtmlFooter()
        {
           
            HtmlGenericControl oCardFooter = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card-footer bg-white px-0 ");
                HtmlGenericControl orow = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row");
                    //ToolBar
                    this.ToolBar.Attributes["IdObjParent"] = this.ClientID;//Referencia de la card con la barra de herramientas      
                    this.ToolBar.Attributes["IdCard"] = this.ClientID;//Referencia de la card con la barra de herramientas      
                    orow.Controls.Add(this.ToolBar);
                    //Para el Panel Personalizado
                       if (this.Panel is EasyPanelUsers)
                        {
                            EasyPanelUsers oEasyPanelUsers = ((EasyPanelUsers)this.Panel);
                            oEasyPanelUsers.Attributes["IdCard"] = this.ClientID;
                            orow.Controls.Add(oEasyPanelUsers);
                        }
                oCardFooter.Controls.Add(orow);
            return oCardFooter;
        }


        HtmlGenericControl CardBase()
        {
                oCardContainer = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "container");
                //oCardContainer.ID = this.Id.Replace("-", "_");
                oCardContainer.ID = this.ID.Replace("-", "_"); 
                this.ID = this.ID.Replace("-", "_");

            oCardContainer.Attributes["IdGrp"] = this.Attributes["IdGrp"];//Contenedor de los cards 
            oCardContainer.Attributes["ObjBE"] = this.Attributes["ObjBE"];
            oCardContainer.Attributes["JSonData"] = this.JsonData;

            /*string ItemBE = EasyUtilitario.Helper.Data.SeriaizedObjetoToClient(this);
                               oCardContainer.Attributes["ObjBE"] = ItemBE;*/

            HtmlGenericControl oCard = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card mt-5 border-5 pt-2 active pb-0 px-3");
                            HtmlGenericControl oCardBody = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card-body");
                                               oCardBody.Controls.Add(HtmlTitle(this.Titulo));
                                               oCardBody.Controls.Add(HtmlRowComentarios());//Pueden ser Uno o varios
                                        oCard.Controls.Add(oCardBody);
                                        oCard.Controls.Add(HtmlFooter());
                    oCardContainer.Controls.Add(oCard);
            return oCardContainer;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
                string fncScriptEvent = ((this.fcOnEventCard != null) ? this.fcOnEventCard + "(Evento,CtrlSource);" : "");
                //Registra el Control
                this.CardBase().RenderControl(writer);
                string ScriptCard = @"<script>
                                            var " + this.ClientID + @"={};
                                                " + this.ClientID + @".OnCardEvent=function(Evento,CtrlSource){
                                                                                        " + fncScriptEvent + @"
                                                                                    }
                                      </script>
                                    ";
                (new LiteralControl(ScriptCard)).RenderControl(writer);
            }
            else
            {
                (new LiteralControl("UserControlList")).RenderControl(writer);
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
