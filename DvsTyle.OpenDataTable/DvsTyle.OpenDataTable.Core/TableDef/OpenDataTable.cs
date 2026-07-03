using DvsTyle.OpenDataTable.Core.TableDef;
using DvStyle.OpenDataTable.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TagHelpers.Extensions;
using TagHelpers.Helpers;


namespace DvStyle.OpenDataTable.TableDef
{
    [HtmlTargetElement("datatable", ParentTag = "datatable-container", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable-header", "datatable-footer", "datatable-settings")]
    public class OpenDataTable : TagHelper
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
                                 .GetCustomAttributes(typeof(DataTableModelAttributeDescription), true);
                if (requiredAttributes != null & requiredAttributes.Count() > 0)
                {
                    var jq = (requiredAttributes[0] as DataTableModelAttributeDescription);
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
