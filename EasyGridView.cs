using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Web.UI.HtmlControls;
using System.Web;
using EasyControlWeb.Form.Controls;
using System.Data;
using System.Linq;
using EasyControlWeb.Form.Estilo;
using static EasyControlWeb.Form.Editor.EasyFormColletionsEditor;
using System.Windows.Markup;
using EasyControlWeb.InterConeccion;
using EasyControlWeb.Filtro;
using EasyControlWeb.InterConecion;
 
namespace EasyControlWeb
{
    /*
     * Autor:Rosales Azabache Eddy 
     * REFERENCIA DE POPUIPM MENU https://codepen.io/Danny-Dasilva/pen/wvGaMxE
     */

    #region TemplateColumn
    public class NumberColumn : ITemplate
        {
            public void InstantiateIn(Control container)
            {
            }
        }
    #endregion


    [
    ParseChildren(true, "EasyGridButtons"),
    ToolboxData("<{0}:EasyGridView runat=server></{0}:EasyGridView>")
    ]
    public class EasyGridView:GridView
    {
        #region Search event and delegate
            public delegate void EasyGridButtonClickEventHandler(EasyGridButton oEasyGridButton, System.Collections.Generic.Dictionary<string, string> Recodset);
            public event EasyGridButtonClickEventHandler EasyGridButton_Click;

            public delegate void EasyGridClickDetalleEventHandler(System.Collections.Generic.Dictionary<string, string> Recodset);
            public event EasyGridClickDetalleEventHandler EasyGridDetalle_Click;
        #endregion

        EasyMessageBox oeasyMessageBox = null;

        #region Constantes
            const string SCR_TOOLBAR_CLICK_ITEM = "_ToolBar_Onclick";
            //Constants to hold value in view state
            /*private const string SHOW_EMPTY_FOOTER = "ShowEmptyFooter";
            private const string SHOW_EMPTY_HEADER = "ShowEmptyHeader";*/
            private const string TEXT_EMPTY_HEADER = "TextEmptyHeader";
            private const string NO_OF_ROWS = "NoOfRows";
            private const string SHOW_ROWNUM = "ShowRowNum";
            private const string DATA_COLLECION = "dtColeccion";
            private const string DATA_TABLE_CLIENT = "dtClient";
            private const string ITEM_COLOR_SELECT = "ItemColorSelect";
            private const string ITEM_COLOR_MOUSEMOVE = "ItemColorMouseMove";
            private const string ITEM_EVENT_ONCLICK = "RowItemClick";
        private const string NO_DATA_FOUND = "NO DATA FOUND";
        #endregion

        #region Controls and constants
        // Controls to implement the search feature
        // Panel _pnlSearchFooter;
        //Controles Auxiliares
        TextBox _txtRowIndex;
        TextBox _txtNroRegSelect;
        TextBox _txtNroPag;

        TextBox _txtSort;

        TextBox _txtSortField;
        TextBox _txtSortAscDesc;

        TextBox txtDemo;

        EasyTextBox tField;
        EasyNumericBox nField;
        EasyDatepicker dField;

        HtmlButton btnIrDetalle;

            List<EasyMessageBox> LstMsgBoxs = new List<EasyMessageBox>();

            Table tblToolBar;
            HtmlButton oBtn;


        #endregion

        #region Variables Simples
        //bool DataNull = false;
        int NroColHeader = 0;
            string NroItem;
            string ArrDataBE = "";
            string Cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            List<LiteralControl> _Scripts = new List<LiteralControl>();
        #endregion

        #region Constructor
        // Constructor
        public EasyGridView(): base()
                {
                    //Initialise controls
                    //_pnlSearchFooter = new Panel();
                   
                    //By default turn on the footer shown property
                    //ShowFooter = true;

                   //Adicionales
                    _txtNroRegSelect = new TextBox();
                    _txtNroRegSelect.ID = "txtIdRegSelected";
            //Control de fila seleccionnada
                    _txtRowIndex = new TextBox();
                    _txtRowIndex.ID= "txtRowIndex";
            //Control de paginacion
                    _txtNroPag = new TextBox();
                    _txtNroPag.ID = "txtIdxPag";
                    _txtNroPag.Text = "0";
            //Control de Sorting
                    _txtSort = new TextBox();
                    _txtSort.ID = "txtSort";

                    _txtSortField = new TextBox();
                    _txtSortField.ID = "txtIdSortField";

                    _txtSortAscDesc = new TextBox();
                    _txtSortAscDesc.ID = "txtIdSortA_D";

                    btnIrDetalle = new HtmlButton();
                    btnIrDetalle.ID = "btnExtendDet";

                    oBtn = new HtmlButton();
                    oBtn.ID = "CmdCommit";


                    txtDemo = new TextBox();
                    txtDemo.ID = "IdDemo";



                    arrEasyGridButtons = new List<EasyGridButton>();
                    this.RowDataBound += new GridViewRowEventHandler(OnRowDataBound);
                    this.PageIndexChanging += new GridViewPageEventHandler(OnPageIndexChanging);
                    this.Sorting += new GridViewSortEventHandler(OnSorting);
            

        }
        #endregion


        #region Propiedades de Colección
        DataTable dtPorPag =new DataTable();

            [Browsable(true)]
            List<EasyGridButton> arrEasyGridButtons;
            [
               Category("Behavior"),
               Description("Colleccion de Botones de extension de funcionalidades"),
               DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
               Editor(typeof(EasyControlCollection.EasyFormCollectionGridButtonEditor), typeof(UITypeEditor)),
               PersistenceMode(PersistenceMode.InnerProperty)
           ]
            public List<EasyGridButton> EasyGridButtons
            {
                get { return arrEasyGridButtons; }
            }


        #endregion


        #region properties
       

        /*TITULO DE LA CABECERA*/
        /*-------------------------------------------------------------------------------------------------------------------*/
        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("Titulo")]
        public string TituloHeader
        {
            get
            {
                if (this.ViewState[TEXT_EMPTY_HEADER] == null)
                {
                    this.ViewState[TEXT_EMPTY_HEADER] = null;
                }

                return (string)this.ViewState[TEXT_EMPTY_HEADER];
            }
            set
            {
                this.ViewState[TEXT_EMPTY_HEADER] = value;
            }
        }


