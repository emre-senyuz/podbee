var datatable, categorytree, id = '', type = '';
(function (w) {
    var socials = function () { };
    socials.constructor = socials;
    socials.prototype = {
        Init: function () {
            $socials.Fill();
            $('[data-js=menu-item-social]').addClass('kt-menu__item--active kt-menu__item--open');
        },
        // Main Table
        Fill: function () {
            datatable = $('#socials').KTDatatable({
                data: {
                    type: 'remote',
                    source: {
                        read: {
                            url: $pointer.GetPointer("JsonSocial")
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
                        title: $language.get('platform')
                    }, {
                        field: 'Url',
                        title: $language.get('url'),
                        width: 400
                    }, {
                        field: 'Icon',
                        title: $language.get('icon'),
                        template: function (row) {
                            return row.Banner != null ? '<i class="fa fa-lg fa-image text-success"></i>' : '<i class="fa fa-lg fa-image"></i>';
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
                            return `<a href="/edit-social?id=${row.ID}" class="btn btn-sm btn-clean btn-icon btn-icon-md" title="${$language.get('edit')}">
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
        AddSocialSuccess: function (response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success');
                if (datatable != undefined) {
                    datatable.reload();
                }
                else if (response.Url != '/blogs') {
                    setTimeout(function () {
                        location.href = response.Url;
                    }, 3000);
                }
            }
            else {
                if (response.Field != null && response.Field != '') {
                    var field = $('[data-js=blog-language-tab]').length ? $('.tab-pane.active').find(`#${response.Field}`) : `#${response.Field}`;
                    $core.Validate.Message(field, response.Message);
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
        },
        DeleteSocialSuccess: function (response) {
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
            fd.append('icon', target.find('[name=Icon]')[0].files[0]);
            return fd;
        },
        Add: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $socials.Set(target), true, $socials.AddSocialSuccess);
        },
        Delete: function (id) {
            $core.Ajax($pointer.GetPointer('DeleteSocial'), 'GET', {
                id: id
            }, true, $socials.DeleteSocialSuccess);
        }
    }
    w.$socials = new socials();
    $socials.Init();
    // Ekleme - Güncelleme - Silme
    $('body').on('click', '[data-js=add-social]', function () {
        var _form = $(this).data('js') == 'add-social' ? $('[data-js=new-social]') : $(this).parents('[data-js=new-social]');
        $socials.Add(_form, 'AddSocial');
    });
    $('body').on('click', '[data-js=update-social]', function () {
        var _form = $('[data-js=edit-social]');
        $socials.Add(_form, 'UpdateSocial');
    });
    $('body').on('click', '[data-js=update-column]', function () {
        var _this = $(this);
        $core.Ajax($pointer.GetPointer('UpdateSocialColumn'), 'GET', {
            id: _this.data('id'),
            column: _this.data('column'),
            value: _this.data('value')
        }, true, $socials.AddSocialSuccess);
    });
    $('body').on('click', '[data-js=delete-confirm]', function () {
        $socials.Delete($(this).data('id'));
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
}(window));