using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyControlWeb.Form.Base;

namespace EasyControlWeb.Filtro
{
    public class EasyFiltroCollectionCriterio : CollectionEditor
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

    public class EasyFiltroCollectionCampo : CollectionEditor
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
