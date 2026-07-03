using System;
using System.Collections.Generic;
using System.Text;
using DvStyle.OpenDataTable.Enums;

namespace DvStyle.OpenDataTable.TableDef
{
    /// <summary>
    /// Cette classe à pour rôle les attributs à définir sur une propriété d'un objet pour qu'il soit considée comme intégrable dans un datatable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DataTableModelAttributeDescription : Attribute
    {
        //Permet de saviur si on rend la recherche possible pour cette proprité de l'objet qui représente le modèle de données.
        public bool Searchable { get; set; } = true;
        public bool Orderable { get; set; } = true;
        public bool IsForeignKey { get; set; } = false;
        public bool Visible { get; set; } = true;
        public string ForeignKeyName { get; set; }
        public JQueryDataTableSearchInputType SearchType { get; set; } = JQueryDataTableSearchInputType.Text;
        public Type? EnumDataType { get; set; }
        public bool IsRequiredCol { get; set; } = false;
        public bool IsColorCol { get; set; }
        public bool OrderableEnum { get; set; } = true;
        public object? EnumDefaultValue { get; set; } = null;
        public object? InputDefaultValue { get; set; } = null;

        public DataTableModelAttributeDescription()
        {
            Searchable = true;
            Orderable = true;
            IsForeignKey = false;
            Visible = true;
            ForeignKeyName = string.Empty;
            SearchType = JQueryDataTableSearchInputType.Text;
            IsRequiredCol = false;
            IsColorCol = false;
            OrderableEnum = false;
            EnumDefaultValue = null;
            InputDefaultValue = null;
            EnumDataType = null;
        }
    }
}
