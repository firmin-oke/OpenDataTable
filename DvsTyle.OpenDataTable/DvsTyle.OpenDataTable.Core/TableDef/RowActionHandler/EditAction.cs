using DvStyle.OpenDataTable.TableDef;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DvsTyle.OpenDataTable.Core.TableDef.RowActionHandler
{

    /// <summary>
    /// Cette classe représente une action d'édition dans un tableau de données. 
    /// Elle est utilisée pour générer un élément HTML correspondant à l'action d'édition, avec des attributs configurables tels que l'URL, le type d'édition, le titre de la modale, etc.
    /// </summary>
    [HtmlTargetElement("datatable-action-edit", ParentTag = "datatable-actions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class EditAction : TagHelper
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

        public bool HasPermission { get; set; } = true;

        public EditAction(IHttpContextAccessor contextAccessor)
        {

        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (HasPermission)
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

                if (DisplayFooterSubmitBtn && !HasPermission)
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
}
