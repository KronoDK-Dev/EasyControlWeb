using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.ComponentModel;
using EasyControlWeb.Test;
using static EasyControlWeb.EasyTypeConvertGeneric;
using System.Data;
using System.Web;

namespace EasyControlWeb.Form.Controls
{
    [TypeConverter(typeof(UsuarioConfiguracion_TypeC))]
    public class EasyUsuario
    {

        private string encryptedPwd;
        public int IdUsuario { get; set; }
        public string Login { get; set; }
        public string Password {
            set{encryptedPwd = EasyEncrypta.Encriptar(value);}
            get { return encryptedPwd; }
        }
        public int IdCentroOperativo { get; set; }
        public string ApellidosyNombres { get; set; }
        public string NroDocumento { get; set; }
        public string Email { get; set; }

        public void SaveOpcions(DataTable dtOpciones) {
            ((System.Web.UI.Page)HttpContext.Current.Handler).Session["OpMenu"] = dtOpciones;
        }
        public DataTable getOpcions()
        {
            return (DataTable)((System.Web.UI.Page)HttpContext.Current.Handler).Session["OpMenu"];
        }
        public bool ValidaPagina() {
            try{
                string[] PagSplit = ((System.Web.UI.Page)HttpContext.Current.Handler).Request.Url.AbsolutePath.Split('/');
                string Pagina = PagSplit[PagSplit.GetUpperBound(0)];
                return ValidaPagina(Pagina);
            }
            catch(Exception ex){
                return false;

            }
        }
        public bool ValidaPagina(string PagName) {
            DataTable dt = this.getOpcions();
            if (dt != null) {
                DataRow []dr = dt.Select("DATOS = '" + PagName + "'");
                int Count = dr.Count();
                return ((Count==0)?false:true);
            }
            return false;
        }

        public override string ToString()
        {
            return $"{IdUsuario} {Login} {ApellidosyNombres} {NroDocumento}{Email}";
        }
        public string ToString(bool MapData)
        {
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaSimple;
            string Item = "{IdUsuario:" + cmll + IdUsuario + cmll + ", Login:" + cmll + Login + cmll + ",ApellidosyNombres:" + cmll + ApellidosyNombres + cmll + ",NroDocumento:" + cmll + NroDocumento + cmll + ",Email:" + cmll + Email + cmll + "}";
            return Item;

        }
        public string ToCliente()
        {
            int idx = 0;
            string Structura = "";
            foreach (var propertyInfo in this.GetType().GetProperties())
            {
                Structura += ((idx == 0) ? "" : EasyUtilitario.Constantes.Caracteres.Coma) + propertyInfo.Name + EasyUtilitario.Constantes.Caracteres.DosPuntos + propertyInfo.GetValue(this, propertyInfo.GetIndexParameters());
                idx++;
            }
            return "{" + Structura + "}";
        }
        }
    }
