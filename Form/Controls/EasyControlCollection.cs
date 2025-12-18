using EasyControlWeb.Form.Base;
using EasyControlWeb.Form.Controls.Cards;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Web.UI.WebControls;

namespace EasyControlWeb.Form.Controls
{ 
    public class EasyControlCollection
    {
        public class EasyFormCollectionHistorialEditor : CollectionEditor
        {
            public EasyFormCollectionHistorialEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyNavigatorBE);
            }

        }

        //Colleccion para el GridView
        public class EasyFormCollectionGridButtonEditor : CollectionEditor
        {
            public EasyFormCollectionGridButtonEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyGridButton);
            }

        }


        //Colleccion para el Menu Context
        public class EasyFormCollectionMnuContextButtonEditor : CollectionEditor
        {
            public EasyFormCollectionMnuContextButtonEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyButtonMenuContext);
            }

        }

        public class EasyFormCollectionListViewEditor : CollectionEditor
        {
            public EasyFormCollectionListViewEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyListItem);
            }

        }

        public class EasyCollectionListItemsEditor : CollectionEditor
        {
            public EasyCollectionListItemsEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyListViewItemTipo);
            }

        }

        

        public class EasyFormCollectionButtons : CollectionEditor
        {
            public EasyFormCollectionButtons(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyButton);
            }

        }

        public class EasyFormCollectionUpFilesEditor : CollectionEditor
        {
            public EasyFormCollectionUpFilesEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyFileInfo);
            }

        }

        /*Para el control Tabs*/

        public class EasyFormCollectionTabsItemEditor : CollectionEditor
        {
            public EasyFormCollectionTabsItemEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyTabItem);
            }

        }

        /*Para el control Paths History*/

        public class EasyFormCollectionPathsItemEditor : CollectionEditor
        {
            public EasyFormCollectionPathsItemEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyPathItem);
            }

        }

        

        /*Para el control Easy view Group log*/
        public class EasyFormCollectionGroupLogItemEditor : CollectionEditor
        {
            public EasyFormCollectionGroupLogItemEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyViewLogGroup);
            }

        }

        /*Para el control Easy view log*/
        public class EasyFormCollectionLogItemEditor : CollectionEditor
        {
            public EasyFormCollectionLogItemEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyItemLog);
            }

        }
        //Para los botones del ItemLog de la clase ViewLog
        public class EasyFormCollectionLogItemButtomEditor : CollectionEditor
        {
            public EasyFormCollectionLogItemButtomEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyViewLogButtom);
            }

        }



        /*Para el control Easy view Card*/
        public class EasyFormCollectionCardItemEditor : CollectionEditor
        {
            public EasyFormCollectionCardItemEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyCard);
            }

        }

        

        //Para los botones del ItemLog de la clase CardView
        public class EasyFormCollectionCardItemButtomEditor : CollectionEditor
        {
            public EasyFormCollectionCardItemButtomEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyCardButton);
            }

        }
        //Para el panel de  botones del Usuarios  CardView
        public class EasyFormCollectionCardUserButtomEditor : CollectionEditor
        {
            public EasyFormCollectionCardUserButtomEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyUserButton);
            }

        }
        //Contenido de card
        //
        public class EasyFormCollectionCardContenidoEditor : CollectionEditor
        {
            public EasyFormCollectionCardContenidoEditor(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyCardBaseContenido);
            }

        }
    }
}
