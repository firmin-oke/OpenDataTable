using System;
using System.Collections.Generic;
using System.Text;

namespace DvStyle.OpenDataTable.TableDef
{
    /// <summary>
    /// Représente une colonne dans un DataTable, avec ses propriétés et ses paramètres de recherche.
    /// </summary>
    public class DataTableColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public int Index { get; set; }
        public bool IsFk { get; set; }
        public string FkName { get; set; }

        public DataTableSearch Search { get; set; }
        public DataTableColumn()
        {
            Search = new DataTableSearch();
            Data= string.Empty;
            Name = string.Empty;
            Searchable = false;
            Orderable = false;
            Index = 0;
            IsFk = false;
            FkName = string.Empty;
        }
    }
}
