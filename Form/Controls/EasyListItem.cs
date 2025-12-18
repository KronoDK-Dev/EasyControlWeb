using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Controls
{
    public class EasyListItem:EasyListViewItemBase
    {

        public EasyListItem() {}
        public EasyListItem(string _url,string _text,string _value)
        {
            this.Text=_text;
            this.Value = _value;
        }
       
    }
}
