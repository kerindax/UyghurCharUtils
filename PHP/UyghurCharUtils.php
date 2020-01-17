<?php
    // +----------------------------------------------------------------------
    // | Update: 2020-01-17 13:54
    // +----------------------------------------------------------------------
    // | Author: Kerindax <1482152356@qq.com>
    // +----------------------------------------------------------------------
    class UyghurCharUtils {
        private $chars = ['ئ'=>['ﺋ','ﺋ','ﺌ','ﺌ','1'],'ا'=>['ﺍ','ﺍ','ﺎ','ﺎ','0'],'ە'=>['ﻩ','ﻩ','ﻪ','ﻪ','0'],'ب'=>['ﺏ','ﺑ','ﺒ','ﺐ','1'],'پ'=>['ﭖ','ﭘ','ﭙ','ﭗ','1'],'ت'=>['ﺕ','ﺗ','ﺘ','ﺖ','1'],'ج'=>['ﺝ','ﺟ','ﺠ','ﺞ','1'],'چ'=>['ﭺ','ﭼ','ﭽ','ﭻ','1'],'خ'=>['ﺥ','ﺧ','ﺨ','ﺦ','1'],'د'=>['ﺩ','ﺩ','ﺪ','ﺪ','0'],'ر'=>['ﺭ','ﺭ','ﺮ','ﺮ','0'],'ز'=>['ﺯ','ﺯ','ﺰ','ﺰ','0'],'ژ'=>['ﮊ','ﮊ','ﮋ','ﮋ','0'],'س'=>['ﺱ','ﺳ','ﺴ','ﺲ','1'],'ش'=>['ﺵ','ﺷ','ﺸ','ﺶ','1'],'غ'=>['ﻍ','ﻏ','ﻐ','ﻎ','1'],'ف'=>['ﻑ','ﻓ','ﻔ','ﻒ','1'],'ق'=>['ﻕ','ﻗ','ﻘ','ﻖ','1'],'ك'=>['ﻙ','ﻛ','ﻜ','ﻚ','1'],'گ'=>['ﮒ','ﮔ','ﮕ','ﮓ','1'],'ڭ'=>['ﯓ','ﯕ','ﯖ','ﯔ','1'],'ل'=>['ﻝ','ﻟ','ﻠ','ﻞ','1'],'م'=>['ﻡ','ﻣ','ﻤ','ﻢ','1'],'ن'=>['ﻥ','ﻧ','ﻨ','ﻦ','1'],'ھ'=>['ﮪ','ﮬ','ﮭ','ﮫ','1'],'و'=>['ﻭ','ﻭ','ﻮ','ﻮ','0'],'ۇ'=>['ﯗ','ﯗ','ﯘ','ﯘ','0'],'ۆ'=>['ﯙ','ﯙ','ﯚ','ﯚ','0'],'ۈ'=>['ﯛ','ﯛ','ﯜ','ﯜ','0'],'ۋ'=>['ﯞ','ﯞ','ﯟ','ﯟ','0'],'ې'=>['ﯤ','ﯦ','ﯧ','ﯥ','1'],'ى'=>['ﻯ','ﯨ','ﯩ','ﻰ','1'],'ي'=>['ﻱ','ﻳ','ﻴ','ﻲ','1'],];
        private $La = 'ﻻ';
        private $_La = 'ﻼ';
        private $Basic_La = 'لا';
        
        function __construct() {
            mb_internal_encoding("UTF-8");
        }
        /**
         * 基本区   转换   扩展区
         * @param $source 要转换的内容
         * @return string
         */
        public function Basic2Extend($source){
            return preg_replace_callback("/([\x{0626}-\x{06d5}]+)/u",
            function($word){
                preg_match_all('/.|[^\x00]/us', $word[0], $arr);
                $returns = "";
                $p = 0;
                $length = count($arr[0]);
                if($length>1){
                    $returns .= $this->_GetChar($arr[0][0],1);
                    for($i=0;$i<$length-2;$i++){
                        $p = $this->_GetChar($arr[0][$i], 4);
                        $returns .= $this->_GetChar($arr[0][$i + 1], (int)$p + 1);
                    }
                    $p = $this->_GetChar($arr[0][$length-2],4);
                    $returns .= $this->_GetChar($arr[0][$length-1], (int)$p * 3);
                }else{
                    $returns = $arr[0][0];
                }
                return $this->_ExtendLa($returns);
            },
            $source);
        }
        /**
         * 基本区  转换   反向扩展区
         * @param $source 要转换的内容
         * @return string
         */
        public function Basic2RExtend($source){
            return $this->_ReverseAscii($this->_ReverseSubject($this->Basic2Extend($source)));
        }
        /**
         * 扩展区   转换   基本区
         * @param $source 要转换的内容
         * @return string
         */
        public function Extend2Basic($source){
            return preg_replace_callback("/([\x{FB00}-\x{FEFF}])/u",
            function($word){
                foreach ($this->chars as $key => $value)
                    if(in_array($word[0], $value))
                        return $key;
            },$source);
        }
        /**
         * 反向扩展区   转换   基本区
         * @param $source 要转换的内容
         * @return string
         */
        public function RExtend2Basic($source){
            return $this->Extend2Basic($this->_ReverseSubject($this->_ReverseAscii($source)));
        }
        private function _ReverseWords($source){
            preg_match_all('/.|[^\x00]/us', $source, $arr);
            return implode("", array_reverse($arr[0]));
        }
        private function _ReverseSubject($source){
            return preg_replace_callback("/(.+)/u",function($word){
                return $this->_ReverseWords($word[0]);
            },$source);
        }
        private function _ReverseAscii($source){
            return preg_replace_callback("/([^\x{FB00}-\x{FEFF}]+)/u",function($word){
                return $this->_ReverseWords($word[0]);
            },$source);
        }
        private function _ExtendLa($source){
            return preg_replace_callback("/(\x{FEE0}\x{FE8E})/u",function(){
                return $this->_La;
            },preg_replace_callback("/(\x{FEDF}\x{FE8E})/u",function(){
                return $this->La;
            },$source));
        }
        private function _BasicLa($source){
            return preg_replace_callback("/([\x{FEFC}\x{FEFB}])/u",function(){
                return $this->Basic_La;
            },$source);
        }
        private function _GetChar($source, $index){
            return $this->chars[$source][$index];
        }
    }