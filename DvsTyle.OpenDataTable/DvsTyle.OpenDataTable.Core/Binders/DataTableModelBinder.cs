using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DvStyle.OpenDataTable.TableDef;

namespace DvStyle.OpenDataTable.Binders
{
    /// <summary>
    /// Modèle de liaison pour les paramètres de traitement de données côté serveur.
    /// Il est utilisé pour lier les données de la requête HTTP aux propriétés du modèle DataTableServerRequestHeader.
    /// </summary>
    public class DataTableModelBinder : IModelBinder
    {
        /// <summary>
        /// Lie les données de la requête HTTP aux propriétés du modèle DataTableServerRequestHeader.
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var teste = bindingContext;
            var model = new DataTableServerRequestHeader
            {
                Draw = Convert.ToInt32(GetValue(bindingContext, "draw")),
                Start = Convert.ToInt32(GetValue(bindingContext, "start")),
                Length = Convert.ToInt32(GetValue(bindingContext, "length")),
                // Search
                Search = new DataTableSearch
                {
                    Value = GetValue(bindingContext, "search[value]"),
                    Regex = Convert.ToBoolean(GetValue(bindingContext, "search[regex]"))
                }
            };
            // Order
            var o = 0;
            var order = new List<DataTableOrder>();
            while (GetValue(bindingContext, "order[" + o + "][column]") != null)
            {
                order.Add(new DataTableOrder
                {
                    Column = Convert.ToInt32(GetValue(bindingContext, "order[" + o + "][column]")),
                    Dir = GetValue(bindingContext, "order[" + o + "][dir]")
                });
                o++;
            }
            model.Order = order.ToArray();
            // Columns
            var c = 0;
            var columns = new List<DataTableColumn>();
            while (!String.IsNullOrEmpty(GetValue(bindingContext, "columns[" + c + "][data]")) && !String.IsNullOrEmpty(GetValue(bindingContext, "columns[" + c + "][name]")))
            {
                columns.Add(new DataTableColumn
                {
                    Data = GetValue(bindingContext, "columns[" + c + "][data]"),
                    Name = GetValue(bindingContext, "columns[" + c + "][name]"),
                    Orderable = Convert.ToBoolean(GetValue(bindingContext, "columns[" + c + "][orderable]")),
                    Searchable = Convert.ToBoolean(GetValue(bindingContext, "columns[" + c + "][searchable]")),
                    Index = c,
                    Search = new DataTableSearch
                    {
                        Value = GetValue(bindingContext, "columns[" + c + "][search][value]"),
                        Regex = Convert.ToBoolean(GetValue(bindingContext, "columns[" + c + "][search][regex]"))
                    }
                });
                c++;
            }
            //Fk
            c = 0;
            while (!String.IsNullOrEmpty(GetValue(bindingContext, "columnfk[" + c + "][col]")) && !String.IsNullOrEmpty(GetValue(bindingContext, "columnfk[" + c + "][fk]")))
            {
                var colname = GetValue(bindingContext, "columnfk[" + c + "][col]");
                if (columns.Any(s => s.Data == colname))
                {
                    var col = columns.Single(s => s.Data == colname);
                    col.IsFk = true;
                    col.FkName = GetValue(bindingContext, "columnfk[" + c + "][fk]");
                }
                c++;
            }
            model.Columns = columns.ToArray();
            c = 0;
            var globalfilters = new List<KeyValuePair<string, object>>();
            while (!String.IsNullOrEmpty(GetValue(bindingContext, "globalqueryfilters[" + c + "][key]")) && !String.IsNullOrEmpty(GetValue(bindingContext, "globalqueryfilters[" + c + "][value]")))
            {
                var key = GetValue(bindingContext, "globalqueryfilters[" + c + "][key]");
                var val = GetValue(bindingContext, "globalqueryfilters[" + c + "][value]");
                globalfilters.Add(new KeyValuePair<string, object>(key, val));
                c++;
            }
            model.Globalqueryfilters = globalfilters.ToArray();
            c = 0;
            List<Guid> uoFilters = new List<Guid>();
            while(!string.IsNullOrEmpty(GetValue(bindingContext, "organisationunitfilters[" + c + "][key]")))
            {
                uoFilters.Add(Guid.Parse(GetValue(bindingContext, "organisationunitfilters[" + c + "][key]")));
                c++;
            }
           
            var ss = GetValue(bindingContext, "dtperiodestart");
            var se = GetValue(bindingContext, "dtperiodeend");
            DateTime parseDate;

            if (DateTime.TryParse(ss, out parseDate))
                model.SearchStartDate = parseDate;
            if (DateTime.TryParse(se, out parseDate))
                model.SearchEndDate = parseDate;

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Récupère la valeur d'un paramètre de la requête HTTP en utilisant le ValueProvider du contexte de liaison.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetValue(ModelBindingContext context, string key)
        {
            var result = context.ValueProvider.GetValue(key);
            return result == null ? string.Empty : result.FirstValue;
        }
    }
}
