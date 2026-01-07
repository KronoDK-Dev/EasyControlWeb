using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI; 

namespace EasyControlWeb.Filtro
{
    public class EasyCampoListaSelect
    {
        public enum TipoCampoSelected { 
            SelectValor,
            SelectTexto,
            Campo,
        }
        private string campo;
        private string textHeader;
        private TipoCampoSelected tipoSeleccion;
        private int orden;


        public EasyCampoListaSelect()
           : this(String.Empty, String.Empty,0)
        {
        }

        public EasyCampoListaSelect(string _Campo, string _TextHeader, int _Orden)
        {
            Campo = _Campo;
            TextHeader = _TextHeader;
            orden = _Orden;
        }

        [
          Category("Behavior"),
          DefaultValue(""),
          Description("Campo que contiene el ID del registro el cul sera tomado al momento de ser seleccionado"),
          NotifyParentProperty(true),
        ]
        public string Campo { get { return campo; } set { this.campo = value; } }

        [
          Category("Behavior"),
          DefaultValue(""),
          Description("Campo contiene texto descriptivo del registro"),
          NotifyParentProperty(true),
        ]
        public string TextHeader { get { return textHeader; } set { this.textHeader = value; } }

        [
        Category("Behavior"),
        DefaultValue(""),
        Description("Campo el tipo de atributo del registro"),
        NotifyParentProperty(true),
        ]
        public TipoCampoSelected TipoSeleccion  { get { return tipoSeleccion; } set { this.tipoSeleccion = value; } }

        public int Orden { get { return orden; } set { this.orden = value; } }

        public override string ToString()
        {
            return "EasyCampoListaSelect";
        }
        public string ToString(bool outClienteBE)
        {
            string cmll = "\"";
            string strBaseBE = "{"
                 + "Campo" + ":" + cmll + this.Campo + cmll + ","
                 + "TextHeader" + ":" + cmll + this.TextHeader + cmll + ","
                 + "TipoCampoSelected" + ":" + cmll + this.TipoSeleccion.ToString() + cmll + ","
                 + "Orden" + ":" + cmll + this.Orden + cmll + "}";

            return strBaseBE.ToString();
        }

    }
}
