/**
 * 
 * @param { Object } [options] Options list.
 * @param { String | Boolean } [options.header = false] Header text.Default false - do not show header block.
 * @param { Boolean } [options.closeButton = true] Show or not header close button.It makes sense only if the options.header is logical true.
 * @param { Boolean } [options.footer = false] Show or not footer block.Default false;
 * @param { String | Boolean } [options.footerCloseButton = false] Footer close button text.Default false - do not show footer close button.
 * @param { String } [options.id = "myModal"] Modal container id attribute;

 */


class BootStrapModal {
    constructor(options) {
        this.selector;
        this.window;
        this.options = options ? options : {}
        this.options.modalsize = (options.modalsize !== undefined && options.modalsize != null && options.modalsize != "") ? options.modalsize : "modal-lg";
        this.options.header = options.header !== undefined ? options.header : false;
        this.options.footer = options.footer !== undefined ? options.footer : true;
        this.options.closeButton = options.closeButton !== undefined ? options.closeButton : true;
        this.options.id = options.id !== undefined ? options.id : "myModal";
        //this.options.id = tmpId+'_'+this.generateModalId();
        this.selector = "#" + this.options.id;
        this.options.footerCloseButton = options.footerCloseButton !== undefined ? options.footerCloseButton : 'Fermer';
        this.options.footerSubmitFormButton = options.footerSubmitFormButton !== undefined ? options.footerSubmitFormButton :'Valider';
        this.options.parentModalId = options.parentModalId !== undefined ? options.parentModalId : null;
        this.options.displayCloseBtn = options.displayCloseBtn !== undefined ? options.displayCloseBtn : true;
        this.options.displaySubmitBtn = options.displaySubmitBtn !== undefined ? options.displaySubmitBtn : true;
        this.options.isPaused = false;
        if (this.options.footerSubmitFormButton) {
            this.postformBtn = this.initModalPostFormBtn();
            this.postModalForm();
        }
        if (!$(this.selector).length) {
            this.createModal();
        }
        this.window = $(this.selector);
        this.setHeader(this.options.header);
        this.window.modal({
            backdrop: 'static',
            keyboard: false
        });
        var $w = this;
        $w.window.on('hide.bs.modal', function (event) {

        });
        $w.window.on('hidden.bs.modal', function (e) {
            window.MemoryCurrentModal = null;
            if (!$w.options.isPaused)
            {
                e.target.remove();
            }
            if ($w.options.parentModalId != null) {
                $("#" + $w.options.parentModalId).modal('show');
            }
        });
        $w.window.on("shown.bs.modal", function () {
            $w.options.isPaused = false;
            window.MemoryCurrentModal = $w;
            $($.fn.dataTable.tables(false)).DataTable().columns.adjust();
        });
    }

    /**
 * Set header text. It makes sense only if the options.header is logical true.
 * @param {String} html New header text.
 */
    setHeader(html) {
        this.window.find('.modal-title').html(html);
    }

    /**
     * Set body HTML.
     * @param {String} html New body HTML
     */
    setBody(html) {
        this.window.find('.modal-body').html(html);
    }

    /**
     * Set footer HTML.
     * @param {String} html New footer HTML
     */
    setFooter(html) {
        this.window.find('.modal-footer').html(html);
    }

    /**
     * Return window body element.
     * @returns {jQuery} The body element
     */
    getBody() {
        return this.window.find('.modal-body');
    }
    /**
     * Show modal window
    */
    show() {
        this.window.modal('show');
    }

    /**
     * hide modal window
    */
    hide() {
        this.window.modal('hide');
    }

    /**
 * hide modal window
*/
    closeModal() {
        this.window.modal('hide');
    }

    /**
     * Toggle modal window
     */
    toggle() {
        this.window.modal('toggle');
    }
    initModalPostFormBtn() {
        return $('<button type="button" class="btn btn-primary" data-modalid="' + this.options.id + '">' + this.options.footerSubmitFormButton + '</button>');
    }
    postModalForm() {
        var $this = this;
        this.postformBtn.click(function (e) {
            e.preventDefault();
            try {
                var form = $this.window.find("form");
                var action = form.attr("action");
                if (!form.valid())
                    return false;

                for (var instance in CKEDITOR.instances) {
                    CKEDITOR.instances[instance].updateElement();
                }

                $.ajax({
                    url: action,
                    type: 'POST',
                    data: new FormData(form[0]),
                    async: true,
                    success: function (result) {

                        try {
                            var handler = new CustomJsonResponseHandler({ modalwindow: $this });
                            handler.parseAjaxResponse(result);
                        }
                        catch (j) {
                            console.log(j);
                        }
                    },
                    cache: false,
                    contentType: false,
                    processData: false,
                });
            }
            catch (x) {
                console.log(x);
            }
        });
    }

