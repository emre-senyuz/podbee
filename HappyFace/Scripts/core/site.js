(function (w) {
    var site = function () { };
    site.constructor = site;
    site.prototype = {
        Init: function () { },
        MemberFormPost: function (form, successfn) {
            $core.Ajax(form.attr('action'), 'POST', form.serialize(), true, successfn);
        }
    }
    w.$site = new site();
}(window));