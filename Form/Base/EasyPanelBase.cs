using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Base
{
  
    public  class EasyPanelBase:CompositeControl
    {
        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]

        public string fncItemOnClick { get; set; }

        #region evento naturales
        protected override void OnInit(EventArgs e)
        {
            // base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
                //Registra el Script
                string fncExterna = ((this.fncItemOnClick == null) ? "" : this.fncItemOnClick + "(oUser);");
                string scriptBase = @"<script>
                                              //Script del COntrol Base o heredado 
                                      </script>
                                    ";
                (new LiteralControl(scriptBase)).RenderControl(writer);

            }
            else
            {
                (new LiteralControl("PanerlBase")).RenderControl(writer);
            }
        }
        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        #endregion


    }
}
