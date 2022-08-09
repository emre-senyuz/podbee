var datatable, id = '', type = '';
(function (w) {
    var slides = function () { };
    slides.constructor = slides;
    slides.prototype = {
        Init: function () {
            $slides.Fill();
            $admin.Summernote('[data-js=Description]');
            $('[data-js=menu-item-carousel]').addClass('kt-menu__item--active kt-menu__item--open');
        },
        // Main Table
        Fill: function () {
            datatable = $('#slides').KTDatatable({
                data: {
                    type: 'remote',
                    source: {
                        read: {
                            url: $pointer.GetPointer("JsonSlide")
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
                        title: $language.get('description'),
                        width: 400
                    }, {
                        field: 'Lang',
                        title: $language.get('language'),
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
                            return `<a href="/edit-slide?id=${row.ID}" class="btn btn-sm btn-clean btn-icon btn-icon-md" title="${$language.get('edit')}">
	                                    <i class="la la-edit"></i>
                                    </a>
                                    <button type="button" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-js="delete-record" data-id="${row.ID}" title="${$language.get('delete')}">
	                                    <i class="la la-trash"></i>
                                    </button>`;
                        }
                    }
                ]
            });
        },
        AddSlideSuccess: function (response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success');
                if (datatable != undefined) {
                    datatable.reload();
                }
                else if (response.Url != '/slides') {
                    setTimeout(function () {
                        location.href = response.Url;
                    }, 3000);
                }
            }
            else {
                if (response.Field != null && response.Field != '') {
                    var field = `#${response.Field}`;
                    $core.Validate.Message(field, response.Message);
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
        },
        DeleteSlideSuccess: function (response) {
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
            fd.append('banner', target.find('[name=Banner]')[0].files[0]);
            return fd;
        },
        Add: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $slides.Set(target), true, $slides.AddSlideSuccess);
        },
        Delete: function (id) {
            $core.Ajax($pointer.GetPointer('DeleteSlide'), 'GET', {
                id: id
            }, true, $slides.DeleteSlideSuccess);
        }
    }
    w.$slides = new slides();
    $slides.Init();
    // Ekleme - Güncelleme - Silme
    $('body').on('click', '[data-js=add-slide]', function () {
        var _form = $('[data-js=new-slide]');
        $slides.Add(_form, 'AddSlide');
    });
    $('body').on('click', '[data-js=update-slide]', function () {
        var _form = $('[data-js=edit-slide]');
        $slides.Add(_form, 'UpdateSlide');
    });
    $('body').on('click', '[data-js=update-column]', function () {
        var _this = $(this);
        $core.Ajax($pointer.GetPointer('UpdateSlideColumn'), 'GET', {
            id: _this.data('id'),
            column: _this.data('column'),
            value: _this.data('value')
        }, true, $slides.AddSlideSuccess);
    });
    $('body').on('click', '[data-js=delete-confirm]', function () {
        $slides.Delete($(this).data('id'));
        $('#modal-' + $(this).data('target')).modal('hide');
    });
    $('body').on('click', '[data-js=delete-record]', function () {
        var text = $language.get('surefordelete');
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
            ],
            class: 'btn-danger',
            text: $language.get('delete')
        }];
        $core.OpenModal({
            id: 'delete-confirm',
            body: text,
            buttons: buttons
        });
    });
    // Slide Kopyalama
    $('body').on('click', '[data-js=copy-slide]', function () {
        $core.Ajax($pointer.GetPointer("CopySlide"), 'GET', {
            id: $(this).data('id')
        }, true, $slides.AddSlideSuccess);
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
        $('#kt_form_language').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'Lang');
        });
        $('#kt_form_language').selectpicker();
    }
}(window));