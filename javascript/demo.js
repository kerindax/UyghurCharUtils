    require("UyghurCharUtils.js");
    var utils  = new UyghurCharUtils();
    var source = "سالام JS";
    
    var target1 = utils.Basic2Extend(source);//基本区 转换 扩展区
    var target2 = utils.Extend2Basic(target1);//扩展区 转换 基本区

    var target3 = utils.Basic2RExtend(source);//基本区 转换 反向扩展区
    var target4 = utils.RExtend2Basic(target3);//反向扩展区 转换 基本区

    console.log(target1);
    console.log(target2);

    console.log(target3);
    console.log(target4);