        EasyBtnStyle oEasyBtnStyle = new EasyBtnStyle();
        [TypeConverter(typeof(Type_StyleToolBarButtom))]
        [Description("Define estilo vigente para cada control button"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public EasyBtnStyle EasyStyleBtn
        {
            get { return oEasyBtnStyle; }
            set { oEasyBtnStyle = (EasyBtnStyle)value; }
        }



        EasyDataInterConect oEasyDataInterConect = new EasyDataInterConect();
        [TypeConverter(typeof(Type_DataInterConect))]
        [Category("Editor"),
           Description("Conexion a un servicios de donde se obtiene los datos."),
           DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public EasyDataInterConect DataInterconect
        {
            get { return oEasyDataInterConect; }
            set { oEasyDataInterConect = value; }
        }



        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public bool ShowRowNumber
        {
            get
            {
                if (this.ViewState[SHOW_ROWNUM] == null)
                {
                    this.ViewState[SHOW_ROWNUM] = true;
                }
                return (bool)this.ViewState[SHOW_ROWNUM];
            }
            set
            {               
                this.ViewState[SHOW_ROWNUM] = value;
            }
        }

        
        EasyGridExtended oEasyGridExtended = new EasyGridExtended();
        [Category("Editor"),Description("Define el comportamiento de colores de cada fila"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public EasyGridExtended EasyExtended
        {
            get {
                    if (oEasyGridExtended == null)
                    {
                        oEasyGridExtended = new EasyGridExtended();
                    }
                return oEasyGridExtended; 
                }
            set
            {
                oEasyGridExtended = value;
            }
        }


        EasyGridRowGroup oEasyGridRowGroup = new EasyGridRowGroup();
        [Category("Editor"), Description("Define estilo vigente para cada control button"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public EasyGridRowGroup EasyRowGroup
        {
            get
            {
                if (oEasyGridRowGroup == null)
                {
                    oEasyGridRowGroup = new EasyGridRowGroup();
                }
                return oEasyGridRowGroup;
            }
            set
            {
                oEasyGridRowGroup = value;
            }
        }


        [Category("Function Cliente")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public string ToolBarButtonClick
        {
            get
            {
                if (this.ViewState["ToolBarButtonClick"] == null)
                {
                    this.ViewState["ToolBarButtonClick"] = "OnEasyGridButton_Click";
                }

                return (string)this.ViewState["ToolBarButtonClick"];
            }
            set
            {
                this.ViewState["ToolBarButtonClick"] = value;
            }
        }
        [Category("Function Cliente")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public string fncExecBeforeServer
        {
            get
            {
                return (string)this.ViewState["fncExecBeforeServer"];
            }
            set
            {
                this.ViewState["fncExecBeforeServer"] = value;
            }
        }
        #endregion

        #region Suprimir  Propiedades Innecesarias
        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string EmptyDataText { get; set; }
      /*  [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new bool AutoGenerateColumns { get; set; }*/

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new bool AutoGenerateDeleteButton { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new bool AutoGenerateEditButton { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new bool AutoGenerateSelectButton { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new string Caption { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("", true)]
        public new  TableCaptionAlign CaptionAlign { get; set; }

        #endregion



        #region Titulo de la grilla
       
        GridViewRow gridRowTitulo(int NroCols)
        {
                GridViewRow r = new GridViewRow(-1, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableCell tc = new TableCell();
                tc.ColumnSpan = NroCols;
                tc.Text = TituloHeader;
                r.Controls.Add(tc);
                r.Attributes.Add("TipoRow", "1");
                NroColHeader++;
            return r;
        }

        #endregion

        #region ToolBar
        void CrearToolBar()//Definicion de la tabla que contendra los botones
            {
                tblToolBar = new Table();
                tblToolBar.Style.Add("width", "100%");
                TableRow trToolBar = new TableRow();
                if (IsDesign()) { 
                    tblToolBar.Attributes.Add("border", "1px");
                    trToolBar.BorderStyle = BorderStyle.Dotted;
                    trToolBar.BorderColor = System.Drawing.Color.FromArgb(127, 127, 127);
                }

                TableCell tcToolBar = new TableCell();
                tcToolBar.Style.Add("width", "33.33%");
                tcToolBar.Attributes.Add("align","left");
                //tcToolBar.Attributes.Add("align", "right");
                tcToolBar.Style.Add("white-space", "pre-wrap");
                tcToolBar.Style.Add("word-wrap", "break-word");
                trToolBar.Controls.Add(tcToolBar);
            
                tcToolBar = new TableCell();
                tcToolBar.Style.Add("width", "33.33%");
                tcToolBar.Attributes.Add("align", "justify");
                tcToolBar.Style.Add("white-space", "pre-wrap");
                tcToolBar.Style.Add("word-wrap", "break-word");
                trToolBar.Controls.Add(tcToolBar);

                tcToolBar = new TableCell();
                tcToolBar.Style.Add("width", "33.33%");
                tcToolBar.Style.Add("white-space", "pre-wrap");
                tcToolBar.Style.Add("word-wrap", "break-word");

                tcToolBar.Attributes.Add("align", "right");
                //tcToolBar.Attributes.Add("align", "left");
                trToolBar.Controls.Add(tcToolBar);
                tblToolBar.Controls.Add(trToolBar);
                //Controles de manipulacion de la grilla

                #region Controles de manipulacion
                    btnIrDetalle.Attributes.Add("runat", "server");
                    btnIrDetalle.InnerText = "CallWebForm";
                    btnIrDetalle.Style.Add("display", "none");
                    btnIrDetalle.ServerClick += new System.EventHandler(OnEasyGridDetalle_Click);
                    tblToolBar.Rows[0].Cells[0].Controls.Add(btnIrDetalle);

                    /*Crear el boton para la ejecucion de los comandos de los botones de la barra de herramientas del grid*/
                    oBtn.InnerText = "Commit;";
                    oBtn.Attributes.Add("runat", "server");
                    oBtn.ServerClick += new System.EventHandler(OnEasyGridButton_Click);
                    oBtn.Style.Add("display", "none");
                    tblToolBar.Rows[0].Cells[0].Controls.Add(oBtn);



                    _txtNroRegSelect.Style.Add("display", "none");
                    _txtRowIndex.Style.Add("display", "none");


                    tblToolBar.Rows[0].Cells[0].Controls.Add(_txtNroRegSelect);
                    tblToolBar.Rows[0].Cells[0].Controls.Add(_txtRowIndex);
                    //COntrol  de paginacion de la grilla
                    _txtNroPag.Style.Add("display", "none");
                    tblToolBar.Rows[0].Cells[0].Controls.Add(_txtNroPag);
                    //Control de Ordenamiento de la grilla
                    _txtSort.Style.Add("display", "none");
                    tblToolBar.Rows[0].Cells[0].Controls.Add(_txtSort);

                    _txtSortField.Style.Add("display", "none");
                    tblToolBar.Rows[0].Cells[0].Controls.Add(_txtSortField);

                    _txtSortAscDesc.Style.Add("display", "none");
                    tblToolBar.Rows[0].Cells[0].Controls.Add(_txtSortAscDesc);

                #endregion

            }
          
        GridViewRow gridRowToolBar(int NroCols)
        {
            GridViewRow r = new GridViewRow(-1, 0, DataControlRowType.Header, DataControlRowState.Normal);
            r.BackColor = System.Drawing.Color.White;
            TableCell HeaderTC = new TableCell();
            HeaderTC.ColumnSpan = NroCols;
                foreach (EasyGridButton oEasyGridButton in this.EasyGridButtons.Where(oBE => oBE.Ubicacion != EasyUtilitario.Enumerados.Ubicacion.Footer && (oBE.Id != null || oBE.Id.Length > 0)))
                {
                    HtmlGenericControl btnItem = oEasyGridButton.Materialized(this.EasyStyleBtn.ClassName, this.EasyStyleBtn.TextColor);
                    if (!IsDesign())
                    {
                        btnItem.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), this.ClientID + SCR_TOOLBAR_CLICK_ITEM + "(" + oEasyGridButton.ToString(true) + ");");
                    }

                    switch (oEasyGridButton.Ubicacion)
                    {
                        case EasyUtilitario.Enumerados.Ubicacion.Izquierda:
                            tblToolBar.Rows[0].Cells[0].Controls.Add(btnItem);
                            break;
                        case EasyUtilitario.Enumerados.Ubicacion.Centro:
                            tblToolBar.Rows[0].Cells[1].Controls.Add(btnItem);
                            break;
                        case EasyUtilitario.Enumerados.Ubicacion.Derecha:
                            tblToolBar.Rows[0].Cells[2].Controls.Add(btnItem);
                            break;
                    }
                }

           
            HeaderTC.Controls.Add(tblToolBar);

            //Crear y enlazar los controles
            r.Controls.Add(HeaderTC);

            return r;
        }

        #endregion



     
       

        #region overridden functions
        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            ConData++;
            /*Crea la tabla toolbar*/
            /*----------------------------------------------------------------------------------------*/
            CrearToolBar();
            /*----------------------------------------------------------------------------------------*/
                int count = base.CreateChildControls(dataSource, dataBinding);

            /*--------------------------------------------------------------------------------------------------*/
            //this.GenerarScriptDataSource(dataSource);//17-11-2022
            /*--------------------------------------------------------------------------------------------------*/

            //  no hay filas en la cuadrícula. crear encabezado y pie de página en este caso
            //if (count == 0 && (ShowEmptyFooter || ShowEmptyHeader))
            if (count == 0)//No existen registro a mostrar pero crea la barra de herramientas y las cabecera disponible para su manipulacion
            {

                Table table = this.CreateChildTable();
                DataControlField[] fields;


                if (this.AutoGenerateColumns)
                {
                    PagedDataSource source = new PagedDataSource();
                    source.DataSource = dataSource;

                    System.Collections.ICollection autoGeneratedColumns = this.CreateColumns(source, true);
                    fields = new DataControlField[autoGeneratedColumns.Count];
                    autoGeneratedColumns.CopyTo(fields, 0);
                }
                else
                {

                    fields = new DataControlField[this.Columns.Count];
                    this.Columns.CopyTo(fields, 0);
                }

                //Crear la Barra de herramientas
                if (this.EasyGridButtons != null)
                {
                    GridViewRow rowToolBar = gridRowToolBar(fields.Length);
                    table.Rows.Add(rowToolBar);
                }


                //if (ShowEmptyHeader)

                {
                    if ((TituloHeader != null) && (TituloHeader.Length > 0))//Titulo en la cabecera de la grilla
                    {
                        GridViewRow headerTitle = gridRowTitulo(fields.Length);
                        table.Rows.Add(headerTitle);
                    }

                    //Crea la Fila de la cabecera
                    GridViewRow headerRow = base.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
                    headerRow.Attributes.Add("TipoRow", "1");
                    NroColHeader++;

                    this.InitializeRow(headerRow, fields);
                    OnRowCreated(new GridViewRowEventArgs(headerRow));//desencadena el evento y pasa el parametro de la fila creada
                    table.Rows.Add(headerRow); //Adiciona la fila creada a la cuadricula gridview
                }

                //  create the empty row
                /*GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
                TableCell cell = new TableCell();
                cell.ColumnSpan = fields.Length;
                cell.Width = Unit.Percentage(100);

               if (this.EmptyDataTemplate != null)
                {
                    this.EmptyDataTemplate.InstantiateIn(cell);
                }
                else if (!string.IsNullOrEmpty(this.EmptyDataText))
                {
                    cell.Controls.Add(new LiteralControl(EmptyDataText));
                }

                emptyRow.Cells.Add(cell);
                table.Rows.Add(emptyRow);*/
                


                //if (ShowEmptyFooter)
                {

                    //  row en blanco
                    GridViewRow RowNull = base.CreateRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                    RowNull.Attributes.Add("TipoRow", "4");
                  //  RowNull.Style["Display"] = "none";
                    this.InitializeRow(RowNull, fields);
                    OnRowCreated(new GridViewRowEventArgs(RowNull));
                    table.Rows.Add(RowNull);


                    //  create footer row que se usara para ubicar el control de resutaldps del filtro
                    if (ShowFooter && ((this.oEasyGridExtended.IdGestorFiltro!=null)||(this.oEasyGridExtended.IdGestorFiltro.Length > 0)))
                    {
                        GridViewRow footerRow = base.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);
                        this.InitializeRow(footerRow, fields);
                        OnRowCreated(new GridViewRowEventArgs(footerRow));
                        table.Rows.Add(footerRow);
                    }

                }

                //this.Controls.Clear();
                this.Controls.Add(table);
                //llamar a la creacion de cabecera
              

            }

            return count;
        }

        protected override ICollection CreateColumns(PagedDataSource dataSource, bool useDataSource)
        {
            if (dataSource != null)
                ViewState[NO_OF_ROWS] = dataSource.DataSourceCount;

            return base.CreateColumns(dataSource, useDataSource);
        }

        protected override void OnInit(EventArgs e)
        {
           
            if (!this.IsDesign())
            {
                CrearColumnaNro(ShowRowNumber);
                //Atributos del lado del servidor traducidos a propiedades del control de lado del cliente
                this.Attributes.Add("ItemColorSeleccionado", this.EasyExtended.ItemColorSeleccionado);
                this.Attributes.Add("ItemColorMouseMove", this.EasyExtended.ItemColorMouseMove);
            }
            base.OnInit(e);
        }
        
      
        void CrearColumnaNro(bool _ShowRowNumber) {
            if (_ShowRowNumber)
            {
                TemplateField tmpCol = new TemplateField();
                NumberColumn numCol = new NumberColumn();
                tmpCol.ItemTemplate = numCol;
                // Insert this as the first column
                this.Columns.Insert(0, tmpCol);
            }
        }

    
        void OnEasyGridDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> Data = new Dictionary<String, string>();
                if (_txtNroRegSelect.Text.Length > 0)
                {
                    //Data = FindRegSelected(Convert.ToInt32(_txtNroRegSelect.Text) - 1);
                    Data = FindRegSelected(_txtNroRegSelect.Text);
                }
                if (EasyGridDetalle_Click != null) EasyGridDetalle_Click?.Invoke(Data);
            }
            catch (Exception ex) {
                oeasyMessageBox = new EasyMessageBox();
                oeasyMessageBox.ID = "MsgGrid";
                oeasyMessageBox.Titulo = "Error";
                string msg = ex.Message.Replace("'", "").Replace("'", "").Replace("\n", "");
                oeasyMessageBox.Contenido = msg;
                oeasyMessageBox.Tipo = EasyUtilitario.Enumerados.MessageBox.Tipo.AlertType;
                oeasyMessageBox.AlertStyle = EasyUtilitario.Enumerados.MessageBox.AlertStyle.modern;
                Page.Controls.Add(oeasyMessageBox);
            }
        }
        protected virtual void OnEasyGridButton_Click(object sender, EventArgs e)
        {
            //string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            bool ExcecuteConData = true;
            string strID = "";
            string TipoBtn = ((Type)sender.GetType()).Name;
            switch (TipoBtn) {
                case "HtmlButton":
                    strID = ((HtmlButton)sender).ID;

                    break;
                case "ImageButton":
                    strID = ((ImageButton)sender).ID;
                    ExcecuteConData = false;
                    break;
            }
            if (ExcecuteConData)
            {
                //Obtener valor del argumento 
                HttpRequest ContextRequest = ((System.Web.UI.Page)HttpContext.Current.Handler).Request;
                strID = ContextRequest["__EVENTARGUMENT"];

                var item = this.EasyGridButtons.Find(x => x.Id == strID);//Boton seleccionado
                //Almacena el Boton Seleccionado quedando disponible para su verificacion
              //  ViewState["btnSelected"] = item;

                Dictionary<string, string> Data = new Dictionary<String, string>();
                if (_txtNroRegSelect.Text.Length > 0)
                {
                    //Data = FindRegSelected(Convert.ToInt32(_txtNroRegSelect.Text)-1);
                    Data = FindRegSelected(_txtNroRegSelect.Text);
                }
                if (EasyGridButton_Click != null) EasyGridButton_Click?.Invoke(item, Data);
            }
          /*
            if (this._txtNroRegSelect.Text.Trim().Length > 0) {
                    string _ScriptSelectItem = @"<script>
                                                    (function(){
                                                                try{
                                                                    var ogridView=document.getElementById('" + this.ClientID + @"');
                                                                    var rows = ogridView.querySelectorAll('[TipoRow=" + Cmll + "2" + Cmll + @"]'); 
                                                                    var GuidRegSelect = '" + this._txtNroRegSelect.Text + @"';
                                                                        alert(GuidRegSelect);
                                                                        rows.forEach(function(row,idx){
                                                                                        if(jNet.get(row).attr('Guid')==GuidRegSelect){
                                                                                            SIMA.GridView.Extended.OnEventClickChangeColor(row);
                                                                                        }
                                                                                   }
                                                                                 );
                                                                    }
                                                                    catch(ex){
                                                                    } 
                                                                 }
                                                        )();
                                                </script>
                                               ";


                _Scripts.Add(new LiteralControl(_ScriptSelectItem));
            }
            */
        }

        // Dictionary<string, string> FindRegSelected(int NroReg) {
        Dictionary<string, string> FindRegSelected(string NroReg)
        {
            Dictionary<string, string> DataExist = new Dictionary<String, string>();
            try
            {
               // if (NroReg == -1) { return null; }
                DataTable dtRowSelected = (DataTable)ViewState[DATA_COLLECION];
                DataRow[] drs = dtRowSelected.Select("Guid='" + NroReg + "'");
                DataRow dr = drs[0];
                //DataRow dr = dtRowSelected.Rows[NroReg];
                foreach (DataColumn dc in dtRowSelected.Columns)
                {
                    DataExist.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
            }
            catch (Exception ex)
            {
                oeasyMessageBox = new EasyMessageBox();
                oeasyMessageBox.ID = "MsgGrid";
                oeasyMessageBox.Titulo = "Error";
                string msg = ex.Message.Replace("'", "").Replace("'", "").Replace("\n", "");
                oeasyMessageBox.Contenido = msg;
                oeasyMessageBox.Tipo = EasyUtilitario.Enumerados.MessageBox.Tipo.AlertType;
                oeasyMessageBox.AlertStyle = EasyUtilitario.Enumerados.MessageBox.AlertStyle.modern;
                //Page.Controls.Add(oeasyMessageBox);
                ((System.Web.UI.Page)HttpContext.Current.Handler).Controls.Add(oeasyMessageBox);
            }

            return DataExist;
        }

        string ScriptDataTableDef = "";
        string ScriptDataTableDefDC = "";
        string ScripTDataRow = "";
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            base.OnRowCreated(e);
            ConData++;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                try
                {
                    if (Controls.Count > 0)
                    {
                        int NroCols = e.Row.Cells.Count;
                        Table gridControl = (Table)Controls[0];//Control grid 
                        //Crear la Barra de herramientas
                        if (this.EasyGridButtons != null)
                        {
                            GridViewRow rowToolBar = gridRowToolBar(NroCols);//Crear la fila en la grilla con la barra de herramientas

                            if (this.EasyGridButtons.Count == 0) { rowToolBar.Attributes["style"] = "display:none"; }

                            gridControl.Rows.Add(rowToolBar);//agrega la fila creada con la barra de herramientas a la grilla

                        }
                        //Titulo de la grilla
                        if ((TituloHeader != null) && (TituloHeader.Length > 0))//Titulo en la cabecera de la grilla
                        {
                            GridViewRow headerTitle = gridRowTitulo(NroCols);
                            gridControl.Rows.Add(headerTitle);
                        }
                    }

                    if (this.EasyExtended.IdGestorFiltro != null)//Si el nombre del control de filtros asociados existe
                    {
                        EasyGestorFiltro oGFiltro = (EasyGestorFiltro)Page.FindControl(this.EasyExtended.IdGestorFiltro);
                        if(oGFiltro!=null) {
                            int idx = 0;
                            string _DataField = "";
                            string _DataFieldTitle = "";
                            foreach (var col in this.Columns)
                            {
                                if (col.GetType() == typeof(BoundField))
                                {
                                    BoundField oBoundField = (BoundField)col;
                                    if ((oBoundField.DataField != null) && (oBoundField.DataField.Length > 0))
                                    {
                                        _DataField = oBoundField.DataField;
                                        _DataFieldTitle = oBoundField.HeaderText;
                                        //Script para buscar el CAMPO de la columna seleccionada
                                        //string scOnDblClick = "EasyGestorFiltro1_FindCampo('" + _DataField + "','" + _DataFieldTitle + "'); EasyGestorFiltro1_OpenWinModal('EasyGestorFiltro1_Gen',undefined,'" + _DataField + "');";
                                        string scOnDblClick = "EasyGestorFiltro1_SwichWinOpcion('" + _DataField + "','" + _DataFieldTitle + "');";
                                        e.Row.Cells[idx].Attributes[EasyUtilitario.Enumerados.EventosJavaScript.ondblclick.ToString()] = "javascript:" + scOnDblClick;
                                    }
                                }
                                else if (col.GetType() == typeof(TemplateField))
                                {

                                }
                                idx++;
                            }
                        }
                    }


                    #region controles relacionados a las columnas por su tipo de datos  disponibles para el control filtros falta copletar la funcionalidad
                    /*
                    //establece evento en cada columna de la grilla cuya propiedad data field contenga valor valido
                    EasyGestorFiltro oGFiltro = (EasyGestorFiltro)Page.FindControl(this.EasyExtended.IdGestorFiltro);
                    if (oGFiltro != null)//Si el nombre del control de filtros asociados existe
                    {
                        int idx = 0;
                        string _DataField = "";
                        string _DataFieldTitle = "";
                        if ((this.Columns != null) && (this.Columns.Count > 0))
                        {
                            //Obtiene las columnas del DataView o DataTable   21/11/2024
                            System.Data.DataColumnCollection ColumnsColections = null;
                            if (this.DataSource.GetType() == typeof(DataView))
                            {
                                ColumnsColections = ((DataView)this.DataSource).Table.Columns;
                            }
                            else//Es un datatable
                            {
                                ColumnsColections = ((DataTable)this.DataSource).Columns;
                            }
                            foreach (var col in this.Columns)
                            {
                                if (col.GetType() == typeof(BoundField))
                                {
                                    BoundField oBoundField = (BoundField)col;
                                    if ((oBoundField.DataField != null) && (oBoundField.DataField.Length > 0))
                                    {
                                        _DataField = oBoundField.DataField;
                                        _DataFieldTitle = oBoundField.HeaderText;

                                        if (ColumnsColections.Contains(_DataField))
                                        {
                                            string idCtrl = "Fld" + _DataField;
                                            DataColumn dc = ColumnsColections[_DataField];
                                            if ((dc.DataType == typeof(Char)) || (dc.DataType == typeof(String)))
                                            {
                                                tField = new EasyTextBox();
                                                tField.ID = idCtrl;
                                                tField.Attributes["style"] = "display:none";
                                                this.Controls.Add(tField);
                                            }
                                            else if ((dc.DataType == typeof(Decimal)) || (dc.DataType == typeof(Double)) || (dc.DataType == typeof(Int16)) || (dc.DataType == typeof(Int32)) || (dc.DataType == typeof(Int64)))
                                            {
                                                nField = new EasyNumericBox();
                                                nField.ID = idCtrl;
                                                nField.Attributes["style"] = "display:none";
                                                this.Controls.Add(nField);
                                            }
                                            else if (dc.DataType == typeof(DateTime))
                                            {
                                                dField = new EasyDatepicker();
                                                dField.ID = idCtrl;
                                                dField.Attributes["style"] = "display:none";
                                                this.Controls.Add(dField);
                                            }

                                        }

                                        //Script para buscar el CAMPO de la columna seleccionada
                                        string scOnDblClick = "EasyGestorFiltro1_FindCampo('" + _DataField + "','" + _DataFieldTitle + "'); EasyGestorFiltro1_OpenWinModal('EasyGestorFiltro1_Gen',undefined,'" + _DataField + "');";
                                        e.Row.Cells[idx].Attributes[EasyUtilitario.Enumerados.EventosJavaScript.ondblclick.ToString()] = "javascript:" + scOnDblClick;
                                    }
                                }
                                else if (col.GetType() == typeof(TemplateField))
                                {

                                }
                                idx++;
                            }
                        }
                    }
                    */
                    //Cierra Region
                    #endregion
                }
                catch (Exception ex)
                {
                    int u = 0;
                }




                if (!IsDesign() && ShowRowNumber)
                {
                    e.Row.Cells[0].Text = "NRO";                        
                }
                e.Row.Attributes.Add("TipoRow", "1");//Para ser usuado por las funcinalidades JScript
                NroColHeader++;

            }
            else if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                NroItem = (e.Row.RowIndex + (this.PageSize * this.PageIndex) + 1).ToString();
                this.SetRowScriptFunctions(e.Row);
                /*
                if (!IsDesign()&& ShowRowNumber)//Numeracion de registro
                {
                        e.Row.Cells[0].Style.Add("Width", "3%");
                        e.Row.Cells[0].Text = NroItem;
                        e.Row.Cells[0].Style.Add("text-decoration","underline");
                        e.Row.Cells[0].Style.Add("cursor", "hand");                        
                        if (!IsDesign())
                        {
                        //e.Row.Cells[0].Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), this.ClientID + "_OnDetalle('" + e.Row.RowIndex + "')");
                            e.Row.Cells[0].Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), this.ClientID + "_OnDetalle(jNet.get(this.parentNode))");                        
                        }
                }

                e.Row.Attributes.Add("onmouseover", "javascript:SIMA.GridView.Extended.OnEventMouseInOutChangeColor(this, true);");
                e.Row.Attributes.Add("onmouseout", "javascript:SIMA.GridView.Extended.OnEventMouseInOutChangeColor(this, false);");
                if (this.EasyExtended.RowItemClick != null)
                {
                    //e.Row.Attributes.Add("onclick", "javascript:jNet.get('" + this.ClientID + "_txtIdRegSelected').attr('value','" + (e.Row.RowIndex + 1).ToString() + "'); SIMA.GridView.Extended.OnEventClickChangeColor(this);" + this.ClientID + @"_OnRowClick('" + e.Row.RowIndex + "');");
                    e.Row.Attributes.Add("onclick", "javascript:var otxtNroReg=jNet.get('" + this.ClientID + "_txtIdRegSelected'); otxtNroReg.value=jNet.get(this).attr('Guid'); SIMA.GridView.Extended.OnEventClickChangeColor(this);" + this.ClientID + @"_OnRowClick(jNet.get(this));");
                }
                else {
                        e.Row.Attributes.Add("onclick", "javascript:var otxtNroReg=jNet.get('" + this.ClientID + "_txtIdRegSelected'); otxtNroReg.value=jNet.get(this).attr('Guid'); SIMA.GridView.Extended.OnEventClickChangeColor(this);");
                }
                */

                /*if (e.Row.Attributes["TipoRow"] != "4")
                {
                    e.Row.Attributes.Add("TipoRow", "2");
                }*/
                /*---------------------*/
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
              
                if ((ShowFooter && e.Row.Cells.Count > 0)&& ((this.oEasyGridExtended.IdGestorFiltro!=null)||(this.oEasyGridExtended.IdGestorFiltro.Length>0)))
                {
                    int NroColSpan = e.Row.Cells.Count;
                    e.Row.Cells[0].ColumnSpan = NroColSpan;
                    for (int i = 1; i <= e.Row.Cells.Count-1 ; i++){e.Row.Cells[i].Visible = false;}

                    e.Row.Attributes.Add("TipoRow", "3");
                   
                    //Crear una tabla para los botones
                    e.Row.Cells[0].Controls.Add(BoxFooter());//agrega los botones en el footer

                }  
            }

           
        }
        void SetRowScriptFunctions(GridViewRow gvr) {
            if (ShowRowNumber)//Numeracion de registro
            {
                gvr.Cells[0].Style.Add("Width", "3%");
                gvr.Cells[0].Text = NroItem;
                gvr.Cells[0].Style.Add("text-decoration", "underline");
                gvr.Cells[0].Style.Add("cursor", "hand");
                gvr.Cells[0].Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), this.ClientID + "_OnDetalle(jNet.get(this.parentNode))");
            }
            gvr.Attributes.Add("onmouseover", "javascript:SIMA.GridView.Extended.OnEventMouseInOutChangeColor(this, true);");
            gvr.Attributes.Add("onmouseout", "javascript:SIMA.GridView.Extended.OnEventMouseInOutChangeColor(this, false);");
            if (this.EasyExtended.RowItemClick != null)
            {
                gvr.Attributes.Add("onclick", "javascript:var otxtNroReg=jNet.get('" + this.ClientID + "_txtIdRegSelected'); otxtNroReg.value=jNet.get(this).attr('Guid'); SIMA.GridView.Extended.OnEventClickChangeColor(this);" + this.ClientID + @"_OnRowClick(jNet.get(this));");
            }
            else
            {
                gvr.Attributes.Add("onclick", "javascript:var otxtNroReg=jNet.get('" + this.ClientID + "_txtIdRegSelected'); otxtNroReg.value=jNet.get(this).attr('Guid'); SIMA.GridView.Extended.OnEventClickChangeColor(this);");
            }
            //gvr.Attributes.Add("TipoRow", "2");
            if (gvr.Attributes["TipoRow"] != "4")
            {
                gvr.Attributes.Add("TipoRow", "2");
            }
        }

        public GridViewRow InsertGridRow(int Posicion,DataRow drItem) {
            GridViewRow dgr = new GridViewRow(Posicion, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
            dgr.Attributes.Add("TipoRow", "2");
            dgr.DataItem = drItem;
            //Clona las columnas para pa nueva fila
            for (int i = 0; i <= this.Columns.Count - 1; i++)
            {
                TableCell tc = new TableCell();
                dgr.Cells.Add(tc);
            }

            GridView dg = (GridView)this;
            Table t = (Table)dg.Controls[0];
                //eSTABLECE LOS EENTOS A NIVEL DE JS
                this.SetRowScriptFunctions(dgr);
            t.Rows.Add(dgr);
            //Crea el script de DataRow de lado del cliente
            if (dgr.DataItem != null) {
                DataRow drRowItem = (DataRow)dgr.DataItem;
                try {
                    DataColumn dc = new DataColumn("Guid", System.Type.GetType("System.String"));
                    drRowItem.Table.Columns.Add(dc);
                }
                catch (Exception ex) { }

                Guid myuuid = Guid.NewGuid();//Identificador unico que servira para relacionar la fila de la grilla con sus data en script
                string myuuidAsString = myuuid.ToString();
                drRowItem["Guid"] = myuuidAsString;
                dgr.Attributes["Guid"] = myuuidAsString;
                /*---------------------------------------------------------------------------------*/
                ScripTDataRow += "\n oDR_" + this.ClientID + " = oDT_" + this.ClientID + ".newRow();\n";
                ScripTDataRow += "\n   oDR_" + this.ClientID + "[" + Cmll + "Guid" + Cmll + "] = " + Cmll + myuuidAsString + Cmll + ";";
                ScripTDataRow += "\n   oDR_" + this.ClientID + "[" + Cmll + "Modo" + Cmll + "] = " + Cmll + "M" + Cmll + ";";


                DataTable dttmp = (DataTable)ViewState[DATA_COLLECION];
                DataRow drNew = dttmp.NewRow();

                foreach (DataColumn dcn in drRowItem.Table.Columns)
                {
                    ScripTDataRow += "   oDR_" + this.ClientID + "[" + Cmll + dcn.ColumnName + Cmll + "] = '" + drRowItem[dcn.ColumnName].ToString().Replace("'", "").Replace(Enter.ToString(), "").Replace(Enter2.ToString(), "") + "';";
                    drNew[dcn.ColumnName] = drRowItem[dcn.ColumnName];
                }
                ScripTDataRow += "\n oDT_" + this.ClientID + ".Rows.Add(oDR_" + this.ClientID + ");\n";
                 
                 dttmp.Rows.Add(drNew);
                ViewState[DATA_COLLECION]= dttmp;
                // this.Controls.Add(new LiteralControl("\n <script>" + ViewState[DATA_TABLE_CLIENT].ToString() +"" + "</script>"));
            }


            return dgr;
        }
        public void LoadData()
        {
            string ctrlFiltro = "";
            if (this.EasyExtended.IdGestorFiltro != null) {
                ctrlFiltro = this.EasyExtended.IdGestorFiltro;
            }
            
            if (ctrlFiltro.Length > 0)
            {
                EasyGestorFiltro oGFiltro = (EasyGestorFiltro) Page.FindControl(ctrlFiltro);
                if (oGFiltro != null)
                {
                    this.LoadData(oGFiltro.getFilterString());
                }
                else {
                    this.LoadData("");
                }
            }
            else {
                this.LoadData("");
            }

        }
        public void LoadData(string CriterioFiltro)
        {
            DataTable dt = this.DataInterconect.GetDataTable();
            DataView dv = dt.DefaultView;

            if (CriterioFiltro.Length > 0)
            {
                dv.RowFilter = CriterioFiltro;
                this.DataSource = dv;
            }
            else
            {
                this.DataSource = dt;
            }
            this.DataBind();


        }




        HtmlTable BoxFooter() {
            HtmlTable otblBtn = EasyUtilitario.Helper.HtmlControlsDesign.CrearTabla(1, 3);
            otblBtn.ID = "FooterButtons";
            otblBtn.Attributes.Add("border", "0px");
            otblBtn.Attributes.Add("width", "100%");
            otblBtn.Rows[0].Cells[0].Attributes.Add("width", "80%");
            otblBtn.Rows[0].Cells[1].Attributes.Add("width", "20%");

            otblBtn.Rows[0].Cells[1].Align = "right";
            //Inicia la carga de controles en el footer
            //foreach (EasyGridButton oEasyGridButton in arrEasyGridButtons.Where(oBE => oBE.Ubicacion == EasyUtilitario.Enumerados.Ubicacion.Footer))
            foreach (EasyGridButton oEasyGridButton in this.EasyGridButtons.Where(oBE => oBE.Ubicacion == EasyUtilitario.Enumerados.Ubicacion.Footer))
            {
                HtmlGenericControl Btn = oEasyGridButton.Materialized("btn btn-light", "black");
                Btn.Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), this.ClientID + SCR_TOOLBAR_CLICK_ITEM + "(" + oEasyGridButton.ToString(true) + ");");

                otblBtn.Rows[0].Cells[1].Controls.Add(Btn);
            }
            //Integrando filtros
            if (this.EasyExtended.IdGestorFiltro != null)
            {
                string FnFooterBarFiltro = @"<script>
                                                (function(){window.setTimeout(" + this.ClientID + @"_Integrar_EasyFiltro_View,100);})();
                                                function " + this.ClientID + @"_Integrar_EasyFiltro_View(){
                                                        try{
                                                            var obj =jNet.get('" + this.EasyExtended.IdGestorFiltro + "_BarraFiltro" + @"');
                                                            var otblContext =jNet.get('" + this.ClientID + "_FooterButtons" + @"');
                                                            jNet.get(otblContext.rows[0].cells[0]).insert(obj);
                                                            obj.css('display','block');
                                                        }
                                                        catch(ex){
                                                        }
                                                }
                                              </script>";

                _Scripts.Add((new LiteralControl(FnFooterBarFiltro)));
            }

            return otblBtn;
        }

        protected void OnSorting(object sender, GridViewSortEventArgs e)
        {
            GetSortDirection(e.SortExpression);
            _txtNroRegSelect.Text = "0";
            _txtNroPag.Text = "0";
        }

        private void GetSortDirection(string column)
        {
            string sortDirection = "ASC";
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }
            setSorting(column, sortDirection);
        }


        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.PageIndex = e.NewPageIndex;
            _txtNroPag.Text = this.PageIndex.ToString();
            _txtNroRegSelect.Text = "0";
        }
        char Enter = (char)13;
        char Enter2 = (char)10;
        protected  void OnRowDataBound(object sender, GridViewRowEventArgs e) {//nueva implementacion para obtener el registeo seleccionado

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obtiene los registro de la pagina actual
                DataRowView drv = (DataRowView)e.Row.DataItem;
                DataRow dr = drv.Row;
                string NoDataFound = ((ShowRowNumber) ? e.Row.Cells[0].Text : e.Row.Cells[1].Text);

                if (e.Row.RowIndex== 0)//genera la definicion de la estructura del datatable como objeto de lado del cliente
                {
                    //Marcar la fila para que no sea tocada
                    e.Row.Attributes.Add("NoDelete", "No");

                    /*DataColumn dc = new DataColumn("bookmark", System.Type.GetType("System.Decimal"));
                    dtPorPag.Columns.Add(dc);*/
                    DataColumn dc;
                    try
                    {
                        dc = new DataColumn("Guid", System.Type.GetType("System.String"));
                        dtPorPag.Columns.Add(dc);
                    
                        /*--------------------------------------------------------------------------------*/
                        /*ScriptDataTableDefDC += " oDC_" + this.ClientID + " = new SIMA.Data.DataColumn(" + Cmll + "bookmark" + Cmll + ");"
                                                                 + " oDT_" + this.ClientID + ".Columns.Add(oDC_" + this.ClientID + ");";*/
                        ScriptDataTableDefDC += " oDC_" + this.ClientID + " = new SIMA.Data.DataColumn(" + Cmll + "Guid" + Cmll + ");"
                                                               + " oDT_" + this.ClientID + ".Columns.Add(oDC_" + this.ClientID + ");";
                        foreach (DataColumn dcn in dr.Table.Columns)
                        {
                            dc = new DataColumn(dcn.ColumnName, dcn.DataType);
                            dtPorPag.Columns.Add(dc);
                            /*---------------------------------------------------------------------------------------------------*/
                            ScriptDataTableDefDC += " oDC_" + this.ClientID + " = new SIMA.Data.DataColumn(" + Cmll + dcn.ColumnName + Cmll + ");"
                                                                     + " oDT_" + this.ClientID + ".Columns.Add(oDC_" + this.ClientID + ");";
                        }

                    }
                    catch (Exception ex)
                    {
                        int x = 0;
                    }
                    //Tipo de columnas de la grilla
                    /*int idx = 0;
                    try
                    {
                        string JsBoundColumn = "";
                        string objDataFields = "";
                        
                        if ((this.Columns != null) && (this.Columns.Count > 0))
                        {
                            foreach (var col in this.Columns)
                            {
                                if (col.GetType() == typeof(BoundField))
                                {
                                    BoundField oBoundField = (BoundField)col;
                                    if ((oBoundField.DataField != null) && (oBoundField.DataField.Length > 0)){
                                        JsBoundColumn = "{Tipo:" + Cmll + "BoundField" + Cmll + ",DataField:" + Cmll + oBoundField.DataField + Cmll + ",Value:" + Cmll + dr[oBoundField.DataField] + Cmll + "}";
                                    }
                                }
                                else if (col.GetType() == typeof(TemplateField))
                                {
                                    TemplateField oTemplateField = (TemplateField)col;
                                    JsBoundColumn = "{Tipo:" + Cmll + "TemplateField" + Cmll + ",DataField:" + Cmll + "" + Cmll + ",Value:" + Cmll + "" + Cmll + "}";
                                }
                                objDataFields += ((idx==0)?"":",") +  JsBoundColumn;
                                idx++;
                            }
                            this.Attributes.Add("DataBoundColumns", "[" + objDataFields + "]");
                        }
                    }
                    catch (Exception ex)
                    {
                        e.Row.Cells[idx].Attributes["style"] = "background-color:powderblue;";
                        int i = 0;
                        // _tbSearch.Text = ex.Message.ToString();
                    }*/

                }

                DataRow drow = dtPorPag.NewRow();
               // drow["bookmark"] = NroItem;
                Guid myuuid = Guid.NewGuid();//Identificador unico que servira para relacionar la fila de la grilla con sus data en script
                string myuuidAsString = myuuid.ToString();
                drow["Guid"] = myuuidAsString;
                e.Row.Attributes["Guid"] = myuuidAsString;
                /*---------------------------------------------------------------------------------*/
                ScripTDataRow += "\n oDR_" + this.ClientID + " = oDT_" + this.ClientID + ".newRow();\n";
                //registra el Nro de Fila 
                //ScripTDataRow += "\n   oDR_" + this.ClientID + "[" + Cmll + "bookmark" + Cmll + "] = " + Cmll + NroItem + Cmll + ";";
                //Guid
                ScripTDataRow += "\n   oDR_" + this.ClientID + "[" + Cmll + "Guid" + Cmll + "] = " + Cmll + myuuidAsString + Cmll + ";";
                ScripTDataRow += "\n   oDR_" + this.ClientID + "[" + Cmll + "Modo" + Cmll + "] = " + Cmll + "M" + Cmll + ";";


              

                foreach (DataColumn dcn in dr.Table.Columns)
                {
                    drow[dcn.ColumnName] = dr[dcn.ColumnName];
                    /*-----------------------------------------------------------------------*/
                    ScripTDataRow += "   oDR_" + this.ClientID + "[" + Cmll + dcn.ColumnName + Cmll + "] = '" + dr[dcn.ColumnName].ToString().Replace("'","").Replace(Enter.ToString(), "").Replace(Enter2.ToString(), "") + "';";

                }
                dtPorPag.Rows.Add(drow);
                /*---------------------------------------------*/
                ScripTDataRow += "\n oDT_" + this.ClientID + ".Rows.Add(oDR_" + this.ClientID + ");\n";
                if ((e.Row.RowIndex+1) <= this.PageSize)// almacena solo la cantidad de registros delimitado por el Nro de registros en la paginacion
                {
                    ViewState[DATA_COLLECION] = dtPorPag;
                }
                //Establece el atributo del modo de registro
                e.Row.Attributes["MODO"] = "M";


                if (NoDataFound == NO_DATA_FOUND)
                {
                   
                    e.Row.Cells[0].ColumnSpan = e.Row.Cells.Count ;
                    e.Row.Cells[0].Text = NO_DATA_FOUND;
                    e.Row.Attributes.Add("NoDelete", "No");
                    e.Row.Cells[0].Attributes.Add(EasyUtilitario.Enumerados.EventosJavaScript.onclick.ToString(), "");
                    for (int col = 1; col <= e.Row.Cells.Count-1; col++)
                    {
                        e.Row.Cells[col].Visible = false;
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer) {
              
            }
        }

        int ConData = 0;
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
          
            if (ConData == 0) {
                this.AllowSorting = false;
                this.AllowPaging = false;

                //DataControlField[] tempDataControlField = new DataControlField[this.Columns.Count];
                bool ColumnField = false;
                DataTable dt = new DataTable();
                BoundField oBoundField;
                foreach (DataControlField field in this.Columns)
                {
                    if (field.GetType() == typeof(BoundField))
                    {
                        oBoundField = (BoundField)field;
                        dt.Columns.Add(new DataColumn(oBoundField.DataField, typeof(String)));
                        ColumnField = true;
                    }
                }
                if (ColumnField == true)
                {
                    DataRow workRow = dt.NewRow();
                    foreach (DataControlField field in this.Columns)
                    {
                        if (field.GetType() == typeof(BoundField))
                        {
                            oBoundField = (BoundField)field;
                            if (oBoundField.DataField.Length >0)
                            {
                                workRow[oBoundField.DataField] = NO_DATA_FOUND;
                            }
                        }
                    }
                    dt.Rows.Add(workRow);
                    this.DataSource = dt;
                    this.DataBind();
                }
            }
           
        }

        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
        #endregion
          
       

