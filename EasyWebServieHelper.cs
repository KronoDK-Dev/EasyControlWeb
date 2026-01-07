using System.IO;
using System.Web.Services.Description;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Net;
using System.Text;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using EasyControlWeb.Filtro;
using System.Collections.Generic;
using System.Xml;
//using System.Net.Http.Headers;

namespace EasyControlWeb
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



        public static DataTable CallServiceRestOracle(string Metodo,  List<EasyFiltroParamURLws> oParams)
        {
            //TagName = "//LineaNegocio"
            string TagName = "Table";
            string strData = LoadBackGroundWS(Metodo, oParams).Result;

            XmlDocument Mi_xml = new XmlDocument();
            Mi_xml.LoadXml(strData);

            XmlNodeList datos_XML = Mi_xml.SelectNodes(TagName); // se coloca el nombre de nodo que es la tabla/vista

            DataSet conjunto_dx = new DataSet();
            conjunto_dx.ReadXml(new XmlNodeReader(Mi_xml));

            DataTable dt = conjunto_dx.Tables[0];
            dt.TableName = "Table";
            return dt;
        }

        static async Task<string> LoadBackGroundWS(string Metodo, List<EasyFiltroParamURLws> oParams)
        {
            //referencia : https://stackoverflow.com/questions/40291526/why-async-function-returning-system-threading-tasks-task1system-string
            string susername = EasyUtilitario.Helper.Configuracion.UserWSOracle.ToString();
            string spassword = EasyUtilitario.Helper.Configuracion.PwdWSOracle.ToString();
            string sParametros = "";

            if ((oParams != null) && (oParams.Count > 0))
            {
                int idx = 0;
                foreach (EasyFiltroParamURLws param in oParams)
                {
                    sParametros += ((idx == 0) ? "" : EasyUtilitario.Constantes.Caracteres.SignoAmperson) + param.ParamName + EasyUtilitario.Constantes.Caracteres.SignoIgual + param.Paramvalue;
                }
            }

            string sServicio = EasyUtilitario.Helper.Configuracion.WebServiceDBOracle.ToString(); //"http://10.10.90.168:7060/xml_oracle/xml_api";
                                                                                                  //string sMetodo = ".get_planilla?"; // En este metodo esta expuesta toda la información necesaria

            using (var httpWS = new HttpClient())
            {
                httpWS.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(susername + ":" + spassword)));
                var resultado = await httpWS.GetAsync(sServicio + EasyUtilitario.Constantes.Caracteres.Punto + Metodo + EasyUtilitario.Constantes.Caracteres.SignoInterrogacion + sParametros);
                string get_contenido = await resultado.Content.ReadAsStringAsync();

                return get_contenido;
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