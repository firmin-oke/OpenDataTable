using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DvsTyle.OpenDataTable.Core.TableDef.RowActionHandler
{
    /// <summary>
    /// class RowAction est une classe qui représente une action sur une ligne dans un tableau de données. 
    /// Cette classe est un TagHelper qui sert uniquement de parent pour les actions de ligne sur les tableaux. 
    /// Il ne faut pas l'utiliser directement dans le code HTML, mais plutôt comme conteneur pour les actions de ligne spécifiques.
    /// </summary>
    [HtmlTargetElement("datatable-actions", ParentTag = "datatable-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable-action-updaterow", "datatable-action-addto", "datatable-action-edit", "datatable-action-delete","datatable-action-selectrowdata", "datatable-action-downloadfile")]
    public class RowAction : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            await output.GetChildContentAsync();
        }
    }
}
