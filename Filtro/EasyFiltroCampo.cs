using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using EasyControlWeb.Form.Editor;
using EasyControlWeb.Form.Templates;
using EasyControlWeb.InterConeccion;
using static EasyControlWeb.EasyUtilitario.Enumerados.Configuracion.SeccionKey;

namespace EasyControlWeb.Filtro
{
    [
        AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Nombre"),
        ParseChildren(true, "Nombre"),
        ToolboxData("<{0}:EasyFiltroCampo runat=server></{0}:EasyFiltroCampo>")
    ]

    [Serializable]
    public class EasyFiltroCampo
    {
        private string nombre;
        private string descripcion;
        EasyUtilitario.Enumerados.TiposdeDatos tipodeDato;

        public EasyFiltroCampo() : this(String.Empty, String.Empty) { }

        public EasyFiltroCampo(string NOMBRE, string DESCRIPCION)
        {
            Nombre = NOMBRE;
            descripcion = DESCRIPCION;
        }

        [
            Category("Behavior"),
            DefaultValue(""),
            Description("Nombre del Campo de la BD o del Parametro Url"),
            NotifyParentProperty(true),
        ]
        public string Nombre { get { return nombre; } set { this.nombre = value; } }

        [
            Category("Behavior"),
            DefaultValue(""),
            Description("Descripcion entendible del campo para el usuario final"),
            NotifyParentProperty(true),
        ]
        public string Descripcion { get { return descripcion; } set { this.descripcion = value; } }

        [
            Category("Behavior"),
            DefaultValue(""),
            Description("Descripcion que se mostrará disponible para la utilizacion del usuario en la elaboracion de su criterio"),
            NotifyParentProperty(true),
        ]
        public EasyUtilitario.Enumerados.TiposdeDatos TipodeDato { get { return tipodeDato; } set { this.tipodeDato = value; } }

        EasyDataInterConect oEasyDataInterConect = new EasyDataInterConect();
        [TypeConverter(typeof(EasyFormColletionsEditor.Type_DataInterConect))]
        [Category("Editor"),
         Description("Permite conectar con web service para obtener los datos."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public EasyDataInterConect DataInterconect
        {
            get { return oEasyDataInterConect; }
            set { oEasyDataInterConect = value; }
        }

        EasyFormItemTemplate oEasyFormItemTemplate = new EasyFormItemTemplate();

        [TypeConverter(typeof(EasyFormColletionsEditor.Type_FormItemTemplate))]
        [Category("Editor"),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public EasyFormItemTemplate EasyControlAsociado
        {
            get
            {
                if (oEasyFormItemTemplate.TemplateType == "EasyITemplateTextBox")
                {
                    oEasyFormItemTemplate = new EasyITemplateTextBox(oEasyFormItemTemplate);
                }
                else if (oEasyFormItemTemplate.TemplateType == "EasyITemplateDatepicker")
                {
                    oEasyFormItemTemplate = new EasyITemplateDatepicker(oEasyFormItemTemplate);
                }
                else if (oEasyFormItemTemplate.TemplateType == "EasyITemplateNumericBox")
                {
                    oEasyFormItemTemplate = new EasyITemplateNumericBox(oEasyFormItemTemplate);
                }
                else if (oEasyFormItemTemplate.TemplateType == "EasyITemplateDropdownList")
                {
                    oEasyFormItemTemplate = new EasyITemplateDropdownList(oEasyFormItemTemplate);
                }
                else if (oEasyFormItemTemplate.TemplateType == "EasyITemplateAutoCompletar")
                {
                    oEasyFormItemTemplate = new EasyITemplateAutoCompletar(oEasyFormItemTemplate);
                }

                return oEasyFormItemTemplate;
            }
            set
            {
                oEasyFormItemTemplate = value;
            }
        }

        #region Implementacion por ahora en standby  hasta averriguar  como implelemar este modelo
        /*    Control oControlBase;// = new Control();

            [TypeConverter(typeof(Type_FormControlGeneric))]
            [Category("Editor"),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
            public Control EasyControl
            {
                get
                {
                    if (oControlBase is EasyDatepicker)
                    {
                        oControlBase = new  EasyDatepicker();
                    }
                    else if (oControlBase is EasyNumericBox)
                    {
                        oControlBase = new EasyNumericBox();
                    }

                    return oControlBase;
                }
                set
                {
                    if (value is EasyDatepicker)
                    {
                        oControlBase = (EasyDatepicker)value;
                    }
                    else if (value is EasyNumericBox)
                    {
                        oControlBase = (EasyNumericBox)value;
                    }
                    else {
                        oControlBase = value;
                    }
                }
            }
*/
        #endregion

        public override string ToString()
        {
            return "EasyCampo." + this.Nombre;
        }

        public string ToString(bool outClienteBE)
        {
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string strBaseBE = "{" + "Nombre" + ":" + cmll + this.Nombre + cmll + "," + "Descripcion" + ":" + cmll + this.Descripcion + cmll + "}";
            return strBaseBE.ToString();
        }

        public string ToCliente()
        {
            /* Referencia : GestorFiltro buscar la variable strCriterio
             * {"Contenga","Inicie con","Finalize con","Igual","No sea Igual","Mayor que","Menor que","Mayor o Igual que","Menor o Igual que" }*/

            string LstNotCriterio = "";
            switch (this.TipodeDato)
            {
                case EasyUtilitario.Enumerados.TiposdeDatos.Int:
                    LstNotCriterio = "0,1,2";
                    break;
                case EasyUtilitario.Enumerados.TiposdeDatos.String:
                    LstNotCriterio = "5,6,7,8";
                    break;
                case EasyUtilitario.Enumerados.TiposdeDatos.Date:
                    LstNotCriterio = "0,1,2,4";
                    break;
                case EasyUtilitario.Enumerados.TiposdeDatos.Double:
                    LstNotCriterio = "0,1,2";
                    break;
            }
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string strBaseBE = "{" + "Nombre" + ":" + cmll + this.Nombre + cmll + "," + "Descripcion" + ":" + cmll + this.Descripcion + cmll + ",TipoCtrl:" + cmll + this.EasyControlAsociado.GetTipo() + cmll + ",NotCriterio:" + cmll + LstNotCriterio + cmll + "}";
            return strBaseBE.ToString();
        }
    }
}
