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
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Windows.Markup;
using System.Windows.Controls;

namespace EasyControlWeb.Form.Controls
{
    [
           AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
           AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
           DefaultProperty("PathFoto"),
           ParseChildren(true, "PathFoto"),
           ToolboxData("<{0}:EasyCardPerfil runat=server></{0}:EasyCardPerfil")
       ]

    public class EasyCardPerfil: CompositeControl
    {

        public HtmlTable tblCard;

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Data"), Description("Ruta y Foto"), DefaultValue("")]
        public string PathFoto{ get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Data"), Description("Apellidos y Nombres"), DefaultValue("")]
        public string ApellidosyNombres{ get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Data"), Description("Area"), DefaultValue("")]
        public string Area { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Data"), Description("Correo electronico"), DefaultValue("")]
        public string Email { get; set; }


        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Style"), Description("Estilo de clase para el titulo"), DefaultValue("")]
        public string CssClassLine1 { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Style"), Description("Estilo de clase para el titulo"), DefaultValue("")]
        public string CssClassLine2 { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Style"), Description("Estilo de clase para el titulo"), DefaultValue("")]
        public string CssClassLine3 { get; set; }


        public EasyCardPerfil() { }

        #region eventos de diseño
        protected override void OnInit(EventArgs e)
        {
            if (this.DesignMode == true)
            {
                this.EnsureChildControls();
            }                                                                                 // Page.GetPostBackEventReference(this, "LoginPostBack");//Genera PostBack
        }
        protected override void CreateChildControls()
        {
            Controls.Clear();
         

          //  this.Controls.Add(tblCard);
        }
        HtmlTable Pintar() {
            tblCard = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(3, 2);
            tblCard.Style.Add("Width", "100%");
            tblCard.ID = "CardPerfil";
            tblCard.Attributes["class"] = this.CssClass;

            string _bkColor= System.Drawing.ColorTranslator.ToHtml(this.BackColor); 
            tblCard.Rows[0].Style.Add("background-color", _bkColor);
            tblCard.Rows[1].Style.Add("background-color", _bkColor);
            tblCard.Rows[2].Style.Add("background-color", _bkColor);

            string _BorderStyle = this.BorderWidth.ToString() + " " + this.BorderStyle.ToString() + " " + System.Drawing.ColorTranslator.ToHtml(this.BorderColor); 
            tblCard.Style.Add("border", _BorderStyle);

            tblCard.Rows[0].Cells[0].RowSpan = 3;
            tblCard.Rows[1].Cells[0].Style.Add("display", "none");tblCard.Rows[2].Cells[0].Style.Add("display", "none");

            HtmlImage oimg = new HtmlImage();
            oimg.Attributes["Width"] = "50px";
            oimg.Attributes["class"] = "ms-n2 rounded-circle img-fluid";
            oimg.Attributes["src"] = this.PathFoto;

            tblCard.Rows[0].Cells[0].Controls.Add(oimg);
            tblCard.Rows[0].Cells[0].Style.Add("padding-left", "15px");

            tblCard.Rows[0].Cells[1].Attributes["class"] = this.CssClassLine1;
            tblCard.Rows[0].Cells[1].InnerText = this.ApellidosyNombres;
            tblCard.Rows[0].Cells[1].Style.Add("padding-top", "10px");
            tblCard.Rows[0].Cells[1].Style.Add("padding-right", "15px");
            tblCard.Rows[0].Cells[1].Style.Add("Width", "90%");

            tblCard.Rows[1].Cells[1].Attributes["class"] = this.CssClassLine2;
            tblCard.Rows[1].Cells[1].InnerText = this.Area;
            //tblCard.Rows[1].Cells[1].Style.Add("padding-top", "5px");
            tblCard.Rows[1].Cells[1].Style.Add("padding-right", "15px");

            tblCard.Rows[2].Cells[1].Attributes["class"] = this.CssClassLine3;
            tblCard.Rows[2].Cells[1].InnerText =this.Email;
            tblCard.Rows[2].Cells[1].Style.Add("padding-bottom", "10px");
            tblCard.Rows[2].Cells[1].Style.Add("padding-right", "15px");
            return tblCard;
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
                Pintar().RenderControl(writer);
            }
            else
            {
                (new LiteralControl("Card Perfil")).RenderControl(writer);
            }
        }

        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        /*----------------------------------------------------------------------------------------------------------------*/
        #endregion
    }
}
