using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Estilo
{
    [TypeConverter(typeof(Type_StyleToolBarButtom))]
    [Serializable]
    public class EasyBtnStyle
    {
        string className;
        public string ClassName { get { return className; } set { className = value; } }

        string fontSize;
        [TypeConverter(typeof(FontSizeTypeConverter)),
        Description("Esta es una propiedad de cadena que tiene una colección de valores estándar definida en su TypeConverter (FontSizeTypeConverter)." +
                    " La cuadrícula de propiedades utiliza esta colección para crear una lista y proporcionarla al usuario para que la seleccione.")]
        public string FontSize { get { return fontSize; } set { fontSize = value; } }


        string textColor;
        public string TextColor { get { return textColor; } set { textColor = value; } }

        public EasyBtnStyle() {}
        public EasyBtnStyle(string _ClassName,string _FontSize,string _TextColor) {
            this.ClassName = _ClassName;
            this.FontSize = _FontSize;
            this.TextColor = _TextColor;
        }
    }
}
