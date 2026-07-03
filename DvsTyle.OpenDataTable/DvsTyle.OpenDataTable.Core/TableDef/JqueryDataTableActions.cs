using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DvStyle.OpenDataTable.TableDef
{
    [HtmlTargetElement("datatable-actions", ParentTag = "datatable-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("datatable-action-updaterow", "datatable-action-addto", "datatable-action-edit", "datatable-action-delete", 
        "datatable-action-newmodalfromrow", "datatable-action-selectrowdata", "datatable-action-downloadfile")]
    public class JQueryDataTableActions : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            await output.GetChildContentAsync();
        }
    }

    [HtmlTargetElement("datatable-action-edit", ParentTag = "datatable-actions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableActionEdit : TagHelper
    {
        [HtmlAttributeName("url")]
        public string Url { get; set; }

        [HtmlAttributeName("type")]
        public DataTableAddEditMode EditType { get; set; }

        [HtmlAttributeName("modaltitle")]
        public string ModalTitle { get; set; }

        [HtmlAttributeName("closemodaltitle")]
        public string CloseBtnTitle { get; set; }

        [HtmlAttributeName("modalid")]
        public string ModalId { get; set; }

        [HtmlAttributeName("modalcallbackgrid")]
        public string ModalCallBackGrid { get; set; }

        [HtmlAttributeName("submitmodaltitle")]
        public string SubmitModalTitle { get; set; }

        [HtmlAttributeName("Disabled")]
        public bool Disabled { get; set; }

        [HtmlAttributeName("pagetitle")]
        public string PageTitle { get; set; }

        [HtmlAttributeName("iconclass")]
        public string IconClass { get; set; }

        [HtmlAttributeName("buttonclass")]
        public string ButtonClass { get; set; }

        [HtmlAttributeName("display-footer-closebtn")]
        public bool DisplayFooterCloseBtn { get; set; } = true;

        [HtmlAttributeName("display-footer-submitbtn")]
        public bool DisplayFooterSubmitBtn { get; set; } = true;

        [HtmlAttributeName("modalsize")]
        public string ModalSize { get; set; }

        protected readonly IHttpContextAccessor _contextAccessor;

        [HtmlAttributeName("isnotedit")]
        public bool IsNotEdit { get; set; }

        public JQueryDataTableActionEdit(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
           if(context.CheckUserPermissionAccess(OperationAction.Read, _contextAccessor.HttpContext.User))
            {
                output.Attributes.Add("isnotedit", true.ToString().ToLowerInvariant());
                output.Attributes.Add("visible", true.ToString().ToLowerInvariant());
                output.Attributes.Add("url", Url);
                output.Attributes.Add("editmode", EditType.ToString());
                output.Attributes.Add("disabled", Disabled.ToString().ToLower());
                output.Attributes.Add("pagetitle", string.IsNullOrEmpty(PageTitle) ? "Titre de la page" : PageTitle);
                output.Attributes.Add("isnotedit", IsNotEdit.ToString().ToLower());

                if (EditType == DataTableAddEditMode.PopUp || EditType == DataTableAddEditMode.Custom)
                {
                    output.Attributes.Add("modaltitle", ModalTitle);
                    if (context.AllAttributes["modalsize"] != null && !string.IsNullOrEmpty(ModalSize))
                    {
                        output.Attributes.Add("modalsize", ModalSize);
                    }
                    output.Attributes.Add("closemodaltitle", CloseBtnTitle);
                    output.Attributes.Add("modalid", ModalId);
                    output.Attributes.Add("modalcallbackgrid", ModalCallBackGrid);
                    output.Attributes.Add("submitmodaltitle", SubmitModalTitle);
                }
                if (context.AllAttributes["iconclass"] != null && !string.IsNullOrEmpty(IconClass))
                {
                    output.Attributes.Add("iconclass", IconClass);
                }
                if (context.AllAttributes["buttonclass"] != null && !string.IsNullOrEmpty(ButtonClass))
                {
                    output.Attributes.Add("buttonclass", ButtonClass);
                }
                output.Attributes.Add("footerclosebtn", DisplayFooterCloseBtn.ToString().ToLower());

                if(DisplayFooterSubmitBtn && !context.CheckUserPermissionAccess(OperationAction.Update, _contextAccessor.HttpContext.User))
                {
                    DisplayFooterSubmitBtn = false;
                }
                output.Attributes.Add("footersubmitbtn", DisplayFooterSubmitBtn.ToString().ToLower());
                output.TagMode = TagMode.StartTagAndEndTag;
            }
            else
            {
                output.Attributes.Add("visible", false.ToString().ToLowerInvariant());
            }
        }
    }

    [HtmlTargetElement("datatable-action-downloadfile", ParentTag = "datatable-actions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableActionDownloadFile : TagHelper
    {
        [HtmlAttributeName("url")]
        public string Url { get; set; }

        [HtmlAttributeName("iconclass")]
        public string IconClass { get; set; }

        [HtmlAttributeName("buttonclass")]
        public string ButtonClass { get; set; }

        protected readonly IHttpContextAccessor _contextAccessor;

        public JQueryDataTableActionDownloadFile(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
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

    [HtmlTargetElement("datatable-action-newmodalfromrow", ParentTag = "datatable-actions", TagStructure = TagStructure.WithoutEndTag)]
    public class JQueryDataTableActionCreateNewModalFromRowSelection : TagHelper
    {
        [HtmlAttributeName("Disabled")]
        public bool Disabled { get; set; }

        [HtmlAttributeName("url")]
        public string Url { get; set; }

        [HtmlAttributeName("type")]
        public DataTableAddEditMode EditType { get; set; }

        [HtmlAttributeName("modaltitle")]
        public string ModalTitle { get; set; }

        [HtmlAttributeName("modalid")]
        public string ModalId { get; set; }

        [HtmlAttributeName("modalcallbackgrid")]
        public string ModalCallBackGrid { get; set; }

        [HtmlAttributeName("action-params")]
        public IDictionary<string, string> ActionParams { get; set; } = new Dictionary<string, string>();

        [HtmlAttributeName("iconclass")]
        public string IconClass { get; set; }

        [HtmlAttributeName("buttonclass")]
        public string ButtonClass { get; set; }

        [HtmlAttributeName("display-footer-closebtn")]
        public bool DisplayFooterCloseBtn { get; set; }

        [HtmlAttributeName("display-footer-submitbtn")]
        public bool DisplayFooterSubmitBtn { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("url", Url);
            output.Attributes.Add("editmode", EditType.ToString());
            output.Attributes.Add("visible", true.ToString().ToLowerInvariant());

            if (EditType == DataTableAddEditMode.PopUp)
            {
                output.Attributes.Add("modaltitle", ModalTitle);
                output.Attributes.Add("modalid", ModalId);
                output.Attributes.Add("modalcallbackgrid", ModalCallBackGrid);
                output.Attributes.Add("disabled", Disabled.ToString().ToLower());
                output.Attributes.Add("footerclosebtn", DisplayFooterCloseBtn.ToString().ToLower());
                output.Attributes.Add("footersubmitbtn", DisplayFooterSubmitBtn.ToString().ToLower());

                if (!string.IsNullOrEmpty(IconClass))
                    output.Attributes.Add("iconclass", IconClass);
                else
                    output.Attributes.Add("iconclass", "fas fa-check-circle");

                if (!string.IsNullOrEmpty(ButtonClass))
                    output.Attributes.Add("buttonclass", ButtonClass);
                else
                    output.Attributes.Add("buttonclass", "btn btn-success btn-icon btn-sm btn-rounded");

                string paramlist = string.Empty;
                foreach (var p in ActionParams)
                {
                    paramlist += string.IsNullOrEmpty(paramlist) ? string.Empty : ";";
                    paramlist += string.Format("{0}:{1}", p.Key, p.Value);
                }
                output.Attributes.Add("actionparams", paramlist);
            }
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }

    [HtmlTargetElement("datatable-action-selectrowdata", ParentTag = "datatable-actions", TagStructure = TagStructure.WithoutEndTag)]
    public class JQueryDataTableActionSelectRowData : TagHelper
    {
        [HtmlAttributeName("Disabled")]
        public bool Disabled { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.SelfClosing;
            output.Attributes.Add("disabled", Disabled.ToString().ToLower());
            output.Attributes.Add("visible", true.ToString().ToLowerInvariant());
            base.Process(context, output);
        }
    }

    [HtmlTargetElement("datatable-action-delete", ParentTag = "datatable-actions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableActionDelete : TagHelper
    {
        [HtmlAttributeName("Disabled")]
        public bool Disabled { get; set; }

        [HtmlAttributeName("url")]
        public string Url { get; set; }

        [HtmlAttributeName("displayconfirm")]
        public bool DisplayConfirm { get; set; }

        [HtmlAttributeName("confirmmessage")]
        public string ConfirmMessage { get; set; }

        [HtmlAttributeName("iconclass")]
        public string IconClass { get; set; }

        [HtmlAttributeName("buttonclass")]
        public string ButtonClass { get; set; }

        protected readonly IHttpContextAccessor _contextAccessor;

        public JQueryDataTableActionDelete(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(context.CheckUserPermissionAccess(OperationAction.Delete, _contextAccessor.HttpContext.User))
            {
                output.Attributes.Add("visible", true.ToString().ToLowerInvariant());
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.Add("displayconfirm", DisplayConfirm.ToString().ToLowerInvariant());
                output.Attributes.Add("confirmmessage", ConfirmMessage);
                output.Attributes.Add("disabled", Disabled.ToString().ToLower());
                output.Attributes.Add("url", Url);
                if (context.AllAttributes["iconclass"] != null && !string.IsNullOrEmpty(IconClass))
                {
                    output.Attributes.Add("iconclass", IconClass);
                }
                if (context.AllAttributes["buttonclass"] != null && !string.IsNullOrEmpty(ButtonClass))
                {
                    output.Attributes.Add("buttonclass", ButtonClass);
                }
            }
            else
            {
                output.Attributes.Add("visible", false.ToString().ToLowerInvariant());
            }
        }
    }

    /// <summary>
    /// Action du data table permettant de rajouter l'entité sélectionné à une entre entité à laquelle est est liée.
    /// Par exemple l'ajout d'un agent à une unité d'organisation par sélection de l'agent dans une table qui 
    /// affiche la liste des agents.
    /// </summary>
    [HtmlTargetElement("datatable-action-addto", ParentTag = "datatable-actions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableActionAddFromSelection : TagHelper
    {
        [HtmlAttributeName("Disabled")]
        public bool Disabled { get; set; }

        [HtmlAttributeName("callbackdatatable")]
        public string CallBackDataTable { get; set; }

        [HtmlAttributeName("currentdatatable")]
        public string CurrentDataTable { get; set; }

        [HtmlAttributeName("url")]
        public string Url { get; set; }

        [HtmlAttributeName("foreignentityid")]
        public string ForeignEntityId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.SelfClosing;
            output.Attributes.Add("callbackdatatable", CallBackDataTable);
            output.Attributes.Add("currentdatatable", CurrentDataTable);
            output.Attributes.Add("disabled", Disabled.ToString().ToLower());
            output.Attributes.Add("foreignentityid", ForeignEntityId);
            output.Attributes.Add("url", Url);
            output.Attributes.Add("visible", true.ToString().ToLowerInvariant());
        }
    }

    [HtmlTargetElement("datatable-action-updaterow", ParentTag = "datatable-actions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableActionUpdateRow : TagHelper
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

        protected readonly IHttpContextAccessor _contextAccessor;

        public JQueryDataTableActionUpdateRow(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            ActionParams = new Dictionary<string, string>();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            if (context.CheckUserPermissionAccess(OperationAction.Delete, _contextAccessor.HttpContext.User))
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
