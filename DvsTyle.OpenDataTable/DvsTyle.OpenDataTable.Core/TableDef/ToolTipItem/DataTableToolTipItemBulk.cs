using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DvsTyle.OpenDataTable.Core.TableDef.ToolTipItem
{

    [HtmlTargetElement("tooltipitem-bulk", ParentTag = "datatable-tooltip-modal")]
    public class JQueryDataTableToolTipItemBulk : DataTableToolTipiItem
    {
        public JQueryDataTableToolTipItemBulk(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {

        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            output.Attributes.Add("class", ButtonClass + " tooltip-item-bulk");
        }
    }
}
