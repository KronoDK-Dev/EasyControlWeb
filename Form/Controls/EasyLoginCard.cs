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
using System.ComponentModel.DataAnnotations;
using EasyControlWeb.Test;
using System.Windows.Forms.Design;
using static EasyControlWeb.EasyTypeConvertGeneric;
using EasyControlWeb.Filtro;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Security;

namespace EasyControlWeb.Form.Controls
{
    [
        AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("CadenaLDAP"),
        ParseChildren(true, "CadenaLDAP"),
        ToolboxData("<{0}:EasyLoginCard runat=server></{0}:EasyLoginCard")
    ]

    /*
     * referencia LDAP: https://code-examples.net/es/q/1b56fb6
     * Autor:Rosales Azabache Eddy
     * Cargo: Analista desarrollador
     * Fecha:27-04-2022
     */
    public class EasyLoginCard : CompositeControl//WebControl 
    {

        public enum LoginAccion
        {
            Validacion,
            Olvido,
            Registrate
        }
        #region EVentos Externos
        public delegate void pnValidacion(EasyUsuario oEasyUsuario, LoginAccion Accion);
        public event pnValidacion Validacion;
        #endregion


        public EasyLoginCard()
        {
            arrUsuarios = new List<EasyUsuario>();
            easyParams = new List<EasyFiltroParamURLws>();
        }
        public EasyLoginCard(bool AutenticacionWindows)
        {

        }


        #region variables
            private List<EasyUsuario> arrUsuarios;
            private List<EasyFiltroParamURLws> easyParams;
            EasyToolBarButtons oEasyToolBarButtons;
        #endregion
        #region variables de tipo objeto
        HtmlGenericControl _Contenedor;
        //EasyAutocompletar txtAutocomplear;
            TextBox _inputUser;
            TextBox _inputPwd;

            EasyMessageBox msgb;
        #endregion



        #region Propiedades

        [Category("Autentica por BD")]
        public string UrlWebServiceMetodo { get; set; }
        [
          Category("Autentica por BD"),
          Description("Parametros que utilizara el WebService en la propiedad UrlWebServiceMetodo"),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
          Editor(typeof(EasyFiltroCollectionParams), typeof(UITypeEditor)),
          PersistenceMode(PersistenceMode.InnerProperty)
       ]
        public List<EasyFiltroParamURLws> EasyParmsWS{ get { return easyParams; } }
        /**seguridad   cadenaLDAP:=LDAP://simaperu.com.pe*/
        [Category("Seguridad(Autenticacion LDAP)")]
        public string CadenaLDAP { get; set; }

        [Category("Seguridad(Autenticacion LDAP)")]
        public bool AutenticacionWindows { get; set; }

        /*
        
        DataDemoBE oDataDemo = new DataDemoBE();
        [Category("Editor"),
        Description("Detalle de la clase usuario."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public DataDemoBE DataDemo
        {
            get { return oDataDemo; }
            set { oDataDemo = value; }
        }*/
      
        #endregion



        #region Controles Html
        HtmlGenericControl LoginContainer()
        {
            HtmlGenericControl _Row = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row justify-content-center");
            HtmlGenericControl _div = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "col-md-8");
            _div.Controls.Add(CardGroup());
            _Row.Controls.Add(_div);
            return _Row;
        }
        HtmlGenericControl CardGroup() {
            HtmlGenericControl _CardGroup = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card-group mb-0");
            HtmlGenericControl _Card = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card p-4");
                _Card.Controls.Add(CardBody());
                _CardGroup.Controls.Add(_Card);
                _CardGroup.Controls.Add(CardLogo());
            return _CardGroup;
        }
        HtmlGenericControl CardLogo() {
            HtmlGenericControl _CardLogo = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card text-white bg-primary py-5 d-md-down-none");
            _CardLogo.Style.Add("width", "44%");
                HtmlGenericControl _CardLogoBody = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card-body text-center");
                HtmlGenericControl _div = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div");
                //HtmlGenericControl _h2 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("h2");
               // _h2.InnerText = "Sign up";
                //_div.Controls.Add(_h2);
                HtmlImage img = new HtmlImage();
                img.Src = EasyUtilitario.Constantes.ImgDataURL.LogoEscudoSIMA;
                img.Style.Add("Width", "200px");
                _div.Controls.Add(img);

             

            _CardLogoBody.Controls.Add(_div);

            _CardLogo.Controls.Add(_CardLogoBody);
            return _CardLogo;
        }


