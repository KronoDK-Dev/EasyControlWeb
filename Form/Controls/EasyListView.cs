using EasyControlWeb.Form.Templates.ListView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Controls 
{

    public enum TipoItemView
    {
        Simple,
        ImagenCircular,
        hyperlink,
    }

    [
   /* AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),*/
    DefaultProperty("ListItems"),
    ParseChildren(true, "ListItems"),
    ToolboxData("<{0}:EasyListView runat=server></{0}:EasyListView>")
    ]
    [Serializable]
    public class EasyListView : CompositeControl
    {
    
        //Eventos externos

        
        public delegate void onItem();
        
        #region Variables
        HtmlGenericControl ListGroup;
            List<LiteralControl> ScriptCollection = new List<LiteralControl>();
            TextBox txtLstItems;
            HtmlButton btnItemClick;
        #endregion
        #region Eventos
            public delegate void LinkView(EasyListItem oEasyListItem);
            public event LinkView ViewItem;

        #endregion

        private string className;
        [Browsable(true)]
        public string ClassName {
            get { return className; }
            set { className = value; }
        }

        private string ancho;
        [Browsable(true)]
        public string Ancho { get { return this.ancho; } set { this.ancho = value; } }

        private string fncItemOnCLick;
        [Browsable(true)]
        public string FncItemOnCLick {
            get { return fncItemOnCLick; }
            set { fncItemOnCLick = value; }
        }


        private string fncItemOnMouseMove;
        [Browsable(true)]
        public string FncItemOnMouseMove
        {
            get { return fncItemOnMouseMove; }
            set { fncItemOnMouseMove = value; }
        }


        EasyUtilitario.Enumerados.Ubicacion textAlign;
        [Browsable(true)]
        public EasyUtilitario.Enumerados.Ubicacion TextAlign {
            get { return textAlign; }
            set { textAlign = value;}
        }

        private TipoItemView tipoItem;

        [Browsable(true)]
        public TipoItemView TipoItem {
            get { return tipoItem; }
            set { this.tipoItem = value; }
        }

        List<EasyListItem> arrEasyListItems;
        [Browsable(true)]
        [
           Category("Behavior"),
           Description("Colleccion de Items"),
           DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
           Editor(typeof(EasyControlCollection.EasyFormCollectionListViewEditor), typeof(UITypeEditor)),
           PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public List<EasyListItem> ListItems
        {
            get { return arrEasyListItems; }
        }



        
        [Browsable(true)]
        public string AlertTitulo { get; set; }
        [Browsable(true)]
        public string AlertMensaje { get; set; }
        [Browsable(true)]
        public Dictionary<string, string> DataComplete = new Dictionary<string, string>();


        [Browsable(true)]
        public bool RunAtServer { get; set; }




        public EasyListView()
        {
            arrEasyListItems = new List<EasyListItem>();
        }




        #region Funciones Propias

        Dictionary<string, string> _DicScript = new Dictionary<string, string>();
        //if(!_DicScript.ContainsKey(NomFuncion)){
        //          _DicScript.Add(NomFuncion, "");
        //          ScriptCollection.Add(new LiteralControl(msgbox));
        //      }
      
        void CrearListGrup()
        {
            //Crear los datos adicionales del grupo
            string Datos = ""; int point = 0;
                ListGroup.Attributes["className"] = ((this.TipoItem == TipoItemView.Simple)?"list-group" :"d-flex mb-5");
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>();
                foreach (KeyValuePair<string, string> dc in this.DataComplete)
                {
                    kvp = dc;
                    Datos += ((point == 0) ? "" : ",") + dc.Key + ":'" + dc.Value + "'";
                    point++;
                }

                ListGroup.Attributes["DataComplete"] = "{" + Datos + "}";

            #region Script Embebidos
            //Crea el script para la administracion del listview
            string script_Object = @"var " + this.ClientID + "= jNet.get('" + ListGroup.ClientID + @"');
                                                " + this.ClientID + @".DataComplete={"+ Datos + @"};
                                                " + this.ClientID + @".Collection = new Array();
                                                " + this.ClientID + @".Collection.Find=function(ItemView){
                                                                                            this.forEach(function(ItemArray,i){
                                                                                                   if(ItemArray.ItemV.Text == ItemView.Text){
                                                                                                        return ItemArray.ItemV;
                                                                                                   }
                                                                                            })
                                                                                            return null;
                                                                                       } 
                                                " + this.ClientID + @".Collection.Add=function(ItemView){
                                                                                            var ItemViewFind = this.Find(ItemView);
                                                                                            if(ItemViewFind==null){
                                                                                                var htmlItemView =  " + this.ClientID + @".HtmlItemView(ItemView);
                                                                                                " + this.ClientID + @".Collection.AddInServer(htmlItemView.id,ItemView);
                                                                                                " + this.ClientID + @".insert(htmlItemView);
                                                                                            }
                                                                                      };
                                                 " + this.ClientID + @".Collection.EnumeraItems=function(){
                                                                                                var strLst='';
                                                                                                this.forEach(function(oitemArray,i){
                                                                                                    strLst += ((i==0)?'':'" + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal + @"') +  " + this.ClientID + @".Serialized(oitemArray.ItemV);
                                                                                                });
                                                                                                jNet.get('" + txtLstItems.ClientID + @"').attr('value',strLst);                                                
                                                 }
                                                 " + this.ClientID + @".Collection.AddInServer=function(IdItem,ItemView){
                                                                                                this[this.length] = new Array();
                                                                                                this[this.length-1] = {IdHtml:IdItem,ItemV:ItemView};
                                                                                               " + this.ClientID + @".Collection.EnumeraItems();
                                                                                      };
                                                " + this.ClientID + @".Collection.Eliminar=function(IdHtmlItem){
                                                                                                var Me=this;
                                                                                                var ConfigMsgb={Titulo:'" + AlertTitulo + @"'
                                                                                                                ,Descripcion: '" + AlertMensaje + @"'
                                                                                                                ,Icono: 'fa fa-question-circle'
                                                                                                                ,EventHandle: function(btn){
                                                                                                                        if(btn=='OK'){
                                                                                                                                Me.forEach(function(ItemArray,i){
                                                                                                                                    if(ItemArray.IdHtml == IdHtmlItem){
                                                                                                                                        var result = " + FncItemOnCLick + @"('Delete'," + this.ClientID + @",ItemArray.ItemV);
                                                                                                                                         if (result){
                                                                                                                                            //Me.splice(i, 1); //Borra de una pocicion espcifica un elemento del array
                                                                                                                                            ItemArray.ItemV.IdEstado=0;
                                                                                                                                            var tblItem = jNet.get(IdHtmlItem);
                                                                                                                                            var CellB = jNet.get(tblItem.rows[0].cells[1]);
                                                                                                                                            CellB.css('color','red');
                                                                                                                                            CellB.css('text-decoration','line-through');
                                                                                                                                            " + this.ClientID + @".Collection.EnumeraItems();
                                                                                                                                            /* var obj = document.getElementById(IdHtmlItem);
                                                                                                                                            " + this.ClientID + ".remove(obj);" + @"*/
                                                                                                                                        }
                                                                                                                                         return;
                                                                                                                                    }
                                                                                                                                });
                                                                                                                        }
                                                                                                                    }
                                                                                                                };
                                                                                                                var oMsg = new SIMA.MessageBox(ConfigMsgb);
                                                                                                                oMsg.confirm();

                                                                                                };

                                                 " + this.ClientID + @".HtmlItemView=function(ItemView){
                                                                                         var tblItem = jNet.get(SIMA.Utilitario.Helper.HtmlControlsDesign.HtmlTable(1, 3));
                                                                                             tblItem.id = '" + this.ClientID + @"_Item' + ItemView.Key;
                                                                                             tblItem.className = '" + this.ClassName + @"';
                                                                                             var CellA = jNet.get(tblItem.rows[0].cells[0]);
                                                                                             CellA.innerHTML = this.Collection.length+1;
                                                                                             CellA.css('width','3%');
                                                                                             CellA.css('text-decoration','underline');
                                                                                             //CellA.style.cssText = 'text-decoration:underline';
                                                                                             CellA.addEvent('click', function(){
                                                                                                                        " + FncItemOnCLick + @"('Open'," + this.ClientID + @",ItemView);
                                                                                                                    });

                                                                                             var CellB =jNet.get(tblItem.rows[0].cells[1]);
                                                                                             CellB.innerHTML = ItemView.Text;
                                                                                             CellB.css('width','90%');

                                                                                             if((ItemView.Url!=undefined)&&(ItemView.Url.length>0)){
                                                                                                 CellB.css('color','blue');
                                                                                                 CellB.css('text-decoration','underline');
                                                                                                 CellB.addEvent('click', function(){
                                                                                                                                        " + FncItemOnCLick + @"('Link'," + this.ClientID + @",ItemView);
                                                                                                                                    });
                                                                                             }

                                                                                             var  oimg = jNet.get(document.createElement('IMG')); 
                                                                                             oimg.src =  '" + EasyUtilitario.Constantes.ImgDataURL.IconDeItem + @"';
                                                                                             oimg.css('width','17px');
                                                                                             oimg.addEvent('click', function(){
                                                                                                                        " + this.ClientID + @".Collection.Eliminar(tblItem.id);
                                                                                                                    });

                                                                                             var CellC = jNet.get(tblItem.rows[0].cells[2]);
                                                                                             CellC.insert(oimg);
                                                                                             CellC.attr('VAlign','center');
                                                                                             CellC.attr('HAlign','center');
                                                                                             CellC.css('width','5%');

                                                                                             tblItem.css('width','100%');
                                                                                             tblItem.css('margin','3px');

                                                                                            return tblItem;
                                                                                    }

                                              " + this.ClientID + @".Serialized=function(ItemView){
                                                                                        var cmll=" + EasyUtilitario.Constantes.Caracteres.ComillaDoble + @"\" + EasyUtilitario.Constantes.Caracteres.ComillaDoble + EasyUtilitario.Constantes.Caracteres.ComillaDoble + @";
                                                                                        var strEntity='';p=0;
                                                                                        for (var [key, value] of Object.entries(ItemView)){
                                                                                            strEntity += ((p==0)?'':',') +  key + ':' + cmll + value + cmll;
                                                                                            p++;
                                                                                        }
                                                                                        strEntity ='{' + strEntity + '}';
                                                                                        
                                                                                        return strEntity;
                                                                                    }
                                            " + this.ClientID + @".FindKey=function(ItemView){
                                                                                var oItemExist = null;
                                                                                var oSource = this;
                                                                                oSource.forEach(function(oItemFind,i){
                                                                                                        var ItemData;
                                                                                                        eval('ItemData = ' + oItemFind.attr('ItemData'));
                                                                                                        if(ItemData.Key == ItemView.Key){
                                                                                                            oItemExist  = oItemFind;
                                                                                                            return oItemExist;
                                                                                                        }
                                                                                                    }
                                                                                                );
                                                                        return oItemExist;
                                                                } 

                                        ";
                ScriptCollection.Add(new LiteralControl("\n<script>" + script_Object + " </script>"));
            #endregion


           
            string script_ListItem = "";
                int i = 0;
                if (this.arrEasyListItems != null)
                {
                    foreach (EasyListViewItemBase oEasyListItem in this.arrEasyListItems)
                    {
                        oEasyListItem.IdEstado = 1;
                        oEasyListItem.Key = i.ToString();
                        if (this.TipoItem == TipoItemView.Simple)
                        {
                            HtmlTable tblItem = ItemSimple(i, oEasyListItem);
                            ListGroup.Controls.Add(tblItem);
                            HtmlImage oimg = EasyUtilitario.Helper.HtmlControlsDesign.CrearImagen(EasyUtilitario.Constantes.ImgDataURL.IconDeItem);
                        
                            string MetodoDEL = "" + this.ClientID + @".Collection.Eliminar('" + tblItem.ClientID + "');";
                            oimg.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = MetodoDEL;
                           

                            oimg.Style.Add("width", "17px");
                            //oimg.Style.Add("class", "btnCircle");
                        tblItem.Rows[0].Cells[2].Controls.Add(oimg);

                            script_ListItem += "\n " + this.ClientID + @".Collection.AddInServer('" + tblItem.ClientID + "'," + oEasyListItem.ToString(true) + @");";//agrega al array de datos
                        }
                        else if (this.TipoItem == TipoItemView.ImagenCircular)
                        {
                            HtmlImage oImgItem = ItemImagenCircular(i, oEasyListItem);
                            //Pregunta si posee mensajes
                            if (oEasyListItem.NroMsg > 0) {
                                HtmlGenericControl dvBadge = new HtmlGenericControl("div");
                                dvBadge.Style.Add("display","inline-block");
                                dvBadge.Attributes.Add("width", "50px");
                                HtmlGenericControl Badge = new HtmlGenericControl("span");
                                Badge.Attributes.Add("class", "badge");
                                Badge.InnerText= oEasyListItem.NroMsg.ToString();
                                dvBadge.Controls.Add(Badge);
                                dvBadge.Controls.Add(oImgItem);
                                ListGroup.Controls.Add(dvBadge);
                            }
                            else {
                                ListGroup.Controls.Add(oImgItem);
                            }
                        
                            script_ListItem += "\n " + this.ClientID + @".Collection.AddInServer('" + oImgItem.ClientID + "'," + oEasyListItem.ToString(true) + @");";//agrega al array de datos
                        }
                        i++;
                    }
                   /* if ((this.TipoItem == TipoItemView.ImagenCircular )&&(i>1))
                    {
                        HtmlGenericControl htmlCount = ItemImagenCount(i);
                        ListGroup.Controls.Add(htmlCount);
                    }*/
                    ScriptCollection.Add(new LiteralControl("\n<script>setTimeout(function(){ " + script_ListItem + "\n},1000);</script>"));
                }
        }


        public HtmlTable ItemSimple(int Nro, EasyListViewItemBase oEasyListItemBase)
        {
            EasyListItem oEasyListItem = (EasyListItem)oEasyListItemBase;
            HtmlTable tbl = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(1, 3, this.ClassName);
            tbl.Attributes["ItemData"] = oEasyListItem.ToString(true);
            tbl.ID = "Item" + oEasyListItem.Key.ToString();
            //tbl.Border = 2;
            tbl.Rows[0].Cells[0].InnerText = (Nro+1).ToString();
            tbl.Rows[0].Cells[0].Style["width"] = "3%";
            if (RunAtServer) {
                    tbl.Rows[0].Cells[0].Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = "javascript:__doPostBack('" + this.btnItemClick.ClientID + "', 'OPEN" + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal + oEasyListItem.ToString(true) + "')";
            }
            else
            {
                tbl.Rows[0].Cells[0].Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = "javascript:" + FncItemOnCLick + "('Open', jNet.get('" + ListGroup.ClientID + "'), " + oEasyListItem.ToString(true) + ");";
            }
            tbl.Rows[0].Cells[0].Style["TEXT-DECORATION"] = "underline";


            tbl.Rows[0].Cells[1].InnerText = oEasyListItem.Text;
            tbl.Rows[0].Cells[1].Align = ((TextAlign == EasyUtilitario.Enumerados.Ubicacion.Izquierda) ? "left" : ((TextAlign == EasyUtilitario.Enumerados.Ubicacion.Derecha)? "right" : "center"));
            tbl.Rows[0].Cells[1].Style["width"] = "90%";
            if (oEasyListItem.Url!=null) {
                tbl.Rows[0].Cells[1].Style["TEXT-DECORATION"] = "underline";
                tbl.Rows[0].Cells[1].Style["color"] = "blue";
                tbl.Rows[0].Cells[1].Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = "javascript:" + FncItemOnCLick + "('Link', jNet.get('" + ListGroup.ClientID + "'), " + oEasyListItem.ToString(true) + ");";                                                                                                                
            }
           
            tbl.Rows[0].Cells[2].Style["width"] = "5%";
            tbl.Rows[0].Cells[2].Attributes["VAlign"] = "center";
            tbl.Rows[0].Cells[2].Attributes["HAlign"] = "center";
            tbl.Style["width"] = this.Ancho;
            tbl.Style["margin"] = "3px";

            return tbl;
        }
        public HtmlImage ItemImagenCircular(int Nro, EasyListViewItemBase oEasyListItemBase) {
            EasyListItem oEasyListItem = (EasyListItem)oEasyListItemBase;
            // HtmlImage oImgItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearImagen( this.PathFotos + oEasyListItem.Src, "ms-n2 rounded-circle img-fluid");
            HtmlImage oImgItem = EasyUtilitario.Helper.HtmlControlsDesign.CrearImagen(oEasyListItem.Src, "ms-n2 rounded-circle img-fluid");
            oImgItem.Attributes["onerror"] = "this.onerror=null;this.src=SIMA.Utilitario.Constantes.ImgDataURL.ImgSF;";
            oImgItem.Attributes["style"] = "width:32px; height: 32px; object-fit: cover;";
            oImgItem.Attributes["ItemData"] = oEasyListItem.ToString(true);
          
            oImgItem.ID = "Item" + oEasyListItem.Key.ToString();
            if (RunAtServer)
            {
                oImgItem.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = "javascript:__doPostBack('" + this.btnItemClick.ClientID + "', 'OPEN" + EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal + oEasyListItem.ToString(true) + "')";
            }
            else
            {
                oImgItem.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = "javascript:" + FncItemOnCLick + "('Open', jNet.get('" + ListGroup.ClientID + "'), " + oEasyListItem.ToString(true) + ");";
                oImgItem.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onmousemove.ToString()] = "javascript:" + fncItemOnMouseMove + "('" + this.ClientID + "',jNet.get('" + ListGroup.ClientID + "'), " + oEasyListItem.ToString(true) + ");";
            }

            return oImgItem;
        }

        public HtmlGenericControl ItemImagenCount(int Count) {
            HtmlGenericControl htmlCount = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "ms-n2 d-inline-flex justify-content-center align-items-center text-primary-dark  rounded-circle");
            htmlCount.Attributes["style"] = "width:32px; height: 32px; font-size: 12px;background-color:WhiteSmoke";
            htmlCount.InnerText = Count.ToString(); 
            return htmlCount;
        }   

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //this.CreateChildControls();
            //this.ChildControlsCreated = true;//vuelve a llamar  al metodo  this.CreateChildControls()
            Page.GetPostBackEventReference(this, "ListViewPost");//Genera PostBack
                                                                 
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            ListGroup = new HtmlGenericControl("div");
            ListGroup.ID = "LstGrp";
            this.Controls.Add(ListGroup);

            txtLstItems = new TextBox();
            txtLstItems.ID = "lstItems";
            txtLstItems.Style["display"] = "none";
            this.Controls.Add(txtLstItems);


            if (RunAtServer)
            {
                //Boton postback
                btnItemClick = new HtmlButton();
                btnItemClick.ID = "btnItemEvent";
                btnItemClick.Attributes.Add("runat", "server");
                btnItemClick.InnerText = "ItemView";
                //btnItemClick.Style.Add("display", "none");
                btnItemClick.ServerClick += EasyLisItem_Click;

                this.Controls.Add(btnItemClick);
            }

            //Crear los items
           CrearListGrup();
        }
        protected override void Render(HtmlTextWriter writer)
        {
           base.Render(writer);
            if (IsDesign()){
                (new LiteralControl("ListView")).RenderControl(writer);
            }
           // CrearListGrup();

            foreach (LiteralControl lc in ScriptCollection) {
                lc.RenderControl(writer);
            }

        }

        #endregion

        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        public string getListItems() {
            return ((txtLstItems==null)?"": txtLstItems.Text);
        }
      


        protected virtual void EasyLisItem_Click(object sender, EventArgs e)
        {
            string Argument = ((System.Web.UI.Page)HttpContext.Current.Handler).Request["__EVENTARGUMENT"];

            EasyListItem oEasyListItem = new EasyListItem();
            ViewItem?.Invoke(oEasyListItem);
        }
     

    }
}
