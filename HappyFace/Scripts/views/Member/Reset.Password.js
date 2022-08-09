(function () {
    var resetpass = function () { };
    resetpass.constructor = resetpass;
    resetpass.prototype = {
        Init: function () { },
        Success: function (response) {
            $core.Validate.Response(response, '[data-js=resetpass-message]');
        }
    }
    var $resetpass = new resetpass();
    $resetpass.Init();
    $('body').on('click', '[data-js=button-resetpass]', function () {
        $site.MemberFormPost($('[data-js=resetpass-form]'), $resetpass.Success);
    });
}());