using EasyControlWeb.Form.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.Design; 
using System.Reflection;


namespace EasyControlWeb.Filtro
{

    public class EasyFiltroCollectionCriterio: CollectionEditor
    {
        public EasyFiltroCollectionCriterio(Type type) : base(type)
        {
        }
        protected override bool CanSelectMultipleInstances()
        {
            return true;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(EasyFiltroItem);
        }

    }

    //collecion para uso de campos de base de datos
    public class EasyFiltroCollectionCampo: CollectionEditor
    {
        public EasyFiltroCollectionCampo(Type type) : base(type)
        {
        }
        protected override bool CanSelectMultipleInstances()
        {
            return true;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(EasyFiltroCampo);
        }

    }

   

    //coleccion de parametros
    public class EasyFiltroCollectionParams : CollectionEditor
    {
        public EasyFiltroCollectionParams(Type type) : base(type)
        {
        }
        protected override bool CanSelectMultipleInstances()
        {
            return true;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(EasyFiltroParamURLws);
        }

    }


    /*Para La lista de controles relacionados */
    public class EasyCollectionNomControls : CollectionEditor
    {
        public EasyCollectionNomControls(Type type) : base(type)
        {
        }
        protected override bool CanSelectMultipleInstances()
        {
            return true;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(EasyControlBE);
        }

    }




}
