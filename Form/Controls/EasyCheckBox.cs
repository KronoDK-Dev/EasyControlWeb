using EasyControlWeb.Form.Estilo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Controls
{
    public  class EasyCheckBox:CheckBox
    {

        //reference: https://stackoverflow.com/questions/63192157/html-change-the-background-color-of-a-checked-checkbox
        public EasyCheckBox() : base()
        {
        }

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Placeholder { get; set; }

        

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
            get { return oBootstrap; }
            set { oBootstrap = (Bootstrap)value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Attributes["disabled"] = ((this.Enabled) ? null : "disabled");
            if (this.Enabled == false)
            {
                this.Style["border-color"] = "#C0C0C0";
                this.Style["color"] = "#0000";
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.Attributes.Add("class", this.CssClass);
            this.Attributes.Add("placeholder", this.Placeholder);
           // this.Style.Add("background", "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconTextBox + "') right center no-repeat; padding-right:5px;");
            base.Render(writer);

            string scriptGetSet = this.ClientID + @".GetValue=function(){
                                                return jNet.get('" + this.ClientID + @"').checked;
                                        }
                                    " + this.ClientID + @".SetValue=function(value){
                                                jNet.get('" + this.ClientID + @"').checked=value;
                                        }
                                    ";

            (new LiteralControl("<script>\n" + scriptGetSet + "\n" + "</script>\n")).RenderControl(writer);
        }
    }
}
