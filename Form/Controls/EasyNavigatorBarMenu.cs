using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/*
 referencia de iconos: https://fontawesome.com/v4/examples/
 referencia de estilo: https://www.w3schools.com/cssref/playdemo.asp?filename=playcss_border-color
 */
namespace EasyControlWeb.Form.Controls
{
    [
       AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
       AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
       DefaultProperty("PathFotos"),
       ParseChildren(true, "PathFotos"),
       ToolboxData("<{0}:EasyNavigatorBarMenu runat=server></{0}:EasyNavigatorBarMenu")
     ]
    public class EasyNavigatorBarMenu : CompositeControl
    {


        #region Constantes
        const string IMG_LOGO_HEADER = "LogoHeader";
        const string PATH_FOTOS_USER = "PathFotos";
        const string PATH_PAG_LOGIN = "PathPagLogin";
        const string EXT_FOTO = "Ext_Foto";

        const string NOMCTR_POPUP = "_PopupHelDesk";
        const string NOMCTRL_IMG = "SnapShot";
        const string NOMCTRL_TXTDESC = "txtDescripcion";

        const string NOMCTRL_BTNACCION = "AccionShapShot";
        const string NOMCTRL_BTNITEMMENU = "cmdItemMenu";


        const string KEY_PARAM_MNUID = "MnuIdOP";
        //const string KEY_PARAM_MNUNOMBRE = "MnuIdOP";

        #endregion

        #region Variables
        EasyUsuario oUsuario = new EasyUsuario();
        string ParamMenuText = "MnuNombre";
        string ParamMenuDescrip = "MnuDescrip";
        private Dictionary<string, string> _Data = new Dictionary<String, string>();
        #endregion


        #region Eventos
        public delegate void CompletarSnapShot(EasySnapShotBE oEasySnapShotBE);
        public event CompletarSnapShot HelpSnapShot;
        #endregion


        #region Controles
            HtmlGenericControl contentMenu;
            List<LiteralControl> lcMenu = new List<LiteralControl>();


            HtmlTable objTblMenu;
            EasyNavigatorHistorial oEasyNavigatorHistorial;
            EasyPopupBase easyPopupBase;
            HtmlButton btnAccion;
            HtmlButton btnItemMenu;
            TextBox txtDescripcion;
        #endregion

        #region Propiedades

      //  public int NroAlertas { get; set; }

