using System;
using System.Text.RegularExpressions;
namespace Uyghur
{
    public class CharUtils
    {
        public int[,] U = { { 0x626, 0xFE8B, 0xFE8B, 0xFE8C, 0xFE8C, 1 }, { 0x627, 0xFE8D, 0xFE8D, 0xFE8E, 0xFE8E, 0 }, { 0x6D5, 0xFEE9, 0xFEE9, 0xFEEA, 0xFEEA, 0 }, { 0x628, 0xFE8F, 0xFE91, 0xFE92, 0xFE90, 1 }, { 0x67E, 0xFB56, 0xFB58, 0xFB59, 0xFB57, 1 }, { 0x62A, 0xFE95, 0xFE97, 0xFE98, 0xFE96, 1 }, { 0x62C, 0xFE9D, 0xFE9F, 0xFEA0, 0xFE9E, 1 }, { 0x686, 0xFB7A, 0xFB7C, 0xFB7D, 0xFB7B, 1 }, { 0x62E, 0xFEA5, 0xFEA7, 0xFEA8, 0xFEA6, 1 }, { 0x62F, 0xFEA9, 0xFEA9, 0xFEAA, 0xFEAA, 0 }, { 0x631, 0xFEAD, 0xFEAD, 0xFEAE, 0xFEAE, 0 }, { 0x632, 0xFEAF, 0xFEAF, 0xFEB0, 0xFEB0, 0 }, { 0x698, 0xFB8A, 0xFB8A, 0xFB8B, 0xFB8B, 0 }, { 0x633, 0xFEB1, 0xFEB3, 0xFEB4, 0xFEB2, 1 }, { 0x634, 0xFEB5, 0xFEB7, 0xFEB8, 0xFEB6, 1 }, { 0x63A, 0xFECD, 0xFECF, 0xFED0, 0xFECE, 1 }, { 0x641, 0xFED1, 0xFED3, 0xFED4, 0xFED2, 1 }, { 0x642, 0xFED5, 0xFED7, 0xFED8, 0xFED6, 1 }, { 0x643, 0xFED9, 0xFEDB, 0xFEDC, 0xFEDA, 1 }, { 0x6AF, 0xFB92, 0xFB94, 0xFB95, 0xFB93, 1 }, { 0x6AD, 0xFBD3, 0xFBD5, 0xFBD6, 0xFBD4, 1 }, { 0x644, 0xFEDD, 0xFEDF, 0xFEE0, 0xFEDE, 1 }, { 0x645, 0xFEE1, 0xFEE3, 0xFEE4, 0xFEE2, 1 }, { 0x646, 0xFEE5, 0xFEE7, 0xFEE8, 0xFEE6, 1 }, { 0x6BE, 0xFBAA, 0xFBAC, 0xFBAD, 0xFBAB, 1 }, { 0x648, 0xFEED, 0xFEED, 0xFEEE, 0xFEEE, 0 }, { 0x6C7, 0xFBD7, 0xFBD7, 0xFBD8, 0xFBD8, 0 }, { 0x6C6, 0xFBD9, 0xFBD9, 0xFBDA, 0xFBDA, 0 }, { 0x6C8, 0xFBDB, 0xFBDB, 0xFBDC, 0xFBDC, 0 }, { 0x6CB, 0xFBDE, 0xFBDE, 0xFBDF, 0xFBDF, 0 }, { 0x6D0, 0xFBE4, 0xFBE6, 0xFBE7, 0xFBE5, 1 }, { 0x649, 0xFEEF, 0xFBE8, 0xFBE9, 0xFEF0, 1 }, { 0x64A, 0xFEF1, 0xFEF3, 0xFEF4, 0xFEF2, 1 } };
        private string reg_str = "([\u0626-\u06d5]+)";
        public CharUtils()
        {
        }
        /** 基本区   转换   扩展区*/
        public string Basic2Extend(string source)
        {
            Regex reg1 = new Regex(reg_str);
            return reg1.Replace(source, word => {
                var str = word.Value;
                string returns = "";
                string target = "";
                string target2 = "";
                int ch;
                int p;
                int length = str.Length;
                if (length > 1)
                {
                    target = str.Substring(0, 1);
                    ch = _GetCode(target, 2);
                    returns += _ChrW(ch);
                    for (var i = 0; i <= length - 3; i++)
                    {
                        target = str.Substring(i, 1);
                        target2 = str.Substring(i + 1, 1);
                        p = _GetCode(target, 5);
                        ch = _GetCode(target2, 2 + p);
                        returns += _ChrW(ch);
                    }
                    target = str.Substring(length - 2, 1);
                    target2 = str.Substring(length - 1, 1);
                    p = _GetCode(target, 5) * 3;
                    ch = _GetCode(target2, 1 + p);
                    returns += _ChrW(ch);
                }
                else
                {
                    ch = _GetCode(str, 1);
                    returns += _ChrW(ch);
                }
                return _ExtendLa(returns.Trim());
            });
        }
        /**基本区  转换   反向扩展区*/
        public string Basic2RExtend(string source)
        {
            var ThisText = Basic2Extend(source);
            var ReverseString = _ReverseString(ThisText);
            return _ReverseAscii(ReverseString);
        }
        /**扩展区   转换   基本区 */
        public string Extend2Basic(string source)
        {
            int i;
            string ch;
            var target = "";
            source = _BasicLa(source);
            for (i = 0; i <= source.Length - 1; i++)
            {
                ch = source.Substring(i, 1);
                target += _ChrW(_GetCode(ch, 0));
            }
            return target;
        }
        /**反向扩展区   转换   基本区 */
        public string RExtend2Basic(string source)
        {
            var target = _ReverseAscii(source);
            target = _ReverseString(target);
            target = Extend2Basic(target);
            return target;
        }
        private string _ReverseAscii(string source)
        {
            Regex reg1 = new Regex(@"([^\uFB00-\uFEFF\s]+)");
            return reg1.Replace(source, word =>
            {
                return _ReverseString(word.Value.ToString());
            });
        }
        private string _ReverseString(string source)
        {// 反转
            string reverse = string.Empty;
            for (int i = source.Length - 1; i >= 0; i--)
            {
                reverse += source[i];
            }
            return reverse;
        }
        private string _ExtendLa(string source)
        {
            Regex reg1 = new Regex(@"(\uFEDF\uFE8E)");
            Regex reg2 = new Regex(@"(\uFEDF\uFE8E)");
            return reg2.Replace(reg1.Replace(source, word => {
                return _ChrW(0xFEFB);
            }), word => {
                return _ChrW(0xFEF);
            });
        }
        private string _BasicLa(string source)
        {
            Regex reg1 = new Regex(@"(\uFEFB)");
            Regex reg2 = new Regex(@"(\uFEFC)");
            return reg2.Replace(reg1.Replace(source, word =>
            {
                return _ChrW(0x644) + _ChrW(0x627);
            }), word =>
            {
                return _ChrW(0x644) + _ChrW(0x627);
            });
        }
        private int _GetCode(string source, int index)
        {
            if (source.Length == 0)
                return 0;
            if (index > 5)
                return _AscW(source);
            for (var i = 0; i <= 32; i++)
            {
                int code = _AscW(source);
                if (code == U[i, 0] || code == U[i, 1] || code == U[i, 2] || code == U[i, 3] || code == U[i, 4])
                    return U[i, index];
            }
            return _AscW(source);
        }
        private int _AscW(char ch)
        {
            return Convert.ToChar(ch);
        }
        private int _AscW(string source)
        {
            return _AscW(source[0]);
        }
        private string _ChrW(int number)
        {
            return Convert.ToChar(number).ToString();
        }
    }
}
