using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace EasyControlWeb.Form.Controls
{
   [
    DefaultProperty("LstCtrlValue"),
    ParseChildren(true, "LstCtrlValue"),
    ToolboxData("<{0}:EasyNavigatorBE runat=server></{0}:EasyNavigatorBE>")
    ]
    [Serializable] 
    public class EasyNavigatorBE
    {
        public string Texto { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }

        public string Pagina { get; set; }


        private List<EasyNavigatorParam> easyParams;
        public List<EasyNavigatorParam> Params
        {
            get
            {
                if (easyParams == null)
                {
                    easyParams = new List<EasyNavigatorParam>();
                }
                return easyParams;
            }
        }




        public string LstCtrlValue { get; set; }
        public object LstCtrlValues { get; set; }



        public EasyNavigatorBE(): this(String.Empty){}
        public EasyNavigatorBE(string _Texto)
        {
            Texto = _Texto;
        }
        public override string ToString()
        {
            int idx = 0;
            string _Params = "";
            if (this.Params != null)
            {
                foreach (EasyNavigatorParam oParam in this.Params)
                {
                    _Params += ((idx == 0) ? "" : EasyUtilitario.Constantes.Caracteres.SignoAmperson) + oParam.ToString();
                    idx++;
                }
            }
            return _Params;
        }
    }
}
