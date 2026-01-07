using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyControlWeb.Form.Controls
{
    public class EasyButtonMenuContext: EasyGridButton
    {
      [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
      public new string Ubicacion { get; set; }


        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string SolicitaConfirmar { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string RequiereSelecciondeReg { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string MsgConfirm { get; set; }

        
    }
}
