CKEDITOR.dialog.add('uploadDialog', function (editor) {
    return {
        title: '上传文件',
        minWidth: 600,
        minHeight: 400,
        contents: [
            {
                id: 'tab-basic',
                label: 'Basic Settings',
                elements: [
                    {
                        type: 'html',
                        html: '<iframe src="/admin/upload.aspx?t='+ Math.random() + '" style="width:100%;height:380px;"></iframe>',
                        id: 'upload',
                        label: 'Abbreviation',
                        validate: CKEDITOR.dialog.validate.notEmpty("Abbreviation field cannot be empty")
                    }
                ]
            }
        ],
        onOk: function () {
            var dialog = this;
            
            var content = dialog.getContentElement("tab-basic", "upload");
            var el = content.getElement();
            var iframeId = el.$.getAttribute("id");
            var frameDom = jQuery("#" + iframeId).contents()[0];
            //jQuery(frameDom.getElementById("form1")).html()
            

            

            //abbr.setAttribute('title', dialog.getValueOf('tab-basic', 'title'));
            //abbr.setText(dialog.getValueOf('tab-basic', 'upload'));
            
            //if (id)
            //    abbr.setAttribute('id', id);
            //debugger;
            if (window.frames[iframeId].document.__getUploadFileUrls) {
                var urls = window.frames[iframeId].document.__getUploadFileUrls();
                for (var i = 0; i < urls.length; i++) {
                    var img = editor.document.createElement('img');
                    img.setAttribute('src',urls[i]);
                    editor.insertElement(img);
                }
                
            }

            
        }
    };
});