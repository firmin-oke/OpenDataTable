class DataTableBuilder {
    constructor() {
    }

    static initModalDataTable(selector, modal) {

        $(selector).find(".datagrid").each(function (index, element) {
            if ($(element).attr('id') !== undefined && $(element).attr('id') != null) {
                var datatable = new DataTableInitializer($(element), modal);
                datatable.JqueryDataTableInit();
            }
        });

    }
    static init(selector) {

        $(selector).find(".datagrid").each(function (index, element) {
            if ($(element).attr('id') !== undefined && $(element).attr('id') != null) {
                var datatable = new DataTableInitializer($(element), null);
                datatable.JqueryDataTableInit();
            }
        });
    }

    static initDocument() {
        $(document).find(".datagrid").each(function (index, element) {
            if ($(element).attr('id') !== undefined && $(element).attr('id') != null) {
                var datatable = new DataTableInitializer($(element), null);
                datatable.JqueryDataTableInit();
            }
        });
    }
}