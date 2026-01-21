using EasyControlWeb.Filtro;
using EasyControlWeb.Form.Base;
using EasyControlWeb.InterConeccion;
using EasyControlWeb.InterConecion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Controls
{


    [DefaultProperty("fnOnSelected")]
    [ToolboxData("<{0}:EasyDropdownList runat=server></{0}:EasyDropdownList>")]
    [Serializable]
    public class EasyDropdownList : EasyList
    {
        TextBox txtText = new TextBox();
        TextBox txtVal = new TextBox();

        [Browsable(false)]
        public void SetReadOnly()
        {
            this.Enabled = false;
        }

        [Browsable(false)]
        public void SetValue(string Text, string Value)
        {
            ListItem item = this.Items.FindByValue(Value);
            if (item != null)
            {
                item.Selected = true;
                txtText.Text = item.Text;
                txtVal.Text = item.Value;

            }
        }
        public void SetText(string Text, string Value)
        {
            ListItem item = this.Items.FindByText(Text);
            if (item != null)
            {
                item.Selected = true;
                txtText.Text = item.Text;
                txtVal.Text = item.Value;

            }
        }
        [Browsable(false)]
        public void SetValue(string Value)
        {
            SetValue("", Value);
        }
        public void SetText(string Value)
        {
            SetText(Value, "");
        }
        public string getText() { return ((txtText.Text.Length > 0) ? txtText.Text : this.SelectedItem.Text); }

        public string getValue() { return ((txtVal.Text.Length > 0) ? txtVal.Text : this.SelectedValue); }

        #region Propiedades
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string fnEventoExterno = "";
            List<LiteralControl> ScriptCollection = new List<LiteralControl>();
            [Category("Funcionalidad"), Description("function javascript para enlazar al evento de seleccion define 1 parametro que es la clase que contiene el registro seleccionado"), DefaultValue("")]
            [RefreshProperties(RefreshProperties.All)]
            [NotifyParentProperty(true)]
            public string fnOnSelected { get; set; }

            public void RemoveItem(string ItemValue)
            {
                ListItem litem = this.Items.FindByValue(ItemValue);
                if (litem != null)
                {
                    this.Items.Remove(litem);
                }
            }

        #endregion

        public EasyDropdownList() : base()
        {
            if (txtVal == null) { txtVal = new TextBox(); }
            if (txtText == null) { txtText = new TextBox(); }
        }
        protected override void CreateChildControls()
        {
            txtVal.ID = this.ClientID + "_Value";
            txtVal.Style.Add("display", "none");
            txtText.ID = this.ClientID + "_Text";
            txtText.Style.Add("display", "none");
        }
        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onchange.ToString()] = this.ClientID + "_OnChange(this);";

        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (!IsDesign())
            {
                //functiones de lectura
                string scriptGet = this.ClientID + @".GetValue=function(){
                                                                var e = jNet.get('" + this.ClientID + @"');
                                                                return  e.options[e.selectedIndex].value;
                                                            }
                                    " + this.ClientID + @".GetText=function(){
                                                                var e = jNet.get('" + this.ClientID + @"');
                                                                return  e.options[e.selectedIndex].text;
                                                            }
                                    " + this.ClientID + @".FindValue=function(value){
                                                                " + this.ClientID + @".FindItem(0,value);                          
                                                            }
                                    " + this.ClientID + @".FindText=function(value){
                                                                " + this.ClientID + @".FindItem(1,value);                          
                                                            }
                                    " + this.ClientID + @".FindItem=function(TipoFind,value){
                                                               var dd = jNet.get('" + this.ClientID + @"');
                                                                for (var i = 0; i < dd.options.length; i++) {
                                                                var opFind =((TipoFind==0)?dd.options[i].value:dd.options[i].text);
                                                                    if (opFind === value) {
                                                                        dd.selectedIndex = i;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                      " + this.ClientID + @".SetDefault=function(){
                                                                " + this.ClientID + @".FindItem(0,'-1');
                                                            }
                                    ";
                scriptGet.Replace("var Valor = var", "var"); // 24.12.2024 quitamos el error
                (new LiteralControl("\n <script>\n" + scriptGet + "\n" + "</script>\n")).RenderControl(writer);

                string scriptLoadData = this.ClientID + @".LoadData=function(){
                                       var oParamCollections = new SIMA.ParamCollections();
                                       var oParam = null;
                                  ";

                foreach (EasyFiltroParamURLws oParam in this.DataInterconect.UrlWebServicieParams)
                {
                    string Valor = "";
                    switch (oParam.ObtenerValor)
                    {
                        case EasyFiltroParamURLws.TipoObtenerValor.DinamicoPorURL:
                            Valor = "Valor = Page.Request.Params['" + oParam.Paramvalue + "']";
                            break;
                        case EasyFiltroParamURLws.TipoObtenerValor.FormControl:

                            if (!string.IsNullOrEmpty(oParam.Paramvalue)) // 23.12.2024 validad que tenga data para llenar la variable Valor
                            {
                                Valor = @"  var obj =jNet.get('" + oParam.Paramvalue + @"');
                                    var TipoObj = obj.tagName;
                                    switch(TipoObj){
                                        case 'SELECT':
                                                Valor = obj.options[obj.selectedIndex].value;
                                          
                                            break;
                                        default:
                                            Valor = obj.Value;
                                    }
                                ";

                            }

                            break;
                        case EasyFiltroParamURLws.TipoObtenerValor.Session:
                            Valor = "'" + ((System.Web.UI.Page)HttpContext.Current.Handler).Session[oParam.Paramvalue.ToString()].ToString() + "'";
                            break;
                        case EasyFiltroParamURLws.TipoObtenerValor.Fijo:
                            Valor = "'" + oParam.Paramvalue + "'";
                            break;
                        case EasyFiltroParamURLws.TipoObtenerValor.FunctionScript:
                            //Valor = "'" + oParam.Paramvalue + "'";
                            Valor = oParam.Paramvalue;
                            break;
                    }

                    if (!string.IsNullOrEmpty(Valor) && Valor != "''") // 23.12.2024 validad que tenga data para llenar la variable Valor
                    {
                        scriptLoadData += @"    var Valor =" + Valor + @";
                                        oParam = new SIMA.Param('" + oParam.ParamName + @"',  Valor ,'" + oParam.TipodeDato.ToString() + @"');
                                        oParamCollections.Add(oParam);
                                                                                                   ";
                    }
                }

                string CadenaConexion = "";
                if (this.DataInterconect.MetodoConexion == EasyDataInterConect.MetododeConexion.WebServiceInterno)
                {
                    CadenaConexion = EasyUtilitario.Helper.Pagina.PathSite() + this.DataInterconect.UrlWebService + "/" + this.DataInterconect.Metodo;//aqui hay un error
                }
                else
                {
                    CadenaConexion = EasyUtilitario.Helper.Pagina.PathSite() + this.DataInterconect.UrlWebService;//aqui hay un error
                }

                scriptLoadData += @"                      var ddl = jNet.get('" + this.ClientID + @"');
                                                            options = ddl.getElementsByTagName('option');
                                                            for (var i=options.length; i--;) {
                                                                ddl.removeChild(options[i]);
                                                            }



                                                         var oEasyDataInterConect = new EasyDataInterConect();
                                                             oEasyDataInterConect.MetododeConexion = ModoInterConect." + this.DataInterconect.MetodoConexion.ToString() + @";
                                                             oEasyDataInterConect.UrlWebService = '" + this.DataInterconect.UrlWebService + @"';
                                                             oEasyDataInterConect.Metodo = '" + this.DataInterconect.Metodo + @"';
                                                             oEasyDataInterConect.ParamsCollection = oParamCollections;

                                                            //Se conecta y Obtiene los datos en formato DataTable
                                                         var oEasyDataResult = new EasyDataResult(oEasyDataInterConect);
                                                         var oDataTable = new SIMA.Data.DataTable('tbl');
                                                             oDataTable = oEasyDataResult.getDataTable();


                                                            var opt = document.createElement('option');
                                                                opt.value = '0';
                                                                opt.innerHTML = '[Seleccionar...]';
                                                                ddl.appendChild(opt);

                                                            oDataTable.Rows.forEach(function(oDataRow) {
                                                                                        opt = document.createElement('option');
                                                                                            opt.value = oDataRow['" + this.DataValueField + @"'];
                                                                                            opt.innerHTML = oDataRow['" + this.DataTextField + @"'];
                                                                                            ddl.appendChild(opt);
                                                                                   });
                                }";


                scriptLoadData = scriptLoadData.Replace("var Valor =  var obj", "var obj");  // 24.12.2024 quitamos el error

                (new LiteralControl("\n <script>\n" + scriptLoadData + "\n" + "</script>\n")).RenderControl(writer);

                /**/
                string ddlOnChange = "";
                string fnEventoExterno = ((this.fnOnSelected != null) && (this.fnOnSelected.Length > 0) ? this.fnOnSelected + "(ListItem);" : "");
                if ((this.EasyCtrlDepend != null) && (this.EasyCtrlDepend.Count > 0))
                {
                    string lstCtrl = "";
                    int post = 0;
                    foreach (EasyControlBE oEasyControlBE in this.EasyCtrlDepend)
                    {
                        lstCtrl += ((post == 0) ? "" : ";") + oEasyControlBE.Nombre;
                        post++;
                    }

                    ddlOnChange = @" function " + this.ClientID + @"_OnChange(e){
                                                var ListItem= e.options[e.selectedIndex];
                                                jNet.get('" + this.ClientID + "_Text" + @"').value = ListItem.text;
                                                jNet.get('" + this.ClientID + "_Value" + @"').value = ListItem.value;
                                                 " + fnEventoExterno + @"
                                                    if('" + lstCtrl + @"'.Length>0){ 
                                                        var arrCtrl = '" + lstCtrl + @"'.split(';');
                                                        arrCtrl.forEach(function(str,i){
                                                                var objDep = jNet.get(str);
                                                                var TipoObj = objDep.tagName;
                                                                    switch(TipoObj){
                                                                        case 'SELECT':
                                                                            objDep.LoadData();
                                                                            break;
                                                                        default:
                                                                            Valor = obj.Value;
                                                                    }
                                                        });
                                                    }
                                }
                                ";
                }
                else
                {
                    ddlOnChange = @" function " + this.ClientID + @"_OnChange(e){
                                                var ListItem= e.options[e.selectedIndex];
                                                    jNet.get('" + this.ClientID + "_Text" + @"').value = ListItem.text;
                                                    jNet.get('" + this.ClientID + "_Value" + @"').value = ListItem.value;
                                                        " + fnEventoExterno + @"
                                    }
                                ";
                }

             (new LiteralControl("\n <script>\n" + ddlOnChange + "\n" + "</script>\n")).RenderControl(writer);

                txtText.RenderControl(writer);
                txtVal.RenderControl(writer);
            }

        }


    }
}
