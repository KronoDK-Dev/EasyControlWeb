using EasyControlWeb.Form.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.UI;
using System.Web.UI.WebControls;


//referencia para crear el control de horas
/*
 * https://codepen.io/elmahdim/pen/nVWKrq
 * https://thecodedeveloper.com/add-datetimepicker-jquery-plugin/
 */
namespace EasyControlWeb.Form.Controls
{
    [DefaultProperty("FormatoFecha")]
    [ToolboxData("<{0}:EasyDatepicker runat=server></{0}:EasyDatepicker>")]
    //El control que lo contenga debe de ser runat=server y detro de un .container form
    [Serializable]
    public class EasyDatepicker : EasyTexto
    {
        const string ClaseContenedora = "container";
        public EasyDatepicker() {
            
        }
        public EasyDatepicker(string _FormatInPut)
        {
            this.FormatInPut = _FormatInPut;
        }
        public EasyDatepicker(string _FormatOutPut, string _FormatInPut) {
            this.FormatOutPut = _FormatOutPut;
            this.FormatInPut = _FormatInPut;
        }


        [Browsable(true)]
        [Category("Behavior"), DefaultValue(""),Description("")]
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
        public string Placeholder { get; set; }

       /* private string formato;
        [Category("Appearance"), Description("Formato de fecha por defecto"), DefaultValue("DD/MM/YYYY")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string FormatoFecha 
        {
            get
            {
                if (formato == null)
                {
                    formato = "dd/mm/yyyy";
                }
                return formato;
            }
            set
            {
                formato = value;
            }
        }*/



        private string formatInPut;
        [Category("Appearance"), Description("Formato de fecha por defecto"), DefaultValue("DD/MM/YYYY")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]        public string FormatInPut
        {
            get { return formatInPut; }
            set { formatInPut = value; }
        }
        private string formatOutPut;
        [Category("Appearance"), Description("Formato de fecha por defecto"), DefaultValue("DD/MM/YYYY")]
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
        public string fncSelectDate{get;set;}


        [Browsable(true)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public DateTime Hoy { get; set; }



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
            this.Style.Add("background", "white url('" + EasyUtilitario.Constantes.ImgDataURL.IconDatePick + "') right center no-repeat; padding-right:5px;");
            base.Render(writer);

            string strFunction = ((fncSelectDate != null) ? fncSelectDate+ "(e.format());" : "return null;");
            string IdCtrl = ((this.ClientID == null) ? this.ID : this.ClientID);

            string strFnc = IdCtrl + @".Change=function(e){
                                                    " + strFunction + @"
                                                    }";


            string Formato = ((this.FormatInPut == null) ? "dd/mm/yyyy":this.FormatInPut);
            string scriptCall = "EasyDatepicker.Setting(" + cmll + IdCtrl + cmll + "," + cmll + Formato + cmll + ");";


            

            (new LiteralControl("<script>\n" + strFnc +  "\n" + scriptCall + "\n" + "</script>\n")).RenderControl(writer);
            
            //Obtenr valor 
            string scritpGet = this.ClientID + @".GetValue=function(){
                                                return jNet.get('" + this.ClientID + @"').value;
                                        }
                                    " + this.ClientID + @".GetText=function(){
                                                return jNet.get('" + this.ClientID + @"').value;
                                        }
                                    " + this.ClientID + @".SetValue=function(value){
                                                jNet.get('" + this.ClientID + @"').value=value;
                                        }
                                    ";
            (new LiteralControl("<script>\n" + strFnc + "\n" + scritpGet + "\n" + "</script>\n")).RenderControl(writer);

        }
        protected override void CreateChildControls()
        {
           
        }
       
            
    }
}
