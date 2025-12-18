using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyControlWeb.Test
{
    public class FilteredFileNameEditor : System.Drawing.Design.UITypeEditor
    {
        private OpenFileDialog ofd = new OpenFileDialog();
        
        public override UITypeEditorEditStyle GetEditStyle(
         ITypeDescriptorContext context)
        {           
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(
         ITypeDescriptorContext context,
         IServiceProvider provider,
         object value)
        {
            ofd.FileName = value.ToString();
            ofd.Filter = "Text File|*.txt|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }
            return base.EditValue(context, provider, value);
        }
    }
}
