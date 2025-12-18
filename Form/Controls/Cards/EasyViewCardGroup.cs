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

namespace EasyControlWeb.Form.Controls.Cards
{
    public class EasyViewCardGroup: CompositeControl
    {
        #region Constructor
        public EasyViewCardGroup() : this(String.Empty) { }

        public EasyViewCardGroup(string _Titulo)
        {
            oEasyCardCollection = new List<EasyCard>();
        }
        #endregion

       /* [Category("indentificacion"), PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(true)]
        [Description("Id")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Id { get; set; }*/

        [Category("indentificacion"), PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(true)]
        [Description("Titulo")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Titulo { get; set; }


       

        [Category("Tool"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public EasyPathHistory PathHistory { get; set; }



        [Browsable(false)]
        List<EasyCard> oEasyCardCollection;

        [Editor(typeof(EasyControlCollection.EasyFormCollectionCardItemEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contiene los items Cards Generados")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyCard> CardItems
        {
            get
            {
                if (oEasyCardCollection == null)
                {
                    oEasyCardCollection = new List<EasyCard>();
                }
                return oEasyCardCollection;
            }
        }


        HtmlGenericControl HtmlCardGroup() {
            HtmlGenericControl vLogGroupCardShadow = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card shadow-sm mb-4");
            vLogGroupCardShadow.Style.Add("width", "100%");
            HtmlGenericControl vLogCardBody = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card-body");
            if (this.PathHistory != null)
            {
                vLogCardBody.Controls.Add(this.PathHistory);
            }

            #region Contenido de los cards
            HtmlGenericControl vPosition = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "position-relative");
            //vPosition.ID = this.Id+ "_ContentPosition";
            vPosition.ID = this.ID + "_ContentPosition";

            foreach (EasyCard oEasyCard in this.CardItems)
            {
                oEasyCard.Attributes["IdGrp"] = vPosition.ID;
                string JsonBE= new JavaScriptSerializer().Serialize(oEasyCard).Replace(EasyUtilitario.Constantes.Caracteres.ComillaDoble, EasyUtilitario.Constantes.Caracteres.ComillaSimple); ;
                oEasyCard.Attributes["ObjBE"]= JsonBE;

                vPosition.Controls.Add(oEasyCard);
            }
            vLogCardBody.Controls.Add(vPosition);
            #endregion

            vLogGroupCardShadow.Controls.Add(vLogCardBody);

            return vLogGroupCardShadow;

        }



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
                //Registra el Control
                HtmlCardGroup().RenderControl(writer);
            }
            else
            {
                (new LiteralControl("UserCardView")).RenderControl(writer);
            }
        }

        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }


    }
}
