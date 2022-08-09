(function () {
    var register = function () { };
    register.constructor = register;
    register.prototype = {
        Init: function () {
            $('[data-js=BirthDate]').datepicker({
                language: 'tr'
            });
        },
        Success: function (response) {
            $core.Validate.Response(response, '[data-js=register-message]');
        }
    }
    var $register = new register();
    $register.Init();
    $('body').on('click', '[data-js=button-register]', function () {
        $site.MemberFormPost($('[data-js=register-form]'), $register.Success);
    });
}());