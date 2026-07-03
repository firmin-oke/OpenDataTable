using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DvStyle.OpenDataTable.Enums;
using TagHelpers.Extensions;
using TagHelpers.Helpers;


namespace DvStyle.OpenDataTable.TableDef
{
    [HtmlTargetElement("datatable-container", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable", "datatable-tooltip-modal", "datatable-tooltip-modal-select", "datatable-tooltip-external", "datatable-tooltip-inline", "datatable-settings")]
    public class JQueryDataTableContainer : TagHelper
    {
        public override void Init(TagHelperContext context)
        {
            base.Init(context);
            context.Items.Add(new KeyValuePair<object, object>("datatablecontainerid", context.UniqueId));
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "datatablerootcontainer");
            string? id  = context.Items["datatablecontainerid"]==null ? Guid.NewGuid().ToString() :  context.Items["datatablecontainerid"] as string;
            output.Attributes.Add("id", id);

            output.TagMode = TagMode.StartTagAndEndTag;
            base.Process(context, output);
        }
    }

    [HtmlTargetElement("datatabletooltip", ParentTag = "datatable-container", TagStructure = TagStructure.WithoutEndTag)]
    public abstract class JQueryDataTableToolTip : TagHelper
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

        public JQueryDataTableToolTip(IHttpContextAccessor contextAccessor)
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

    [HtmlTargetElement("datatable-tooltip-modal", ParentTag = "datatable-container", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("tooltipitem-bulk", "tooltipitem-modal")]
    public class JQueryDataTableModalToolTip : JQueryDataTableToolTip
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

        public JQueryDataTableModalToolTip(IHttpContextAccessor contextAccessor) : base(contextAccessor)
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

    public class JQueryDataTableTooltipiItem : TagHelper
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

        public JQueryDataTableTooltipiItem(IHttpContextAccessor contextAccessor)
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

    [HtmlTargetElement("tooltipitem-bulk", ParentTag = "datatable-tooltip-modal")]
    public class JQueryDataTableToolTipItemBulk : JQueryDataTableTooltipiItem
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

    [HtmlTargetElement("tooltipitem-modal", ParentTag = "datatable-tooltip-modal")]
    public class JQueryDataTableToolTipItemModal : JQueryDataTableTooltipiItem
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

        public JQueryDataTableToolTipItemModal(IHttpContextAccessor contextAccessor) : base(contextAccessor)
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


    [HtmlTargetElement("datatable-tooltip-modal-select", ParentTag = "datatable-container", TagStructure = TagStructure.WithoutEndTag)]
    public class JQueryDataTableModalToolTipSelect : JQueryDataTableModalToolTip
    {
        [HtmlAttributeName("select-button-action")]
        public string SelectButtonAction { get; set; }

        [HtmlAttributeName("select-modalid")]
        public string SelectModalId { get; set; }

        [HtmlAttributeName("select-modaltitle")]
        public string SelectModalTitle { get; set; }

        [HtmlAttributeName("select-btn-title")]
        public string SelectBtnTitle { get; set; }

        [HtmlAttributeName("select-display-footer-closebtn")]
        public bool SelectDisplayFooterCloseBtn { get; set; }

        [HtmlAttributeName("select-display-footer-submitbtn")]
        public bool SelectDisplayFooterSubmitBtn { get; set; }

        [HtmlAttributeName("select-tooltip-disabled")]
        public bool DisabledSelect { get; set; }

        private TagBuilder SelectButton { get; set; }

        public string SelectIconClass { get; set; }
        public string SelectButtonClass { get; set; }

        public JQueryDataTableModalToolTipSelect(IHttpContextAccessor contextAccessor)
            : base(contextAccessor)
        {

        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);
            if (HasPermission)
            {
                SelectButton = new TagBuilder("button");
                if (!string.IsNullOrEmpty(SelectButtonClass))
                {
                    SelectButton.AddCssClass(SelectButtonClass);
                }
                else
                {
                    SelectButton.AddCssClass("btn btn-warning btn-icon");
                }
                if (DisabledSelect)
                {
                    SelectButton.AddCssClass("disabled");
                    SelectButton.Attributes.Add("disabled", "disabled");
                }
                SelectButton.AddCssClass("init-tooltip-modal");
                //SelectButton.AddCssClass("ml-1");

                SelectButton.Attributes.Add("title", SelectBtnTitle);
                SelectButton.Attributes.Add("data-tooltipaction", SelectButtonAction);

                SelectButton.Attributes.Add("data-modaltitle", SelectModalTitle);

                SelectButton.Attributes.Add("data-footerclosebtn", SelectDisplayFooterCloseBtn.ToString().ToLower());
                SelectButton.Attributes.Add("data-footersubmitbtn", SelectDisplayFooterSubmitBtn.ToString().ToLower());

                SelectButton.Attributes.Add("data-modalsize", Modalsize);
                SelectButton.Attributes.Add("data-modalcallbackgrid", Modalcallbackgrid);

                SelectButton.Attributes.Add("data-modalid", SelectModalId);
                var span = new TagBuilder("i");
                if (!string.IsNullOrEmpty(SelectIconClass))
                {
                    span.AddCssClass(SelectIconClass);
                }
                else
                {
                    span.AddCssClass("fas fa-reply");
                }
                SelectButton.InnerHtml.AppendHtml(span);
                TagItems.Add(SelectButton);
                output.PreContent.AppendHtml(SelectButton);
                await output.GetChildContentAsync();
            }
        }
    }


    [HtmlTargetElement("datatable-tooltip-external", ParentTag = "datatable-container", TagStructure = TagStructure.WithoutEndTag)]
    public class JQueryDataTableExternalToolTip : JQueryDataTableToolTip
    {
        public string PageTitle { get; set; }

        public JQueryDataTableExternalToolTip(IHttpContextAccessor contextAccessor)
            : base(contextAccessor)
        {

        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            if (HasPermission)
            {
                TooltipButton.AddCssClass("init-tooltip-external");
                TooltipButton.Attributes.Add("data-pagetitle", PageTitle);
                foreach (var i in TagItems)
                {
                    output.PreContent.AppendHtml(i);
                }
                await output.GetChildContentAsync();
            }
        }
    }

    [HtmlTargetElement("datatable-tooltip-inline", ParentTag = "datatable-container", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableToolTipIline : TagHelper
    {
        protected TagBuilder AddRowButton { get; set; }
        protected TagBuilder DeleteRowButton { get; set; }

        public JQueryDataTableToolTipIline(TagBuilder deleteRowButton, TagBuilder addRowButton)
        {
            AddRowButton = addRowButton;
            DeleteRowButton = deleteRowButton;
        }

        public JQueryDataTableToolTipIline()
        {
            AddRowButton = new TagBuilder("button");
            DeleteRowButton = new TagBuilder("button");
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("style", "display:inline;");
            AddRowButton = new TagBuilder("button");
            AddRowButton.AddCssClass("inlineaddrowbtn btn btn-icon btn-primary");

            DeleteRowButton = new TagBuilder("button");
            DeleteRowButton.AddCssClass("inlinedeleterowbtn btn btn-icon btn-danger");

            AddRowButton.AddCssClass("ml-1");
            var spanadd = new TagBuilder("i");
            spanadd.AddCssClass("ti-plus");

            var spanremove = new TagBuilder("i");
            spanremove.AddCssClass("ti-trash");

            AddRowButton.InnerHtml.AppendHtml(spanadd);
            DeleteRowButton.InnerHtml.AppendHtml(spanremove);

            output.Content.AppendHtml(AddRowButton);
            output.Content.AppendHtml(DeleteRowButton);

            base.Process(context, output);
        }
    }

    [HtmlTargetElement("datatable", ParentTag = "datatable-container", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable-header", "datatable-footer", "datatable-settings")]
    public class JQueryDataTable : TagHelper
    {
        [HtmlAttributeName("id")]
        public string Id { get; set; }

        [HtmlAttributeName("translate")]
        public bool Translate { get; set; } = false;

        [HtmlAttributeName("translateurl")]
        public string TranslateLocation { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";
            output.Attributes.Add("id", string.IsNullOrEmpty(Id) == true ? context.UniqueId : Id);
            output.Attributes.Add("class", "datagrid table table-striped table-bordered nowrap w-100");
            output.Attributes.Add("style", "width:100%");
            output.Attributes.Add("cellspacing", "0");
            if (Translate)
            {
                output.Attributes.Add("data-translateurl", TranslateLocation);
            }
            output.TagMode = TagMode.StartTagAndEndTag;
            base.Process(context, output);
        }
    }

    [HtmlTargetElement("datatable-header", ParentTag = "datatable", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("headerrow")]
    public class JQueryDataTableHeader : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "thead";
            output.TagMode = TagMode.StartTagAndEndTag;
            var datas = await output.GetChildContentAsync();
        }
    }

    [HtmlTargetElement("headerrow", ParentTag = "datatable-header", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableHeaderRow : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "tr";
            output.TagMode = TagMode.StartTagAndEndTag;
            var datas = await output.GetChildContentAsync();
        }
    }

    [HtmlTargetElement("headercol", ParentTag = "headerrow", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableHeaderColumn : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression Source { get; set; }
        [HtmlAttributeName("global-rows-selector")]
        public bool DataTableGlobalRowsSelector { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "th";
            output.TagMode = TagMode.StartTagAndEndTag;
            if (Source != null)
            {
                output.Content.Append(String.IsNullOrEmpty(Source.Metadata.DisplayName) != true ? Source.Metadata.DisplayName : Source.Name);
            }
            else
            {
                if (DataTableGlobalRowsSelector)
                {
                    var input = new TagBuilder("input");
                    input.Attributes.Add("type", "checkbox");
                    input.Attributes.Add("name", "datatable_select_all_rows");
                    input.Attributes.Add("value", "1");
                    output.Content.AppendHtml(input);
                }
                else
                {
                    output.Content.Append(string.Empty);
                }
            }
        }
    }

    [HtmlTargetElement("datatable-footer", ParentTag = "datatable", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("footercol")]
    public class JQueryDataTableFooter : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "tfoot";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.PreContent.SetHtmlContent("<tr>");
            output.PostContent.SetHtmlContent("</tr>");
            var datas = await output.GetChildContentAsync();
        }
    }

    [HtmlTargetElement("footercol", ParentTag = "datatable-footer", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableFooterColumn : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression Source { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "th";
            output.TagMode = TagMode.StartTagAndEndTag;
            if (Source != null)
            {
                output.Content.Append(String.IsNullOrEmpty(Source.Metadata.DisplayName) != true ? Source.Metadata.DisplayName : Source.Name);
            }
            else
            {
                output.Content.Append(string.Empty);
            }
        }
    }

    [HtmlTargetElement("datatable-settings", ParentTag = "datatable-container", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable-columndefs", "datatable-columns", "datatable-actions", "datatable-dataoptions",
        "datatable-buttons", "rowcolor")]
    public class JQueryDataTableSettings : TagHelper
    {
        [HtmlAttributeName("datasource")]
        [Required]
        public string DataSource { get; set; }

        [HtmlAttributeName("globalqueryfilters")]
        public Dictionary<string, object> GlobalqueryFilters { get; set; }

        [HtmlAttributeName("display-daterange-filter")]
        public bool DisplayDateRangeFilter { get; set; }

        [HtmlAttributeName("daterange-filter-periode")]
        public DataTableDataRangeFilterPeriode DataTableDataRangeFilterPeriode { get; set; } = DataTableDataRangeFilterPeriode.Month;

        [HtmlAttributeName("display-organisationunit-filter")]
        public bool DisplayOrganisationUnitFilter { get; set; }

        [HtmlAttributeName("organisationunit-filter-action")]
        public string OrganisationUnitFilterAction { get; set; }

        [HtmlAttributeName("organisationunit-filter-modaltitle")]
        public string OrganisationUnitFilterModalTitle { get; set; }

        public JQueryDataTableSettings()
        {
            GlobalqueryFilters = new Dictionary<string, object>();
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("datasource", DataSource);
            string paramlist = string.Empty;
            foreach (var p in GlobalqueryFilters)
            {
                paramlist += string.IsNullOrEmpty(paramlist) ? string.Empty : ";";
                paramlist += string.Format("{0}:{1}", p.Key, p.Value);
            }
            output.Attributes.Add("globalqueryfilters", paramlist);
            output.Attributes.Add(nameof(DisplayDateRangeFilter).ToLowerInvariant(), DisplayDateRangeFilter.ToString().ToLowerInvariant());

            output.Attributes.Add(nameof(DisplayOrganisationUnitFilter).ToLowerInvariant(), DisplayOrganisationUnitFilter.ToString().ToLowerInvariant());
            output.Attributes.Add(nameof(OrganisationUnitFilterAction).ToLowerInvariant(), OrganisationUnitFilterAction);
            output.Attributes.Add(nameof(OrganisationUnitFilterModalTitle).ToLowerInvariant(), OrganisationUnitFilterModalTitle);

            await output.GetChildContentAsync();
            if (DisplayDateRangeFilter)
            {
                var now = DateTime.Today;
                DateTime? start = null;
                DateTime? end = null;

                switch (DataTableDataRangeFilterPeriode)
                {
                    case DataTableDataRangeFilterPeriode.Year:
                        start = new DateTime(now.Year, 1, 1);
                        end = new DateTime(now.Year, 12, 31, 23, 59, 0);
                        break;
                    case DataTableDataRangeFilterPeriode.Month:
                        start = new DateTime(now.Year, now.Month, 1);
                        end = start.GetValueOrDefault().AddMonths(1).AddDays(-1);
                        end = end.GetValueOrDefault().AddHours(23).AddMinutes(59);
                        break;
                    case DataTableDataRangeFilterPeriode.Week:
                        DayOfWeek day = DateTime.Now.DayOfWeek;
                        int days = day - DayOfWeek.Monday;
                        start = DateTime.Now.AddDays(-days);
                        end = start.GetValueOrDefault().AddDays(6);
                        end = end.GetValueOrDefault().AddHours(23).AddMinutes(59);
                        break;
                    case DataTableDataRangeFilterPeriode.Day:
                        start = DateTime.Today;
                        end = start.GetValueOrDefault().AddHours(23).AddMinutes(59);
                        break;
                }
                if (start.HasValue)
                {
                    output.Attributes.Add("daterange-filter-start", start.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm"));
                }
                else
                {
                    output.Attributes.Add("daterange-filter-start", string.Empty);
                }

                if (end.HasValue)
                {
                    output.Attributes.Add("daterange-filter-end", end.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm"));
                }
                else
                {
                    output.Attributes.Add("daterange-filter-end", string.Empty);
                }

            }
        }
    }

    [HtmlTargetElement("datatable-columndefs", ParentTag = "datatable-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable-columndef")]
    public class JQueryDataTableColumndefs : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            await output.GetChildContentAsync();
        }
    }

    [HtmlTargetElement("datatable-columndef", ParentTag = "datatable-columndefs", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableColumnDef : TagHelper
    {
        [HtmlAttributeName("targets")]
        public int Targets { get; set; }

        [HtmlAttributeName("visible")]
        public bool Visible { get; set; } = true;

        [HtmlAttributeName("searchable")]
        public bool Searchable { get; set; } = true;

        [HtmlAttributeName("orderable")]
        public bool Orderable { get; set; } = true;

        public string Width { get; set; }

        [HtmlAttributeName("is-select-row-checkbox")]
        public bool IsSelectRowCheckBox { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("targets", Targets);
            output.Attributes.Add("visible", Visible.ToString().ToLowerInvariant());
            output.Attributes.Add("searchable", Searchable.ToString().ToLowerInvariant());
            output.Attributes.Add("isselectrowcheckbox", IsSelectRowCheckBox.ToString().ToLowerInvariant());
            if (context.AllAttributes["width"] != null)
            {
                output.Attributes.Add("width", Width.ToLowerInvariant());
            }
            output.Attributes.Add("orderable", Orderable.ToString().ToLowerInvariant());
            await output.GetChildContentAsync();
        }
    }

    [HtmlTargetElement("datatable-columns", ParentTag = "datatable-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable-column")]
    public class JQueryDataTableColumns : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            await output.GetChildContentAsync();
        }
    }

    [HtmlTargetElement("datatable-column", ParentTag = "datatable-columns", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableColumn : TagHelper
    {
        [HtmlAttributeName("autoWidth")]
        public bool AutoWidth { get; set; } = true;

        [HtmlAttributeName("asp-for")]
        public ModelExpression Source { get; set; }

        [HtmlAttributeName("is-foreign-key")]
        public bool IsForeignKey { get; set; } = false;

        [HtmlAttributeName("foreign-key-name")]
        public string ForeignKeyName { get; set; }

        public bool IsDateTime { get; set; } = false;

        [HtmlAttributeName("orderable")]
        public bool Orderable { get; set; }

        [HtmlAttributeName("searchable")]
        public bool Searchable { get; set; }

        [HtmlAttributeName("visible")]
        public bool Visible { get; set; }

        [HtmlAttributeName("is-colorcol")]
        public bool IsColorcol { get; set; }

        [HtmlAttributeName("is-requiredcol")]
        public bool IsRequiredCol { get; set; }

        [HtmlAttributeName("search-input-type")]
        public JQueryDataTableSearchInputType SearchInputType { get; set; }

        [HtmlAttributeName("enum-default-value")]
        public object EnumDefaultValue { get; set; } = null;

        [HtmlAttributeName("input-default-value")]
        public object InputDefaultValue { get; set; } = null;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            if (Source != null)
            {
                output.Attributes.Add("data", Source.Name);
                output.Attributes.Add("name", string.IsNullOrEmpty(Source.Metadata.DisplayName) != true ? Source.Metadata.DisplayName : Source.Name);
                output.Attributes.Add("autoWidth", AutoWidth.ToString().ToLowerInvariant());


                var requiredAttributes = Source.Metadata.ContainerType
                                 .GetProperty(Source.Metadata.PropertyName)
                                 .GetCustomAttributes(typeof(JQueryDataTableAttribute), true);
                if (requiredAttributes != null & requiredAttributes.Count() > 0)
                {
                    var jq = (requiredAttributes[0] as JQueryDataTableAttribute);
                    output.Attributes.Add("isfk", jq.IsForeignKey.ToString().ToLowerInvariant());
                    output.Attributes.Add("fkname", jq.ForeignKeyName);
                    output.Attributes.Add("searchable", jq.Searchable.ToString().ToLower());
                    output.Attributes.Add("visible", jq.Visible.ToString().ToLower());
                    output.Attributes.Add("orderable", jq.Orderable.ToString().ToLower());
                    output.Attributes.Add("iscolorcol", jq.IsColorCol.ToString().ToLower());
                    output.Attributes.Add("isrequiredcol", jq.IsRequiredCol.ToString().ToLower());

                    if (jq.EnumDefaultValue != null)
                    {
                        output.Attributes.Add("enumdefaultvalue", jq.EnumDefaultValue.ToString());
                    }
                    else if (EnumDefaultValue != null)
                    {
                        output.Attributes.Add("enumdefaultvalue", EnumDefaultValue.ToString());
                    }

                    if (jq.InputDefaultValue != null)
                    {
                        output.Attributes.Add("inputdefaultvalue", jq.InputDefaultValue.ToString());
                    }
                    else if (!InputDefaultValue.IsNull())
                    {
                        output.Attributes.Add("inputdefaultvalue", InputDefaultValue.ToString());
                    }

                    if (Source.ModelExplorer.ModelType.IsEnum || (Nullable.GetUnderlyingType(Source.ModelExplorer.ModelType) != null && Nullable.GetUnderlyingType(Source.ModelExplorer.ModelType).IsEnum))
                    {
                        jq.SearchType = JQueryDataTableSearchInputType.EnumList;
                    }
                    else if (Source.ModelExplorer.ModelType == typeof(DateTime) || (Nullable.GetUnderlyingType(Source.ModelExplorer.ModelType)) == typeof(DateTime)
                        || Source.ModelExplorer.ModelType == typeof(DateTime?))
                    {
                        object[]? datatypes = Source.Metadata.ContainerType.GetProperty(Source.Metadata.PropertyName)
                            .GetCustomAttributes(typeof(DataTypeAttribute), true);
                        if (datatypes != null)
                        {

                            if (datatypes.Length > 0)
                            {
                                var datatype = (datatypes[0] as DataTypeAttribute);
                                if (datatype != null && datatype.DataType == DataType.Time)
                                {
                                    jq.SearchType = JQueryDataTableSearchInputType.TimeCalendar;
                                }
                                else
                                {
                                    jq.SearchType = JQueryDataTableSearchInputType.Calendar;
                                }
                            }
                            else
                            {
                                jq.SearchType = JQueryDataTableSearchInputType.Calendar;
                            }
                        }
                        else
                        {
                            jq.SearchType = JQueryDataTableSearchInputType.Calendar;
                        }

                    }
                    else if (Source.ModelExplorer.ModelType == typeof(long) || Source.ModelExplorer.ModelType == typeof(int))
                    {
                        jq.SearchType = JQueryDataTableSearchInputType.Number;
                    }
                    else if (Source.ModelExplorer.ModelType == typeof(bool))
                    {
                        jq.SearchType = JQueryDataTableSearchInputType.EnumList;
                    }
                    else if (Source.ModelExplorer.ModelType == typeof(IFormFile) || Source.ModelExplorer.ModelType == typeof(FormFileWrapper))
                    {
                        jq.SearchType = JQueryDataTableSearchInputType.InputFileUpload;
                    }
                    else
                    {
                        jq.SearchType = JQueryDataTableSearchInputType.Text;
                    }
                    output.Attributes.Add("searchtype", jq.SearchType.ToString().ToLower());

                    if (jq.SearchType == JQueryDataTableSearchInputType.EnumList && (Source.ModelExplorer.ModelType.IsEnum || (Nullable.GetUnderlyingType(Source.ModelExplorer.ModelType) != null
                        && Nullable.GetUnderlyingType(Source.ModelExplorer.ModelType).IsEnum)))
                    {
                        string enumvalues = string.Empty;
                        List<KeyValuePair<string, string>> enumValsMap = new List<KeyValuePair<string, string>>();
                        if (Nullable.GetUnderlyingType(Source.ModelExplorer.ModelType) != null)
                        {
                            var types = Nullable.GetUnderlyingType(Source.ModelExplorer.ModelType);
                            foreach (var e in Enum.GetValues(types))
                            {
                                enumValsMap.Add(new KeyValuePair<string, string>(EnumsHelpers.GetDisplayName(e), e.ToString()));
                            }
                        }
                        else
                        {
                            foreach (var e in Enum.GetValues(Source.ModelExplorer.ModelType))
                            {
                                enumValsMap.Add(new KeyValuePair<string, string>(EnumsHelpers.GetDisplayName(e), e.ToString()));
                            }
                        }
                        var orderableEnumValsMap = jq.OrderableEnum ? enumValsMap.OrderBy(s => s.Key).ToList() : enumValsMap.ToList();
                        foreach (var el in orderableEnumValsMap)
                        {
                            enumvalues += string.IsNullOrEmpty(enumvalues) ? enumvalues : ";";
                            enumvalues += el.Value.ToString() + "," + el.Key;
                        }
                        output.Attributes.Add("enumvalues", enumvalues);
                    }
                    else if (Source.ModelExplorer.ModelType == typeof(bool) && jq.SearchType == JQueryDataTableSearchInputType.EnumList)
                    {
                        output.Attributes.Add("enumvalues", "true,Oui;false,Non");
                    }
                }
                else
                {
                    IsForeignKey = context.AllAttributes["is-foreign-key"] == null ? false : IsForeignKey;
                    ForeignKeyName = context.AllAttributes["foreign-key-name"] == null ? string.Empty : ForeignKeyName;
                    Orderable = context.AllAttributes["orderable"] == null ? false : Orderable;
                    Searchable = context.AllAttributes["searchable"] == null ? false : Searchable;
                    Visible = context.AllAttributes["visible"] == null ? false : Visible;
                    SearchInputType = context.AllAttributes["search-input-type"] == null ? JQueryDataTableSearchInputType.Text : SearchInputType;

                    output.Attributes.Add("isfk", IsForeignKey.ToString().ToLowerInvariant());
                    output.Attributes.Add("fkname", ForeignKeyName);
                    output.Attributes.Add("searchable", Searchable.ToString().ToLowerInvariant());
                    output.Attributes.Add("orderable", Orderable.ToString().ToLowerInvariant());
                    output.Attributes.Add("searchtype", JQueryDataTableSearchInputType.Text.ToString().ToLower());

                    output.Attributes.Add("iscolorcol", IsColorcol.ToString().ToLower());
                    output.Attributes.Add("isrequiredcol", IsRequiredCol.ToString().ToLower());

                    if (EnumDefaultValue != null)
                    {
                        output.Attributes.Add("enumdefaultvalue", EnumDefaultValue.ToString());
                    }

                    if (!InputDefaultValue.IsNull())
                    {
                        output.Attributes.Add("inputdefaultvalue", InputDefaultValue.ToString());
                    }
                }
                output.Attributes.Add("isdatetime", IsDateTime.ToString().ToLowerInvariant());
                if (Source.ModelExplorer.ModelType == typeof(bool))
                {
                    output.Attributes.Add("isbool", true.ToString().ToLowerInvariant());
                }
            }
        }
    }

    [HtmlTargetElement("datatable-dataoptions", ParentTag = "datatable-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable-button")]
    public class JQueryDataTableDataOptions : TagHelper
    {
        [HtmlAttributeName("processing")]
        public bool Processing { get; set; } = true;

        [HtmlAttributeName("serverSide")]
        public bool ServerSide { get; set; } = true;

        [HtmlAttributeName("filter")]
        public bool Filter { get; set; } = true;

        [HtmlAttributeName("ordermulti")]
        public bool OrderMulti { get; set; } = false;

        [HtmlAttributeName("has-fixed-columns")]
        public bool HasFixedColumns { get; set; } = false;

        [HtmlAttributeName("fixedcolumn-left")]
        public int FixedColumnLeft { get; set; } = 1;

        [HtmlAttributeName("fixedcolumn-right")]
        public int FixedColumnRight { get; set; } = 1;

        public bool EnabledRowSelection { get; set; } = false;
        public DataTableRowSelectionMode RowSelectionMode { get; set; }

        public bool IsPuckUpTable { get; set; }

        public bool Paging { get; set; } = true;
        public bool Info { get; set; } = true;
        public bool ShowLengthMenu { get; set; } = true;

        public bool IsInLineDataTable { get; set; } = false;
        public string RowCollectionName { get; set; }
        public int InLineTableMinRow { get; set; } = 0;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("processing", Processing.ToString().ToLowerInvariant());
            output.Attributes.Add("serverSide", ServerSide.ToString().ToLowerInvariant());
            output.Attributes.Add("filter", Filter.ToString().ToLowerInvariant());
            output.Attributes.Add("orderMulti", OrderMulti.ToString().ToLowerInvariant());

            output.Attributes.Add("hasfixedcolumns", HasFixedColumns.ToString().ToLowerInvariant());
            output.Attributes.Add("fixedcolumnleft", FixedColumnLeft.ToString());
            output.Attributes.Add("fixedcolumnright", FixedColumnRight.ToString());

            output.Attributes.Add("enabledrowselection", EnabledRowSelection.ToString().ToLowerInvariant());
            output.Attributes.Add("rowselectionmode", RowSelectionMode.ToString());

            output.Attributes.Add("ispickuptable", IsPuckUpTable.ToString().ToLowerInvariant());

            output.Attributes.Add(nameof(Paging).ToLowerInvariant(), Paging.ToString().ToLowerInvariant());
            output.Attributes.Add(nameof(Info).ToLowerInvariant(), Info.ToString().ToLowerInvariant());
            output.Attributes.Add(nameof(ShowLengthMenu).ToLowerInvariant(), ShowLengthMenu.ToString().ToLowerInvariant());

            output.Attributes.Add(nameof(IsInLineDataTable).ToLowerInvariant(), IsInLineDataTable.ToString().ToLowerInvariant());
            output.Attributes.Add(nameof(RowCollectionName).ToLowerInvariant(), RowCollectionName);
            output.Attributes.Add(nameof(InLineTableMinRow).ToLowerInvariant(), InLineTableMinRow);

        }
    }

    [HtmlTargetElement("datatable-button", ParentTag = "datatable-dataoptions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTablButtons : TagHelper
    {
        public DataTableButtonType Name { get; set; }
        public string FileName { get; set; }
        public string FileTitle { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            switch (Name)
            {
                case DataTableButtonType.csv:
                    output.Attributes.Add("name", Name.ToString());
                    output.Attributes.Add("iconclass", "fas fa-file-excel");
                    output.Attributes.Add("buttonclass", "btn bg-green fg-white btn-icon btn-square");
                    output.Attributes.Add("filename", FileName);
                    output.Attributes.Add("filetitle", FileTitle);
                    break;
                case DataTableButtonType.selectAll:
                    output.Attributes.Add("name", Name.ToString());
                    output.Attributes.Add("iconclass", "mdi mdi-check-all");
                    output.Attributes.Add("buttonclass", "btn btn-success btn-icon btn-square");
                    break;
                case DataTableButtonType.selectNone:
                    output.Attributes.Add("name", Name.ToString());
                    output.Attributes.Add("iconclass", "feather icon-x-circle");
                    output.Attributes.Add("buttonclass", "btn btn-warning btn-icon btn-square");
                    break;
            }
            base.Process(context, output);
        }
    }

    [HtmlTargetElement("datatable-rowoptions", ParentTag = "datatable-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable-rowoption")]
    public class JQueryDataTableDataRowOptions : TagHelper
    {
        //public override void Process(TagHelperContext context, TagHelperOutput output)
        //{
        //    base.Process(context, output);
        //}
    }

    [HtmlTargetElement("datatable-rowoption", ParentTag = "datatable-rowoptions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableDataRowOption : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("value")]
        public string ColValue { get; set; }

        [HtmlAttributeName("css-class")]
        public string CssClass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //base.Process(context, output);
            //output.TagMode = TagMode.StartTagOnly;
            output.Attributes.Add("colname", For.Name);
            output.Attributes.Add("value", ColValue);
            output.Attributes.Add("class", CssClass);
        }
    }

    [HtmlTargetElement("rowcolor", ParentTag = "datatable-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RowColor : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("colname", For.Name);
            await output.GetChildContentAsync();
        }
    }

    [HtmlTargetElement("rowcolorvalue", ParentTag = "rowcolor", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RowColorValue : TagHelper
    {
        public string ColValue { get; set; }
        public string CssClass { get; set; }

        public override void Init(TagHelperContext context)
        {
            base.Init(context);
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("value", ColValue);
            output.Attributes.Add("class", CssClass);
            await base.ProcessAsync(context, output);
        }
    }

    public enum DataTableAddEditMode
    {
        PopUp = 1,
        External = 2,
        Custom = 3
    }

    public enum DataTableRowSelectionMode
    {
        Single = 1,
        Multi = 2
    }

    public enum DataTableButtonType
    {
        csv = 1,
        selectAll = 2,
        selectNone = 3
    }

    public enum DataTableDataRangeFilterPeriode
    {
        Year = 0,
        Month = 1,
        Week = 2,
        Day = 3,
        All = 4
    }
}
