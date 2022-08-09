(function (w) {
    var layout = function () { };
    layout.constructor = layout;
    layout.prototype = {
        Init: function () {
            if (type != 'Register' && type != 'Login' && type != 'Profile' && type != 'ForgetPassword' && type != 'ResetPassword' && type != 'ChangePassword' && type != 'Activation' && type != 'Information') {
                $layout.Member();
            }
        },
        Member: function () {
            $core.InitPartial($pointer.GetPointer('ViewMember'), '[data-js=member]', false);
        },
        SetLanguage: function (language) {
            function SetLanguageSuccess(response) {
                if (response.Status) {
                    location.href = response.Url;
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
            $core.Ajax($pointer.GetPointer('SetLanguage'), 'GET', {
                language: language,
                type: type
            }, false, SetLanguageSuccess);
        },
        LoginSuccess: function (response) {
            $core.Validate.Response(response, '[data-js=header-login-message]');
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
            console.log(fields)
            for (var i = 0; i < fields.length; i++) {
                if (fd.get(fields[i].name) == null) {
                    fd.append(fields[i].name, fields[i].value);
                }
            }
            return fd;
        },
        NewsletterSuccess: function (response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success', '[data-js=newsletter-message]');
            }
            else {
                if (response.Field != null && response.Field != '') {
                    var field = `[data-js=newsletter] #${response.Field}`;
                    $core.Validate.Message(field, response.Message);
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
        },
        Newsletter: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $layout.Set(target), true, $layout.NewsletterSuccess);
        },
        ContactSuccess: function (response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success', '[data-js=contact-message]');
            }
            else {
                if (response.Field != null && response.Field != '') {
                    var field = `[data-js=contact] #${response.Field}`;
                    $core.Validate.Message(field, response.Message);
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
        },
        Contact: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $layout.Set(target), true, $layout.ContactSuccess);
        },
        CommentSuccess: function (response) {
            if (response.Status) {
                $core.Notify(response.Message, 'success', '[data-js=comment-message]');
                setTimeout(function () {
                    location.reload();
                }, 3000);
            }
            else {
                if (response.Field != null && response.Field != '') {
                    var field = `[data-js=comment] #${response.Field}`;
                    $core.Validate.Message(field, response.Message);
                }
                else {
                    $core.Notify(response.Message, 'danger');
                }
            }
        },
        Comment: function (target, url) {
            $core.AjaxPost($pointer.GetPointer(url), 'POST', $layout.Set(target), true, $layout.CommentSuccess);
        },
        VoteComment: function (id, isLike) {
            $core.Ajax($pointer.GetPointer('VoteComment'), 'GET', { id, isLike }, true, function (response) {
                $core.Notify(response.Message, response.Status ? 'success' : 'danger');
            });
        },
    }
    w.$layout = new layout();
    $layout.Init();
    $('body').on('click', '[data-js=drop-down-toggle]', function () {
        var _this = $(this);
        if (!_this.hasClass('js-active')) {
            $('[data-js=drop-down-toggle]').removeClass('js-active');
            $('[data-js=drop-down-menu]').addClass('d-none');
        }
        _this.toggleClass('js-active');
        $('[data-id=' + _this.data('target') + ']').toggleClass('d-none');
    });
    $('body').on('click', function (e) {
        if ($(e.target).closest('[data-js=drop-down-toggle], [data-js=drop-down-menu]').length == 0) {
            $('[data-js=drop-down-toggle]').removeClass('js-active');
            $('[data-js=drop-down-menu]').addClass('d-none');
        }
    });
    $('body').on('click', '[data-js=button-header-login]', function () {
        $site.MemberFormPost($('[data-js=header-login-form]'), $layout.LoginSuccess);
    });
    $('body').on('click', '[data-js=set-language]', function (e) {
        e.preventDefault();
        $layout.SetLanguage($(this).data('prefix'));
    });
    $('body').on('click', '[data-js=send-newsletter]', function (e) {
        e.preventDefault();
        var _form = $('[data-js=newsletter]');
        $layout.Newsletter(_form, 'Newsletter');
    });
    $('body').on('click', '[data-js=send-contact]', function (e) {
        e.preventDefault();
        var _form = $('[data-js=contact]');
        $layout.Contact(_form, 'Contact');
    });
    $('body').on('click', '[data-js=send-comment]', function (e) {
        e.preventDefault();
        var _form = $('[data-js=comment]');
        $layout.Comment(_form, 'Comment');
    });
    $('body').on('click', '[data-js=comment-vote-button]', function (e) {
        e.preventDefault();
        var _this = $(this);
        $layout.VoteComment(_this.data('id'), _this.data('type') == 'like');
    });
    $('body').on('click', '[data-js=trailer]', function (e) {
        e.preventDefault();
        var _this = $(this);
        var _isActive = _this.hasClass('active');
        var _audio = _this.parents('.artist').find('audio')[0]
        if (_isActive && _audio != 'undefined' && _audio != undefined) {
            _audio.pause();
            _this.removeClass('active');
        }
        else if (_audio != 'undefined' && _audio != undefined) {
            $('[data-js=trailer].active').click();
            _audio.play();
            _this.addClass('active');
        }
        _this.find('svg').toggleClass('d-none');
    });
}(window));