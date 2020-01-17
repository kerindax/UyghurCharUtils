	--  +----------------------------------------------------------------------
	--  | Update: 2020-01-17 14:31
	--  +----------------------------------------------------------------------
	--  | Author: Kerindax,Sherer <1482152356@qq.com>
	--  +----------------------------------------------------------------------

	#开通函数功能
	SET GLOBAL log_bin_trust_function_creators=1;

	# 基本区   转换   扩展区
	DROP FUNCTION IF EXISTS `Basic2Extend`;
	CREATE FUNCTION `Basic2Extend`(`source` text CHARSET utf8) RETURNS text CHARSET utf8
	BEGIN
		CREATE TEMPORARY TABLE IF NOT EXISTS `table` (`str` text CHARSET utf8, uni INT);
		DELETE FROM `table`;
		CALL _Participle(`source`, 0x0626, 0x06d5);
		UPDATE `table` SET str = _ReplaceWord(str) WHERE uni = 1;
		RETURN (SELECT GROUP_CONCAT(str SEPARATOR '') FROM `table`);
	END;

	# 基本区  转换   反向扩展区
	DROP FUNCTION IF EXISTS `Basic2RExtend`;
	CREATE FUNCTION `Basic2RExtend`(`source` text CHARSET utf8) RETURNS text CHARSET utf8
	BEGIN
		SET @_ThisText = Basic2Extend(`source`);
		SET @_ReverseString = _ReverseString(@_ThisText);
		RETURN _ReverseAscii(@_ReverseString);
	END;

	# 扩展区   转换   基本区
	DROP FUNCTION IF EXISTS `Extend2Basic`;
	CREATE FUNCTION `Extend2Basic`(`source` text CHARSET utf8) RETURNS text CHARSET utf8
	BEGIN
		SET @_source = _BasicLa(`source`);
		SET @_len = CHAR_LENGTH(@_source);
		SET @_i = 1;
		SET @_target = '';
		while @_i <= @_len do
			SET @_ch = SUBSTR(@_source,@_i,1);
			SET @_target = CONCAT(@_target, _ChrW(_GetCode(@_ch, 0)));
			SET @_i = @_i + 1; 
		end while;
		RETURN @_target;
	END;

	# 反向扩展区   转换   基本区
	DROP FUNCTION IF EXISTS `RExtend2Basic`;
	CREATE FUNCTION `RExtend2Basic`(`source` text CHARSET utf8) RETURNS text CHARSET utf8
	BEGIN
		SET @_target = _ReverseAscii(`source`);
		SET @_target = _ReverseString(@_target);
		SET @_target = Extend2Basic(@_target);
		RETURN @_target;
	END;

	DROP PROCEDURE IF EXISTS `_Participle`;
	CREATE PROCEDURE `_Participle`(`source` text CHARSET utf8,number1 INT,number2 INT)
	BEGIN
	SET @_len = CHAR_LENGTH(`source`);
	SET @_i = 1;
	SET @_uni = 0;
	SET @_str = '';
	WHILE @_i <= @_len DO
		SET @_ch = SUBSTRING(`source`,@_i,1);
		SET @_code =_AscW(@_ch);
		SET @_regi = (number1 <= @_code AND @_code <= number2);
		IF @_regi <> @_uni THEN
			INSERT INTO `table` VALUES(@_str,@_uni);
			SET @_uni = @_regi;
			SET @_str = @_ch;
		ELSE
			SET @_str = CONCAT(@_str,@_ch);
			SET @_uni = @_regi;
		END IF;
		SET @_i = @_i + 1; 
	END WHILE;
	INSERT INTO `table` VALUES(@_str,@_uni);
	END;

	DROP FUNCTION IF EXISTS `_ReplaceWord`;
	CREATE FUNCTION `_ReplaceWord`(`source` text CHARSET utf8) RETURNS text CHARSET utf8
	BEGIN
		SET @_str = `source`;
		SET @_returns = '';
		SET @_target = '';
		SET @_target2 = '';
		SET @_ch = 0;
		SET @_p = 0;
		SET @_length = CHAR_LENGTH(@_str);
		IF(@_length>1) THEN
			SET @_target = SUBSTRING(@_str,1,1);
			SET @_ch = _GetCode(@_target,2);
			SET @_returns = CONCAT(@_returns,_ChrW(@_ch));
			SET @_i = 0;
			WHILE @_i<=@_length-3 DO
				SET @_target = SUBSTRING(@_str,@_i+1,1);
				SET @_target2 = SUBSTRING(@_str,@_i+2,1);
				SET @_p = _GetCode(@_target,5);
				SET @_ch = _GetCode(@_target2,2+@_p);
				SET @_returns = CONCAT(@_returns,_ChrW(@_ch));
				SET @_i = @_i + 1;
			END WHILE;
			SET @_target = SUBSTRING(@_str,@_length-1,1);
			SET @_target2 = SUBSTRING(@_str,@_length,1);
			SET @_p = _GetCode(@_target,5) * 3;
			SET @_ch = _GetCode(@_target2,1+@_p);
			SET @_returns = CONCAT(@_returns,_ChrW(@_ch));
		ELSE
			SET @_ch = _GetCode(@_str,1);
			SET @_returns = CONCAT(@_returns,_ChrW(@_ch));
		END IF;
		RETURN _ExtendLa(TRIM(@_returns));
	END;

	DROP FUNCTION IF EXISTS `_ReverseAscii`;
	CREATE FUNCTION `_ReverseAscii`(`source` text CHARSET utf8) RETURNS text CHARSET utf8
	BEGIN
		CREATE TEMPORARY TABLE IF NOT EXISTS `table` (`str` text CHARSET utf8, uni INT);
		DELETE FROM `table`;
		CALL _Participle(`source`, 0xFB00, 0xFEFF);
		UPDATE `table` SET str = _ReverseString(str) WHERE uni = 0;
		RETURN (SELECT GROUP_CONCAT(str SEPARATOR '') FROM `table`);
	END;

	DROP FUNCTION IF EXISTS `_ReverseString`;
	CREATE FUNCTION `_ReverseString`(`source` text CHARSET utf8) RETURNS text CHARSET utf8
	BEGIN
		RETURN reverse(source);
	END;

	DROP FUNCTION IF EXISTS `_ExtendLa`;
	CREATE FUNCTION `_ExtendLa`(`source` text CHARSET utf8) RETURNS text CHARSET utf8
	BEGIN
		SET @_temp = REPLACE(`source`, CONCAT(_ChrW(0xFEDF), _ChrW(0xFE8E)),_ChrW(0xFEFB));
		SET @_temp = REPLACE(@_temp, CONCAT(_ChrW(0xFEE0), _ChrW(0xFE8E)), _ChrW(0xFEFC));
		RETURN @_temp;
	END;

	DROP FUNCTION IF EXISTS `_BasicLa`;
	CREATE FUNCTION `_BasicLa`(`source` text CHARSET utf8) RETURNS text CHARSET utf8
	BEGIN
		SET @_temp = REPLACE(`source`, _ChrW(0xFEFB), CONCAT(_ChrW(0x644), _ChrW(0x627)));
		SET @_temp = REPLACE(@_temp, _ChrW(0xFEFC), CONCAT(_ChrW(0x644), _ChrW(0x627)));
		RETURN @_temp;
	END;

	DROP FUNCTION IF EXISTS `_GetCode`;
	CREATE FUNCTION `_GetCode`(`source` VARCHAR(1) CHARSET utf8, _index int(11)) RETURNS int(11)
	BEGIN
		CREATE TEMPORARY TABLE IF NOT EXISTS `U` (`0` int,`1` int,`2` int,`3` int,`4` int,`5` int);
		DELETE FROM `U`;
		INSERT INTO `U` VALUES(0x626, 0xFE8B, 0xFE8B, 0xFE8C, 0xFE8C, 1), (0x627, 0xFE8D, 0xFE8D, 0xFE8E, 0xFE8E, 0), (0x6D5, 0xFEE9, 0xFEE9, 0xFEEA, 0xFEEA, 0), (0x628, 0xFE8F, 0xFE91, 0xFE92, 0xFE90, 1), (0x67E, 0xFB56, 0xFB58, 0xFB59, 0xFB57, 1), (0x62A, 0xFE95, 0xFE97, 0xFE98, 0xFE96, 1), (0x62C, 0xFE9D, 0xFE9F, 0xFEA0, 0xFE9E, 1), (0x686, 0xFB7A, 0xFB7C, 0xFB7D, 0xFB7B, 1), (0x62E, 0xFEA5, 0xFEA7, 0xFEA8, 0xFEA6, 1), (0x62F, 0xFEA9, 0xFEA9, 0xFEAA, 0xFEAA, 0), (0x631, 0xFEAD, 0xFEAD, 0xFEAE, 0xFEAE, 0), (0x632, 0xFEAF, 0xFEAF, 0xFEB0, 0xFEB0, 0), (0x698, 0xFB8A, 0xFB8A, 0xFB8B, 0xFB8B, 0), (0x633, 0xFEB1, 0xFEB3, 0xFEB4, 0xFEB2, 1), (0x634, 0xFEB5, 0xFEB7, 0xFEB8, 0xFEB6, 1), (0x63A, 0xFECD, 0xFECF, 0xFED0, 0xFECE, 1), (0x641, 0xFED1, 0xFED3, 0xFED4, 0xFED2, 1), (0x642, 0xFED5, 0xFED7, 0xFED8, 0xFED6, 1), (0x643, 0xFED9, 0xFEDB, 0xFEDC, 0xFEDA, 1), (0x6AF, 0xFB92, 0xFB94, 0xFB95, 0xFB93, 1), (0x6AD, 0xFBD3, 0xFBD5, 0xFBD6, 0xFBD4, 1), (0x644, 0xFEDD, 0xFEDF, 0xFEE0, 0xFEDE, 1), (0x645, 0xFEE1, 0xFEE3, 0xFEE4, 0xFEE2, 1), (0x646, 0xFEE5, 0xFEE7, 0xFEE8, 0xFEE6, 1), (0x6BE, 0xFBAA, 0xFBAC, 0xFBAD, 0xFBAB, 1), (0x648, 0xFEED, 0xFEED, 0xFEEE, 0xFEEE, 0), (0x6C7, 0xFBD7, 0xFBD7, 0xFBD8, 0xFBD8, 0), (0x6C6, 0xFBD9, 0xFBD9, 0xFBDA, 0xFBDA, 0), (0x6C8, 0xFBDB, 0xFBDB, 0xFBDC, 0xFBDC, 0), (0x6CB, 0xFBDE, 0xFBDE, 0xFBDF, 0xFBDF, 0), (0x6D0, 0xFBE4, 0xFBE6, 0xFBE7, 0xFBE5, 1), (0x649, 0xFEEF, 0xFBE8, 0xFBE9, 0xFEF0, 1), (0x64A, 0xFEF1, 0xFEF3, 0xFEF4, 0xFEF2, 1);
		SET @_code =_AscW(source);
		RETURN (SELECT CASE count(*) WHEN 1 THEN(CASE _index WHEN 0 THEN `0` WHEN 1 THEN `1` WHEN 2 THEN `2` WHEN 3 THEN `3` WHEN 4 THEN `4` WHEN 5 THEN `5` END) ELSE @_code END FROM U WHERE `0`=@_code OR `1`=@_code OR `2`=@_code OR `3`=@_code OR `4`=@_code);
	END;

	DROP FUNCTION IF EXISTS `_ChrW`;
	CREATE FUNCTION `_ChrW`(`number` INT) RETURNS VARCHAR(1) CHARSET utf8
	BEGIN
		RETURN CONVERT( unhex(hex(number)) USING ucs2);
	END;

	DROP FUNCTION IF EXISTS `_AscW`;
	CREATE FUNCTION `_AscW`(`source` VARCHAR(1) CHARSET utf8) RETURNS int(11)
	BEGIN
		RETURN ORD(CONVERT( source USING ucs2 ));
	END;