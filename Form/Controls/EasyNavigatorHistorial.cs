using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Security.Permissions;
using System.Web.UI.HtmlControls;
using EasyControlWeb.Errors;
using EasyControlWeb.Filtro;
using System.Windows.Resources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Drawing; 

namespace EasyControlWeb.Form.Controls
{

    /**********************************************************************************************************/
    public enum TipoValorRetorno
    { 
        Simple,
        Doble,
    }

    /**********************************************************************************************************/
    [
     AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
     DefaultProperty("defaultPageName"),
     ParseChildren(true, "defaultPageName"),
     ToolboxData("<{0}:EasyNavigatorHistorial runat=server></{0}:EasyNavigatorHistorial")
    ]
    public class EasyNavigatorHistorial : CompositeControl
    {

        #region Variables Locales
            string BlockScript = "";
            List<LiteralControl> _ScriptsHS = new List<LiteralControl>();
            string scriptSelect = "";
        #endregion

        #region Controles
        HtmlGenericControl _Navigator;
            HtmlButton btnPostBack;
            HtmlGenericControl _btnAtras;
        #endregion

        #region Propiedades Simples


      /*  public string ApplicationPath
        {
            get {
                string NomAPP = ((System.Web.UI.Page)HttpContext.Current.Handler).Request.ApplicationPath.Replace("/", "");
                string StringUrl = ((System.Web.UI.Page)HttpContext.Current.Handler).Request.Url.OriginalString;
                int MaxLeng = StringUrl.IndexOf(NomAPP);
                string UrlAbsoluta = StringUrl.Substring(0, ((MaxLeng + NomAPP.Length) + 1));
                return UrlAbsoluta;
            }
        }*/
            public string IconColor { get; set; }
            public string IconColorOver { get; set; }

       
            private string defaultPageName;
            public string DefaultPageName {
                get
                {
                    if (defaultPageName == null)
                    {
                        defaultPageName = "Default.aspx";
                    }
                    return defaultPageName;
                }
                set
                {
                    defaultPageName = value;
                }
            } 
            public string ParamMenuText { get; set; }
            public string ParamMenuDescrip { get; set; }
        
        #endregion



        private List<EasyNavigatorBE> History = new List<EasyNavigatorBE>();
        private List<EasyNavigatorBE> getHistorial()
        {
            List<EasyNavigatorBE> oEasyNavigatorBElst = new List<EasyNavigatorBE>();
            try
            {
                oEasyNavigatorBElst = (List<EasyNavigatorBE>)((System.Web.UI.Page)HttpContext.Current.Handler).Session[EasyUtilitario.Constantes.Sessiones.Historial];
            }
            catch (Exception ex) {
                oEasyNavigatorBElst = null;
            }
            return oEasyNavigatorBElst;
        }
        private void setHistorial(List<EasyNavigatorBE> oHistorial)
        {
            ((System.Web.UI.Page)HttpContext.Current.Handler).Session[EasyUtilitario.Constantes.Sessiones.Historial] = oHistorial;
        }

