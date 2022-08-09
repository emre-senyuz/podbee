(function (w) {
    var admin = function () { };
    admin.constructor = admin;
    admin.prototype = {
        Init: function () {},
        SetLanguage: function (language) {
            function SetLanguageSuccess(response) {
                if (response.Status) {
                    location.reload();
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
            $core.Ajax($pointer.GetPointer('SetLanguage'), 'GET', {
                language: language,
                type: 'Home'
            }, false, SetLanguageSuccess);
        },
        Datatable: function (selector, url, columns) {
            return $(selector).KTDatatable({
                data: {
                    type: 'remote',
                    source: {
                        read: {
                            url: url
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
                search: {
                    input: $('#generalSearch')
                },
                columns: columns
            });
        },
        Summernote: function (target) {
            var _target = $(target);
            _target.summernote({
                height: 150
            });
        }
    }
    w.$admin = new admin();
    $admin.Init();
    $('body').on('click', '.kt-header__topbar-item--langs .kt-nav__item:not(.kt-nav__item--active)', function (e) {
        e.preventDefault();
        $admin.SetLanguage($(this).data('prefix'));
    });
}(window));