        /*Para la Agrupacion de los registro merge rows*/
        #region Merege Rows
        protected override void OnDataBound(EventArgs e)
        {
            base.OnDataBound(e);
            int ColMerg = 0, Grp = 0;
            if (!IsDesign())
            {
                if (this.ShowRowNumber)
                {
                    Grp = this.EasyRowGroup.GroupedDepth + 1;
                    this.EasyRowGroup.GroupedDepth = Grp;
                    ColMerg = this.EasyRowGroup.ColIniRowMerge + 1;//No permitira que se ejecute con la columna 0
                }
            this.SpanCellsRecursive(ColMerg, 0, this.Rows.Count);
            }
        }

        private void SpanCellsRecursive(int columnIndex, int startRowIndex, int endRowIndex)
        {
             if (columnIndex >= (this.EasyRowGroup.GroupedDepth) || columnIndex >= this.Columns.Count)
                    return;

            TableCell groupStartCell = null;
            int groupStartRowIndex = startRowIndex;

            for (int i = startRowIndex; i < endRowIndex; i++)
            {
                TableCell currentCell = this.Rows[i].Cells[columnIndex];
                bool isNewGroup = (null == groupStartCell) || (0 != String.CompareOrdinal(currentCell.Text, groupStartCell.Text));

                if (isNewGroup)
                {
                    if (null != groupStartCell)
                    {
                        SpanCellsRecursive(columnIndex + 1, groupStartRowIndex, i);
                    }

                    groupStartCell = currentCell;
                    groupStartCell.RowSpan = 1;
                    groupStartRowIndex = i;
                }
                else
                {
                    currentCell.Visible = false;
                    groupStartCell.RowSpan += 1;
                }
            }
            SpanCellsRecursive(columnIndex + 1, groupStartRowIndex, endRowIndex);
        }



