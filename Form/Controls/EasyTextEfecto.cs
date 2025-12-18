using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Permissions;
using System.Collections;
using System.Drawing.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.Design;
namespace EasyControlWeb.Form.Controls
{
    public class EasyTextEfecto : CompositeControl
    {
        HtmlGenericControl _h1;
        public string Texto
        {
            get
            {
                if (this.ViewState["txt"] == null)
                {
                    this.ViewState["txt"] = "Bienvenido...";
                }

                return (string)this.ViewState["txt"];
            }
            set
            {
                this.ViewState["txt"] = value;
            }
        }

    string MiStyle() {
            string _ScriptStyle= @"<style>
                                h1 {
                                    height: 100px;
                                }
  
                                h1 span {
                                    position: relative;
                                    top: 20px;
                                    display: inline-block;
                                    animation: bounce .3s ease infinite alternate;
                                    font-family: 'Titan One'cursive;
                                    font-size: 80px;
                                    color:   #fff;
                                    text-shadow: 0 1px 0 #CCC,
                                        0 2px 0 #CCC,
                                        0 3px 0 #CCC,
                                        0 4px 0 #CCC,
                                        0 5px 0 #CCC,
                                        0 6px 0 transparent,
                                        0 7px 0 transparent,
                                        0 8px 0 transparent,
                                        0 9px 0 transparent,
                                        0 10px 10px rgb(0 38  255);
                
                                }
  
                                h1 span:nth-child(2) {
                                    animation-delay: .1s;
                                }
  
                                h1 span:nth-child(3) {
                                    animation-delay: .2s;
                                }
  
                                h1 span:nth-child(4) {
                                    animation-delay: .3s;
                                }
  
                                h1 span:nth-child(5) {
                                    animation-delay: .4s;
                                }
  
                                h1 span:nth-child(6) {
                                    animation-delay: .5s;
                                }
  
                                h1 span:nth-child(7) {
                                    animation-delay: .6s;
                                }
  
                                h1 span:nth-child(8) {
                                    animation-delay: .7s;
                                }
  
                                @keyframes bounce {
                                    100% {
                                        top: -20px;
                                        text-shadow: 0 1px 0 #CCC,
                                            0 2px 0 #CCC,
                                            0 3px 0 #CCC,
                                            0 4px 0 #CCC,
                                            0 5px 0 #CCC,
                                            0 6px 0 #CCC,
                                            0 7px 0 #CCC,
                                            0 8px 0 #CCC,
                                            0 9px 0 #CCC,
                                            0 50px 25px rgba(0, 0, 0, 0.2);
                                    }
                                }
                        </style>
                ";
            return _ScriptStyle;
        }


        protected override void CreateChildControls()
        {

            Controls.Clear();
            _h1 = new HtmlGenericControl("h1");
            foreach (char caracter in this.Texto) {
                HtmlGenericControl _span = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span");
                _span.InnerText = caracter.ToString();
                _h1.Controls.Add(_span);
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            (new LiteralControl(MiStyle())).RenderControl(writer);
            _h1.RenderControl(writer);
        }
    }
}
