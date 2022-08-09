(function () {
    var changepass = function () { };
    changepass.constructor = changepass;
    changepass.prototype = {
        Init: function () {
            $('[data-target=change-password]').addClass('active');
        },
        Success: function (response) {
            $core.Validate.Response(response, '[data-js=changepass-message]');
        }
    }
    var $changepass = new changepass();
    $changepass.Init();
    $('body').on('click', '[data-js=button-changepass]', function () {
        $site.MemberFormPost($('[data-js=changepass-form]'), $changepass.Success);
    });
}());