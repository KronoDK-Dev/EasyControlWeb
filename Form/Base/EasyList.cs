using EasyControlWeb.Filtro;
using EasyControlWeb.Form.Controls;
using EasyControlWeb.Form.Estilo;
using EasyControlWeb.InterConeccion;
using EasyControlWeb.InterConecion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Base
{
    public class EasyList : DropDownList
    {

        private bool cargaInmediata;
        [Category("Validación"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public bool CargaInmediata { get { return cargaInmediata; } set { cargaInmediata= value; } }


        [Category("Validación"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Etiqueta { get; set; }

        private bool requerido;
        [Category("Validación"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public bool Requerido { get { return ((ViewState["ddlR" + this.ClientID] ==null)? requerido: (bool)ViewState["ddlR" + this.ClientID]); } set { ViewState["ddlR" + this.ClientID] = value; requerido = value; } }

        private string mensajeValida;
        [Category("Validación"), Description("Descripcion que se debe mostrar si el campo es requerido u obligatorio")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string MensajeValida { get { return mensajeValida; } set { mensajeValida = value; } }



        Bootstrap oBootstrap = new Bootstrap();
        [TypeConverter(typeof(Type_Style))]
        [Description("Define estilo vigente para cada control"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public Bootstrap EasyStyle
        {
            get { return oBootstrap; }
            set { oBootstrap = (Bootstrap)value; }
        }

        EasyDataInterConect oEasyDataInterConect = new EasyDataInterConect();
        [TypeConverter(typeof(Type_DataInterConect))]
        [Category("Editor"),
           Description("Conexion a un servicios de donde se obtiene los datos."),
           DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public EasyDataInterConect DataInterconect
        {
            get { return oEasyDataInterConect; }
            set { oEasyDataInterConect = value; }
        }


        /*Propiedad de collecion de cadenas*/
        [Browsable(true)]
        private List<EasyControlBE> easyCtrlDepend;
        [Category("Behavior"),
        Description("Lista de Nombre de Controles relacionados"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(EasyCollectionNomControls), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyControlBE> EasyCtrlDepend
        {
            get
            {
                if (easyCtrlDepend == null)
                {
                    easyCtrlDepend = new List<EasyControlBE>();
                }
                return easyCtrlDepend;
            }
        }





        public EasyList()
        {
            this.CssClass = EasyStyle.ClassName;
            this.Attributes.Add("autocomplete", "off");
            this.Attributes.Add("data-validate", "true");
        }
        public void LoadData() {
            if (CargaInmediata)
            {
                if ((this.DataInterconect.UrlWebServicieParams != null) && (this.DataInterconect.UrlWebServicieParams.Count != 0))
                {
                    int pos = 0;
                    object[] param = new object[this.DataInterconect.UrlWebServicieParams.Count];
                    foreach (EasyFiltroParamURLws oEasyFiltroParamURLws in this.DataInterconect.UrlWebServicieParams)
                    {
                        string Valor = "";
                        if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.DinamicoPorURL)
                        {
                            Valor = ((System.Web.UI.Page)HttpContext.Current.Handler).Request.Params[oEasyFiltroParamURLws.ParamName].ToString();
                        }
                        else if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.Fijo)
                        {
                            Valor = oEasyFiltroParamURLws.Paramvalue;
                        }
                        else if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.Session)
                        {
                            string NSesion = oEasyFiltroParamURLws.Paramvalue.ToString().Trim();
                            switch (NSesion)
                            {
                                case "IdUsuario":
                                    Valor = ((EasyUsuario)EasyUtilitario.Helper.Sessiones.Usuario.get()).IdUsuario.ToString();
                                    break;
                                case "UserName":
                                    Valor = ((EasyUsuario)EasyUtilitario.Helper.Sessiones.Usuario.get()).Login;
                                    break;
                                default:
                                    Valor = ((System.Web.UI.Page)HttpContext.Current.Handler).Session[NSesion].ToString();
                                    break;
                            }
                        }

                        else if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.FormControl)
                        {
                            string NomCtrlContext = this.Attributes["CtrlContext"];
                            string NomCtrl = oEasyFiltroParamURLws.Paramvalue;
                            Valor = NomCtrl;
                        }
                        switch (oEasyFiltroParamURLws.TipodeDato)
                        {
                            case EasyUtilitario.Enumerados.TiposdeDatos.String:
                                param[pos] = Valor;
                                break;
                            case EasyUtilitario.Enumerados.TiposdeDatos.Int:
                                param[pos] = Convert.ToInt32(Valor);
                                break;
                            case EasyUtilitario.Enumerados.TiposdeDatos.Double:
                                param[pos] = Convert.ToDouble(Valor);
                                break;
                            case EasyUtilitario.Enumerados.TiposdeDatos.Date:
                                param[pos] = Convert.ToDateTime(Valor);
                                break;
                        }
                        pos++;
                    }
                    string CadenaConexion = "";
                    if (this.DataInterconect.MetodoConexion == EasyDataInterConect.MetododeConexion.WebServiceInterno)
                    {
                        CadenaConexion = EasyUtilitario.Helper.Pagina.PathSite() + this.DataInterconect.UrlWebService;
                    }
                    else
                    {
                        CadenaConexion = this.DataInterconect.UrlWebService;//aqui hay un error
                    }
                    DataTable dt = (DataTable)EasyWebServieHelper.InvokeWebService(CadenaConexion, "", DataInterconect.Metodo, param);

                    // DataTable dt = (DataTable)EasyUtilitario.Helper.Data.getResultInterConect(DataInterconect);
                    this.DataSource = dt;
                    this.DataBind();
                }
                ListItem litem = new ListItem("[Seleccionar...]", "-1");
                this.Items.Insert(0, litem);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Attributes["disabled"] = ((this.Enabled) ? null : "disabled");
            if (this.Enabled == false)
            {
                this.Style["border-color"] = "#C0C0C0";
                this.Style["color"] = "#0000";
            }
        }
     
     }
}