        [Category("Imagen")]
        [Browsable(true)]
        [Description("Archivo de Imagen de cabecera")]
        [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
        public string ImagenLogoHeader
        {
            get
            {
                if (this.ViewState[IMG_LOGO_HEADER] == null)
                {
                    // 22.01.2026 se corrige la ruta de la imagen
                    this.ViewState[IMG_LOGO_HEADER] = "~/Recursos/Img/header.jpg";
                }
                return (string)this.ViewState[IMG_LOGO_HEADER];
            }
            set
            {
                this.ViewState[IMG_LOGO_HEADER] = value;
            }
        }
        public string PathFotos
        {
            get
            {
                if (this.ViewState[PATH_FOTOS_USER] == null)
                {
                    this.ViewState[PATH_FOTOS_USER] = "http://10.10.90.13/fotopersonal/";
                }
                return (string)this.ViewState[PATH_FOTOS_USER];
            }
            set
            {
                this.ViewState[PATH_FOTOS_USER] = value;
            }
        }

        public string PathyPaginaLogin
        {
            get
            {
                if (this.ViewState[PATH_PAG_LOGIN] == null)
                {
                    this.ViewState[PATH_PAG_LOGIN] = "Login.aspx";
                }
                return (string)this.ViewState[PATH_PAG_LOGIN];
            }
            set
            {
                this.ViewState[PATH_PAG_LOGIN] = value;
            }
        }
        public string ExtensionFoto
        {
            get
            {
                if (this.ViewState[EXT_FOTO] == null)
                {
                    this.ViewState[EXT_FOTO] = ".jpg";
                }
                return (string)this.ViewState[EXT_FOTO];
            }
            set
            {
                this.ViewState[EXT_FOTO] = value;
            }
        }

        public string IconColor
        {
            get { return (string)this.ViewState["IconColor"]; }
            set { this.ViewState["IconColor"] = value; }
        }
        public string IconColorOver
        {
            get { return (string)this.ViewState["IconColorOver"]; }
            set { this.ViewState["IconColorOver"] = value; }
        }

        [Browsable(true)]
        private List<EasyNavigatorBarIconBE> NBIcons;
        [
        Category("Behavior"),
        Description("Coleccion de iconos"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(EasyFormCollectionEditor.EasyFormoCollectionNavBarIcons), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public List<EasyNavigatorBarIconBE> OptionsIcon
        {
            get
            {
                return NBIcons;
            }
        }


        public string fc_OnMenuItem_Click { get; set; }

        [Browsable(true)]
        private List<EasyNavigatorBarMenuBE> LstEasyNavigatorBarMenuBE;
        [
        Category("Behavior"),
        Description("Coleccion de items de menu"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(EasyFormCollectionEditor.EasyFormoCollectionEditorMenuItem), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public List<EasyNavigatorBarMenuBE> CollectionMenu
        {
            get
            {
                return LstEasyNavigatorBarMenuBE;
            }
        }
        #endregion



        #region Metodos de diseño
        HtmlTable TablaBarMenu()
        {
            objTblMenu = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(4, 1);
            
            objTblMenu.Attributes.Add("Border", "0px");
            objTblMenu.Style.Add("width", "100%");

            objTblMenu.Rows[0].Cells[0].Controls.Add(NavigatorBar());//Celda del logo


            if (!DesignMode)
            {
                    HtmlGenericControl jqxMenu = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div");
                jqxMenu.Style.Add("visibility", "hidden");
                jqxMenu.ID = "jqxMenu";
                jqxMenu.Controls.Add(MenuBar1_1());
                //Contenedor del menu
                contentMenu = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div");
                contentMenu.ID = "content";
                contentMenu.Attributes.Add("class", "ContentMnu");
                contentMenu.Style.Add("display", "none");
                //contentMenu.Style.Add("backgroundColor", "gray");

                // LiteralControl lcMenu = new LiteralControl("<script>" + ScriptConfigMenu() + "</script>");
                lcMenu.Add(new LiteralControl("<script>" + ScriptConfigMenu() + "</script>"));
                //contentMenu.Controls.Add(lcMenu);
                contentMenu.Controls.Add(jqxMenu);

                //objTblMenu.Rows[1].Cells[0].Controls.Add(contentMenu);
                objTblMenu.Rows[1].Cells[0].Style.Add("height", "35px");
                this.Controls.Add(contentMenu);
            }
            else {
                objTblMenu.Rows[1].Cells[0].Controls.Add(new LiteralControl("Menú"));
            }
         




            if (!DesignMode)
            {
                objTblMenu.Rows[2].Cells[0].Controls.Add(oEasyNavigatorHistorial);
                //objTblMenu.Rows[2].Cells[0].Controls.Add(new LiteralControl("historial"));
            }

            return objTblMenu;
        }

        #region Menu Control
        HtmlGenericControl TextMenu(string Texto, string UrlPage, string className)
        {
            HtmlGenericControl _aref = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a", className);
            _aref.Attributes.Add("href", UrlPage);
            _aref.Controls.Add(new LiteralControl(Texto));

            return _aref;
        }
       
        #endregion
      
        HtmlGenericControl MenuBar1_1()
        {
            HtmlGenericControl Menu = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("ul");
           /* Menu.Attributes.Add("class", "bg-secondary text-white");
            Menu.Style.Add("padding-left", "0px");
            Menu.Style.Add("padding-top", "0px");
            Menu.Style.Add("padding-bottom", "0px");*/

            if ((LstEasyNavigatorBarMenuBE != null) && (LstEasyNavigatorBarMenuBE.Count > 0))
            {
                
                foreach (EasyNavigatorBarMenuBE oEasyNavigatorBarMenuBE in LstEasyNavigatorBarMenuBE.Where(oBE => oBE.IdOpcionPadre == 0))
                {
                    HtmlGenericControl MenuItem = new HtmlGenericControl();
                    if (oEasyNavigatorBarMenuBE.NroSubItems > 0)
                    {
                        MenuItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("li");
                        MenuItem.Controls.Add(new LiteralControl(oEasyNavigatorBarMenuBE.Nombre));
                        SubMenuBar1_1(oEasyNavigatorBarMenuBE.IdOpcion, MenuItem, oEasyNavigatorBarMenuBE.AnchoGrp,true);
                    }
                    else
                    {
                        MenuItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("li");
                        HtmlGenericControl a = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a");
                        a.InnerText = oEasyNavigatorBarMenuBE.Nombre;
                        a.Attributes.Add("href", "#");
                        MenuItem.Controls.Add(a);
                    }
                  
                    Menu.Controls.Add(MenuItem);
                }
            }
            return Menu;
        }

        
        void SubMenuBar1_1(int IdPadre, HtmlGenericControl opMenu,int AnchoGrp,bool Principal)
        {
            HtmlGenericControl SubMenuItem;
            SubMenuItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("ul");
            SubMenuItem.Style.Add("width", AnchoGrp.ToString() + "px");

            foreach (EasyNavigatorBarMenuBE oEasyNavigatorBarMenuBE in LstEasyNavigatorBarMenuBE.Where(oBE => oBE.IdOpcionPadre == IdPadre))
            {
                
                string strParamName = "";
                string strParamDescrip = "";
                string strQueryStringBase = "";
                string strQueryString = "";
                string UrlReference = "";
                HtmlGenericControl MenuItem = new HtmlGenericControl();
                MenuItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("li");
                if (oEasyNavigatorBarMenuBE.NroSubItems > 0)
                {
                    MenuItem.Controls.Add(new LiteralControl(oEasyNavigatorBarMenuBE.Nombre));
                    MenuItem.Style.Add("width", AnchoGrp.ToString() + "px");

                    SubMenuBar1_1(oEasyNavigatorBarMenuBE.IdOpcion, MenuItem, oEasyNavigatorBarMenuBE.AnchoGrp,false);
                }
                else
                {
                    strParamName = this.ParamMenuText + EasyUtilitario.Constantes.Caracteres.SignoIgual.ToString() + oEasyNavigatorBarMenuBE.Nombre;
                    strParamDescrip = this.ParamMenuDescrip + EasyUtilitario.Constantes.Caracteres.SignoIgual.ToString() + oEasyNavigatorBarMenuBE.Descripcion;
                    strQueryStringBase = KEY_PARAM_MNUID + EasyUtilitario.Constantes.Caracteres.SignoIgual.ToString() + oEasyNavigatorBarMenuBE.IdOpcion + EasyUtilitario.Constantes.Caracteres.SignoAmperson.ToString() + strParamName + EasyUtilitario.Constantes.Caracteres.SignoAmperson.ToString() + strParamDescrip;

                    string[] arrParam = oEasyNavigatorBarMenuBE.RutaPag.Split('?');

                    /*  strQueryString = oEasyNavigatorBarMenuBE.RutaPag + ((arrParam.Length > 1) ? EasyUtilitario.Constantes.Caracteres.SignoAmperson : EasyUtilitario.Constantes.Caracteres.SignoInterrogacion) + strQueryStringBase;
                      UrlReference = strQueryString;
                      */
                    if (arrParam.Length > 1)
                    {
                        strQueryString = arrParam[1] + EasyUtilitario.Constantes.Caracteres.SignoAmperson + strQueryStringBase;
                    }
                    else {
                        strQueryString = strQueryStringBase;
                    }
                    UrlReference = arrParam[0];//Ruta de la pagina

                    HtmlGenericControl a = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a");
                    a.InnerText = oEasyNavigatorBarMenuBE.Nombre;
                    a.Attributes.Add("Pagina", UrlReference);
                    a.Attributes.Add("Params", strQueryString);
                    a.Attributes.Add("href", "#");
                    a.Style.Add("width", "100%");
                    //a.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), "MenuItemOnClick(this);");

                    MenuItem.Controls.Add(a);
                    MenuItem.Attributes.Add("Pagina", UrlReference);
                    MenuItem.Attributes.Add("Params", strQueryString);
                    MenuItem.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), "MenuItemOnClick(this);");
                }
                //Establece linea divisora para aquellos que tienen hijos
                if ((oEasyNavigatorBarMenuBE.NroSubItems > 0) && (Principal == false))
                {
                    if(opMenu.Controls.Count>0){ 
                      HtmlGenericControl Separador = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("li");
                      Separador.Attributes.Add("Type", "separator");
                      SubMenuItem.Controls.Add(Separador);
                    }
                }
                SubMenuItem.Controls.Add(MenuItem);
            }
            opMenu.Controls.Add(SubMenuItem);
        }



        HtmlImage ImgLogo()
        {
            HtmlImage img = new HtmlImage();
            //img.Src = this.ImagenLogoHeader;  // 22.01.2026 obtiene ruta segura
            img.Src = this.ResolveClientUrl(this.ImagenLogoHeader);
            img.Attributes["Style"] = "height:50; opacity: 0.5; filter:alpha(opacity =30);"; 
            return img;
        }


        HtmlGenericControl NavigatorBar()
        {
            //referencia de colo en la cabecera https://www.emoreau.com/Entries/Articles/2017/12/Creating-an-Add-In-for-Outlook-in-Net.aspx
            HtmlGenericControl NB = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "navbar navbar-expand-xl navbar-dark bg-dark");
            //HtmlGenericControl NB = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "navbar navbar-expand-xl navbar-dark bg-dark mb-2 bg-primary text-white");
            NB.Style.Add("margin-left", "0px");
            NB.Style.Add("padding-left", "1px");
            NB.Style.Add("padding-top", "1px");
            NB.Style.Add("padding-bottom", "1px");
          

            NB.Controls.Add(ImgLogo());
            if (!DesignMode)
            {
                NB.Controls.Add(NavFlex());//Iconos de funcionaliades generales
            }
            return NB;
        }
       

        #region screen SNAPSHOT
        string ScriptSnapShotFlash()
        {
            string script = @"function SnapShotFlash(){ 
                                            SIMA.Utilitario.Helper.Wait('Preparando',800,function(){
                                                    ExecuteSnapShot('" + this.ClientID + NOMCTR_POPUP + "_" + NOMCTRL_IMG + @"');
                                                    $('#" + this.ClientID + NOMCTR_POPUP + @"').modal('toggle'); 
                                            });
                                 }
                                ";
            return script;
        }
        string ScriptExecuteSnapShot()
        {
            string _Script = @" var DataBinarySnapShot;
                                function ExecuteSnapShot(idImage) {//instantanea
                                    html2canvas(document.body,{allowTaint: true, useCORS: true }).then(function (canvas) {
                                        DataBinarySnapShot = canvas.toDataURL();
                                        var image = jNet.get(idImage);
                                        image.src = DataBinarySnapShot;
                                    });
                                 }
                        ";
            return _Script;
        }
        string ScriptSnapShotAceptar()
        {
            string _Script = @"function SnapShot_Aceptar(){
                                    var txtDes = jNet.get('" + this.ClientID + NOMCTR_POPUP + "_" + NOMCTRL_TXTDESC + @"');
                                    var idOP = Page.Request.Params['idMenuOp'];
                                    var strb64 = DataBinarySnapShot.slice(DataBinarySnapShot.indexOf(',') + 1);
                                    var DataArrival= txtDes.value + '&' + strb64;
                                    __doPostBack('" + this.ClientID.Replace("_", "$") + @"$" + NOMCTRL_BTNACCION + @"', DataArrival);
                                }
                                ";
            return _Script;
        }


