(function () {
    var personelInformation = function () { };
    personelInformation.constructor = personelInformation;
    personelInformation.prototype = {
        Init: function () {
            $('[data-target=personel-information]').addClass('active');
        },
        Success: function (response) {
            $core.Validate.Response(response, '[data-js=personel-information-message]');
        }
    }
    var $personelInformation = new personelInformation();
    $personelInformation.Init();
    $('body').on('click', '[data-js=button-personel-information]', function () {
        $site.MemberFormPost($('[data-js=personel-information-form]'), $personelInformation.Success);
    });
}());