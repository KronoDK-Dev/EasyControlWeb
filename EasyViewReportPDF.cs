using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace EasyControlWeb
{
    public class EasyViewReportPDF : Control
    {
        protected override void OnInit(EventArgs e)
        {
            if (!Page.IsClientScriptBlockRegistered("Spotu_HelloWorld"))
            {
                string strCode = @"<script>
                                    function HelloWorld(id)
                                    {
                                      document.all(id).innerText = 'Hello World';
                                    }
                                  </script>";

                Page.RegisterClientScriptBlock("Spotu_HelloWorld", strCode);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string cmll = "\"";

            writer.Write("<span id = " + cmll + "ShowPdf1" + cmll + " style = " + cmll + "color:#C00000;height:99%;width:100%;" + cmll + ">"
                         + "<div>"
                         + "     <iframe src = " + cmll + "http://Localhost/RptToPdf/eysrfn45nvkijeiimkjbmgfu061020210514pm15.pdf " + cmll + " width=100% height=99% "
                         + "         <p>View PDF:  <a href= " + cmll + "http://Localhost/RptToPdf/eysrfn45nvkijeiimkjbmgfu061020210514pm15.pdf" + cmll + "></a></p>"
                         + "     </iframe>"
                         + " </ div >"
                         + "</ span >");
        }
    }
}
