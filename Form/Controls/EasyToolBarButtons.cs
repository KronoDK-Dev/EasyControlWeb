using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Web; 
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Controls
{
    [
        AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("EasyButtons"),
        ParseChildren(true, "EasyButtons"),
        ToolboxData("<{0}:EasyToolBarButtons runat=server></{0}:EasyToolBarButtons")
      ]
    public class EasyToolBarButtons : CompositeControl
    {


        #region Event Handler
        public delegate void EasButtonClick(EasyButton oEasyButton);
        public event EasButtonClick onClick;
        #endregion

        #region Constantes
        const string SCR_TOOLBAR_CLICK_ITEM = "_ToolBar_Onclick";
        #endregion 

        #region Variables
            Table tblToolBar;
            HtmlButton oBtn;
            List<LiteralControl> _Scripts = new List<LiteralControl>();

        #endregion
        #region Propiedades
            [Browsable(true)]
            List<EasyButton> arrEasyButtons;
            [
               Category("Behavior"),
               Description("Colleccion de Botones de extension de funcionalidades"),
               DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
               Editor(typeof(EasyControlCollection.EasyFormCollectionButtons), typeof(UITypeEditor)),
               PersistenceMode(PersistenceMode.InnerProperty)
           ]
            public List<EasyButton> EasyButtons
            {
                get { return arrEasyButtons; }
            }
        
            [TypeConverter(typeof(FontSizeTypeConverter)),
            Description("Esta es una propiedad de cadena que tiene una colección de valores estándar definida en su TypeConverter (FontSizeTypeConverter)." +
                " La cuadrícula de propiedades utiliza esta colección para crear una lista y proporcionarla al usuario para que la seleccione.")]
            public string BtnFontSize { get; set; }

            public string BtnClassName
            {
                get
                {
                    if (this.ViewState["btnClassName"] == null)
                    {
                        this.ViewState["btnClassName"] = "btn btn-primary";
                    }
                    return (string)this.ViewState["btnClassName"];
                }
                set
                {
                    this.ViewState["btnClassName"] = value;
                }
            }
            public string BtnTextColor
            {
                get
                {
                    if (this.ViewState["btnTextColor"] == null)
                    {
                        this.ViewState["btnTextColor"] = "white";
                    }
                    return (string)this.ViewState["btnTextColor"];
                }
                set
                {
                    this.ViewState["btnTextColor"] = value;
                }
            }


        [Category("Client Event"), Description("esta function permitira ejecutarse antes que se ejecuta el evento clik del lado del servidor del boton presionado")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public string fnPreOnServerButtonClick
        {
            get;set;
        }

        [Category("Client Event"), Description("esta function deberá ser definida de esta manera OnEasyToolbarButton_Click(btn). Lo utilizarán aquellos botones cuya propiedad RunAtServer=false")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public string fnToolBarButtonClick
        {
            get
            {
               if (this.ViewState["ToolBarButtonClick"] == null)
                {
                    this.ViewState["ToolBarButtonClick"] = "OnEasyToolbarButton_Click";
                }

                return (string)this.ViewState["ToolBarButtonClick"];
            }
            set
            {
                this.ViewState["ToolBarButtonClick"] = value;
            }
        }
        #endregion

        public EasyToolBarButtons()
        {
            arrEasyButtons = new List<EasyButton>();
        }

        #region Metodos internos
        void CrearTableToolBar()//Definicion de la tabla que contendra los botones
        {
            tblToolBar = new Table();
            tblToolBar.Style.Add("width", "100%");
            tblToolBar.Attributes["border"] = "0px";
            TableRow trToolBar = new TableRow();
            if (IsDesign())
            {
                tblToolBar.Attributes.Add("border", "1px");
                trToolBar.BorderStyle = BorderStyle.Dotted;
                trToolBar.BorderColor = System.Drawing.Color.FromArgb(127, 127, 127);
            }

                TableCell tcToolBar = new TableCell();
                tcToolBar.Style.Add("width", "43%");
                tcToolBar.Attributes["border"]= "0px";

                tcToolBar.Attributes.Add("align","left");
                //tcToolBar.Attributes.Add("align", "right");
                tcToolBar.Style.Add("white-space", "pre-wrap");
                tcToolBar.Style.Add("word-wrap", "break-word");
            trToolBar.Controls.Add(tcToolBar);

                tcToolBar = new TableCell();
                tcToolBar.Style.Add("width", "10%");
                tcToolBar.Attributes.Add("align", "justify");
                //tcToolBar.Style.Add("white-space", "pre-wrap");
                tcToolBar.Style.Add("word-wrap", "break-word");
                tcToolBar.Style.Add("white-space", "nowrap");
            trToolBar.Controls.Add(tcToolBar);

                tcToolBar = new TableCell();
                tcToolBar.Style.Add("width", "43%");
                tcToolBar.Style.Add("white-space", "pre-wrap");
                tcToolBar.Style.Add("word-wrap", "break-word");
                tcToolBar.Attributes.Add("align", "right");
            trToolBar.Controls.Add(tcToolBar);

            tblToolBar.Controls.Add(trToolBar);

            Controls.Add(tblToolBar);


        }

        void CrearBotonesInToolBar() {
            if (arrEasyButtons != null)
            {
                foreach (EasyButton oEasyButton in arrEasyButtons.Where(oBE => oBE.Ubicacion != EasyUtilitario.Enumerados.Ubicacion.Footer))
                {
                    HtmlGenericControl btnItem = oEasyButton.Materialized(this.BtnClassName, this.BtnTextColor);
                    btnItem.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), this.ClientID + SCR_TOOLBAR_CLICK_ITEM + "(" + oEasyButton.ToString(true) + ");");

                    switch (oEasyButton.Ubicacion)
                    {
                        case EasyUtilitario.Enumerados.Ubicacion.Izquierda:
                            tblToolBar.Rows[0].Cells[0].Controls.Add(btnItem);
                            break;
                        case EasyUtilitario.Enumerados.Ubicacion.Centro:
                            tblToolBar.Rows[0].Cells[1].Controls.Add(btnItem);
                            break;
                        case EasyUtilitario.Enumerados.Ubicacion.Derecha:
                            tblToolBar.Rows[0].Cells[2].Controls.Add(btnItem);
                            break;
                    }
                }
            }
        }
       
        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        #endregion



        #region Scripts
        void ScriptBtnOnCLick() {
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string FncPre = @"if(" + fnPreOnServerButtonClick + "(btnItem)" + @"==true){
                                __doPostBack('" + oBtn.UniqueID + @"',btnItem.Id);
                             }";
            string PreOnCLlick = (((fnPreOnServerButtonClick!=null)&&(fnPreOnServerButtonClick.Length > 0)) ? FncPre :"  __doPostBack('" + oBtn.UniqueID +"',btnItem.Id);");

            string _ScriptToolBarOnclick = @"<script>
                                                        function " + this.ClientID + SCR_TOOLBAR_CLICK_ITEM + @"(btnItem){
                                                                var msgb='';
                                                                if(btnItem.RunAtServer.toLowerCase()=='true'){
                                                                    "+ PreOnCLlick + @"
                                                                }
                                                                else{
                                                                    if(" + this.fnToolBarButtonClick.Length + @">0){
                                                                        " + this.fnToolBarButtonClick + @"(btnItem);
                                                                    }
                                                                    else{
                                                                        $.alert({title: 'Error',
                                                                                        icon: 'fa fa-warning fa-4',
                                                                                        type: 'red',
                                                                                        closeIcon: true,
                                                                                        content: 'No existe funcion script de lado del cliente relacionado a ' + btnItem.Id ,
                                                                                        });
                                                                    }
                                                                }
                                                        }
                                                      </script>
                                                    ";
            _Scripts.Add(new LiteralControl(_ScriptToolBarOnclick));
        }
        #endregion


            protected override void OnInit(EventArgs e)
            {
                if (this.DesignMode == true)
                {
                    this.EnsureChildControls();
                }
            }

            protected override void CreateChildControls()
            {
                Controls.Clear();
                CrearTableToolBar();
                CrearBotonesInToolBar();

            oBtn = new HtmlButton();
            oBtn.ID = "CmdCommit";
            oBtn.InnerText = "Commit";
            oBtn.Attributes.Add("runat", "server");
            oBtn.Style.Add("display", "none");
            oBtn.ServerClick += new System.EventHandler(OnEasyButton_Click);
            tblToolBar.Rows[0].Cells[1].Controls.Add(oBtn);

            ScriptBtnOnCLick();
        }

            


            protected override void Render(HtmlTextWriter writer)
            {
                tblToolBar.RenderControl(writer);
                if (this.DesignMode==true) {
                    if (arrEasyButtons==null) {
                        (new LiteralControl("ToolBar[0]")).RenderControl(writer);
                    }
                     (new LiteralControl("ToolBar")).RenderControl(writer);
                }
                /*Los nuevos Script*/
                foreach (LiteralControl ScriptLC in _Scripts)
                {
                    ScriptLC.RenderControl(writer);
                }
            }

        #region ejecucion de event handler
        protected virtual void OnEasyButton_Click(object sender, EventArgs e)
        {
            string strID = "";
            string TipoBtn = ((Type)sender.GetType()).Name;
            switch (TipoBtn)
            {
                case "HtmlButton":
                    strID = ((HtmlButton)sender).ID;

                    break;
                case "ImageButton":
                    strID = ((ImageButton)sender).ID;
                    break;
            }
          
                //Obtener valor del argumento 
                HttpRequest ContextRequest = ((System.Web.UI.Page)HttpContext.Current.Handler).Request;
                strID = ContextRequest["__EVENTARGUMENT"];

                var item = arrEasyButtons.Find(x => x.Id == strID);//Boton seleccionado

                if (onClick != null) onClick?.Invoke(item);
          

            

        }
        #endregion

    }
}
