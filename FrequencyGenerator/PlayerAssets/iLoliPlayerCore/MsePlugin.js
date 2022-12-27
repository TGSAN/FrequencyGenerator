/** @ts-check */
// let _addSourceBuffer = MediaSource.prototype.addSourceBuffer;
// MediaSource.prototype.addSourceBuffer = function (mime) {
//     console.log("MediaSource addSourceBuffer Type: ", mime);
//     var sourceBuffer = _addSourceBuffer.call(this, mime);
//     try {
//         JSPlayerBridge.MediaSourceAddSourceBuffer(mime);
//     } catch(err) {
//         console.error(err);
//     }
//     var _append = sourceBuffer.appendBuffer;
//     var isVideo = (mime.startsWith("audio") ? false : true);
//     var type = (isVideo ? "video" : "audio");
//     var key = type + "_" + performance.now().toString();
//     var startDate = new Date();
//     dumpEndTasks.push(() => {
//         endToSave = true;
//         console.warn(`轨道： ${mime} 已结束保存。`);
//         SavedDataList[key] = sourceBufferData;
//         let downloadCaption = `下载${(isVideo ? "视频" : "音频")}数据 (${BytesToSize(sourceBufferData.length)}, at ${dateFormat(startDate, "yyyy/MM/dd hh:mm:ss")})`;
//         GM_registerMenuCommand(downloadCaption, () => { DownloadDataCmd(key, type, startDate); });
//     });
//     sourceBuffer.appendBuffer = function (buffer) {
        
//         _append.call(this, buffer);
//     }
//     return sourceBuffer;
// }

MediaSource = NativeMediaSource;