var datatable, categorytree, id = '', type = '';
(function (w) {
    var comments = function () { };
    comments.constructor = comments;
    comments.prototype = {
        Init: function () {
            $comments.Fill();
            $admin.Summernote('[data-js=Content]');
            $('[data-js=menu-item-contact]').addClass('kt-menu__item--active kt-menu__item--open');
        },
        // Main Table
        Fill: function () {
            datatable = $('#comments').KTDatatable({
                data: {
                    type: 'remote',
                    source: {
                        read: {
                            url: $pointer.GetPointer("JsonComment")
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
                        field: 'Comment',
                        title: $language.get('comment')
                    }, {
                        field: 'Type',
                        title: $language.get('type')
                    }, {
                        field: 'Status',
                        title: $language.get('status'),
                        template: function (row) {
                            return row.Status ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="Status" data-value="true" data-id="' + row.ID + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="Status" data-value="false" data-id="' + row.ID + '"></button>';
                        }
                    }, {
                        field: 'IsConfirmed',
                        title: $language.get('isconfirmed'),
                        template: function (row) {
                            return row.IsConfirmed ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="IsConfirmed" data-value="true" data-id="' + row.ID + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="IsConfirmed" data-value="false" data-id="' + row.ID + '"></button>';
                        }
                    }, {
                        field: 'Actions',
                        title: $language.get('actions'),
                        sortable: false,
                        width: 110,
                        overflow: 'visible',
                        autoHide: false,
                        template: function (row) {
                            return `<a href="/answer-comment?id=${row.ID}" class="btn btn-sm btn-clean btn-icon btn-icon-md" title="${$language.get('edit')}">
	                                    <i class="la la-edit"></i>
                                    </a>
                                    <button type="button" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-js="delete-record" data-id="${row.ID}" title="${$language.get('delete')}">
	                                    <i class="la la-trash"></i>
                                    </button>`;
                        }
                    }
                ]
            });
            if ($('#comment-answers').length) {
                datatable = $('#comment-answers').KTDatatable({
                    data: {
                        type: 'remote',
                        source: {
                            read: {
                                url: $pointer.GetPointer("JsonCommentAnswer") + '?parent=' + $('#comment-answers').data('parent')
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
                            field: 'Comment',
                            title: $language.get('comment')
                        }, {
                            field: 'Type',
                            title: $language.get('type')
                        }, {
                            field: 'Status',
                            title: $language.get('status'),
                            template: function (row) {
                                return row.Status ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="Status" data-value="true" data-id="' + row.ID + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="Status" data-value="false" data-id="' + row.ID + '"></button>';
                            }
                        }, {
                            field: 'IsConfirmed',
                            title: $language.get('isconfirmed'),
                            template: function (row) {
                                return row.IsConfirmed ? '<button type="button" class="btn px-0 text-success flaticon2-checkmark" data-js="update-column" data-column="IsConfirmed" data-value="true" data-id="' + row.ID + '"></button>' : '<button type="button" class="btn px-0 text-danger flaticon2-close-cross" data-js="update-column" data-column="IsConfirmed" data-value="false" data-id="' + row.ID + '"></button>';
                            }
                        }, {
                            field: 'Actions',
                            title: $language.get('actions'),
                            sortable: false,
                            width: 110,
                            overflow: 'visible',
                            autoHide: false,
                            template: function (row) {
                                return `<a href="/answer-comment?id=${row.ID}" class="btn btn-sm btn-clean btn-icon btn-icon-md" title="${$language.get('edit')}">
	                                    <i class="la la-edit"></i>
                                    </a>
                                    <button type="button" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-js="delete-record" data-id="${row.ID}" title="${$language.get('delete')}">
	                                    <i class="la la-trash"></i>
                                    </button>`;
                            }
                        }
                    ]
                });
            }
        },
        AnswerCommentSuccess: function (response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success');
                if (datatable != undefined) {
                    datatable.reload();
                }
                else if (response.Url != '/comments') {
                    setTimeout(function () {
                        location.href = response.Url;
                    }, 3000);
                }
            }
            else {
                if (response.Field != null && response.Field != '') {
                    var field = `[data-js=answer-comment] #${response.Field}`;
                    $core.Validate.Message(field, response.Message);
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
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
        Answer: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $comments.Set(target), true, $comments.AnswerCommentSuccess);
        },
        DeleteCommentSuccess: function (response) {
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
        Delete: function (id) {
            $core.Ajax($pointer.GetPointer('DeleteComment'), 'GET', {
                id: id
            }, true, $comments.DeleteCommentSuccess);
        }
    }
    w.$comments = new comments();
    $comments.Init();
    // Güncelleme - Yanıtlama - Silme
    $('body').on('click', '[data-js=update-column]', function () {
        var _this = $(this);
        $core.Ajax($pointer.GetPointer('UpdateCommentColumn'), 'GET', {
            id: _this.data('id'),
            column: _this.data('column'),
            value: _this.data('value')
        }, true, $comments.AnswerCommentSuccess);
    });
    $('body').on('click', '[data-js=answer]', function () {
        var _form = $('[data-js=answer-comment]');
        $comments.Answer(_form, 'AnswerComment');
    });
    $('body').on('click', '[data-js=delete-confirm]', function () {
        $comments.Delete($(this).data('id'));
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