        HtmlGenericControl CardBody() {
            HtmlGenericControl cBody = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card-body");
            HtmlImage img = new HtmlImage();
            img.Src = EasyUtilitario.Constantes.ImgDataURL.LogoApp;
            img.Style.Add("Width", "50px");
            

            HtmlGenericControl h1 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("h1");
            h1.InnerText = "Login";
            //cBody.Controls.Add(h1);

            HtmlTable tbl = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(1,2);
            tbl.Rows[0].Cells[0].Controls.Add(img);
            tbl.Rows[0].Cells[1].Controls.Add(h1);
            cBody.Controls.Add(tbl);

            HtmlGenericControl p = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("p", "text-muted");
            p.InnerText = "Iniciar sesión en su cuenta";
            cBody.Controls.Add(p);

            //Agrega los controles inputs
            cBody.Controls.Add(InputText("input-group mb-3", "fa fa-user"));
            cBody.Controls.Add(InputText("input-group mb-4", "fa fa-lock"));
            cBody.Controls.Add(RowButtons());
            return cBody;
        }

        HtmlGenericControl InputText(string ClassGroup,string Icono)
        {
            HtmlGenericControl _InputGroup = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", ClassGroup);
            HtmlGenericControl _Span = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "input-group-addon");
            HtmlGenericControl _I = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("i", Icono);
            _Span.Controls.Add(_I);
            _InputGroup.Controls.Add(_Span);
            if (Icono == "fa fa-user")
            {
                _inputUser = new TextBox();
                _inputUser.CssClass = "form-control";
                _inputUser.ID = "AutoBox";
                _inputUser.Attributes.Add("placeholder", "Username");
                _inputUser.Attributes.Add("type", "text");
                _inputUser.Style.Add("width", "100%");
                _InputGroup.Controls.Add(_inputUser);
            }
            else {
                _inputPwd = new TextBox();
                _inputPwd.CssClass = "form-control";
                _inputPwd.ID = "pwd";
                _inputPwd.Attributes.Add("placeholder", "Password");
                _inputPwd.Attributes.Add("type", "password");
                _inputPwd.Style.Add("width", "100%");
                _InputGroup.Controls.Add(_inputPwd);
            }
            
