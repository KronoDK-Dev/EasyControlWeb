using EasyControlWeb.Filtro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Controls
{
    /*  
     * Lib Ref:  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.css">
                 <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.js"></script>
     */

    /*
     * REEFERENCIA DE LA FUNCIONALIDAD :https://craftpip.github.io/jquery-confirm/
    */

    public class EasyMessageBox : CompositeControl
    {
        #region Variables
        string NombreFuncion = "";
        #endregion

        #region Controles
        #endregion

        #region propiedades

        [Category("Diseño")]
        public EasyUtilitario.Enumerados.MessageBox.Tipo Tipo {get;set;}

        [Category("Diseño")]
        public EasyUtilitario.Enumerados.MessageBox.AlertStyle AlertStyle { get; set; }

        [Category("Display")]
        public string Titulo { get; set; }

        [Category("Display")]
        public string Contenido { get; set; }
        [Category("Display")]
        public string PromptEtiqueta { get; set; }


        [Category("ScriptEvent")]
        public string fnButtonClick { get; set; }
        [Category("ScriptEvent")]
        public string fnEventKey { get; set; }

        [Category("Adicional")]
        public string ContenidoAdicional { get; set; }
        [Category("Adicional")]
        public string KeysAction { get; set; }
        [Category("Adicional")]
        public string btnClass { get; set; }

        #endregion




        #region Metodos

        string AlertType(EasyUtilitario.Enumerados.MessageBox.AlertStyle AlertStyle) {
            string[,] Ico_Type = new string[4,2] {{ "fa fa-smile-o", "blue" },{ "fa fa-question-circle-o", "orange" }, { "fa fa-question", "orange" }, { "fa fa-question", "orange" } };
            string Icono = "";
            string _type = "";
            switch (AlertStyle)
            {
                case EasyUtilitario.Enumerados.MessageBox.AlertStyle.modern:
                    Icono = Ico_Type[0,0];
                    _type = Ico_Type[0, 1];
                    break;
                case EasyUtilitario.Enumerados.MessageBox.AlertStyle.supervan:
                    Icono =  Ico_Type[1, 0];
                    _type = Ico_Type[1, 1];
                    break;
                case EasyUtilitario.Enumerados.MessageBox.AlertStyle.material:
                    Icono = Ico_Type[2, 0];
                    _type = Ico_Type[2, 1];
                    break;
                case EasyUtilitario.Enumerados.MessageBox.AlertStyle.bootstrap:
                    _type = Ico_Type[3, 1];
                    Icono = Ico_Type[3, 0];
                    break;

            }
            string _AlertType = @" $.confirm({
                                                title:'" + this.Titulo + @"',
                                                content: '" + this.Contenido + @"',
                                                icon: '" + Icono + @"',
                                             //   theme: '" + AlertStyle.ToString() + @"',
                                                closeIcon: true,
                                                animation: 'scale',
                                                type: '" + _type + @"',
                                            });";
            NombreFuncion = this.ClientID + "_AlertType()";
            ViewState["msgAct"] = NombreFuncion;
            return "function " + NombreFuncion + "{" + _AlertType + "}";

        }
        string Alert() {
            string _Alert = @"$.alert({
                                        title: '" + this.Titulo + @"',
                                        content: '" + this.Contenido + @"',
                                    });";
            NombreFuncion = this.ClientID + "_Alert()";
            ViewState["msgAct"] = NombreFuncion;
            return "function " + NombreFuncion  + "{" + _Alert + "}";
        }

        string Confirm()
        {
            string strClassBtn = (((this.btnClass != null) && (this.btnClass.Length > 0)) ? this.btnClass : "btn-blue");
            string strKeys = (((this.KeysAction != null) && (this.KeysAction.Length > 0)) ? this.KeysAction : "'enter', 'shift'");

           

            string _Confirm = @" $.confirm({
                                             title: '" + this.Titulo + @"',
                                             content: '" + this.Contenido + @"',
                                             icon: 'fa fa-question-circle',
                                             animation: 'scale',
                                             closeAnimation: 'scale',
                                             opacity: 0.5,
                                             buttons: {
                                                 'confirm': {
                                                     text: 'Aceptar',
                                                     btnClass: 'btn-blue',
                                                     action: function () {
                                                        " + this.fnButtonClick + @"('btnOK');
                                                     }
                                                 },
                                                 cancel: function () {
                                                        " + this.fnButtonClick + @"('btnCancel');
                                                 },
                    
                                             }
                                         });
                            ";
            NombreFuncion = this.ClientID + "_Confirm()";
            ViewState["msgAct"] = NombreFuncion;
            return "function " + NombreFuncion + "{" + _Confirm + "}";
        }
        string ConfirmError()
        {
            string strClassBtn = (((this.btnClass != null) && (this.btnClass.Length > 0)) ? this.btnClass : "btn-blue");
            string strKeys = (((this.KeysAction != null) && (this.KeysAction.Length > 0)) ? this.KeysAction : "'enter', 'shift'");

            string _Confirm = @"$.confirm({
                                            title: '" + this.Titulo + @"',
                                            content: '" + this.Contenido + @"',
                                            type: 'red',
                                            typeAnimated: true,
                                            buttons: {
                                                tryAgain: {
                                                    text: 'Aceptar',
                                                    btnClass: 'btn-red',
                                                    action: function(){
                                                     " + this.fnButtonClick + @"('btnOK');
        }
                                                },
                                                close: function () {
                                                    " + this.fnButtonClick + @"('btnClose');
                                                }
                                            }
                                        });";
            NombreFuncion = this.ClientID + "_ConfirmError()";
            ViewState["msgAct"] = NombreFuncion;
            return "function " + NombreFuncion + "{" + _Confirm + "}";
        }

        string Prompt()
        {
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string strClassBtn = (((this.btnClass!=null)&&(this.btnClass.Length > 0)) ? this.btnClass : "btn-blue");
            string strKeys = (((this.KeysAction!=null) &&(this.KeysAction.Length > 0)) ? this.KeysAction : "'enter', 'shift'");

            string _Prompt = @"$.confirm({title: '" + this.Titulo + @"',
                                            content: '' +
                                            '<form action=" + cmll  + " " + cmll + " class=" + cmll +"formName" + cmll + @">' +
                                            '<div class="+ cmll + "form-group" + cmll + @">' +
                                            '<label>" + this.Contenido + @"</label>' +
                                            '<input type=" + cmll + "text" + cmll +" placeholder=" + cmll + PromptEtiqueta + cmll + " class=" + cmll  + this.ClientID + "_ValInput form-control" + cmll + @" required />' +
                                            '</div>' +
                                            '</form>',
                                            buttons:
                                                    {
                                                    formSubmit:
                                                        {
                                                        text: 'Aceptar',
                                                        btnClass: '" + strClassBtn + @"',
                                                        action: function() {
                                                                var valInput = this.$content.find('." + this.ClientID + @"_ValInput').val();
                                                                    if(!valInput){
                                                                        $.alert('No se ha ingresado valor requerido');
                                                                        return false;
                                                                    }

                                                                return " + this.fnButtonClick + @"('btnOK',valInput);
                                                            }
                                                        },
                                                        cancel: function() {
                                                            " + this.fnButtonClick + @"('btnCancel','');
                                                        },
                                            },
                                            onContentReady: function() {
                                                        // bind to events
                                                        var jc = this;
                                                        this.$content.find('form').on('submit', function(e) {
                                                            // if the user submits the form by pressing enter in the field.
                                                            e.preventDefault();
                                                            jc.$$formSubmit.trigger('click'); // reference the button and click it
                                                        });
                                                    }
                                                });";
            NombreFuncion = this.ClientID + "_Prompt()";
            ViewState["msgAct"] = NombreFuncion;
            return "function " + NombreFuncion + "{" + _Prompt + "}";
        }

        #endregion


        #region Eventos internos
            protected override void OnInit(EventArgs e)
            {
                if (this.DesignMode == true) { }
                //Page.GetPostBackEventReference(this, "MenuPopupPostBack");//Genera PostBack
            }
            protected override void CreateChildControls()
            {

            }
            public string getExecute() {
                NombreFuncion = this.ClientID + "_" + this.Tipo + "()";
                return "<script>\n" + NombreFuncion + ";\n</script>";
            }
            protected override void Render(HtmlTextWriter writer)
            {
                switch (this.Tipo) {
                    case EasyUtilitario.Enumerados.MessageBox.Tipo.Alert:
                        (new LiteralControl("<script>\n" + Confirm() + "\n</script>")).RenderControl(writer);
                        break;
                    case EasyUtilitario.Enumerados.MessageBox.Tipo.Prompt:
                        (new LiteralControl("<script>\n" + Prompt() + "\n</script>")).RenderControl(writer);
                        break;
                    case EasyUtilitario.Enumerados.MessageBox.Tipo.Dialogs:
                        (new LiteralControl("<script>\n" + ConfirmError() + "\n</script>")).RenderControl(writer);
                        break;
                    case EasyUtilitario.Enumerados.MessageBox.Tipo.AlertType:
                        (new LiteralControl("<script>\n" + AlertType( this.AlertStyle) + "\n</script>")).RenderControl(writer);
                        break;
                    case EasyUtilitario.Enumerados.MessageBox.Tipo.Confirm:
                        (new LiteralControl("<script>\n" + Confirm() + "\n</script>")).RenderControl(writer);
                        break;

                }
            new LiteralControl(getExecute()).RenderControl(writer);
            }
        #endregion

        #region Eventos Externos
        #endregion


    }
}
