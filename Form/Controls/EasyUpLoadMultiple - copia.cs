using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Resources;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;

namespace EasyControlWeb.Form.Controls
{
    public  class EasyUpLoadMultiple: CompositeControl
    {
        HtmlGenericControl ContainerFile;
        TextBox txtLstItems;



        const string UPLOAD_PAGE = "PagProceso";
        #region Rutas

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
            get { return oEasyPathLocalWeb; }
            set { oEasyPathLocalWeb = (EasyPathLocalWeb)value; }
        }

        #endregion

        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncTempleteFile { get; set; }

        [Category("Scripts"), Description("")]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public string fncItemComplete { get; set; }







        [Browsable(false)]
        List<EasyFileInfo> oEasyFileItem;


        [Editor(typeof(EasyControlCollection.EasyFormCollectionUpFilesEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Contiene todos los controles necesarios en una List collection")]
        [Category("Behaviour"),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]

        public List<EasyFileInfo> FileCollections
        {
            get
            {
                if (oEasyFileItem == null)
                {
                    oEasyFileItem = new List<EasyFileInfo>();
                }
                return oEasyFileItem;
            }
        }

        public EasyUpLoadMultiple() : base()
        {
            if (txtLstItems == null) { txtLstItems = new TextBox(); }
        }




        protected override void OnInit(EventArgs e)
        {
        }
        protected override void OnPreRender(EventArgs e)
        {
        }
        protected override void CreateChildControls()
        {
            txtLstItems.ID = "txtLstItemsFile";
            txtLstItems.Style.Add("display", "none");
            this.Controls.Add(txtLstItems);
        }
        string[] GetData()
        {
            string[] LstItems = txtLstItems.Text.Split(EasyUtilitario.Constantes.Caracteres.Separador.ToCharArray());
            return LstItems;
        }
        public List<EasyFileInfo> GetCollection()
        {
            List<EasyFileInfo> oCollection = new List<EasyFileInfo>();
            string[] LstItems = GetData();
            if ((LstItems.Length > 0)&&(LstItems[0].Length>0))
            {
                foreach (string Item in LstItems)
                {
                    EasyFileInfo oLitemInfo = new EasyFileInfo();
                    Dictionary<string, string> oData = EasyUtilitario.Helper.Data.SeriaizedDiccionario(Item);
                    oLitemInfo.Nombre = oData["Nombre"];
                    oLitemInfo.Tipo = oData["Tipo"];
                    oLitemInfo.Size= oData["Size"];
                    oLitemInfo.Temporal=((oData["Existe"]=="true")?false:true);
                    oLitemInfo.IdEstado = Convert.ToInt32(oData["IdEstado"]);
                    oLitemInfo.Enviado = Convert.ToBoolean(oData["Enviado"]);
                    oLitemInfo.Existe = Convert.ToBoolean(oData["Existe"]);
                    oCollection.Add(oLitemInfo);
                }
            }
            return oCollection;
        }


        HtmlGenericControl HtmlPathContent()
        {
            ContainerFile = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("Div", "containerFile");
            ContainerFile.Attributes.Add("style", "width:" + this.Width);

            HtmlTable otbl = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(1, 2);
            otbl.Attributes.Add("style", "width:"+ this.Width);

            HtmlGenericControl DivNroFile = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("Div");
            DivNroFile.ID = this.ClientID + "_num-of-files";
            DivNroFile.InnerText = "Nro de Archivs seleccinados";
            otbl.Rows[0].Cells[0].Controls.Add(DivNroFile);
            otbl.Rows[0].Cells[0].Attributes.Add("style", "width:90%");

            HtmlGenericControl Label = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("label");
            Label.Attributes["for"] = this.ClientID;
            Label.InnerText = "Subir";
            otbl.Rows[0].Cells[1].Controls.Add(Label);

            LiteralControl inputFile = new LiteralControl("<input type=\"file\" id=\""+ this.ClientID + "\" multiple />");

            ContainerFile.Controls.Add(inputFile);
            ContainerFile.Controls.Add(otbl);
            //detalle de contenido
            HtmlGenericControl FileList = EasyUtilitario.Helper.HtmlControlsDesign.CrearControl("Div", "ulFile ScrollFile");
            FileList.Attributes.Add("style","height:"+ this.Height);

            FileList.ID = this.ClientID +"_files-list" ;
            ContainerFile.Controls.Add(FileList);

            return ContainerFile;
        }
        //referencia de Visor de Archivos:https://www.javascripttutorial.net/web-apis/javascript-filereader/

        string Scriptembedded() {
            string backslash = "\\";
            string PaginaProceso = this.PaginaProceso + @"?PathLocal=@" + this.PathLocalyWeb.CarpetaTemporal.ToString().Replace(backslash, @".");
            string fncItemCompeteTMP = ((this.fncItemComplete != null) ? this.fncItemComplete + "(NombreFile,LstFiles);" : "");//Cuando termina de subir el item en el servidor
            string NameInput = this.ClientID + "fileInput";
            string FncScriptTemplate= ((this.fncTempleteFile != null) ? this.fncTempleteFile + "(oFileBE);" : "`<p>${oFileBE.Nombre}</p><p>${oFileBE.Size}</p>`;");
            return @"<script>
                        var " + this.ClientID + @"={};
                       
                        " + this.ClientID + @".onCompleteTask=function(NombreFile,LstFiles){
                            " + fncItemCompeteTMP + @"
                            " + this.ClientID + @"_SetListCollection();
                        }
                          " + this.ClientID + @".HtmlFileItem = function(oFileBE){
                                    let listItem = jNet.create('div');
                                        listItem.attr('id',oFileBE.Nombre);
                                        listItem.style['width'] = '100%';
                                        listItem.innerHTML = " + FncScriptTemplate + @"
                              return listItem;
                          }
                        " + this.ClientID + @".UpLoad = new EasyUpLoad();
                         " + this.ClientID + @".UpLoad.PaginaProceso ='" + PaginaProceso  + @"';                         
                          let " + NameInput + " =  document.getElementById('" + this.ClientID + @"');
                          let "+ this.ClientID + "fileList = jNet.get('"+ this.ClientID + @"_files-list');
                          let "+ this.ClientID + "numOfFiles = document.getElementById('" + this.ClientID + @"_num-of-files');
                          " + this.ClientID + @".UpLoad.fncItemComplete = " + this.ClientID + @".onCompleteTask;
                            "+ NameInput + @".addEventListener('change', () => {
                                    for (file of " + NameInput + @".files) {
                                                let reader = new FileReader();
                                                let fileName = file.name;
                                                let fileSize = ((file.size / 1024).toFixed(1)).toString() +' KB';
                                                if (fileSize >= 1024) {
                                                    fileSize = ((fileSize / 1024).toFixed(1)).toString() + ' MB';
                                                }
                                        //Verifica si existe 
                                        var oResultBE = " + this.ClientID + @".GetFile(file.name);
                                        var oIemBE = new EasyUploadFileBE(file);
                                        if(oResultBE.FileBE==null){
                                                 " + this.ClientID + @"fileList.appendChild( " + this.ClientID + @".HtmlFileItem(oIemBE));
                                                 if(file){
                                                        " + this.ClientID + @".UpLoad.FileCollections.Add(oIemBE);
                                                 }
                                         }
                                         else if((oResultBE.FileBE!=null)&&(oResultBE.FileBE.IdEstado==0)){
                                                oResultBE.FileBE.IdEstado=1;
                                              " + this.ClientID + @".UpLoad.FileCollections[oResultBE.FileBE.Posicion]=oResultBE.FileBE;

                                              " + this.ClientID + @"fileList.appendChild( " + this.ClientID + @".HtmlFileItem(oResultBE.FileBE));
                                         }
                                         else{
                                                var msgConfig = { Titulo: 'Up Load File', Descripcion: 'Error Al cargar el Archivo' +  file.name +'\n ya existe' };
                                                     var oMsg = new SIMA.MessageBox(msgConfig);
                                                     oMsg.Alert();
                                         }
                                    }
                                    " + this.ClientID + @".UpLoad.Send();
                                    //Elabora el Json para ser usado en el lado del servidor
                                    " + this.ClientID + @"numOfFiles.textContent = `${(" + this.ClientID + @".UpLoad.Count())} Files Selected`;   
                                    " + this.ClientID + @"_SetListCollection();

                            });
                            " + this.ClientID + @".DoUpLoad=function(){
                                    if (" + this.ClientID + @".UpLoad.Count() > 0) {//verifica si contiene archivos a ser cargadoos
                                        " + this.ClientID + @".UpLoad.Send();
                                        " + this.ClientID + @".UpLoad.Clear();
                                    }
                            }
                              " + this.ClientID + @".GetFile=function(IdObj){
                                    var oFile=null;Idx=0;
                                     " + this.ClientID + @".UpLoad.FileCollections.forEach(function(FileBE,p){
                                                if(IdObj==FileBE.Nombre){
                                                        oFile=FileBE;
                                                        Idx=p;
                                                }
                                        });
                                    
                                    return {FileBE:oFile,Posicion:Idx};
                              }
                            " + this.ClientID + @".Find=function(IdObj){
                                var oResultBE = " + this.ClientID + @".GetFile(IdObj);
                                return oResultBE;
                            }
                            " + this.ClientID + @".RemoveItem=function(IdObj){

                                 var objItemFile = jNet.get(IdObj);
                                     objItemFile.css('border','1px dotted #5394C8');
                                 var ConfigMsgb = {
                                         Titulo: 'Gestor de Cargas'
                                         , Descripcion: 'Desea ud eliminar este archivo de la Lista ahora?'
                                         , Icono: 'fa fa-question-circle'
                                         , EventHandle: function (btn) {
                                             if (btn == 'OK') {
                                                //Elimina el Control
                                                " + this.ClientID + @"fileList.remove(IdObj);
                                                    var oResultBE= " + this.ClientID + @".Find(IdObj);
                                                    var oItemBE = oResultBE.FileBE;
                                                    oItemBE.IdEstado=0;
                                                    " + this.ClientID + @".UpLoad.FileCollections[oResultBE.Posicion]=oItemBE;
                                                  "+ this.ClientID + @"numOfFiles.textContent = `${(" + this.ClientID + @".UpLoad.Count())} Files Selected`;
                                                  " + this.ClientID + @"_SetListCollection();
                                             }
                                             else{
                                                objItemFile.css('border-style','none');
                                             }
                                         }
                                     };
                                     var oMsg = new SIMA.MessageBox(ConfigMsgb);
                                     oMsg.confirm();

                            }
                    </script>";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsDesign())
            {
                txtLstItems.RenderControl(writer);
                this.HtmlPathContent().RenderControl(writer);
                (new LiteralControl(Scriptembedded())).RenderControl(writer);
                //Script Cargar coleccion in Script
                string ScriptFile = this.ClientID + @".HttpTmp='" + this.PathLocalyWeb.UrlTemporal + @"';
                                    " + this.ClientID + @".HttpFinal='" + this.PathLocalyWeb.UrlTemporal + @"';
                                    " + this.ClientID + @".Paint=function(){
                                                            var oFile=null;
                                                            var oIemBE=null;";
                foreach (EasyFileInfo oEasyFileInfo in this.FileCollections) {
                    string []Size = FileSize(this.PathLocalyWeb.CarpetaFinal + oEasyFileInfo.Nombre);
                    ScriptFile += @"    oFile  = {name:'" + oEasyFileInfo.Nombre + "',type :'" + oEasyFileInfo.Tipo + "',size:'" + Size[0] + @"',Binary:''};
                                        oIemBE = new EasyUploadFileBE(oFile);
                                       // oIemBE.Binary=urlToBinary('" + EasyUtilitario.Helper.Configuracion.Leer("ConfigModHelpDesk", "HelpDeskHttpFiles")+ oEasyFileInfo.Nombre + @"');
                                        oIemBE.Existe=true;
                                   " + this.ClientID + @".UpLoad.FileCollections.Add(oIemBE);
                                              
                                ";
                }

                ScriptFile += @"
                                 " + this.ClientID + @".UpLoad.FileCollections.forEach(function(oFileBE,p){
                                                        " + this.ClientID + @"fileList.appendChild( " + this.ClientID + @".HtmlFileItem(oFileBE));    
                                                    });
                        }
                         Manager.Task.Excecute(function () {
                                                    " + this.ClientID + @".Paint();
                                                     " + this.ClientID + @"_SetListCollection();//convierte a JSON para su lectura den el server
                                                }, 1000,true);
                    ";

                (new LiteralControl("<script>\n" + ScriptFile + "</script>\n")).RenderControl(writer);

                string ScriptSet = "function " + this.ClientID + @"_SetListCollection(){
                                            var objTxtLst = jNet.get('" + txtLstItems.ClientID + @"');
                                            objTxtLst.value = '';
                                            " + this.ClientID + @".UpLoad.FileCollections.forEach(function(oItem, i){
                                                        var strBE = JSON.stringify(oItem);
                                                            objTxtLst.value += ((i == 0) ? '' : '" + EasyUtilitario.Constantes.Caracteres.Separador.ToString() + @"') + strBE;
                                            });
                                        }";
                (new LiteralControl("<script>\n" + ScriptSet + "</script>\n")).RenderControl(writer);

            }
            else {
                (new LiteralControl("Subir Archivos")).RenderControl(writer);
            }
        }
        string []FileSize(string PathFile)
        {
            double len = 0;
            int order = 0;
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            try
            {
                len = new FileInfo(PathFile).Length;
                
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }
                // string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            }
            catch (Exception ex) {
                len = 0;    
            }
            string[] Result = { len.ToString(), len.ToString() + " " + sizes[order] };
            return Result;
        }

        public void TemporalToFinal()
        {
            TemporalToFinal(null);

        }
        public void TemporalToFinal(EasyFileInfo oEasyFileInfo) {
            try
            {
                if (oEasyFileInfo == null)
                {
                    List<EasyFileInfo> oCollection = new List<EasyFileInfo>();
                    string[] LstItems = GetData();
                    if (LstItems.Length > 0)
                    {
                        foreach (string Item in LstItems)
                        {
                            EasyFileInfo oLitemInfo = new EasyFileInfo();
                            Dictionary<string, string> oData = EasyUtilitario.Helper.Data.SeriaizedDiccionario(Item);
                            string NombreFile = oData["Nombre"];
                            if (Convert.ToBoolean(oData["Existe"]) && Convert.ToBoolean(oData["Enviado"]) == true)
                            {
                                File.Move(this.PathLocalyWeb.CarpetaTemporal + NombreFile, this.PathLocalyWeb.CarpetaFinal + NombreFile);
                            }
                        }
                    }
                }
                else {
                    File.Move(this.PathLocalyWeb.CarpetaTemporal + oEasyFileInfo.Nombre, this.PathLocalyWeb.CarpetaFinal + oEasyFileInfo.Nombre);
                }
            }
            catch (Exception ex) { 

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
