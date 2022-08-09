(function () {
    var forgetpass = function () { };
    forgetpass.constructor = forgetpass;
    forgetpass.prototype = {
        Init: function () { },
        Success: function (response) {
            $core.Validate.Response(response, '[data-js=forgetpass-message]');
        }
    }
    var $forgetpass = new forgetpass();
    $forgetpass.Init();
    $('body').on('click', '[data-js=button-forgetpass]', function () {
        $site.MemberFormPost($('[data-js=forgetpass-form]'), $forgetpass.Success);
    });
}());