<?php

	header("Content-type: text/html; charset=utf-8");
	require_once "UyghurCharUtils.php";
    $utils = new UyghurCharUtils();
    $source = "سالام PHP";
	
    $target1 = $utils->Basic2Extend($source);//基本区 转换 扩展区
    $target2 = $utils->Extend2Basic($target1);//扩展区 转换 基本区
	
    $target3 = $utils->Basic2RExtend($source);//基本区 转换 反向扩展区
    $target4 = $utils->RExtend2Basic($target3);//反向扩展区 转换 基本区
	
	echo $target1."<br/>";
	echo $target2."<br/>";

	echo $target3."<br/>";
	echo $target4."<br/>";
?>