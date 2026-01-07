using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Windows.Resources;
using EasyControlWeb.Form.Controls;

namespace EasyControlWeb.Form.Base
{
    public class EasyProgressbarBase: CompositeControl
    {

        EasyTextBox oTxtAvance;

        //Refrencia:https://www.jose-aguilar.com/blog/como-mover-una-barra-de-progreso-con-javascript/
        public EasyProgressbarBase() {
            oTxtAvance = new EasyTextBox();
            oTxtAvance.SetValue("0");
        }
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public int Progreso { get; set; }

        [Category("Avance"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string ImgProgreso { get; set; }


        HtmlGenericControl HtmlProgressBar()
        {
            HtmlGenericControl ProgressContainer = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "progress");
            HtmlGenericControl Progress = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "progress-bar");
            Progress.Attributes["id"] = this.ClientID;
            // Progress.Attributes["style"] = "width: " + this.Progreso + "%; background: url('" + this.ImgProgreso + "');";
            Progress.Attributes["style"] = "width: " + this.Progreso + "%; ";

            HtmlGenericControl TextPorc = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "progress-bar-text");
                TextPorc.InnerText=this.Progreso +"%";

                Progress.Controls.Add(TextPorc);

            ProgressContainer.Controls.Add(Progress);
            return ProgressContainer;
        }

        public void SetValue(int value){
            this.Progreso = value;
            oTxtAvance.SetValue(value.ToString());
        }
        public int GetValue(int value)
        {
            return Convert.ToInt32(oTxtAvance.GetValue());
        }
        protected override void CreateChildControls()
        {
            oTxtAvance.Style.Add("display", "none");
            oTxtAvance.ID = "Value";
            this.Controls.Add(oTxtAvance);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
                oTxtAvance.RenderControl(writer);
                HtmlProgressBar().RenderControl(writer);
                string ScriptBlock = @"<script>
                                            var " + this.ClientID + @"={};
                                            " + this.ClientID + @".SetValue=function(Porc){
                                                jNet.get('" + this.ClientID + @"').css('width',Porc);
                                                " + this.ClientID + @"_Value.SetValue(Porc);
                                            }
                                            " + this.ClientID + @".GetValue=function(){
                                               return " + this.ClientID + @"_Value.GetValue();
                                            }
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
