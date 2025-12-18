using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyControlWeb.Form.Controls
{
    public  class EasyPathItem
    {
        public string Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string ClassName { get; set; }

        public EasyPathItem() { }

    }
}
