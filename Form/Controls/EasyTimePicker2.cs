using EasyControlWeb.Form.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace EasyControlWeb.Form.Controls
{
    [DefaultProperty("FormatoHora")]
    [ToolboxData("<{0}:EasyTimePicker2 runat=server></{0}:EasyTimePicker2>")]
    // El control que lo contenga debe de ser runat=server y dentro de un .container form
    [Serializable]
    public class EasyTimePicker2 : EasyTexto
    {
        const string ClaseContenedora = "container";

        public EasyTimePicker2()
        {
        }

        public EasyTimePicker2(string _FormatInPut)
        {
            this.FormatInPut = _FormatInPut;
        }

        public EasyTimePicker2(string _FormatOutPut, string _FormatInPut)
        {
            this.FormatOutPut = _FormatOutPut;
            this.FormatInPut = _FormatInPut;
        }

        [Browsable(true)]
        [Category("Behavior"), DefaultValue(""), Description("")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Placeholder { get; set; }

        private int minuteStep = 1;

        [Category("Behavior")]
        [Description("Intervalo de minutos permitido")]
        [DefaultValue(1)]
        public int MinuteStep
        {
            get { return minuteStep; }
            set { minuteStep = value; }
        }

        private string formatInPut;
        [Category("Appearance"), Description("Formato de hora por defecto"), DefaultValue("HH:mm")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string FormatInPut
        {
            get { return formatInPut; }
            set { formatInPut = value; }
        }

        private string formatOutPut;
        [Category("Appearance"), Description("Formato de hora por defecto"), DefaultValue("HH:mm")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string FormatOutPut
        {
            get { return formatOutPut; }
            set { formatOutPut = value; }
        }

        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncSelectTime { get; set; }

        [Browsable(true)]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public DateTime Hora { get; set; }

        protected override void OnInit(EventArgs e)
        {
            if (!Page.IsClientScriptBlockRegistered("RegScript"))
            {
                string JavaScriptCode = @"<script>
                                            function RegistrarControl(){}
                                         </script>";
                Page.RegisterClientScriptBlock("RegScript", JavaScriptCode);
            }
            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;

            this.Attributes.Add("placeholder", this.Placeholder);
            this.Style.Add(
                "background",
                "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconDatePick + "') right center no-repeat; padding-right:5px;"
            );

            base.Render(writer);

            string strFunction = ((fncSelectTime != null)
                ? fncSelectTime + "(e.value);"
                : "return null;");

            string IdCtrl = ((this.ClientID == null) ? this.ID : this.ClientID);

            string strFnc = IdCtrl + @".Change=function(e){
                                " + strFunction + @"
                            }";

            string Formato = string.IsNullOrEmpty(this.FormatInPut)
                ? "H:i"
                : this.FormatInPut;

            string scriptCall =
                "EasyTimepicker.Setting(" +
                cmll + IdCtrl + cmll + "," +
                cmll + Formato + cmll + "," +
                this.MinuteStep +
                ");";

            (new LiteralControl("<script>\n" + strFnc + "\n" + scriptCall + "\n</script>\n"))
                .RenderControl(writer);

        }

        protected override void CreateChildControls()
        {
        }
    }
}
