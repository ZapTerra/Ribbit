mergeInto(LibraryManager.library, {
 
 
  PasteHereWindow: function (sometext) {
    var pastedtext= prompt("Enter 7 digit verification code:", "");
    if (pastedtext!=null) {
    	SendMessage("uimesh", "GetPastedText", pastedtext);
	}
  },
 
});