using EasyControlWeb.Form.Controls;
using EasyControlWeb.Form.Editor;
using EasyControlWeb.Form.Estilo;
using System; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Base
{
    public class EasyTexto:TextBox
    {

        private bool enableOnChange;
        [Category("Validación"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public bool EnableOnChange { get { return enableOnChange; } set { enableOnChange = value; } }

        [Category("Validación"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Etiqueta { get; set; }

        private bool requerido;
        [Category("Validación"), Description("Activa la validacion de campos obligatorios")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public bool Requerido { get { return requerido; } set { requerido = value; } }

       
        private string mensajeValida;
        [Category("Validación"), Description("Descripcion que se debe mostrar si el campo es requerido u obligatorio")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string MensajeValida { get { return mensajeValida; } set { mensajeValida = value; } }


        Bootstrap oBootstrap = new Bootstrap();
        [TypeConverter(typeof(Type_Style))]
        [Description("Define estilo vigente para cada control"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public Bootstrap EasyStyle
        {
            get{return oBootstrap;}
            set{oBootstrap = (Bootstrap)value;}
        }

       

        public EasyTexto() {
            this.CssClass = EasyStyle.ClassName;
            this.Attributes.Add("autocomplete", "off");
            this.Attributes.Add("data-validate", "true");
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Attributes["disabled"] = ((this.Enabled) ?null: "disabled" );
            if (this.Enabled == false)
            {
                this.Style["border-color"] = "#C0C0C0";
                this.Style["color"] = "#0000";
            }
        }

    }
}
