using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DvsTyle.OpenDataTable.Core.TableDef.RowActionHandler
{
    /// <summary>
    /// Classe pour gérer l'action de mise à jour d'une ligne dans un tableau de données. Cette classe hérite de TagHelper et est utilisée pour générer un élément HTML représentant l'action de mise à jour d'une ligne.
    /// </summary>
    [HtmlTargetElement("datatable-action-updaterow", ParentTag = "datatable-actions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class UpdateRowAction : TagHelper
    {
        [HtmlAttributeName("tooltip-action")]
        public string TooltipAction { get; set; }

        [HtmlAttributeName("callbackgrid")]
        public string Callbackgrid { get; set; }

        [HtmlAttributeName("Disabled")]
        public bool Disabled { get; set; }

        [HtmlAttributeName("iconclass")]
        public string IconClass { get; set; }

        [HtmlAttributeName("buttonclass")]
        public string ButtonClass { get; set; }

        [HtmlAttributeName("action-params")]
        public IDictionary<string, string> ActionParams { get; set; }

        public bool HasPermission { get; set; } = true;

        public UpdateRowAction(IHttpContextAccessor contextAccessor)
        {
            ActionParams = new Dictionary<string, string>();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            if (HasPermission)
            {
                output.Attributes.Add("visible", true.ToString().ToLowerInvariant());
                output.Attributes.Add("tooltipaction", TooltipAction);
                output.Attributes.Add("callbackgrid", Callbackgrid);

                if (!string.IsNullOrEmpty(IconClass))
                    output.Attributes.Add("iconclass", IconClass);
                else
                    output.Attributes.Add("iconclass", "fas fa-check-circle");

                if (!string.IsNullOrEmpty(ButtonClass))
                    output.Attributes.Add("buttonclass", ButtonClass);
                else
                    output.Attributes.Add("buttonclass", "btn btn-success btn-icon btn-sm btn-rounded");

                string paramlist = string.Empty;
                if (ActionParams != null)
                {
                    foreach (var p in ActionParams)
                    {
                        paramlist += string.IsNullOrEmpty(paramlist) ? string.Empty : ";";
                        paramlist += string.Format("{0}:{1}", p.Key, p.Value);
                    }
                }
                output.Attributes.Add("actionparams", paramlist);
            }
            else
            {
                output.Attributes.Add("visible", false.ToString().ToLowerInvariant());
            }
        }
    }
}
