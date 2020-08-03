import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
// +----------------------------------------------------------------------
// | Update: 2020-07-28 00:00
// +----------------------------------------------------------------------
// | Author: Kerindax <1482152356@qq.com>
// +----------------------------------------------------------------------
public class UyghurCharUtils {
    /**
     * 补充接口
     */
    private interface replaceCallBack {
        String replace(Matcher matcher);
    }
    /**
     * 补充函数
     */
    private static String replaceAll(String string, String patternRang, replaceCallBack replacement) {
        if (string == null) {
            return null;
        }
        Pattern pattern = Pattern.compile(patternRang);
        Matcher m = pattern.matcher(string);
        if (m.find()) {
            StringBuffer sb = new StringBuffer();
            while (true) {
                m.appendReplacement(sb, replacement.replace(m));
                if (!m.find()) {
                    break;
                }
            }
            m.appendTail(sb);
            return sb.toString();
        }
        return string;
    }
    // 双目字对象
    private class SpecialObject
    {
        public int[] basic;
        public int[] extend;
        public int[] link;
    }

    private final int BASIC = 0; //基本区形式  A
    private final int ALONE = 1; //单独形式    A
    private final int HEAD  = 2; //头部形式    A_
    private final int CENTR = 3; //中部形式   _A_
    private final int REAR  = 4; //后部形式   _A

    private final String convertRang = "[\\u0622-\\u064a\\u0675-\\u06d5]+"; //转换范围；不包含哈语的0x0621字母,问号,双引号和Unicode区域的符号
    private final String suffixRang = "[^\\u0627\\u062F-\\u0632\\u0648\\u0688-\\u0699\\u06C0-\\u06CB\\u06D5]"; //分割范围，有后尾的字符表达式
    private final String extendRang = "[\\ufb50-\\ufdff\\ufe70-\\ufeff]"; //扩展区范围；FB50-FDFF ->区域A    FE70-FEFF -> 区域B
    private final String notExtendRang = "[^\\ufb50-\\ufdff\\ufe70-\\ufeff\\s]+(\\s[^\\ufb50-\\ufdff\\ufe70-\\ufeff\\s]+)*"; //不包含扩展区中部包含空格字符集；FB50-FDFF ->区域A    FE70-FEFF -> 区域B

    //特助转换区，扩展区反向转换的时候需要替换
    private final String symbolRang = "[\\)\\(\\]\\[\\}\\{\\>\\<\\»\\«]";
    private final HashMap<String, String> symbolList = new HashMap<>(){
        {
            put(")", "(");
            put("(", ")");
            put("]", "[");
            put("[", "]");
            put("}", "{");
            put("{", "}");
            put(">", "<");
            put("<", ">");
            put("»", "«");
            put("«", "»");
        }
    };
    //数字转换对应的字母
    private final String fromCharCode(int number)
    {
        return String.valueOf((char) number);
    }
    // 双字母列表
    private ArrayList<SpecialObject> special = new ArrayList(){
        {
            add(new SpecialObject(){
                {
                    basic = new int[]{ 0x644, 0x627 };
                    extend = new int[]{ 0xfefc };
                    link = new int[]{ 0xfee0, 0xfe8e };
                }
            });
            add(new SpecialObject(){
                {
                    basic = new int[]{ 0x644, 0x627 };
                    extend = new int[]{ 0xfefb };
                    link = new int[]{ 0xfedf, 0xfe8e };
                }
            });
        }
    };
    // 单字母列表
    private HashMap<String, ArrayList> charCode = new HashMap<>();

