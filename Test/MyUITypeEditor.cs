using EasyControlWeb.Form.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace EasyControlWeb.Test
{
    public class MyUITypeEditor : UITypeEditor
    {
        /*
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            System.Windows.MessageBox.Show("press ok to continue");
            return "You can't edit this";
        }
        */

        /*
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            EasyUsuario oEasyUsuario = value as EasyUsuario;

            oEasyUsuario.Email = "erosales1@hotmail.com";
            oEasyUsuario.Login = "erosale";
            oEasyUsuario.NroDocumento = "18018828";




            //IWindowsFormsEditorService svc = context.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            //if (svc != null)
           // {
             //   using (PersonEditorForm personEditorForm = new PersonEditorForm(oEasyUsuario))
             //   {
             //       if (svc.ShowDialog(personEditorForm) == DialogResult.OK)
              //      {
              //          return personEditorForm.Person;
              //      }
              //  }
           // }

            return value;
        }
    */
        /* public override object EditValue(ITypeDescriptorContext context,IServiceProvider provider, object value)
         {
             //Just as an example, show a message box containing values from owner object
             var MyEasyUsuario = ((EasyLogin)context.Instance).oEasyUsuario;


             //MessageBox.Show(string.Format("You can choose from {0}.",string.Join(",", list.Email)));

             return base.EditValue(context, provider, MyEasyUsuario);
         }*/
        /*  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
          {
              return UITypeEditorEditStyle.Modal;
          }*/

     


    }


    #region  Prueba

    /*
        public class MyConverter : TypeConverter
        {
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(typeof(EasyUsuario));
            }
        }

        public class UsuarioCOnfiguracion : TypeConverter
        {
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(typeof(EasyUsuario));
            }
        }

    */
    #endregion






}
