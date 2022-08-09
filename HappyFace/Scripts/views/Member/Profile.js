(function () {
    var profile = function () { };
    profile.constructor = profile;
    profile.prototype = {
        Init: function () {
            $('[data-target=profile]').addClass('active');
        }
    }
    var $profile = new profile();
    $profile.Init();
}());