    public UyghurCharUtils() {
        /**
         * 基本码, 单独形式, 头部形式, 中部形式, 后部形式]
         * [  A  ,     A   ,    A_   ,   _A_  ,   _A   ]
         */
        for (int[] row : new int[][] {
                new int[] {0x626, 0xfe8b, 0xfe8b, 0xfe8c, 0xfe8c}, // 1 --- 00-Hemze
                new int[] {0x627, 0xfe8d, 0xfe8d, 0xfe8e, 0xfe8e}, // 0 --- 01-a
                new int[] {0x6d5, 0xfee9, 0xfee9, 0xfeea, 0xfeea}, // 0 --- 02-:e
                new int[] {0x628, 0xfe8f, 0xfe91, 0xfe92, 0xfe90}, // 1 --- 03-b
                new int[] {0x67e, 0xfb56, 0xfb58, 0xfb59, 0xfb57}, // 1 --- 04-p
                new int[] {0x62a, 0xfe95, 0xfe97, 0xfe98, 0xfe96}, // 1 --- 05-t
                new int[] {0x62c, 0xfe9d, 0xfe9f, 0xfea0, 0xfe9e}, // 1 --- 06-j
                new int[] {0x686, 0xfb7a, 0xfb7c, 0xfb7d, 0xfb7b}, // 1 --- 07-q
                new int[] {0x62e, 0xfea5, 0xfea7, 0xfea8, 0xfea6}, // 1 --- 08-h
                new int[] {0x62f, 0xfea9, 0xfea9, 0xfeaa, 0xfeaa}, // 0 --- 09-d
                new int[] {0x631, 0xfead, 0xfead, 0xfeae, 0xfeae}, // 0 --- 10-r
                new int[] {0x632, 0xfeaf, 0xfeaf, 0xfeb0, 0xfeb0}, // 0 --- 11-z
                new int[] {0x698, 0xfb8a, 0xfb8a, 0xfb8b, 0xfb8b}, // 0 --- 12-:zh
                new int[] {0x633, 0xfeb1, 0xfeb3, 0xfeb4, 0xfeb2}, // 1 --- 13-s
                new int[] {0x634, 0xfeb5, 0xfeb7, 0xfeb8, 0xfeb6}, // 1 --- 14-x
                new int[] {0x63a, 0xfecd, 0xfecf, 0xfed0, 0xfece}, // 1 --- 15-:gh
                new int[] {0x641, 0xfed1, 0xfed3, 0xfed4, 0xfed2}, // 1 --- 16-f
                new int[] {0x642, 0xfed5, 0xfed7, 0xfed8, 0xfed6}, // 1 --- 17-:k
                new int[] {0x643, 0xfed9, 0xfedb, 0xfedc, 0xfeda}, // 1 --- 18-k
                new int[] {0x6af, 0xfb92, 0xfb94, 0xfb95, 0xfb93}, // 1 --- 19-g
                new int[] {0x6ad, 0xfbd3, 0xfbd5, 0xfbd6, 0xfbd4}, // 1 --- 20-:ng
                new int[] {0x644, 0xfedd, 0xfedf, 0xfee0, 0xfede}, // 1 --- 21-l
                new int[] {0x645, 0xfee1, 0xfee3, 0xfee4, 0xfee2}, // 1 --- 22-m
                new int[] {0x646, 0xfee5, 0xfee7, 0xfee8, 0xfee6}, // 1 --- 23-n
                new int[] {0x6be, 0xfbaa, 0xfbac, 0xfbad, 0xfbab}, // 1 --- 24-:h
                new int[] {0x648, 0xfeed, 0xfeed, 0xfeee, 0xfeee}, // 0 --- 25-o
                new int[] {0x6c7, 0xfbd7, 0xfbd7, 0xfbd8, 0xfbd8}, // 0 --- 26-u
                new int[] {0x6c6, 0xfbd9, 0xfbd9, 0xfbda, 0xfbda}, // 0 --- 27-:o
                new int[] {0x6c8, 0xfbdb, 0xfbdb, 0xfbdc, 0xfbdc}, // 0 --- 28-v
                new int[] {0x6cb, 0xfbde, 0xfbde, 0xfbdf, 0xfbdf}, // 0 --- 29-w
                new int[] {0x6d0, 0xfbe4, 0xfbe6, 0xfbe7, 0xfbe5}, // 1 --- 30-e
                new int[] {0x649, 0xfeef, 0xfbe8, 0xfbe9, 0xfef0}, // 1 --- 31-i
                new int[] {0x64a, 0xfef1, 0xfef3, 0xfef4, 0xfef2}, // 1 --- 32-y

                new int[] {0x6c5, 0xfbe0, 0xfbe0, 0xfbe1, 0xfbe1}, // 0 --- kz o_
                new int[] {0x6c9, 0xfbe2, 0xfbe2, 0xfbe3, 0xfbe3}, // 0 --- kz o^
                new int[] {0x62d, 0xfea1, 0xfea3, 0xfea4, 0xfea2}, // 1 --- kz h
                new int[] {0x639, 0xfec9, 0xfecb, 0xfecc, 0xfeca}, // 1 --- kz c
        }) {
            ArrayList list = new ArrayList();
            for(var el : row)
            {
                list.add(fromCharCode(el));
            }
            for(var item : list)
            {
                if (!charCode.containsKey(item))
                {
                    charCode.put((String)item, list);
                }
            }
        }
    }
    /**
     * 基本区   转换   扩展区
     * @param source
     * 要转换的内容，可以包含混合字符串
     * @return
     * 已转换的内容
     */
    public String Basic2Extend(String source) {
        return replaceAll(source, convertRang, new replaceCallBack() {
            public String replace(Matcher m) {
                var returns = replaceAll(replaceAll(replaceAll(replaceAll(replaceAll(m.group(0), suffixRang, new replaceCallBack() {
                    public String replace(Matcher m) {
                        return m.group(0) + "  ";
                    }
                }).trim(), "(?<=^|\\S)(\\S)(?=$|\\S)", new replaceCallBack() {//中部字母-后部有尾
                    public String replace(Matcher m) {
                        return getChar(m.group(1), ALONE);
                    }
                }), "(?<=\\S|^)(\\S)\\s", new replaceCallBack() {//中部字母-前后有尾
                    public String replace(Matcher m) {
                        return getChar(m.group(1), HEAD);
                    }
                }), "\\s(\\S)\\s", new replaceCallBack() {//最后字母-前部有尾
                    public String replace(Matcher m) {
                        return getChar(m.group(1), CENTR);
                    }
                }), "\\s(\\S)(?=\\S|$)", new replaceCallBack() {//中部字母-前部有尾
                    public String replace(Matcher m) {
                        return getChar(m.group(1), REAR);
                    }
                });
                return extendLa(returns);
            }
        });
    }
    /**
     * 扩展区   转换   基本区
     * @param source
     * 要转换的内容
     * @return
     * 已转换的内容
     */
    public String Extend2Basic(String source) {
        return replaceAll(this.basicLa(source), extendRang, new replaceCallBack() {
            public String replace(Matcher m) {
                return getChar(m.group(0), BASIC);
            }
        });
    }
    /**
     * 基本区  转换   反向扩展区
     * @param source
     * 要转换的内容
     * @return
     * 已转换的内容
     */
    public String Basic2RExtend(String source) {
        return this.reverseAscii(this.reverseSubject(this.Basic2Extend(source)));
    }
    /**
     * 反向扩展区   转换   基本区
     * @param source
     * 要转换的内容
     * @return
     * 已转换的内容
     */
    public String RExtend2Basic(String source) {
        return this.Extend2Basic(this.reverseSubject(this.reverseAscii(source)));
    }
    /**
     * Ascii区反转
     */
    private String reverseAscii(String source)
    {
        return replaceAll(source, notExtendRang, new replaceCallBack() {
            public String replace(Matcher m) {
                var word = new StringBuffer(m.group(0)).reverse().toString();
                return replaceAll(word, symbolRang, new replaceCallBack() {
                    public String replace(Matcher m) {
                        var ch = m.group(0);
                        if (symbolList.containsKey(ch))
                            return symbolList.get(ch);
                        else
                            return ch;
                    }
                });
            }
        });
    }
    /**
     * 对象反转
     */
    private String reverseSubject(String str)
    {
        return replaceAll(str, ".+", new replaceCallBack() {
            public String replace(Matcher m) {
                return new StringBuffer(m.group(0)).reverse().toString();
            }
        });
    }
    /**
     * 获取对应字母
     */
    private String getChar(String ch, int index)
    {
        if (charCode.containsKey(ch))
            return charCode.get(ch).get(index).toString();
        else
            return ch;
    }
    /**
     * La字母转换扩展区
     */
    private String extendLa(String source)
    {
        for(SpecialObject item : this.special)
        {
            source = source.replace(this.getString(item.link), this.getString(item.extend));
        }
        return source;
    }
    /**
     * La字母转换基本区
     */
    private String basicLa(String source)
    {
        for(SpecialObject item : this.special)
        {
            source = source.replace(this.getString(item.extend), this.getString(item.basic));
        }
        return source;
    }
    /**
     * 双目字母转换字符串
     */
    public String getString(int[] value){
        StringBuilder sb = new StringBuilder();
        for(int item : value){
            sb.append(fromCharCode(item));
        }
        return sb.toString();
    }

}