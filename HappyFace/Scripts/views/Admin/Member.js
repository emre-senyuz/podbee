var datatable, id = '', type = '';
(function (w) {
    var members = function () { };
    members.constructor = members;
    members.prototype = {
        Init: function () {
            $members.Fill();
            $admin.Summernote('[data-js=Description]');
            $('[data-js=menu-item-carousel]').addClass('kt-menu__item--active kt-menu__item--open');
        },
        // Main Table
        Fill: function () {
            datatable = $('#members').KTDatatable({
                data: {
                    type: 'remote',
                    source: {
                        read: {
                            url: $pointer.GetPointer("JsonMember")
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
                        field: 'Name',
                        title: $language.get('name')
                    }, {
                        field: 'Surname',
                        title: $language.get('surname')
                    }, {
                        field: 'Email',
                        title: $language.get('email'),
                        width: 400
                    }, {
                        field: 'Activated',
                        title: $language.get('activation'),
                        template: function (row) {
                            return row.Activated ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="Activated" data-value="true" data-id="' + row.ID + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="Activated" data-value="false" data-id="' + row.ID + '"></button>';
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
                            return `<a href="/edit-member?id=${row.ID}" class="btn btn-sm btn-clean btn-icon btn-icon-md" title="${$language.get('edit')}">
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
        AddMemberSuccess: function (response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success');
                if (datatable != undefined) {
                    datatable.reload();
                }
                else if (response.Url != '/members') {
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
        DeleteMemberSuccess: function (response) {
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
            return fd;
        },
        Add: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $members.Set(target), true, $members.AddMemberSuccess);
        },
        Delete: function (id) {
            $core.Ajax($pointer.GetPointer('DeleteMember'), 'GET', {
                id: id
            }, true, $members.DeleteMemberSuccess);
        }
    }
    w.$members = new members();
    $members.Init();
    // Ekleme - Güncelleme - Silme
    $('body').on('click', '[data-js=add-member]', function () {
        var _form = $('[data-js=new-member]');
        $members.Add(_form, 'AddMember');
    });
    $('body').on('click', '[data-js=update-member]', function () {
        var _form = $('[data-js=edit-member]');
        $members.Add(_form, 'UpdateMember');
    });
    $('body').on('click', '[data-js=update-column]', function () {
        var _this = $(this);
        $core.Ajax($pointer.GetPointer('UpdateMemberColumn'), 'GET', {
            id: _this.data('id'),
            column: _this.data('column'),
            value: _this.data('value')
        }, true, $members.AddMemberSuccess);
    });
    $('body').on('click', '[data-js=delete-confirm]', function () {
        $members.Delete($(this).data('id'));
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
    }
}(window));