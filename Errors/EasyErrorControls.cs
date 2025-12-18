using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EasyControlWeb.Errors
{
    [Serializable]
    public class EasyErrorControls : Exception
    {
        public string Origen { get; set; }
        //public string Pagina { get; set; }
        public string Mensaje { get; set; }
        public string Pagina { get; set; }

        public EasyErrorControls()
        {
        }

        public EasyErrorControls(string message) : base(message)
        {
            this.Origen = message;
        }

        public EasyErrorControls(string message, Exception innerException) : base(message, innerException)
        {
            this.Origen = message;
            //  this.Pagina = innerException.Message;
        }

        protected EasyErrorControls(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public void LanzarException(string Metodo)
        {
            string StructBE = "{Origen:'" + this.Origen + "',Mensaje:'" + this.Mensaje + "',Pagina:'" + this.Pagina + "',Metodo:'" + Metodo + "'}";
            ((System.Web.UI.Page)HttpContext.Current.Handler).Session["Error"] = StructBE;
            string ScriptRedirect = @"<script>
                                            (function(){
                                                sessionStorage.setItem('Context', 'EasyControls');
                                                sessionStorage.setItem('Origen', '" + this.Origen + @"');
                                                sessionStorage.setItem('Mensaje', '" + this.Mensaje + @"');
                                                sessionStorage.setItem('Pagina', '" + this.Pagina + @"');
                                                sessionStorage.setItem('Metodo', '" + Metodo + @"');

                                                Page.Response.redirect('" + EasyUtilitario.Helper.Pagina.PathSite() + "/Error.aspx" + @"');
                                            })();
                                      </script>";


            ((System.Web.UI.Page)System.Web.HttpContext.Current.Handler).RegisterStartupScript("Error2020", ScriptRedirect);
        }
    }
}
