using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Permissions;
using System.Collections;
using System.Drawing.Design;
using System.Web.UI.HtmlControls;
using EasyControlWeb.Form.Templates;

namespace EasyControlWeb.Form
{
    [
       AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
       AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
       DefaultProperty("Titulo"),
       ParseChildren(true, "Titulo"),
       ToolboxData("<{0}:EasyFormGrupo runat=server></{0}:EasyFormGrupo")
   ]
    [Serializable]
    public class EasyFormGrupo
    {


        public EasyFormGrupo() : this(String.Empty) { }
        public EasyFormGrupo(string _Titulo)
        {
            Titulo = _Titulo;
            easyFormItems = new List<EasyFomItem>();
            // ViewStateMode[""]  = new List<EasyFomItem>();
        }


        #region Propiedades Simples
        public string Titulo { get; set; }
            public System.Web.UI.WebControls.BorderStyle BorderStyle { get; set; }
        #endregion

        #region Propiedades de Collecion
        [Browsable(true)]
        private List<EasyFomItem> easyFormItems;
        [
        Category("Behavior"),
        Description("Coleccion de Seccion de grupos de formularios"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(EasyFormCollectionEditor.EasyFormoCollectionEditorItems), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public List<EasyFomItem> FormItems
        {
            get
            {
                InicializaColecciones();
                return easyFormItems;
            }

        }
        void InicializaColecciones() {
            if (easyFormItems == null)
            {
                easyFormItems = new List<EasyFomItem>();
               
            }
        }
        #endregion

        /*
        [Browsable(true)]
        List<Control> _MultipleItems;

        // custom editor attribute
        [Editor(typeof(CustomCollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contains  Button and  CheckBox items in a List collection")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public List<Control> MultipleItems
        {
            get { 
                if (_MultipleItems == null) {
                    _MultipleItems = new List<Control>();
                }
                return _MultipleItems; 
            }
        }
        */

        

    }
}
