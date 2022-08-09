(function (w) {
    var core = function () { };
    core.constructor = core;
    core.prototype = {
        Init: function () { },
        Ajax: function (url, type, params, loader, successfn, errorfn, completefn) {
            if (loader) $core.LoadingStart();
            $.ajax({
                url: url,
                type: type,
                data: params,
                async: true,
                success: function (response) {
                    if (typeof successfn == 'function') successfn(response);
                },
                error: function (response) {
                    if (typeof errorfn == 'function') errorfn(response);
                },
                complete: function (response) {
                    if (loader) $core.LoadingStop();
                    if (typeof completefn == 'function') completefn(response);
                }
            });
        },
        AjaxPost: function (url, type, params, loader, successfn, errorfn, completefn) {
            if (loader) $core.LoadingStart();
            $.ajax({
                url: url,
                type: type,
                data: params,
                processData: false,
                contentType: false,
                async: true,
                success: function (response) {
                    if (typeof successfn == 'function') successfn(response);
                },
                error: function (response) {
                    if (typeof errorfn == 'function') errorfn(response);
                },
                complete: function (response) {
                    if (loader) $core.LoadingStop();
                    if (typeof completefn == 'function') completefn(response);
                }
            });
        },
        OptionsModal: {
            id: 'general',
            size: 'modal-lg',
            html: '',
            title: $language.get('attention'),
            body: null,
            buttons: [],
            callback: null
        },
        OpenModal: function myfunction(options) {
            $('body').append($core.CreateModal($.extend({}, $core.OptionsModal, options)));
            if (options.callback != null && typeof options.callback == 'function') {
                $('#modal-' + options.id).on('shown.bs.modal', function () {
                    setTimeout(function myfunction() {
                        options.callback();
                    }, 500);
                });
            }
            $('#modal-' + options.id).modal('show').on('hidden.bs.modal', function () {
                setTimeout(function myfunction() {
                    $('#modal-' + options.id).remove();
                }, 500);
            });
        },
        CreateModal: function (options) {
            if (options.html == null || options.html == '') {
                options.html += options.title != null && options.title ?
                    `<div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLongTitle">${options.title}</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>
                    </div>` :
                    `<div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>
                    </div>`;
                if (options.body != null && options.body != '') {
                    options.html += `<div class="modal-body">${options.body}</div>`;
                }
                options.html += `<div class="modal-footer">
									<button type="button" class="btn btn-secondary" data-dismiss="modal">${$language.get('close')}</button>
                                    ${$core.ButtonsModal(options.id, options.buttons)}
								</div>`;
            }
            return `<div class="modal fade" id="modal-${options.id}" tabindex="-1" role="dialog" aria-labelledby="modal-${options.id}" aria-modal="true">
                        <div class="modal-dialog modal-dialog-centered ${options.size}" role="document">
                            <div class="modal-content" data-js="modal-content">
                                ${options.html}
                            </div>
                        </div>
                    </div>`;
        },
        ButtonsModal: function (target, buttons) {
            var html = '';
            $.each(buttons, function (i, e) {
                var dataset = [];
                $.each(e.data, function (i, e) {
                    dataset[i] = 'data-' + e.name + '="' + e.value + '"';
                });
                html += `<button type="button" class="btn ${e.class}" ${dataset.join(' ')} data-target="${target}">${e.text}</button>`;
            });
            return html;
        },
        LoadingStart: function () {
            $('body').addClass('loader').append('<div class="fixed-top w-100 loading"><span class="position-absolute"></span></div>');
        },
        LoadingStop: function () {
            $('body').removeClass('loader');
            $('.loading').addClass('hide');
            setTimeout(function () {
                $('.loading').remove();
            }, 250)
        },
        InitPartial: function (url, target, append = true) {
            function InitPartialSuccess(response) {
                append ? $(target).append(response) : $(target).html(response);
            }
            $core.Ajax(url, 'GET', null, false, InitPartialSuccess);
        },
        Notify: function (message, type, target, hide = true) {
            var notify = $(`<div class="alert alert-${type} py-1 px-3 small" role="alert">${message}</div>`);
            if (hide == false) {
                $(target != null ? target : '[data-js=notifies]').empty();
            }
            $(target != null ? target : '[data-js=notifies]').prepend(notify);
            setTimeout(function () {
                notify.addClass('show');
            }, 250);
            typeof hide == 'number' ? hide += 50 : false;
            if (hide != false) {
                hide ? hide = 3000 : hide;
                setTimeout(function () {
                    notify.removeClass('show');
                    setTimeout(function () {
                        notify.remove();
                    }, 300);
                }, hide);
            }
        },
        Validate: {
            Response: function (response, target) {
                if (response.Status) {
                    $core.Notify(response.Message, 'success', target, false);
                    setTimeout(function () {
                        location.href = response.Redirect;
                    }, 3000);
                }
                else {
                    if (response.Field != null && response.Field != '') {
                        $core.Validate.Message(`#${response.Field}`, response.Message);
                    }
                    else {
                        $core.Notify(response.Message, 'danger');
                    }
                }
            },
            Message: function (field, message = $language.Get('erroremptyfield')) {
                var _field = field == 'object' ? field : $(field);
                typeof _field.addClass('is-invalid');
                _field.parent().find('.invalid-feedback').remove();
                $(`<div class="invalid-feedback">${message}</div>`).insertAfter(_field);
            },
            Clear: function () {
                $('.is-invalid').removeClass('is-invalid');
                $('.invalid-feedback').remove();
            }
        }
    }
    w.$core = new core();
    $core.Init();
    $('body').on('blur', '.is-invalid', function () {
        $core.Validate.Clear();
    });
}(window));