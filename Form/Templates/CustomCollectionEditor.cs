using EasyControlWeb.Form.Controls;
using EasyControlWeb.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Templates
{
 

    public sealed  class CustomCollectionEditor : CollectionEditor
    {
        public CustomCollectionEditor() : base(typeof(List<Control>)){ }
       
        protected override object CreateInstance(Type itemType)
        {
            if (itemType == typeof(CheckBox))
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Text = "My CheckBox";
                return checkbox;

            }
            Control control = base.CreateInstance(itemType) as Control;
            //control.Text = "Other, hede";
            return control;
        }

        protected override CollectionEditor.CollectionForm CreateCollectionForm()
        {
            return base.CreateCollectionForm();
        }

        // here you can return a text which will be appeared in the list
        // for the given item
      /*  protected override string GetDisplayText(object value)
        {
            Control control = value as Control;
            return string.Format("{0} - {1}", control.GetType().Name, "Textoctrl");
        }
        */
        // we allow 3 types can be added to our collection
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[]
            {
                typeof(Button), typeof(CheckBox),typeof(EasyDatepicker)
            };
        }
    }

}