            return _InputGroup;
        }

        HtmlGenericControl RowButtons() {
            HtmlGenericControl _row = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "row");
            HtmlGenericControl Col6A = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "col-10");

            oEasyToolBarButtons = new EasyToolBarButtons();

            EasyButton oEasyButtonA = new EasyButton();
            oEasyButtonA.Id = "btnLogin";
            oEasyButtonA.ClassName = "btn btn-primary";
            oEasyButtonA.Texto = "Aceptar";
            oEasyButtonA.RunAtServer = true;
            oEasyButtonA.Ubicacion = EasyUtilitario.Enumerados.Ubicacion.Izquierda;

            oEasyToolBarButtons.EasyButtons.Add(oEasyButtonA);

            EasyButton oEasyButtonC = new EasyButton();
            oEasyButtonC.Id = "btnOlvido";
            oEasyButtonC.ClassName = "btn btn-link";
            oEasyButtonC.Texto = "¿Se te olvidó tu contraseña?";
            oEasyButtonC.RunAtServer = true;
            oEasyButtonC.Ubicacion = EasyUtilitario.Enumerados.Ubicacion.Derecha;

            oEasyToolBarButtons.EasyButtons.Add(oEasyButtonC);
            oEasyToolBarButtons.onClick += EasyToolBarButtons_onClick;

            Col6A.Controls.Add(oEasyToolBarButtons);
            _row.Controls.Add(Col6A);

            return _row;
        }


        #endregion

        protected void EasyToolBarButtons_onClick(EasyControlWeb.Form.Controls.EasyButton oEasyButton)
        {
            if (oEasyButton != null)
            {
                if (Validacion != null)//verifica si esta asociado en la pagina con el evento
                {

                    switch (oEasyButton.Id)
                    {
                        case "btnLogin":
                            ValidarUsuario();
                            break;
                        case "btnOlvido":
                            MsgNoImplementado();
                            Validacion?.Invoke(null, LoginAccion.Olvido);
                            break;
                        case "btnRegistrate":
                            Validacion?.Invoke(null, LoginAccion.Registrate);
                            break;
                    }
                }
            }
        }




        #region functiones de creacion de interface


        TextBox CrearControlUsuario() {
            _inputUser = new TextBox();
            _inputUser.ID = "AutoBox";
            _inputUser.Style.Add("width", "100%");
            return _inputUser;
        }
        #endregion


        #region Eventos Externos

        bool Valida() {
            msgb = new EasyMessageBox();
            msgb.ID = "msgb";
            msgb.Titulo = "Seguridad Integrada";
            if (this._inputUser.Text.Length == 0) { 
                msgb.Contenido = "Ingrese nombre de usuario para iniciar una sesion";
                msgb.Tipo = EasyUtilitario.Enumerados.MessageBox.Tipo.AlertType;
                msgb.AlertStyle = EasyUtilitario.Enumerados.MessageBox.AlertStyle.modern;
                this.Controls.Add(msgb);
                return false;
            }
            if (this._inputPwd.Text.Length == 0)
            {
                msgb.Contenido = "Ingrese clave de usuario para iniciar una sesion";
                msgb.Tipo = EasyUtilitario.Enumerados.MessageBox.Tipo.AlertType;
                msgb.AlertStyle = EasyUtilitario.Enumerados.MessageBox.AlertStyle.modern;
                this.Controls.Add(msgb);
                return false;
            }
            return true;
        }

        void MsgNoImplementado()
        {
            msgb = new EasyMessageBox();
            msgb.ID = "msgb";
            msgb.Titulo = "Funcionalidad";
            msgb.Contenido = "Actividad no implementada";
            msgb.Tipo = EasyUtilitario.Enumerados.MessageBox.Tipo.AlertType;
            msgb.AlertStyle = EasyUtilitario.Enumerados.MessageBox.AlertStyle.material;
            this.Controls.Add(msgb);
        }

        void ValidarUsuario() {
            EasyUsuario oEasyUsuario = new EasyUsuario();
            if (Valida() == false) { return; }

            oEasyUsuario.Login = _inputUser.Text;
            oEasyUsuario.Password = _inputPwd.Text;
            if (this.AutenticacionWindows)
            {
                if (ValidaUserAD(oEasyUsuario))
                {
                    Validacion?.Invoke(oEasyUsuario, LoginAccion.Validacion);
                }
                else
                {
                    msgb = new EasyMessageBox();
                    msgb.ID = "msgb";
                    msgb.Titulo = "Seguridad Integrada (AD)";
                    msgb.Contenido = "Usuario o Clave incorrecta..!!";
                    msgb.Tipo = EasyUtilitario.Enumerados.MessageBox.Tipo.AlertType;
                    msgb.AlertStyle = EasyUtilitario.Enumerados.MessageBox.AlertStyle.supervan;
                    this.Controls.Add(msgb);
                }
            }
            else
            {
                Validacion?.Invoke(oEasyUsuario, LoginAccion.Validacion);
            }
        }

        #endregion

        //Inicia la declaracion de la interface
        /*----------------------------------------------------------------------------------------------------------------*/
        #region eventos de diseño
                protected override void OnInit(EventArgs e)
                {
                    if (this.DesignMode == true)
                    {
                        this.EnsureChildControls();
                
                    }
                        ((System.Web.UI.Page)HttpContext.Current.Handler).Session.Clear();//reinicia todas las sessiones
                    // Page.GetPostBackEventReference(this, "LoginPostBack");//Genera PostBack
                }
        protected override void CreateChildControls()
                {
                    Controls.Clear();
                    _Contenedor = LoginContainer();
                    _Contenedor.ID = "Target";
                    _Contenedor.Attributes.Add("class", "CardLogin");
                    this.Controls.Add(_Contenedor);
                }
        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
                _Contenedor.RenderControl(writer);
                if (msgb != null)
                {
                    msgb.RenderControl(writer);
                }
            }
            else {
                (new LiteralControl("Card Login")).RenderControl(writer);
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



        #region Eventos Internos
        bool ValidaUserAD(EasyUsuario oEasyUsuario){

                    bool userOk = false;
                        try
                        {
                            using (DirectoryEntry directoryEntry = new DirectoryEntry(CadenaLDAP, oEasyUsuario.Login, EasyEncrypta.DesEncriptar(oEasyUsuario.Password)))

                            {
                                using (DirectorySearcher searcher = new DirectorySearcher(directoryEntry))
                                {
                                    searcher.Filter = "(samaccountname=" + oEasyUsuario.Login + ")";
                                    searcher.PropertiesToLoad.Add("displayname");

                                    SearchResult adsSearchResult = searcher.FindOne();

                                    if (adsSearchResult != null)
                                    {
                                        if (adsSearchResult.Properties["displayname"].Count == 1)
                                        {
                                            oEasyUsuario.ApellidosyNombres = (string)adsSearchResult.Properties["displayname"][0];
                                        }
                                        userOk = true;
                                    }
                                }
                            }
                        }
                        catch (Exception ex) {
                            userOk = false;
                        }


           // ArrayList Pcs = getNetworkComputers();


                    return userOk;
                }



        string GetCurrentDomainPath()
        {
            System.DirectoryServices.DirectoryEntry de = new System.DirectoryServices.DirectoryEntry("LDAP://RootDSE");

            return "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();
        }
        void GetAllUsers()
        {
            //Referencia : https://www.codemag.com/article/1312041/Using-Active-Directory-in-.NET
            SearchResultCollection results;
            DirectorySearcher ds = null;
            string ldap = GetCurrentDomainPath();
            System.DirectoryServices.DirectoryEntry de = new System.DirectoryServices.DirectoryEntry(ldap);

            ds = new DirectorySearcher(de);
            ds.Filter = "(&(objectCategory=User)(objectClass=person))";

            results = ds.FindAll();

            string users = "";
            foreach (SearchResult sr in results)
            {
                // Using the index zero (0) is required!
                users += sr.Properties["name"][0].ToString() + "&";
            }

            int i = 0;
        }
        #endregion


        #region Dll Imports
        [DllImport("Netapi32", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        public static extern int NetServerEnum(
            string ServerNane,
            int dwLevel,
            ref IntPtr pBuf,
            int dwPrefMaxLen,
            out int dwEntriesRead,
            out int dwTotalEntries,
            int dwServerType,
            string domain,
            out int dwResumeHandle
            );
        [DllImport("Netapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        public static extern int NetApiBufferFree(
            IntPtr pBuf);
        [StructLayout(LayoutKind.Sequential)]
        public struct _SERVER_INFO_100
        {
            internal int sv100_platform_id;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string sv100_name;
        }
        #endregion

        #region Public Methods
        public ArrayList getNetworkComputers()
        {
            ArrayList networkComputers = new ArrayList();
            const int MAX_PREFERRED_LENGTH = -1;
            int SV_TYPE_WORKSTATION = 1;
            int SV_TYPE_SERVER = 2;
            IntPtr buffer = IntPtr.Zero;
            IntPtr tmpBuffer = IntPtr.Zero;
            int entriesRead = 0;
            int totalEntries = 0;
            int resHandle = 0;
            int sizeofINFO = Marshal.SizeOf(typeof(_SERVER_INFO_100));
            try
            {

                int ret = NetServerEnum(null, 100, ref buffer, MAX_PREFERRED_LENGTH, out entriesRead, out totalEntries, SV_TYPE_WORKSTATION | SV_TYPE_SERVER, null, out resHandle);
                if (ret == 0)
                {
                    for (int i = 0; i < totalEntries; i++)
                    {
                        tmpBuffer = new IntPtr((int)buffer + (i * sizeofINFO));

                        _SERVER_INFO_100 svrInfo = (_SERVER_INFO_100)
                            Marshal.PtrToStructure(tmpBuffer, typeof(_SERVER_INFO_100));
                        networkComputers.Add(svrInfo.sv100_name);
                    }
                }
            }
            catch (Exception ex)
            {

                string ss = ex.Message;
                return null;
            }
            finally
            {
                NetApiBufferFree(buffer);
            }
            return networkComputers;
        }
        #endregion


    }
}
