using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyControlWeb.InterConecion
{
    public class EasyBaseEntityBE
    {
        private object objResultBE = null;
        public EasyBaseEntityBE() { }
        public EasyBaseEntityBE(object objEntityResult) {
            objResultBE = objEntityResult;
        }
        

        
        public string GetValue(string FieldName) {
            Type tModelType = objResultBE.GetType();
            FieldInfo[] thisFieldInfo = objResultBE.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            string ValReturn = null;
            foreach (FieldInfo property in thisFieldInfo)
            {
                if (property.Name.ToUpper().Equals(FieldName.ToUpper()))
                {
                    try
                    {
                        ValReturn = property.GetValue(objResultBE).ToString();
                    }
                    catch (Exception ex)
                    {
                        ValReturn = "";
                    }
                }

            }
            return ValReturn;
        }
    }
}
