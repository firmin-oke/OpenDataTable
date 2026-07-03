using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DvsTyle.OpenDataTable.Core.TableDef.RowActionHandler
{

    /// <summary>
    /// Défini comment construire une action de suppréssion de ligne dans un tableau de données. 
    /// Cette action est généralement représentée par un bouton ou un lien qui permet à l'utilisateur de supprimer une ligne spécifique du tableau.
    /// </summary>
    [HtmlTargetElement("datatable-action-delete", ParentTag = "datatable-actions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JQueryDataTableActionDelete : TagHelper
    {
        /// <summary>
        /// iNDIQUE SI L'ACTION DE SUPPRESSION EST DÉSACTIVÉE. SI TRUE, LE BOUTON OU LE LIEN DE SUPPRESSION SERA GRISÉ ET NON CLIQUABLE.
        /// </summary>
        [HtmlAttributeName("Disabled")]
        public bool Disabled { get; set; }
        /// <summary>
        /// L'url vers laquelle la requête de suppression sera envoyée. Cette URL est généralement un point de terminaison d'API ou une action de contrôleur qui gère la logique de suppression côté serveur.
        /// </summary>

        [HtmlAttributeName("url")]
        public string Url { get; set; }
        /// <summary>
        /// Indique si un message de confirmation doit être affiché avant d'effectuer l'action de suppression. Si true, une boîte de dialogue de confirmation apparaîtra, demandant à l'utilisateur de confirmer ou d'annuler l'action.
        /// </summary>

        [HtmlAttributeName("displayconfirm")]
        public bool DisplayConfirm { get; set; }
        /// <summary>
        /// Message de confirmation à afficher lorsque DisplayConfirm est true. Ce message informe l'utilisateur de l'action qu'il est sur le point d'effectuer et lui demande de confirmer ou d'annuler.
        /// </summary>

        [HtmlAttributeName("confirmmessage")]
        public string ConfirmMessage { get; set; }

        /// <summary>
        /// classe CSS de l'icône à afficher sur le bouton ou le lien de suppression. Cela permet de personnaliser l'apparence de l'icône en fonction des besoins de l'application.
        /// </summary>

        [HtmlAttributeName("iconclass")]
        public string IconClass { get; set; }

        /// <summary>
        /// Class CSS du bouton de suppression. Cela permet de personnaliser l'apparence du bouton en fonction des besoins de l'application.
        /// </summary>

        [HtmlAttributeName("buttonclass")]
        public string ButtonClass { get; set; }

        public bool HasPermission { get; set; } = true;
        public JQueryDataTableActionDelete(IHttpContextAccessor contextAccessor)
        {
      
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (HasPermission)
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
}
