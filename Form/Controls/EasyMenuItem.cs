using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Form.Controls
{
    public class EasyMenuItem 
    { 
        public enum MenuIcon
        {
            Not, 
            cut,
            edit,
            copy,
            paste,
            delete
        }
        public enum MenuTipoItem
        {
            Item,
            Separador
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public MenuIcon Icono { get; set; }
        public bool Disabled { get; set; }
        public MenuTipoItem Tipo { get; set; }
        public string AccessKey { get; set; }
        public bool CallclickServer { get; set; }
        public override string ToString()
        {
            return "EasyMenuItem";
        }
        public string ToString(bool Separador)
        {
            string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string Item;
            if (this.Tipo==MenuTipoItem.Separador)
            {
                Item = Id + ":" + cmll + ((this.Text == string.Empty)?"---------":this.Text) + cmll;
            }
            else
            {
                Item = Id + ":{name:" + cmll + Text + cmll + ", icon:" + cmll + Icono + cmll + ((AccessKey == string.Empty) ? "" : ", accesskey: " + cmll + AccessKey + cmll) + ((Disabled == false) ? ",disabled: false" : ", disabled: true") + ",CallEventServer:" + this.CallclickServer.ToString().ToLower() +  ",Id:" + cmll + this.Id + cmll +  "}";
            }
            return Item;

        }
    }
}
