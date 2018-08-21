
import java.util.ArrayList;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
/**
 *
 * @author uqkun_000
 */
public class UyghurCharUtils {

    private final int[][] U = {{0x626, 0xFE8B, 0xFE8B, 0xFE8C, 0xFE8C, 1}, {0x627, 0xFE8D, 0xFE8D, 0xFE8E, 0xFE8E, 0}, {0x6D5, 0xFEE9, 0xFEE9, 0xFEEA, 0xFEEA, 0}, {0x628, 0xFE8F, 0xFE91, 0xFE92, 0xFE90, 1}, {0x67E, 0xFB56, 0xFB58, 0xFB59, 0xFB57, 1}, {0x62A, 0xFE95, 0xFE97, 0xFE98, 0xFE96, 1}, {0x62C, 0xFE9D, 0xFE9F, 0xFEA0, 0xFE9E, 1}, {0x686, 0xFB7A, 0xFB7C, 0xFB7D, 0xFB7B, 1}, {0x62E, 0xFEA5, 0xFEA7, 0xFEA8, 0xFEA6, 1}, {0x62F, 0xFEA9, 0xFEA9, 0xFEAA, 0xFEAA, 0}, {0x631, 0xFEAD, 0xFEAD, 0xFEAE, 0xFEAE, 0}, {0x632, 0xFEAF, 0xFEAF, 0xFEB0, 0xFEB0, 0}, {0x698, 0xFB8A, 0xFB8A, 0xFB8B, 0xFB8B, 0}, {0x633, 0xFEB1, 0xFEB3, 0xFEB4, 0xFEB2, 1}, {0x634, 0xFEB5, 0xFEB7, 0xFEB8, 0xFEB6, 1}, {0x63A, 0xFECD, 0xFECF, 0xFED0, 0xFECE, 1}, {0x641, 0xFED1, 0xFED3, 0xFED4, 0xFED2, 1}, {0x642, 0xFED5, 0xFED7, 0xFED8, 0xFED6, 1}, {0x643, 0xFED9, 0xFEDB, 0xFEDC, 0xFEDA, 1}, {0x6AF, 0xFB92, 0xFB94, 0xFB95, 0xFB93, 1}, {0x6AD, 0xFBD3, 0xFBD5, 0xFBD6, 0xFBD4, 1}, {0x644, 0xFEDD, 0xFEDF, 0xFEE0, 0xFEDE, 1}, {0x645, 0xFEE1, 0xFEE3, 0xFEE4, 0xFEE2, 1}, {0x646, 0xFEE5, 0xFEE7, 0xFEE8, 0xFEE6, 1}, {0x6BE, 0xFBAA, 0xFBAC, 0xFBAD, 0xFBAB, 1}, {0x648, 0xFEED, 0xFEED, 0xFEEE, 0xFEEE, 0}, {0x6C7, 0xFBD7, 0xFBD7, 0xFBD8, 0xFBD8, 0}, {0x6C6, 0xFBD9, 0xFBD9, 0xFBDA, 0xFBDA, 0}, {0x6C8, 0xFBDB, 0xFBDB, 0xFBDC, 0xFBDC, 0}, {0x6CB, 0xFBDE, 0xFBDE, 0xFBDF, 0xFBDF, 0}, {0x6D0, 0xFBE4, 0xFBE6, 0xFBE7, 0xFBE5, 1}, {0x649, 0xFEEF, 0xFBE8, 0xFBE9, 0xFEF0, 1}, {0x64A, 0xFEF1, 0xFEF3, 0xFEF4, 0xFEF2, 1}};
    private final String reg_str = "([\\u0626-\\u06d5]+)";
    public String Basic2Extend(String source) {
        Pattern reg1 = Pattern.compile(reg_str);
        Matcher m = reg1.matcher(source);
        StringBuffer sb = new StringBuffer();
        while (m.find()) {
            String str = m.group(0);
            String returns = "", target = "", target2 = "";
            int ch, p, length = str.length();

            if (length > 1) {
                target = str.substring(0, 1);
                ch = _GetCode(target, 2);
                returns += _ChrW(ch);
                for (int i = 0; i < length - 2; i++) {
                    target = str.substring(i, i + 1);
                    target2 = str.substring(i + 1, i + 2);
                    p = _GetCode(target, 5);
                    ch = _GetCode(target2, p + 2);
                    returns += _ChrW(ch);
                }
                target = str.substring(length - 2, length - 1);
                target2 = str.substring(length - 1, length);
                p = _GetCode(target, 5) * 3;
                ch = _GetCode(target2, 1 + p);
                returns += _ChrW(ch);
            } else {
                ch = _GetCode(str, 1);
                returns += _ChrW(ch);
            }

            m.appendReplacement(sb, _ExtendLa(returns.trim()));
        }

        m.appendTail(sb);
        return sb.toString();
    }

