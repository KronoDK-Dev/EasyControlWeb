using EasyControlWeb.Filtro;
using EasyControlWeb.Form.Controls;
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

namespace EasyControlWeb.InterConeccion
{

   

    [TypeConverter(typeof(Type_DataInterConect))]
    [Serializable]
    public class EasyDataInterConect//:CompositeControl
    {
    public EasyDataInterConect() { }

        public enum MetododeConexion
        {
            WebServiceInterno,
            WebServiceExterno,
            PaginaASPX,
        }
        public EasyDataInterConect(string _UrlWebService,string _Metodo, List<EasyFiltroParamURLws> _UrlWebServicieParams) {
            this.UrlWebService= _UrlWebService;
            this.Metodo = _Metodo;
            this.easyParams = _UrlWebServicieParams;
        }
        public EasyDataInterConect(string _UrlWebService, string _Metodo)
        {
            this.UrlWebService = _UrlWebService;
            this.Metodo = _Metodo;
            this.easyParams = null;
        }


        private MetododeConexion metodoConexion;
        public MetododeConexion MetodoConexion {
            get { return metodoConexion; }
            set { metodoConexion=value; }
        }

        private string _configPathSrvRemoto;
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public string ConfigPathSrvRemoto
        {
            get
            {
                return _configPathSrvRemoto;
            }
            set
            {
                _configPathSrvRemoto = value;
            }
        }


        private string _UrlWebService;
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public string UrlWebService
        {
            get { 
                return _UrlWebService; 
            } 
            set {
                _UrlWebService = value; 
                } 
        }

        private string _Metodo;
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public string Metodo
        {
            get
            {
                return _Metodo;
            }
            set
            {
                _Metodo = value;
            }
        }
        //Campos para Seleccion de datos
        [Browsable(true)]
        private List<EasyFiltroParamURLws> easyParams;
        [Category("Behavior"),
        Description("Parámetros que utilizara el WebService/Metodo para procesar su ejecución"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(EasyFiltroCollectionParams), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyFiltroParamURLws> UrlWebServicieParams
        {
            get
            {
                if (easyParams == null) {
                    easyParams = new List<EasyFiltroParamURLws>();
                }
                return easyParams;
            }
        }


        private object[] ParamsGetValues() {
                int pos = 0;
                object[] param = new object[this.UrlWebServicieParams.Count];
                foreach (EasyFiltroParamURLws oEasyFiltroParamURLws in this.UrlWebServicieParams)
                {
                    string Valor = "";
                    if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.DinamicoPorURL)
                    {
                        Valor = ((System.Web.UI.Page)HttpContext.Current.Handler).Request.Params[oEasyFiltroParamURLws.ParamName].ToString();
                    }
                    else if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.FormControl)
                    {
                        Control oCtrl = ((System.Web.UI.Page)HttpContext.Current.Handler).FindControl(oEasyFiltroParamURLws.Paramvalue);
                        if (oCtrl is EasyNumericBox)
                        {
                            EasyNumericBox onb = (EasyNumericBox)oCtrl;
                            Valor = onb.Text;
                        }
                        if (oCtrl is EasyTextBox)
                        {
                            EasyTextBox otb = (EasyTextBox)oCtrl;
                            Valor = otb.Text;
                        }
                        else if (oCtrl is EasyDatepicker)
                        {
                            EasyDatepicker dpk = (EasyDatepicker)oCtrl;
                            Valor = dpk.Text;
                        }
                        else if (oCtrl is EasyAutocompletar)
                        {
                            EasyAutocompletar AC = (EasyAutocompletar)oCtrl;
                            Valor = AC.GetValue();

                        }
                        else if (oCtrl is EasyDropdownList)
                        {
                            EasyDropdownList ddl = (EasyDropdownList)oCtrl;
                            Valor = ddl.getValue();
                        }
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
                            case "IdCentro":
                                Valor = ((EasyUsuario)EasyUtilitario.Helper.Sessiones.Usuario.get()).IdCentroOperativo.ToString();
                                break;
                            default:
                                Valor = ((System.Web.UI.Page)HttpContext.Current.Handler).Session[NSesion].ToString();
                                break;
                        }
                    }
                   /*else if (oEasyFiltroParamURLws.ObtenerValor == EasyFiltroParamURLws.TipoObtenerValor.FormControl)
                    {
                        string NomCtrlContext = this.Attributes["CtrlContext"];
                        string NomCtrl = oEasyFiltroParamURLws.Paramvalue;
                        Valor = NomCtrl;
                    }*/
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
            return param;
        }


        private  object GetObjectInfo() {
            if ((this.UrlWebServicieParams != null) && (this.UrlWebServicieParams.Count != 0))
            {
                object[] param = ParamsGetValues();
                string CadenaConexion = "";
                if (this.MetodoConexion == EasyDataInterConect.MetododeConexion.WebServiceInterno)
                {
                    CadenaConexion = EasyUtilitario.Helper.Pagina.PathSite() + this.UrlWebService;
                }
                else
                {
                    if (this.ConfigPathSrvRemoto != null)
                    {//Seccion de COnfiguracion de los servidores remotos de servicios  que se puedan usar 

                        CadenaConexion = EasyUtilitario.Helper.Configuracion.Leer("RemotoServerWebService", this.ConfigPathSrvRemoto) + this.UrlWebService;
                    }
                    else
                    {
                        CadenaConexion = this.UrlWebService;//aqui hay un error
                    }
                }


                return EasyWebServieHelper.InvokeWebService(CadenaConexion, "", this.Metodo, param);
            }





                return null;
        }
        public DataTable GetDataTable() {
            return (DataTable) this.GetObjectInfo();
        }
        public EasyBaseEntityBE GetEntity()
        {
            object obj = this.GetObjectInfo();
            EasyBaseEntityBE oEasyBaseEntityBE = new EasyBaseEntityBE(obj);
            return oEasyBaseEntityBE;
        }
        public string SendData()
        {
            return (string)this.GetObjectInfo();
        }


    }
}
