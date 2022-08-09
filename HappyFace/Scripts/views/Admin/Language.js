var datatable;
(function (w) {
    var languages = function () { };
    languages.constructor = languages;
    languages.prototype = {
        Init: function () {
            $languages.Fill();
            $('[data-js=menu-item-language]').addClass('kt-menu__item--active kt-menu__item--open');
        },
        Fill: function () {
            datatable = $admin.Datatable('#languages', '/Admin/LanguagesJson', [
                {
                    field: 'Title',
                    title: $language.get('title')
                }, {
                    field: 'Prefix',
                    title: $language.get('code')
                }, {
                    field: 'Direction',
                    title: $language.get('direction')
                }, {
                    field: 'Icon',
                    title: $language.get('icon'),
                    template: function (row) {
                        return row.Icon != null ? '<img src="' + row.Icon + '" alt="' + row.Title + '" height="14" />' : '';
                    }
                }, {
                    field: 'IsDefault',
                    title: $language.get('default'),
                    template: function (row) {
                        return row.IsDefault ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="IsDefault" data-value="true" data-prefix="' + row.Prefix + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="IsDefault" data-value="false" data-prefix="' + row.Prefix + '"></button>';
                    }
                }, {
                    field: 'Status',
                    title: $language.get('status'),
                    width: 110,
                    template: function (row) {
                        return row.Status ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="Status" data-value="true" data-prefix="' + row.Prefix + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="Status" data-value="false" data-prefix="' + row.Prefix + '"></button>';
                    }
                }
            ]);
        },
        AddLanguageSuccess: function(response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success');
                if (location.pathname != response.Url) {
                    setTimeout(function () {
                        location.href = response.Url;
                    }, 3000);
                }
                else if (datatable != undefined) {
                    datatable.reload();
                }
            }
            else {
                if (response.Field != null && response.Field != '') {
                    $core.Validate.Message(`#${response.Field}`, response.Message)
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
        },
        Add: function (update) {
            var fd = new FormData();
            var fields = update ? $('[data-js=edit-language]').serializeArray() : $('[data-js=new-language]').serializeArray();
            for (var i = 0; i < fields.length; i++) {
                fd.append(fields[i].name, fields[i].value);
            }
            fd.append('file', $('[name=Icon]')[0].files[0]);
            $core.AjaxPost(update ? $pointer.GetPointer('UpdateLanguage') : $pointer.GetPointer('AddLanguage'), 'POST', fd, true, $languages.AddLanguageSuccess);
        },
        Delete: function (id, prefix) {
            function DeleteLanguageSuccess(response) {
                if (response.Status) {
                    $core.Notify(response.Message, 'success');
                    if (location.pathname != response.Url) {
                        location.href = response.Url;
                    }
                    else {
                        datatable.reload();
                    }
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
            $core.Ajax($pointer.GetPointer('DeleteLanguage'), 'GET', {
                id: id,
                prefix: prefix
            }, true, DeleteLanguageSuccess);
        }
    }
    w.$languages = new languages();
    $languages.Init();
    $('body').on('click', '[data-js=add-language]', function () {
        $languages.Add();
    });
    $('body').on('click', '[data-js=update-language]', function () {
        $languages.Add(true);
    });
    $('body').on('click', '[data-js=update-column]', function () {
        var _this = $(this);
        $core.Ajax($pointer.GetPointer('UpdateLanguageColumn'), 'GET', {
            prefix: _this.data('prefix'),
            column: _this.data('column'),
            value: _this.data('value')
        }, true, $languages.AddLanguageSuccess);
    });
    $('body').on('click', '[data-js=delete-confirm]', function () {
        $languages.Delete($(this).data('id'), $(this).data('prefix'));
        $('#modal-' + $(this).data('target')).modal('hide');
    });
    $('body').on('click', '[data-js=delete-record]', function () {
        $core.OpenModal({
            id: 'delete-confirm',
            body: $language.get('surefordelete'),
            buttons: [{
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
                        name: 'prefix',
                        value: $(this).data('prefix')
                    }
                ],
                class: 'btn-danger',
                text: $language.get('delete')
            }]
        });
    });
}(window));