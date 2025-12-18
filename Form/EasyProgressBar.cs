using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form
{
    public  class EasyProgressBar: CompositeControl
    {
        public EasyProgressBar() { }
        public EasyProgressBar(int _Progreso,string _ImgProgreso) {
            this.Progreso = _Progreso;
            this.ImgProgreso = _ImgProgreso;
        }

        [Category("Avance"), Description("")]
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
            HtmlGenericControl ProgressContainer = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "containerBar");
            HtmlGenericControl TextoPorcentaje = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("label", "label");
            TextoPorcentaje.InnerText = this.Progreso + "%";
            ProgressContainer.Controls.Add(TextoPorcentaje);

            HtmlGenericControl Progress = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "progress");
            Progress.Style.Add("background-color", "white");

            HtmlGenericControl Bar = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "progress-bar");
            Bar.Attributes["style"] = "width: " + this.Progreso + "%; background: url('" + this.ImgProgreso + "');";
            Progress.Controls.Add(Bar);

            ProgressContainer.Controls.Add(Progress);
            return ProgressContainer;
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
                HtmlProgressBar().RenderControl(writer);
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
