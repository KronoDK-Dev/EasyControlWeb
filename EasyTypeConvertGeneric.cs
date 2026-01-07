using EasyControlWeb.Form.Controls;
using EasyControlWeb.Form.Templates;
using EasyControlWeb.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel; 
using System.Reflection;
using System.Web.UI;

namespace EasyControlWeb
{
    public class EasyTypeConvertGeneric
    {
        //Tipo para  la clase usuario 
        
    }


    public class UsuarioConfiguracion_TypeC : TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(EasyUsuario));
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            // JSON.NET checks for this and if false uses its default
            if (destinationType == typeof(string))
                return false;

            return base.CanConvertTo(context, destinationType);
        }
    }
   
    public class DataConfig_TypeC : ExpandableObjectConverter
    {
        public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)){return true;}
            else{return base.CanConvertFrom(context, sourceType);}
        }

        public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {return true;}
            else
            {return base.CanConvertTo(context, destinationType);}
        }

        public override object ConvertFrom(ITypeDescriptorContext context,System.Globalization.CultureInfo culture,object value)
        {
            if (value is string)
            {
                string[] valores = ((string)value).Split(' ');
                if (valores.Length == 2)
                {
                    return new DataDemoBE(Convert.ToInt32(valores[0]), valores[1]);
                }
                else
                {
                    return new DataDemoBE();
                }
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context,System.Globalization.CultureInfo culture,object value, Type destinationType)
        {
            if (value is string)
            {
                DataDemoBE oDataDemoBE = value as DataDemoBE;
                return oDataDemoBE.IdUsuario.ToString() + " " + oDataDemoBE.Login;
            }
            else
            {
                return base.ConvertTo(context, culture, value,destinationType);
            }
        }
    }



    //Usado en la clase login
    public class BloodTypeConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context){return true;}
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context){return true;}

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "O− Negativo", "O+ Positivo", "A−", "A+", "B−", "B+", "AB−", "AB+" });
        }
    }
    public class FontSizeTypeConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "1em", "1.5em", "2em", "2.5em", "3em", "3.5em", "4em", "4.5em" });
        }
    }

  

    #region Types Oficiales
    //http://www.windows-tech.info/3/0e2ab72f8a302924.php

   
    #endregion


  
    #region FormItemTemplate
    /*
     * AUTOR:Rosales AZABACHE Eddy
     * REFERENCIA: //https://books.google.com.pe/books?id=f2lcvqNAeo4C&pg=PA43&lpg=PA43&dq=c%23++System.Object++ExpandableObjectConverter&source=bl&ots=nyipdKNUki&sig=ACfU3U0n6syxdHeBWgtjRRVbm-3igz8MZg&hl=es&sa=X&ved=2ahUKEwiT_djFsoP6AhUEIbkGHeFuDRQ4HhDoAXoECBsQAw#v=onepage&q=c%23%20%20System.Object%20%20ExpandableObjectConverter&f=false
     * FECHA:13/09/2022
     */
   
    #endregion




    #region Aun Falta averiguar para compleatr la  imlementacion por ahora sera comentado implementado en EasyFiltroCampo
    public class Type_FormControlGeneric : ExpandableObjectConverter
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
            Control oControl = null;

            if (value is string)
            {

                switch (value.ToString())
                {
                    case "EasyITemplateDatepicker":
                        {
                            oControl = new EasyDatepicker();
                            break;
                        }
                    case "EasyITemplateNumericBox":
                        {
                            oControl = new EasyNumericBox();
                            break;
                        }
                    default:
                        {
                            oControl = null;
                            break;
                        }
                }
                return oControl;
            }
            return base.ConvertFrom(context, culture, value);


        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string convertedValue = "(None)";
                if (value is EasyDatepicker)
                {
                    convertedValue = "EasyITemplateDatepicker";
                }
                else if (value is EasyNumericBox)
                {
                    convertedValue = "EasyITemplateNumericBox";
                }

                return convertedValue;

            }
            return base.ConvertTo(context, culture, value, destinationType);

        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        private readonly object[] _LstFormControls = new object[] { null
                                                                    , new EasyDatepicker()
                                                                    , new EasyNumericBox()
                                                                    };

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(_LstFormControls);
        }

    }

    #endregion




}
