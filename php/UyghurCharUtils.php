<?php
// +----------------------------------------------------------------------
// | Update: 2020-06-21 00:00
// +----------------------------------------------------------------------
// | Author: Kerindax <1482152356@qq.com>
// +----------------------------------------------------------------------

// 补充函数
if (!function_exists('mb_str_replace'))
{
   function mb_str_replace($search, $replace, $subject, &$count = 0)
   {
      if (!is_array($subject))
      {
         $searches = is_array($search) ? array_values($search) : array($search);
         $replacements = is_array($replace) ? array_values($replace) : array($replace);
         $replacements = array_pad($replacements, count($searches), '');
         foreach ($searches as $key => $search)
         {
            $parts = mb_split(preg_quote($search), $subject);
            $count += count($parts) - 1;
            $subject = implode($replacements[$key], $parts);
         }
      }
      else
      {
         foreach ($subject as $key => $value)
         {
            $subject[$key] = mb_str_replace($search, $replace, $value, $count);
         }
      }
      return $subject;
   }
}
// 补充函数
if (!function_exists('mb_str_reverse'))
{
    function mb_str_reverse($source){
        preg_match_all('/.|[^\x00]/us', $source, $arr);
        return implode("", array_reverse($arr[0]));
    }
}

const BASIC = 0; //基本区形式  A
const ALONE = 1; //单独形式    A
const HEAD  = 2; //头部形式    A_
const CENTR = 3; //中部形式   _A_
const REAR  = 4; //后部形式   _A

const convertRang = "/[\x{0622}-\x{064a}\x{0675}-\x{06d5}]+/u"; //转换范围；不包含哈语的0x0621字母,问号,双引号和Unicode区域的符号
const suffixRang = "/[^\x{0627}\x{062F}-\x{0632}\x{0648}\x{0688}-\x{0699}\x{06C0}-\x{06CB}\x{06D5}]/u"; //分割范围，有后尾的字符表达式
const extendRang = "/[\x{fb50}-\x{fdff}\x{fe70}-\x{feff}]/u"; //扩展区范围；FB50-FDFF ->区域A    FE70-FEFF -> 区域B
const notExtendRang = "/[^\x{fb50}-\x{fdff}\x{fe70}-\x{feff}\s]+(\s[^\x{fb50}-\x{fdff}\x{fe70}-\x{feff}\s]+)*/u"; //不包含扩展区中部包含空格字符集；FB50-FDFF ->区域A    FE70-FEFF -> 区域B

// //特助转换区，扩展区反向转换的时候需要替换
const symbolRang = "/[\)\(\]\[\}\{\>\<\»\«]/u";
const symbolList = [
  ')'=> '(',
  '('=> ')',
  ']'=> '[',
  '['=> ']',
  '}'=> '{',
  '{'=> '}',
  '>'=> '<',
  '<'=> '>',
  '»'=> '«',
  '«'=> '»',
];
// 数字转换对应的字母
function fromCharCode($code){
    return mb_convert_encoding('&#x'.dechex($code).';', 'UTF-8', 'HTML-ENTITIES'); 
};

class UyghurCharUtils {
    // 双字母列表
    private $special = [
        [ 'basic'=> [0x644, 0x627], 'extend'=> [0xfefc], 'link'=> [0xfee0, 0xfe8e], ],// LA
        [ 'basic'=> [0x644, 0x627], 'extend'=> [0xfefb], 'link'=> [0xfedf, 0xfe8e], ],//_LA
    ];
    // 单字母列表
    private $charCode = [];

