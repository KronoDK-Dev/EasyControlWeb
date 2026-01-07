using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Controls
{
    [
        AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Empresa"),
        ParseChildren(true, "Empresa"),
        ToolboxData("<{0}:EasyClockDigital runat=server></{0}:EasyClockDigital")
    ]
    public class EasyClockDigital : CompositeControl
    {
        HtmlGenericControl ContextClock;
        string cmll = "";


        public string Empresa { get; set; }
        public EasyClockDigital()
        {
            cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
        }



        HtmlGenericControl ClockBody() {
            HtmlGenericControl CardDigital = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "DPDC");
            CardDigital.Attributes.Add("cityid","8495");
            CardDigital.Attributes.Add("lang", "es");
            CardDigital.Attributes.Add("id", "dayspedia_widget_3eaa11131bde0cbe");

            CardDigital.Attributes.Add("host", "https://dayspedia.com");
            CardDigital.Attributes.Add("ampm", "true");
            CardDigital.Attributes.Add("nightsign", "true");
            CardDigital.Attributes.Add("sun", "false");

            CardDigital.Controls.Add(new LiteralControl(TargetStyle()));

            HtmlGenericControl LogoSima = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a", "DPl");
                LogoSima.Attributes["style"] = "display:block!important;text-decoration:none!important;border:none!important;cursor:pointer!important;background:transparent!important;line-height:0!important;text-shadow:none!important;position:absolute;z-index:1;top:0;right:0;bottom:0;left:0";
                HtmlGenericControl span = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span");
                span.Attributes["style"] = "position:absolute;right:28px;bottom:6px;height:10px;width:60px;overflow:hidden;text-align:right;font:normal 10px/10px Arial,sans-serif!important;color:#007DBF";
                span.InnerText = "SIMA PERU";
                LogoSima.Controls.Add(span);
            CardDigital.Controls.Add(LogoSima);


            HtmlGenericControl Titulo = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "DPDCh");
                Titulo.InnerText = "Hora Actual en Lima-Callao";
            CardDigital.Controls.Add(Titulo);

            /*Panel DIGITAL*/
            HtmlGenericControl PanelDIgital = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "DPDCt");
                HtmlGenericControl SegHora = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "DPDCth");
                SegHora.InnerText = "10";
                PanelDIgital.Controls.Add(SegHora);
                HtmlGenericControl SegMinuto = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "DPDCtm");
                SegMinuto.InnerText = "36";
                PanelDIgital.Controls.Add(SegMinuto);
                HtmlGenericControl SegSegundo = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "DPDCts");
                SegSegundo.InnerText = "17";
                PanelDIgital.Controls.Add(SegSegundo);
                HtmlGenericControl SegAM_PM = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "DPDCt12");
                SegAM_PM.InnerText = "am";
            PanelDIgital.Controls.Add(SegAM_PM);
            CardDigital.Controls.Add(PanelDIgital);


            /*PANEL INFORMATIVO*/
            HtmlGenericControl PanelInfo = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "DPDCd");
                HtmlGenericControl infoDate = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "DPDCdt");
                infoDate.InnerText = "mié, 6 de julio";
                PanelInfo.Controls.Add(infoDate);

                HtmlGenericControl infoTik = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "DPDCtn");
                infoTik.Style.Add("display", "none");
                HtmlGenericControl i = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("i");
                infoTik.Controls.Add(i);

            PanelInfo.Controls.Add(infoTik);
            CardDigital.Controls.Add(PanelInfo);

            /*Borde y contorno*/
            HtmlGenericControl PaintBorder = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "DPDCs");
                PaintBorder.Style.Add("display", "none");
                HtmlGenericControl _span = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "DPDCsr");
                string sgv = "<svg xmlns = " + cmll + "http://www.w3.org/2000/svg" + cmll + " viewbox = " + cmll + "0 0 24 24" + cmll + "><path d=" + cmll + "M12,4L7.8,8.2l1.4,1.4c0,0,0.9-0.9,1.8-1.8V14h2c0,0,0-3.3,0-6.2l1.8,1.8l1.4-1.4L12,4z" + cmll + "></path><path d=" + cmll + "M6.8,15.3L5,13.5l-1.4,1.4l1.8,1.8L6.8,15.3z M4,21H1v2h3V21z M20.5,14.9L19,13.5l-1.8,1.8l1.4,1.4L20.5,14.9z M20,21v2h3 v-2H20z M6.1,23C6,22.7,6,22.3,6,22c0-3.3,2.7-6,6-6s6,2.7,6,6c0,0.3,0,0.7-0.1,1H6.1z" + cmll + "></path></svg>06:29<sup>am</sup>";
                _span.Controls.Add(new LiteralControl(sgv));
            PaintBorder.Controls.Add(_span);

            HtmlGenericControl _span1 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "DPDCsl");
                _span1.InnerText = "11:26";
            PaintBorder.Controls.Add(_span1);

            HtmlGenericControl _span2 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "DPDCss");
                sgv = "<svg xmlns=" + cmll + "http://www.w3.org/2000/svg" + cmll + " viewbox=" + cmll + "0 0 24 24" + cmll + " ><path d=" + cmll + "M12,14L7.8,9.8l1.4-1.4c0,0,0.9,0.9,1.8,1.8V4h2c0,0,0,3.3,0,6.2l1.8-1.8l1.4,1.4L12,14z" + cmll + "></path><path d=" + cmll + "M6.8,15.3L5,13.5l-1.4,1.4l1.8,1.8L6.8,15.3z M4,21H1v2h3V21z M20.5,14.9L19,13.5l-1.8,1.8l1.4,1.4L20.5,14.9z M20,21v2h3 v-2H20z M6.1,23C6,22.7,6,22.3,6,22c0-3.3,2.7-6,6-6s6,2.7,6,6c0,0.3,0,0.7-0.1,1H6.1z" + cmll + "></path></svg> 05:55<sup>pm</sup>";
                _span2.Controls.Add(new LiteralControl(sgv));
            PaintBorder.Controls.Add(_span2);
            
            CardDigital.Controls.Add(PaintBorder);

            return CardDigital;
        }

        string  TargetStyle() {
            string StrStyle = @"<style media=" + cmll + "screen" + cmll + " id =" + cmll + "dayspedia_widget_3eaa11131bde0cbe_style" + cmll + @">
                                   .float-nav-Clock{position: fixed;bottom: 20px;right: 20px;z-index: 9999;}
                                     .float-nav-Clock:hover {cursor:pointer;}
				                            /*COMMON*/
		                            .DPDC{display:table;position:relative;box-sizing:border-box;font-size:100.01%;font-style:normal;font-family:Arial;background-position:50% 50%;background-repeat:no-repeat;background-size:cover;overflow:hidden;user-select:none}
		                            .DPDCh,.DPDCd{width:fit-content;line-height:1.4}
		                            .DPDCh{margin-bottom:1em}
		                            .DPDCd{margin-top:.24em}
		                            .DPDCt{line-height:1}
		                            .DPDCth,.DPDCtm,.DPDCts{display:inline-block;vertical-align:text-top;white-space:nowrap}
		                            .DPDCth{min-width:1.15em}
		                            .DPDCtm,.DPDCts{min-width:1.44em}
		                            .DPDCtm::before,.DPDCts::before{display:inline-block;content:':';vertical-align:middle;margin:-.34em 0 0 -.07em;width:.32em;text-align:center;opacity:.72;filter:alpha(opacity=80)}
		                            .DPDCt12{display:inline-block;vertical-align:text-top;font-size:40%}
		                            .DPDCdm::after{content:' '}
		                            .DPDCda::after{content:', '}
		                            .DPDCdt{margin-right:.48em}
		                            .DPDCtn{display:inline-block;position:relative;width:13px;height:13px;border:2px solid;border-radius:50%;overflow:hidden}
		                            .DPDCtn>i{display:block;content:'';position:absolute;right:33%;top:-5%;width:85%;height:85%;border-radius:50%}
		                            .DPDCs{margin:.96em 0 0 -3px;font-size:90%;line-height:1;white-space:nowrap}
		                            .DPDCs sup{padding-left:.24em;font-size:65%}
		                            .DPDCsl::before,.DPDCsl::after{display:inline-block;opacity:.4}
		                            .DPDCsl::before{content:'~';margin:0 .12em}
		                            .DPDCsl::after{content:'~';margin:0 .24em}
		                            .DPDCs svg{display:inline-block;vertical-align:bottom;width:1.2em;height:1.2em;opacity:.48}
		                            /*CUSTOM*/
		
		                            .DPDC{width:auto;padding:24px;background-color:#ffffff;border:1px solid #343434;border-radius:8px} /* widget width, padding, background, border, rounded corners */
		                            .DPDCh{color:#007DBF;font-weight:normal} /* headline color, font-weight*/
		                            .DPDCt,.DPDCd{color:#343434;font-weight:bold} /* time & date color, font-weight */
		                            .DPDCtn{border-color:#343434} /* night-sign color = time & date color */
		                            .DPDCtn>i{background-color:#343434} /* night-sign color = time & date color */
		                            .DPDCt{font-size:48px} /* time font-size */
		                            .DPDCh,.DPDCd{font-size:16px} /* headline & date font-size */	        
                            </style>";
            return StrStyle;
        }
        string ScriptClockIni() {
            string script = @"<script>
                                    var s, t; s = document.createElement(" + cmll + "script" + cmll + "); s.type = " + cmll + "text/javascript" + cmll + @";
                                    s.src = " + cmll + "//cdn.dayspedia.com/js/dwidget.min.vb46adaa2.js" + cmll + @";
                                    t = document.getElementsByTagName('script')[0]; t.parentNode.insertBefore(s,t);
                                    s.onload=function(){
                                        window.dwidget = new window.DigitClock();
                                        window.dwidget.init(" + cmll + "dayspedia_widget_3eaa11131bde0cbe" + cmll + @");
                                    };
                             </script>
                            ";
            return script;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.DesignMode == true)
            {
                this.EnsureChildControls();
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
        }
        protected override void Render(HtmlTextWriter writer)
        {
            ContextClock = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "float-nav-Clock");
            ContextClock.Controls.Add(ClockBody());
            ContextClock.RenderControl(writer);
            (new LiteralControl(ScriptClockIni())).RenderControl(writer);
        }

     }
}
