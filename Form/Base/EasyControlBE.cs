using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq; 
using System.Text;

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
