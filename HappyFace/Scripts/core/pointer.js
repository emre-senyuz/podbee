(function (w) {
    var pointer = function () { };
    pointer.constructor = pointer;
    pointer.prototype = {
        List: {
            /* Views */
            ViewMember: '/membermenu',
            /* Methods */
            GetUrlString: '/get-url-string',
            /* Languages */
            SetLanguage: '/setlanguage',
            DeleteLanguage: '/delete-language',
            AddLanguage: '/new-language',
            UpdateLanguage: '/edit-language',
            UpdateLanguageColumn: '/update-language',
            /* Categories */
            JsonCategory: '/json-categories',
            JsonCategoryTree: '/json-category-tree',
            AddCategory: '/new-category',
            UpdateCategory: '/edit-category',
            UpdateCategoryColumn: '/update-category',
            DeleteCategory: '/delete-category',
            GetParentCategory: '/get-parent-category',
            CheckParentCategory: '/check-parent-category',
            DeleteSubCategory: '/delete-sub-category',
            MoveCategory: '/move-category',
            CopyCategory: '/copy-category',
            /* Posts */
            JsonPost: '/json-posts',
            AddPost: '/new-post',
            UpdatePost: '/edit-post',
            UpdatePostColumn: '/update-post',
            DeletePost: '/delete-post',
            MovePost: '/move-post',
            CopyPost: '/copy-post',
            DeleteTrailer: '/delete-trailer',
            UpdateWeeklyBees: '/weekly-bees',
            UpdateLatestEpisode: '/latest-posts',
            JsonLatestPost: '/json-latest-posts',
            DeleteLatestEpisode: '/delete-latest-post',
            /* Blogs */
            JsonBlog: '/json-blogs',
            AddBlog: '/new-blog',
            UpdateBlog: '/edit-blog',
            UpdateBlogColumn: '/update-blog',
            DeleteBlog: '/delete-blog',
            MoveBlog: '/move-blog',
            CopyBlog: '/copy-blog',
            /* Slides */
            JsonSlide: '/json-slides',
            AddSlide: '/new-slide',
            UpdateSlide: '/edit-slide',
            UpdateSlideColumn: '/update-slide',
            DeleteSlide: '/delete-slide',
            CopySlide: '/copy-slide',
            /* Socials */
            JsonSocial: '/json-socials',
            AddSocial: '/new-social',
            UpdateSocial: '/edit-social',
            UpdateSocialColumn: '/update-social',
            DeleteSocial: '/delete-social',
            /* Newsletters */
            JsonNewsletter: '/json-newsletters',
            DeleteNewsletter: '/delete-newsletter',
            /* Contacts */
            JsonContact: '/json-contacts',
            AnswerContact: '/answer-contact',
            DeleteContact: '/delete-contact',
            /* Comments */
            Comment: '/comment',
            JsonComment: '/json-comments',
            JsonCommentAnswer: '/json-comment-answers',
            UpdateCommentColumn: '/update-comment',
            DeleteComment: '/delete-comment',
            VoteComment: '/vote-comment',
            /* Members */
            JsonMember: '/json-members',
            AddMember: '/new-member',
            UpdateMember: '/edit-member',
            UpdateMemberColumn: '/update-member',
            DeleteMember: '/delete-member',
            /* Others */
            Newsletter: '/newsletter',
        },
        GetPointer: function (name) {
            return $pointer.List[name];
        }
    }
    w.$pointer = new pointer();
}(window));