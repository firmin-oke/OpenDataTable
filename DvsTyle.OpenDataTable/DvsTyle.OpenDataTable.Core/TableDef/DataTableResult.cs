using System;
using System.Collections.Generic;
using System.Text;
using DvStyle.OpenDataTable.Binders;

namespace DvStyle.OpenDataTable.TableDef
{
    public class DataTableResult : DataTableServerRequestResponse
    {
        public object[] Data { get; set; }
        public DataTableResult()
        {
            Data = new object[] { };
        }
    }
}
