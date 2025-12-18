using EasyControlWeb.Form.Editor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Estilo
{
    [TypeConverter(typeof(Type_Style))]
    [Serializable]
    public class Bootstrap
    {


        public enum Talla
        {
            md,
            sm,
            xs,
            lg
        }
        public enum Tamaño
        {
            Uno = 1,
            Dos = 2,
            Tres = 3,
            Cuatro = 4,
            Cinco = 5,
            Seis = 6,
            Siete = 7,
            Ocho = 8,
            Nueve = 9,
            Diez = 10,
            Once=11,
            Doce=12
        }




        private string className;
        [Category("Style"), Description("Clases y Estilos"), DefaultValue("form-control")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string ClassName
        {
            get
            {
                if (className == null)
                {
                    className = "form-control";
                }
                return className;
            }
            set
            {
                className = value;
            }
        }




        private string classLabel;
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [Category("Style"), Description("Clases y Estilos"), DefaultValue("form-label")]
        public string ClassLabel
        {
            get
            {
                if (classLabel == null)
                {
                    classLabel = "form-label";
                }
                return classLabel;
            }
            set
            {
                classLabel = value;
            }
        }
        
        
        private Talla tipoTalla;
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [Category("Ubicacion"), Description("Talla para definir el tamaño"), DefaultValue(Talla.md)]
        public Talla TipoTalla
        {
            get
            {
                if (tipoTalla.Equals(null))
                {
                    tipoTalla = Talla.md;
                }
                return tipoTalla;
            }
            set
            {
                tipoTalla = value;
            }
        }


        private Tamaño ancho;
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [Category("Ubicacion"), Description("Tamaño del control"), DefaultValue(Tamaño.Cuatro)]
        public Tamaño Ancho
        {
            get
            {
                /*if (ancho== Tamaño)
                {
                    ancho = Tamaño.Dos;
                }*/
                if ((ancho.Equals(null))|| (ancho.Equals(" ")) || (ancho==0))
                {
                    ancho = Tamaño.Dos;
                }
                /*else {
                    ancho = Tamaño.Tres;
                }*/

                return ancho;
            }
            set
            {
                ancho = value;
            }
        }


        public Bootstrap() { }

        public Bootstrap(string _className,string _classLabel,Talla _tipoTalla,Tamaño _ancho) {
           // className = _className;
            classLabel = _classLabel;
            tipoTalla = _tipoTalla;
            ancho = _ancho;
        }

    }
}
