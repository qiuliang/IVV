//文件上传组件，依赖于jquery和jquery ui
var uploader = function (obj) {
    this._param = obj;
    
    
};

uploader.prototype.init = function () {
    var _self = this;
    $("#" + _self._param.dialogId).append('<iframe id="uploadFrame" src="/admin/upload.aspx?t=' + Math.random() + '" style="width:100%;height:380px;"></iframe>');
    $("#" + _self._param.triggerId).click(function () {
        $("#" + _self._param.dialogId).dialog({
            width : 650,
            autoOpen : true,
            buttons: [
                {
                    text: "Ok",
                    click: function () {
                        var urls = window.frames["uploadFrame"].document.__getUploadFileUrls();
                        for (var i = 0; i < urls.length; i++) {
                            var imgstr = '<img src="' + urls[i] + '" style="width:150px;" />';
                            $("#" + _self._param.container).html("");
                            $("#" + _self._param.container).append($(imgstr));
                            $("#" + _self._param.textId).val(urls[i]);
                        }
                        $("#" + _self._param.dialogId).dialog("close");
                    }
                }
            ]
        });
    });
};