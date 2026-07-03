using System;
using System.Collections.Generic;
using System.Text;
using DvStyle.OpenDataTable.Enums;

namespace DvStyle.OpenDataTable.TableDef
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class JQueryDataTableAttribute : Attribute
    {
        public bool Searchable { get; set; } = true;
        public bool Orderable { get; set; } = true;
        public bool IsForeignKey { get; set; } = false;
        public bool Visible { get; set; } = true;
        public string ForeignKeyName { get; set; }
        public JQueryDataTableSearchInputType SearchType { get; set; } = JQueryDataTableSearchInputType.Text;
        public Type EnumDataType { get; set; }
        public bool IsRequiredCol { get; set; } = false;
        public bool IsColorCol { get; set; }
        public bool OrderableEnum { get; set; } = true;
        public object EnumDefaultValue { get; set; } = null;
        public object InputDefaultValue { get; set; } = null;

        public JQueryDataTableAttribute()
        {

        }
    }
}