    function __construct() {
        mb_internal_encoding("UTF-8");
        /**
         * 基本码, 单独形式, 头部形式, 中部形式, 后部形式]
         * [  A  ,     A   ,    A_   ,   _A_  ,   _A   ]
         */
        foreach ([
            [0x626, 0xfe8b, 0xfe8b, 0xfe8c, 0xfe8c], // 1 --- 00-Hemze
            [0x627, 0xfe8d, 0xfe8d, 0xfe8e, 0xfe8e], // 0 --- 01-a   
            [0x6d5, 0xfee9, 0xfee9, 0xfeea, 0xfeea], // 0 --- 02-:e  
            [0x628, 0xfe8f, 0xfe91, 0xfe92, 0xfe90], // 1 --- 03-b   
            [0x67e, 0xfb56, 0xfb58, 0xfb59, 0xfb57], // 1 --- 04-p   
            [0x62a, 0xfe95, 0xfe97, 0xfe98, 0xfe96], // 1 --- 05-t   
            [0x62c, 0xfe9d, 0xfe9f, 0xfea0, 0xfe9e], // 1 --- 06-j   
            [0x686, 0xfb7a, 0xfb7c, 0xfb7d, 0xfb7b], // 1 --- 07-q   
            [0x62e, 0xfea5, 0xfea7, 0xfea8, 0xfea6], // 1 --- 08-h   
            [0x62f, 0xfea9, 0xfea9, 0xfeaa, 0xfeaa], // 0 --- 09-d   
            [0x631, 0xfead, 0xfead, 0xfeae, 0xfeae], // 0 --- 10-r   
            [0x632, 0xfeaf, 0xfeaf, 0xfeb0, 0xfeb0], // 0 --- 11-z   
            [0x698, 0xfb8a, 0xfb8a, 0xfb8b, 0xfb8b], // 0 --- 12-:zh 
            [0x633, 0xfeb1, 0xfeb3, 0xfeb4, 0xfeb2], // 1 --- 13-s   
            [0x634, 0xfeb5, 0xfeb7, 0xfeb8, 0xfeb6], // 1 --- 14-x   
            [0x63a, 0xfecd, 0xfecf, 0xfed0, 0xfece], // 1 --- 15-:gh 
            [0x641, 0xfed1, 0xfed3, 0xfed4, 0xfed2], // 1 --- 16-f   
            [0x642, 0xfed5, 0xfed7, 0xfed8, 0xfed6], // 1 --- 17-:k  
            [0x643, 0xfed9, 0xfedb, 0xfedc, 0xfeda], // 1 --- 18-k   
            [0x6af, 0xfb92, 0xfb94, 0xfb95, 0xfb93], // 1 --- 19-g   
            [0x6ad, 0xfbd3, 0xfbd5, 0xfbd6, 0xfbd4], // 1 --- 20-:ng 
            [0x644, 0xfedd, 0xfedf, 0xfee0, 0xfede], // 1 --- 21-l   
            [0x645, 0xfee1, 0xfee3, 0xfee4, 0xfee2], // 1 --- 22-m   
            [0x646, 0xfee5, 0xfee7, 0xfee8, 0xfee6], // 1 --- 23-n   
            [0x6be, 0xfbaa, 0xfbac, 0xfbad, 0xfbab], // 1 --- 24-:h  
            [0x648, 0xfeed, 0xfeed, 0xfeee, 0xfeee], // 0 --- 25-o   
            [0x6c7, 0xfbd7, 0xfbd7, 0xfbd8, 0xfbd8], // 0 --- 26-u   
            [0x6c6, 0xfbd9, 0xfbd9, 0xfbda, 0xfbda], // 0 --- 27-:o  
            [0x6c8, 0xfbdb, 0xfbdb, 0xfbdc, 0xfbdc], // 0 --- 28-v   
            [0x6cb, 0xfbde, 0xfbde, 0xfbdf, 0xfbdf], // 0 --- 29-w   
            [0x6d0, 0xfbe4, 0xfbe6, 0xfbe7, 0xfbe5], // 1 --- 30-e   
            [0x649, 0xfeef, 0xfbe8, 0xfbe9, 0xfef0], // 1 --- 31-i   
            [0x64a, 0xfef1, 0xfef3, 0xfef4, 0xfef2], // 1 --- 32-y 
            
            [0x6c5, 0xfbe0, 0xfbe0, 0xfbe1, 0xfbe1], // 0 --- kz o_
            [0x6c9, 0xfbe2, 0xfbe2, 0xfbe3, 0xfbe3], // 0 --- kz o^
            [0x62d, 0xfea1, 0xfea3, 0xfea4, 0xfea2], // 1 --- kz h
            [0x639, 0xfec9, 0xfecb, 0xfecc, 0xfeca], // 1 --- kz c
        ] as $row){
            $list = array_map(function($item){
                return fromCharCode($item);
            },$row);
            foreach ($list as $item){
                $this->charCode[$item] = $list;
            }
        }
    }
    /**
     * 基本区   转换   扩展区
     * @param $source 要转换的内容，可以包含混合字符串
     * @return string
     */
    public function Basic2Extend($source){
        return preg_replace_callback(convertRang,function($word){
            $returns = preg_replace_callback("/\s(\S)(?=\S|$)/u",function($ch){
                return $this->getChar($ch[1], REAR);
            },
            preg_replace_callback("/\s(\S)\s/u",function($ch){
                return $this->getChar($ch[1], CENTR);
            },
            preg_replace_callback("/(?<=\S|^)(\S)\s/u",function($ch){
                return $this->getChar($ch[1], HEAD);
            },
            preg_replace_callback("/(?<=^|\S)(\S)(?=$|\S)/u",function($ch){
                return $this->getChar($ch[1], ALONE);
            },
            trim(preg_replace_callback(suffixRang,function($ch){
                return $ch[0] . '  ';
            },
            $word[0]))))));
            return $this->extendLa($returns);
        }, $source);
    }
    /**
     * 扩展区   转换   基本区
     * @param $source 要转换的内容
     * @return string
     */
    public function Extend2Basic($source){
        return preg_replace_callback(extendRang,function($ch){
                return $this->getChar($ch[0], BASIC);
        },$this->basicLa($source));
    }
    /* 基本区  转换   反向扩展区
     * @param $source 要转换的内容
     * @return string
     */
    public function Basic2RExtend($source){
        return $this->reverseAscii($this->reverseSubject($this->Basic2Extend($source)));
    }
    /**
     * 反向扩展区   转换   基本区
     * @param $source 要转换的内容
     * @return string
     */
    public function RExtend2Basic($source){
        return $this->Extend2Basic($this->reverseSubject($this->reverseAscii($source)));
    }
    /**
     * Ascii区反转
     */
    private function reverseAscii($source){
        return preg_replace_callback(symbolRang,function($ch){//替换符号
            return symbolList[$ch[0]] ?? $ch[0];
        },preg_replace_callback(notExtendRang,function($word){
            return mb_str_reverse($word[0]);
        },$source));
    }
    /**
     * 对象反转
     */
    private function reverseSubject($str){
        return preg_replace_callback("/(.+)/u",function($subject){//不包含换行符
            return mb_str_reverse($subject[0]);
        },$str);
    }
    /**
     * 获取对应字母
     */
    private function getChar($ch, $index){
        if(array_key_exists($ch,$this->charCode)){
            return $this->charCode[$ch][$index];
        }else{
            return $ch;
        }
    }
    /**
     * La字母转换扩展区
     */
    private function extendLa($source){
        foreach ($this->special as $item){
            $source = mb_str_replace($this->getString($item['link']), $this->getString($item['extend']),$source);
        }
        return $source;
    }
    /**
     * La字母转换基本区
     */
    private function basicLa($source){
        foreach ($this->special as $item){
            $source = mb_str_replace($this->getString($item['extend']), $this->getString($item['basic']),$source);
        }
        return $source;
    }
    /**
     * 双目字母转换字符串
     */
    private function getString($value){
        $sb = array('');
        foreach ($value as $item){
            array_push($sb,fromCharCode($item));
        }
        return implode("",$sb);
    }
}
