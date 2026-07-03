using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;

namespace DvStyle.OpenDataTable.Binders
{
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
