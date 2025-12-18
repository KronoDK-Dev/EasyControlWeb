using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*https://stackoverflow.com/questions/630803/associating-enums-with-strings-in-c-sharp*/
namespace EasyControlWeb.Form.Controls
{
   

    public  class EasyItemLog
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public TipoItemLog Tipo { get; set; }

        
        public EasyItemLog() { }
    public EasyItemLog(string message) { }

    }

    public static class EasyItemLogExtensions
    {
        public static string ToDescription(this TipoItemLog Describe)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])Describe
               .GetType()
               .GetField(Describe.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
