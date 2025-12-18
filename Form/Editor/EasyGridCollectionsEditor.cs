using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Form.Editor
{
    public class EasyGridCollectionsEditor
    {
        public class Type_EasyGridViewExtend : ExpandableObjectConverter
        { 
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string)) { return true; }
                else { return base.CanConvertFrom(context, sourceType); }
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string))
                { return true; }
                else
                { return base.CanConvertTo(context, destinationType); }
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value is string)
                {
                    string[] valores = ((string)value).Split(Convert.ToChar(EasyUtilitario.Constantes.Caracteres.Espacio.ToString()));
                    return new EasyGridExtended(valores[0], valores[1], valores[2], valores[3]);
                }
                else
                {
                    return base.ConvertFrom(context, culture, value);
                }
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (value is string)
                {
                    EasyGridExtended oEasyGridExtended = value as EasyGridExtended;

                    return oEasyGridExtended.ItemColorMouseMove
                            + EasyUtilitario.Constantes.Caracteres.Espacio.ToString()
                            + oEasyGridExtended.ItemColorSeleccionado
                            + EasyUtilitario.Constantes.Caracteres.Espacio.ToString()
                            + oEasyGridExtended.RowItemClick
                            + EasyUtilitario.Constantes.Caracteres.Espacio.ToString()
                            + oEasyGridExtended.IdGestorFiltro;
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }
    }
}
