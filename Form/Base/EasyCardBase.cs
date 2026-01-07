using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Base
{
    public  class EasyCardBase: CompositeControl
    {
        public EasyCardBase() { }

      //  public string Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }
}
