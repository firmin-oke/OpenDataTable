using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DvsTyle.OpenDataTable.Core.TableDef.ToolTipItem
{
    [HtmlTargetElement("tooltipitem-modal", ParentTag = "datatable-tooltip-modal")]
    public class DataTableToolTipItemModal : DataTableToolTipiItem
    {
        [HtmlAttributeName("modaltitle")]
        public string Modaltitle { get; set; }

        [HtmlAttributeName("display-footer-closebtn")]
        public bool DisplayFooterCloseBtn { get; set; }

        [HtmlAttributeName("display-footer-submitbtn")]
        public bool DisplayFooterSubmitBtn { get; set; }

        [HtmlAttributeName("modalsize")]
        public string Modalsize { get; set; }

        [HtmlAttributeName("modalcallbackgrid")]
        public string Modalcallbackgrid { get; set; }

        [HtmlAttributeName("modalid")]
        public string Modalid { get; set; }

        [HtmlAttributeName("displaymodalfooter")]
        public bool NotDisplayModalFooter { get; set; }

        public DataTableToolTipItemModal(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            output.Attributes.Add("class", ButtonClass + " tooltip-item-modal");
            output.Attributes.Add("data-modaltitle", Modaltitle);

            output.Attributes.Add("data-footerclosebtn", DisplayFooterCloseBtn.ToString().ToLower());
            output.Attributes.Add("data-footersubmitbtn", DisplayFooterSubmitBtn.ToString().ToLower());

            output.Attributes.Add("data-modalsize", Modalsize);
            output.Attributes.Add("data-modalcallbackgrid", Modalcallbackgrid);

            output.Attributes.Add("data-modalid", Modalid);
            output.Attributes.Add("data-displaymodalfooter", true.ToString().ToLowerInvariant());
        }
    }
}
