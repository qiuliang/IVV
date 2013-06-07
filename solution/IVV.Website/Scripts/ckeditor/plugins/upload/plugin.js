CKEDITOR.plugins.add('upload', {
    icons: 'upload',
    init: function (editor) {
        editor.addCommand('uploadDialog', new CKEDITOR.dialogCommand('uploadDialog'));
        editor.ui.addButton('Upload', {
            label: '上传文件',
            command: 'uploadDialog',
            toolbar: 'insert,1'
        });

        CKEDITOR.dialog.add('uploadDialog', this.path + 'dialogs/upload.js');
    }
});