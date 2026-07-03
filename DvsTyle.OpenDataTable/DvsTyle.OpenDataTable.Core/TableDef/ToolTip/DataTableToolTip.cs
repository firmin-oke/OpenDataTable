using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DvsTyle.OpenDataTable.Core.TableDef.ToolTip
{
    [HtmlTargetElement("datatabletooltip", ParentTag = "datatable-container", TagStructure = TagStructure.WithoutEndTag)]
    public abstract class DataTableToolTip : TagHelper
    {
        protected TagBuilder TooltipButton { get; set; }

        [HtmlAttributeName("tooltip-action")]
        public string TooltipAction { get; set; }

        [HtmlAttributeName("datatable-id")]
        public string DataTableId { get; set; }

        [HtmlAttributeName("refresh-only")]
        public bool RefreshOnly { get; set; } = false;
        protected TagBuilder btnContainer;

        [HtmlAttributeName("disabled-tooltip")]
        public bool DisabledToolTip { get; set; }

        [HtmlAttributeName("buttontitle")]
        public string ToolTipButtontitle { get; set; }

        //Custom button
        private TagBuilder CustomButtonButton { get; set; }

        [HtmlAttributeName("display-custombutton")]
        public bool DisplayCustomButton { get; set; }

        [HtmlAttributeName("custombutton-modaltitle")]
        public string CustomButtonModaltitle { get; set; }

        [HtmlAttributeName("custombutton-title")]
        public string CustomButtonTitle { get; set; }

        [HtmlAttributeName("custombutton-display-footer-closebtn")]
        public bool CustomButtonDisplayFooterCloseBtn { get; set; }

        [HtmlAttributeName("custombutton-display-footer-submitbtn")]
        public bool CustomButtonDisplayFooterSubmitBtn { get; set; }

        [HtmlAttributeName("custombutton-modalsize")]
        public string CustomButtonModalsize { get; set; }

        [HtmlAttributeName("custombutton-modalcallbackgrid")]
        public string CustomButtonModalcallbackgrid { get; set; }

        [HtmlAttributeName("custombutton-modalid")]
        public string CustomButtonModalid { get; set; }

        [HtmlAttributeName("custombutton-displaymodalfooter")]
        public bool CustomButtonNotDisplayModalFooter { get; set; }

        [HtmlAttributeName("custombutton-iconclass")]
        public string CustomButtonIconClass { get; set; }

        [HtmlAttributeName("custombutton-tooltip-action")]
        public string CustomButtonTooltipAction { get; set; }

        protected readonly IHttpContextAccessor _contextAccessor;

        protected List<TagBuilder> TagItems = new List<TagBuilder>();

        public bool HasPermission { get; set; } = true;

        public DataTableToolTip(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            TooltipButton = new TagBuilder("button");
            btnContainer = new TagBuilder("div");
            btnContainer.AddCssClass("col-12");
            btnContainer.AddCssClass("pb-2");
            btnContainer.AddCssClass("border-bottom");
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            output.TagName = "div";
            output.Attributes.Add("style", "display:inline;");
            TagBuilder refreshbtn = new TagBuilder("button");
            refreshbtn.AddCssClass("refreshgridbtn btn btn-icon btn-dark");

            if (DisabledToolTip)
            {
                refreshbtn.AddCssClass("disabled");
                refreshbtn.Attributes.Add("disabled", "disabled");
            }
            var spanrefresh = new TagBuilder("i");
            spanrefresh.AddCssClass("ti-reload");
            refreshbtn.Attributes.Add("data-tableid", DataTableId);
            refreshbtn.InnerHtml.AppendHtml(spanrefresh);
            TagItems.Add(refreshbtn);
            if (!RefreshOnly)
            {
                if (HasPermission)
                {
                    TooltipButton.AddCssClass("btn btn-primary btn-icon");
                    TooltipButton.Attributes.Add("title", ToolTipButtontitle);

                    if (DisabledToolTip)
                    {
                        TooltipButton.AddCssClass("disabled");
                        TooltipButton.Attributes.Add("disabled", "disabled");
                    }
                    //TooltipButton.AddCssClass("mr-1");
                    var span = new TagBuilder("i");
                    span.AddCssClass("ti-plus");
                    TooltipButton.Attributes.Add("data-tooltipaction", TooltipAction);
                    TooltipButton.InnerHtml.AppendHtml(span);
                    TagItems.Add(TooltipButton);
                }
            }

            if (DisplayCustomButton && HasPermission)
            {
                CustomButtonButton = new TagBuilder("button");
                CustomButtonButton.AddCssClass("btn btn-primary btn-icon");
                CustomButtonButton.Attributes.Add("title", CustomButtonTitle);

                if (DisabledToolTip)
                {
                    CustomButtonButton.AddCssClass("disabled");
                    CustomButtonButton.Attributes.Add("disabled", "disabled");
                }
                CustomButtonButton.Attributes.Add("data-tooltipaction", CustomButtonTooltipAction);
                CustomButtonButton.AddCssClass("init-tooltip-modal");
                CustomButtonButton.Attributes.Add("data-modaltitle", CustomButtonModaltitle);
                CustomButtonButton.Attributes.Add("data-footerclosebtn", CustomButtonDisplayFooterCloseBtn.ToString().ToLower());
                CustomButtonButton.Attributes.Add("data-footersubmitbtn", CustomButtonDisplayFooterSubmitBtn.ToString().ToLower());
                CustomButtonButton.Attributes.Add("data-modalsize", CustomButtonModalsize);
                CustomButtonButton.Attributes.Add("data-modalcallbackgrid", CustomButtonModalcallbackgrid);
                CustomButtonButton.Attributes.Add("data-modalid", CustomButtonModalid);
                CustomButtonButton.Attributes.Add("data-displaymodalfooter", true.ToString().ToLowerInvariant());
                var span = new TagBuilder("i");
                span.AddCssClass(CustomButtonIconClass);
                CustomButtonButton.InnerHtml.AppendHtml(span);
                TagItems.Add(CustomButtonButton);
            }

            string? id = context.Items["datatablecontainerid"] == null ? Guid.NewGuid().ToString() : context.Items["datatablecontainerid"] as string;
            output.PreContent.SetHtmlContent("<div id='memorydatatablemenu_" + id + "' style='float:left'>");
            output.PostContent.SetHtmlContent("</div>");
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
