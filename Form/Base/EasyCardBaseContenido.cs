using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Resources;

namespace EasyControlWeb.Form.Base
{
    public  class EasyCardBaseContenido: CompositeControl
    {
        public string Icono { get; set; }
        public string Comentario { get; set; }
        public string IconoStatus { get; set; }
        public string DescripcionStatus { get; set; }
        public string FechaHoraStatus { get; set; }

        public EasyCardBaseContenido() { }


        string IdLocal = "";
        HtmlGenericControl HtmlComentario()
        {
            HtmlGenericControl h6 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("h6", "card-subtitle mb-2 text-muted");
                HtmlGenericControl p = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("p", "card-text text-muted small ");
                    HtmlGenericControl Img = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("img", "mr-1 ");
                        Img.Attributes["src"] = this.Icono;
                        Img.Attributes["width"] = "19";
                        Img.Attributes["height"] = "19";
                      p.Controls.Add(Img);
                        // p.Controls.Add(new LiteralControl("<span class='" + this.IconoStatus + "'></span>"));
                        //p.Controls.Add(new LiteralControl("<i class='fa fa-users text-muted '></i> " + this.DescripcionStatus + " <span class='vl ml-1 mr-2 '></span>"));

                      p.Controls.Add(new LiteralControl("<i class='" + this.IconoStatus + " '></i> " + this.DescripcionStatus + " <span class='vl ml-1 mr-2 '></span>"));
            // p.Controls.Add(new LiteralControl("<span></span>Updated by <span class='font-weight-bold'> " + this.Descripcion + "</span> "+ this.Fecha));
                    HtmlGenericControl span = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("span", "font-weight-bold");
                                        IdLocal = "cmt_" + this.Attributes["IdCard"];
                                        span.ID = IdLocal;
                                        span.Attributes["IdGrp"] = this.Attributes["IdGrp"];//Contenedor de los cards 
                                        span.Attributes["ObjBE"] = this.Attributes["ObjBE"];
                                        span.Controls.Add(new LiteralControl(this.Comentario));
                                        string fncOnClick= IdLocal + ".EditComentario('" + this.Attributes["IdCard"] + "')";
                                        span.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = fncOnClick;
                      p.Controls.Add(span);
                        span = new HtmlGenericControl();
                        span.Controls.Add(new LiteralControl(this.FechaHoraStatus));
                     p.Controls.Add(span);

            //p.Controls.Add(new LiteralControl("<span class='font-weight-bold'> " + this.Comentario + "</span> <span>" + this.FechaHoraStatus + "</span>"));


            h6.Controls.Add(p);
           return h6;

        }

        #region evento naturales
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
            if (!IsDesign())
            {
                //Registra el Script
                HtmlComentario().RenderControl(writer);

                string ScriptComentario = @"<script>
                                                var " + IdLocal + @"={};
                                                " + IdLocal + @".EditComentario=function(IdCard){
                                                                                " + this.Attributes["IdCard"] + @".OnCardEvent('OnComent',IdCard);
                                                                            }
                                            </script>
                                        ";

                (new LiteralControl(ScriptComentario)).RenderControl(writer);
            }
            else
            {
                (new LiteralControl("Comentario Base")).RenderControl(writer);
            }
        }
        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        #endregion


    }
}