    createModal() {
        $('body').append('<div id="' + this.options.id + '" class="modal fade in modalwindow modalwindowcontainer" role="dialog"></div>');
        $(this.selector).append('<div class="' + this.options.modalsize+' modal-dialog" role="document"><div class="modal-content"></div></div>');
        var win = $('.modal-content', this.selector);
        if (this.options.header) {
            win.append('<div class="modal-header border-bottom memory-modal-header"><h5 class="modal-title"></h5></div>');

            if (this.options.closeButton) {
                win.find('.modal-header').append('<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>');
            }
        }
        win.append('<div class="modal-body memory-modal-scrollable pb-5"></div>');
        if (this.options.footer) {
            win.append('<div class="modal-footer border-top"></div>');
            if (this.options.displaySubmitBtn) {
                win.find('.modal-footer')
                    .append(this.postformBtn);
            }
            if (this.options.displayCloseBtn) {
                win.find('.modal-footer')
                    .append('<button type="button" class="btn btn-secondary" data-dismiss="modal">' + this.options.footerCloseButton + '</button>');
            }
        }
    }

    initAjaxComponent() {
        var $this = this;
        AjaxFormHelperMethods.initJqueryFormValidation(this.selector);
        DataTableBuilder.initModalDataTable(this.selector, this);
        AjaxDropDownListFor.initAjaxDropDownListFor(this.selector);
        AjaxButtonFactory.InitAjaxButtonHandler(this.selector);
        $(this.selector).find('input[maxlength]').each(function (i, e) {
            $(e).maxlength({ alwaysShow: true, appendToParent: true,});
        });
        $(this.selector).find('textarea[maxlength]').each(function (i, e) {
            $(e).maxlength({ alwaysShow: true, appendToParent: true, });
        });
        $(this.selector).find('.isckeditor').each(function (i, e) {
            CKEDITOR.replace($(this).attr('id'), {
                removePlugins: 'wsc,scayt,about,sourcearea,save,image,link,maximize,showblocks,newpage',
                extraPlugins: 'font,colorbutton,justify',
                font_names: "Arial/Arial, Helvetica, sans-serif;Bookman Old Style,Bookman Old Style Bold,Bookman Old Style Bold Italic,Bookman Old Style Italic;Comic Sans MS/Comic Sans MS, cursive;Courier New/Courier New, Courier, monospace;Georgia/Georgia, serif;Lucida Sans Unicode/Lucida Sans Unicode, Lucida Grande, sans-serif;Tahoma/Tahoma, Geneva, sans-serif;Times New Roman/Times New Roman, Times, serif;Trebuchet MS/Trebuchet MS, Helvetica, sans-serif;Verdana/Verdana, Geneva, sans-serif"
            });
        });
        $(this.selector).find('.isckeditor-readonly').each(function (i, e) {
            CKEDITOR.replace($(this).attr('id'), {
                readOnly: true,
                removePlugins: 'about,sourcearea, elementspath,save,image,flash,iframe,link,smiley,tabletools,find,pagebreak,templates,maximize,showblocks,newpage,language'
            });
        });
        $(this.selector).find('.is_select2').each(function (i, e) {
            var ajaxdatasource = $(e).data("ajaxdatasource");
            if (ajaxdatasource !== undefined && ajaxdatasource != null) {
                $(e).select2({
                    dropdownParent: $this.window,
                    minimumInputLength: 2,
                    tags: [],
                    ajax: {
                        url: ajaxdatasource,
                        dataType: 'json',
                        type: "GET",
                        quietMillis: 50,
                        data: function (params) {
                            return {
                                searchTerm: params.term
                            };
                        },
                        processResults: function (response) {
                            return {
                                results: response
                            };
                        },
                        cache: true
                    }
                });

                $(e).trigger('change');
            }
            else {
                $(e).select2({
                    dropdownParent: $this.window,
                    allowClear: true,
                    minimumResultsForSearch: -1,
                });
            }
        });
        $(this.selector).find('.datetimepicker').each(function (i, e) {
            $(e).bootstrapMaterialDatePicker({
                format: 'DD/MM/YYYY HH:mm',
                lang: 'fr',
                weekStart: 1,
                cancelText: 'ANNULER',
                switchOnClick: true
            });
        });
        $(this.selector).on('shown.bs.tab', 'a[data-toggle="pill"]', function (e) {
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        });
        $(this.selector).find('.selectloadpartialview').each(function (i, e) {
            var area = $(this).data("replacearea");
            $.get($(this).val(), function (datas) {
                $("#" + area).html(datas);
                DataTableBuilder.initModalDataTable("#" + area, $this);
            });
        });
        $(this.selector).on('change', '.selectloadpartialview', function (e) {
            var area = $(this).data("replacearea");
            $.get($(this).val(), function (datas) {
                $("#" + area).html(datas);
                DataTableBuilder.initModalDataTable("#" + area, $this);
            });
        });

        TreeViewInitializer.initModalTreeView($this.options.id, this);
    }
}