        HtmlGenericControl CrearHistorial() {
            int idx = 1;
            _Navigator =  EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("nav");
            _Navigator.Attributes.Add("aria-label", "breadcrumb");

            //_Navigator.Attributes.CssStyle.Add("opacity", "0.5");
           // _Navigator.Attributes.CssStyle.Add("background-color","transparent");
            

            HtmlGenericControl _ol = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("ol", "breadcrumb breadcrumb-arrow HistoryBar");
            //_ol.Attributes.Add("class", "breadcrumb breadcrumb-arrow p-0");

            PaintBrush:
            this.History = getHistorial();
            if (this.History != null)
            {
                foreach (EasyNavigatorBE oEasyNavigatorBE in this.History)
                {
                    HtmlGenericControl _li = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("li");
                   // _li.Attributes.CssStyle.Add("opacity", "0.5");

                    HtmlGenericControl _a = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a");
                    _a.Attributes.Add("href", "#");
                    _a.InnerText = oEasyNavigatorBE.Texto;
                    _a.Attributes.Add("runat", "server");
                    string Parametro = idx.ToString() + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal + oEasyNavigatorBE.Url;
                    _a.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), "IrA('" + Parametro + "');");

                    if (idx == 1)
                    {
                        _li.Attributes.Add("class", "breadcrumb-item");
                        _a.Attributes.Add("class", "text-uppercase pl-3");
                        _li.Controls.Add(_a);
                    }
                    else if (idx < this.History.Count)
                    {
                        _li.Attributes.Add("class", "breadcrumb-item pl-0");
                        _a.Attributes.Add("class", "text-uppercase");
                        _li.Controls.Add(_a);
                    }
                    else
                    {
                        _li.Attributes.Add("aria-current", "page");
                        _li.Attributes.Add("class", "breadcrumb-item pl-0 active text-uppercase pl-4");
                        _li.InnerText = oEasyNavigatorBE.Texto;
                    }

                    _ol.Controls.Add(_li);
                    idx++;
                }
                _Navigator.Controls.Add(_ol);
            }
            else {
                if (!IsDesign()) {
                    string pathPag = ((System.Web.UI.Page)HttpContext.Current.Handler).Request.RawUrl;
                    string App = "/"+EasyUtilitario.Helper.Pagina.AppName();
                    pathPag = pathPag.Replace(App, "");
                    //Dibuja la pagina por defecto
                    EasyNavigatorBE oEasyNavigatorBE = new EasyNavigatorBE();
                    oEasyNavigatorBE.Texto = "HOME";
                    oEasyNavigatorBE.Descripcion = "Pagina por PRINCIPAL";
                    oEasyNavigatorBE.Url = pathPag;
                    ColleccionarNavegacion(oEasyNavigatorBE);
                    goto PaintBrush;
                }
                else
                {
                    HtmlGenericControl _li = new HtmlGenericControl("li");
                    HtmlGenericControl _a = new HtmlGenericControl("a");
                    _a.Attributes.Add("href", "#");
                    _a.InnerText = "Diseño>";
                    _li.Controls.Add(_a);
                    _ol.Controls.Add(_li);
                    _Navigator.Controls.Add(_ol);
                }
            }

          