        #endregion

        #region ScriptMenu
        string ScriptConfigMenu()
        {
            string _Script = @"  $(document).ready(function () {
                                        var NomMENU = '" + this.ClientID + "_MENU5" + @"';
                                        $('#' + NomMENU).menu();

                                        $('#'+ NomMENU).menu({
                                            position: { my: 'left top', at: 'left bottom' },
                                            blur: function () {
                                                $(this).menu('option', 'position', { my: 'left top', at: 'left bottom' });
                                            },
                                            focus: function (e, ui) {
                                                if ($('#'+ NomMENU).get(0) !== $(ui).get(0).item.parent().get(0)) {
                                                    $(this).menu('option', 'position', { my: 'left top', at: 'right top' });
                                                }
                                            },
                                        });
                                    });

                    ";

            _Script = @"$(document).ready(function() {
                                 var NomMENU = '" + this.ClientID + "_jqxMenu" + @"';
                                $('#' + NomMENU).jqxMenu({ width: '100%', height: '35px' });
                                //$('#' + NomMENU ).css('visibility', 'visible');
                                window.setTimeout(function () {
                                                                var MnuAndScript=jNet.get('"+ this.ClientID + @"_content');
                                                                MnuAndScript.css('display','block');
                                                                var tblBar = jNet.get('" + this.ClientID + @"_tblBar');
                                                                jNet.get(tblBar.rows[1].cells[0]).insert(MnuAndScript);
                                                                $('#' + NomMENU ).css('visibility', 'visible');
                                                    }, 10);

                        });";

            return _Script;
        }
        string ScriptMenuItemOnCLick()
        {
            string _script = @"function MenuItemOnClick(e){
                                    var ItemMenu=jNet.get(e);
                                                    var Argument =  ItemMenu.attr('Pagina') + '" + EasyUtilitario.Constantes.Caracteres.SignoInterrogacion + @"' + ItemMenu.attr('Params') ;
                                                   SIMA.Utilitario.Helper.Wait('Preparando',0,function(){
                                                                                                    __doPostBack('" + this.ClientID.Replace("_", "$") + @"$" + NOMCTRL_BTNITEMMENU + @"',Argument);  
                                                                                                });
                                }";
            return _script;
        }
        #endregion

        HtmlGenericControl NavFlex()
        {

            HtmlGenericControl _NF = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "navbar-nav ml-auto d-flex");
            /*_NF.Style.Add("border-style", "solid");_NF.Style.Add("border-color", "white");*/

            /*Botones de Opcion*/
            if ((this.NBIcons != null) && (this.NBIcons.Count > 0))
            {
                foreach (EasyNavigatorBarIconBE oEasyNavigatorBarIconBE in this.NBIcons)
                {
                    HtmlGenericControl _a = new HtmlGenericControl("a");
                    _a = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a", "nav-item nav-link");
                    _a.Attributes.Add("href", "#");
                    _a.Style.Add("margin-top", "10px");
                    if (oEasyNavigatorBarIconBE.TipoImg == false)
                    {
                        HtmlGenericControl _i = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("i", oEasyNavigatorBarIconBE.fa_icon);
                        _i.Style.Add("color", this.IconColor);
                        _i.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onmouseout.ToString(), "this.style.color='" + this.IconColor + "';");
                        _i.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onmouseover.ToString(), "this.style.color='" + this.IconColorOver + "';");

                        _a.Controls.Add(_i);

                        if (oEasyNavigatorBarIconBE.call_fcScript != string.Empty)
                        {
                            _a.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), oEasyNavigatorBarIconBE.call_fcScript);
                        }
                    }
                    else {
                        HtmlImage img = new HtmlImage();
                        img.Src = oEasyNavigatorBarIconBE.fa_icon;
                        img.Attributes["style"] = "width: 25px;height:25px;";
                        img.Attributes["name"] = "_Infinity";
                        _a.Controls.Add(img);
                    }
                    _NF.Controls.Add(_a);
                }
            }

            /*Usuario Autenticado*/
            _NF.Controls.Add(PerfilUsuario());

            return _NF;
        }

        public struct NavigatorRight
        {
            public struct Usuario
            {
                public static HtmlGenericControl Perfil(string _PathFotos, string NombreUsuario, string NroDocumento, string ExtensionFile,string ColorText)
                {

                    HtmlGenericControl _a = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a", "nav-item nav-link dropdown-toggle user-action");
                    _a.Attributes.Add("data-toggle", "dropdown");

                        HtmlGenericControl _Container = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "icon-container");
                            HtmlImage oimg = new HtmlImage();
                            oimg.Attributes["src"] = EasyUtilitario.Helper.Archivo.UrlImagen.DownloadToUrlData(new Uri(_PathFotos + NroDocumento + ExtensionFile));

                            oimg.Attributes.Add("class", "avatar Usu");
                            oimg.Attributes["style"] = "width:50px;height:50px;display: inline;";
                        _Container.Controls.Add(oimg);

                            HtmlGenericControl _Status = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "status-circle");
                            _Status.ID = "ChatStatus";
                        _Container.Controls.Add(_Status);

                    _a.Controls.Add(_Container);

                    //_a.Controls.Add(oimg);
                    _a.Controls.Add(new LiteralControl(NombreUsuario));
                    _a.Attributes.CssStyle.Add("color", ColorText);
                    HtmlGenericControl _b = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("b", "caret");
                    _b.Attributes.Add("class", "caret");
                    _a.Controls.Add(_b);
                    return _a;

                    // _pu.Controls.Add(_a);
                    // return _pu;
                }
                public static HtmlGenericControl MenuConfig(string fc_OnMenuItem_Click, string IdUsuario)
                {
                    HtmlGenericControl _DropDownMenuUsr = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "dropdown-menu dropdown-menu-end");
                    HtmlGenericControl _aItem;
                    string[,] ConfigMenu = new string[2, 5]{{ "fa fa-user-o", "fa fa-calendar-o", "fa fa-sliders", "divider dropdown-divider", "material-icons"}
                                                        ,{"Perfil","Calendar", "Settings","","Logout"}
                                                       };

                    for (int pos = 0; pos <= ConfigMenu.GetUpperBound(1); pos++)
                    {
                        _aItem = new HtmlGenericControl();

                        if (ConfigMenu[0, pos].Equals("divider dropdown-divider"))
                        {
                            _aItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", ConfigMenu[0, pos]);
                        }
                        else
                        {
                            _aItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a", "dropdown-item");
                            HtmlGenericControl _I = new HtmlGenericControl();
                            string ss = ConfigMenu[0, pos];
                            _I = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("i", ConfigMenu[0, pos]);
                            if (pos == ConfigMenu.GetUpperBound(1)) { _I.InnerHtml = "&#xE8AC;"; }
                            _aItem.Controls.Add(_I);
                            _aItem.Controls.Add(new LiteralControl(ConfigMenu[1, pos]));
                            _aItem.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), fc_OnMenuItem_Click + "('Key'+ " + pos.ToString() + ",'MnuPerfil','" + IdUsuario + "');");

                        }

                        _DropDownMenuUsr.Controls.Add(_aItem);
                    }
                    return _DropDownMenuUsr;
                }

            }
        }
        HtmlGenericControl PerfilUsuario()
        {
            string IdUsuario = "";
            string NombreUsuario = "";
            string NroDocumento = "";
            if (oUsuario != null)
            {
                IdUsuario = oUsuario.IdUsuario.ToString();
                NroDocumento = oUsuario.NroDocumento;
                NombreUsuario = oUsuario.Login;
            }
            //Pefil de usuario
            HtmlGenericControl _pu = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "dropdown");
            _pu.Controls.Add(NavigatorRight.Usuario.Perfil(this.PathFotos, NombreUsuario, NroDocumento, this.ExtensionFoto,this.IconColor));
            _pu.Controls.Add(NavigatorRight.Usuario.MenuConfig(this.fc_OnMenuItem_Click, IdUsuario));

            return _pu;
        }



        #endregion


        public EasyNavigatorBarMenu()
        {
            LstEasyNavigatorBarMenuBE = new List<EasyNavigatorBarMenuBE>();
            NBIcons = new List<EasyNavigatorBarIconBE>();
        }
        protected override void OnInit(EventArgs e)
        {
            try
            {
                oEasyNavigatorHistorial = new EasyNavigatorHistorial();
                oEasyNavigatorHistorial.ID = "PathNav";
                oEasyNavigatorHistorial.ParamMenuText = this.ParamMenuText;
                oEasyNavigatorHistorial.ParamMenuDescrip = this.ParamMenuDescrip;
                oEasyNavigatorHistorial.IconColor = this.IconColor;
                oEasyNavigatorHistorial.IconColorOver = "black";

                easyPopupBase = new EasyPopupBase();
                easyPopupBase.ID = this.ClientID + NOMCTR_POPUP;
                easyPopupBase.Titulo = "HelpDesk-Instantanea";
                easyPopupBase.DisplayButtons = true;
                easyPopupBase.fncScriptAceptar = "SnapShot_Aceptar";

                txtDescripcion = new TextBox();

                Page.GetPostBackEventReference(this, "MenuPost");//Genera PostBack
            }
            catch (Exception ex)
            {
                int i = 0;
            }
        }

        HtmlTable WindowDetalleSanpShot()
        {
            //Elaborar la ventana
            HtmlTable tblWindows = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(4, 2);
            try
            {
                tblWindows.Style.Add("Width", "100%");
                //tblWindows.Attributes.Add("border", "4px");
                tblWindows.Rows[0].Cells[0].ColSpan = 2;
                tblWindows.Rows[0].Cells[0].Style.Add("border-style", "dotted");
                tblWindows.Rows[0].Cells[0].Style.Add("border-color", "#4f4e4d");


                tblWindows.Rows[0].Cells[1].Visible = false;
                //Control Imagen
                HtmlImage oImgSnapShot = new HtmlImage();
                oImgSnapShot.ID = NOMCTRL_IMG;
                oImgSnapShot.Style.Add("Width", "100%");
                oImgSnapShot.Style.Add("Height", "100%");
                //oImgSnapShot.Src = "Image/header.jpg";//Aqui la ruta el nombre de la imagen generada
                // 22.01.2026 se corrgie la ruta a la imagen
                oImgSnapShot.Src = this.ResolveClientUrl("~/Recursos/Img/header.jpg");
                tblWindows.Rows[0].Cells[0].Controls.Add(oImgSnapShot);
                tblWindows.Rows[0].Cells[0].Style.Add("Height", "300px");

                HtmlGenericControl lbl = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("Label");
                lbl.InnerText = "USUARIO:";
                lbl.Style.Add("font-weight", "bold");
                tblWindows.Rows[1].Cells[0].Controls.Add(lbl);
                //Crear INPUT
                HtmlGenericControl INPUT_USER = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("INPUT");
                INPUT_USER.Attributes.Add("Width", "100px");
                INPUT_USER.Attributes.Add("value", oUsuario.Login);

                HtmlGenericControl grupoCTRL = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "form-group");
                grupoCTRL.Controls.Add(INPUT_USER);
                tblWindows.Rows[1].Cells[1].Controls.Add(grupoCTRL);

                tblWindows.Rows[2].Cells[0].InnerText = "DESCRIPCION:";
                tblWindows.Rows[2].Cells[0].Style.Add("font-weight", "bold");

                txtDescripcion.ID = NOMCTRL_TXTDESC;
                txtDescripcion.Attributes.Add("class", "form-group");
                txtDescripcion.TextMode = TextBoxMode.MultiLine;
                txtDescripcion.Style.Add("Width", "100%");


                tblWindows.Rows[3].Cells[0].ColSpan = 2;
                tblWindows.Rows[3].Cells[1].Visible = false;

                tblWindows.Rows[3].Cells[0].Style.Add("Width", "400px");
                tblWindows.Rows[3].Cells[0].Controls.Add(txtDescripcion);

            }
            catch (Exception ex)
            {
                EasyControlWeb.Form.Controls.EasyNavigatorBE oEasyNavigatorBE = new EasyControlWeb.Form.Controls.EasyNavigatorBE();
                oEasyNavigatorBE.Texto = "Error al invocar la pagina: ";
                oEasyNavigatorBE.Descripcion = ex.Message.ToString();
                //oEasyNavigatorBE.Url = "Error.aspx?Err=" + ex.Message.ToString();
                oEasyNavigatorBE.Pagina = "Error.aspx";
                oEasyNavigatorBE.Params.Add(new EasyNavigatorParam("Error", ex.Message.ToString()));

                oEasyNavigatorHistorial.IrA(oEasyNavigatorBE);
            }
            return tblWindows;
        }


        protected override void CreateChildControls()
        {
            
            objTblMenu = TablaBarMenu();
            objTblMenu.ID = "tblBar";// se agrego
            btnAccion = new HtmlButton();
            btnAccion.ID = NOMCTRL_BTNACCION;
            btnAccion.Attributes.Add("runat", "server");
            btnAccion.Style.Add("display", "none");
            btnAccion.InnerText = "AceptarSnapSHot";
            btnAccion.ServerClick += new System.EventHandler(EasySnapShot_Click);
            objTblMenu.Rows[3].Cells[0].Controls.Add(btnAccion);

            //Para la caja de dialogo de busqueda
            btnItemMenu = new HtmlButton();
            btnItemMenu.ID = NOMCTRL_BTNITEMMENU;
            btnItemMenu.Attributes.Add("runat", "server");
            btnItemMenu.Style.Add("display", "none");
            btnItemMenu.InnerText = "ItemMenuClick";
            btnItemMenu.ServerClick += new System.EventHandler(EasyItemMenuSeleted_Click);
            objTblMenu.Rows[3].Cells[0].Controls.Add(btnItemMenu);
            this.Controls.Add(objTblMenu);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            objTblMenu.RenderControl(writer);//Barra de Menu
            if (!DesignMode)
            {
                //btnItemMenu.RenderControl(writer);//Boton que enlaza al itemMenu
                easyPopupBase.Controls.Add(WindowDetalleSanpShot());
                easyPopupBase.RenderControl(writer);
                contentMenu.RenderControl(writer);//Contenedor del menu
                foreach (LiteralControl lc in lcMenu) { 
                    lc.RenderControl(writer);
                }
            }

            LiteralControl LCScript = (new LiteralControl("<script>" + ScriptSnapShotFlash() + "</script>"));
            LCScript.RenderControl(writer);
            LCScript = (new LiteralControl("\n<script>" + ScriptExecuteSnapShot() + "</script>"));
            LCScript.RenderControl(writer);

            LCScript = (new LiteralControl("\n<script>" + ScriptSnapShotAceptar() + "</script>"));
            LCScript.RenderControl(writer);


            /*LCScript = (new LiteralControl("\n<script>" + ScriptConfigMenu() + "</script>"));
            LCScript.RenderControl(writer);*/

            LCScript = (new LiteralControl("\n<script>" + ScriptMenuItemOnCLick() + "</script>"));
            LCScript.RenderControl(writer);




            // btnAccion.RenderControl(writer);
        }

        public void SetUser(EasyUsuario easyUsuario)
        {
            oUsuario = easyUsuario;
        }
        protected virtual void EasyItemMenuSeleted_Click(object sender, EventArgs e)
        {
            string Argument = "";
            EasyControlWeb.Form.Controls.EasyNavigatorBE oEasyNavigatorBE = new EasyControlWeb.Form.Controls.EasyNavigatorBE();
            try
            {
                HttpRequest ContextRequest = ((System.Web.UI.Page)HttpContext.Current.Handler).Request;
                Argument = ContextRequest["__EVENTARGUMENT"];
                string[] DataArrival = (Argument.Split('@'))[0].Split('?');

                string LstParams = DataArrival[1];

                RequestParamToData(LstParams, '&', '=');

                oEasyNavigatorHistorial.UbicateEn("1|", true);//reinicia el historial

                oEasyNavigatorBE.Texto = _Data[this.ParamMenuText];
                oEasyNavigatorBE.Descripcion = _Data[this.ParamMenuDescrip];
                oEasyNavigatorBE.Pagina = DataArrival[0];
                if (DataArrival.Length > 1)
                {
                    foreach (string strParam in LstParams.ToString().Split('&'))
                    {
                        string[] sParam = strParam.Split('=');
                        oEasyNavigatorBE.Params.Add(new EasyNavigatorParam(sParam[0], sParam[1]));
                    }

                }

                oEasyNavigatorHistorial.IrA(oEasyNavigatorBE);
            }
            catch (Exception ex)
            {
                oEasyNavigatorBE.Texto = "Error al invocar la pagina: " + Argument;
                oEasyNavigatorBE.Descripcion = ex.Message.ToString();
                //oEasyNavigatorBE.Url = "Error.aspx?Err=" + ex.Message.ToString();
                oEasyNavigatorBE.Pagina = "Error.aspx";
                oEasyNavigatorBE.Params.Add(new EasyNavigatorParam("Error", ex.Message.ToString()));
                oEasyNavigatorHistorial.IrA(oEasyNavigatorBE);
            }
        }
        void RequestParamToData(string Params, char SplitBase, char delimitador)
        {
            string[] arrData = Params.Split(SplitBase);
            foreach (string Param in arrData)
            {
                string[] Name_Value = Param.Split(delimitador);
                _Data.Add(Name_Value[0], Name_Value[1]);
            }

        }


        protected virtual void EasySnapShot_Click(object sender, EventArgs e)
        {
            if (HelpSnapShot != null)
            {
                HttpRequest ContextRequest = ((System.Web.UI.Page)HttpContext.Current.Handler).Request;
                string Argument = ContextRequest["__EVENTARGUMENT"];
                string[] DataArrival = Argument.Split('&');

                string IdOpMenu = ((ContextRequest["idMenuOp"] == null) ? "0" : ContextRequest["idMenuOp"]);

                byte[] imgByteArray = Convert.FromBase64String(DataArrival[1]);

                EasySnapShotBE oEasySnapShotBE = new EasySnapShotBE(Convert.ToInt32(IdOpMenu), DataArrival[0], imgByteArray);
                //Call invoke          
                HelpSnapShot?.Invoke(oEasySnapShotBE);
            }
        }
    }
}
