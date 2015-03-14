
function ajaxDownloadDoc() {
	//get the form
	var f = $("#aspnetForm");
	//get the serialized data
	var serializedForm = f.serialize();
	var url = "downloader.ashx?what=saveAsWord";
	var oEditor = CKEDITOR.instances.editor1;
	var dataPost = editor.getData();

	$.ajax({
		type: "POST",
		url: url,
		data: { "richtext": dataPost },
		beforeSend: AjaxSaveStarted,
		success: AjaxSucceeded,
		error: AjaxFailed
	});
	return false;
}

function AjaxSucceeded(result) {
	var arr = result.toString().split('docx');
	var url = arr[0] + "docx";
	var newWindow = window.open(url, '', 'scrollbars=no,menubar=no,height=1,width=1,resizable=no,toolbar=no,location=no,status=no');
	//newWindow.close();
	//alert(result.toString());

}

function AjaxFailed(result) {
	//alert(result.status + ' ' + result.statusText);
	var errorMessage = 'Internal server error';
	try {//Error handling for POST calls
		var err = JSON.parse(result.responseText);
		errorMessage = err.Message;
	}
	catch (ex) {//Error handling for GET calls
		$('#divMaster').append('<div id="eMessage" class="DisplayNone">' + result.responseText + '</div>');
		errorMessage = 'Page load error: ';
		errorMessage += $('#divErrorResponse').find('h2 i').html();
	}
	alert(errorMessage);
}

function AjaxSaveStarted(result) {
	// alert(result.status + ' ' + result.statusText);
}
