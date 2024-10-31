mergeInto(LibraryManager.library, {
    GetProtocol: function() {
        // 返回当前页面的协议
        var returnStr = window.location.protocol;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    GetIP: function() {
        // 返回当前页面的 IP 地址
        var returnStr = window.location.hostname;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        console.log("returnStr ==",returnStr);
        return buffer;
    },
    GetPort: function() {
        var returnStr = window.location.port;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        // console.log("returnStr ==",returnStr);
        return buffer;
    },
    GetLocalStorage: function(key) {
        // console.log("localStorage GetLocalStorage ==", UTF8ToString(key));
        // 返回当前页面的 URL
        // console.log("GetPageLocation ==", a, b, window.location.href);
        let returnStr = window.localStorage.getItem(UTF8ToString(key));
        // console.log("localStorage returnStr ==",returnStr);
        let buffer; 
        let bufferSize;
        if(returnStr){
            bufferSize = lengthBytesUTF8(returnStr) + 1;
            buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            // console.log("returnStr ==",returnStr);
        }
        // console.log("localStorage buffer ==",buffer);
        return buffer;
    },
    UnitySendMessageToWeb: function(Message) {
        console.log("UnitySendMessageToWeb ==", UTF8ToString(Message));
        if(window.ReceiveMessageFromUnity){
            window.ReceiveMessageFromUnity(UTF8ToString(Message));
        }
    }
	// PostMsg: funtion(msg){
  	// 	var worker = new Worker("Your Worker JS Path");
	// 	worker.postMessage(UTF8ToString(msg));
	// 	worker.onmessage = function(e){
    //       // todo
    //     }
	// },
});

