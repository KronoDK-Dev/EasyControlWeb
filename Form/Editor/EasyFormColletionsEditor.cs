using EasyControlWeb.Filtro;
using EasyControlWeb.Form.Controls;
using EasyControlWeb.Form.Estilo;
using EasyControlWeb.Form.Templates;
using EasyControlWeb.Form.Templates.ListView;
using EasyControlWeb.InterConeccion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web.UI; 
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;

namespace EasyControlWeb.Form.Editor
{
    public class EasyFormColletionsEditor
    {
        public sealed class AllItemsControls: CollectionEditor
        {
            public AllItemsControls() : base(typeof(List<Control>)) { }

            protected override object CreateInstance(Type itemType)
            {
                Control control = base.CreateInstance(itemType) as Control;
                return control;
            }

            protected override CollectionEditor.CollectionForm CreateCollectionForm()
            {
                return base.CreateCollectionForm();
            }
         
            protected override Type[] CreateNewItemTypes()
            {
                return new Type[]
                                {
                                typeof(EasyTextBox)
                                ,typeof(EasyNumericBox)
                                ,typeof(EasyUpLoad)
                                ,typeof(EasyDatepicker)
                                ,typeof(EasyAutocompletar)
                                ,typeof(EasyDropdownList)
                                ,typeof(EasyListAutocompletar)
                                ,typeof(EasyCheckBox)
                                };
            }
        }




        public class Type_Header : ExpandableObjectConverter
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
                    string[] valores = ((string)value).Split(' ');
                    if (valores.Length == 3)
                    {
                        return new EasyFormHeader(valores[0], valores[1], valores[2]);
                    }
                    else
                    {
                        return new EasyFormHeader();
                    }
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
                    EasyFormHeader oEasyFormHeader = value as EasyFormHeader;
                    return oEasyFormHeader.Titulo + " " + oEasyFormHeader.Descripcion + " " + oEasyFormHeader.Snippetby ;
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }



