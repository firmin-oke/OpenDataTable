using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DvsTyle.OpenDataTable.Core.TableDef.ToolTipItem
{
  

    public abstract class DataTableToolTipiItem : TagHelper
    {
        public string Action { get; set; }
        public string IconClass { get; set; }
        public string ButtonClass { get; set; }
        public string ButtonTitle { get; set; }

        [HtmlAttributeName("action-params")]
        public IDictionary<string, string> ActionParams { get; set; }

        // public OperationAction OperationAction { get; set; } = OperationAction.Update;
        //public OperationRessource? OperationRessource { get; set; }

        protected readonly IHttpContextAccessor _contextAccessor;

        public bool HasPermission { get; set; } = true;

        public DataTableToolTipiItem(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            ActionParams = new Dictionary<string, string>();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            if (HasPermission)
            {
                output.TagName = "button";
                output.Attributes.Add("data-action", Action);
                output.Attributes.Add("title", ButtonTitle);
                var i = new TagBuilder("i");
                i.AddCssClass(IconClass);
                output.Content.AppendHtml(i);

                string paramlist = string.Empty;
                if (ActionParams != null)
                {
                    foreach (var p in ActionParams)
                    {
                        paramlist += string.IsNullOrEmpty(paramlist) ? string.Empty : ";";
                        paramlist += string.Format("{0}:{1}", p.Key, p.Value);
                    }
                }
                output.Attributes.Add("data-actionparams", paramlist);
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}
