using EasyControlWeb.Form.Templates.ListView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Controls
{

    public class EasyListViewItemTipo
    {

       // private TipoItemView _oTipoListItem;

        /*Coleccion de tipos de Control Listview*/
        EasyListViewTemplate oEasyListViewTemplate = new EasyListViewTemplate( );
       // EasyListViewTemplate oEasyListViewTemplate;

        [TypeConverter(typeof(Type_LisItemTemplate))]
        [Category("Editor"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public EasyListViewTemplate EasyListItemAsociado
        {
            get
            {
                 if (oEasyListViewTemplate.TemplateType == "EasyITemplateListItem")
                 {
                    oEasyListViewTemplate.TemplateType = "EasyITemplateListItem";
                    oEasyListViewTemplate = new EasyITemplateListItem(oEasyListViewTemplate);
                 }
                 else if (oEasyListViewTemplate.TemplateType == "EasyITemplateListItemImg")
                 {
                    oEasyListViewTemplate.TemplateType = "EasyITemplateListItemImg";
                    oEasyListViewTemplate = new EasyITemplateListItemImg(oEasyListViewTemplate);
                 }
                 return oEasyListViewTemplate;

               /* if (_oTipoListItem == TipoItemView.Simple)
                {
                        oEasyListViewTemplate.TemplateType = "EasyITemplateListItem";
                        oEasyListViewTemplate = new EasyITemplateListItem(oEasyListViewTemplate);
                }
                else if (_oTipoListItem == TipoItemView.ImagenCircular)
                {
                    oEasyListViewTemplate.TemplateType = "EasyITemplateListItemImg";
                    oEasyListViewTemplate = new EasyITemplateListItemImg(oEasyListViewTemplate);
                }
                return oEasyListViewTemplate;*/
            }
            set
            {
                oEasyListViewTemplate = value;
            }
        }


        public EasyListViewItemTipo() { }
        /*public EasyListViewItemTipo(TipoItemView oTipoListItem)
        {
            _oTipoListItem = oTipoListItem;
        }*/


    }
}
