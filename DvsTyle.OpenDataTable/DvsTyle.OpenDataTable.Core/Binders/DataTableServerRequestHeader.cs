using System;
using System.Collections.Generic;
using System.Text;
using DvStyle.OpenDataTable.TableDef;

namespace DvStyle.OpenDataTable.Binders
{
    public class DataTableServerRequestHeader
    {
        public DataTableOrder[] Order { get; set; }

        public DataTableColumn[] Columns { get; set; }

        public DataTableSearch Search { get; set; }

        public DataTableServerRequestHeader(DataTableOrder[] order, DataTableColumn[] columns, DataTableSearch search)
        {
            Order = order;
            Columns = columns;
            Search = search;
            Globalqueryfilters = new KeyValuePair<string, object>[] { };
            UoFilters = new Guid[] { };
        }

        public DataTableServerRequestHeader()
        {
            Order = new DataTableOrder[] { };
            Columns = new DataTableColumn[] { };
            Search = new DataTableSearch();
            Globalqueryfilters = new KeyValuePair<string, object>[] { };
            UoFilters = new Guid[] { };
        }

        public KeyValuePair<string, object>[] Globalqueryfilters { get; set; }

        public DateTime? SearchStartDate { get; set; }

        public DateTime? SearchEndDate { get; set; }

        public Guid[] UoFilters { get; set; }

        public int Length { get; set; }
        public int Draw { get; set; }
        public int Start { get; set; }
    }
}
