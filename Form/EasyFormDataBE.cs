using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Form
{
    public class EasyFormDataBE
    {
        /* public enum ModoEdit
         {
             N,
             M,
             E,
             C
         }*/

        // public ModoEdit ModoEdicion { get; set; }
        public EasyUtilitario.Enumerados.ModoPagina ModoEdicion { get; set; }
         

        public  Dictionary<string, string> Data = new Dictionary<String, string>();

        public EasyFormDataBE()
        : this(EasyUtilitario.Enumerados.ModoPagina.N)
        {
        }
        public EasyFormDataBE(EasyUtilitario.Enumerados.ModoPagina _ModoEdiciion)
        {
            ModoEdicion = _ModoEdiciion;
        }
        public KeyValuePair<string, string> getRow() {
            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>();
            foreach (KeyValuePair<string, string> kvpUnique in Data)
            {
                kvp = kvpUnique;                
            }
            return new KeyValuePair<string, string>(kvp.Key, kvp.Value); ;
        }

}
}
