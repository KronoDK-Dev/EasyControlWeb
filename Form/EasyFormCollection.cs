using EasyControlWeb.Form.Controls;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace EasyControlWeb.Form
{
    public class EasyFormCollectionEditor
    {
        public class EasyFormoCollectionEditorGrupo : CollectionEditor
        {
            public EasyFormoCollectionEditorGrupo(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            /*protected override Type CreateCollectionItemType()
            {
                return typeof(EasyFormGrupo);
            }*/

        }

        //Nuevo
        /******valido para el nuevo objeto ESAYFORM*****************************************************************/
        public class EasyFormoCollectionEditorSeccion : CollectionEditor
        {
            public EasyFormoCollectionEditorSeccion(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyFormSeccion);
            }

        }
        /***********************************************************************/




        /// <summary>
        /// PROXIMO A ELIMINAR
        /// </summary>
        /*public class EasyFormoCollectionEditorItems : CollectionEditor
        {
            public EasyFormoCollectionEditorItems(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyFomItem);
            }

        }*/

        /// <summary>
        /// PROXIMO A ELIMINAR
        /// </summary>

        public class EasyFormCollectionEditorControls : CollectionEditor
        {
            public EasyFormCollectionEditorControls(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(System.Web.UI.WebControls.TextBox);
            }

        }

        public class EasyFormCollectionEditorMenuItems : CollectionEditor
        {
            public EasyFormCollectionEditorMenuItems(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyMenuItem);
            }

        }

        public class EasyFormCollectionEditorUsuarios : CollectionEditor
        {
            public EasyFormCollectionEditorUsuarios(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyUsuario);
            }

        }


        public class EasyFormoCollectionEditorMenuItem : CollectionEditor
        {
            public EasyFormoCollectionEditorMenuItem(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyNavigatorBarMenuBE);
            }

        }


        public class EasyFormoCollectionNavBarIcons : CollectionEditor
        {
            public EasyFormoCollectionNavBarIcons(Type type) : base(type)
            {
            }
            protected override bool CanSelectMultipleInstances()
            {
                return true;
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(EasyNavigatorBarIconBE);
            }

        }





    }
}