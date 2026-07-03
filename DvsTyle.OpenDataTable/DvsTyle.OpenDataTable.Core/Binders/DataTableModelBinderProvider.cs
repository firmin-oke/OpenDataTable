using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;

namespace DvStyle.OpenDataTable.Binders
{
    /// <summary>
    /// c'est un fournisseur de modèle de liaison pour le modèle DataTableServerRequestHeader.
    /// Il est utilisé pour fournir le DataTableModelBinder lorsque le type de modèle est DataTableServerRequestHeader.
    /// </summary>
    public class DataTableModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null){
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Metadata.ModelType == typeof(DataTableServerRequestHeader)){
                return new BinderTypeModelBinder(typeof(DataTableModelBinder));
            }
            return null;
        }
    }
}
