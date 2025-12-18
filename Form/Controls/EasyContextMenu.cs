using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Markup;
using static EasyControlWeb.EasyUtilitario.Helper;

namespace EasyControlWeb.Form.Controls
{


    [
        AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("LstClass"),
        ParseChildren(true, "LstClass"),
        ToolboxData("<{0}:EasyContextMenu runat=server></{0}:EasyContextMenu")
    ]
    [Serializable]
    public class EasyContextMenu : CompositeControl
    {
        #region Eventos
        
        public delegate void EasyMnuContextButtonClickEventHandler(EasyButtonMenuContext oEasyButtonMenuContexton);
        public event EasyMnuContextButtonClickEventHandler MnuItemButton_Click;
                                          
        #endregion

        #region Variables
        string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string ScriptMenu = "";
        #endregion

        #region Controles
            HtmlButton oBtn;
        
        #endregion

        [Browsable(true)]
        private string lstClass;
        public string LstClass { get { return lstClass; } set { lstClass = value; } }

        [Browsable(true)]
        List<EasyButtonMenuContext> arrEasyMnuButtons;
        [
           Category("Behavior"),
           Description("Colleccion de Botones de extension de funcionalidades"),
           DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
           Editor(typeof(EasyControlCollection.EasyFormCollectionMnuContextButtonEditor), typeof(UITypeEditor)),
           PersistenceMode(PersistenceMode.InnerProperty)
       ]
        public List<EasyButtonMenuContext> EasyMnuButtons
        {
            get { return arrEasyMnuButtons; }
        }


        #region Propiedades Eventos de lado del CLiente

        [Browsable(true)]
        private string fncOnShow;
        public string FncOnShow { get { return fncOnShow; } set { fncOnShow = value; } }

        [Browsable(true)]
        private string fncOnHide;
        public string FncOnHide { get { return fncOnHide; } set { fncOnHide = value; } }


        [Browsable(true)]
        private string fncMnuItem_OnClick;
        public string FncMnuItem_OnClick { get { return fncMnuItem_OnClick; } set { fncMnuItem_OnClick = value; } }

        #endregion





        public EasyContextMenu() : base() {
            arrEasyMnuButtons = new List<EasyButtonMenuContext>();
            oBtn = new HtmlButton();
            oBtn.ID = "CmdCommit";
        }



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
            oBtn.InnerText = "Commit;";
            oBtn.Attributes.Add("runat", "server");
            oBtn.ServerClick += new System.EventHandler(EasyMnuButton_Click);
            oBtn.Style.Add("display", "none");
            this.Controls.Add(oBtn);

            //  this.Controls.Add(tblCard);
        }

        HtmlTable tblScreen() {
            HtmlTable tbl = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(1, 1);
            tbl.Attributes["style"] = "background-color:cornflowerblue;width:" + this.Width + ";border: 1px  dotted #696666;";
            tbl.Rows[0].Cells[0].InnerText = "MenuContext";
            return tbl;
        }

        string ScriptMenuItems() {
            int i = 0;
          
            string sItems ="";
            foreach (EasyButtonMenuContext omnuItem in arrEasyMnuButtons) {
                string icon = "<i class=" + cmll + omnuItem.Icono + cmll + "></i>";
                //string fnc= (((this.fncMnuItem_OnClick != String.Empty)&&(this.fncMnuItem_OnClick != null)) ? this.fncMnuItem_OnClick + "('" +  context.Id + "')," : "null,");
                string fnc = (((this.fncMnuItem_OnClick != String.Empty) && (this.fncMnuItem_OnClick != null)) ? this.fncMnuItem_OnClick  : "null");
                string fncClick = this.ClientID + "_DefaultEventClick({Id:"+ cmll + omnuItem.Id + cmll+ ",RunAtServer:" + cmll + omnuItem.RunAtServer + cmll +",Fnc:" + fnc + " }),";
                sItems += ((i==0)?"":",\n") + @"{
                                                    label:" + cmll + omnuItem.Texto + cmll + @"
                                                    ,icon: '" + icon + @"'
                                                    ,action: () =>" + fncClick + @"
                                                 }," +"\n";
                i++;
            }
            return sItems;

        }

        string _ScriptDefaultButton = "";

        string OnDefaultEventClick() {
            _ScriptDefaultButton = @"function " + this.ClientID + @"_DefaultEventClick(oItembtn){
                                        if(oItembtn.RunAtServer.toUpperCase()=='TRUE'){
                                            __doPostBack('" + oBtn.UniqueID + @"',oItembtn.Id);
                                        }
                                        else{
                                            if(oItembtn.Fnc!=null){
                                                oItembtn.Fnc(oItembtn);
                                            }
                                        }
                                    }
                                    ";
            return _ScriptDefaultButton;
        }

        protected override void Render(HtmlTextWriter writer)
        {        
            if (!IsDesign())
            {
                //Registra la funcion  del item menu
                (new LiteralControl("<script>\n" + OnDefaultEventClick() + "\n</script>")).RenderControl(writer);

                //string lstClass = ".context-area,.btn-context,.context-link";
                string lstClassNames = this.LstClass;
                string fnSHow = (((this.FncOnShow != String.Empty)&& (this.FncOnShow !=null)) ? this.FncOnShow +"();" : "");
                string fnHide = (((this.FncOnHide != String.Empty) && (this.FncOnHide != null)) ? this.FncOnHide + "();" : "");

                ScriptMenu = @" $(" + cmll + lstClassNames + cmll + @").simpleContextMenu(
                                                                        {
                                                                            class: null
                                                                            ,onShow: function () { " + fnSHow + @"}
                                                                            ,onHide: function () { " + fnHide + @"}
                                                                            ,options: [" + ScriptMenuItems() + @"],
                                                                        }
                                                                       );";
                (new LiteralControl("<script>\n" + ScriptMenu + "\n</script>")).RenderControl(writer);
            }
            else
            {
                tblScreen().RenderControl(writer);
            }
        }

        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }


        protected virtual void EasyMnuButton_Click(object sender, EventArgs e)
        {
            HttpRequest ContextRequest = ((System.Web.UI.Page)HttpContext.Current.Handler).Request;
            string strID = ContextRequest["__EVENTARGUMENT"];
            var item = this.EasyMnuButtons.Find(x => x.Id == strID);//Boton seleccionado

            if (MnuItemButton_Click != null) MnuItemButton_Click?.Invoke(item);

        }


     }




    }
