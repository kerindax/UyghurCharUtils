-- 基本区   转换   扩展区
CREATE DEFINER=`root`@`localhost` FUNCTION `Basic2Extend`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END;
-- 基本区  转换   反向扩展区
CREATE DEFINER=`root`@`localhost` FUNCTION `Basic2RExtend`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END;
-- 扩展区   转换   基本区
CREATE DEFINER=`root`@`localhost` FUNCTION `Extend2Basic`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END;
-- 反向扩展区   转换   基本区
CREATE DEFINER=`root`@`localhost` FUNCTION `RExtend2Basic`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END;

CREATE DEFINER=`root`@`localhost` FUNCTION `_ReverseAscii`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END;

CREATE DEFINER=`root`@`localhost` FUNCTION `_ReverseString`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END;

CREATE DEFINER=`root`@`localhost` FUNCTION `_ExtendLa`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END;

CREATE DEFINER=`root`@`localhost` FUNCTION `_BasicLa`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END;

CREATE DEFINER=`root`@`localhost` FUNCTION `_GetCode`(`source` CHAR(1) CHARSET utf8, _index int(11)) RETURNS int(11)
BEGIN
DROP TEMPORARY TABLE IF EXISTS `U`;
CREATE TEMPORARY TABLE `U` (`0` int,`1` int,`2` int,`3` int,`4` int,`5` int);
INSERT INTO `U` VALUES(0x626, 0xFE8B, 0xFE8B, 0xFE8C, 0xFE8C, 1), (0x627, 0xFE8D, 0xFE8D, 0xFE8E, 0xFE8E, 0), (0x6D5, 0xFEE9, 0xFEE9, 0xFEEA, 0xFEEA, 0), (0x628, 0xFE8F, 0xFE91, 0xFE92, 0xFE90, 1), (0x67E, 0xFB56, 0xFB58, 0xFB59, 0xFB57, 1), (0x62A, 0xFE95, 0xFE97, 0xFE98, 0xFE96, 1), (0x62C, 0xFE9D, 0xFE9F, 0xFEA0, 0xFE9E, 1), (0x686, 0xFB7A, 0xFB7C, 0xFB7D, 0xFB7B, 1), (0x62E, 0xFEA5, 0xFEA7, 0xFEA8, 0xFEA6, 1), (0x62F, 0xFEA9, 0xFEA9, 0xFEAA, 0xFEAA, 0), (0x631, 0xFEAD, 0xFEAD, 0xFEAE, 0xFEAE, 0), (0x632, 0xFEAF, 0xFEAF, 0xFEB0, 0xFEB0, 0), (0x698, 0xFB8A, 0xFB8A, 0xFB8B, 0xFB8B, 0), (0x633, 0xFEB1, 0xFEB3, 0xFEB4, 0xFEB2, 1), (0x634, 0xFEB5, 0xFEB7, 0xFEB8, 0xFEB6, 1), (0x63A, 0xFECD, 0xFECF, 0xFED0, 0xFECE, 1), (0x641, 0xFED1, 0xFED3, 0xFED4, 0xFED2, 1), (0x642, 0xFED5, 0xFED7, 0xFED8, 0xFED6, 1), (0x643, 0xFED9, 0xFEDB, 0xFEDC, 0xFEDA, 1), (0x6AF, 0xFB92, 0xFB94, 0xFB95, 0xFB93, 1), (0x6AD, 0xFBD3, 0xFBD5, 0xFBD6, 0xFBD4, 1), (0x644, 0xFEDD, 0xFEDF, 0xFEE0, 0xFEDE, 1), (0x645, 0xFEE1, 0xFEE3, 0xFEE4, 0xFEE2, 1), (0x646, 0xFEE5, 0xFEE7, 0xFEE8, 0xFEE6, 1), (0x6BE, 0xFBAA, 0xFBAC, 0xFBAD, 0xFBAB, 1), (0x648, 0xFEED, 0xFEED, 0xFEEE, 0xFEEE, 0), (0x6C7, 0xFBD7, 0xFBD7, 0xFBD8, 0xFBD8, 0), (0x6C6, 0xFBD9, 0xFBD9, 0xFBDA, 0xFBDA, 0), (0x6C8, 0xFBDB, 0xFBDB, 0xFBDC, 0xFBDC, 0), (0x6CB, 0xFBDE, 0xFBDE, 0xFBDF, 0xFBDF, 0), (0x6D0, 0xFBE4, 0xFBE6, 0xFBE7, 0xFBE5, 1), (0x649, 0xFEEF, 0xFBE8, 0xFBE9, 0xFEF0, 1), (0x64A, 0xFEF1, 0xFEF3, 0xFEF4, 0xFEF2, 1);

	RETURN (SELECT 
	CASE _index 
	WHEN 1 THEN `1` 
	WHEN 2 THEN `2` 
	WHEN 3 THEN `3` 
	WHEN 4 THEN `4` 
	ELSE `0` END
FROM U WHERE `0`=_AscW(source));
END;

CREATE DEFINER=`root`@`localhost` FUNCTION `_ChrW`(`number` INT) RETURNS char(1) CHARSET utf8
BEGIN
	RETURN CONVERT( unhex(hex(number)) USING ucs2);
END;

CREATE DEFINER=`root`@`localhost` FUNCTION `_AscW`(`source` CHAR(1) CHARSET utf8) RETURNS int(11)
BEGIN
	RETURN ORD(CONVERT( source USING ucs2 ));
END;