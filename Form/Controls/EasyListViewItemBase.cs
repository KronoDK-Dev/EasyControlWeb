using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Form.Controls
{
    public class EasyListViewItemBase
    {
        private string src;
        private string key;
        private string text;
        private string _value;
        private string _url;
        private int _idEstado;
        private int _NroMsg;

        public int NroMsg { 
            get { return _NroMsg; }
            set { _NroMsg = value; }
        }
        public string Src
        {
            get { return src; }
            set { src = value; }
        }
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        public int IdEstado {
            get { return _idEstado; }
            set { _idEstado = value; }
        }

        public Dictionary<string, string> DataComplete { get; set; }

        public EasyListViewItemBase() { }
        public EasyListViewItemBase(string _url, string _text, string _value)
        {
            this.Text = _text;
            this.Value = _value;
        }
        public string ToString(bool ver)
        {
            string Structura = "";int idx = 0;
            foreach (var propertyInfo in this.GetType().GetProperties())
            {
                if (propertyInfo.Name.Equals("DataComplete"))
                {
                    string strdc = "";int i = 0;
                    bool ConValor = ((Dictionary<string, string>)propertyInfo.GetValue(this, propertyInfo.GetIndexParameters())) == null;
                    if (ConValor == false)
                    {
                        Dictionary<string, string> dc = (Dictionary<string, string>)propertyInfo.GetValue(this, propertyInfo.GetIndexParameters());
                        foreach (var item in dc)
                        {
                            strdc += ((i == 0) ? "" : ",") + item.Key + ":" + EasyUtilitario.Constantes.Caracteres.ComillaDoble + item.Value + EasyUtilitario.Constantes.Caracteres.ComillaDoble;
                            i++;
                        }
                        Structura += ",DataComplete:{" + strdc + "}";
                    }
                    else
                    {
                        Structura += ",DataComplete:null";
                    }
                }
                else
                {
                    Structura += ((idx == 0) ?"":EasyUtilitario.Constantes.Caracteres.Coma) + propertyInfo.Name + ":" + EasyUtilitario.Constantes.Caracteres.ComillaDoble + propertyInfo.GetValue(this, propertyInfo.GetIndexParameters()) + EasyUtilitario.Constantes.Caracteres.ComillaDoble;
                }
                idx++;
            }
            return "{" + Structura + "}";
        }

    }
}
