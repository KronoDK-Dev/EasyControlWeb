using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data; 
using System.Security.Permissions;
using System.Collections;
using System.Drawing.Design;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using EasyControlWeb.Form.Base;
using System.Xml.Linq;

namespace EasyControlWeb.Form.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:EasyNumericBox runat=server></{0}:EasyNumericBox>")]
    public class EasyNumericBox: EasyTexto//TextBox
    {
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public override string Text
        {
            get{return base.Text;}
            set{base.Text = value;}
        }

        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public int NroDecimales { get; set; }


        [Category("Mensaje"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Placeholder { get; set; }


        //Ocutar Propiedades
        public EasyNumericBox() : base()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            string JavaScriptCode = "";
            if (NroDecimales == 0)
            {
              
                    JavaScriptCode = @"<script>
                                                function setInputFilter(textbox, inputFilter){
                                                        ['input','keydown','keyup','mousedown','mouseup','select','contextmenu','drop'].forEach(function(event) {
                                                                    textbox.addEventListener(event, function() {
                                                                        if (inputFilter(this.value)) {
                                                                            this.oldValue = this.value;
                                                                            this.oldSelectionStart = this.selectionStart;
                                                                              this.oldSelectionEnd = this.selectionEnd;
                                                                          } else if (this.hasOwnProperty('oldValue')){
                                                                                this.value = this.oldValue;
                                                                                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                                                                          } else {
                                                                             this.value ='';
                                                                          }
                                                                    });
                                                        });
                                                }
                                            </script>";
                if (!Page.IsClientScriptBlockRegistered("fncNumericBox"))
                {
                    Page.RegisterClientScriptBlock("fncNumericBox", JavaScriptCode);
                }
            }

            base.OnInit(e);
        }



        protected override void Render(HtmlTextWriter writer)
        {
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            this.Attributes.Add("placeholder", this.Placeholder);
            this.Attributes["class"] = this.CssClass;
            this.Style.Add("background", "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconNumeric + "') right center no-repeat; padding-right:5px;");
            base.Render(writer);
           
            char bs= EasyUtilitario.Constantes.Caracteres.BackSlash;
            char backslash = @"\"[0];

            string ScriptExec = "";

            if (NroDecimales > 0)
            {
                ScriptExec = "$(" + cmll + "#" + ClientID + cmll + @").on({
                                        " + cmll + "focus" + cmll + @": function(event){
                                                                        $(event.target).select();
                                                                    },
                                        " + cmll + "keyup" + cmll + @": function(event){
                                                                        $(event.target).val(function(index, value) {
                                                                                                return value.replace(/" + bs + "D/g, " + cmll + cmll + @")
                                                                                                            .replace(/([0-9])([0-9]{3})$/,'$1.$2')
                                                                                                            .replace(/" + bs + @"B(?=(" + bs + @"d{3})+(?!" + bs + @"d)" + bs + ".?)/g, " + cmll + "," + cmll + @");
                                                                                            });
                                                                    }
                                });";
                (new LiteralControl("<script>\n" + ScriptExec + "\n" + "</script>\n")).RenderControl(writer);

            }
            else
            {
                ScriptExec = "setInputFilter(document.getElementById(" + cmll + ((this.ClientID == null) ? this.ID : this.ClientID) + cmll + "), function (value) {\n"
                                    + "                                                                        return  /^" + bs + "d*" + bs + ".?" + bs + "d*$/.test(value); \n"
                                    + "                                                                 });\n";

                (new LiteralControl("<script>\n" + ScriptExec + "\n" + "</script>\n")).RenderControl(writer);
            }

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

        protected override void CreateChildControls()
        {
        
        }
      
    }
}
