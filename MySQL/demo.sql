SET @source = 'سالام MySQL';
SET @target1 = Basic2Extend(@source); #基本区 转换 扩展区
SET @target2 = Extend2Basic(@target1); #扩展区 转换 基本区

SET @target3 = Basic2RExtend(@source); #基本区 转换 反向扩展区
SET @target4 = RExtend2Basic(@target3); #反向扩展区 转换 基本区
SELECT @target1,@target2,@target3,@target4;