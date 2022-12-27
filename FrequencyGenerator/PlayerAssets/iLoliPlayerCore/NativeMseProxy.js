/** @ts-check */
class NativeMediaSource {
    // sourceBuffers 未实现

    // activeSourceBuffers 未实现

    /** @type {ReadyState} */
    get readyState() {
        return this.mediaSource.readyState;
    }

    constructor() {
        console.log("NativeMediaSource constructor");
        this.mediaSource = JSPlayerBridge.createMediaSource();
        // addSourceBuffer;
        // removeSourceBuffer;
        this.clearLiveSeekableRange = this.mediaSource.clearLiveSeekableRange;
        this.setLiveSeekableRange = this.mediaSource.setLiveSeekableRange;
        this.endOfStream = this.mediaSource.endOfStream;
        console.log("Created", this.mediaSource);
    }

    static isTypeSupported(type) {
        return JSPlayerBridge.mediaSourceIsTypeSupported(type);
    }

    /** 
     * @param {string} type
     * @returns {NativeSourceBuffer}
     */
    addSourceBuffer(type) {
        return new NativeSourceBuffer(this.mediaSource, type);
    }

    /** 
     * @param {NativeSourceBuffer} sourceBuffer
     */
    removeSourceBuffer(sourceBuffer) {
        let nativeSourceBuffer = sourceBuffer.getNativeSourceBuffer();
        this.mediaSource.removeSourceBuffer(nativeSourceBuffer);
    }
}

class NativeSourceBuffer {
    /** @type {number | null} */
    get appendWindowEnd() {
        return this.sourceBuffer.appendWindowEnd;
    }
    set appendWindowEnd(value) {
        this.sourceBuffer.appendWindowEnd = value;
    }

    /** @type {number} */
    get appendWindowStart() {
        return this.sourceBuffer.appendWindowStart;
    }
    set appendWindowStart(value) {
        this.sourceBuffer.appendWindowStart = value;
    }

    /** @type {TimeRanges} */
    get buffered() {
        // TODO: 封装成 TimeRanges
        console.log(this.sourceBuffer.buffered);
    }

    /** @type {AppendMode} */
    get mode() {
        return this.sourceBuffer.mode;
    }
    set mode(value) {
        this.sourceBuffer.mode = value;
    }

    /** @type {number} */
    get timestampOffset() {
        return this.sourceBuffer.timestampOffset;
    }
    set timestampOffset(value) {
        this.sourceBuffer.timestampOffset = value;
    }

    /** @type {boolean} */
    get updating() {
        return this.sourceBuffer.updating;
    }

    constructor(mediaSource, type) {
        this.mediaSource = mediaSource;
        this.sourceBuffer = this.mediaSource.addSourceBuffer(type);
        this.abort = this.sourceBuffer.abort;
        // appendBuffer
        // changeType 未实现
        this.remove = this.sourceBuffer.remove;
    }

    // 返回上下文
    getNativeMediaSource() {
        return this.mediaSource;
    }

    // 返回原始对象
    getNativeSourceBuffer() {
        return this.sourceBuffer;
    }

    /**
     * @param {BufferSource} data 
     */
    appendBuffer(data) {
        let dataBytes = new Uint8Array(data);
        this.sourceBuffer.appendBuffer(dataBytes);
    }
}