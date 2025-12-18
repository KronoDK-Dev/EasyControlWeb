using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Security.Permissions;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace EasyControlWeb.Form.Controls
{
    [Serializable] 
    [DefaultProperty("Titulo")]
    [ToolboxData("<{0}:EasyFormHeader runat=server></{0}:EasyFormHeader>")]
    public class EasyFormHeader : CompositeControl
    {
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Category("Descripcion"), Description("Identificacion del control"), DefaultValue("")]
        public string Titulo {
            get;set;
        }
        [StringLength(200, MinimumLength = 2)]
        [Category("Descripcion"), Description("Descripcion de la funcionalidad"), DefaultValue("")]
        public string Descripcion
        {
            get; set;
        }

        [Category("Descripcion"), Description("Fragmento descriptivo adicional"), DefaultValue("")]
        public string Snippetby { get; set; }


        public EasyFormHeader() { }

        public EasyFormHeader(string _Titulo,string _Descripcion,string _Fragmentode) {
            this.Titulo = _Titulo;
            this.Descripcion = _Descripcion;
            this.Snippetby = _Fragmentode;
        }



        HtmlGenericControl _header;
        protected override void OnInit(EventArgs e)
        {
            _header = new HtmlGenericControl("header");
            _header.Attributes.Add("class", "text-center text-black");

            HtmlGenericControl _TItulo = new HtmlGenericControl("h1");
            _TItulo.Attributes.Add("class","display-6");
            _TItulo.InnerText = this.Titulo;
            
            HtmlGenericControl _Descripcion = new HtmlGenericControl("p");
            _Descripcion.Attributes.Add("class", "lead mb-0");
            _Descripcion.InnerText = this.Descripcion;

            HtmlGenericControl _Fragmento = new HtmlGenericControl("p");
            _Fragmento.Attributes.Add("class", "font-italic");
            _Fragmento.InnerText = this.Snippetby;

            _header.Controls.Add(_TItulo);
            _header.Controls.Add(_Descripcion);
            _header.Controls.Add(_Fragmento);


        }


        protected override void Render(HtmlTextWriter writer)
        {
            _header.RenderControl(writer);
        }



    }

}
