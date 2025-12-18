using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasyControlWeb.Errors
{
    public class EasyErrorWebService : Exception
    {
        public string Origen { get; set; }
        //public string Pagina { get; set; }
        public string Mensaje { get; set; }
        public string Pagina { get; set; }

        public EasyErrorWebService()
        {
        }

        public EasyErrorWebService(string message) : base(message)
        {
            this.Origen = message;
        }

        public EasyErrorWebService(string message, Exception innerException) : base(message, innerException)
        {
            this.Origen = message;
            //  this.Pagina = innerException.Message;
        }

        protected EasyErrorWebService(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public void LanzarException(string Metodo)
        {
            string ScriptRedirect = @"<script>
                                            (function(){
                                                sessionStorage.setItem('Context', 'EasyControls');
                                                sessionStorage.setItem('Origen', '" + this.Origen + @"');
                                                sessionStorage.setItem('Mensaje', '" + this.Mensaje + @"');
                                                sessionStorage.setItem('Pagina', '" + this.Pagina + @"');
                                                sessionStorage.setItem('Metodo', '" + Metodo + @"');
                                                Page.Response.redirect(Page.Request.App_Protocol_Path_Name + '/Error.aspx');
                                            })();
                                      </script>";

            ((System.Web.UI.Page)System.Web.HttpContext.Current.Handler).RegisterStartupScript("Error2022", ScriptRedirect);
        }
    }
}
