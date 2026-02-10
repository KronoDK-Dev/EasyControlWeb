using EasyControlWeb.Filtro;
using EasyControlWeb.Form.Base;
using EasyControlWeb.InterConeccion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace EasyControlWeb.Form.Controls
{
        [
           AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
           AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
           DefaultProperty("fncOnChange"),
           ParseChildren(true, "fncOnChange"),
           ToolboxData("<{0}:EasyTimePicker runat=server></{0}:EasyTimePicker")
       ]



    [Serializable]
    public class EasyTimePicker : CompositeControl
    {
        public EasyDropdownList ddlHora;
        public EasyDropdownList ddlMinutos;
        public EasyDropdownList ddlAMPM;



        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncOnChange { get; set; }


        public EasyTimePicker(string Formato) { }

        public EasyTimePicker() : base()
        {
            if (ddlHora == null) { ddlHora = new EasyDropdownList(); }
            if (ddlMinutos == null) { ddlMinutos = new EasyDropdownList(); }
            if (ddlAMPM == null) { ddlAMPM = new EasyDropdownList(); }
        }


        protected override void OnInit(EventArgs e)
        {
          
            this.Attributes["disabled"] = ((this.Enabled) ? null : "disabled");
            ListItem li;
            if (this.Enabled == false)
            {
                ddlHora.Style["border-color"] = "#C0C0C0";
                ddlHora.Style["color"] = "#0000";
                /*if (this.Attributes["required"] != null){txtText.Attributes["required"] = "";}*/

                ddlMinutos.Style["border-color"] = "#C0C0C0";
                ddlMinutos.Style["color"] = "#0000";

                ddlAMPM.Style["border-color"] = "#C0C0C0";
                ddlAMPM.Style["color"] = "#0000";
              
            }
            for (int i = 1; i <= 12; i++)
            {
                li = new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0'));
                ddlHora.Items.Add(li);
            }
            ddlHora.Items.Insert(0,new ListItem("---", "-1"));

            for (int i = 0; i <= 60; i++)
            {
                li = new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0'));
                ddlMinutos.Items.Add(li);
            }
            ddlMinutos.Items.Insert(0, new ListItem( "---", "-1"));
            /*-------------------------------------*/
            li = new ListItem("AM", "AM");
            ddlAMPM.Items.Add(li);

            li = new ListItem("PM", "PM");
            ddlAMPM.Items.Add(li);
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            ddlHora.ID = "ddlH";
            ddlHora.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onchange.ToString(), this.fncOnChange + "('H',this)");
            ddlHora.Attributes["class"] = "timeSelectds";
            ddlHora.Attributes["required"] = "";
            ddlHora.Attributes["TypeIn"] = "TIMEPICK";

            this.Controls.Add(ddlHora);

            ddlMinutos.ID = "ddlM";
            ddlMinutos.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onchange.ToString(), this.fncOnChange + "('M',this)");
            ddlMinutos.Attributes["class"] = "timeSelectds";
            ddlMinutos.Attributes["required"] = "";
            ddlMinutos.Attributes["TypeIn"] = "TIMEPICK";
            this.Controls.Add(ddlMinutos);

            ddlAMPM.ID = "ddlAP";
            ddlAMPM.Attributes["class"] = "timeSelectds";
            ddlAMPM.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onchange.ToString(), this.fncOnChange + "('AP',this)");
           

            this.Controls.Add(ddlAMPM);
        }

        [Browsable(false)]
        public void SetValue(string Hora,string Minuto, string AP)
        {
            ddlHora.SetValue(Hora);
            ddlMinutos.SetValue(Minuto);
            ddlAMPM.SetValue(AP.ToUpper().Trim());
        }
        [Browsable(false)]
        public void SetValue(string Time)
        {
            Time = Time.Replace(".", "");
            if (IsValidTime(Time) ==true) {
                string[] stime = Time.Split(' ');
                string []HoraMin = stime[0].Split(':');
                SetValue(HoraMin[0], HoraMin[1], stime[1]);
            }
        }

        public bool IsValidTime(string thetime)
        {
            Regex checktime =new Regex(@"^(?:(?:0?[1-9]|1[0-2]):[0-5][0-9]\s?(?:[AP][Mm]?|[ap][m]?)?|(?:00?|1[3-9]|2[0-3]):[0-5][0-9])$");

            return checktime.IsMatch(thetime);
        }


        HtmlGenericControl htmlTimePick() {
            
            HtmlGenericControl dvPanel = new HtmlGenericControl("div");
            dvPanel.Attributes["class"] = "time-picker";

            HtmlTable tbl = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(1, 4);
            tbl.Attributes.Add("style", "width:100%");

            tbl.Rows[0].Cells[0].Controls.Add(ddlHora);
            tbl.Rows[0].Cells[0].Attributes["reference"] = ddlHora.ClientID;

            HtmlGenericControl sp = new HtmlGenericControl("span");
            sp.InnerText = ":";
            tbl.Rows[0].Cells[1].Controls.Add(sp);

            tbl.Rows[0].Cells[2].Controls.Add(ddlMinutos);
            tbl.Rows[0].Cells[2].Attributes["reference"] = ddlMinutos.ClientID;

            tbl.Rows[0].Cells[3].Controls.Add(ddlAMPM);

            dvPanel.Controls.Add(tbl); 
            return dvPanel;
        }


        protected override void Render(HtmlTextWriter writer)
        {
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            htmlTimePick().RenderControl(writer);

            if (!IsDesign())
            {
                //dpHIni_ddlH
                string scriptGet = @" var " + this.ClientID + @"={};
                                        var " + this.ClientID +"_ctrlH = jNet.get('" + ddlHora.ClientID + @"');
                                        var " + this.ClientID +"_ctrlM = jNet.get('" + ddlMinutos.ClientID + @"');
                                        var " + this.ClientID +"_ctrlAP = jNet.get('" + ddlAMPM.ClientID + @"');
                                    " +
                                    this.ClientID + @".GetValue=function(){
                                                                        var ListItemH = " + this.ClientID + @"_ctrlH.options[" + this.ClientID + @"_ctrlH.selectedIndex];
                                                                        var ListItemM = " + this.ClientID + @"_ctrlM.options[" + this.ClientID + @"_ctrlM.selectedIndex];
                                                                        var ListItemAP = " + this.ClientID + @"_ctrlAP.options[" + this.ClientID + @"_ctrlAP.selectedIndex];
                                                                        var AP ='';
                                                                        if(ListItemAP.value=='AM'){
                                                                            AP='a.m.';
                                                                        }
                                                                        else{
                                                                            AP='p.m.';
                                                                        }
                                                                    return ListItemH.value + ':' + ListItemM.value + ' ' + AP;
                                                                }
                                    " + this.ClientID + @".SetValue=function(Hora,Minuto,AP){
                                                                 " + this.ClientID + @"_ctrlH.FindValue(Hora);
                                                                 " + this.ClientID + @"_ctrlM.FindValue(Minuto);
                                                                 " + this.ClientID + @"_ctrlAP.FindValue(AP);
                                                            }
                                    ";

                (new LiteralControl("\n <script>\n" + scriptGet + "\n" + "</script>\n")).RenderControl(writer);



            }

        }

















        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }


    }
}
