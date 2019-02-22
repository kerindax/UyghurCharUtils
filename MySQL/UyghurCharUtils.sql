--基本区   转换   扩展区
CREATE DEFINER=`root`@`localhost` FUNCTION `Basic2Extend`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END
--基本区  转换   反向扩展区
CREATE DEFINER=`root`@`localhost` FUNCTION `Basic2RExtend`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END
--扩展区   转换   基本区
CREATE DEFINER=`root`@`localhost` FUNCTION `Extend2Basic`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END
--反向扩展区   转换   基本区
CREATE DEFINER=`root`@`localhost` FUNCTION `RExtend2Basic`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END
CREATE DEFINER=`root`@`localhost` FUNCTION `_ReverseAscii`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END
CREATE DEFINER=`root`@`localhost` FUNCTION `_ReverseString`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END
CREATE DEFINER=`root`@`localhost` FUNCTION `_ExtendLa`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END
CREATE DEFINER=`root`@`localhost` FUNCTION `_BasicLa`(`source` text) RETURNS text CHARSET utf8
BEGIN
	#Routine body goes here...

	RETURN '';
END
CREATE DEFINER=`root`@`localhost` FUNCTION `_GetCode`(`source` CHAR(1), _index int(11)) RETURNS int(11)
BEGIN
	#Routine body goes here...

	RETURN 0;
END
CREATE DEFINER=`root`@`localhost` FUNCTION `_ChrW`(`number` INT) RETURNS char(1) CHARSET utf8
BEGIN
	RETURN CONVERT( unhex(hex(number)) USING utf8);
END
CREATE DEFINER=`root`@`localhost` FUNCTION `_AscW`(`source` CHAR(1)) RETURNS int(11)
BEGIN
	RETURN ORD(CONVERT( source USING utf8 ));
END