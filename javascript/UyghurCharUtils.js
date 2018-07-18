const U = [[0x626, 0xFE8B, 0xFE8B, 0xFE8C, 0xFE8C, 1], [0x627, 0xFE8D, 0xFE8D, 0xFE8E, 0xFE8E, 0], [0x6D5, 0xFEE9, 0xFEE9, 0xFEEA, 0xFEEA, 0], [0x628, 0xFE8F, 0xFE91, 0xFE92, 0xFE90, 1], [0x67E, 0xFB56, 0xFB58, 0xFB59, 0xFB57, 1], [0x62A, 0xFE95, 0xFE97, 0xFE98, 0xFE96, 1], [0x62C, 0xFE9D, 0xFE9F, 0xFEA0, 0xFE9E, 1], [0x686, 0xFB7A, 0xFB7C, 0xFB7D, 0xFB7B, 1], [0x62E, 0xFEA5, 0xFEA7, 0xFEA8, 0xFEA6, 1], [0x62F, 0xFEA9, 0xFEA9, 0xFEAA, 0xFEAA, 0], [0x631, 0xFEAD, 0xFEAD, 0xFEAE, 0xFEAE, 0], [0x632, 0xFEAF, 0xFEAF, 0xFEB0, 0xFEB0, 0], [0x698, 0xFB8A, 0xFB8A, 0xFB8B, 0xFB8B, 0], [0x633, 0xFEB1, 0xFEB3, 0xFEB4, 0xFEB2, 1], [0x634, 0xFEB5, 0xFEB7, 0xFEB8, 0xFEB6, 1], [0x63A, 0xFECD, 0xFECF, 0xFED0, 0xFECE, 1], [0x641, 0xFED1, 0xFED3, 0xFED4, 0xFED2, 1], [0x642, 0xFED5, 0xFED7, 0xFED8, 0xFED6, 1], [0x643, 0xFED9, 0xFEDB, 0xFEDC, 0xFEDA, 1], [0x6AF, 0xFB92, 0xFB94, 0xFB95, 0xFB93, 1], [0x6AD, 0xFBD3, 0xFBD5, 0xFBD6, 0xFBD4, 1], [0x644, 0xFEDD, 0xFEDF, 0xFEE0, 0xFEDE, 1], [0x645, 0xFEE1, 0xFEE3, 0xFEE4, 0xFEE2, 1], [0x646, 0xFEE5, 0xFEE7, 0xFEE8, 0xFEE6, 1], [0x6BE, 0xFBAA, 0xFBAC, 0xFBAD, 0xFBAB, 1], [0x648, 0xFEED, 0xFEED, 0xFEEE, 0xFEEE, 0], [0x6C7, 0xFBD7, 0xFBD7, 0xFBD8, 0xFBD8, 0], [0x6C6, 0xFBD9, 0xFBD9, 0xFBDA, 0xFBDA, 0], [0x6C8, 0xFBDB, 0xFBDB, 0xFBDC, 0xFBDC, 0], [0x6CB, 0xFBDE, 0xFBDE, 0xFBDF, 0xFBDF, 0], [0x6D0, 0xFBE4, 0xFBE6, 0xFBE7, 0xFBE5, 1], [0x649, 0xFEEF, 0xFBE8, 0xFBE9, 0xFEF0, 1], [0x64A, 0xFEF1, 0xFEF3, 0xFEF4, 0xFEF2, 1]];
export default class UyghurCharUtils {
  constructor() {}
  /**基本区   转换   扩展区*/
  Basic2Extend(source){
    var ch;
    var i;
    var front;
    var frontfront;
    var target = "";
    if (source.length == 1)
      target = this._GetChar(source, 4);
    else {
      source = " " + source + " ";
      for (i = 2; i <= source.length; i++) {
        ch = source.substr(i - 1, 1);
        front = source.substr(i - 2, 1);
        if (this._Normal(ch) == 2) {
          if (this._Normal(front) == 0 | this._Normal(front) == 1)
            target = target + (this._GetChar(ch, 1));
          else
            target += (this._GetChar(ch, 2));
        } 
        if (this._Normal(ch) == 1) {
          if (this._Normal(front) == 0 | this._Normal(front) == 1)
            target += (this._GetChar(ch, 1));
          else
            target += (this._GetChar(ch, 3));
        } 
        if (this._Normal(ch) == 0) {
          if (target.length >= 1)
            target = target.substr(0, target.length - 1); 

          if (this._Normal(front) == 2) {
            frontfront = source.substr(i - 3, 1);
            if (this._Normal(frontfront) == 1)
              target += (this._GetChar(front, 4));
            else if (this._Normal(frontfront) == 0)
              target += (this._GetChar(front, 4));
            else
              target += (this._GetChar(front, 3));
          }

          if (this._Normal(front) == 1) {
            frontfront = source.substr(i - 3, 1);
            if (this._Normal(frontfront) == 1)
              target += (this._GetChar(front, 4));
            else if (this._Normal(frontfront) == 0)
              target += (this._GetChar(front, 1));
            else
              target += (this._GetChar(front, 3));
          }

          if (this._Normal(front) == 0)
            target += (this._GetChar(front, 4));
          target += (this._GetChar(ch, 0));
        }
      }
    }
    return this._ExtendLa(target.trim());
  }
  /**基本区  转换   反向扩展区*/
  Basic2RExtend(source){
    var ThisText = this.Basic2Extend(source);
    var ReverseString = this._ReverseString(ThisText);
    return this._ReverseAscii(ReverseString);
  }
  /**扩展区   转换   基本区 */
  Extend2Basic(source) {
    var i, j, ch;
    var target = '';
    source = this._BasicLa(source);
    for (i = 0; i < source.length; i++) {
      ch = source.substr(i , 1);
      target += this._GetChar(ch, 0);
    }
    return target;
  }
  /**反向扩展区   转换   基本区 */
  RExtend2Basic(source) {
    var target = this._ReverseAscii(source);
    target = this._ReverseString(target);
    target = this.Extend2Basic(target);
    return target;
  }
  GetSyllable(Str) {
    Str = Str.Trim();
    var List = [];
    var Sozuk = "اەوۇۆۈېىئ";
    var ch;
    var Str2 = Str;
    var Check = false;
    for (var i = Str.Length - 1; i >= 0; i += -1) {
      ch = Str.Substring(i, 1);

      if (ch == "ئ") {
        List.Insert(0, Str2.Substring(i));
        Str2 = Str.Substring(0, i);
        continue;
      }
      if (Sozuk.Contains(Str.Substring(i, 1)) && i > 0 && !Sozuk.Contains(Str.Substring(i - 1, 1))) {
        Check = false;
        foreach(Item in Str.Substring(0, i - 1))
        {
          if (Sozuk.Contains(Item)) {
            Check = true;
            break;
          }
        }
        if (Check) {
          List.Insert(0, Str2.Substring(i - 1));
          Str2 = Str.Substring(0, i - 1);
          i -= 1;
        }
        else {
          List.Insert(0, Str2);
          break;
        }
      }
      if (i == 0 && Str2.Trim() != "")
        List.Insert(0, Str2);
    }
    return List;
  }

