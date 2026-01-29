using EasyControlWeb.Form.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.UI;
using System.Web.UI.WebControls;

// referencia para crear el control de horas
/*
 * https://codepen.io/elmahdim/pen/nVWKrq
 * https://thecodedeveloper.com/add-datetimepicker-jquery-plugin/
 */
namespace EasyControlWeb.Form.Controls
{
    [DefaultProperty("FormatoHora")]
    [ToolboxData("<{0}:EasyTimepicker runat=server></{0}:EasyTimepicker>")]
    // El control que lo contenga debe de ser runat=server y dentro de un .container form
    [Serializable]
    public class EasyTimepicker : EasyTexto
    {
        const string ClaseContenedora = "container";

        public EasyTimepicker()
        {
        }

        public EasyTimepicker(string _FormatInPut)
        {
            this.FormatInPut = _FormatInPut;
        }

        public EasyTimepicker(string _FormatOutPut, string _FormatInPut)
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
