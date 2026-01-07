using System;
using System.Web;
using System.Web.UI;
namespace EasyControlWeb
{
    public class EasyViewReportPDF:Control 
    {
        [Obsolete]
#pragma warning disable CS0809 // El miembro obsoleto 'EasyViewReportPDF.OnInit(EventArgs)' invalida el miembro no obsoleto 'Control.OnInit(EventArgs)'
        protected override void OnInit(EventArgs e)
#pragma warning restore CS0809 // El miembro obsoleto 'EasyViewReportPDF.OnInit(EventArgs)' invalida el miembro no obsoleto 'Control.OnInit(EventArgs)'
        {
            if (!Page.IsClientScriptBlockRegistered("Spotu_HelloWorld"))
            {
               string strCode = @"<script>
                                    function HelloWorld(id)
                                    {
                                      document.all(id).innerText = 'Hello World';
                                    }
                                  </script>";

                Page.RegisterClientScriptBlock("Spotu_HelloWorld",strCode);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            /*writer.Write("<button onclick='javascript:HelloWorld(\""
                          + this.UniqueID + "\")'>"
                          + "Click Me</button>");

            writer.Write("<div id='" + this.UniqueID
                         + "'></div>");*/
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