        public class Type_CarpetaUrl : ExpandableObjectConverter
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
                    //string[] valores = ((string)value).Split(' ');
                    string[] valores = ((string)value).Split(Convert.ToChar(EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal)); 
                    if (valores.Length == 4)
                    {
                        return new EasyPathLocalWeb(valores[0], valores[1], valores[2], valores[3]);
                    }
                    else
                    {
                        return new EasyPathLocalWeb();
                    }
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
                    EasyPathLocalWeb oEasyPathLocalWeb = value as EasyPathLocalWeb;
                    return oEasyPathLocalWeb.Serializado();
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }

        public class Type_Style : ExpandableObjectConverter
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
                    string[] valores = ((string)value).Split(' ');
                    if (valores.Length == 4)
                    {
                        Bootstrap.Talla oTipoTalla = (Bootstrap.Talla)System.Enum.Parse(typeof(Bootstrap.Talla), valores[2]);
                        Bootstrap.Tamaño oAncho = (Bootstrap.Tamaño)System.Enum.Parse(typeof(Bootstrap.Tamaño), valores[3]);

                        return new Bootstrap(valores[0], valores[1], oTipoTalla, oAncho);
                    }
                    else
                    {
                        return new Bootstrap();
                    }
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
                    Bootstrap oBootstrarp = value as Bootstrap;
                    return oBootstrarp.ClassName.ToString() + " " + oBootstrarp.ClassLabel.ToString() + " " + oBootstrarp.TipoTalla.ToString() + " " + oBootstrarp.Ancho.ToString();
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }


        public class Type_StyleToolBarButtom : ExpandableObjectConverter
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
                    string[] valores = ((string)value).Split(' ');
                    if (valores.Length == 3)
                    {
                        return new EasyBtnStyle(valores[0], valores[1], valores[2]);
                    }
                    else
                    {
                        return new EasyBtnStyle();
                    }
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
                    EasyBtnStyle oEasyBtnStyle = value as EasyBtnStyle;
                    return oEasyBtnStyle.ClassName.ToString() + " " + oEasyBtnStyle.FontSize+ " " + oEasyBtnStyle.TextColor;
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }

        public class Type_EasyGridViewGroupRow: ExpandableObjectConverter
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
                    string[] valores = ((string)value).Split(Convert.ToChar(EasyUtilitario.Constantes.Caracteres.Espacio.ToString()) );
                        return new EasyGridRowGroup(Convert.ToInt32(valores[0]), Convert.ToInt32(valores[1]));
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
                    EasyGridRowGroup oEasyGridRowGroup = value as EasyGridRowGroup;

                    return oEasyGridRowGroup.GroupedDepth.ToString() 
                            + EasyUtilitario.Constantes.Caracteres.Espacio.ToString() 
                            + oEasyGridRowGroup.ColIniRowMerge;
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }

     

        public class Type_DataInterConect : ExpandableObjectConverter
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
                    string[] valores = ((string)value).Split(Convert.ToChar(EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal));
                        List<EasyFiltroParamURLws> _UrlWebServicieParams = new List<EasyFiltroParamURLws>();
                        string[] arrParam = valores[2].Split(Convert.ToChar(EasyUtilitario.Constantes.Caracteres.SignoArroba));
                        if ((arrParam != null) && (arrParam.Length > 0)){
                            foreach (string ItemParam in arrParam) {
                                string[] strParam = ItemParam.Split(Convert.ToChar(EasyUtilitario.Constantes.Caracteres.PuntoyComa));
                                EasyFiltroParamURLws oEasyFiltroParamURLws = new EasyFiltroParamURLws();
                                oEasyFiltroParamURLws.ParamName = strParam[0];
                                oEasyFiltroParamURLws.Paramvalue = strParam[1];
                                EasyFiltroParamURLws.TipoObtenerValor oTipoObtenerValor = (EasyFiltroParamURLws.TipoObtenerValor)System.Enum.Parse(typeof(EasyFiltroParamURLws.TipoObtenerValor), strParam[3]);
                                oEasyFiltroParamURLws.ObtenerValor = oTipoObtenerValor;
                            // EasyFiltroParamURLws.TiposdeDatos oTipodeDato = (EasyFiltroParamURLws.TiposdeDatos)System.Enum.Parse(typeof(EasyFiltroParamURLws.TiposdeDatos), strParam[4]);
                            EasyUtilitario.Enumerados.TiposdeDatos oTipodeDato = (EasyUtilitario.Enumerados.TiposdeDatos)System.Enum.Parse(typeof(EasyUtilitario.Enumerados.TiposdeDatos), strParam[4]);
                            oEasyFiltroParamURLws.TipodeDato = oTipodeDato;

                        }
                            return new EasyDataInterConect(valores[0], valores[1], _UrlWebServicieParams);
                        }
                        return new EasyDataInterConect(valores[0], valores[1]);
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
                    string strParam = "";int Idx = 0;
                    EasyDataInterConect oEasyDataInterConect = value as EasyDataInterConect;
                    if (oEasyDataInterConect.UrlWebServicieParams != null)
                    {
                        foreach (EasyFiltroParamURLws oParam in oEasyDataInterConect.UrlWebServicieParams)
                        {
                            strParam += ((Idx == 0) ? "" : EasyUtilitario.Constantes.Caracteres.SignoArroba) 
                                                            + oParam.ParamName 
                                                            + EasyUtilitario.Constantes.Caracteres.PuntoyComa 
                                                            + oParam.Paramvalue 
                                                            + EasyUtilitario.Constantes.Caracteres.PuntoyComa 
                                                            + oParam.ObtenerValor.ToString()
                                                            + EasyUtilitario.Constantes.Caracteres.PuntoyComa
                                                            + oParam.TipodeDato.ToString();
                            Idx++;
                        }
                    }
                    return oEasyDataInterConect.UrlWebService 
                        + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal 
                        + oEasyDataInterConect.Metodo 
                        + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal 
                        + strParam;
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }



        public class Type_FormItemTemplate : ExpandableObjectConverter
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
                EasyFormItemTemplate oEasyFormItemTemplate = null;

                if (value is string)
                {
                    switch (value.ToString())
                    {
                        case "EasyITemplateTextBox":
                            {
                                oEasyFormItemTemplate = new EasyITemplateTextBox();
                                oEasyFormItemTemplate.TemplateType = value.ToString();
                                break;
                            }
                        case "EasyITemplateDatepicker":
                            {
                                oEasyFormItemTemplate = new EasyITemplateDatepicker();
                                oEasyFormItemTemplate.TemplateType = value.ToString();
                                break;
                            }
                        case "EasyITemplateNumericBox":
                            {
                                oEasyFormItemTemplate = new EasyITemplateNumericBox();
                                oEasyFormItemTemplate.TemplateType = value.ToString();
                                break;
                            }
                        case "EasyITemplateDropdownList":
                            oEasyFormItemTemplate = new EasyITemplateDropdownList();
                            oEasyFormItemTemplate.TemplateType = value.ToString();
                            break;
                        case "EasyITemplateAutoCompletar":
                            oEasyFormItemTemplate = new EasyITemplateAutoCompletar();
                            oEasyFormItemTemplate.TemplateType = value.ToString();
                            break;
                        case "EasyTemplateCheckBox":
                            oEasyFormItemTemplate = new EasyTemplateCheckBox();
                            oEasyFormItemTemplate.TemplateType = value.ToString();
                            break;

                        default:
                            {
                                oEasyFormItemTemplate = null;
                                break;
                            }
                    }
                   
                    return oEasyFormItemTemplate;
                }
                return base.ConvertFrom(context, culture, value);


            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    string convertedValue = "(None)";
                    if (value is EasyITemplateTextBox)
                    {
                        convertedValue = "EasyITemplateTextBox";
                    }
                    else if (value is EasyITemplateDatepicker)
                    {
                        convertedValue = "EasyITemplateDatepicker";
                    }
                    else if (value is EasyITemplateNumericBox)
                    {
                        convertedValue = "EasyITemplateNumericBox";
                    }
                    else if (value is EasyITemplateDropdownList)
                    {
                        convertedValue = "EasyITemplateDropdownList";
                    }
                    else if (value is EasyITemplateAutoCompletar)
                    {
                        convertedValue = "EasyITemplateAutoCompletar";
                    }
                    else if (value is EasyTemplateCheckBox)
                    {
                        convertedValue = "EasyTemplateCheckBox";
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
                                                                    , new EasyITemplateTextBox()
                                                                    , new EasyITemplateDatepicker()
                                                                    , new EasyITemplateNumericBox()
                                                                    , new EasyITemplateDropdownList()
                                                                    , new EasyITemplateAutoCompletar()
                                                                    , new EasyTemplateCheckBox()
                                                                    };

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new TypeConverter.StandardValuesCollection(_LstFormControls);
            }

        }







        public class Type_LisItemTemplate : ExpandableObjectConverter
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
                 EasyListViewTemplate oEasyListViewTemplate = null;
                 
                 if (value is string)
                 {
                     switch (value.ToString())
                     {
                         case "EasyITemplateListItem":
                             {
                                 oEasyListViewTemplate = new EasyITemplateListItem();
                                 oEasyListViewTemplate.TemplateType = value.ToString();
                                 break;
                             }
                         case "EasyITemplateListItemImg":
                             {
                                 oEasyListViewTemplate = new EasyITemplateListItemImg();
                                 oEasyListViewTemplate.TemplateType = value.ToString();
                                 break;
                             }
                         default:
                             {
                                 oEasyListViewTemplate = null;
                                 break;
                             }
                     }

                    return oEasyListViewTemplate;
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    string convertedValue = "(None)";
                    if (value is EasyITemplateListItem)
                    {
                        convertedValue = "EasyITemplateListItem";
                    }
                    else if (value is EasyITemplateListItemImg)
                    {
                        convertedValue = "EasyITemplateListItemImg";
                    }

                    return convertedValue;

                }
                return base.ConvertTo(context, culture, value, destinationType);

            }
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

             private readonly object[] _LstItemView = new object[] { null
                                                                     , new EasyITemplateListItem()
                                                                     , new EasyITemplateListItemImg()
                                                                     };

           public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new TypeConverter.StandardValuesCollection(_LstItemView);
            }

        }









    }
}