class DataTableModal extends BootStrapModal {
    constructor(options) {

        super(options);
        this.options.callbackgrid = options.callbackgrid !== undefined ? options.callbackgrid : null;
        this.options.displaymessagearea = options.displaymessagearea !== undefined ? options.displaymessagearea : null;
    }

    initModalPostFormBtn() {
        return $('<button type="button" class="btn btn-primary" data-modalid="' + this.options.id + '" data-modalcallbackgrid="' + this.options.callbackgrid + '"> ' + this.options.footerSubmitFormButton + '</button>');
    }
}

class CalendarModal extends BootStrapModal {
    constructor(options) {
        super(options);
        this.options.calendar = options.calendar;
        this.jsonresponseHandler = new CustomJsonResponseHandler({ modalwindow: this})
        var $this = this;
        this.window.on('click', '.calendardeleteevent', function (e) {
            e.preventDefault();
            var $handler = $(this);
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
                        var action = $handler.data("action");
                        if (action !== undefined && action != null && action != "") {
                            $.get(action, function (response) {
                                try {
                                    $this.jsonresponseHandler.parseAjaxResponse(response);
                                }
                                catch (j) {
                                    console.log(j);
                                }
                            });
                        }
                    }
                });
        });

        this.window.on('click', '.calendareditevent', function (e) {
            var $handler = $(this);
            e.preventDefault();
            $this.jsonresponseHandler.CLOSE_CURENT_MODAL();
            var m = new CalendarModal({
                id: 'EditEventCalendar_' + $handler.data("eventid"),
                header: "Editer un évènement",
                footer: true,
                displayCloseBtn: true,
                displaySubmitBtn: true,
                calendar: $this.options.calendar,
                modalsize: 'modal-lg'
            });
            var action = $handler.data("action");
            if (action !== undefined && action != null && action != "") {
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
class FormBootStrapModal extends BootStrapModal 
{
    constructor(options) {
        super(options);
        this.options.refreshaction = options.refreshaction !== undefined ? options.refreshaction : null;
        this.options.refreshareaid = options.refreshareaid !== undefined ? options.refreshareaid : null;
        this.options.refechimgaction = options.refechimgaction !== undefined ? options.refechimgaction : null;
        this.options.fetchingimgid = options.fetchingimgid !== undefined ? options.fetchingimgid : null;
    }

    static openFormModal(selector) {
        $(selector).on('click', '.openformModal', function (e) {
            e.preventDefault();
            var title = $(this).data("modaltitle");
            var modalId = $(this).data("modalid");
            var action = $(this).data("tooltipaction");
            var refreshaction = $(this).data("refreshaction");
            var refresharea = $(this).data("refresharea");
            var refechimgaction = $(this).data("refetchimageaction");
            var fetchingimgid = $(this).data("fetchimageid");
            var modalselector = "#" + modalId;
            var displaySubmit = $(this).data("displaysubmit") != null && $(this).data("displaysubmit") != undefined ? $(this).data("displaysubmit") == "true" ? true : false : true;
            if (action != undefined && action != null) {
                var m = new FormBootStrapModal({
                    id: modalId,
                    header: title,
                    footer: true,
                    refreshaction: refreshaction,
                    refreshareaid: "#" + refresharea,
                    refechimgaction: refechimgaction,
                    fetchingimgid: fetchingimgid,
                    displayCloseBtn: true,
                    displaySubmitBtn: displaySubmit
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

    static openSimpleFormModal(selector) {
        $(selector).on('click', '.opensimpleformModal', function (e) {
            e.preventDefault();
            var title = $(this).data("modaltitle");
            var modalId = $(this).data("modalid");
            var action = $(this).data("tooltipaction");
            var refreshaction = $(this).data("refreshaction");
            var refresharea = $(this).data("refresharea");
            var refechimgaction = $(this).data("refetchimageaction");
            var fetchingimgid = $(this).data("fetchimageid");
            var modalselector = "#" + modalId;

           

            var displaySubmit = $(this).data("displaysubmit") != null && $(this).data("displaysubmit") != undefined ? $(this).data("displaysubmit") == "true" ? true : false : true;
            if (action != undefined && action != null) {
                //var m = new FormBootStrapModal({
                //    id: modalId,
                //    header: title,
                //    footer: true,
                //    refreshaction: refreshaction,
                //    refreshareaid: "#" + refresharea,
                //    refechimgaction: refechimgaction,
                //    fetchingimgid: fetchingimgid,
                //    displayCloseBtn: true,
                //    displaySubmitBtn: displaySubmit
                //});
                $.get(action, function (content) {
                    //m.getBody().html(content).promise().done(function () {
                    //    m.show();
                    //});
                    Metro.dialog.create({
                        title: title,
                        content: content,
                        closeButton: true,
                        width:680
                    });
                });
            }
        });
    }
}

class DataTableFormInputSelectorModal extends BootStrapModal{

    constructor(options) {
        super(options);
        this.options.formdisplayinput = options.formdisplayinput !== undefined ? options.formdisplayinput : null;
        this.options.formhiddeninput = options.formhiddeninput !== undefined ? options.formhiddeninput : null;
        this.options.tablehiddeninput = options.tablehiddeninput !== undefined ? options.tablehiddeninput : null;
        this.options.tabledisplayinput = options.tabledisplayinput !== undefined ? options.tabledisplayinput : null;
        this.options.destinationFormId = options.destinationFormId !== undefined ? options.destinationFormId : null;

        this.options.hasdependentselector = options.hasdependentselector !== undefined && options.hasdependentselector != null ? true : false;
        this.options.dependentselectorid = options.dependentselectorid !== undefined && options.dependentselectorid != null ? options.dependentselectorid : null;

        this.options.hassourceselector = options.hassourceselector !== undefined && options.hassourceselector != null ? true : false;
        this.options.sourceselectorid = options.sourceselectorid !== undefined && options.sourceselectorid != null ? options.sourceselectorid : null;
    }

    static initDataTableFormInputSelectorHandler(selector) {

        $(selector).on('click', '.inputselector-search', function (e) {
            e.preventDefault();
            e.stopPropagation();

            //check if selector button is in modal.
            var modalroot = $(this).parents('.modalwindowcontainer')[0];
            var formId = $(this).closest("form").attr("id");

            var ismodalInput = modalroot !== undefined && modalroot !== null;
            var parentModalid = ismodalInput == true ? $(modalroot).attr("id") : undefined;

            var title = $(this).data("modaltitle");
            var modalId = $(this).data("modalid");
            var action = $(this).data("tooltipaction");

            var formdisplayinput = $(this).data("formdisplayinput");
            var formhiddeninput = $(this).data("formhiddeninput");
            var tablehiddeninput = $(this).data("tablehiddeninput");
            var tabledisplayinput = $(this).data("tabledisplayinput");

            var hasourceselector = $(this).data("hassourceselector");
            var sourceselectorid = $(this).data("sourceselectorid");
            var hasdependentselector = $(this).data("hasdependentselector");
            var dependentselectorid = $(this).data("dependentselectorid");
            var dependentactionparamname = $(this).data("dependentactionparam");

            if (action != undefined && action != null && action!="") {
                var m = new DataTableFormInputSelectorModal({
                    id: modalId,
                    header: title,
                    footer: true,
                    formdisplayinput: formdisplayinput,
                    formhiddeninput: formhiddeninput,
                    tablehiddeninput: tablehiddeninput,
                    tabledisplayinput: tabledisplayinput,
                    parentModalId: parentModalid,
                    destinationFormId: formId,
                    displaySubmitBtn: false,
                    displayCloseBtn: false,
                    hasdependentselector: hasdependentselector,
                    dependentselectorid: dependentselectorid,
                    hassourceselector: hasourceselector,
                    sourceselectorid: sourceselectorid
                });
                var requestparams = {};
                if (m.options.hassourceselector) {

                    var sourceselector_search = $("form[id=" + m.options.destinationFormId + "] " + "#" + m.options.sourceselectorid + "_search");
                    var sourcehideninputid = sourceselector_search.data("formhiddeninput");
                    var $sourcethiddeninput = $("form[id=" + m.options.destinationFormId + "] " + "#" + sourcehideninputid);
                    if ($sourcethiddeninput.exists()) {

                        if (dependentactionparamname !== undefined && dependentactionparamname != null)
                        {
                            requestparams[dependentactionparamname] = $sourcethiddeninput.val();
                        }
                    }
                }
                if (ismodalInput) {

                    window.MemoryCurrentModal.options.isPaused = true;
                    window.MemoryCurrentModal.window.modal('hide').promise().done(function () {
                        $.get(action, requestparams, function (content) {
                            m.getBody().html(content).promise().done(function () {
                                m.initAjaxComponent();
                                m.show();
                            });
                        });

                    })
                    //$("#" + parentModalid).modal('hide').promise().done(function () {
                       
                    //});
                }
                else {

                    $.get(action, requestparams, function (content) {
                        m.getBody().html(content).promise().done(function () {
                            m.initAjaxComponent();
                            m.show();
                        });
                    });
                }
            }
        });

        $(selector).on('click', '.inputselector-add', function (e) {

            e.preventDefault();
            e.stopPropagation();

            var modalroot = $(this).parents('.modalwindowcontainer')[0];
            var formId = $(this).closest("form").attr("id");

            var ismodalInput = modalroot !== undefined && modalroot !== null;
            var currentModal = ismodalInput == true ? $(modalroot).attr("id") : undefined;

            var formId = $(this).closest("form").attr("id");
            var title = $(this).data("modaltitle");
            var modalId = $(this).data("modalid");
            var action = $(this).data("tooltipaction");

            var formdisplayinput = $(this).data("formdisplayinput").toString().split(",")[0];
            var formhiddeninput = $(this).data("formhiddeninput").toString().split(",")[0];
            var objecthiddeninput = $(this).data("objecthiddeninput").toString().split(",")[0];
            var objectdisplayinput = $(this).data("objectdisplayinput").toString().split(",")[0];

            var hasourceselector = $(this).data("hassourceselector");
            var sourceselectorid = $(this).data("sourceselectorid");
            var hasdependentselector = $(this).data("hasdependentselector");
            var dependentselectorid = $(this).data("dependentselectorid");
            var dependentactionparamname = $(this).data("dependentactionparam");

            if (action != undefined && action != null && action != "") {
                var m = new DataTableFormInputSelectorModal({
                    id: modalId,
                    header: title,
                    footer: true,
                    formdisplayinput: formdisplayinput,
                    formhiddeninput: formhiddeninput,
                    tablehiddeninput: objecthiddeninput,
                    tabledisplayinput: objectdisplayinput,
                    parentModalId: currentModal,
                    destinationFormId: formId,
                    displaySubmitBtn: true,
                    displayCloseBtn: true,
                    hasdependentselector: hasdependentselector,
                    dependentselectorid: dependentselectorid,
                    hassourceselector: hasourceselector,
                    sourceselectorid: sourceselectorid
                });
                var requestparams = {};
                if (m.options.hassourceselector) {

                    var sourceselector_search = $("form[id=" + m.options.destinationFormId + "] " + "#" + m.options.sourceselectorid + "_search");
                    var sourcehideninputid = sourceselector_search.data("formhiddeninput");
                    var $sourcethiddeninput = $("form[id=" + m.options.destinationFormId + "] " + "#" + sourcehideninputid);
                    if ($sourcethiddeninput.exists()) {
                        if (dependentactionparamname !== undefined && dependentactionparamname != null) {
                            requestparams[dependentactionparamname] = $sourcethiddeninput.val();
                        }
                    }
                }
                if (ismodalInput) {
                    window.MemoryCurrentModal.options.isPaused = true;
                    window.MemoryCurrentModal.window.modal('hide').promise().done(function () {
                        $.get(action, requestparams, function (d) {
                            m.getBody().html(d).promise().done(function () {
                                m.initAjaxComponent();
                                m.show();
                            });
                        });
                    });

                    //$("#" + currentModal).modal('hide').promise().done(function () {
                       
                    //});
                }
                else {
                    $.get(action, requestparams,function (d) {
                        m.getBody().html(d).promise().done(function () {
                            m.initAjaxComponent();
                            m.show();
                        });
                    });
                }
            }
        });
        $(selector).on('click', '.inputselector-clear', function (e) {
            e.preventDefault();
            e.stopPropagation();

            try {
                var formId = $(this).closest("form").attr("id");
                var formdisplayinput = $(this).data("formdisplayinput");
                var formhiddeninput = $(this).data("formhiddeninput");
                $("form[id=" + formId + "] #" + formdisplayinput).val('');
                $("form[id=" + formId + "] #" + formhiddeninput).val('');

                var hasdependentselector = $(this).data("hasdependentselector") !== undefined && $(this).data("hasdependentselector") != null ? true : false;
                var dependentselectorid = $(this).data("dependentselectorid");

                if (hasdependentselector) {

                    var $search = $("form[id=" + formId + "] " + "#" + dependentselectorid + "_search");
                    var $add = $("form[id=" + formId + "] " + "#" + dependentselectorid + "_add");
                    var $reset = $("form[id=" + formId + "] " + "#" + dependentselectorid + "_clear");

                    //On récupère les input du formulaire géré par le selecteur dépendant du selecteur courant.
                    var dependantdisplayinputid = $search.data("formdisplayinput");
                    var dependanthiddeninputid = $search.data("formhiddeninput");

                    var $dependantdisplayinput = $("form[id=" + formId + "] " + "#" + dependantdisplayinputid);
                    var $dependanthiddeninput = $("form[id=" + formId + "] " + "#" + dependanthiddeninputid);

                    $dependantdisplayinput.val('');
                    $dependanthiddeninput.val('');

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
            }
            catch (xy) {
                console.log(xy);
            }
        });
    }
}

class TreeViewModal extends BootStrapModal {
    constructor(options) {
        super(options);
        this.options.callbackgrid = options.callbackgrid !== undefined ? options.callbackgrid : null;
        this.datagrid = options.datagrid !== undefined ? options.datagrid : null;
        this.treeview;
    }

    initModalPostFormBtn() {
        return $('<button type="button" class="btn btn-primary" data-modalid="' + this.options.id + '">' + this.options.footerSubmitFormButton + '</button>');
    }
    postModalForm() {
        var $this = this;
        this.postformBtn.click(function (e) {
            e.preventDefault();
            var selectedNodes = $($this.treeview.fancyTreeView).fancytree('getTree').getSelectedNodes();
            if (!jQuery.isEmptyObject(selectedNodes)) {
                $this.datagrid.organisationUnitFilterValues = [];
                [].slice.call(selectedNodes).forEach(function (el, i) {
                    $this.datagrid.organisationUnitFilterValues.push({ "key": el.data.id });
                    $this.datagrid.grid.DataTable().settings()[0].ajax.data.organisationunitfilters = $this.datagrid.organisationUnitFilterValues;
                   
                });
                $this.datagrid.grid.DataTable().ajax.reload();
                $this.closeModal();
            }
            else {
                if ($this.datagrid.organisationUnitFilterValues.length > 0) {
                    $this.datagrid.organisationUnitFilterValues = [];
                    $this.datagrid.grid.DataTable().settings()[0].ajax.data.organisationunitfilters = $this.datagrid.organisationUnitFilterValues;
                    $this.datagrid.grid.DataTable().ajax.reload();
                    $this.closeModal();
                }
                else {
                    $this.closeModal();
                }
            }
        });
    }
}