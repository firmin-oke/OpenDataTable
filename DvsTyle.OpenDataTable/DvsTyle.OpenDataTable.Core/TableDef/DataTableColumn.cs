using System;
using System.Collections.Generic;
using System.Text;

namespace DvStyle.OpenDataTable.TableDef
{
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
    }
}
