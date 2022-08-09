(function () {
    var comments = function () { };
    comments.constructor = comments;
    comments.prototype = {
        Init: function () {
            $('[data-target=comments]').addClass('active');
        }
    }
    var $comments = new comments();
    $comments.Init();
}());