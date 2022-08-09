(function () {
    var login = function () { };
    login.constructor = login;
    login.prototype = {
        Init: function () { },
        Success: function (response) {
            $core.Validate.Response(response, '[data-js=login-message]');
        }
    }
    var $login = new login();
    $login.Init();
    $('body').on('click', '[data-js=button-login]', function () {
        $site.MemberFormPost($('[data-js=login-form]'), $login.Success);
    });
}());