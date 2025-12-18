using EasyControlWeb.Form.Estilo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Controls
{
    [Serializable] 
    public  class EasyUpLoad : EasyPopupBase
    {
     

        const string UPLOAD_PAGE = "PagProceso";
        const string IMG_UPLOAD = "ImagenUP";
        const string Pref_Colletion = "Acum";
        string UniqID = "";

        #region Controles
            HtmlButton btnPostBack2;
            HtmlButton btnPostBackDelete;
            HtmlTable tbl;
            EasyListView oListView;
            List<LiteralControl> BloqueScript = new List<LiteralControl>();
        #endregion
        string ScriptColletionBE = "var oEasyListItemBE=null;\n";

        #region Eventos
        public delegate void CompletarCarga(List<EasyFileInfo> oLstEasyFileInfo);
            public event CompletarCarga CargarArchivo;
        #endregion

        #region Propiedades

        private bool requerido;
        [Category("Validación"), Description("Activa la validacion de campos obligatorios")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public bool Requerido { get { return requerido; } set { requerido = value; } }

        [Category("Carga")]
        [Browsable(true)]
        [Description("Pagina preparada para recibir y guardar los archivos")]
        [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string PaginaProceso
        {
            get
            {
                if (this.ViewState[UPLOAD_PAGE] == null)
                {
                    this.ViewState[UPLOAD_PAGE] = "UploadMinimal.aspx";
                }
                return (string)this.ViewState[UPLOAD_PAGE];
            }
            set
            {
                this.ViewState[UPLOAD_PAGE] = value;
            }
        }

      
        [Browsable(false)]
        List<EasyFileInfo> oEasyFileInfo;

        [Editor(typeof(EasyControlCollection.EasyFormCollectionUpFilesEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contiene todos los controles necesarios en una List collection")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public List<EasyFileInfo> ListadeArchivos
        {
            get
            {
                if (oEasyFileInfo == null)
                {
                    oEasyFileInfo = new List<EasyFileInfo>();
                }
                return oEasyFileInfo;
            }
        }

       
        EasyPathLocalWeb oEasyPathLocalWeb = new EasyPathLocalWeb();
        [Category("Editor")]
        [TypeConverter(typeof(Type_CarpetaUrl))]        
        [Description("Rutas de almacenamiento y consulta Tempral y finales"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public EasyPathLocalWeb PathLocalyWeb
        {
            get{return oEasyPathLocalWeb;}
            set{oEasyPathLocalWeb = (EasyPathLocalWeb)value;}
        }



        Bootstrap oBootstrap = new Bootstrap();
        [Category("Apariencia")]
        [TypeConverter(typeof(Type_Style))]
        [Description("Define estilo vigente para cada control"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public Bootstrap EasyStyle
        {
            get { return oBootstrap; }
            set { oBootstrap = (Bootstrap)value; }
        }

        private string etiqueta;
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [Category("Varios"), Description(""), DefaultValue("")]
        public string Etiqueta
        {
            get { return etiqueta; }
            set { etiqueta = value; }
        }

        private string itemFileClass;
        [Category("ItemsFiles"), Description(""), DefaultValue("")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string ItemFileClass
        {
            get { return itemFileClass; }
            set { itemFileClass = value; }
        }

        private string _fncOnComplete;
        [Category("Varios"), Description(""), DefaultValue("")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncOnComplete
        {
            get { return _fncOnComplete; }
            set { _fncOnComplete = value; }
        }

        private string _fncListViewItemClick;
        [Category("Varios"), Description(""), DefaultValue("")]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncListViewItemClick
        {
            get { return _fncListViewItemClick; }
            set { _fncListViewItemClick = value; }
        }


        #endregion



        public EasyUpLoad() : base()
        {
        }

        public EasyUpLoad(string Formato)
        {
        }
        protected override void OnInit(EventArgs e)
        {
            UniqID = this.ClientID;
            string objInstanceUpLoad = "o" + UniqID + "_Exe";

            this.fncScriptAceptar = objInstanceUpLoad + @".Send";
            tbl = new HtmlTable();
            // tbl.ID = this.ClientID + "_tblDisplay";
            tbl.ID = UniqID + "_tblDisplay";

            base.OnInit(e);

        }
        protected override void OnPreRender(EventArgs e)
        {
        }

        string NombreUpLoad = "";

        HtmlGenericControl InputUpLoad() {
            HtmlGenericControl Label = new HtmlGenericControl("label");
            Label.Style.Add("display", "inline-block");

            Label.Style.Add("padding", "7px 15px");
            Label.Style.Add("cursor", "pointer");
            Label.Style.Add("border-radius", "5px");
            Label.Style.Add("color","#ffffff");

            FileUpload oF = new FileUpload();
            oF.Style.Add("display","none");
            oF.ID = "ToUpload";
            NombreUpLoad = oF.ClientID;
            oF.Attributes.Add("onchange", UniqID + "SelectedAll(this.id);");

            HtmlImage dvSGVUpLoad = new HtmlImage();
            dvSGVUpLoad.Src = EasyUtilitario.Constantes.ImgDataURL.ImgLoadUp;
            dvSGVUpLoad.Attributes["style"] = "width:200px";

            Label.Controls.Add(oF);
            Label.Controls.Add(dvSGVUpLoad);
            return Label;
        }

        string scriptOnCompleteInterno(){
            string objInstanceUpLoad = "o" + UniqID + "_Exe";
            //string fncComplete = ((this.fncOnComplete != null) ? objInstanceUpLoad + @".fncComplete = " + this.fncOnComplete + ";" : "");
            string fncComplete = ((this.fncOnComplete != null)&& (this.fncOnComplete.Length >0) ? this.fncOnComplete + "("+ this.ClientID + ",oCollectionsFile);" : this.ClientID + ".Close();");

            string script = UniqID + @"_ListViewItemPaint=function(oCollectionsFile){
                                                                        var Listview= " + oListView.ClientID + @";
                                                                        var Nro = 0;
                                                                        oCollectionsFile.forEach(function(ItemFile,i){
                                                                               var NomFile=ItemFile.Nombre;
                                                                               Nro =Listview.Collection.length + 1;
                                                                               var LItem = {Src:'',Key:Nro,Text:NomFile,Value:Nro,Url:'" + this.PathLocalyWeb.UrlTemporal + @"'+ NomFile,IdEstado:'1',DataComplete:''};
                                                                               Listview.Collection.Add(LItem);

                                                                               //Listview.Collection.Add({Text: NomFile, Key:Nro, Value:Nro,Url:'" + this.PathLocalyWeb.UrlTemporal + @"'+ NomFile,IdEstado:1, DataComplete:'' });
                                                                        });
                                                                        " + fncComplete + @"
                                                                    }";
            return script;
        }
        string scriptDeclare() {
            try
            {
                string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
                char bs = EasyUtilitario.Constantes.Caracteres.BackSlash;
                string PaginayParams = this.PaginaProceso + "?PathLocal=@" + this.PathLocalyWeb.CarpetaTemporal.Replace(bs, '.');
                string objInstanceUpLoad = "o" + UniqID + "_Exe";
                string strDeclare = @"var " + objInstanceUpLoad + @" = new EasyUpLoad();
                                    " + objInstanceUpLoad + @".PaginaProceso = " + cmll + PaginayParams + cmll + @";
                                    " + objInstanceUpLoad + @".CtrlContext = " + cmll + UniqID + "_body" + cmll + @";
                                    " + objInstanceUpLoad + @".fncComplete =" + UniqID + @"_ListViewItemPaint;";
                return strDeclare;

            }
            catch (Exception ex) {
                return "alert('UpLoad:' +'" + this.ClientID + "'   + '" + " No se ha definido valor a la propiedad [RutaLocalTmp]" + "');";
            }
        }

        string ScriptSendFileInfo()
        {
            string strFnc = @"function " + UniqID +"_SendInfo" + @"(strFileInfo){
                    
                __doPostBack('" + this.UniqueID + @"$btnActionSend',strFileInfo);
                            }";
            return strFnc;
        }

        
        string ScriptSelected() {
            string objInstanceUpLoad = "o" + UniqID + "_Exe";
            string script = @" var nomcltrl ='';
                                function " + UniqID + @"SelectedAll(UploadId) {
                                var objInputFile =jNet.get(UploadId);
                                //Traslada el conrol listview a otra ubicacion
                                var file = objInputFile.files[0];
                                if(file){
                                    var _add = true;
                                    var oIemBE = new EasyUploadFileBE(file,'" + UniqID +  @"');
                                    if (" + objInstanceUpLoad + @".FileCollections.length > 0) {
                                        " + objInstanceUpLoad + @".FileCollections.forEach(function (_ItemBE,i) {
                                                                                if (_ItemBE.Nombre == oIemBE.Nombre) {
                                                                                    _add = false;
                                                                                }
                                                                            });
                                        if (_add == true){
                                            oIemBE.Render('" + UniqID + @"_body');
                                            " + objInstanceUpLoad + @".FileCollections.Add(oIemBE);
                                        }
                                    }
                                    else {
                                        oIemBE.Render('" + UniqID + @"_body');
                                        " + objInstanceUpLoad + @".FileCollections.Add(oIemBE);
                                    }
                                }
                                objInputFile.value='';
                            }
                            ";
            return script;
        }
        
        HtmlTable tblScreen() {
            HtmlButton btn = new HtmlButton();
            btn.InnerText = "...";
            btn.Attributes.Add("type", "button");
            btn.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), UniqID + ".Show({backdrop: true});"); 

            tbl.Attributes.Add("border", "0px");
            //tbl.Attributes.Add("style", "width: 200px;");
            tbl.Attributes.Add("style",  this.Width.ToString());

            HtmlTableRow tblrow = new HtmlTableRow();
            
            HtmlTableCell tblcell = new HtmlTableCell();
            tblcell.Style.Add("Width", "90%");
            //tblcell.Controls.Add(PaintItemFile());
           

            tblrow.Controls.Add(tblcell);

            tblcell = new HtmlTableCell();
            tblcell.Attributes.Add("valign", "top");
            
            tblcell.Controls.Add(btn);
            tblcell.Style.Add("padding-left","10px");
            tblrow.Controls.Add(tblcell);

            tbl.Controls.Add(tblrow);
            tbl.Width = this.Width.ToString(); 
            return tbl;
        }
        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            HtmlGenericControl CustomFile = new HtmlGenericControl("div");
            CustomFile.Attributes.Add("class", "custom-file");
            CustomFile.Attributes.Add("align", "center");


            CustomFile.Controls.Add(InputUpLoad());
            //Crea la fila para contener los tag
            HtmlGenericControl _row = new HtmlGenericControl("div");
                _row.Attributes.Add("class", "row");
                _row.Style.Add("padding-left","10px");
            HtmlGenericControl _Col = new HtmlGenericControl("div");
                _Col.Attributes.Add("class", "col-lg-8 col-sm-6 pb-3");
                _row.Controls.Add(_Col);

            this.Controls.Add(CustomFile);

            CustomFile.Controls.Add(_row);

            btnPostBack2 = new HtmlButton();
            btnPostBack2.ID = "btnActionSend";
            btnPostBack2.InnerText = ".";
            btnPostBack2.ServerClick += new System.EventHandler(EasyUpLoadButton_Click);
            btnPostBack2.Style.Add("display", "none");
            this.Controls.Add(btnPostBack2);

            btnPostBackDelete = new HtmlButton(); ;
            //btnPostBackDelete.Attributes.Add("runat", "server");
            btnPostBackDelete.ID = "btnActionDelete";
            btnPostBackDelete.InnerText = ".";
            btnPostBackDelete.ServerClick += new System.EventHandler(EasyDeleteItem_Click);
            btnPostBackDelete.Style.Add("display", "none");
            this.Controls.Add(btnPostBackDelete);
           

            /*Trabajos con el ListView*/
            oListView = new EasyListView();
            oListView.ID = "LFiles";
            oListView.TipoItem = TipoItemView.Simple;
            oListView.DataComplete.Add("Principal", "1");
            oListView.AlertTitulo = "Subir Archivos";
            oListView.AlertMensaje = "Desea eliminar el Item de archivo seleccionado ahora?";
            oListView.ClassName = this.ItemFileClass; //"BaseItem";
            oListView.Ancho = "100%";
            oListView.FncItemOnCLick = this.fncListViewItemClick; 
            oListView.TextAlign = EasyUtilitario.Enumerados.Ubicacion.Izquierda;


            if ((this.ListadeArchivos != null) && (this.ListadeArchivos.Count()>0)){
                foreach (EasyFileInfo oEasyFileInfo in this.ListadeArchivos) {
                    EasyListItem oEasyListItem = new EasyListItem();
                    oEasyListItem.Value = oEasyFileInfo.IdFile;
                    oEasyListItem.Text = oEasyFileInfo.Nombre;
                    oEasyListItem.IdEstado = oEasyFileInfo.IdEstado;
                    oEasyListItem.IdEstado = oEasyFileInfo.IdEstado;
                    oEasyListItem.Url = this.PathLocalyWeb.UrlFinal + oEasyFileInfo.Nombre;
                    oListView.ListItems.Add(oEasyListItem);
                }
            }
            this.Controls.Add(oListView);



            /*Utiliza la creacion del Listview*/
            string MoverListView = @"<script>
                                        try{
                                            var LViewGroup  = '" + oListView.ClientID + @"';
                                            var arrNom = LViewGroup.split('_');
                                            var Nro = arrNom[arrNom.length-1];
                                            var NomLGRP = '';
                                            if(Nro.isNumeric()){
                                                NomLGRP = LViewGroup + '_LstGrp_' + Nro;
                                            }
                                            else{
                                                NomLGRP =LViewGroup + '_LstGrp';
                                            }
                                             var " + oListView.ClientID + @"View = jNet.get(NomLGRP);
                                             var tblContext = jNet.get('" + this.ClientID + @"_tblDisplay');
                                             jNet.get(tblContext.rows[0].cells[0]).insert(" + oListView.ClientID + @"View);
                                         }
                                         catch(err){
                                            alert(err);
                                         }
                                     </script>
                                    ";

            BloqueScript.Add(new LiteralControl(MoverListView));
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (!IsDesign())
            {
                base.RenderContents(output);
               
                //this.Controls.Add(oListView);

            }
        }

        string fcScriptDeleteFile() {
            string _script = @"<script>
                                function " + this.ClientID + @"_FileAction(oItemFile,Action){
                                     __doPostBack('" + this.UniqueID + @"$btnActionDelete',oItemFile.Nombre);
                              }
                              </script>";
            return _script;
        } 
        HtmlGenericControl PaintItemFile() {
            HtmlGenericControl dvContext =EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div");
            List<EasyFileInfo> allEasyFileInfo = (List<EasyFileInfo>)getAllFiles();
            if (allEasyFileInfo != null)
            {
                foreach (EasyFileInfo oEasyFileInfo in allEasyFileInfo.Where(oBE => oBE.IdEstado != 0))
                {
                    HtmlTable tblHead = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(1, 2);
                    tblHead.Style.Add("width", "100%");
                    tblHead.Attributes.Add("border", "0px");


                    string CardID = this.UniqID + "_Card_" + oEasyFileInfo.Nombre.Replace(",", "").Replace("_", "").Replace("-", "").Replace("(", "").Replace(")", "");

                    HtmlGenericControl _Card = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card card-outline-info h-100");
                    HtmlGenericControl _CardBlock = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "card-block");
                    _CardBlock.Style.Add("padding-left", "10px");
                    _CardBlock.Style.Add("padding-top", "10px");
                    _CardBlock.Style.Add("padding-right", "10px");
                    HtmlGenericControl _h4 = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div", "h4 m-0");
                    _h4.InnerHtml = oEasyFileInfo.Size;
                    //_CardBlock.Controls.Add(_h4);
                    tblHead.Rows[0].Cells[0].Controls.Add(_h4);
                    tblHead.Rows[0].Cells[0].Style.Add("width", "100%");

                    HtmlImage img = new HtmlImage();
                    img.Attributes.Add("class", "m-4");
                    img.Src = EasyUtilitario.Constantes.ImgDataURL.IconDeItem;
                    img.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), this.ClientID + "_FileAction(" + oEasyFileInfo.ToString(true) + ",'Delete')");
                    img.Attributes["style"] = "cursor:pointer;";

                     tblHead.Rows[0].Cells[1].Controls.Add(img);
                    _CardBlock.Controls.Add(tblHead);


                    HtmlGenericControl _File = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("div");
                    _File.InnerHtml = ((oEasyFileInfo.Nombre.Length > 58) ? oEasyFileInfo.Nombre.Substring(0, 58) + "..." : oEasyFileInfo.Nombre);
                    _File.Attributes["style"]="text-decoration:underline;color:blue;cursor:pointer;";
                    if (oEasyFileInfo.Temporal==true)
                    {
                        _File.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = "window.location.href ='" + this.PathLocalyWeb.UrlTemporal + oEasyFileInfo.Nombre + "'";
                    }
                    else {
                        _File.Attributes[EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString()] = "window.location.href ='" + this.PathLocalyWeb.UrlFinal + oEasyFileInfo.Nombre + "'";
                    }

                    _CardBlock.Controls.Add(_File);

                    _Card.Controls.Add(_CardBlock);

                    HtmlGenericControl _small = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("small", "text-muted cat col-sm-12");
                    HtmlGenericControl _i = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("i", "far fa-clock text-info");
                    _i.InnerHtml = "Type:";
                    _small.Controls.Add(_i);
                    _small.InnerHtml = oEasyFileInfo.Tipo;

                    _Card.Controls.Add(_small);
                    dvContext.Controls.Add(_Card);

                    HtmlGenericControl Separador = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("br");
                    Separador.Attributes["id"]= "br" + CardID;
                    dvContext.Controls.Add(Separador);
                }
            }
            return dvContext;
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
                (new LiteralControl("<script>\n" + ScriptSelected() + "\n" + ScriptSendFileInfo() + "\n" + "</script>\n")).RenderControl(writer);

                (new LiteralControl("<script>\n" + scriptOnCompleteInterno() + "</script>")).RenderControl(writer);
                (new LiteralControl("<script>\n" + scriptDeclare() + "</script>")).RenderControl(writer);
                

                (new LiteralControl(fcScriptDeleteFile())).RenderControl(writer);
                

                base.Render(writer);
                //Controles Propies
                HtmlTable tblScr= tblScreen();
                tblScr.RenderControl(writer);

                //Crea el Objeto a nivel de javascript
                string ObjAndPrpperty = "var " + this.ClientID + @"={};
                                        " + this.ClientID + @".HttpTmp='" + this.PathLocalyWeb.UrlTemporal + @"';
                                        " + this.ClientID + @".HttpFinal='" + this.PathLocalyWeb.UrlFinal + @"';
                                        " + this.ClientID + @".RutaLocalTmp='" + this.PathLocalyWeb.CarpetaTemporal + @"';
                                        " + this.ClientID + @".RutaLocalFinal='" + this.PathLocalyWeb.CarpetaFinal + @"';";

                (new LiteralControl("<script>\n" + ObjAndPrpperty + "</script>")).RenderControl(writer);


                foreach (LiteralControl lcScript in BloqueScript) {
                    lcScript.RenderControl(writer);

                }

            }
            if (IsDesign())
            {
                (new LiteralControl("Subir Archivos..")).RenderControl(writer);
            }
            //Crear la instacia de los items en la coleccion
            (new LiteralControl("<script>\n" + ScriptColletionBE + "</script>")).RenderControl(writer);

        }

        #region Eventos Locales
            protected virtual void EasyDeleteItem_Click(object sender, EventArgs e)
            {
                string Argument = ((System.Web.UI.Page)HttpContext.Current.Handler).Request["__EVENTARGUMENT"];
                if (Argument.Length > 0)
                {
                    List<EasyFileInfo> allEasyFileInfo = (List<EasyFileInfo>)getAllFiles();
                    EasyFileInfo oEasyFileInfoBE =  allEasyFileInfo.Find(oBE => oBE.Nombre == Argument);
                    int index = allEasyFileInfo.FindIndex(oBE => oBE.Nombre == Argument);

                    if (oEasyFileInfoBE != null) 
                    {
                        oEasyFileInfoBE.IdEstado = 0;
                        allEasyFileInfo[index] = oEasyFileInfoBE;
                        setAllFiles(allEasyFileInfo);
                    }
                }
            }

            protected virtual void EasyUpLoadButton_Click(object sender, EventArgs e)
            {
                //Carga imagen al area temporal   
                string Argument = ((System.Web.UI.Page)HttpContext.Current.Handler).Request["__EVENTARGUMENT"];
                if (Argument.Length > 0)
                {
                List<EasyFileInfo> allEasyFileInfo = (List<EasyFileInfo>)getAllFiles();
                if (allEasyFileInfo == null) allEasyFileInfo = new List<EasyFileInfo>();

                    foreach (string strFileInfo in Argument.Split('@')) {
                        string[] Infor = strFileInfo.Split('|');

                        EasyFileInfo InfoExis =allEasyFileInfo.Find(oBE => oBE.Nombre == Infor[0]);
                        if (InfoExis == null)
                        {
                            EasyFileInfo oEasyFileInfoBE = new EasyFileInfo(Infor[0], Infor[1], Infor[2], Infor[3], false);
                            oEasyFileInfoBE.IdEstado = 1;
                            oEasyFileInfoBE.Temporal = true;
                            allEasyFileInfo.Add(oEasyFileInfoBE);
                        }
                        else {
                            InfoExis.IdEstado = 1;
                        }
                    }
                    setAllFiles(allEasyFileInfo);

                if (CargarArchivo != null)//verifica si esta asociado en la pagina con el evento
                    {
                        CargarArchivo?.Invoke(allEasyFileInfo);
                    }
                }
            }

        public List<EasyFileInfo> getAllFiles()
        {
            return getAllFiles(false);
        }
        public List<EasyFileInfo> getAllFiles(bool paint) {
            //Preguntar si es null el contenido de viewstate para luego revisar el contenido del texbox y acoplar los items cargados en el lado del cliente
            List<EasyFileInfo> allEasyFileInfo = (List<EasyFileInfo>)ViewState[this.ClientID];
            

            //Se obtiene del cuadro de texto
            string []sItem = this.getListFiles().Split(EasyUtilitario.Constantes.Caracteres.SeparadorHorizontal.ToCharArray());
            if ((sItem.Length > 0) && (sItem[0].Length != 0))
            {
                foreach (string ssItem in sItem)
                {
                    string[] arrDatos = ssItem.Replace("{", "")
                                                .Replace("}", "")
                                                .Replace(EasyUtilitario.Constantes.Caracteres.ComillaDoble, "")
                                                .Split(EasyUtilitario.Constantes.Caracteres.Coma.ToCharArray());

                    Dictionary<string,string> oRData = new Dictionary<string,string>();
                    foreach (string CampoAndValor in arrDatos) {
                        string []arrFielVal= CampoAndValor.Split(EasyUtilitario.Constantes.Caracteres.DosPuntos.ToCharArray());
                        oRData[arrFielVal[0]] = arrFielVal[1];
                    }

                    /*string[] NombreValor = arrDatos[2].Split(':');
                    //string Nombre = NombreValor[1];//Obtiene el nombre del archivo de la propiedad Text
                    string Nombre = NombreValor[1];//Obtiene el nombre del archivo de la propiedad Text*/
                    string Nombre = oRData["Text"];

                    if (allEasyFileInfo == null) allEasyFileInfo = new List<EasyFileInfo>();
                    EasyFileInfo InfoExis = allEasyFileInfo.Find(oBE => oBE.Nombre == Nombre);
                    if (InfoExis == null)
                    {
                        string IdFile = arrDatos[3].Split(':')[1];
                        EasyFileInfo oEasyFileInfoBE = new EasyFileInfo();
                        oEasyFileInfoBE.Nombre = Nombre;
                        oEasyFileInfoBE.IdFile = IdFile;
                        oEasyFileInfoBE.IdEstado = 1;
                        oEasyFileInfoBE.Temporal = true;
                        allEasyFileInfo.Add(oEasyFileInfoBE);
                    }
                    else { //
                        InfoExis.IdEstado = Convert.ToInt32(oRData["IdEstado"]);
                    }
                    setAllFiles(allEasyFileInfo);
                }
            }
            if (allEasyFileInfo != null)
            {
                //Limpia el cuadro de texto para volver a cargarlo
                foreach (EasyFileInfo oi in allEasyFileInfo)
                {
                    EasyFileInfo InfoExis = this.ListadeArchivos.Find(oBE => oBE.Nombre == oi.Nombre);
                    if (InfoExis == null)
                    {
                        if (oi.Nombre.Length > 0)
                        {
                            EasyFileInfo oEasyFileInfoBE = new EasyFileInfo();
                            oEasyFileInfoBE.Nombre = oi.Nombre;
                            oEasyFileInfoBE.IdFile = oi.IdFile;
                            oEasyFileInfoBE.IdEstado = oi.IdEstado;
                            oEasyFileInfoBE.Temporal = oi.Temporal;
                            this.ListadeArchivos.Add(oEasyFileInfoBE);
                        }
                    }
                }
                setAllFiles(this.ListadeArchivos);
            }
            //Vlueve a pintar los items
            if (paint) {
                this.CreateChildControls();
            }

            return allEasyFileInfo;
        }
       
        public void setAllFiles(List<EasyFileInfo> allEasyFileInfo)
        {
            ViewState[this.ClientID] = allEasyFileInfo;
        }


        #endregion
        public string getListFiles() {
            return oListView.getListItems();
        }

        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
    }
}
