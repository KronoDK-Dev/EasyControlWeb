using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text; 

namespace EasyControlWeb.Filtro
{
    [Serializable]
    public class EasyFiltroParamURLws
    {
        /* public enum ParamTipo
         {
             Fijo,
             DinamicoPorURL,
             FormControl,
             CriterioInput,
         }*/
        public enum TipoObtenerValor
        {
            Fijo,
            DinamicoPorURL,
            FormControl,
            Session,
            CriterioInput,
            FunctionScript,
        }
      

        public EasyFiltroParamURLws() : this(string.Empty, TipoObtenerValor.Fijo) { }
        public EasyFiltroParamURLws(string _Param, TipoObtenerValor _Tipo) {
            this.ParamName = _Param;
            this.ObtenerValor = _Tipo;
        }
        public EasyFiltroParamURLws(string _Param, TipoObtenerValor _ObtenerValor, EasyUtilitario.Enumerados.TiposdeDatos  _TipodeDato)
        {
            this.ParamName = _Param;
            this.ObtenerValor = _ObtenerValor;
            this.TipodeDato = _TipodeDato;
        }

        [Category("Params")]
        [Browsable(true)]
        [Description("Nombre de parámetro")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string ParamName { get; set; }

        [Category("Params")]
        [Browsable(true)]
        [Description("Valor de parámetro")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Paramvalue{ get; set; }

        [Category("Params")]
        [Browsable(true)]
        [Description("Como se debe de obtener el valor del parámetro definido")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public TipoObtenerValor ObtenerValor { get; set; }

        [Category("Params")]
        [Browsable(true)]
        [Description("Tipo de dato del parámetro")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public EasyUtilitario.Enumerados.TiposdeDatos TipodeDato { get; set; }

        public override string ToString()
        {
            return "EasyFiltroParam." + this.ParamName;
        }
        public string ToString(bool outClienteBE)
        {
           string cmll = "\"";
           string strBaseBE = "{"
                            + "ParamName"  + ":" + cmll + this.ParamName + cmll + ","
                            + "Paramvalue" + ":" + cmll + this.Paramvalue + cmll + ","
                            + "ObtenerValor" + ":" + cmll + this.ObtenerValor.ToString() + cmll + "}";
            return strBaseBE.ToString();
        }
    }
}
