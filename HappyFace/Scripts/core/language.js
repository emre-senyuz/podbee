(function (w) {
    var language = function () { };
    language.constructor = language;
    language.prototype = {
        List: {
            title: {
                tr: 'Başlık',
                en: 'Title'
            },
            code: {
                tr: 'Kod',
                en: 'Code'
            },
            direction: {
                tr: 'Yön',
                en: 'Direction'
            },
            icon: {
                tr: 'İkon',
                en: 'Icon'
            },
            default: {
                tr: 'Ön Tanımlı',
                en: 'Default'
            },
            status: {
                tr: 'Durum',
                en: 'Status'
            },
            actions: {
                tr: 'İşlemler',
                en: 'Actions'
            },
            toolbarInfo: {
                tr: '{{total}} kayıttan {{start}} - {{end}} arası gösteriliyor',
                en: 'Displaying {{start}} - {{end}} of {{total}} records'
            },
            first: {
                tr: 'İlk',
                en: 'First'
            },
            prev: {
                tr: 'Önceki',
                en: 'Prev'
            },
            next: {
                tr: 'Sonraki',
                en: 'Next'
            },
            last: {
                tr: 'Son',
                en: 'Last'
            },
            morePages: {
                tr: 'Daha fazla sayfa',
                en: 'More pages'
            },
            inputNumber: {
                tr: 'Sayfa numarası',
                en: 'Page number'
            },
            selectPageSize: {
                tr: 'Sayfa numarası seç',
                en: 'Select page size'
            },
            noRecords: {
                tr: 'Gösterilecek kayıt bulunamadı',
                en: 'No records found'
            },
            processing: {
                tr: 'Lütfen bekleyin...',
                en: 'Please wait...'
            },
            attention: {
                tr: 'Dikkat',
                en: 'Attention'
            },
            confirm: {
                tr: 'Onay',
                en: 'Confirm'
            },
            move: {
                tr: 'Taşı',
                en: 'Move'
            },
            close: {
                tr: 'Kapat',
                en: 'Close'
            },
            delete: {
                tr: 'Sil',
                en: 'Delete'
            },
            surefordelete: {
                tr: 'Kaydı silmek istediğinize emin misiniz?',
                en: 'Are you sure you want to delete the record?'
            },
            surefordeletewithchild: {
                tr: 'Kaydı, bağlı alt kategori ve içerikler ile birlikte silmek istediğinize emin misiniz?',
                en: 'Are you sure you want to delete the record, along with its sub-categories and contents?'
            },
            description: {
                tr: 'Açıklama',
                en: 'Description'
            },
            keywords: {
                tr: 'Anahtar Kelimeler',
                en: 'Keywords'
            },
            type: {
                tr: 'Tür',
                en: 'Type'
            },
            language: {
                tr: 'Dil',
                en: 'Language'
            },
            showmenu: {
                tr: 'Menü Aktif',
                en: 'Show Menu'
            },
            Product: {
                tr: 'Ürün',
                en: 'Product'
            },
            Page: {
                tr: 'Sayfa',
                en: 'Page'
            },
            News: {
                tr: 'Haber',
                en: 'News'
            },
            Podcast: {
                tr: 'Podcast',
                en: 'Podcast'
            },
            Blog: {
                tr: 'Blog',
                en: 'Blog'
            },
            showsubcategory: {
                tr: 'Alt Kategorileri Göster',
                en: 'Show Sub Categories'
            },
            selectparent: {
                tr: 'Üst Kategori Seçiniz',
                en: 'Select Parent'
            },
            category: {
                tr: 'Kategori',
                en: 'Category'
            },
            maincategory: {
                tr: 'Ana Kategori',
                en: 'Main Category'
            },
            erroremptyfield: {
                tr: 'Bu alan boş bırakılamaz',
                en: 'This field is required'
            },
            nosubcategory: {
                tr: 'Alt kategori bulunmamaktadır',
                en: 'No subcategories'
            },
            addsubcategory: {
                tr: 'Alt kategori ekle',
                en: 'Add subcategory'
            },
            deletesubcategory: {
                tr: 'Alt kategorileri sil',
                en: 'Delete subcategories'
            },
            move: {
                tr: 'Taşı',
                en: 'Move'
            },
            copy: {
                tr: 'Kopyala',
                en: 'Copy'
            },
            deleteselected: {
                tr: 'Seçileni Sil',
                en: 'Delete Selected'
            },
            deleteall: {
                tr: 'Tümünü Sil',
                en: 'Delete All'
            },
            platform: {
                tr: 'Platform',
                en: 'Platform'
            },
            url: {
                tr: 'Url',
                en: 'Url'
            },
            icon: {
                tr: 'İkon',
                en: 'Icon'
            },
            email: {
                tr: 'E-posta',
                en: 'E-mail'
            },
            activation: {
                tr: "Aktivaston",
                en: "Activation"
            },
            edit: {
                tr: 'Düzenle',
                en: 'Edit'
            },
            name: {
                tr: "Ad",
                en: "Name"
            },
            surname: {
                tr: "Soyad",
                en: "Surname"
            },
            subject: {
                tr: "Konu",
                en: "Subject"
            },
            message: {
                tr: "Mesaj",
                en: "Message"
            },
            isreaded: {
                tr: "Okundu",
                en: "Readed"
            },
            isconfirmed: {
                tr: "Onay Durumu",
                en: "Confirmation Status"
            },
            createddate: {
                tr: "Oluşturma Tarihi",
                en: "Created Date"
            },
            comment: {
                tr: "Yorum",
                en: "Comment"
            },
            order: {
                tr: "Sıralama",
                en: "Order"
            }
        },
        get: function (name) {
            return $language.List[name][lang];
        }
    }
    w.$language = new language();
}(window));