using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Controls
{
    //http://www.binaryintellect.net/articles/c1eee649-4e4b-486a-9fd3-2151d6821f0e.aspx
    //https://www.cyotek.com/blog/creating-a-custom-typeconverter-part-1
    [TypeConverter(typeof(DataConfig_TypeC))]
    [Serializable]
    public class DataDemoBE
    {
        private int idUsuario;
        private string login;

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public int IdUsuario {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Login {
            get
            {
                return login;
            }
            set
            {
                login= value;
            }
        }

        public DataDemoBE() { }
        public DataDemoBE(int _idUsuario, string _login) {
            idUsuario = _idUsuario;
            login = _login;
        }
    }
}
