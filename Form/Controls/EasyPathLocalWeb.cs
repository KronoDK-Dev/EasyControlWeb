using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Form.Controls
{
    [Serializable]
    public class EasyPathLocalWeb
    {

        private string carpetaTemporal;
        [Category("Carga")]
        [Browsable(true)]
        [Description("Ubicacion temporal de los archivos cargados")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string CarpetaTemporal
        {
            get
            {
                if ((carpetaTemporal != null) && (carpetaTemporal.Length > 0))
                {
                    if (carpetaTemporal.Substring(carpetaTemporal.Length - 1, 1) == EasyUtilitario.Constantes.Caracteres.BackSlash.ToString())
                    {
                        return carpetaTemporal;
                    }
                    else
                    {
                        return carpetaTemporal + EasyUtilitario.Constantes.Caracteres.BackSlash.ToString();
                    }
                }
                return carpetaTemporal;
            }
            set { carpetaTemporal = value; }
        }


        private string carpetaFinal;
        [Category("Carga")]
        [Browsable(true)]
        [Description("Ubicacion final de los archivos cargados")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string CarpetaFinal
        {
            get
            {
                if((carpetaFinal!=null)&&(carpetaFinal.Length > 0))
                {
                    if (carpetaFinal.Substring(carpetaFinal.Length - 1, 1) == EasyUtilitario.Constantes.Caracteres.BackSlash.ToString())
                    {
                        return carpetaFinal;
                    }
                    else
                    {
                        return carpetaFinal + EasyUtilitario.Constantes.Caracteres.BackSlash.ToString();
                    }
                }
                return carpetaFinal;
            }
            set { carpetaFinal = value; }
        }


        string urlFinal;
        [Category("Visualizacion")]
        [Browsable(true)]
        [Description("Ubicacion final de los archivos cargados")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string UrlFinal
        {
            get { return urlFinal; }
            set { urlFinal = value; }
        }

        string urlTemporal;
        [Category("Visualizacion")]
        [Browsable(true)]
        [Description("Ubicacion final de los archivos cargados")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string UrlTemporal
        {
            get { return urlTemporal; }
            set { urlTemporal = value; }
        }


        public EasyPathLocalWeb()
        {
        }

        public EasyPathLocalWeb(string _CarpetaTemporal,string _CarpetaFinal,string _UrlTemporal,string _UrlFinal)
        {
            this.CarpetaTemporal = _CarpetaTemporal;
            this.CarpetaFinal = _CarpetaFinal;
            this.UrlTemporal = _UrlTemporal;
            this.UrlFinal = _UrlFinal;
        }
        /*Utilizao en la conversion de tipos */
        public string Serializado() {
            return this.CarpetaTemporal + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal + this.CarpetaFinal + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal + this.UrlTemporal + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal + this.UrlFinal;
        }

    }
}
