using EasyControlWeb.Filtro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace EasyControlWeb.Form.Controls
{
    public enum TipoTab{
        UrlLocal,
        UrlExterna,
        Texto,
        ContentCtrl,
    }
    [Serializable]
    public  class EasyTabItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool Enabled { get; set; } = true;
        public TipoTab TipoDisplay{ get; set; }
        public string Value { get; set; }
        public string DataCollection { get; set; }

        public bool AccionRefresh { get; set; }


        //Parametros de la pagina
        [Browsable(true)]
        private List<EasyFiltroParamURLws> easyParams;
        [Category("Datos"),
        Description("Parámetros que utilizara la Pagina al momento de ser invocada para procesar su ejecución"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(EasyFiltroCollectionParams), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyFiltroParamURLws> UrlParams
        {
            get
            {
                if (easyParams == null)
                {
                    easyParams = new List<EasyFiltroParamURLws>();
                }
                return easyParams;
            }
        }

        public bool Selected { get; set; }
     

        public EasyTabItem() { }
        public EasyTabItem(string _Id,string _Text) {
            this.Id = _Id;
            this.Text = _Text;
        }
    }
}
