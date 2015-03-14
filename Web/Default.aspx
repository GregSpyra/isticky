<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" ValidateRequest="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Editor with Mail Merge</title>
    <script src="scripts/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="scripts/ckeditor/adapters/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/jquery-1.3.2.min.js"></script>
    <script src="scripts/myAjax.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        //select the merge fields dropdown and implement the onchange event 
        //to insert the selected value in the text area
        $(document).ready(function() {
            $("#MergeFields").val('0');
            $("#MergeFields").change(onSelectChange);
        });
        
        function onSelectChange() {
            var selected = $("#MergeFields option:selected");
            var oEditor = CKEDITOR.instances.editor1;

            if (selected.val() != 0) {
                var valueToInsert = selected.text();
                // Check the active editing mode.
                if (oEditor.mode == 'wysiwyg') {
                    // Insert the desired HTML.
                    oEditor.insertHtml(valueToInsert);
                }
                else {
                    alert('You must be on WYSIWYG mode!');
                }
            }
            $("#MergeFields").val('0');
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>
		    Online Richtext editor with Mail Merge
	    </h2>
	    <!-- This <div> holds alert messages to be display in the sample page. -->
	    <div id="alerts">
		    <noscript>
			    <p>
				    <strong>Online Richtext editor requires JavaScript to run</strong>. 
				    In a browser with no JavaScript support, like yours, you should still see 
				    the contents (HTML data) and you should be able to edit it normally, 
				    without a rich editor interface.
			    </p>
		    </noscript>
	    </div>
        <p>
            This is online rich text editor sample with mail merge.<br />
            Use the text area below to type in your document. <br />
            Use the dropdowns below that have mail merge fields. 
        </p>
	    <textarea cols="50" id="editor1" name="editor1" rows="10"></textarea>
        <!-- instantiate a new instance of CKEDITOR -->
		<script type="text/javascript">
		    //<![CDATA[
		    // Replace the <textarea id="editor1"> with an CKEditor instance.
		    var editor = CKEDITOR.replace('editor1',
            {
                toolbar: 'myToolBar', skin: 'office2003', width: '60%'
            });
		    //]]>
		</script>
    	
    	<div id="eMessage">
		</div>
		<div id="eButtons">
		    <span><b>Insert Merge Fields from here</b></span>
		    <select id="MergeFields" >
		        <option value="0" selected="selected">Select Merge Fields</option>
		        <option value="1">{^Title^}</option>
		        <option value="2">{^FirstName^}</option>
		        <option value="3">{^LastName^}</option>
		    </select>
		    
		    <input type="button" name="saveAsWord" id="saveAsWord" 
		         onclick="javascript:ajaxDownloadDoc()" value="Download as word" />
		 </div>

    </div>
    </form>
</body>
</html>
