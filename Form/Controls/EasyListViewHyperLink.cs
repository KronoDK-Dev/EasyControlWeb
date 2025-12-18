using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Form.Controls
{
    public class EasyListViewHyperLink:EasyListViewItemBase
    {
        #region Propiedades
        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        #endregion

        public EasyListViewHyperLink() { }
        public EasyListViewHyperLink(string _url, string _text, string _value)
        {
            this.Url = _url; 
            this.Text = _text;
            this.Value = _value;
        }
    }
}
