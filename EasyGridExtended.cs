using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web; 
using System.Web.UI;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;
using static EasyControlWeb.Form.Editor.EasyGridCollectionsEditor;

namespace EasyControlWeb
{

   [
        AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("IdGestorFiltro"),
        ParseChildren(true, "IdGestorFiltro"),
        ToolboxData("<{0}:EasyGridExtended runat=server></{0}:EasyGridExtended>")
    ]
    
    [TypeConverter(typeof(Type_EasyGridViewExtend))]
    [Serializable]
    public class EasyGridExtended
    {
        string itemColorMouseMove;
        string itemColorSeleccionado;
        string rowItemClick;
        string idGestorFiltro;

        string rowCellItemClick;

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string ItemColorMouseMove { 
            get {
                    if (itemColorMouseMove == null) {
                        itemColorMouseMove = "#CDE6F7";
                    }
                return itemColorMouseMove; 
            }   
            set { itemColorMouseMove = value; } 
          }

        
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string ItemColorSeleccionado { 
            get {
                if (itemColorSeleccionado == null) {
                    itemColorSeleccionado = "#ffcc66";
                }
                    return itemColorSeleccionado; 
                } 
            set { 
                itemColorSeleccionado = value; 
            } 
         }

       
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string RowItemClick { 
            get { if (rowItemClick == null) {
                    rowItemClick = "OnEasyGridDetalle_Click";
            }
                return rowItemClick; } 
            set { rowItemClick = value; } 
        }



        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string RowCellItemClick
        {
            get
            {
               /* if (rowCellItemClick == null)
                {
                    rowCellItemClick = "OnEasyGridRowCell_Click";
                }*/
                return rowCellItemClick;
            }
            set { rowCellItemClick = value; }
        }



        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string IdGestorFiltro { get { return idGestorFiltro; } set { idGestorFiltro = value; } }


        public EasyGridExtended() { }
        public EasyGridExtended(string _ItemColorMouseMove,string _ItemColorSeleccionado,string _RowItemClick,string _IdGestorFiltro) {
            this.ItemColorMouseMove = _ItemColorMouseMove;
            this.ItemColorSeleccionado = _ItemColorSeleccionado;
            this.RowItemClick = _RowItemClick;
            this.IdGestorFiltro = _IdGestorFiltro;
        }
        public EasyGridExtended(string _ItemColorMouseMove, string _ItemColorSeleccionado, string _RowItemClick)
        {
            this.ItemColorMouseMove = _ItemColorMouseMove;
            this.ItemColorSeleccionado = _ItemColorSeleccionado;
            this.RowItemClick = _RowItemClick;
        }
    }
}
