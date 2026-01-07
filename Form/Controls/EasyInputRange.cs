using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Controls
{
    public  class EasyInputRange: CompositeControl
    {
        EasyTextBox oTxtValor;
        //HTMLInputRange oInputRange;
        //Refrencia:https://www.jose-aguilar.com/blog/como-mover-una-barra-de-progreso-con-javascript/
        public EasyInputRange()
        {
            oTxtValor = new EasyTextBox();
            oTxtValor.SetValue("0");
        }
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public int Min { get; set; }

        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public int Max { get; set; }

        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public int Valor { get; set; }


        HtmlGenericControl HtmlInputRange()
        {
        
            HtmlGenericControl HTMLRange = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "range");
            HtmlGenericControl HTMLInputRange = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("Input", "rangeInput");
                HTMLInputRange.Attributes["type"] = "range";
                HTMLInputRange.Attributes["min"] = this.Min.ToString();
                HTMLInputRange.Attributes["max"] = this.Max.ToString();
                HTMLInputRange.Attributes["value"] = this.Valor.ToString();
            HTMLInputRange.ID = this.ClientID + "_input";
            HTMLRange.Controls.Add(HTMLInputRange);

            HtmlGenericControl HTMLDivVal = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div");
                HTMLDivVal.Attributes["style"] = "font-size: 26px;width: 50px;text-align: center;";
                HTMLDivVal.ID = this.ClientID + "_Slider";
                HTMLDivVal.InnerText = this.Valor.ToString();
                HTMLRange.Controls.Add(HTMLDivVal);
            return HTMLRange;
        }

        public void SetValue(int value)
        {
            this.Valor=value;
            oTxtValor.SetValue(value.ToString());
        }
        public int GetValue(int value)
        {
            return Convert.ToInt32(oTxtValor.GetValue());
        }
        protected override void CreateChildControls()
        {
            oTxtValor.Style.Add("display", "none");
            oTxtValor.ID = "Value";
            this.Controls.Add(oTxtValor);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
                string cmll = "\"";
                oTxtValor.RenderControl(writer);
                HtmlInputRange().RenderControl(writer);
                var idCtrlInputRange = this.ClientID + "_input";
                var idCtrlSliderRange = this.ClientID + "_Slider";
                string ScriptBlock = @"<script>
                                            var " + this.ClientID + @"={};
                                            var o" + idCtrlInputRange + @"=document.getElementById('" + idCtrlInputRange + @"');
                                                o" + idCtrlInputRange + @".addEventListener('input', (event) => {
                                                   " + this.ClientID + @".SetValue(event.target.value);
                                            });

                                            var o" + idCtrlSliderRange + @"=document.getElementById('" + idCtrlSliderRange + @"');
                                            " + this.ClientID + @".SetValue=function(Valor){
                                                    const tempSliderValue = Number(Value);
                                                    o" + idCtrlSliderRange + @".textContent = tempSliderValue;
                                                    const progress = (tempSliderValue / "+ this.Max +@") * 100;
                                                     o" + idCtrlInputRange + @".style.background = `linear-gradient(to right, #428bca ${progress}%, #ccc ${progress}%)`;
                                                     o" + idCtrlInputRange + @"..style.setProperty(" + cmll +"--thumb-rotate" + cmll + @", `${(tempSliderValue / 100) * 2160}deg`);
                                            }
                                            " + this.ClientID + @".GetValue=function(){
                                               return " + this.ClientID + @"_Value.GetValue();
                                            }
                                             " + this.ClientID + @".SetValue(" + this.Valor.ToString() + @");
                                    </script>";

                (new LiteralControl(ScriptBlock)).RenderControl(writer);
            }
        }
        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
    }
}
