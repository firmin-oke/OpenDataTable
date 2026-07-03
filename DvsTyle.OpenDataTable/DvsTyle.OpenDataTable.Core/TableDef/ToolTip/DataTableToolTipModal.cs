using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DvsTyle.OpenDataTable.Core.TableDef.ToolTip
{
    [HtmlTargetElement("datatable-tooltip-modal", ParentTag = "datatable-container", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("tooltipitem-bulk", "tooltipitem-modal")]
    public class DataTableToolTipModal : DataTableToolTip
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

        public bool AddTooltipModal { get; set; } = true;
        public string TooltipPageTitle { get; set; }

        public DataTableToolTipModal(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {

        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);
            if (AddTooltipModal)
            {
                TooltipButton.AddCssClass("init-tooltip-modal");
            }
            else
            {
                TooltipButton.AddCssClass("init-tooltip-external");
                TooltipButton.Attributes.Add("data-pagetitle", TooltipPageTitle);
            }

            TooltipButton.Attributes.Add("data-modaltitle", Modaltitle);

            TooltipButton.Attributes.Add("data-footerclosebtn", DisplayFooterCloseBtn.ToString().ToLower());
            TooltipButton.Attributes.Add("data-footersubmitbtn", DisplayFooterSubmitBtn.ToString().ToLower());

            TooltipButton.Attributes.Add("data-modalsize", Modalsize);
            TooltipButton.Attributes.Add("data-modalcallbackgrid", Modalcallbackgrid);

            TooltipButton.Attributes.Add("data-modalid", Modalid);
            TooltipButton.Attributes.Add("data-displaymodalfooter", true.ToString().ToLowerInvariant());


            foreach (var i in TagItems)
            {
                output.PreContent.AppendHtml(i);
            }
            await output.GetChildContentAsync();
        }
    }
}
