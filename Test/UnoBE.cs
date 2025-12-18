using EasyControlWeb.Form.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Test
{
        public class UnoBE 
    {
        string primero;
       /*[RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]*/
        public  string Primero {
            get { return primero; }
            set { primero = value; }
        }






        //Anular las propiedades que no se usaran
        [Browsable(false)]
        [Bindable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("", true)]
        public  new string Nombre
        {
            get; set; 
        }

        [Browsable(false)]
        [Bindable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("", true)]
        public new string Segundo
        {
            get;set;
        }
        


        public UnoBE() {
        
        }
        public UnoBE(string _primero) {
            primero = _primero;
        }
        public string ToString(bool InfoProp)
        {

            return this.Primero;
        }
        

      }
}
