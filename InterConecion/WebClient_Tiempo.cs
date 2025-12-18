using System;
using System.Net;

namespace EasyControlWeb.InterConecion
{
    public class WebClient_Tiempo:WebClient 
    {
        public int Timeout { get; set; }

        public WebClient_Tiempo()
        {
            this.Timeout = 600000; // 10 minutos por defecto
        }

        protected override WebRequest GetWebRequest(System.Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }
}
