using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Resources;

namespace EasyControlWeb.Form.Controls.Cards
{
    public class EasyCardToolBar: CompositeControl
    {
       /* [Category("Identificacion"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string Id { get; set; }*/

        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncToolBaOnClick { get; set; }

       /*Para la barra de herramientas en el footer*/
        [Browsable(false)]
        List<EasyCardButton> oEasyCardButton;

        [Editor(typeof(EasyControlCollection.EasyFormCollectionCardItemButtomEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contiene los botones disponibles en cada item Card")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyCardButton> ButtonCollections
        {
            get
            {
                if (oEasyCardButton == null)
                {
                    oEasyCardButton = new List<EasyCardButton>();
                }
                return oEasyCardButton;
            }
        }



        public EasyCardToolBar() : this(String.Empty) { }

        public EasyCardToolBar(string Titulo)
        {
            oEasyCardButton = new List<EasyCardButton>();
        }

        HtmlGenericControl HtmlPanelToolButtons()
        {
            HtmlGenericControl ocol = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", " col-md-auto ");
            foreach (EasyCardButton EasyCardItemButton in this.oEasyCardButton)
            {
                if (EasyCardItemButton.TipoBoton == Base.EasyButtonType.button)
                {
                    HtmlGenericControl a = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("a", "btn btn-outlined btn-black text-muted bg-transparent");
                    a.Attributes["href"] = "#";
                    a.Attributes["data-wow-delay"] = "0.7s";
                    a.Attributes["IdObjParent"] = this.Attributes["IdObjParent"];
                    a.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = this.ClientID + ".onButtonClick(this);";
                    
                    a.Attributes["ObjBE"] = new JavaScriptSerializer().Serialize(EasyCardItemButton).Replace(EasyUtilitario.Constantes.Caracteres.ComillaDoble, EasyUtilitario.Constantes.Caracteres.ComillaSimple);
                    HtmlGenericControl oimg = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("img");
                    oimg.Attributes["src"] = EasyCardItemButton.Icono;
                    oimg.Attributes["width"] = "19";
                    oimg.Attributes["height"] = "19";
                    a.Controls.Add(oimg);
                    a.Controls.Add(new LiteralControl("<small>" + EasyCardItemButton.Titulo + "</small>"));
                    ocol.Controls.Add(a);
                }
                else
                {
                    //Separador
                    ocol.Controls.Add(new LiteralControl("<i class='mdi mdi-settings-outline'></i>"));
                }
            }
            return ocol;
        }

        #region Eventos Propios
        protected override void OnInit(EventArgs e)
        {
            // base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            //this.Controls.Clear();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            
            if (!IsDesign())
            {
                this.ID = this.ID.Replace("-","_");
                string fncToolbtnItem= ((fncToolBaOnClick == null) ? "" : fncToolBaOnClick + "(oBtnBE,IdParent);");
                //Registra el Control
                this.HtmlPanelToolButtons().RenderControl(writer);
                string EventoExterno = ((this.Attributes["IdCard"] != null) ? this.Attributes["IdCard"] + ".OnCardEvent('ToolBarCard',IdParent);" : "");
                
                string strScriptTool = @"<script>
                                            var " + this.ClientID + @"={};
                                            " + this.ClientID + @".onButtonClick=function(e){
                                                                                        event.preventDefault()
                                                                                        var obtn = jNet.get(e);
                                                                                        var oBtnBE = obtn.attr('ObjBE').toString().SerializedToObject();
                                                                                        var IdParent  = obtn.attr('IdObjParent');
                                                                                      /*  var ObjParent = jNet.get(IdParent);
                                                                                        ObjParentBE =  ObjParent.attr('ObjBE').toString().SerializedToObject();*/
                                                                                        //Llama al metodo de sucesos de control vinculado
                                                                                        " + EventoExterno + @"

                                                                                          
                                            " + fncToolbtnItem + @"
                                            }
                                         </script>
                                        ";
                (new LiteralControl(strScriptTool)).RenderControl(writer);
                
            }
            else
            {
                (new LiteralControl("ToolBar")).RenderControl(writer);
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
