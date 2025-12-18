using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;

using System.Collections.Generic;
using System.Linq; 
using System.Text; 
using System.Web;
using System.Data;
using System.Security.Permissions;
using System.Collections;
using System.Drawing.Design;
using System.Web.UI.HtmlControls;
using EasyControlWeb.Form.Controls;

namespace EasyControlWeb.Form.Controls
{

    [
    AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("ControlParent"),
    ParseChildren(true, "ControlParent"),
    ToolboxData("<{0}:EasyMenuPopup runat=server></{0}:EasyMenuPopup")
  ]
    public class EasyMenuPopup : CompositeControl
    {
        public delegate void onItemClickEventHandler(EasyMenuItem oEasyMenuItem);
        public event onItemClickEventHandler ItemClick;
        public EasyMenuPopup()
        {
            arrEasyMenuItems = new List<EasyMenuItem>();
        }
        #region Variables
            string BlockScript;
            string strItems;
        #endregion

        #region Controles
            HtmlButton btnPostBack = new HtmlButton();
        #endregion

        #region Propiedades
        [Browsable(true)]
            private List<EasyMenuItem> arrEasyMenuItems;
            [
            Category("Behavior"),
            Description("Coleccion de Seccion de grupos de formularios"),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
            Editor(typeof(EasyFormCollectionEditor.EasyFormCollectionEditorMenuItems), typeof(UITypeEditor)),
            PersistenceMode(PersistenceMode.InnerProperty)
            ]
            public List<EasyMenuItem> LstEasyMenuItems
            {
                get
                {
                    return arrEasyMenuItems;
                }
            }

            public string ControlParent { get; set; }
            public string fnMenuOnClick { get; set; }


        #endregion



        protected override void OnInit(EventArgs e)
        {
            if (this.DesignMode == true){}
            strItems = "";
            foreach (EasyMenuItem oMenuItem in this.arrEasyMenuItems) {
                strItems += oMenuItem.ToString(true) + ",";
            }
            if (strItems.Length > 0) { strItems = strItems.Substring(0, strItems.Length - 1); }

            Page.GetPostBackEventReference(this, "MenuPopupPostBack");//Genera PostBack

           
            EasyUtilitario.Helper.Genericos.RegistraBlockScript("History", BlockScript);

        }
        protected override void CreateChildControls()
        {
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
           // Controls.Clear();
         
            btnPostBack = new HtmlButton();
            btnPostBack.ServerClick += EasyMenuPopup_Click;
            btnPostBack.ID = "btnPress";
            btnPostBack.InnerText = "MenuClickItem";
            btnPostBack.Style.Add("display", "none");
            this.Controls.Add(btnPostBack);

            string NombreBtn = btnPostBack.UniqueID;//unico id que sirve para usarlo con el metodo postback
            string EmbebedPostBack = "__doPostBack('" + NombreBtn + "',key);";
            string fnEmbebed = ((this.fnMenuOnClick == string.Empty) ? "" : this.fnMenuOnClick + "(oMenuItem);");
            BlockScript = @"$.contextMenu({
                                            selector: '[id*=" + this.ControlParent + @"]',
                                            className: 'data-title',
                                            callback: function(key, options) {
                                                        var oMenuItem = options.items[key];
                                                        if (oMenuItem.CallEventServer==true){
                                                            
                                                            " + EmbebedPostBack + @"
                                                        }
                                                        else{
                                                        " + fnEmbebed + @"
                                                        }
                                                      },
                                            items:{
                                                    " + strItems + @"
                                                  }
                                            });";
            //CallEventServer

        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
          
            btnPostBack.RenderControl(writer);

            (new LiteralControl("<script>\n" + BlockScript + "\n</script>")).RenderControl(writer);
            if (this.DesignMode == true) {
                HtmlGenericControl dv = new HtmlGenericControl("div");
                dv.InnerText = "SIMA:EasyMenuPopup (Requiere: jquery-3.5.1.min.js, jquery.contextMenu(.css , .js))";
                dv.RenderControl(writer);
            }
        }





        #region Metodos Internos
        protected virtual void EasyMenuPopup_Click(object sender, EventArgs e)
        {
            char MiCar = (char)Convert.ToChar(EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal.ToString());
            string Argument = ((System.Web.UI.Page)HttpContext.Current.Handler).Request["__EVENTARGUMENT"];
            if (Argument.Length > 0)
            {
                string[] arrHistSelect = Argument.Split(MiCar);
                EasyMenuItem oEasyMenuItem = new EasyMenuItem();
                foreach (EasyMenuItem oItem in arrEasyMenuItems) {
                    if (oItem.Id.Equals(Argument)) {
                        oEasyMenuItem = oItem;
                        break;
                    }
                }
                ItemClick?.Invoke(oEasyMenuItem);
            }
        }
        #endregion



    }
}
