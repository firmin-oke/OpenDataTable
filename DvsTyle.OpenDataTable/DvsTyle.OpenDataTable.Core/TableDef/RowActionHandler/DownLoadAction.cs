using DvStyle.OpenDataTable.TableDef;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DvsTyle.OpenDataTable.Core.TableDef.RowActionHandler
{

    /// <summary>
    /// Permet de définir une action de téléchargement de fichier dans un tableau de données. Cette action est représentée par un bouton qui, lorsqu'il est cliqué, déclenche le téléchargement d'un fichier à partir d'une URL spécifiée.
    /// </summary>
    [HtmlTargetElement("datatable-action-downloadfile", ParentTag = "datatable-actions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DownLoadAction : TagHelper
    {
        [HtmlAttributeName("url")]
        public string Url { get; set; }

        [HtmlAttributeName("iconclass")]
        public string IconClass { get; set; }

        [HtmlAttributeName("buttonclass")]
        public string ButtonClass { get; set; }

        public DownLoadAction(IHttpContextAccessor contextAccessor)
        {
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("url", Url);
            output.Attributes.Add("visible", true.ToString().ToLowerInvariant());

            if (context.AllAttributes["iconclass"] != null && !string.IsNullOrEmpty(IconClass))
            {
                output.Attributes.Add("iconclass", IconClass);
            }
            if (context.AllAttributes["buttonclass"] != null && !string.IsNullOrEmpty(ButtonClass))
            {
                output.Attributes.Add("buttonclass", ButtonClass);
            }
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
