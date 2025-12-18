using EasyControlWeb.Form.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Controls.Cards
{
   
   
    public class EasyPanelUsers: EasyPanelBase
    {
        public HtmlGenericControl ULContentUsers;
        string NombreCtrlContent = "";

        [Browsable(false)]
        List<EasyUserButton> oEasyUserButton;

        [Editor(typeof(EasyControlCollection.EasyFormCollectionCardUserButtomEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contiene los botones disponibles en cada item Card")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyUserButton> ButtonCollections
        {
            get
            {
                if (oEasyUserButton == null)
                {
                    oEasyUserButton = new List<EasyUserButton>();
                }
                return oEasyUserButton;
            }
        }


        public EasyPanelUsers() : this(String.Empty) { }

        public EasyPanelUsers(string ApellidosyNombres)
        {
            oEasyUserButton = new List<EasyUserButton>();
        }

        string ScriptCollection = "";
        HtmlGenericControl HtmlPanelUsers()
        {
            HtmlGenericControl ocol = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", " col-md-auto ");
            ocol = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", " col-md-auto ");

                    NombreCtrlContent ="ctrl_" + this.ClientID;
                    ULContentUsers = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("ul", "list-inline");
                    ULContentUsers.ID = NombreCtrlContent;
                    //Agrega el control adicionar
                    EasyUserButton oEasyUserButtonPlus = new EasyUserButton();
                    oEasyUserButtonPlus.Id = "ibtnPlus";
                    oEasyUserButtonPlus.ApellidosyNombres = "Agregar  Nuevo Usuario";
                    oEasyUserButtonPlus.PathImagen = "https://img.icons8.com/ios/50/000000/plus.png";
                    this.oEasyUserButton.Add(oEasyUserButtonPlus);

                    foreach (EasyUserButton oEasyUserButton in this.oEasyUserButton){
                            HtmlGenericControl oLi = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("li", "list-inline-item");
                            oLi.ID = this.ClientID +"_"+ oEasyUserButton.Id;
                            oLi.Attributes["UserButton"] = new JavaScriptSerializer().Serialize(oEasyUserButton).Replace(EasyUtilitario.Constantes.Caracteres.ComillaDoble, EasyUtilitario.Constantes.Caracteres.ComillaSimple);
                            ScriptCollection += this.ClientID + @".ButtonCollections.Add(" + oLi.Attributes["UserButton"] + ");";

                            oLi.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] =  this.ClientID +".onButtonClick(" + this.ClientID + ",this);";
                                HtmlGenericControl oImg = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("img", "  rounded-circle img-fluid ");
                                oImg.Attributes["width"] = "35";
                                oImg.Attributes["height"] = "35";
                                oImg.Attributes["src"] = oEasyUserButton.PathImagen;
                                oImg.Style.Add("cursor", "pointer");
                        oLi.Controls.Add(oImg);
                        ULContentUsers.Controls.Add(oLi);  
                    }

                ocol.Controls.Add(ULContentUsers);
            // return ULContentUsers;
            return ocol;
        }


        #region Eventos Propios
        protected override void OnInit(EventArgs e)
        {
            // base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (!IsDesign())
            {
                //Registra el Control
                this.HtmlPanelUsers().RenderControl(writer);
                //Registra el Script
               
               // EasyUtilitario.Helper.Data.CrearObjetoToClient(oEasyUserButtonPlus);    


                string fncExterna = ((this.fncItemOnClick == null) ? "" : this.fncItemOnClick+ "(_EasyPanelUsers,oUser);");
                string ScriptEvents = @"<script>
                                            var "+ this.ClientID +@"={};
                                            "   + this.ClientID + @".ButtonCollections=new Array();
                                            "   + ScriptCollection + @"
                                            "   + this.ClientID + @".onButtonClick=function(_EasyPanelUsers,e){
                                                        var oBtnUser = jNet.get(e);
                                                        var oUser = oBtnUser.attr('UserButton').toString().SerializedToObject();
                                            "   + fncExterna + @"
                                            }
                                            " + this.ClientID + @".CrearCtrl=function(oEasyUserButton){
                                                                            var objStrTransfer='';
                                                                                var IdCtrl='" + this.ClientID + @"_' + oEasyUserButton.Id;
                                                                                var objContent  = jNet.get('" + NombreCtrlContent + @"');
                                                                               
                                                                                var oli = jNet.create('li');
                                                                                    oli.attr('class','list-inline-item')
                                                                                        .attr('id',IdCtrl)
                                                                                        .attr('UserButton',objStrTransfer.toString().BaseSerialized(oEasyUserButton));
                                                                                        oli.addEvent('click',function(){"
                                                                                                                + this.ClientID + @".onButtonClick(" + this.ClientID + @",this);
                                                                                                            });

                                                                                var oImg = jNet.create('img');
                                                                                    oImg.attr('class','  rounded-circle img-fluid ')
                                                                                        .attr('width','35').attr('height','35')
                                                                                        .attr('src',oEasyUserButton.PathImagen)
                                                                                        .css('cursor','pointer');
                                                                                oli.insert(oImg);
                                                                            objContent.insert(oli);
                                            }
                                            " + this.ClientID + @".Add=function(oEasyUserButton){
                                                                            var length = " + this.ClientID + @".ButtonCollections.length-1;
                                                                            " + this.ClientID + @".ButtonCollections.splice(length,0,oEasyUserButton);
                                                                            " + this.ClientID + @".Reload();
                                                                        }
                                            " + this.ClientID + @".Reload=function(){
                                                                            var objContent  = jNet.get('" + NombreCtrlContent + @"');
                                                                                objContent.clear();
                                                                            "   + this.ClientID + @".ButtonCollections.forEach(function(oEasyUserButton,p){
                                                                                             " + this.ClientID + @".CrearCtrl(oEasyUserButton);
                                                                                    });
                                                                        }

                                        </script>
                                        ";
                (new LiteralControl(ScriptEvents)).RenderControl(writer);

            }
            else
            {
                (new LiteralControl("UserControlList")).RenderControl(writer);
            }
        }

        #endregion

        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }



    }
}
