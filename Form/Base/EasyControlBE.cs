using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyControlWeb.Form.Base
{
    public class EasyControlBE
    {
        [Category("Validación"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Nombre { get; set; }

        public EasyControlBE()
        {
        }
    }
}
