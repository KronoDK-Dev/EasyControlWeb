using EasyControlWeb.Filtro;
using EasyControlWeb.InterConeccion;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;
using static EasyControlWeb.EasyUtilitario.Enumerados;

namespace EasyControlWeb.InterConecion
{
    /*referencia:https://programmerclick.com/article/378049431/
     */
    public static class EasyWebServieHelper
    {
        /// <summary>  
        /// Invocación dinámica de WebService
        /// </summary>  
        /// <param name = "url"> Dirección del servicio web </ param>
        /// <param name = "classname"> nombre de clase </ param>
        /// <param name = "methodname"> nombre del método (nombre del módulo) </ param>
        /// <param name = "args"> lista de parámetros </ param>
        /// <returns>object</returns>  

        public static object InvokeWebService(EasyDataInterConect oEasyDataInterConect)
        {
            return InvokeWebService("", oEasyDataInterConect);
        }
        public static object InvokeWebService(string UrlApp, EasyDataInterConect oEasyDataInterConect)
        {
            object[] param = new object[oEasyDataInterConect.UrlWebServicieParams.Count]; int i = 0;

            foreach (EasyFiltroParamURLws objParam in oEasyDataInterConect.UrlWebServicieParams)
            {

                //switch ((TiposdeDatos)System.Enum.Parse(typeof(TiposdeDatos), oEntity["TipoDato"].ToString()))
                switch (objParam.TipodeDato)
                {
                    case TiposdeDatos.String:
                        param[i] = objParam.Paramvalue;
                        break;
                    case TiposdeDatos.Int:
                        param[i] = Convert.ToInt32(objParam.Paramvalue);
                        break;
                    case TiposdeDatos.Double:
                        param[i] = Convert.ToDouble(objParam.Paramvalue);
                        break;
                }
                i++;
            }
            string PathApp = UrlApp + oEasyDataInterConect.UrlWebService;
            return EasyWebServieHelper.InvokeWebService(PathApp, "", oEasyDataInterConect.Metodo, param);

        }
        public static object InvokeWebService2(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "ServiceBase.WebService.DynamicWebLoad";

            if (string.IsNullOrEmpty(classname))
            {
                classname = EasyWebServieHelper.GetClassName(url);
            }

            // Leer el WSDL con timeout extendido usando tu clase WebClient_Tiempo
            using (var wc = new WebClient_Tiempo())
            {
                wc.Timeout = 600000; // 10 minutos
                using (Stream stream = wc.OpenRead(url + "?WSDL"))
                {
                    ServiceDescription sd = ServiceDescription.Read(stream);
                    ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                    sdi.AddServiceDescription(sd, "", "");

                    CodeNamespace cn = new CodeNamespace(@namespace);
                    CodeCompileUnit ccu = new CodeCompileUnit();
                    ccu.Namespaces.Add(cn);
                    sdi.Import(cn, ccu);

                    CSharpCodeProvider csc = new CSharpCodeProvider();
#pragma warning disable CS0618 // ICodeCompiler es obsoleto pero se requiere en este enfoque
                    ICodeCompiler icc = csc.CreateCompiler();
#pragma warning restore CS0618

                    CompilerParameters cplist = new CompilerParameters
                    {
                        GenerateExecutable = false,
                        GenerateInMemory = true
                    };
                    cplist.ReferencedAssemblies.Add("System.dll");
                    cplist.ReferencedAssemblies.Add("System.XML.dll");
                    cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                    cplist.ReferencedAssemblies.Add("System.Data.dll");

                    CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                    if (cr.Errors.HasErrors)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (CompilerError ce in cr.Errors)
                        {
                            sb.AppendLine(ce.ToString());
                        }
                        throw new Exception(sb.ToString());
                    }

                    Assembly assembly = cr.CompiledAssembly;
                    Type t = assembly.GetType(@namespace + "." + classname, true, true);
                    object obj = Activator.CreateInstance(t);

                    // Intentar establecer la propiedad Timeout si existe
                    PropertyInfo timeoutProperty = t.GetProperty("Timeout");
                    if (timeoutProperty != null && timeoutProperty.CanWrite)
                    {
                        timeoutProperty.SetValue(obj, 600000, null); // 10 minutos
                    }

                    MethodInfo mi = t.GetMethod(methodname);
                    try
                    {

                        return mi.Invoke(obj, args);
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw new Exception($"Error al invocar el método '{methodname}': {ex.InnerException?.Message ?? ex.Message}", ex);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error inesperado al invocar el método '{methodname}': {ex.Message}", ex);
                    }
                }
            }
        }

        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "ServiceBase.WebService.DynamicWebLoad";
            if (classname == null || classname == "")
            {
                classname = EasyWebServieHelper.GetClassName(url);
            }
            // Obtener lenguaje de descripción de servicio (WSDL)
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url + "?WSDL");
            ServiceDescription sd = ServiceDescription.Read(stream);
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);
            // Generar código de clase de proxy de cliente
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider csc = new CSharpCodeProvider();
            ICodeCompiler icc = csc.CreateCompiler();
            // Establecer los parámetros del compilador
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");
            // Compilar clase de proxy
            CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }
            // Genera una instancia de proxy y llama al método
            System.Reflection.Assembly assembly = cr.CompiledAssembly;
            Type t = assembly.GetType(@namespace + "." + classname, true, true);
            object obj = Activator.CreateInstance(t);
            methodname = methodname.Replace("\r\n", "");
            System.Reflection.MethodInfo mi = t.GetMethod(methodname);
            return mi.Invoke(obj, args);
        }
        private static string GetClassName(string url)
        {
            string[] parts = url.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }

    }






}
