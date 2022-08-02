import CryptoJS from 'crypto-js'

//随机生成指定数量key
export const generatekey = (num) => {
    let library = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    let key = "";
    for (var i = 0; i < num; i++) {
        let randomPoz = Math.floor(Math.random() * library.length);
        key += library.substring(randomPoz, randomPoz + 1);
    }
    return key;
}
//加密(16进制)
export const encrypt = (word, keyStr) => {
    keyStr = keyStr ? keyStr : '23klsdso89**^^hh';
    var key = CryptoJS.enc.Utf8.parse(keyStr);//Latin1 w8m31+Yy/Nw6thPsMpO5fg==
    var srcs = CryptoJS.enc.Utf8.parse(word);
    var encrypted = CryptoJS.AES.encrypt(srcs, key, {
        iv: key,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });
    return encrypted.ciphertext.toString().toUpperCase();
    // CryptoJS.enc.Base64.stringify(encrypted.ciphertext)  (base64加密)
}
//解密（16进制）
export const decrypt = (word, keyStr) => {

    const wordArray = CryptoJS.enc.Hex.parse(word)
    const base64Word = CryptoJS.enc.Base64.stringify(wordArray)
    //base64解密省略上面过程

    keyStr = keyStr ? keyStr : '23klsdso89**^^hh';
    var key = CryptoJS.enc.Utf8.parse(keyStr);//Latin1 w8m31+Yy/Nw6thPsMpO5fg==
    var decrypt = CryptoJS.AES.decrypt(base64Word, key, {
        iv: key,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });
    return CryptoJS.enc.Utf8.stringify(decrypt).toString();
}
