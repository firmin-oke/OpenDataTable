using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DvsTyle.OpenDataTable.Core.TableDef
{
    /// <summary>
    /// TagHelper pour la définition d'un conteneur de datatable. Ce conteneur peut contenir un ou plusieurs datatables et des modales d'info-bulles.
    /// Comme tout les tag helpers, il est utilisé pour générer des balises non HTML qui seront ensuite parsées pour la construction des taghml
    /// </summary>
    [HtmlTargetElement("datatable-container", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable", "datatable-tooltip-modal", "datatable-tooltip-modal-select", "datatable-tooltip-external", "datatable-tooltip-inline", "datatable-settings")]
    public class DataTableContainer : TagHelper
    {
        private static string _datatablecontainerid = "datatablecontainerid";
        public override void Init(TagHelperContext context)
        {
            base.Init(context);
            context.Items.Add(new KeyValuePair<object, object>(_datatablecontainerid, context.UniqueId));
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "datatablerootcontainer");
            string? id = context.Items["datatablecontainerid"] == null ? Guid.NewGuid().ToString() : context.Items["datatablecontainerid"] as string;
            output.Attributes.Add("id", id);

            output.TagMode = TagMode.StartTagAndEndTag;
            base.Process(context, output);
        }
    }
}