  _ReverseAscii(source){
    var that = this;
    return source.replace(/([^\uFB00-\uFEFF\u0600-\u06FF\s]+)/g, function (word) {
      return that._ReverseString(word);
    });
  }
  _ReverseString(str) {// 反转
    var newstr = str.split("").reverse().join("");
    return newstr;
  }
  _ExtendLa(source) {
    return source.replace(/(\uFEDF\uFE8E)/g, function (word) {
      return String.fromCharCode(0xFEFB);
    }).replace(/(\uFEE0\uFE8E)/g, function (word) {
      return String.fromCharCode(0xFEFC);
    });
  }
  _BasicLa(source) {
    return source.replace(/(\uFEFB)/g, function (word) {
      return String.fromCharCode(0xFEDF) + String.fromCharCode(0xFE8E);
    }).replace(/(\uFEFC)/g, function (word) {
      return String.fromCharCode(0xFEE0) + String.fromCharCode(0xFE8E);
    });
  }
  _Normal(source){
    var i;
    var target = 0;
    for (i = 0; i <= 32; i++) {
      if (source.charCodeAt() == U[i][0]) {
        if (U[i][5] == 1)
          target = 2;
        else
          target = 1;
      }
    }
    return target;
  }
  _GetChar(source, index){
    var i;
    var target = 0;
    var found = false;
    for (i = 0; i <= 32; i++) {
      var Code = source.charCodeAt();
      if (U[i].indexOf(Code)!=-1){
      // if (Code == U[i][0]) {
        found = true;
        switch (index) {
          case 0 : { target = U[i][0]; break; }// asasiy
          case 1 : { target = U[i][2]; break; }// bax
          case 2 : { target = U[i][3]; break; }// ottura
          case 3 : { target = U[i][4]; break; } // ahir
          case 4 : { target = U[i][1]; break; }// yalghuz
        }
      }
    }
    if (found == true)
      return String.fromCharCode(target);
    else
      return source;
  }
}

