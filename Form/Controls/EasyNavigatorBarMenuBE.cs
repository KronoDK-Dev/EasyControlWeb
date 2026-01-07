using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Form.Controls
{
    public class EasyNavigatorBarMenuBE
    {

        public EasyNavigatorBarMenuBE() : this(0,0, String.Empty, String.Empty, String.Empty,0) { }
        public EasyNavigatorBarMenuBE(int _IdOpcion,int _IdOpcionPadre, string _Nombre,string _Descripcion,string _RutaPag,int _NroSubItems)
        {
            this.IdOpcion = _IdOpcion;
            this.IdOpcionPadre = _IdOpcionPadre;
            this.Nombre = _Nombre;
            this.Descripcion = _Descripcion;
            this.RutaPag = _RutaPag;
            this.NroSubItems = _NroSubItems;
        }

        public int IdOpcion { get; set; }
        public int IdOpcionPadre { get; set; }
        public string Nombre{ get; set; }
        public string Descripcion { get; set; }
        public string RutaPag { get; set; }
        public int NroSubItems { get; set; }
        public int AnchoGrp { get; set; }

    }
}
