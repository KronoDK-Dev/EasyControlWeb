using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls; 

namespace EasyControlWeb.Form.Controls
{
    [ToolboxData("<{0}:EasyPdfViewer runat=server></{0}:EasyPdfViewer>")]
    [ParseChildren(true, "PathPDF")]
    public class EasyPdfViewer: WebControl
    {


        #region Variables
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
        #endregion

        public string PathRpt { get; set; }
        public string FileRpt { get; set; }

        public string PathPDF { get; set; }
        public string FilePDF { get; set; }


        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
        }
        protected override void RenderContents(HtmlTextWriter output)
        {
            //output.Write("<embed src=" + cmll + "http://10.10.17.110:90/FileTemp/New_Horizons.pdf" + cmll + " width=" + cmll +  "100%" + " height=" + cmll +"875" + cmll + " type =" + cmll + "application/pdf" + cmll +"> ");
            output.Write("<embed src=" + cmll + PathPDF + FilePDF + cmll + " width=" + cmll + this.Width.ToString() + cmll + " height=" + cmll + this.Height.ToString() + cmll + " type =" + cmll + "application/pdf" + cmll + "> ");
        }


        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.CreateChildControls();
            this.ChildControlsCreated = true;
        }
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            
            //   writer.AddStyleAttribute("width", "600px");
             //  writer.AddStyleAttribute("height", "1200px");
            //writer.RenderBeginTag(HtmlTextWriterTag.Div);

               //writer.AddStyleAttribute("float", "left");
               //writer.AddStyleAttribute("width", "280px");
               //writer.AddStyleAttribute("padding", "10px");
              

        }


        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }

        public void DataBind(DataTable dt) {
            DataSet ds = new DataSet();
            if (dt != null)
            {
                DataTable dtClonado = dt.Copy();
                dtClonado.TableName = this.FileRpt.Replace(".rpt", "");
                ds.Tables.Add(dtClonado);
            }
            DataBind(ds);
        }

        public void DataBind(DataSet ds)
        {
           /* CrystalDecisions.CrystalReports.Engine.ReportDocument _rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            _rpt.Load(this.PathRpt + this.FileRpt);
            _rpt.SetDataSource(ds);



            DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            crDiskFileDestinationOptions.DiskFileName = _filename;
            CrystalDecisions.Shared.ExportOptions crExportOptions = tDoc.ExportOptions;
            crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
            crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //crExportOptions.ExportFormatType = ((EXTENSIONFILE==".pdf")?ExportFormatType.PortableDocFormat:ExportFormatType.Excel);  
            crExportOptions.ExportFormatType = ((EXTENSIONFILE == ".pdf") ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel);
            //crExportOptions.ExportFormatType = ExportFormatType.Excel;  
            tDoc.Export();*/


        }

    }
}
