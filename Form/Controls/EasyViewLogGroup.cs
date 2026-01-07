using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace EasyControlWeb.Form.Controls
{
    public  class EasyViewLogGroup
    {

        #region Constructor
            public EasyViewLogGroup() : this(String.Empty) { }

            public EasyViewLogGroup(string _Titulo)
            {
                oEasyItemCollection = new List<EasyItemLog>();
            }
        #endregion


        [Category("indentificacion"), PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(true)]
        [Description("Titulo")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Titulo { get; set; }


        [Category("indentificacion"), PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(true)]
        [Description("ID")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string ID { get; set; }



        [Category("Tool"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public EasyPathHistory PathHistory { get; set; }



        [Browsable(false)]
        List<EasyItemLog> oEasyItemCollection;

        [Editor(typeof(EasyControlCollection.EasyFormCollectionLogItemEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contiene los items Log Generados")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyItemLog> LogItems
        {
            get
            {
                if (oEasyItemCollection == null)
                {
                    oEasyItemCollection = new List<EasyItemLog>();
                }
                return oEasyItemCollection;
            }
        }


    }
}
