CKEDITOR.dialog.add('uploadDialog', function (editor) {
    return {
        title: '上传文件',
        minWidth: 400,
        minHeight: 200,
        contents: [
            {
                id: 'tab-basic',
                label: 'Basic Settings',
                elements: [
                    {
                        type: 'html',
                        html: '<iframe src="/admin/upload.aspx" style="width:100%;"></iframe>',
                        id: 'upload',
                        label: 'Abbreviation',
                        validate: CKEDITOR.dialog.validate.notEmpty("Abbreviation field cannot be empty")
                    }
                ]
            }
        ],
        onOk: function () {
            var dialog = this;
            
            var aa = dialog.getContentElement("tab-basic", "upload");
            var ff = aa.getElement();
            var iframeId = ff.$.getAttribute("id");
            var ifr = jQuery("#" + iframeId).contents()[0];
            alert(jQuery(ifr.getElementById("form1")).html());
            var abbr = editor.document.createElement('p');
            //abbr.setAttribute('title', dialog.getValueOf('tab-basic', 'title'));
            //abbr.setText(dialog.getValueOf('tab-basic', 'upload'));
            
            var id = dialog.getValueOf('tab-basic', 'upload'); alert(id);
            //if (id)
            //    abbr.setAttribute('id', id);
            editor.insertElement(abbr);
        }
    };
});