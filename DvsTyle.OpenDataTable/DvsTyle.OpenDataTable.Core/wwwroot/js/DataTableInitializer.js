class DataTableInitializer {
    constructor(domObject, modal) {
        this.columndefs = [];
        this.columns = [];
        this.columnsFk = [];
        this.rows_selected = []
        this.processing = true;
        this.serverSide = true;
        this.filter = true;
        this.orderMulti = false;
        this.translateUrl;
        this.Id;
        this.hasFixedColumns;
        this.fixedolumnleft;
        this.fixedcolumnright;
        this.tablebuttons = [];
        this.enabledrowsselection;
        this.rowselectionmode;
        this.paging;
        this.info;
        this.showLengthMenu;
        this.isInLineDataTable;
        this.rowCollectionName;
        this.inlineRowCount = 0;
        this.InLineTableMinRow;
        this.ispickuptable;
        this.domselector;
        this.datasource;
        this.parent;
        this.columndefsselector;
        this.columnsSelector;
        this.dataOptions;
        this.actionsSelectors;
        this.rowdeletebtn;
        this.rowselectbtn;
        this.roweditbtn;
        this.displayDateRangeFilter;
        this.dateRangeFilterStart;
        this.dateRangeFilterEnd;
        this.translateUrl = domObject.data("translateurl");
        this.Id = domObject.attr("id");
        this.grid = $("#" + this.Id);
        this.rowcolors = [];
        this.modal = modal;
        this.globalqueryfilters = [];
        this.displayOrganisationUnitFilter;
        this.organisationUnitFilterAction;
        this.organisationUnitFilterModalTitle;
        this.organisationUnitFilterValues = [];
        this.searchCols = [];
        try {
            this.readDataTableParamerters(domObject);
        }
        catch (xc) {
            console.log(xc);
        }
    }

    readDataTableParamerters(domObject) {

        var $this = this;
        var globalqueryfilterstring;
        this.parent = document.getElementById(this.Id).parentNode;
        this.columndefsselector = this.parent.querySelector("datatable-columndefs");
        this.columnsSelector = this.parent.querySelector("datatable-columns");
        this.datasource = this.parent.querySelector("datatable-settings").getAttribute("datasource");
        this.displayDateRangeFilter = this.parent.querySelector("datatable-settings").getAttribute("displaydaterangefilter") == "true" ? true : false;
        this.dateRangeFilterStart = this.parent.querySelector("datatable-settings").getAttribute("daterange-filter-start");
        this.dateRangeFilterEnd = this.parent.querySelector("datatable-settings").getAttribute("daterange-filter-end");

        this.displayOrganisationUnitFilter = this.parent.querySelector("datatable-settings").getAttribute("displayorganisationunitfilter") == "true" ? true : false;
        this.organisationUnitFilterAction = this.parent.querySelector("datatable-settings").getAttribute("organisationunitfilteraction");
        this.organisationUnitFilterModalTitle = this.parent.querySelector("datatable-settings").getAttribute("organisationunitfiltermodaltitle");

        globalqueryfilterstring = this.parent.querySelector("datatable-settings").getAttribute("globalqueryfilters");

        if (globalqueryfilterstring !== undefined && globalqueryfilterstring != null && globalqueryfilterstring !== "") {
            var splitOne = globalqueryfilterstring.split(";");
            if (splitOne && splitOne.length > 0) {
                $.each(splitOne, function (index, item) {
                    var splitTwo = item.split(":");
                    $this.globalqueryfilters.push({ "key": splitTwo[0], "value": splitTwo[1] });
                });
            }
        }

        if (this.columndefsselector.children.length > 0) {
            var children = this.columndefsselector.children;
            for (var i = 0; i < children.length; i++) {
                var searchable = children[i].getAttribute("searchable") == "true" ? true : false;
                var targets = parseInt(children[i].getAttribute("targets"));
                var visible = children[i].getAttribute("visible") == "true" ? true : false;
                var orderable = children[i].getAttribute("orderable") == "true" ? true : false;
                var wd = children[i].getAttribute("width");
                var isselectrowcheckbox = children[i].getAttribute('isselectrowcheckbox') === "true" ? true : false;
                var colDef = { "searchable": searchable, "targets": targets, "visible": visible, "orderable": orderable };
                if (wd != null && wd !== undefined) {
                    colDef['width'] = wd;
                }
                if (isselectrowcheckbox) {
                    colDef["render"] = function (data, type, row) { return '<input type="checkbox">'; }
                }
                this.columndefs.push(colDef);
            }
        }
        if (this.columnsSelector.children.length > 0) {
            var children = this.columnsSelector.children;
            var i = 0;
            var fkname = null;
            var isFk = false;
            var isdatetime = false;
            for (i = 0; i < children.length; i++) {
                var autoWidth = children[i].getAttribute("autoWidth") == "true" ? true : false;
                isFk = children[i].getAttribute("isfk") == "true" ? true : false;
                isdatetime = children[i].getAttribute("isdatetime") == "true" ? true : false;
                fkname = children[i].getAttribute("fkname");
                var data = children[i].getAttribute("data");
                var name = children[i].getAttribute("name");
                var searchable = children[i].getAttribute("searchable") === "true" ? true : false;
                var orderable = children[i].getAttribute("orderable") === "true" ? true : false;
                var searchtype = children[i].getAttribute("searchtype");
                var isbool = children[i].getAttribute("isbool");
                var enumvalues = children[i].getAttribute("enumvalues");
                var enumdefaultvalue = children[i].getAttribute("enumdefaultvalue");
                var inputdefaultvalue = children[i].getAttribute("inputdefaultvalue");

                var iscolorcolumn = children[i].getAttribute("iscolorcol") === "true" ? true : false;
                var isrequiredcol = children[i].getAttribute("isrequiredcol") === "true" ? true : false;
                var colobj = {
                    "autoWidth": autoWidth, "name": name, "data": data, "searchable": searchable,
                    "orderable": orderable, "searchtype": searchtype, "enumvalues": enumvalues, "isrequiredcol": isrequiredcol,
                    "enumdefaultvalue": enumdefaultvalue, "inputdefaultvalue": inputdefaultvalue
                };
                if ( enumdefaultvalue !== null && enumdefaultvalue !== undefined) {
                    this.searchCols.push({"search": enumdefaultvalue });
                }
                else if (inputdefaultvalue !== null && inputdefaultvalue !== undefined)
                {
                    this.searchCols.push({"search": inputdefaultvalue });
                }
                else {
                    this.searchCols.push(null);
                }
                this.columns.push(colobj);
                if ((fkname != undefined && fkname != null && fkname != "") && isFk == true) {
                    this.columnsFk.push({ "fk": fkname, "col": data });
                }
            }
        }
        this.dataOptions = this.parent.querySelector("datatable-dataoptions");
        this.processing = this.dataOptions.getAttribute("processing") == "true" ? true : false;
        this.serverSide = this.dataOptions.getAttribute("serverSide") == "true" ? true : false;
        this.orderMulti = this.dataOptions.getAttribute("ordermulti") == "true" ? true : false;
        this.filter = this.dataOptions.getAttribute("filter") == "true" ? true : false;
        this.paging = this.dataOptions.getAttribute("paging") == "true" ? true : false;
        this.info = this.dataOptions.getAttribute("info") == "true" ? true : false;
        this.showLengthMenu = this.dataOptions.getAttribute("showlengthmenu") == "true" ? true : false;
        this.InLineTableMinRow = parseInt(this.dataOptions.getAttribute("inlinetableminrow"));
        this.isInLineDataTable = this.dataOptions.getAttribute("isinLinedatatable") == "true" ? true : false;
        this.rowCollectionName = this.dataOptions.getAttribute("rowcollectionname");
        this.hasFixedColumns = this.dataOptions.getAttribute("hasfixedcolumns") == "true" ? true : false;
        this.fixedolumnleft = parseInt(this.dataOptions.getAttribute("fixedcolumnleft"));
        this.fixedcolumnright = parseInt(this.dataOptions.getAttribute("fixedcolumnright"));
        this.enabledrowsselection = this.dataOptions.getAttribute("enabledrowselection") == "true" ? true : false;
        this.rowselectionmode = this.dataOptions.getAttribute("rowselectionmode");
        this.ispickuptable = this.dataOptions.getAttribute("ispickuptable") == "true" ? true : false;

        if (this.dataOptions.children.length > 0) {
            var children = this.dataOptions.children;
            for (var i = 0; i < children.length; i++) {
                var name = children[i].getAttribute("name");
                var iconclass = children[i].getAttribute("iconclass");
                var buttonclass = children[i].getAttribute("buttonclass");
                buttonclass += " mr-1";
                if (name == "csv") {
                    var filename = children[i].getAttribute("filename");
                    var title = children[i].getAttribute("filetitle");
                    this.tablebuttons.push({
                        extend: 'excelHtml5',
                        text: '<i class="' + iconclass + '"></i>',
                        className: buttonclass,
                        charset: 'UTF-8',
                        fieldSeparator: ';',
                        bom: true,
                        filename: filename,
                        title: title,
                        exportOptions: {
                            format: {
                                body: function (data, row, column, node) {
                                    if (data === false) {
                                        return 'Non';
                                    }
                                    else if (data === true) {
                                        return 'Oui'
                                    }
                                    else {
                                        if (data !== null && data !== '' && data !== undefined) {

                                            if ($.isNumeric(data.toString().replace(',', '.'))) {
                                                return data.toString().replace(',', '.');
                                            }
                                            else
                                                return data;
                                        }
                                        else {
                                            return data;
                                        }
                                    }
                                }
                            },
                            columns: ':not(.notexportable)'
                        }
                    });
                }
                else {
                    this.tablebuttons.push({ extend: name, text: '<i class="' + iconclass + '"></i>', className: buttonclass });
                }
            }
        }
        this.rowcolors = [];
        var rowcolordefs = this.parent.querySelectorAll("rowcolor");
        [].forEach.call(rowcolordefs, function (rowcolordef) {
            var colcolorname = rowcolordef.getAttribute('colname');
            var colorsArray = [];
            if (rowcolordef.children.length > 0) {
                var children = rowcolordef.children;
                for (var i = 0; i < children.length; i++) {
                    var ch = children[i];
                    var value = ch.getAttribute('value') === "true" ? true : false;
                    colorsArray.push({ value: value, class: ch.getAttribute('class') })
                }
                $this.rowcolors.push({ colname: colcolorname, colors: colorsArray });
            }
        });
        this.actionsSelectors = this.parent.querySelector("datatable-actions");
        if (this.actionsSelectors != undefined && this.actionsSelectors != null) {
            if (this.actionsSelectors.children.length > 0) {
                var children = this.actionsSelectors.children;
                var btns = '';
                var btnParams = [];
                var iconclass = '';
                var buttonclass = '';
                var handlerclass = '';
                var bntQueryselector;
                for (var i = 0; i < children.length; i++) {
                    btnParams = [];
                    bntQueryselector = children[i];
                    var disabledButton = bntQueryselector.getAttribute("disabled") == "true" ? true : false;
                    var visible = bntQueryselector.getAttribute("visible") == "true" ? true : false;
                    if (visible) {
                        if (bntQueryselector.tagName.toLocaleLowerCase() == "datatable-action-downloadfile") {
                            iconclass = bntQueryselector.getAttribute("iconclass");
                            buttonclass = bntQueryselector.getAttribute("buttonclass");
                            iconclass = iconclass == null || iconclass == "" ? "fas fa-arrow-alt-circle-down" : iconclass;
                            buttonclass = buttonclass == null || buttonclass == "" ? "btn btn-icon btn-rounded btn-success" : buttonclass;
                            btnParams.push({ "key": "url", value: bntQueryselector.getAttribute("url") });
                            handlerclass = "datatable-download-btn";
                            btns += this.initDataTableButtons(btnParams, buttonclass, iconclass, disabledButton, handlerclass);
                        }
                        if (bntQueryselector.tagName.toLowerCase() == "datatable-action-edit") {
                            btnParams.push({ "key": "pagetitle", value: bntQueryselector.getAttribute("pagetitle") });
                            btnParams.push({ "key": "url", value: bntQueryselector.getAttribute("url") });
                            btnParams.push({ "key": "editmode", value: bntQueryselector.getAttribute("editmode") });
                            btnParams.push({ "key": "modaltitle", value: bntQueryselector.getAttribute("modaltitle") });
                            btnParams.push({ "key": "modalid", value: bntQueryselector.getAttribute("modalid") });
                            btnParams.push({ "key": "footerclosebtn", value: bntQueryselector.getAttribute("footerclosebtn") });
                            btnParams.push({ "key": "footersubmitbtn", value: bntQueryselector.getAttribute("footersubmitbtn") });
                            btnParams.push({ "key": "modalcallbackgrid", value: bntQueryselector.getAttribute("modalcallbackgrid") });
                            btnParams.push({ "key": "modalsize", value: bntQueryselector.getAttribute("modalsize") });
                            btnParams.push({ "key": "isnotedit", value: bntQueryselector.getAttribute("isnotedit") });
                            
                            iconclass = bntQueryselector.getAttribute("iconclass");
                            buttonclass = bntQueryselector.getAttribute("buttonclass");
                            iconclass = iconclass == null || iconclass == "" ? "feather icon-edit" : iconclass;
                            buttonclass = buttonclass == null || buttonclass == "" ? "btn btn-icon btn-rounded btn-secondary" : buttonclass;
                            handlerclass = "datatable-edit-btn";
                            btns += this.initDataTableButtons(btnParams, buttonclass, iconclass, disabledButton, handlerclass);
                        }
                        else if (bntQueryselector.tagName.toLowerCase() == "datatable-action-delete") {
                            btnParams.push({ "key": "displayconfirm", value: bntQueryselector.getAttribute("displayconfirm") });
                            btnParams.push({ "key": "confirmmessage", value: bntQueryselector.getAttribute("confirmmessage") });
                            btnParams.push({ "key": "url", value: bntQueryselector.getAttribute("url") });

                            iconclass = bntQueryselector.getAttribute("iconclass");
                            buttonclass = bntQueryselector.getAttribute("buttonclass");
                            iconclass = iconclass == null || iconclass == "" ? "ti-trash" : iconclass;
                            buttonclass = buttonclass == null || buttonclass == "" ? "btn btn-icon btn-rounded btn-danger" : buttonclass;
                            handlerclass = "datatable-delete-btn";
                            btns += this.initDataTableButtons(btnParams, buttonclass, iconclass, disabledButton, handlerclass);
                        }
                        else if (bntQueryselector.tagName.toLowerCase() == "datatable-action-newmodalfromrow") {

                            try {
                                iconclass = bntQueryselector.getAttribute("iconclass");
                                buttonclass = bntQueryselector.getAttribute("buttonclass");
                                handlerclass = "datatable-open-newmodalfromrow";
                                btnParams.push({ "key": "url", value: bntQueryselector.getAttribute("url") });
                                btnParams.push({ "key": "modaltitle", value: bntQueryselector.getAttribute("modaltitle") });
                                btnParams.push({ "key": "editmode", value: bntQueryselector.getAttribute("editmode") });
                                btnParams.push({ "key": "footerclosebtn", value: bntQueryselector.getAttribute("footerclosebtn") });
                                btnParams.push({ "key": "footersubmitbtn", value: bntQueryselector.getAttribute("footersubmitbtn") });
                                btnParams.push({ "key": "modalid", value: bntQueryselector.getAttribute("modalid") });
                                btnParams.push({ "key": "modalcallbackgrid", value: bntQueryselector.getAttribute("modalcallbackgrid") });
                                btnParams.push({ "key": "actionparams", value: bntQueryselector.getAttribute("actionparams") });
                                btns += this.initDataTableButtons(btnParams, buttonclass, iconclass, disabledButton, handlerclass);
                            }
                            catch (exp) {
                                console.log(exp);
                            }
                        }
                        else if (bntQueryselector.tagName.toLowerCase() == "datatable-action-selectrowdata") {
                            iconclass = "fas fa-check-circle";
                            buttonclass = "btn btn-success btn-rounded btn-icon";
                            handlerclass = "datatable-btn-selectrowdata";
                            btns += this.initDataTableButtons(btnParams, buttonclass, iconclass, disabledButton, handlerclass);
                        }
                        else if (bntQueryselector.tagName.toLowerCase() == "datatable-action-addto") {
                            btnParams.push({ "key": "callbackdatatable", value: bntQueryselector.getAttribute("callbackdatatable") });
                            btnParams.push({ "key": "currentdatatable", value: bntQueryselector.getAttribute("currentdatatable") });
                            btnParams.push({ "key": "foreignentityid", value: bntQueryselector.getAttribute("foreignentityid") });
                            btnParams.push({ "key": "url", value: bntQueryselector.getAttribute("url") });
                            iconclass = "fas fa-check-circle";
                            buttonclass = "btn btn-icon btn-rounded btn-success";
                            handlerclass = "datatable-btn-addto";
                            btns += this.initDataTableButtons(btnParams, buttonclass, iconclass, disabledButton, handlerclass);
                        }
                        else if (bntQueryselector.tagName.toLowerCase() == "datatable-action-updaterow") {
                            btnParams.push({ "key": "tooltipaction", value: bntQueryselector.getAttribute("tooltipaction") });
                            btnParams.push({ "key": "callbackgrid", value: bntQueryselector.getAttribute("callbackgrid") });
                            btnParams.push({ "key": "actionparams", value: bntQueryselector.getAttribute("actionparams") });
                            iconclass = bntQueryselector.getAttribute("iconclass");
                            buttonclass = bntQueryselector.getAttribute("buttonclass");
                            handlerclass = "datatable-btn-updaterow";
                            btns += this.initDataTableButtons(btnParams, buttonclass, iconclass, disabledButton, handlerclass);
                        }
                    }
                }
                //if (btns != '') {
                   
                //}
                this.columns.push({ "autoWidth": false, className: 'notexportable', mRender: function (data, type, row) { return btns; } });
            }
            else {
                //if (this.columndefs.length > 1) {
                //    this.columns.push({ "autoWidth": false, className: 'notexportable', mRender: function (data, type, row) { return ''; } });
                //}
            }
        }
    }

    initDataTableButtons(params, btncssclass, iconclass, isDisabled, handlerclass) {
        var btn = $("<button>");
        try {
            btn.addClass(btncssclass);
            btn.addClass(handlerclass);
            var icon = $("<i>");
            icon.addClass(iconclass);
            btn.append(icon);
            if (isDisabled) {
                btn.attr("disabled", "disabled");
                btn.addClass("disabled");
            }
            if (params !== null && params != undefined) {
                for (var z = 0; z < params.length; z++) {
                    btn.attr("data-" + params[z].key.toLowerCase(), params[z].value);
                }
            }
        }
        catch (exp) {
            console.log(exp);
        }
        return btn[0].outerHTML;
    }
    JqueryDataTableInit() {
        var $this = this;
        var options = {
            "language": {
                "decimal": ",",
                "sEmptyTable": "Aucune donnée disponible",
                "sInfo": "Affichage de l'élément _START_ à _END_ sur _TOTAL_ éléments",
                "sInfoEmpty": "Affichage de l'élément 0 à 0 sur 0 élément",
                "sInfoFiltered": "(filtré à partir de _MAX_ éléments au total)",
                "sInfoPostFix": "",
                "sInfoThousands": ",",
                "sLengthMenu": "_MENU_",
                "sLoadingRecords": "Chargement...",
                "sProcessing": "Traitement...",
                "sSearch": "",
                "searchPlaceholder": "Recherche globale",
                "sZeroRecords": "Aucun élément correspondant trouvé",
                "oPaginate": {
                    "sFirst": "Premier",
                    "sLast": "Dernier",
                    "sNext": "Suivant",
                    "sPrevious": "Précédent"
                },
                "oAria": {
                    "sSortAscending": ": activer pour trier la colonne par ordre croissant",
                    "sSortDescending": ": activer pour trier la colonne par ordre décroissant"
                },
                "select": {
                    "rows": {
                        "_": "%d lignes sélectionnées",
                        "0": "Aucune ligne sélectionnée",
                        "1": "1 ligne sélectionnée"
                    }
                }
            },
            "processing": this.processing,
            "serverSide": this.serverSide,
            "filter": this.filter,
            "orderMulti": this.orderMulti,
            "destroy": true,
           "searchCols": this.searchCols,
            orderCellsTop: true,
            scrollCollapse: true,
            order: [],
            scrollX: true,
            scrollY:'450px',
            rowId: 'Id',
            paging: this.paging,
            info : this.info,
            rowCallback: function (row, data, dataIndex) {
                if ($this.ispickuptable == true) {
                    $(row).addClass("datatable-btn-selectrowdata");
                }
            },
            createdRow: function (row, data, dataIndex) {
                var notsearcheablecol = [];
                [].slice.call($this.columndefs).forEach(function (item, ich) {
                    if (item.searcheable == false) {
                        notsearcheablecol.push(item.targets);
                    }
                });
                this.api().columns().every(function (index) {
                    try {
                        var column = this;
                        if (!notsearcheablecol.includes(index)) {
                            if ($this.columns[index].searchable) {
                                if ($this.rowcolors.length > 0) {
                                    var iscolcolor = $this.rowcolors.find(data => data.colname === $this.columns[index].data);
                                    if (iscolcolor != undefined) {
                                        var coldatas = column.data();
                                        [].slice.call(coldatas).forEach(function (cdata, cid) {
                                            var valfind = iscolcolor.colors.find(xx => xx.value === cdata)
                                            if (valfind != undefined) {
                                                $(column.nodes()[cid]).addClass(valfind.class);
                                                $(column.nodes()[cid]).html('');
                                            }
                                        });
                                    }
                                }
                            }
                        }
                    }
                    catch (xy) {
                        console.log(xy);
                    }
                });
            },
            initComplete: function () {   
                var table = this.api().table();
                var container = $this.grid.closest('.datatablerootcontainer');
                table.buttons().containers().appendTo('#memorydatatablemenu_' + container.attr("id"));
                var sb = $(table.container()).children('.dataTables_filter').first();
                var lm = $(table.container()).children('.dataTables_length').first(); 
                lm.addClass("mr-2");
                sb.css("float", "right");
                lm.css("float", "right");  
                sb.appendTo($('#memorydatatablemenu_' + container.attr("id")).parent().eq(0));
                lm.appendTo($('#memorydatatablemenu_' + container.attr("id")).parent().eq(0));
                if ($this.displayDateRangeFilter) {
                    var blck = $('<div class="dropdown mr-2" style="float:right"></div>');
                    var link = $('<button class="dropdown-toggle btn btn-dark btn-icon" data-toggle="dropdown"></button>');
                    link.append('<i class="feather icon-calendar"></i>');
                    blck.append(link);
                    var cont = $('<div class="dropdown-menu dropdown-menu-right" style="width:250px">');
                    var form = $('<div class="container-fluid memory-dropdown"></div>');
                    var inputdate_start = $('<div class="form-group"><label>Début</label><input class="col-12 form-control form-control-sm" type="date" id="dt_date_filter_start_' + container.attr("id") + '" value="' + $this.dateRangeFilterStart+'" /></div>');
                    var inputdate_end = $('<div class="form-group"><label>Fin</label><input class="col-12 form-control form-control-sm" type="date" id="dt_date_filter_end_' + container.attr("id") + '" value="' + $this.dateRangeFilterEnd +'" /></div>');
                    form.append(inputdate_start);
                    form.append(inputdate_end);
                    cont.append(form);
                    blck.append(cont);
                    blck.appendTo($('#memorydatatablemenu_' + container.attr("id")).parent().eq(0));
                }
                if ($this.displayOrganisationUnitFilter) {
                    var btnfilter = $('<button class="bg-red btn btn-icon fg-white displayOrganisationUnitFilterModal" style="float:right"></button>');
                    btnfilter.attr("data-uofilteraction", $this.organisationUnitFilterAction);
                    btnfilter.attr("data-modaltitle", $this.organisationUnitFilterModalTitle);
                    var filtericon = $('<i class="mdi mdi-sitemap"></i>');
                    btnfilter.append(filtericon);
                    btnfilter.appendTo($('#memorydatatablemenu_' + container.attr("id")).parent().eq(0));
                }
                var tableaheader = this.api().table().header();
                var notsearcheablecol = [];
                [].slice.call($this.columndefs).forEach(function (item, ich) {
                    if (item.searcheable == false) {
                        notsearcheablecol.push(item.targets);
                    }
                });
                if ($(tableaheader).children().length > 1) {
                    var searchableheader = $(tableaheader).children().last();
                    this.api().columns().every(function (index) {
                        try {
                            var column = this;
                            var searchzone = searchableheader.children().eq(index);
                            if (!notsearcheablecol.includes(index)) {
                                if ($this.columns[index].searchable) {
                                    var r = genereanteUID();
                                    var colid = r + "_" + $this.Id + "_colheader_search_" + index;
                                    var colid_clear = r + "_" + $this.Id + "_colheader_search_clear_" + index;
                                    if ($this.columns[index].searchtype == "enumlist") {
                                        var select = $('<select class="col-12 form-control-sm"><option value=""></option></select>');
                                        select.attr("id", colid);
                                        var selectblock = $('<div class="d-inline-flex">');
                                        var clear = $(' <span title="Supprimé" style="visibility:hidden"> <i class="feather icon-x"></i></span>');
                                        clear.attr("id", colid_clear);
                                        selectblock.append(select);
                                        selectblock.append(clear);
                                        selectblock.appendTo(searchzone.empty());
                                        var options = $this.columns[index].enumvalues.split(";");
                                        var edefaultval = $this.columns[index].enumdefaultvalue;
                                        [].slice.call(options).forEach(function (op, idx) {
                                            var option = op.split(",");
                                            if (edefaultval == option[0]) {
                                                select.append('<option value="' + option[0] + '" selected>' + option[1] + '</option>');
                                                column.defaultContent = option[0];
                                            }
                                            else {
                                                select.append('<option value="' + option[0] + '">' + option[1] + '</option>');
                                            }
                                        });
                                        $(table.container()).on('change', '#' + colid, function () {
                                            $("#" + colid_clear).css("visibility", ($(this).val().length) ? "visible" : "hidden");
                                            var val = $.fn.dataTable.util.escapeRegex($(this).val());
                                            column.search(val ? val : '', true, false).draw();
                                        });
                                        $(table.container()).on('click', '#' + colid_clear, function () {
                                            $("#" + colid_clear).css("visibility", "hidden");
                                            $('#' + colid).val('');
                                            $('#' + colid).trigger('change');
                                        });
                                    }
                                    else if ($this.columns[index].searchtype == "calendar") {
                                       
                                        var inputdate = $('<input class="col-12 form-control form-control-sm" type="date"/>');
                                        var edefaultval = $this.columns[index].inputdefaultvalue;
                                        if (edefaultval !== null && edefaultval !== undefined) {
                                            inputdate.val(edefaultval);
                                        }

                                        inputdate.attr("id", colid);
                                        var block = $('<div class="input-group-sm"></div>');
                                        var clear = $(' <span title="Supprimé" style="visibility:hidden"> <i class="feather icon-x"></i></span>');
                                        clear.attr("id", colid_clear);
                                        block.append(inputdate);
                                        block.append(clear);
                                        block.appendTo(searchzone.empty());
                                        $(table.container()).on('change', '#' + colid, function () {
                                            $("#" + colid_clear).css("visibility", ($(this).val().length) ? "visible" : "hidden");
                                            var val = $.fn.dataTable.util.escapeRegex($(this).val());
                                            column.search(val ? val : '', true, false).draw();
                                        });
                                        $(table.container()).on('click', '#' + colid_clear, function () {
                                            $("#" + colid_clear).css("visibility", "hidden");
                                            $("#" + colid).val('');
                                            $("#" + colid).trigger('change');
                                        });
                                    }
                                    else if ($this.columns[index].searchtype == "timecalendar") {
                                        var inputdate = $('<input class="col-12 form-control form-control-sm" type="time"/>');
                                        var edefaultval = $this.columns[index].inputdefaultvalue;
                                        if (edefaultval !== null && edefaultval !== undefined) {
                                            inputdate.val(edefaultval);
                                        }
                                        inputdate.attr("id", colid);
                                        var block = $('<div class="input-group-sm"></div>');
                                        var clear = $(' <span title="Supprimé" style="visibility:hidden"> <i class="feather icon-x"></i></span>');
                                        clear.attr("id", colid_clear);
                                        block.append(inputdate);
                                        block.append(clear);
                                        block.appendTo(searchzone.empty());
                                        $(table.container()).on('change', '#' + colid, function () {
                                            $("#" + colid_clear).css("visibility", ($(this).val().length) ? "visible" : "hidden");
                                            var val = $.fn.dataTable.util.escapeRegex($(this).val());
                                            column.search(val ? val : '', true, false).draw();
                                        });
                                        $(table.container()).on('click', '#' + colid_clear, function () {
                                            $("#" + colid_clear).css("visibility", "hidden");
                                            $("#" + colid).val('');
                                            $("#" + colid).trigger('change');
                                        });
                                    }
                                    else if ($this.columns[index].searchtype == "number") {
                                        var inputtext = $('<input class="col-12 form-control form-control-sm" type="number" min="1"/>');
                                        var edefaultval = $this.columns[index].inputdefaultvalue;
                                        if (edefaultval !== null && edefaultval !== undefined) {
                                            inputtext.val(edefaultval);
                                        }
                                        inputtext.attr("id", colid);
                                        var block = $('<div class="input-group-sm">');
                                        var clear = $(' <span title="Supprimé" style="visibility:hidden"> <i class="feather icon-x"></i></span>');
                                        clear.attr("id", colid_clear);
                                        block.append(inputtext);
                                        block.append(clear);
                                        block.appendTo(searchzone.empty());
                                        $(table.container()).on('keyup change', '#' + colid, function () {
                                            $("#" + colid_clear).css("visibility", ($(this).val().length) ? "visible" : "hidden");
                                            var val = $.fn.dataTable.util.escapeRegex($(this).val());
                                            column.search(val ? val : '', true, false).draw();
                                        });
                                        $(table.container()).on('click', '#' + colid_clear, function () {
                                            $("#" + colid_clear).css("visibility", "hidden");
                                            $("#" + colid).val('');
                                            $("#" + colid).trigger('change');
                                        });
                                    }
                                    else {
                                        var inputtext = $('<input class="col-12 form-control form-control-sm" type="text"/>');
                                        var edefaultval = $this.columns[index].inputdefaultvalue;
                                        if (edefaultval !== null && edefaultval !== undefined) {
                                            inputtext.val(edefaultval);
                                        }
                                        inputtext.attr("id", colid);
                                        var block = $('<div class="input-group-sm">');
                                        var clear = $('<span title="Supprimé" style="visibility:hidden"> <i class="feather icon-x"></i></span>');
                                        clear.attr("id", colid_clear);
                                        block.append(inputtext);
                                        block.append(clear);
                                        block.appendTo(searchzone.empty());
                                        $(table.container()).on('keyup change', '#' + colid, function () {
                                            $("#" + colid_clear).css("visibility", ($(this).val().length) ? "visible" : "hidden");
                                            var val = $.fn.dataTable.util.escapeRegex($(this).val());
                                            column.search(val ? val : '', true, false).draw();
                                        });
                                        $(table.container()).on('click', '#' + colid_clear, function () {
                                            $("#" + colid_clear).css("visibility", "hidden");
                                            $("#" + colid).val('');
                                            $("#" + colid).trigger('change');
                                        });
                                    }
                                }
                                else {
                                    searchzone.empty();
                                }
                            }
                        }
                        catch (xy) {
                            console.log(xy);
                        }
                    });
                }
            }
        };
        if (this.datasource != undefined && this.datasource != "" && this.datasource != null) {
            var c = this.grid.closest('.datatablerootcontainer');
            options.ajax = {
                url: this.datasource,
                type: "POST",
                data: {
                    columnfk: this.columnsFk,
                    globalqueryfilters: this.globalqueryfilters,
                    dtperiodestart: this.dateRangeFilterStart,
                    dtperiodeend: this.dateRangeFilterEnd,
                    organisationunitfilters: this.organisationUnitFilterValues
                },
                datatype: "json",
                error: function (xhr, error, thrown) {
                    console.log(error);
                }
            }
        }
        if (this.showLengthMenu) {
            options.lengthMenu = [20, 50, 100,200,500];
        }
        if (!this.isInLineDataTable)
        {
            options.columnDefs = this.columndefs;
            options.columns = this.columns;
        }
        if (this.enabledrowsselection && (this.rowselectionmode !== undefined && this.rowselectionmode != null)) {
            options.select = {
                style: this.rowselectionmode
            }
        }
        if (this.tablebuttons.length > 0) {
            options.dom = 'Blfrt<"row mt-2"<"col-12"i>><"row mt-2"<"col-12"p>><"clear">'
            options.buttons = this.tablebuttons;
        }
        else {
            options.dom = 'lfrt<"row mt-2"<"col-12"i>><"row text-center mt-2"<"col-12"p>><"clear">'
        }
        if (this.hasFixedColumns) {
            options.fixedColumns = {
                leftColumns: $this.fixedolumnleft > 0 ? $this.fixedolumnleft : undefined,
                rightColumns: $this.fixedcolumnright > 0 ? $this.fixedcolumnright : undefined
            };
        }

        this.grid.DataTable(options);
        this.initExternalTooltipHandler();
        this.initModalTooltipHandler();
        this.dataTableEditRowAction();
        this.deleteDataTableEventHandler();
        this.openNewModalFromRowSelection();
        this.refreshDataTableHandler();
        this.selectDataTableRowData();
        this.dataTableAddToEventHandler();
        this.dataTableUpdateRowBtnEventHandler();
        this.dataTableDownLoadEventHandler();
        this.initToolTipItemBulk();
        this.inittooltipItemModal();
        this.inlineToolTipAddRow();
        this.inlineToolTipDeleteRow();
        if (this.isInLineDataTable && this.InLineTableMinRow>0) {
             this.addInlineRow(this);
        }
        this.refreshDataTableOnPeriodeDateChange();
        this.displayOrganisationFilterModal();
    }
    dataTableEditRowAction() {
        var $this = this;
        $this.grid.on('click', '.datatable-edit-btn', function (e) {
            e.preventDefault();
            var data = $this.grid.DataTable().row($(this).parents('tr')).data();
            var contenturl = $(this).data("url");
            var editmode = $(this).data("editmode");
            var pageTitle = $(this).data("pagetitle");
            var isnotedit = $(this).data("isnotedit") == "true" ? true : false;
            switch (editmode) {
                case 'External':
                    if (window.MemoryCurrentModal != null && window.MemoryCurrentModal != undefined) {
                        window.MemoryCurrentModal.window.modal('hide').delay(5000);
                    }
                    var $hash = window.location.hash;
                    $hash = $hash.replace('#', '');
                    if (isnotedit) {
                        window.location.hash = '#New|' + $hash + '|' + contenturl + '/' + data.Id + '|' + pageTitle;
                    }
                    else {
                        window.location.hash = '#Edit|' + $hash + '|' + contenturl + '/' + data.Id + '|' + pageTitle;
                    }
                    break;
                case 'PopUp':
                    var modalid = $(this).data("modalid");
                    var title = $(this).data("modaltitle");
                    var modalsize =  $(this).data("modalsize");
                    var modalcallbackgrid = $(this).data("modalcallbackgrid");
                    var modalselector = "#" + modalid;
                    var displaysubmit = $(this).data("footersubmitbtn") == true ? true : false;
                    var displayclosebtn = $(this).data("footerclosebtn") == true ? true : false;

                    var m = new DataTableModal({
                        id: modalid,
                        header: title,
                        footer: true,
                        callbackgrid: modalcallbackgrid,
                        displayCloseBtn: displayclosebtn,
                        displaySubmitBtn: displaysubmit,
                        modalsize: modalsize
                    });
                    $.get(contenturl, { Id: data.Id }, function (content) {
                        m.getBody().html(content).promise().done(function () {
                            m.initAjaxComponent();
                            m.show();
                        });
                    });
                    break;
                case 'Custom':
                    var modalid = $(this).data("modalid");
                    var title = $(this).data("modaltitle")
                    var modalcallbackgrid = $(this).data("modalcallbackgrid");
                    var displaysubmit = $(this).data("footersubmitbtn") == true ? true : false;
                    var displayclosebtn = $(this).data("footerclosebtn") == true ? true : false;
                    var modalsize = $(this).data("modalsize");

                    var modaltemplate =
                    {
                        id: modalid,
                        header: title,
                        footer: true,
                        callbackgrid: modalcallbackgrid,
                        displayCloseBtn: displayclosebtn,
                        displaySubmitBtn: displaysubmit,
                        modalsize: modalsize
                    };
                    var handler = new CustomJsonResponseHandler({ newmodaltemplate: modaltemplate, customdatatable: modalcallbackgrid});
                    $.get(contenturl, { Id: data.Id }, function (content) {
                        handler.parseAjaxResponse(content);
                    });
                    break;
            }
        });
    }

    dataTableDownLoadEventHandler() {
        var $this = this;
        this.grid.on('click', '.datatable-download-btn', function (e) {
            e.preventDefault();
            e.stopImmediatePropagation();
            var data = $this.grid.DataTable().row($(this).parents('tr')).data();
            var partialloc = $(this).data("url");
            var loc = window.location;
            var location = loc.protocol + '//' + loc.hostname + (loc.port ? ':' + loc.port : '') + partialloc;

            var url = new URL(location);
            url.searchParams.append('Id', data.Id);
            window.location = url;
        });
    }

    dataTableAddToEventHandler() {
        var $this = this;
        this.grid.on('click', '.datatable-btn-addto', function (e) {
            e.preventDefault();
            var callbackdatatable = $(this).data("callbackdatatable");
            var currentdatatable = $(this).data("currentdatatable");
            var foreignentityid = $(this).data("foreignentityid");
            var url = $(this).data("url");
            var data = $this.grid.DataTable().row($(this).parents('tr')).data();
            var datatableSelector = $("#" + callbackdatatable);
            $.get(url, { id: data.Id, foreignentityId: foreignentityid }, function (callbackresult) {
                //Check if data table is in modal window
                if ($this.modal != null) {
                    $this.modal.window.modal('hide').promise().done(function () {
                        datatableSelector.DataTable().ajax.reload();
                    });
                }
                else {
                    datatableSelector.DataTable().ajax.reload();
                }
            });
        });
    }

    dataTableUpdateRowBtnEventHandler() {
        var $this = this;
        this.grid.on('click', '.datatable-btn-updaterow', function (e) {
            e.preventDefault();
            var data = $this.grid.DataTable().row($(this).parents('tr')).data();
            var action = $(this).data("tooltipaction");
            var actionparams = $(this).data("actionparams");
            var requestparams = {};
            var parsedParams = {};
            if (actionparams !== undefined && actionparams != null) {
                parsedParams = actionparams.split(";");
                $.each(parsedParams, function (index, item) {
                    if (item !== "" && item !== undefined && item != null) {
                        var param = item.split(":");
                        requestparams[param[0]] = param[1];
                    }
                });
            }
            if (action !== undefined && action != null) {
                requestparams['Id'] = data.Id;
                $.get(action, requestparams, function (content) {
                    try {
                        var handler = new CustomJsonResponseHandler({ customdatatable: $this.Id });
                        handler.parseAjaxResponse(content);
                    }
                    catch (xy) {
                        console.log(xy);
                    }
                });
            }
        });
    }
    openNewModalFromRowSelection() {
        var $this = this;
        this.grid.on('click', '.datatable-open-newmodalfromrow', function (e) {
            e.preventDefault();
            var data = $this.grid.DataTable().row($(this).parents('tr')).data();
            var contenturl = $(this).data("url");
            var editmode = $(this).data("editmode");;
            var actionparams = $(this).data("actionparams");
            var requestparams = {};
            var parsedParams = {};
            if (actionparams !== undefined && actionparams != null) {
                parsedParams = actionparams.split(";");
                $.each(parsedParams, function (index, item) {
                    if (item !== "" && item !== undefined && item != null) {
                        var param = item.split(":");
                        requestparams[param[0]] = param[1];
                    }
                });
            }
            switch (editmode) {
                case 'External':
                    if ($this.modal != null) {
                        $this.modal.window.modal('hide').promise().done(function () {
                            requestparams['Id'] = data.Id;
                            $.get(contenturl, requestparams, function (content) {
                                var selector = MemoryAppSettings.MainDataContainer;
                                $(selector).html(content).promise().done(function () {
                                    $(selector).trigger(MemoryAppSettings.MainDataConatinerHtmlChangeEvent);
                                });
                            });
                        });
                    }
                    else {
                        requestparams['Id'] = data.Id;
                        $.get(contenturl, requestparams, function (content) {
                            var selector = MemoryAppSettings.MainDataContainer;
                            $(selector).html(content).promise().done(function () {
                                $(selector).trigger(MemoryAppSettings.MainDataConatinerHtmlChangeEvent);
                            });
                        });
                    }
                    break;

                case 'PopUp':
                    var modalid = $(this).data("modalid");
                    var title = $(this).data("modaltitle")
                    var modalcallbackgrid = $(this).data("modalcallbackgrid");
                    var modalselector = "#" + modalid;
                    var displaysubmit = $(this).data("footersubmitbtn") == true ? true : false;
                    var displayclosebtn = $(this).data("footerclosebtn") == true ? true : false;
                    if ($this.modal != null) {
                        $this.modal.window.modal('hide').promise().done(function () {
                            $('body').remove($this.modal.selector);
                            requestparams['Id'] = data.Id;
                            console.log(data);
                            $.get(contenturl, requestparams, function (content) {
                                var m = new DataTableModal({
                                    id: modalid,
                                    header: title,
                                    footer: true,
                                    callbackgrid: modalcallbackgrid,
                                    displayCloseBtn: displayclosebtn,
                                    displaySubmitBtn: displaysubmit,
                                });
                                m.getBody().html(content).promise().done(function () {
                                    m.initAjaxComponent();
                                    m.window.modal('handleUpdate');
                                    m.show();
                                });
                            });
                        });
                    }
                    else {
                        requestparams['Id'] = data.Id;
                        $.get(contenturl, requestparams, function (content) {
                            var m = new DataTableModal({
                                id: modalid,
                                header: title,
                                footer: true,
                                callbackgrid: modalcallbackgrid,
                                displayCloseBtn: displayclosebtn,
                                displaySubmitBtn: displaysubmit,
                            });
                            m.getBody().html(content).promise().done(function () {
                                m.initAjaxComponent();
                                m.window.modal('handleUpdate');
                                m.show();
                            });
                        });
                    }
                    break;
            }
        });
    }

    deleteDataTableEventHandler() {
        var $this = this;
        $this.grid.on('click', '.datatable-delete-btn', function (e) {
            e.preventDefault();
            var data = $this.grid.DataTable().row($(this).closest('tr')[0]).data();
            var action = $(this).data("url");
            swal({
                title: "Confirmer la suppréssion ?",
                text: "Après suppréssion il ne sera plus possible de restaurer cette information.",
                icon: "warning",
                buttons: {
                    cancel: {
                        text: "Annuler",
                        value: false,
                        visible: true,
                        className: "",
                        closeModal: true,
                    },
                    confirm: {
                        text: "Valider",
                        value: true,
                        visible: true,
                        className: "",
                        closeModal: true
                    }
                },
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.get(action, { Id: data.Id }, function (result) {
                            var handler = new CustomJsonResponseHandler({ customdatatable: $this.Id });
                            handler.parseAjaxResponse(result);
                        });
                    }
                });
        });
    }

    selectDataTableRowData() {
        var $this = this;
        $this.grid.on('click', '.datatable-btn-selectrowdata', function (e) {
            e.preventDefault();
            var data = $this.grid.DataTable().row($(this).closest('tr')[0]).data();
            var displaynames = $this.modal.options.tabledisplayinput.split(",");
            var formDisplayInputNames = $this.modal.options.formdisplayinput.split(",");
            if (formDisplayInputNames.length > 1 && displaynames.length == formDisplayInputNames.length) {
                for (var i = 0; i < formDisplayInputNames.length; i++) {
                    var display_input = $("form[id=" + $this.modal.options.destinationFormId + "] #" + formDisplayInputNames[i]);
                    display_input.val(data[displaynames[i]]);
                    display_input.focusout();
                    if (display_input.hasClass("isckeditor")) {
                        CKEDITOR.instances[formDisplayInputNames[i]].setData(data[displaynames[i]]);

                        var $inputerror = display_input.parents('.form-group:first').find('.is-invalid:first');
                        if ($inputerror.exists()) {
                            $inputerror.removeClass('is-invalid');
                        }
                        var $errorSpan = display_input.parents('.form-group:first').find('.field-validation-error:first');
                        if ($errorSpan.exists()) {
                            $errorSpan.removeClass('field-validation-error');
                            $errorSpan.addClass('field-validation-valid');
                            $errorSpan.html('');
                        }
                    }
                }
            }
            else {
                var displayValue = "";
                for (var i = 0; i < displaynames.length; i++) {
                    displayValue = i != 0 ? displayValue += " " : displayValue;
                    displayValue += data[displaynames[i]];
                }
                var displayInput = $("form[id=" + $this.modal.options.destinationFormId + "] #" + $this.modal.options.formdisplayinput);
                displayInput.focusin();
                displayInput.val(displayValue);
                displayInput.focusout();
                if (displayInput.hasClass("isckeditor")) {
                    CKEDITOR.instances[$this.modal.options.formdisplayinput].setData(displayValue);
                }
                var $inputerror = displayInput.parents('.form-group:first').find('.is-invalid:first');
                if ($inputerror.exists()) {
                    $inputerror.removeClass('is-invalid');
                }
                var $errorSpan = displayInput.parents('.form-group:first').find('.field-validation-error:first');
                if ($errorSpan.exists()) {
                    $errorSpan.removeClass('field-validation-error');
                    $errorSpan.addClass('field-validation-valid');
                    $errorSpan.html('');
                }
            }

            var hiddenValue = data[$this.modal.options.tablehiddeninput];
            var hiddenInput = $("form[id=" + $this.modal.options.destinationFormId + "] #" + $this.modal.options.formhiddeninput);
            var oldHiddenInputValue = hiddenInput.val();
            hiddenInput.val(hiddenValue);
            hiddenInput.trigger('change');

            if ($this.modal.options.hasdependentselector == true) {

                var $search = $("form[id=" + $this.modal.options.destinationFormId + "] " + "#" + $this.modal.options.dependentselectorid + "_search");
                var $add = $("form[id=" + $this.modal.options.destinationFormId + "] " + "#" + $this.modal.options.dependentselectorid + "_add");
                var $reset = $("form[id=" + $this.modal.options.destinationFormId + "] " + "#" + $this.modal.options.dependentselectorid + "_clear");

                //On récupère les input du formulaire géré par le selecteur dépendant du selecteur courant.
               
                if (oldHiddenInputValue != hiddenValue) {

                    var dependantdisplayinputid = $search.data("formdisplayinput");
                    var dependanthiddeninputid = $search.data("formhiddeninput");

                    var $dependanthiddeninput = $("form[id=" + $this.modal.options.destinationFormId + "] " + "#" + dependanthiddeninputid);
                    $dependanthiddeninput.val('');

                    if (dependantdisplayinputid.split(",").length > 1) {
                        var dependantdisplayinputids = dependantdisplayinputid.split(",");
                        for (var i = 0; i < dependantdisplayinputids.length; i++) {
                            var $dependantdisplayinput = $("form[id=" + $this.modal.options.destinationFormId + "] " + "#" + dependantdisplayinputids[i]);
                            $dependantdisplayinput.val("");
                            $dependantdisplayinput.focusout();

                            if ($dependantdisplayinput.hasClass("isckeditor")) {
                                CKEDITOR.instances[dependantdisplayinputids[i]].setData("");

                                var $inputerror = $dependantdisplayinput.parents('.form-group:first').find('.is-invalid:first');
                                if ($inputerror.exists()) {
                                    $inputerror.removeClass('is-invalid');
                                }
                                var $errorSpan = $dependantdisplayinput.parents('.form-group:first').find('.field-validation-error:first');
                                if ($errorSpan.exists()) {
                                    $errorSpan.removeClass('field-validation-error');
                                    $errorSpan.addClass('field-validation-valid');
                                    $errorSpan.html('');
                                }
                            }
                        }
                    }
                    else {

                        var $dependantdisplayinput = $("form[id=" + $this.modal.options.destinationFormId + "] " + "#" + dependantdisplayinputid);
                        $dependantdisplayinput.val('');
                    }
                }

                if ($search) {
                    if ($search[0].getAttribute("disabled")) {
                        $search.removeAttr("disabled");
                        $search.toggleClass("disabled");
                    }
                }

                if ($add.exists()) {
                    if ($add[0].getAttribute("disabled")) {
                        $add.removeAttr("disabled");
                        $add.toggleClass("disabled");
                    }
                }
                if ($reset.exists()) {
                    if ($reset[0].getAttribute("disabled")) {
                        $reset.removeAttr("disabled");
                        $reset.toggleClass("disabled");
                    }
                }
            }
            if ($this.modal != null) {
                $this.modal.hide();
            }
        });
    }

    initModalTooltipHandler() {
        var $this = this;
        $($this.parent).on('click', '.init-tooltip-modal', function (e) {
            e.preventDefault();

            var title = $(this).data("modaltitle");
            var modalsize = $(this).data("modalsize") === undefined || $(this).data("modalsize") == null ? "modal-lg" : $(this).data("modalsize");

            var displaycloseBtn = $(this).data("footerclosebtn");
            var displaySubmitBtn = $(this).data("footersubmitbtn");

            var displayfooter = $(this).data("displaymodalfooter") !== undefined
                && $(this).data("displaymodalfooter") != null ? $(this).data("displaymodalfooter") : undefined;

            var modalid = $(this).data("modalid");
            var modalcallbackgrid = $(this).data("modalcallbackgrid");
            var action = $(this).data("tooltipaction");
            var modalselector = "#" + modalid;

            if (action != undefined && action != null) {
                var m = new DataTableModal({
                    id: modalid,
                    header: title,
                    footer: true,
                    displayCloseBtn: displaycloseBtn,
                    displaySubmitBtn: displaySubmitBtn,
                    callbackgrid: modalcallbackgrid,
                    modalsize: modalsize
                });

                $.get(action, function (content) {
                    m.getBody().html(content).promise().done(function () {
                        m.initAjaxComponent();
                        m.show();
                    });
                });
            }
        });
    }

    initExternalTooltipHandler() {
        var $this = this;
        $($this.parent).on('click', '.init-tooltip-external', function (e) {
            e.preventDefault();
            var pageTitle = $(this).data("pagetitle");
            //var location = splitedHash[2];
            var pageTitleSelector = $(MemoryAppSettings.PageTitleSectionId);
            var breadcrumSelector = $(MemoryAppSettings.BreadCrumbSectionId);
            var action = $(this).data("tooltipaction");
            var selector = MemoryAppSettings.MainDataContainer;
            if (action != undefined && action != null) {


                var $hash = window.location.hash;
                $hash = $hash.replace('#', '');
                window.location.hash = '#New|' + $hash + '|' + action + '|' + pageTitle;

                //$.get(action, function (res) {
                //    $(selector).html(res).promise().done(function () {
                //        breadcrumSelector.append('<li class="breadcrumb-item"><a href="#!">' + pageTitle + '</a></li>');
                //        pageTitleSelector.html('<h4 class="m-b-10">' + pageTitle + '</h4>');
                //        $(selector).trigger(MemoryAppSettings.MainDataConatinerHtmlChangeEvent);
                //    });
                //});
            }
        });
    }

    refreshDataTableHandler() {
        var $this = this;
        $($this.parent).on('click', '.refreshgridbtn', function (e) {
            e.preventDefault();
            $this.grid.DataTable().ajax.reload();
        });
    }

    initToolTipItemBulk() {
        var $this = this;
        $($this.parent).on('click', '.tooltip-item-bulk', function (e) {
            e.preventDefault();
            var selectedrows = [];
            $this.grid.DataTable().rows('.selected').data().each(function (v, i) {
                selectedrows.push(v.Id);
            });
            if (selectedrows.length > 0) {
                var action = $(this).data("action");
                var actionparams = $(this).data("actionparams");
                var confirmationTitle = $(this).attr("title");
                swal({
                    title: "Confirmer",
                    text: confirmationTitle,
                    icon: "info",
                    dangerMode: true,
                    buttons: {
                        cancel: {
                            text: "Annuler",
                            value: false,
                            visible: true,
                            className: "",
                            closeModal: true,
                        },
                        confirm: {
                            text: "Valider",
                            value: true,
                            visible: true,
                            className: "",
                            closeModal: true
                        }
                    }
                }).then((willDelete) => {
                        if (willDelete) {
                            if (action !== undefined && action != null && action !== "") {
                                var requestparams = {};
                                var parsedParams = {};
                                if (actionparams !== undefined && actionparams != null) {
                                    parsedParams = actionparams.split(";");
                                    $.each(parsedParams, function (index, item) {
                                        if (item !== "" && item !== undefined && item != null) {
                                            var param = item.split(":");
                                            requestparams[param[0]] = param[1];
                                        }
                                    });
                                }
                                requestparams['Ids'] = selectedrows;
                                $.post(action, requestparams, function (result) {
                                    var handler = new CustomJsonResponseHandler({ customdatatable: $this.Id, modalwindow: $this.modal });
                                    handler.parseAjaxResponse(result);
                                });
                            }
                        }
                    });
            }
            else {
                swal("Erreur", "Acune donnée sélectionnée", "error");
            }
        });
    }

    inittooltipItemModal() {
        var $this = this;
        $($this.parent).on('click', '.tooltip-item-modal', function (e) {
            e.preventDefault();
            var title = $(this).data("modaltitle");
            var modalsize = $(this).data("modalsize") === undefined || $(this).data("modalsize") == null ? "modal-lg" : $(this).data("modalsize");

            var displaycloseBtn = $(this).data("footerclosebtn");
            var displaySubmitBtn = $(this).data("footersubmitbtn");

            var displayfooter = $(this).data("displaymodalfooter") !== undefined
                && $(this).data("displaymodalfooter") != null ? $(this).data("displaymodalfooter") : undefined;

            var modalid = $(this).data("modalid");
            var modalcallbackgrid = $(this).data("modalcallbackgrid");
            var action = $(this).data("action");
            var modalselector = "#" + modalid;

            if (action != undefined && action != null) {
                var m = new DataTableModal({
                    id: modalid,
                    header: title,
                    footer: true,
                    displayCloseBtn: displaycloseBtn,
                    displaySubmitBtn: displaySubmitBtn,
                    callbackgrid: modalcallbackgrid,
                    modalsize: modalsize
                });

                $.get(action, function (content) {
                    m.getBody().html(content).promise().done(function () {
                        m.initAjaxComponent();
                        m.show();
                    });
                });
            }
        });
    }
    addInlineRow(ref) {
        var $this = ref;
        var rowcolumns = [];
        if ($this.isInLineDataTable) {
            [].slice.call($this.columns).forEach(function (item, ich) {
                var container = $('<div></div>');
                if (item.searchtype == "enumlist") {
                    if (item.isrequiredcol) {
                        var select = $('<select class="col-12 form-control-sm"><option value=""></option></select>');
                        select.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.name);
                        select.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.name);
                        select.attr('data-val', 'true');
                        select.attr('data-val-required', 'Le champ ' + item.name + 'est requis');
                        var options = item.enumvalues.split(";");
                        [].slice.call(options).forEach(function (op, idx) {
                            var option = op.split(",");
                            select.append('<option value="' + option[0] + '">' + option[1] + '</option>');
                        });
                        container.append(select);
                        container.append($('<span data-valmsg-for="' + $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data + '" data-valmsg-replace="true" class="text-danger field-validation-valid"></span>'));
                        rowcolumns.push(container[0].outerHTML);
                    }
                    else {
                        var select = $('<select class="col-12 form-control-sm"><option value=""></option></select>');
                        select.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.name);
                        select.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.name);

                        var options = item.enumvalues.split(";");
                        [].slice.call(options).forEach(function (op, idx) {
                            var option = op.split(",");
                            select.append('<option value="' + option[0] + '">' + option[1] + '</option>');
                        });
                        rowcolumns.push(select[0].outerHTML);
                    }
                }
                else if (item.searchtype == "calendar") {
                    if (item.isrequiredcol) {
                        var inputdate = $('<input class="form-control" type="date"/>');
                        inputdate.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        inputdate.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        inputdate.attr('data-val', 'true');
                        inputdate.attr('data-val-required', 'Le champ ' + item.name + ' est obligatoire');
                        container.append(inputdate);
                        container.append($('<span data-valmsg-for="' + $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data + '" data-valmsg-replace="true" class="text-danger field-validation-valid"></span>'));
                        rowcolumns.push(container[0].outerHTML);
                    }
                    else {
                        var inputdate = $('<input class="form-control" type="date"/>');
                        inputdate.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        inputdate.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        rowcolumns.push(inputdate[0].outerHTML);
                    }
                  
                }
                else if (item.searchtype == "timecalendar") {
                    if (item.isrequiredcol) {
                        var inputdate = $('<input class="form-control" type="time"/>');
                        inputdate.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        inputdate.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);

                        inputdate.attr('data-val', 'true');
                        inputdate.attr('data-val-required', 'Le champ ' + item.name + ' est obligatoire');
                        container.append(inputdate);
                        container.append($('<span data-valmsg-for="' + $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data + '" data-valmsg-replace="true" class="text-danger field-validation-valid"></span>'));
                        rowcolumns.push(container[0].outerHTML);
                    }
                    else {
                        var inputdate = $('<input class="form-control" type="time"/>');
                        inputdate.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        inputdate.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        rowcolumns.push(inputdate[0].outerHTML);
                    }
                   
                }
                else if (item.searchtype == "number") {

                    if (item.isrequiredcol) {

                        var inputtext = $('<input class="form-control" type="number" min="1"/>');
                        inputtext.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        inputtext.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);

                        inputtext.attr('data-val', 'true');
                        inputtext.attr('data-val-required', 'Le champ ' + item.name + ' est obligatoire');

                        container.append(inputtext);
                        container.append($('<span data-valmsg-for="' + $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data + '" data-valmsg-replace="true" class="text-danger field-validation-valid"></span>'));
                        rowcolumns.push(container[0].outerHTML);
                    }
                    else {
                        var inputtext = $('<input class="form-control" type="number" min="1"/>');
                        inputtext.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        inputtext.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        rowcolumns.push(inputtext[0].outerHTML);
                    }
                  
                }
                else if (item.searchtype == "inputfileupload") {
                    if (item.isrequiredcol) {
                        var inputfile = $('<input class="form-control" type="file"/>');
                        inputfile.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data + ".File");
                        inputfile.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data + ".File");
                        inputfile.attr('data-val', 'true');
                        inputfile.attr('data-val-required', 'Le champ ' + item.name + 'est requis');
                        container.append(inputfile);
                        container.append($('<span data-valmsg-for="' + $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data +'.File' + '" data-valmsg-replace="true" class="text-danger field-validation-valid"></span>'));
                        rowcolumns.push(container[0].outerHTML);
                    }
                    else {
                        var inputfile = $('<input class="form-control" type="file"/>');
                        inputfile.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data + ".File");
                        inputfile.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data + ".File");
                        rowcolumns.push(inputfile[0].outerHTML);
                    }
                }
                else {
                    if (item.isrequiredcol) {
                        var inputtext = $('<input class="form-control" type="text"/>');
                        inputtext.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        inputtext.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        inputtext.attr('data-val', 'true');
                        inputtext.attr('data-val-required', 'Le champ ' + item.name + ' est obligatoire');
                        container.append(inputtext);
                        container.append($('<span data-valmsg-for="' + $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data + '" data-valmsg-replace="true" class="text-danger field-validation-valid"></span>'));
                        rowcolumns.push(container[0].outerHTML);
                    }
                    else {
                        var inputtext = $('<input class="form-control" type="text"/>');
                        inputtext.attr("id", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        inputtext.attr("name", $this.rowCollectionName + "[" + $this.inlineRowCount + "]." + item.data);
                        rowcolumns.push(inputtext[0].outerHTML);
                    }
                }
            });
            $this.grid.dataTable().fnAddData(rowcolumns);
            $this.inlineRowCount++;
        }
    }
    inlineToolTipAddRow() {
        var $this = this;
        $($this.parent).on('click', '.inlineaddrowbtn', function (e) {
            e.preventDefault();
            $this.addInlineRow($this);
        });
    }

    inlineToolTipDeleteRow() {
        var $this = this;
        $($this.parent).on('click', '.inlinedeleterowbtn', function (e) {
            e.preventDefault();
            if ($this.inlineRowCount > $this.InLineTableMinRow) {
                $this.grid.dataTable().fnDeleteRow($this.inlineRowCount - 1);
                $this.inlineRowCount--;
            }
        });
    }

    refreshDataTableOnPeriodeDateChange() {
        var $this = this;
        var container = $this.grid.closest('.datatablerootcontainer');
        $($this.parent).on('change', '#dt_date_filter_start_' + container.attr("id"), function (e) {
            e.preventDefault();
            if (($(this).val() !== undefined && $(this).val() != null && $(this).val() !== "")
                && ($this.dateRangeFilterEnd !== undefined && $this.dateRangeFilterEnd != null && $this.dateRangeFilterEnd !== ""))
            {
                $this.dateRangeFilterStart = $(this).val();
                $this.grid.DataTable().settings()[0].ajax.data.dtperiodestart = $this.dateRangeFilterStart;
                $this.grid.DataTable().ajax.reload();
            }
           
        });
        $($this.parent).on('change', '#dt_date_filter_end_' + container.attr("id"), function (e) {
            e.preventDefault();
            if (($(this).val() !== undefined && $(this).val() != null && $(this).val() !== "")
                && ($this.dateRangeFilterStart !== undefined && $this.dateRangeFilterStart != null && $this.dateRangeFilterStart !== "")) {
                $this.dateRangeFilterEnd = $(this).val();
                $this.grid.DataTable().settings()[0].ajax.data.dtperiodeend = $this.dateRangeFilterEnd;
                $this.grid.DataTable().ajax.reload();
            }
        });
    }

    displayOrganisationFilterModal() {
        var $this = this;
        $($this.parent).on('click', '.displayOrganisationUnitFilterModal', function (e) {
            var container = $this.grid.closest('.datatablerootcontainer');
            var action = $(this).data("uofilteraction");
            var modalTitle = $(this).data("modaltitle");
            e.preventDefault();
            if (action != undefined && action != null) {
                var m = new TreeViewModal({
                    id: "displayOrganisationUnitFilter_" + container.attr("id"),
                    header: modalTitle,
                    footer: true,
                    displayCloseBtn: true,
                    displaySubmitBtn: true,
                    callbackgrid: $this.Id,
                    modalsize: "modal-lg",
                    datagrid: $this
                });
                $.get(action, function (content) {
                    m.getBody().html(content).promise().done(function () {
                        m.initAjaxComponent();
                        m.show();
                    });
                });
            }
        });
    }
}

