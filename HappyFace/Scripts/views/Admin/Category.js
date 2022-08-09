var datatable, categorytree, id, type;
(function (w) {
    var categories = function () { };
    categories.constructor = categories;
    categories.prototype = {
        Init: function () {
            $categories.Fill();
            $admin.Summernote('[data-js=Content]');
            $('[data-js=menu-item-category]').addClass('kt-menu__item--active kt-menu__item--open');
        },
        // Sub Table
        GetSub: function (main) {
            var sub = $('<div id="child_data_local_' + main.data.ID + '"></div>');
            if (main.data.Parent) {
                sub.appendTo(main.detailCell);
                sub = $('#child_data_local_' + main.data.ID).KTDatatable({
                    data: {
                        type: 'remote',
                        source: {
                            read: {
                                url: $pointer.GetPointer("JsonCategory") + '?parent=' + main.data.ID
                            }
                        },
                        serverPaging: true,
                        serverFiltering: true
                    },
                    layout: {
                        scroll: true,
                        height: 'auto',
                        footer: false
                    },
                    sortable: true,
                    detail: {
                        title: $language.get('showsubcategory'),
                        content: $categories.GetSub,
                    },
                    toolbar: {
                        placement: ['bottom'],
                        items: {
                            pagination: {
                                pageSizeSelect: [5, 10, 20, 30, 50],
                            },
                            info: false
                        }
                    },
                    translate: {
                        toolbar: {
                            pagination: {
                                items: {
                                    info: $language.get('toolbarInfo'),
                                    default: {
                                        first: $language.get('first'),
                                        prev: $language.get('prev'),
                                        next: $language.get('next'),
                                        last: $language.get('last'),
                                        more: $language.get('morePages'),
                                        input: $language.get('inputNumber'),
                                        select: $language.get('selectPageSize')
                                    }
                                }
                            }
                        },
                        records: {
                            noRecords: $language.get('noRecords'),
                            processing: $language.get('processing')
                        }
                    },
                    columns: [
                        {
                            field: 'ID',
                            title: '',
                            sortable: false,
                            width: 30,
                            textAlign: 'center'
                        }, {
                            field: 'Title',
                            title: $language.get('title')
                        }, {
                            field: 'Description',
                            title: $language.get('description')
                        }, {
                            field: 'Type',
                            title: $language.get('type'),
                            template: function (row) {
                                return $language.get(row.Type);
                            }
                        }, {
                            field: 'Banner',
                            title: 'Banner',
                            template: function (row) {
                                return row.Banner != null ? '<i class="fa fa-lg fa-image text-success"></i>' : '<i class="fa fa-lg fa-image"></i>';
                            }
                        }, {
                            field: 'ShowMenu',
                            title: $language.get('showmenu'),
                            template: function (row) {
                                return row.ShowMenu ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="ShowMenu" data-value="true" data-id="' + row.ID + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="ShowMenu" data-value="false" data-id="' + row.ID + '"></button>';
                            }
                        }, {
                            field: 'Status',
                            title: $language.get('status'),
                            template: function (row) {
                                return row.Status ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="Status" data-value="true" data-id="' + row.ID + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="Status" data-value="false" data-id="' + row.ID + '"></button>';
                            }
                        }, {
                            field: 'Actions',
                            title: $language.get('actions'),
                            sortable: false,
                            width: 110,
                            overflow: 'visible',
                            autoHide: false,
                            template: function (row) {
                                return `<div class="dropdown">
	                                        <a data-toggle="dropdown" class="btn btn-sm btn-clean btn-icon btn-icon-md" aria-expanded="false">
		                                        <i class="la la-ellipsis-h"></i>
	                                        </a>
	                                        <div class="dropdown-menu dropdown-menu-right" x-placement="bottom-end">
		                                        <button class="dropdown-item" data-js="delete-sub-category" data-id="${row.SharedID}">${$language.get('deletesubcategory')}</button>
		                                        <a href="/new-category?id=${row.SharedID}&type=${row.Type}" class="dropdown-item">${$language.get('addsubcategory')}</a>
		                                        <button class="dropdown-item" data-js="move-category" data-id="${row.SharedID}" data-parentid="${row.ParentID}">${$language.get('move')}</button>
		                                        <button class="dropdown-item" data-js="copy-category" data-id="${row.SharedID}">${$language.get('copy')}</button>
	                                        </div>
                                        </div>
                                        <a href="/edit-category?id=${row.ID}" class="btn btn-sm btn-clean btn-icon btn-icon-md" title="${$language.get('edit')}">
	                                        <i class="la la-edit"></i>
                                        </a>
                                        <button type="button" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-js="delete-record" data-id="${row.ID}" data-isparent="${row.Parent}" title="${$language.get('delete')}">
	                                        <i class="la la-trash"></i>
                                        </button>`;
                            }
                        }
                    ]
                });
            }
            else {
                main.detailCell.empty();
                sub.html(`<small class="d-flex justify-content-center">${$language.get('nosubcategory')}</small>`).appendTo(main.detailCell);
            }
        },
        // Main Table
        Fill: function () {
            datatable = $('#categories').KTDatatable({
                data: {
                    type: 'remote',
                    source: {
                        read: {
                            url: $pointer.GetPointer("JsonCategory")
                        }
                    },
                    serverPaging: true
                },
                layout: {
                    scroll: true,
                    height: 'auto',
                    footer: false
                },
                sortable: true,
                detail: {
                    title: $language.get('showsubcategory'),
                    content: $categories.GetSub,
                },
                search: {
                    input: $('#search'),
                },
                toolbar: {
                    placement: ['bottom'],
                    items: {
                        pagination: {
                            pageSizeSelect: [5, 10, 20, 30, 50],
                        }
                    }
                },
                translate: {
                    toolbar: {
                        pagination: {
                            items: {
                                info: $language.get('toolbarInfo'),
                                default: {
                                    first: $language.get('first'),
                                    prev: $language.get('prev'),
                                    next: $language.get('next'),
                                    last: $language.get('last'),
                                    more: $language.get('morePages'),
                                    input: $language.get('inputNumber'),
                                    select: $language.get('selectPageSize')
                                }
                            }
                        }
                    },
                    records: {
                        noRecords: $language.get('noRecords'),
                        processing: $language.get('processing')
                    }
                },
                columns: [
                    {
                        field: 'ID',
                        title: '',
                        sortable: false,
                        width: 30,
                        textAlign: 'center'
                    },
                    {
                        field: 'Title',
                        title: $language.get('title')
                    }, {
                        field: 'Description',
                        title: $language.get('description')
                    }, {
                        field: 'Type',
                        title: $language.get('type'),
                        template: function (row) {
                            return $language.get(row.Type);
                        }
                    }, {
                        field: 'Banner',
                        title: 'Banner',
                        template: function (row) {
                            return row.Banner != null ? '<i class="fa fa-lg fa-image text-success"></i>' : '<i class="fa fa-lg fa-image"></i>';
                        }
                    }, {
                        field: 'ShowMenu',
                        title: $language.get('showmenu'),
                        template: function (row) {
                            return row.ShowMenu ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="ShowMenu" data-value="true" data-id="' + row.ID + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="ShowMenu" data-value="false" data-id="' + row.ID + '"></button>';
                        }
                    }, {
                        field: 'Status',
                        title: $language.get('status'),
                        template: function (row) {
                            return row.Status ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="Status" data-value="true" data-id="' + row.ID + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="Status" data-value="false" data-id="' + row.ID + '"></button>';
                        }
                    }, {
                        field: 'Actions',
                        title: $language.get('actions'),
                        sortable: false,
                        width: 110,
                        overflow: 'visible',
                        autoHide: false,
                        template: function (row) {
                            return `<div class="dropdown">
	                                    <a data-toggle="dropdown" class="btn btn-sm btn-clean btn-icon btn-icon-md" aria-expanded="false">
		                                    <i class="la la-ellipsis-h"></i>
	                                    </a>
	                                    <div class="dropdown-menu dropdown-menu-right" x-placement="bottom-end">
		                                    <button class="dropdown-item" data-js="delete-sub-category" data-id="${row.SharedID}">${$language.get('deletesubcategory')}</button>
		                                    <a href="/new-category?id=${row.SharedID}&type=${row.Type}" class="dropdown-item">${$language.get('addsubcategory')}</a>
		                                    <button class="dropdown-item" data-js="move-category" data-id="${row.SharedID}" data-parentid="${row.ParentID}">${$language.get('move')}</button>
		                                    <button class="dropdown-item" data-js="copy-category" data-id="${row.SharedID}">${$language.get('copy')}</button>
	                                    </div>
                                    </div>
                                    <a href="/edit-category?id=${row.ID}" class="btn btn-sm btn-clean btn-icon btn-icon-md" title="Edit details">
	                                    <i class="la la-edit"></i>
                                    </a>
                                    <button type="button" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-js="delete-record" data-id="${row.ID}" data-isparent="${row.Parent}" title="Delete">
	                                    <i class="la la-trash"></i>
                                    </button>`;
                        }
                    }
                ]
            });
        },
        AddCategorySuccess: function (response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success');
                if (datatable != undefined) {
                    datatable.reload();
                }
                else if (response.Url != '/categories') {
                    setTimeout(function () {
                        location.href = response.Url;
                    }, 3000);
                }
            }
            else {
                if (response.Field != null && response.Field != '') {
                    var field = $('[data-js=category-language-tab]').length ? $('.tab-pane.active').find(`#${response.Field}`) : `#${response.Field}`;
                    $core.Validate.Message(field, response.Message);
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
        },
        DeleteCategorySuccess: function (response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success');
                if (location.pathname != response.Url) {
                    setTimeout(function () {
                        location.href = response.Url;
                    }, 3000);
                }
                else {
                    datatable.reload();
                }
            }
            else {
                $core.Notify(response.Message, 'danger');
            }
        },
        Set: function (target) {
            var fd = new FormData();
            var fields = target.serializeArray();
            $.each(target.find('[disabled=disabled]'), function () {
                fields.push({
                    name: this.name,
                    value: $(this).val()
                });
            });
            for (var i = 0; i < fields.length; i++) {
                if (fd.get(fields[i].name) == null) {
                    fd.append(fields[i].name, fields[i].value);
                }
            }
            fd.append('thumbnail', target.find('[name=Thumbnail]')[0]?.files[0]);
            fd.append('banner', target.find('[name=Banner]')[0]?.files[0]);
            return fd;
        },
        Add: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $categories.Set(target), true, $categories.AddCategorySuccess);
        },
        Delete: function (id, isparent, withchild) {
            $core.Ajax($pointer.GetPointer('DeleteCategory'), 'GET', {
                id: id,
                isparent: isparent,
                withchild: withchild
            }, true, $categories.DeleteCategorySuccess);
        }
    }
    w.$categories = new categories();
    $categories.Init();
    // Tipe göre üst kategori listeleme
    $('body').on('change', '[data-js=type]', function () {
        $('[data-js=typetrigger]').val($(this).val()).change();
        function ParentCategorySuccess(response) {
            var _select = $('[data-js=parent]');
            _select.empty().append(`<option value="0">${$language.get('maincategory')}</option>`);
            $.each(response, function (i, e) {
                if (e.SharedID != $('[data-js=sharedid]').val() && e.Lang == _select.data('lang')) {
                    _select.append(`<option value="${e.SharedID}">${e.Title}</option>`);
                }
            });
            $.each($('[data-js=parenttrigger]'), function (i, e) {
                var _selectlang = $(e);
                _selectlang.empty().append(`<option value="0">${$language.get('maincategory')}</option>`);
                $.each(response, function (i, e) {
                    if (e.SharedID != $('[data-js=sharedid]').val() && e.Lang == _selectlang.data('lang')) {
                        _selectlang.append(`<option value="${e.SharedID}">${e.Title}</option>`);
                    }
                });
            });
            if (id != '') {
                $('[data-js=parent], [data-js=parenttrigger]').val(id).change();
            }
        }
        if ($('[data-js=type]').val() != 0) {
            $core.Ajax($pointer.GetPointer('GetParentCategory'), 'GET', {
                type: $('[data-js=type]').val()
            }, true, ParentCategorySuccess);
        }
    });
    // Üst kategori seçiminde dil kontrolü
    $('body').on('change', '[data-js=parent]', function () {
        var _this = $(this);
        function CheckParentCategorySuccess(response) {
            if (!response.Status) {
                $core.Notify(response.Message, 'danger');
                _this.val(0).change();
            }
            else {
                $('[data-js=parenttrigger]').val(_this.val()).change();
            }
        }
        if (_this.val() != 0) {
            $core.Ajax($pointer.GetPointer('CheckParentCategory'), 'GET', {
                id: _this.val()
            }, true, CheckParentCategorySuccess);
        }
    });
    // Başlık girildiğinde url oluşturma
    $('body').on('blur', '[data-js=title]', function () {
        var _this = $(this);
        function GetUrlSuccess(response) {
            _this.parents('form').find('[data-js=url]').val(response.url);
        }
        if (_this.parents('form').find('[data-js=url]').val() == null || _this.parents('form').find('[data-js=url]').val() == '') {
            $core.Ajax($pointer.GetPointer('GetUrlString'), 'GET', {
                text: _this.val()
            }, true, GetUrlSuccess);
        }
    });
    // Ana kategori değiştiğinde diğer dilleri değiştirme
    $('body').on('change', '[data-js=showmenu]', function () {
        var _this = $(this);
        $('[data-js=showmenutrigger]').prop('checked', _this.prop('checked')).val(_this.prop('checked'));
    });
    $('body').on('change', '[data-js=status]', function () {
        var _this = $(this);
        $('[data-js=statustrigger]').prop('checked', _this.prop('checked')).val(_this.prop('checked'));
    });
    // Ekleme - Güncelleme - Silme
    $('body').on('click', '[data-js=add-category], [data-js=add-single-category]', function () {
        var _form = $(this).data('js') == 'add-category' ? $('[data-js=new-category]') : $(this).parents('[data-js=new-category]');
        $categories.Add(_form, 'AddCategory');
    });
    $('body').on('click', '[data-js=update-category], [data-js=update-single-category]', function () {
        var _form = $(this).data('js') == 'update-category' ? $('[data-js=edit-category]') : $(this).parents('[data-js=edit-category]');
        $categories.Add(_form, 'UpdateCategory');
    });
    $('body').on('click', '[data-js=update-all-categories]', function () {
        var _forms = $('[data-type=category-form]');
        $.each(_forms, function (findex, e) {
            function AddAllCategorySuccess(response) {
                if (response.Status) {
                    $core.Notify(response.Message, 'success');
                }
                else {
                    $('[data-js=category-language-tab] [data-lang=' + $(e).find('#Lang').val() + ']').click();
                    if (response.Field != null && response.Field != '') {
                        $core.Validate.Message($(e).find(`#${response.Field}`), response.Message)
                    }
                    else {
                        $core.Notify(response.Message, 'danger');
                    }
                }
            }
            $core.AjaxPost($pointer.GetPointer('UpdateCategory'), 'POST', $categories.Set($(e)), true, AddAllCategorySuccess);
        });
    });
    $('body').on('click', '[data-js=update-column]', function () {
        var _this = $(this);
        $core.Ajax($pointer.GetPointer('UpdateCategoryColumn'), 'GET', {
            id: _this.data('id'),
            column: _this.data('column'),
            value: _this.data('value')
        }, true, $categories.AddCategorySuccess);
    });
    $('body').on('click', '[data-js=delete-confirm]', function () {
        $categories.Delete($(this).data('id'), $(this).data('isparent'), $(this).data('withchild'));
        $('#modal-' + $(this).data('target')).modal('hide');
    });
    $('body').on('click', '[data-js=delete-record]', function () {
        var text = $(this).data('isparent') ? $language.get('surefordeletewithchild') : $language.get('surefordelete');
        var buttons = [{
            data: [
                {
                    name: 'js',
                    value: 'delete-confirm'
                },
                {
                    name: 'id',
                    value: $(this).data('id')
                },
                {
                    name: 'isparent',
                    value: $(this).data('isparent')
                },
                {
                    name: 'withchild',
                    value: true
                }
            ],
            class: 'btn-danger',
            text: $(this).data('isparent') ? $language.get('deleteall') : $language.get('delete')
        }];
        if ($(this).data('isparent')) {
            buttons.push({
                data: [
                    {
                        name: 'js',
                        value: 'delete-confirm'
                    },
                    {
                        name: 'id',
                        value: $(this).data('id')
                    },
                    {
                        name: 'isparent',
                        value: $(this).data('isparent')
                    },
                    {
                        name: 'withchild',
                        value: false
                    }
                ],
                class: 'btn-label-danger btn-outline-danger',
                text: $language.get('deleteselected')
            });
        }
        $core.OpenModal({
            id: 'delete-confirm',
            body: text,
            buttons: buttons
        });
    });
    // Alt Kategori Silme
    $('body').on('click', '[data-js=delete-sub-category]', function () {
        $core.Ajax($pointer.GetPointer("DeleteSubCategory"), 'GET', {
            id: $(this).data('id')
        }, true, $categories.DeleteCategorySuccess);
    });
    // Kategori Taşıma
    $('body').on('click', '[data-js=move-category]', function () {
        var _this = $(this);
        $core.OpenModal({
            id: 'move-confirm',
            size: 'modal-md',
            body: `<div id="move-category-tree" data-js="move-category-tree">
                        <div class="w-100 py-4 bg-transparent loading"><span class="position-absolute"></span></div>
                    </div>`,
            buttons: [{
                data: [
                    {
                        name: 'js',
                        value: 'move-confirm'
                    },
                    {
                        name: 'id',
                        value: _this.data('id')
                    }
                ],
                class: 'btn-success',
                text: $language.get('move')
            }],
            callback: function () {
                categorytree = $('#move-category-tree').jstree({
                    'core': {
                        'themes': {
                            'responsive': false
                        },
                        // so that create works
                        'check_callback': true,
                        'data': {
                            'url': function (node) {
                                return $pointer.GetPointer("JsonCategoryTree") + '?id=' + _this.data('id');
                            },
                            'data': function (node) {
                                return {
                                    'id': node.id,
                                    'parent': node.id
                                };
                            }
                        }
                    },
                    'types': {
                        'default': {
                            'icon': 'fa fa-folder kt-font-brand'
                        },
                        'file': {
                            'icon': 'fa fa-file kt-font-brand'
                        }
                    },
                    'state': { 'key': 'category-tree' },
                    'plugins': ['dnd', 'state', 'types']
                });
            }
        });
    });
    $('body').on('click', '[data-js=move-confirm]', function () {
        var parent = categorytree.find('[aria-selected="true"]').attr('id') / 1 > -1 ? categorytree.find('[aria-selected="true"]').attr('id') : 0;
        $core.Ajax($pointer.GetPointer("MoveCategory"), 'GET', {
            id: $(this).data('id'),
            parentid: parent
        }, true, $categories.AddCategorySuccess);
        $('#modal-' + $(this).data('target')).modal('hide');
    });
    // Kategori Kopyalama
    $('body').on('click', '[data-js=copy-category]', function () {
        $core.Ajax($pointer.GetPointer("CopyCategory"), 'GET', {
            id: $(this).data('id')
        }, true, $categories.AddCategorySuccess);
    });
    $('body').on('click', '[data-js=copy-selected-categories]', function () {
        var sids = [];
        $.each(datatable.getSelectedRecords(), function (i, e) {
            $.each(datatable.getDataSet(), function (ii, ee) {
                if (ee.ID == $(e).find('td[data-field=ID] a').data('value')) {
                    sids.push(ee.SharedID);
                    pids.push(ee.ParentID);
                    if (ee.Parent) isparent = true;
                }
            });
        });
        $core.Ajax($pointer.GetPointer("CopySelectedCategories"), 'GET', {
            sharedids: ids
        }, true, $categories.AddCategorySuccess);
    });
    // Datatable
    if (datatable != undefined) {
        datatable.on(
            'kt-datatable--on-check kt-datatable--on-uncheck kt-datatable--on-layout-updated',
            function (e) {
                var checkedNodes = datatable.rows('.kt-datatable__row--active').nodes();
                var count = checkedNodes.length;
                $('#kt_datatable_selected_number').html(count);
                if (count > 0) {
                    $('#kt_datatable_group_action_form').collapse('show');
                } else {
                    $('#kt_datatable_group_action_form').collapse('hide');
                }
            });
        $('#kt_form_status').on('change', function () {
            datatable.search($(this).val(), 'Status');
        });
        $('#kt_form_type').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'Type');
        });
        $('#kt_form_status,#kt_form_type').selectpicker();
    }
    if (!!id && id != '' && !!type && type != '') {
        $('[data-js=type]').val(type).change();
    }
}(window));