using EasyControlWeb.Form.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI; 
using System.Windows;

namespace EasyControlWeb.Form.Controls
{
    public class EasyTextBox:EasyTexto
    {
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Placeholder { get; set; }
        #region Ocultar Attr EasyITemplateNumericBox
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
            public new int EnableOnChange { get; set; }
        #endregion

        public EasyTextBox() : base()
        {
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
           // this.Attributes["disabled"] = ((this.Activado)? "disabled":null);
        }
        public string GetValue() {
            return this.Text;
        }
        public void SetValue(string Value) { 
            this.Text = Value;
        }
        protected override void Render(HtmlTextWriter writer)
        {
            this.Attributes.Add("class", this.CssClass);
            
            this.Attributes.Add("placeholder", this.Placeholder);
            string backColor = ((ColorTranslator.ToHtml(this.BackColor).Length >0) ? ColorTranslator.ToHtml(this.BackColor) : "white");
            this.Style.Add("background", backColor + " url('" + EasyUtilitario.Constantes.ImgDataURL.IconTextBox + "') right center no-repeat; padding-right:5px;");
            base.Render(writer);

            string scriptGetSet = this.ClientID + @".GetValue=function(){
                                                return jNet.get('" + this.ClientID + @"').value;
                                        }
                                    " + this.ClientID + @".GetText=function(){
                                                return jNet.get('" + this.ClientID + @"').value;
                                        }
                                    " + this.ClientID + @".SetValue=function(value){
                                                jNet.get('" + this.ClientID + @"').value=value;
                                        }
                                    ";

            (new LiteralControl("<script>\n" + scriptGetSet + "\n" + "</script>\n")).RenderControl(writer);
        }
    }
}