            return _Navigator;
        }

        HtmlGenericControl CrearBotonAtras() {
            HtmlGenericControl btnNull = new HtmlGenericControl();
            _btnAtras = new HtmlGenericControl();

            HtmlGenericControl btnAtras = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a", "float-nav-2");
            btnAtras.Attributes.Add("href", "#");
            btnAtras.ID = "ArrowAtras";
            HtmlGenericControl btni = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("i", "fa fa-arrow-circle-o-left fa-4x");
            btni.Attributes.Add("aria-hidden", "true");
            btni.Style.Add("color", this.IconColor);

            btni.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onmouseout.ToString(), "this.style.color='" + this.IconColor + "';");
            btni.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onmouseover.ToString(), "this.style.color='" + this.IconColorOver + "';");

            this.History = getHistorial();
            if (this.History != null)
            {
                string Parametro = "";
                if (this.History.Count >= 2)
                {
                    EasyNavigatorBE oEasyNavigatorBE = (EasyNavigatorBE)History[History.Count - 2];
                    Parametro = (History.Count - 1).ToString() + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal + oEasyNavigatorBE.Url;
                }
                btni.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), "IrA('" + Parametro + "');");
                btnAtras.Controls.Add(btni);

                _btnAtras = ((History.Count == 1) ? btnNull : btnAtras);
            }
            return _btnAtras;
        }
        string StyleColorBtnMerge() {
            string _style = @".float-nav-2 {
                                  position: fixed;
                                  bottom: 20px;
                                  right: 20px;
                                  z-index: 9999;
                                }
                                .float-nav-2:hover {
                                cursor:pointer;
                                }
                            ";
            return _style;
        }

        //metodo para limpiar literales en paginas
        private void RegisterStartupScript(string key, string script)
        {
            var page = this.Page ?? (Page)HttpContext.Current?.CurrentHandler;
            if (page == null) return;

            if (ScriptManager.GetCurrent(page) != null)
            {
                // Compatible con UpdatePanel / ASP.NET AJAX
                ScriptManager.RegisterStartupScript(
                    page,
                    typeof(EasyNavigatorHistorial),
                    key,
                    script,
                    addScriptTags: true
                );
            }
            else
            {
                // Página sin UpdatePanel
                page.ClientScript.RegisterStartupScript(
                    typeof(EasyNavigatorHistorial),
                    key,
                    script,
                    addScriptTags: true
                );
            }
        }


        public void getAllCtrlMemoryValue() {
            //Restaura los valores de los controles guardados
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;

            string UrlActual = "/" + EasyUtilitario.Helper.Pagina.UrlContextActual();//Modificado 08-11-2022 se agrego el caracter /
            this.History = getHistorial();
            if (this.History == null) { return; }
            foreach (EasyNavigatorBE oEasyNavigatorBE in this.History.Where(oBE => oBE.Pagina + ((oBE.ToString().Length > 0) ? "?" : "") + oBE.ToString() == UrlActual))
                //foreach (EasyNavigatorBE oEasyNavigatorBE in this.History.Where(oBE => oBE.Url == UrlActual))
            {
                if (oEasyNavigatorBE.LstCtrlValue != null)
                {
                    string[] LstCtrl = oEasyNavigatorBE.LstCtrlValue.Split(EasyUtilitario.Constantes.Caracteres.SignoAmperson.ToCharArray());
                    foreach (string strCtrlValue in LstCtrl)
                    {
                        string[] Ctrl_Value = strCtrlValue.Split(EasyUtilitario.Constantes.Caracteres.SignoIgual.ToCharArray());
                        object obj = ((System.Web.UI.Page)HttpContext.Current.Handler).Page.FindControl(Ctrl_Value[0]);
                        if (obj != null)
                        {
                            if (obj.GetType() == typeof(TextBox))
                            {
                                ((TextBox)(obj)).Text = Ctrl_Value[1];
                            }
                            else if (obj.GetType() == typeof(EasyDatepicker))
                            {
                                ((EasyDatepicker)(obj)).Text = Ctrl_Value[1];
                            }
                            else if (obj.GetType() == typeof(EasyNumericBox))
                            {
                                ((EasyNumericBox)(obj)).Text = Ctrl_Value[1];
                            }
                            else if (obj.GetType() == typeof(EasyAutocompletar))
                            {
                                string[] Text_Valor = Ctrl_Value[1].Split(EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal.ToCharArray());
                                ((EasyAutocompletar)(obj)).SetValue(Text_Valor[0], Text_Valor[1]);
                            }
                            else if (obj.GetType() == typeof(DropDownList))
                            {
                                ((DropDownList)(obj)).SelectedIndex = Convert.ToInt32(Ctrl_Value[1]);
                            }
                            else if (obj.GetType() == typeof(ListBox))
                            {
                                ((ListBox)(obj)).SelectedIndex = Convert.ToInt32(Ctrl_Value[1]);
                            }
                            else if (obj.GetType() == typeof(EasyGestorFiltro))
                            {
                                EasyGestorFiltro oEasyGestorFiltro = ((EasyGestorFiltro)(obj));
                                oEasyGestorFiltro.setCollectionCriterios((List<EasyFiltroItem>)oEasyNavigatorBE.LstCtrlValues);
                            }
                            else if (obj.GetType() == typeof(EasyGridView))//Tambie se debara de incluir el Sorting
                            {
                                EasyGridView oEasyGridView = ((EasyGridView)(obj));
                                string[] PagSort = Ctrl_Value[1].Split(EasyUtilitario.Constantes.Caracteres.SignoArroba.ToCharArray());
                                oEasyGridView.setPagina(PagSort[0]);
                                oEasyGridView.PageIndex = Convert.ToInt32(PagSort[0]);
                                if ((PagSort[1].Length > 0) && (PagSort[2].Length > 0)){
                                    oEasyGridView.setSorting(PagSort[1], PagSort[2]);
                                }
                                if (PagSort[3].Length > 0) {//Script para el registro seleccionado
                                    string NroFila = PagSort[3];
                                    string _RowIndex = PagSort[4];
                                    oEasyGridView.setNroRegSelect(NroFila);
                                    /*
                                    string fncSelectedRow = @"function " + oEasyGridView.ClientID + @"_onSeleted(){
                                                                  var ogridView=document.getElementById('" + oEasyGridView.ClientID + @"');
                                                                  var rows = ogridView.querySelectorAll('[TipoRow=" + cmll + "2" + cmll + @"]'); 
                                                                      rows.forEach(function(row,idx){
                                                                                        var orow = jNet.get(row);
                                                                                        if(row.rowIndex=='" + _RowIndex + @"'){
                                                                                            " + oEasyGridView.ClientID + @"_OnRowClick(row);
                                                                                            SIMA.GridView.Extended.OnEventClickChangeColor(row);
                                                                                        }
                                                                                    });
                                                                }
                                                        ";
                                    /*  reemplazamos estas lineas para optmizar el error de literales
                                    ((System.Web.UI.Page)HttpContext.Current.Handler).Controls.Add(new LiteralControl("<script>" + fncSelectedRow + "</script>"));

                                    scriptSelect = @" setTimeout(" + oEasyGridView.ClientID + @"_onSeleted(),9000);";
                                    ((System.Web.UI.Page)HttpContext.Current.Handler).Controls.Add(new LiteralControl("<script>" + scriptSelect + "</script>"));
                                    */

                                    string functionName = oEasyGridView.ClientID + "_onSelected";
                                    string fncSelectedRow = $@"
                                                function {functionName}(){{
                                                    var ogridView = document.getElementById('{oEasyGridView.ClientID}');
                                                    if(!ogridView) return;
                                                    var rows = ogridView.querySelectorAll('[TipoRow=""2""]');
                                                    rows.forEach(function(row, idx){{
                                                        var orow = jNet.get(row);
                                                        if(row.rowIndex == '{_RowIndex}'){{
                                                            {oEasyGridView.ClientID}_OnRowClick(row);
                                                            SIMA.GridView.Extended.OnEventClickChangeColor(row);
                                                        }}
                                                    }});
                                                }}";

                                       // Conservamos tu delay (9000 ms) tal como estaba.
                                          string startup = $"setTimeout(function(){{ {functionName}(); }}, 9000);";

                                        // Registramos ambos en un solo bloque, con clave única por Grid.
                                          RegisterStartupScript("ENH_SelectedRow_" + oEasyGridView.ClientID, fncSelectedRow + "\n" + startup);


                                }
                            }

                        }
                    }
                }
            }
        }
        public void SavePageCtrlStatus(params object[] LstCtrl)
        {
            try
            {
                string UrlActual = "/" + EasyUtilitario.Helper.Pagina.UrlContextActual();//Modificado 08-11-2022 se agrego el caracter /
                this.History = getHistorial();

                foreach (EasyNavigatorBE oEasyNavigatorBE in this.History.Where(oBE =>  oBE.Pagina  +  ((oBE.ToString().Length>0)?"?":"") + oBE.ToString() == UrlActual))
                {
                    oEasyNavigatorBE.LstCtrlValue = null;
                    //Localiza los controles del array para guardar su valor
                    foreach (object ObjCtrl in LstCtrl)
                    {
                        object obj;string objName;
                        if (ObjCtrl.GetType() == typeof(string))
                        {
                            objName = ((string)(ObjCtrl));
                            obj = ((System.Web.UI.Page)HttpContext.Current.Handler).Page.FindControl(objName);
                        }
                        else {
                            //objName = ((string)(ObjCtrl));
                            obj = ObjCtrl;
                        }
                        
                        //oEasyNavigatorBE.LstCtrlValue += objName + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                        if (obj.GetType() == typeof(TextBox))
                        {
                            oEasyNavigatorBE.LstCtrlValue += ((TextBox)(obj)).ID + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                            oEasyNavigatorBE.LstCtrlValue += ((TextBox)(obj)).Text;
                        }
                        else if (obj.GetType() == typeof(EasyTextBox))
                        {
                            oEasyNavigatorBE.LstCtrlValue += ((EasyTextBox)(obj)).ID + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                            oEasyNavigatorBE.LstCtrlValue += ((EasyTextBox)(obj)).Text;
                        }
                        else if (obj.GetType() == typeof(EasyDatepicker))
                        {
                            oEasyNavigatorBE.LstCtrlValue += ((EasyDatepicker)(obj)).ID + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                            oEasyNavigatorBE.LstCtrlValue += ((EasyDatepicker)(obj)).Text;
                        }
                        else if (obj.GetType() == typeof(EasyNumericBox))
                        {
                            oEasyNavigatorBE.LstCtrlValue += ((EasyNumericBox)(obj)).ID + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                            oEasyNavigatorBE.LstCtrlValue += ((EasyNumericBox)(obj)).Text;
                        }
                        else if (obj.GetType() == typeof(EasyAutocompletar))
                        {
                            oEasyNavigatorBE.LstCtrlValue += ((EasyAutocompletar)(obj)).ID + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                            oEasyNavigatorBE.LstCtrlValue += ((EasyAutocompletar)(obj)).GetText() + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal + ((EasyAutocompletar)(obj)).GetValue();
                        }
                        else if (obj.GetType() == typeof(DropDownList))
                        {
                            oEasyNavigatorBE.LstCtrlValue += ((DropDownList)(obj)).ID + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                            oEasyNavigatorBE.LstCtrlValue += ((DropDownList)(obj)).SelectedIndex.ToString();
                        }
                        else if (obj.GetType() == typeof(EasyDropdownList))
                        {
                            oEasyNavigatorBE.LstCtrlValue += ((EasyDropdownList)(obj)).ID + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                            oEasyNavigatorBE.LstCtrlValue += ((EasyDropdownList)(obj)).SelectedIndex.ToString();
                        }
                        else if (obj.GetType() == typeof(ListBox))
                        {
                            oEasyNavigatorBE.LstCtrlValue += ((ListBox)(obj)).ID + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                            oEasyNavigatorBE.LstCtrlValue += ((ListBox)(obj)).SelectedIndex.ToString();
                        }
                        else if (obj.GetType() == typeof(EasyGestorFiltro))
                        {
                            EasyGestorFiltro oEasyGestorFiltro = ((EasyGestorFiltro)(obj));
                            oEasyNavigatorBE.LstCtrlValue += oEasyGestorFiltro.ID + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                            oEasyNavigatorBE.LstCtrlValues = oEasyGestorFiltro.getCollectionCriterios();

                        }
                        else if (obj.GetType() == typeof(EasyGridView))
                        {
                            EasyGridView oEasyGridView = ((EasyGridView)(obj));
                            oEasyNavigatorBE.LstCtrlValue += oEasyGridView.ID + EasyUtilitario.Constantes.Caracteres.SignoIgual;
                            oEasyNavigatorBE.LstCtrlValue += oEasyGridView.getPagina() + EasyUtilitario.Constantes.Caracteres.SignoArroba.ToString() + oEasyGridView.getSorting(1) + EasyUtilitario.Constantes.Caracteres.SignoArroba.ToString() + oEasyGridView.getSorting(2) + EasyUtilitario.Constantes.Caracteres.SignoArroba.ToString() + oEasyGridView.getNroRegSelect() + EasyUtilitario.Constantes.Caracteres.SignoArroba.ToString() + oEasyGridView.getRowIndex();

                        }

                        

                        oEasyNavigatorBE.LstCtrlValue += EasyUtilitario.Constantes.Caracteres.SignoAmperson;

                    }
                    oEasyNavigatorBE.LstCtrlValue = ((oEasyNavigatorBE.LstCtrlValue.Substring(oEasyNavigatorBE.LstCtrlValue.Length - 1) == EasyUtilitario.Constantes.Caracteres.SignoAmperson) ? oEasyNavigatorBE.LstCtrlValue.Substring(0, oEasyNavigatorBE.LstCtrlValue.Length - 1) : oEasyNavigatorBE.LstCtrlValue);
                }
            }
            catch (Exception ex)
            {
                EasyErrorControls oEasyErrorControls = new EasyErrorControls("EasyNavigatorHistorial:Metodo=SavePageCtrlStatus ");
                oEasyErrorControls.Pagina = EasyUtilitario.Helper.Pagina.UrlContextActual();
                oEasyErrorControls.Mensaje = ex.Message;
                throw oEasyErrorControls;
            }

        }




        /*Eventos Propios de control*/
        protected override void OnInit(EventArgs e)
        {
            Page.GetPostBackEventReference(this, "HistorialPostBack");//Genera PostBack
            BlockScript = @"function IrA(argument){
                                        var arrParam = argument.split('|');
                                        var Argument =  arrParam[0] + '|' +  arrParam[1]; 
                                        try{
                                          SIMA.Utilitario.Helper.Wait(arrParam[0],0,function(){
                                                __doPostBack('" + this.ClientID.Replace("_", "$") + @"$btnPress',Argument);
                                            });
                                        }
                                        catch(ex){
                                            __doPostBack('" + this.ClientID.Replace("_", "$") + @"$btnPress',Argument);
                                        }                                    
                            }
                        ";
            EasyUtilitario.Helper.Genericos.RegistraBlockScript("History", BlockScript);
        }


        protected override void CreateChildControls()
        {
            Controls.Clear();
            this.Controls.Add(CrearHistorial());

            this.Controls.Add(CrearBotonAtras());
            btnPostBack = new HtmlButton();
            btnPostBack.ServerClick += EasyHistorial_Click;
            btnPostBack.ID = "btnPress";
            btnPostBack.InnerText = "IrA";
            btnPostBack.Style.Add("display", "None");
            this.Controls.Add(btnPostBack);

        }
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                _Navigator.RenderControl(writer);
                if (!IsDesign())
                {
                     btnPostBack.RenderControl(writer);
                     _btnAtras.RenderControl(writer);
                }
                LiteralControl lcStyle = new LiteralControl("<style>" + StyleColorBtnMerge() + "</style>");
                lcStyle.RenderControl(writer);
                //Registrar los script
                /*foreach (LiteralControl lcScript in _ScriptsHS) {
                     lcScript.RenderControl(writer);
                 }*/
                //(new LiteralControl(scriptSelect)).RenderControl(writer);
            }
            catch (Exception ex) {
                string msg = ex.Message;
            }


        }

        #region Metodos Internos
        protected virtual void EasyHistorial_Click(object sender, EventArgs e)
        {
            string Argument= ((System.Web.UI.Page)HttpContext.Current.Handler).Request["__EVENTARGUMENT"];
            if (Argument.Length > 0)
            {
                UbicateEn(Argument);
            }
        }
        #endregion

        #region Metodos Externos
        public void UbicateEn(string Argument)
        {
            UbicateEn(Argument, false);
        }
        public void UbicateEn(string Argument, bool OnlyRemove)
        {
            char MiCar = (char)Convert.ToChar(EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal.ToString());
                this.History = getHistorial();
                string[] arrHistSelect = Argument.Split(MiCar);
                int PosActual = Convert.ToInt32(arrHistSelect[0]);
                int PosUlt = this.History.Count;
                if (PosActual == this.History.Count) { return; }//esta en ña mima posicion
                for (int p = PosActual; p <= PosUlt; p++)
                {
                    this.History.RemoveAt(PosActual);
                    PosUlt--;
                }
                setHistorial(this.History);
                EasyNavigatorBE oNavBE = (EasyNavigatorBE)this.History[PosActual - 1];
            if (OnlyRemove == false)
            {
                Go(oNavBE);
                //((System.Web.UI.Page)HttpContext.Current.Handler).Response.Redirect(EasyUtilitario.Helper.Pagina.PathSite() + oNavBE.Url, false);               
            }
        }
        bool ExisteInLast(EasyNavigatorBE oEasyNavigatorBE) {
            this.History = getHistorial();
            if(this.History!=null)
            {
                EasyNavigatorBE LastEasyNavigatorBE = (EasyNavigatorBE)this.History[this.History.Count - 1];
                /*string[] Local = LastEasyNavigatorBE.Url.Split('/');
                string[] Remoto = oEasyNavigatorBE.Url.Split('/');
                if (Local[Local.Length-1] == Remoto[Remoto.Length-1]) {
                    return true;
                }

                */
                string[] Local = LastEasyNavigatorBE.Pagina.Split('/');
                string[] Remoto = oEasyNavigatorBE.Pagina.Split('/');
                string LocalyParams = Local[Local.Length - 1] + LastEasyNavigatorBE.ToString();
                string RemotoyParams = Remoto[Remoto.Length - 1] + LastEasyNavigatorBE.ToString();
                if (LocalyParams == RemotoyParams) {
                    return true;
                }
            }
            return false;
        }
         
        public void IrA(EasyNavigatorBE oEasyNavigatorBE) {
            this.ColleccionarNavegacion(oEasyNavigatorBE);
            Go(oEasyNavigatorBE);
            //((System.Web.UI.Page)HttpContext.Current.Handler).Response.Redirect(EasyUtilitario.Helper.Pagina.PathSite() + oEasyNavigatorBE.Url, false);
        }

        private void ColleccionarNavegacion(EasyNavigatorBE oEasyNavigatorBE) {
            if (ExisteInLast(oEasyNavigatorBE) == false)
            {
                this.History = getHistorial();
                if (this.History == null)
                {
                    this.History = new List<EasyNavigatorBE>();
                }
                this.History.Add(oEasyNavigatorBE);
                setHistorial(this.History);
            }
        }


        public void Atras() {
            this.History = getHistorial();
            if (this.History != null)
            {
                EasyNavigatorBE oEasyNavigatorBE = (EasyNavigatorBE)this.History[this.History.Count - 1];
                this.History.Remove(oEasyNavigatorBE);
                oEasyNavigatorBE = (EasyNavigatorBE)this.History[this.History.Count - 1];
                setHistorial(this.History);
                Go(oEasyNavigatorBE);
                //((System.Web.UI.Page)HttpContext.Current.Handler).Response.Redirect(EasyUtilitario.Helper.Pagina.PathSite() + oEasyNavigatorBE.Url, false);
            }
        }
        void Go(EasyNavigatorBE oEasyNavigatorBE) {
            string _url = oEasyNavigatorBE.Pagina + EasyUtilitario.Constantes.Caracteres.SignoInterrogacion + oEasyNavigatorBE.ToString();
            ((System.Web.UI.Page)HttpContext.Current.Handler).Response.Redirect(EasyUtilitario.Helper.Pagina.PathSite() + _url, false);
        }

        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        #endregion








    }
}