        protected override void Render(HtmlTextWriter writer)
        {
            Attributes.Remove("align");
            base.Render(writer);
                      
            #region Efectos de Paginacion
                string ScriptPaginacionStyle = @"SIMA.GridView.Extended.Paginacion.Apply('" + this.ClientID + "', '');";
                (new LiteralControl("<script>" + ScriptPaginacionStyle + "</script>")).RenderControl(writer);
            #endregion


            string NomTextRegSele = this.ClientID + "_txtIdRegSelected";
            string NomRowIndexSele = this.ClientID + "_txtRowIndex";

            //el valor de la propiedad this.RowItemClick  como evento debe de existir de lo contrario llamara al evento de boton btnIrDetalle y har'a un postBack de lado del servidor al evento asociado
            if (!IsDesign())
            {
                string scriptOnClickRow = "";
                   string StamentSeccion = "";
                if (this.EasyExtended.RowCellItemClick != null){
                    StamentSeccion = ((this.EasyExtended.RowCellItemClick.Length > 0) ? this.EasyExtended.RowCellItemClick + "(ItemRowBE);" : "SIMA.Utilitario.Helper.Wait('Redireccionando a...', 0, function () { });" + this.ClientID + ".PostBack();");
                }
                scriptOnClickRow = @"function " + this.ClientID + @"_OnDetalle(otr) {
                                                                 //SIMA.Utilitario.Helper.Wait('Titulo',1000,function(){
                                                                      var oDataRow =   ((oDT_" + this.ClientID + @"==null)?new Array():oDT_" + this.ClientID + @".Select('Guid', '=', otr.attr('Guid')));
                                                                      var ItemRowBE =oDataRow[0];
                                                                      $('#" + NomTextRegSele + @"').val(otr.attr('Guid'));
                                                                      $('#" + NomRowIndexSele + @"').val(otr.rowIndex);
                                                                   " + StamentSeccion + @" 
                                                                 //});
                                                   }";

                /*string scriptOnClickRow = @"function " + this.ClientID + @"_OnDetalle(otr) {
                                                                var oDataRow =   ((oDT_" + this.ClientID + @"==null)?new Array():oDT_" + this.ClientID + @".Select('Guid', '=', otr.attr('Guid')));
                                                                var ItemRowBE =oDataRow[0];
                                                                    $('#" + NomTextRegSele + @"').val(otr.attr('Guid'));
                                                                    $('#" + NomRowIndexSele + @"').val(otr.rowIndex);

                                                                if(" + (this.EasyExtended.RowCellItemClick.Length + @">0){
                                                                    SIMA.Utilitario.Helper.Wait('Titulo',1000,function(){
                                                                      " + StamentSeccion + @" 
                                                                    });
                                                                }
                                                                else{
                                                                    SIMA.Utilitario.Helper.Wait('Redireccionando a...', 0, function () { });
                                                                     " + StamentSeccion + @"
                                                                }
                                                  }";*/

                (new LiteralControl("<script>" + ArrDataBE + "" + scriptOnClickRow + "</script>")).RenderControl(writer);

                StamentSeccion = "";
                /*************************************************************************************************/
                if (this.EasyExtended.RowItemClick != null)
                {
                    StamentSeccion = ((this.EasyExtended.RowItemClick.Length>0)? this.EasyExtended.RowItemClick + "(ItemRowBE);":"");
                    //Adicionado el 27-10-2023 solo para uso de la seleccion de una fila
                    scriptOnClickRow = @"function " + this.ClientID + @"_OnRowClick(otr) {
                                                                           var oDataRow =   ((oDT_" + this.ClientID + @"==null)?new Array():oDT_" + this.ClientID + @".Select('Guid', '=', otr.attr('Guid')));
                                                                           var ItemRowBE =oDataRow[0];
                                                                             $('#" + NomTextRegSele + @"').val(otr.attr('Guid'));
                                                                             $('#" + NomRowIndexSele + @"').val(otr.rowIndex);
                                                                             " + StamentSeccion + @"
                                                      }";


                    (new LiteralControl("<script>" + ArrDataBE + "" + scriptOnClickRow + "</script>")).RenderControl(writer);
                }

