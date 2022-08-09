var datatable, categorytree, id = '', type = '';
(function (w) {
    var posts = function () { };
    posts.constructor = posts;
    posts.prototype = {
        Init: function () {
            $posts.Fill('#posts', $pointer.GetPointer("JsonPost"), [
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
                    field: 'Banner',
                    title: 'Banner',
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
                    field: 'Carousel',
                    title: $language.get('order')
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
		                                <button class="dropdown-item" data-js="move-post" data-id="${row.SharedID}" data-parentid="${row.ParentID}">${$language.get('move')}</button>
		                                <button class="dropdown-item" data-js="copy-post" data-id="${row.SharedID}">${$language.get('copy')}</button>
	                                </div>
                                </div>
                                <a href="/edit-post?id=${row.ID}" class="btn btn-sm btn-clean btn-icon btn-icon-md" title="${$language.get('edit')}">
	                                <i class="la la-edit"></i>
                                </a>
                                <button type="button" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-js="delete-record" data-sharedid="${row.SharedID}" title="${$language.get('delete')}">
	                                <i class="la la-trash"></i>
                                </button>`;
                    }
                }
            ]);
            $posts.Fill('#latest-posts', $pointer.GetPointer("JsonLatestPost"), [
                {
                    field: 'ID',
                    title: '',
                    sortable: false,
                    width: 30,
                    textAlign: 'center'
                },
                {
                    field: 'Title',
                    title: $language.get('title'),
                    width: 200
                }, {
                    field: 'Description',
                    title: $language.get('description')
                }, {
                    field: 'Actions',
                    title: $language.get('actions'),
                    sortable: false,
                    width: 110,
                    overflow: 'visible',
                    autoHide: false,
                    template: function (row) {
                        return `<button type="button" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-js="delete-episode" data-id="${row.ID}" title="${$language.get('delete')}">
	                                <i class="la la-trash"></i>
                                </button>`;
                    }
                }
            ]);
            $admin.Summernote('[data-js=Content]');
            $('[data-js=menu-item-podcast]').addClass('kt-menu__item--active kt-menu__item--open');
        },
        // Main Table
        Fill: function (target, pointer, columns) {
            datatable = $(target).KTDatatable({
                data: {
                    type: 'remote',
                    source: {
                        read: {
                            url: pointer
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
                columns: columns
            });
        },
        AddPostSuccess: function (response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success');
                if (datatable != undefined) {
                    datatable.reload();
                }
                else if (response.Url != '/posts') {
                    setTimeout(function () {
                        location.href = response.Url;
                    }, 3000);
                }
                else if (response.Url == '/refresh') {
                    setTimeout(function () {
                        location.reload();
                    }, 3000);
                }
            }
            else {
                if (response.Field != null && response.Field != '') {
                    var field = $('[data-js=post-language-tab]').length ? $('.tab-pane.active').find(`#${response.Field}`) : `#${response.Field}`;
                    $core.Validate.Message(field, response.Message);
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
        },
        DeletePostSuccess: function (response) {
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
        Set: function (target, withMedia = true) {
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
            if (withMedia) {
                fd.append('thumbnail', target.find('[name=Thumbnail]')[0].files[0]);
                fd.append('banner', target.find('[name=Banner]')[0].files[0]);
                fd.append('trailer', target.find('[name=Trailer]')[0].files[0]);
            }
            if (target.find('[name=Alternates]').length) {
                fd.set('Alternates', target.find('[name=Alternates]').val().join(','));
            }
            return fd;
        },
        Add: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $posts.Set(target), true, $posts.AddPostSuccess);
        },
        Delete: function (sharedid) {
            $core.Ajax($pointer.GetPointer('DeletePost'), 'GET', {
                sharedid: sharedid
            }, true, $posts.DeletePostSuccess);
        },
        UpdateWeeklyBees: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $posts.Set(target, false), true, $posts.AddPostSuccess);
        },
        UpdateLatestEpisode: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $posts.Set(target, false), true, function (response) {
                $posts.AddPostSuccess(response);
                var form = $('[data-js=latest-episode]');
                form.addClass('d-none');
                form.resetForm();
            });
        },
        DeleteEpisode: function (id) {
            $core.Ajax($pointer.GetPointer('DeleteLatestEpisode'), 'GET', {
                id: id
            }, true, $posts.AddPostSuccess);
        }
    }
    w.$posts = new posts();
    $posts.Init();
    // Tipe göre üst kategori listeleme
    $('body').on('change', '[data-js=category]', function () {
        $('[data-js=categorytrigger]').val($(this).val()).change();
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
    $('body').on('change', '[data-js=status]', function () {
        var _this = $(this);
        $('[data-js=statustrigger]').prop('checked', _this.prop('checked')).val(_this.prop('checked'));
    });
    // Ekleme - Güncelleme - Silme
    $('body').on('click', '[data-js=add-post], [data-js=add-single-post]', function () {
        var _form = $(this).data('js') == 'add-post' ? $('[data-js=new-post]') : $(this).parents('[data-js=new-post]');
        $posts.Add(_form, 'AddPost');
    });
    $('body').on('click', '[data-js=update-post], [data-js=update-single-post]', function () {
        var _form = $(this).data('js') == 'update-post' ? $('[data-js=edit-post]') : $(this).parents('[data-js=edit-post]');
        $posts.Add(_form, 'UpdatePost');
    });
    $('body').on('click', '[data-js=update-all-posts]', function () {
        var _forms = $('[data-type=post-form]');
        $.each(_forms, function (findex, e) {
            function AddAllPostSuccess(response) {
                if (response.Status) {
                    $core.Notify(response.Message, 'success');
                }
                else {
                    $('[data-js=post-language-tab] [data-lang=' + $(e).find('#Lang').val() + ']').click();
                    if (response.Field != null && response.Field != '') {
                        $core.Validate.Message($(e).find(`#${response.Field}`), response.Message)
                    }
                    else {
                        $core.Notify(response.Message, 'danger');
                    }
                }
            }
            $core.AjaxPost($pointer.GetPointer('UpdatePost'), 'POST', $posts.Set($(e)), true, AddAllPostSuccess);
        });
    });
    $('body').on('click', '[data-js=update-column]', function () {
        var _this = $(this);
        $core.Ajax($pointer.GetPointer('UpdatePostColumn'), 'GET', {
            id: _this.data('id'),
            column: _this.data('column'),
            value: _this.data('value')
        }, true, $posts.AddPostSuccess);
    });
    $('body').on('click', '[data-js=delete-confirm]', function () {
        $posts.Delete($(this).data('sharedid'));
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
                    name: 'sharedid',
                    value: $(this).data('sharedid')
                },
            ],
            class: 'btn-danger',
            text: $(this).data('isparent') ? $language.get('deleteall') : $language.get('delete')
        }];
        $core.OpenModal({
            id: 'delete-confirm',
            body: text,
            buttons: buttons
        });
    });
    // Post Taşıma
    $('body').on('click', '[data-js=move-post]', function () {
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
        var category = categorytree.find('[aria-selected="true"]').attr('id') / 1 > -1 ? categorytree.find('[aria-selected="true"]').attr('id') : 0;
        $core.Ajax($pointer.GetPointer("MovePost"), 'GET', {
            id: $(this).data('id'),
            categoryid: category
        }, true, $posts.AddPostSuccess);
        $('#modal-' + $(this).data('target')).modal('hide');
    });
    // Post Kopyalama
    $('body').on('click', '[data-js=copy-post]', function () {
        $core.Ajax($pointer.GetPointer("CopyPost"), 'GET', {
            id: $(this).data('id')
        }, true, $posts.AddPostSuccess);
    });
    // Trailer Silme
    $('body').on('click', '[data-js=delete-trailer]', function () {
        $core.Ajax($pointer.GetPointer("DeleteTrailer"), 'GET', {
            id: $(this).data('id')
        }, true, $posts.AddPostSuccess);
    });
    // Weekly Bees Güncelleme
    $('body').on('click', '[data-js=update-weekly-bees]', function () {
        var form = $('[data-js=weekly-bees-form]');
        $posts.UpdateWeeklyBees(form, 'UpdateWeeklyBees');
    });
    // Güncel Bölüm Ekleme
    $('body').on('click', '[data-js=new-latest-episode]', function () {
        var form = $('[data-js=latest-episode]');
        form.removeClass('d-none');
    });
    $('body').on('click', '[data-js=close-latest-episode]', function () {
        var form = $('[data-js=latest-episode]');
        form.addClass('d-none');
    });
    $('body').on('change', '[data-js=posts]', function () {
        var selected = $(this).find('option:selected');
        var url = $('[data-js=url]');
        var thumbnail = $('[data-js=thumbnail]');
        var title = $('[data-js=title]');
        var description = $('[data-js=description]');
        url.val(selected.data('url'));
        thumbnail.val(selected.data('thumbnail'));
        title.val(selected.text());
        description.val(selected.data('description'));
    });
    $('body').on('click', '[data-js=update-latest-episode]', function () {
        var form = $('[data-js=latest-episode-form]');
        $posts.UpdateLatestEpisode(form, 'UpdateLatestEpisode');
    });
    // Güncel Bölüm Silme
    $('body').on('click', '[data-js=delete-episode-confirm]', function () {
        $posts.DeleteEpisode($(this).data('id'));
        $('#modal-' + $(this).data('target')).modal('hide');
    });
    $('body').on('click', '[data-js=delete-episode]', function () {
        var text = $language.get('surefordelete');
        var buttons = [{
            data: [
                {
                    name: 'js',
                    value: 'delete-episode-confirm'
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
            id: 'delete-episode-confirm',
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
        $('#kt_form_category').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'CategoryID');
        });
        $('#kt_form_category').selectpicker();
    }
}(window));