    public String Basic2RExtend(String source) {
        String thisText = Basic2Extend(source);
        String reverseString = _ReverseString(thisText);
        return _ReverseAscii(reverseString);
    }

    public String Extend2Basic(String source) {
        String ch, target = "";
        source = _BasicLa(source);
        for (int i = 0; i < source.length(); i++) {
            ch = source.substring(i, i + 1);
            target += _ChrW(_GetCode(ch, 0));
        }

        return target;
    }

    public String RExtend2Basic(String source) {
        String target = _ReverseAscii(source);
        target = _ReverseString(target);
        target = Extend2Basic(target);
        return target;
    }

    public List<String> GetSyllable(String source) {
        source = source.trim();
        List<String> list = new ArrayList<>();
        String sozuk = "اەوۇۆۈېىئ";

        String ch, str = source;
        boolean check = false;
        for (int i = source.length() - 1; i >= 0; i--) {
            ch = source.substring(i, i + 1);

            if (ch.equals("ئ")) {
                list.add(0, str.substring(i));
                str = source.substring(0, i);
                continue;
            }

            if (sozuk.contains(source.substring(i, i + 1)) && i > 0 && !sozuk.contains(source.substring(i - 1, i))) {
                check = false;
                for (char c : source.substring(0, i - 1).toCharArray()) {
                    if (sozuk.contains(String.valueOf(c))) {
                        check = true;
                        break;
                    }
                }

                if (check) {
                    list.add(0, str.substring(i - 1));
                    str = source.substring(0, i - 1);
                    i--;
                } else {
                    list.add(0, str);
                    break;
                }
            }

            if (i == 0 && !str.trim().equals("")) {
                list.add(0, str);
            }
        }

        return list;
    }

    private String _ReverseString(String source) {
        return new StringBuffer(source).reverse().toString();
    }

    private String _ReverseAscii(String source) {
        Pattern reg1 = Pattern.compile("([^\\uFB00-\\uFEFF\\s]+)");
        Matcher m = reg1.matcher(source);
        StringBuffer sb = new StringBuffer();
        while (m.find()) {
            String str = m.group(0);
            m.appendReplacement(sb, _ReverseString(str));
        }
        m.appendTail(sb);
        return sb.toString();
    }

    private String _ExtendLa(String source) {
        Pattern reg1 = Pattern.compile("(\\uFEDF\\uFE8E)");
        Pattern reg2 = Pattern.compile("(\\uFEE0\\uFE8E)");
        return reg2.matcher(reg1.matcher(source).replaceAll(_ChrW(0xFEFB))).replaceAll(_ChrW(0xFEFC));
    }

    private String _BasicLa(String source) {
        Pattern reg1 = Pattern.compile("(\\uFEFB)");
        Pattern reg2 = Pattern.compile("(\\uFEFC)");
        return reg2.matcher(reg1.matcher(source).replaceAll(_ChrW(0xFEDF) + _ChrW(0xFE8E))).replaceAll(_ChrW(0xFEE0) + _ChrW(0xFE8E));
    }

    private int _GetCode(String source, int index) {
        int target = 0;
        if (source.length() == 0) {
            return 0;
        }
        if (index > 5) {
            return _AscW(source);
        }
        for (int i = 0; i < 33; i++) {
            if (_AscW(source) == U[i][0]) {
                return U[i][index];
            }
        }
        return _AscW(source);
    }

    public static int _AscW(char ch) {
        return Integer.valueOf(ch);//.intValue();
    }

    public static int _AscW(String source) {
        return _AscW(source.charAt(0));
    }

    public static String _ChrW(int code) {
        return String.valueOf((char) code);
    }
}