                /*************************************************************************************************/

            }

            if (NroItem != null)
            {
                ScriptDataTableDef = @"var oDT_" + this.ClientID + @"= new SIMA.Data.DataTable();
                                              var oDC_" + this.ClientID + @"=null;
                                              var oDR_" + this.ClientID + @" = null;" + "\n";

                ViewState[DATA_TABLE_CLIENT] = ScriptDataTableDef + ScriptDataTableDefDC + ScripTDataRow;//este script se registra en el evento RENDER

                (new LiteralControl("\n <script>" + ViewState[DATA_TABLE_CLIENT].ToString() + "</script>")).RenderControl(writer);
            }
            //Clonar Fila
           

            string strRowsCount = this.ClientID + @".RowCount=function(){
                                                         var ogridView = jNet.get('" + this.ClientID + @"');
                                                         var rows = ogridView.querySelectorAll('[TipoRow=" + Cmll + "2" + Cmll + @"]');
                                                        return rows.length;
                                                    }
                                                ";
            _Scripts.Add(new LiteralControl("<script>" + strRowsCount + "</script>"));


            string strClonRow = this.ClientID + @".RowClone=function(rowIndexClone,onClone){
                                                                var row=null;
                                                                    var UID = generateUUID();
                                                                    var oDataRowNew = oDT_" + this.ClientID + @".newRow();
                                                                    oDataRowNew[" + Cmll + "Guid" + Cmll + @"]= UID;
                                                                    oDataRowNew[" + Cmll + "Modo" + Cmll + @"]='N';
                                                                    oDT_" + this.ClientID + @".Rows.Add(oDataRowNew);

                                                                    var grid = jNet.get('" + this.ClientID + @"');
                                                                    var rowBase =grid.rows[rowIndexClone];
                                                                    var TipoRow = rowBase.getAttribute('TipoRow');
                                                                    if(TipoRow=='4'){
                                                                        row = jNet.get(rowBase);
                                                                        " + ((this.ShowRowNumber == true) ? "row.cells[0].innerText='1';" : "") + @"
                                                                        row.attr('TipoRow', '2');
                                                                    }
                                                                    else{
                                                                        row = jNet.get(rowBase.cloneNode(true));
                                                                        row.removeAttribute('NoDelete');
                                                                        row.attr('TipoRow', '2');
                                                                        $(" + Cmll + "#" + this.ClientID + Cmll + @").append(row);
                                                                    }
                                                                    row.attr('Modo','N');
                                                                    row.attr('Guid',UID);
                                                                    row.GetData=function(){
                                                                                     return " + this.ClientID + @".GetDataRow(this.attr('Guid'));
                                                                                  }
                                                                    
                                                                    if(Mod(grid.rows.length)==0){
                                                                            row.attr('class','ItemGrilla');
                                                                    }
                                                                    else{
                                                                            row.attr('class','AlternateItemGrilla');
                                                                    }
                                                                    //Asignar evento
                                                                    row.addEvent('mouseover', function(){SIMA.GridView.Extended.OnEventMouseInOutChangeColor(this, true);});
                                                                    row.addEvent('mouseout', function(){SIMA.GridView.Extended.OnEventMouseInOutChangeColor(this, false);});
                                                                    //row.addEvent('click',function(){var _guid=this.attr('Guid'); var otxtIdReg= jNet.get('" + this.ClientID + @"_txtIdRegSelected');otxtIdReg.value=_guid;var otxtRowIndex= jNet.get('" + this.ClientID + @"_txtRowIndex');otxtRowIndex.value=this.rowIndex; SIMA.GridView.Extended.OnEventClickChangeColor(this);});
                                                                    row.addEvent('click',function(){" + this.ClientID + @"_OnRowClick(this);SIMA.GridView.Extended.OnEventClickChangeColor(this);});
                                                                    //Elimina el contenido de la celda
                                                                    $(row).find('td').each(function(index,oCell){
                                                                        while (oCell.firstChild) {
                                                                            oCell.removeChild(oCell.firstChild);
                                                                        }
                                                                    });
                                                                    //Llama al metodo externo
                                                                     $(row).find('td').each(function(index,oCell){
                                                                        onClone(jNet.get(oCell),index);
                                                                    });
                                                                return row;
                                                            }";

            _Scripts.Add(new LiteralControl("<script>" + strClonRow + "</script>"));

            //Insertar Filas en ubicaciones 
            string strInsertRow = this.ClientID + @".InsertRow=function(RowPosition,onClone){
                                                                var oNewRow = null;
                                                                    var UID = generateUUID();
                                                                    var oDataRowNew = oDT_" + this.ClientID + @".newRow();
                                                                    oDataRowNew[" + Cmll + "Guid" + Cmll + @"]= UID;
                                                                    oDataRowNew[" + Cmll + "Modo" + Cmll + @"]='N';
                                                                    oDT_" + this.ClientID + @".Rows.Add(oDataRowNew);

                                                                    var grid = jNet.get('" + this.ClientID + @"');
                                                                    var orowsBase= grid.querySelectorAll('tr[TipoRow=" + Cmll + "1" + Cmll + @"]');
                                                                    var orBase=orowsBase[orowsBase.length-1];
                                                                    oNewRow = jNet.get(grid.insertRow(RowPosition));
                                                                    oNewRow.attr('TipoRow', '2');
                                                                    oNewRow.attr('Modo','N');
                                                                    oNewRow.attr('Guid',UID);
                                                                    oNewRow.css('height','35px');
                                                                    oNewRow.GetData=function(){return " + this.ClientID + @".GetDataRow(this.attr('Guid'));}

                                                                    if(Mod(oNewRow.rowIndex)==0){
                                                                            oNewRow.attr('class','ItemGrilla');
                                                                    }
                                                                    else{
                                                                            oNewRow.attr('class','AlternateItemGrilla');
                                                                    }
                                                                    oNewRow.addEvent('mouseover', function(){SIMA.GridView.Extended.OnEventMouseInOutChangeColor(this, true);});
                                                                    oNewRow.addEvent('mouseout', function(){SIMA.GridView.Extended.OnEventMouseInOutChangeColor(this, false);});
                                                                    //oNewRow.addEvent('click',function(){var _guid=this.attr('Guid'); var otxtIdReg= jNet.get('" + this.ClientID + @"_txtIdRegSelected');otxtIdReg.value=_guid; SIMA.GridView.Extended.OnEventClickChangeColor(this);});
                                                                    oNewRow.addEvent('click',function(){" + this.ClientID + @"_OnRowClick(this);SIMA.GridView.Extended.OnEventClickChangeColor(this);});
                                                                    $(orBase).find('th').each(function(index,oCell){
                                                                        oNewRow.insertCell(index);
                                                                        var _CellNew = jNet.get(oNewRow.cells[index]);
                                                                        onClone(_CellNew,index);
                                                                    });
                                                                
                                                                return oNewRow;
                                                            }";

            _Scripts.Add(new LiteralControl("<script>" + strInsertRow + "</script>"));


            string strClearRow = this.ClientID + @".Clear=function(){
                                                                var grid = jNet.get('" + this.ClientID + @"');
                                                                var i=0;rowIndexIni =0;
                                                                " + this.ClientID + @".ItemsforEach(function(orow){
                                                                            var oDataR = orow.GetData();
                                                                            oDataR['Modo']='E';

                                                                            if(i==0){
                                                                                rowIndexIni=orow.rowIndex;//Limpiar la celdas de la fila por default
                                                                                  $(orow).find('td').each(function(index,oCell){
                                                                                                while (oCell.firstChild) {
                                                                                                    oCell.removeChild(oCell.firstChild);
                                                                                                }
                                                                                  });
                                                                                orow.attr('TipoRow', '4');
                                                                            }
                                                                            else{
                                                                                grid.deleteRow(rowIndexIni+1);
                                                                            }
                                                                          i++;
                                                                    });
                                                        }
                                    ";
            _Scripts.Add(new LiteralControl("<script>" + strClearRow + "</script>"));





            //Recorrido de filar
            //string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string strRowBound = this.ClientID + @".ItemsforEach=function(onRowBound){
                                                                var grid = jNet.get('" + this.ClientID + @"');
                                                                var rows = grid.querySelectorAll('[TipoRow=" + Cmll + "2" + Cmll + @"]'); 
                                                                rows.forEach(function(row,idx){
                                                                                var _row = jNet.get(row);
                                                                                    _row.GetData=function(){
                                                                                                     return " + this.ClientID + @".GetDataRow(this.attr('Guid'));
                                                                                                  }
                                                                                    _row.addEvent('click',function(){
                                                                                                                " + this.ClientID + @"_OnRowClick(this);
                                                                                                            });
                                                                                    onRowBound(_row,idx);
                                                                                }
                                                                            );

                                                            }
                                                   ";

           
            _Scripts.Add(new LiteralControl("<script>" + strRowBound + "</script>"));

            string strNtoFila = this.ClientID + @".GetNroFila=function(){
                                                                       var NroFilas=0; "
                                                                       + this.ClientID + @".ItemsforEach(function (orow) {
                                                                                                            NroFilas=orow.rowIndex;
                                                                                                        }
                                                                                                    );
                                                                    return NroFilas;
                                                              }
                                     ";

            _Scripts.Add(new LiteralControl("<script>" + strNtoFila + " </script>"));


            string strgetRowActivo = this.ClientID + @".GetRowActive=function(){
                                                                        var oRowActive = null;
                                                                        var oTxtNroReg = jNet.get('" + this.ClientID + @"_txtIdRegSelected');
                                                                        var GuidActive = oTxtNroReg.value;
                                                                        if((GuidActive!=null)||(GuidActive!=0)){
                                                                            oRowActive = " + this.ClientID + @".RowFind(GuidActive);
                                                                            oRowActive.GetData=function(){
                                                                                oDataRow = ((oDT_" + this.ClientID + @"==null)?new Array():oDT_" + this.ClientID + @".Select('Guid', '=', this.attr('Guid')));
                                                                                return oDataRow[0];
                                                                            }
                                                                        }
                                                                        return oRowActive;
                                                              }
                                     ";

            _Scripts.Add(new LiteralControl("<script>" + strgetRowActivo + " </script>"));

            //find row
            string strRowFind = this.ClientID + @".RowFind=function(Guid){
                                                                var RowReturn=null;
                                                                " + this.ClientID + @".ItemsforEach(function(row){
                                                                        //var GuidRow = jNet.get(row).attr('Guid');
                                                                        var GuidRow = row.attr('Guid');
                                                                        if(GuidRow==Guid){
                                                                            RowReturn=row;
                                                                        }
                                                                });
                                                               return jNet.get(RowReturn); 
                                                            }";


            _Scripts.Add(new LiteralControl("<script>" + strRowFind + "</script>"));

            /*Tratamiento del DataRow de lado del cliente 26-09-2023*/
            //se modifico: 27-12-2023: oNroReg = $('#" + this.ClientID + @"_txtIdRegSelected').val();
            string strGetDataRow = this.ClientID + @".GetDataRow = function (_Guid) {
                                                                        var oDataRow =null;
                                                                        if(_Guid==undefined){
                                                                             _Guid = jNet.get('" + this.ClientID + @"_txtIdRegSelected').value;
                                                                        }
                                                                        oDataRow = ((oDT_" + this.ClientID + @"==null)?new Array():oDT_" + this.ClientID + @".Select('Guid', '=', _Guid));
                                                                        return oDataRow[0];
                                                                    }
                                                  ";
            _Scripts.Add(new LiteralControl("<script>" + strGetDataRow + "</script>"));
            string strSetDataRow = this.ClientID + @".SetDataRow = function (oDataRow) {
                                                                        //Verifica si dicha fila existe
                                                                        var oArrRows = oDT_" + this.ClientID + @".Select('Guid','=',oDataRow['Guid']);
                                                                        if((oArrRows==null)&&(oArrRows.length==0)){
                                                                            oDT_" + this.ClientID + @".Rows.Add(oDataRow); 
                                                                        }
                                                                    }
                                                  ";
            _Scripts.Add(new LiteralControl("<script>" + strSetDataRow + "</script>"));




            //Ejecutar RowPostBack
            string strRowPostBack = this.ClientID + @".PostBack=function(){
                                                                     $('#" + btnIrDetalle.ClientID + @"').click();
                                                            }";



            _Scripts.Add(new LiteralControl("\n<script>" + strRowPostBack + "</script>"));
            //


            //string cmll = EasyUtilitario.Constantes.Caracteres.ComillaDoble;
            string VerificaMetodoScript = (((this.ToolBarButtonClick != null) || (this.ToolBarButtonClick.Length > 0)) ? this.ToolBarButtonClick + "(btnItem,ItemRowBE);" : "alert('No se ha definido funcion script para esta propiedad: ToolBarButtonClick');");
            string ExecServerOrClient = @"if(btnItem.RunAtServer.toLowerCase()=='true'){
                                                if(" + (((this.fncExecBeforeServer != null)&& (this.fncExecBeforeServer.Length>0)) ? this.fncExecBeforeServer + @"(btnItem,ItemRowBE)" : "true") + @"==true){ 
                                                    if(btnItem.SilenceWait.toLowerCase()=='false'){//Simula Carga 17/10/2025
                                                       SIMA.Utilitario.Helper.Wait('Redireccionando a...', 0, function () { });
                                                    }
                                                    __doPostBack('" + oBtn.UniqueID + @"',btnItem.Id);
                                                }
                                           }
                                           else{
                                                if(btnItem.SilenceWait.toLowerCase()=='false'){//Simula Carga 17/10/2025
                                                                SIMA.Utilitario.Helper.Wait('Extendiendo',1000,function(){
                                                                                                    " + VerificaMetodoScript + @"
                                                                                                });
                                                }
                                                else{
                                                   " + VerificaMetodoScript + @"
                                                }
                                           }";

            string _SolicitaConfirmacion = @"
                                                $.confirm({title: '',
                                                            content: btnItem.MsgConfirm,
                                                            icon: 'fa fa-user-times',
                                                            animation: 'scale',
                                                            closeAnimation: 'scale',
                                                            opacity: 0.5,
                                                            buttons: {
                                                                'confirm': {
                                                                    text: 'Aceptar',
                                                                    btnClass: 'btn-blue',
                                                                    action: function () {
                                                                                " + ExecServerOrClient + @"
                                                                            }
                                                                },
                                                                cancel: function () {
                                                                },
                    
                                                            }
                                                        });";
            string _ScriptToolBarOnclick = @"<script>
                                                        function " + this.ClientID + SCR_TOOLBAR_CLICK_ITEM + @"(btnItem){
                                                            var otxt = jNet.get('" + this.ClientID + "_txtIdRegSelected" + @"');
                                                            //var oIdReg= otxt.value.replace(' ','');
                                                            var oIdReg= otxt.value;
                                                            var ItemRowBE = null;
                                                            var oDataRow =   ((oDT_" + this.ClientID + @"==null)?new Array():oDT_" + this.ClientID + @".Select('Guid', '=', oIdReg));
                                                            ItemRowBE =oDataRow[0];

                                                            if(btnItem.RequiereSelecciondeReg.toLowerCase()=='true'){
                                                                       if(oIdReg.length==0){
                                                                            $.alert({title: 'Error',
                                                                                        icon: 'fa fa-warning fa-4',
                                                                                        type: 'red',
                                                                                        closeIcon: true,
                                                                                        content: 'Seleccionar un registro, para concretar esta operación',
                                                                                        });
                                                                             return;
                                                                        }
                                                                      if(btnItem.SolicitaConfirmar.toLowerCase()=='true'){
                                                                         " + _SolicitaConfirmacion + @"
                                                                      }
                                                                      else{
                                                                            " + ExecServerOrClient + @"
                                                                      }
                                                            }
                                                            else{
                                                                  if(btnItem.SolicitaConfirmar.toLowerCase()=='true'){
                                                                        " + _SolicitaConfirmacion + @"
                                                                  }
                                                                  else{
                                                                    " + ExecServerOrClient + @"
                                                                  }
                                                            }
                                                            
                                                            
                                                        }
                                             </script>";


            _Scripts.Add(new LiteralControl(_ScriptToolBarOnclick));



            string strRowDelete = this.ClientID + @".DeleteRowActive=function(EraseLogical){
                                                                try{
                                                                  var grid = jNet.get('" + this.ClientID + @"');
                                                                  var _Guid = jNet.get('" + _txtNroRegSelect.ClientID + @"').value;
                                                                  var rowReturn = " + this.ClientID + @".RowFind(_Guid);
                                                                  var rowExiste = jNet.get(rowReturn);
                                                                  if(rowExiste.attr('Modo')=='E'){
                                                                                $.alert({title: 'Error',
                                                                                        icon: 'fa fa-exclamation-triangle',
                                                                                        type: 'red',
                                                                                        closeIcon: true,
                                                                                        content: 'Registro ya se encuentra Eliminado',
                                                                                        });

                                                                  }
                                                                  else{
                                                                            var oDataR = rowReturn.GetData();
                                                                            oDataR['Modo']='E';
                                                                            rowExiste.attr('Modo','E');
                                                                            if(EraseLogical==true){
                                                                                for(var i = 0; i < rowExiste.cells.length; i++) {
                                                                                    var cell = jNet.get(rowExiste.cells[i]);
                                                                                    cell.css('color','red');
                                                                                    cell.css('text-decoration','line-through');
                                                                                }
                                                                            }
                                                                            else{
                                                                                grid.deleteRow(rowExiste.rowIndex);                                                                 
                                                                            }
                                                                  }
                                                                }
                                                                catch(ex){
                                                                    alert(ex);
                                                                }
                                                            }";

            _Scripts.Add(new LiteralControl("\n<script>" + strRowDelete + "</script>"));


            string strRowDeletes = this.ClientID + @".DeleteRow=function(_Guid,Logico){
                                                                var UID =null;
                                                                try{
                                                                      var grid = jNet.get('" + this.ClientID + @"');
                                                                      var rowReturn = " + this.ClientID + @".RowFind(_Guid);
                                                                      var rowExiste = jNet.get(rowReturn);
                                                                      if(rowExiste.attr('Modo')=='E'){
                                                                                    $.alert({title: 'Error',
                                                                                            icon: 'fa fa-exclamation-triangle',
                                                                                            type: 'red',
                                                                                            closeIcon: true,
                                                                                            content: 'Registro ya se encuentra Eliminado',
                                                                                            });
                                                                        }
                                                                        else{
                                                                            var oDataR = rowReturn.GetData();
                                                                                oDataR['Modo']='E';
                                                                                if(jNet.get(rowReturn).attr('NoDelete')){
                                                                                        $(rowReturn).find('td').each(function(index,oCell){
                                                                                                                            while (oCell.firstChild) {
                                                                                                                                oCell.removeChild(oCell.firstChild);
                                                                                                                            }
                                                                                                                });
                                                                                        rowReturn.attr('TipoRow', '4');
                                                                                        UID = generateUUID();
                                                                                            var oDataRowNew = oDT_" + this.ClientID + @".newRow();
                                                                                            oDataRowNew[" + Cmll + "Guid" + Cmll + @"]= UID;
                                                                                            oDataRowNew[" + Cmll + "Modo" + Cmll + @"]='N';
                                                                                            oDT_" + this.ClientID + @".Rows.Add(oDataRowNew);
                                                                                        rowReturn.attr('Guid',UID);
                                                                                }
                                                                                else{
                                                                                        UID=_Guid;
                                                                                        grid.deleteRow(rowExiste.rowIndex);                                                                 
                                                                                }
                                                                        }
                                                                       // return UID;
                                                                }
                                                                catch(ex){
                                                                    alert(ex);
                                                                }
                                                            }";

            _Scripts.Add(new LiteralControl("\n<script>" + strRowDeletes + "</script>"));





            string strQuerySelector = this.ClientID + @".querySelector=function(Filtro,fncLoop){
                                                                  var AllCtrl = document.getElementById('" + this.ClientID + @"').querySelectorAll('[' + Filtro + ']');
                                                                  Array.prototype.forEach.call(AllCtrl, function(el){
                                                                                                            fncLoop(jNet.get(el));
                                                                                                          }); 
                                                            }";

            _Scripts.Add(new LiteralControl("\n<script>" + strQuerySelector + "</script>"));






            /*Los nuevos Script*/
            foreach (LiteralControl ScriptLC in _Scripts) {
                ScriptLC.RenderControl(writer);
            }
            //
          

        }

        public string getPagina() {
            return _txtNroPag.Text;
        }
        public void setPagina(string NroPagina)
        {
            _txtNroPag.Text = NroPagina;
        }

        string  SaveSorting(string SE, string SD) {
            ViewState["SortExpression"] = SE;
            ViewState["SortDirection"] = SD;

            _txtSortField.Text = (string)ViewState["SortExpression"];
            _txtSortAscDesc.Text = (string)ViewState["SortDirection"];

            string ResultSorted = _txtSortField.Text + " " + _txtSortAscDesc.Text;
            return ((ResultSorted.Replace(" ", "").Length == 0) ? "" : ResultSorted);
        }

        public string getSorting(int i)
        {
            string strResult= "";
            switch (i) {
                case 0:
                    strResult = _txtSort.Text;
                    break;
                case 1:
                    strResult = _txtSortField.Text;
                    break;
                case 2:
                    strResult = _txtSortAscDesc.Text;
                    break;
            }
            return strResult;
        }
        public string getNroRegSelect() {
            return _txtNroRegSelect.Text;
        }
        public void setNroRegSelect(string NroReg)
        {
            _txtNroRegSelect.Text= NroReg;
        }
        public string getRowIndex() { 
            return _txtRowIndex.Text;   
        }
        public void setRowIndex(string Index){
            _txtRowIndex.Text= Index;
        }

        public string getSorting() {
            return getSorting(0);
        }
      

        public void setSorting(string SE, string SD)
        {
             _txtSort.Text = SaveSorting(SE, SD);
        }

        /* public Dictionary<string, string> getDataItemSelected() {
             Dictionary<string, string> Data = new Dictionary<String, string>();
             if (_txtNroRegSelect.Text.Length > 0)
             {
                 //Data = FindRegSelected(Convert.ToInt32(_txtNroRegSelect.Text)-1);
                 Data = FindRegSelected(_txtNroRegSelect.Text);
             }
             return Data;
         }*/
        public Dictionary<string, string> getDataItemSelected()
        {
            Dictionary<string, string> Data = new Dictionary<String, string>();
            if (_txtNroRegSelect.Text.Length > 0)
            {
                Data = getData(_txtNroRegSelect.Text);
            }
            return Data;
        }


        private  Dictionary<string, string> getData(string _Guid)
        {
            Dictionary<string, string> Data = new Dictionary<String, string>();
                Data = FindRegSelected(_Guid);
            return Data;
        }
        public Dictionary<string, string> getDataRow(string _Guid)
        {
            return getData(_Guid);
        }

        #endregion









    }




}

