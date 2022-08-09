(function () {
    var reactivation = function () { };
    reactivation.constructor = reactivation;
    reactivation.prototype = {
        Init: function () { },
        Success: function (response) {
            $core.Validate.Response(response, '[data-js=reactivation-message]');
        }
    }
    var $reactivation = new reactivation();
    $reactivation.Init();
    $('body').on('click', '[data-js=button-reactivation]', function () {
        $site.MemberFormPost($('[data-js=reactivation-form]'), $reactivation.Success);
    });
}());