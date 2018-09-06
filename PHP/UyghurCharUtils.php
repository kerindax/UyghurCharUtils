<?php 
    class UyghurCharUtils {
        private $U = array(array(0x626, 0xFE8B, 0xFE8B, 0xFE8C, 0xFE8C, 1), array(0x627, 0xFE8D, 0xFE8D, 0xFE8E, 0xFE8E, 0), array(0x6D5, 0xFEE9, 0xFEE9, 0xFEEA, 0xFEEA, 0), array(0x628, 0xFE8F, 0xFE91, 0xFE92, 0xFE90, 1), array(0x67E, 0xFB56, 0xFB58, 0xFB59, 0xFB57, 1), array(0x62A, 0xFE95, 0xFE97, 0xFE98, 0xFE96, 1), array(0x62C, 0xFE9D, 0xFE9F, 0xFEA0, 0xFE9E, 1), array(0x686, 0xFB7A, 0xFB7C, 0xFB7D, 0xFB7B, 1), array(0x62E, 0xFEA5, 0xFEA7, 0xFEA8, 0xFEA6, 1), array(0x62F, 0xFEA9, 0xFEA9, 0xFEAA, 0xFEAA, 0), array(0x631, 0xFEAD, 0xFEAD, 0xFEAE, 0xFEAE, 0), array(0x632, 0xFEAF, 0xFEAF, 0xFEB0, 0xFEB0, 0), array(0x698, 0xFB8A, 0xFB8A, 0xFB8B, 0xFB8B, 0), array(0x633, 0xFEB1, 0xFEB3, 0xFEB4, 0xFEB2, 1), array(0x634, 0xFEB5, 0xFEB7, 0xFEB8, 0xFEB6, 1), array(0x63A, 0xFECD, 0xFECF, 0xFED0, 0xFECE, 1), array(0x641, 0xFED1, 0xFED3, 0xFED4, 0xFED2, 1), array(0x642, 0xFED5, 0xFED7, 0xFED8, 0xFED6, 1), array(0x643, 0xFED9, 0xFEDB, 0xFEDC, 0xFEDA, 1), array(0x6AF, 0xFB92, 0xFB94, 0xFB95, 0xFB93, 1), array(0x6AD, 0xFBD3, 0xFBD5, 0xFBD6, 0xFBD4, 1), array(0x644, 0xFEDD, 0xFEDF, 0xFEE0, 0xFEDE, 1), array(0x645, 0xFEE1, 0xFEE3, 0xFEE4, 0xFEE2, 1), array(0x646, 0xFEE5, 0xFEE7, 0xFEE8, 0xFEE6, 1), array(0x6BE, 0xFBAA, 0xFBAC, 0xFBAD, 0xFBAB, 1), array(0x648, 0xFEED, 0xFEED, 0xFEEE, 0xFEEE, 0), array(0x6C7, 0xFBD7, 0xFBD7, 0xFBD8, 0xFBD8, 0), array(0x6C6, 0xFBD9, 0xFBD9, 0xFBDA, 0xFBDA, 0), array(0x6C8, 0xFBDB, 0xFBDB, 0xFBDC, 0xFBDC, 0), array(0x6CB, 0xFBDE, 0xFBDE, 0xFBDF, 0xFBDF, 0), array(0x6D0, 0xFBE4, 0xFBE6, 0xFBE7, 0xFBE5, 1), array(0x649, 0xFEEF, 0xFBE8, 0xFBE9, 0xFEF0, 1), array(0x64A, 0xFEF1, 0xFEF3, 0xFEF4, 0xFEF2, 1));
        private $reg_str = "/([\x{0626}-\x{06d5}]+)/u";

        function __construct() {
            mb_internal_encoding("UTF-8");
        }
        /**
         * 基本区   转换   扩展区
         * @param $source 要转换的内容
         * @return 已转换的内容
         */
        public function Basic2Extend($source){
            return preg_replace_callback($this->reg_str,
            function($word){
                $str = $word[0];
                $returns = "";
                $target = "";
                $target2 = "";
                $ch = 0;
                $p = 0;
                $length = mb_strlen($str);
                if($length>1){
                    $target = mb_substr($str,0,1);
                    $ch = $this->_GetCode($target,2);
                    $returns .= $this->_ChrW($ch);
                    for($i=0;$i<$length-2;$i++){
                        $target = mb_substr($str,$i,1);
                        $target2 = mb_substr($str,$i+1,1);
                        $p = $this->_GetCode($target,5);
                        $ch = $this->_GetCode($target2,2+$p);
                        $returns .= $this->_ChrW($ch);
                    }
                    $target = mb_substr($str,$length-2,1);
                    $target2 = mb_substr($str,$length-1,1);
                    $p = $this->_GetCode($target,5) * 3;
                    $ch = $this->_GetCode($target2, 1 + $p);
                    $returns .= $this->_ChrW($ch);
                }else{
                    $ch = $this->_GetCode($str,1);
                    $returns .= $this->_ChrW($ch);
                }
                return $this->_ExtendLa(trim($returns));
            },
            $source);
        }
        /**
         * 基本区  转换   反向扩展区
         * @param $source 要转换的内容
         * @return 已转换的内容
         */
       public function Basic2RExtend($source){
            $ThisText = $this->Basic2Extend($source);
            $ReverseString = $this->_ReverseString($ThisText);
            return $this->_ReverseAscii($ReverseString);
        }
        /**
         * 扩展区   转换   基本区
         * @param $source 要转换的内容
         * @return 已转换的内容
         */
        public function Extend2Basic($source){
            $ch = "";
            $target = "";
            $source = $this->_BasicLa($source);
            for($i=0;$i<mb_strlen($source);$i++){
                $ch = mb_substr($source,$i,1);
                $target .= $this->_ChrW($this->_GetCode($ch,0));
            }
            return $target;
        }
        /**
         * 反向扩展区   转换   基本区
         * @param $source 要转换的内容
         * @return 已转换的内容
         */
        public function RExtend2Basic($source){
            $target = $this->_ReverseAscii($source);
            $target = $this->_ReverseString($target);
            $target = $this->Extend2Basic($target);
            return $target;
        }
        private function _ReverseString($source){
            $r = '';
            for ($i = mb_strlen($source); $i>=0; $i--) {
                $r .= mb_substr($source, $i, 1);
            }
            return $r;
        }
        private function _ReverseAscii($source){
            return preg_replace_callback("/([^\x{FB00}-\x{FEFF}\s]+)/u",
            function($word){
                return $this->_ReverseString($word[0]);
            },
            $source);
        }
        private function _ExtendLa($source){
            return preg_replace_callback("/(\x{FEE0}\x{FE8E})/u",
            function($word){
                return $this->_ChrW(0x644) . $this->_ChrW(0xFEFC);
            },
            preg_replace_callback("/(\x{FEDF}\x{FE8E})/u",
            function($word){
                return $this->_ChrW(0xFEFB);
            },
            $source));
        }
        private function _BasicLa($source){
            return preg_replace_callback("/(\x{FEFC})/u",
            function($word){
                return $this->_ChrW(0x644) . $this->_ChrW(0x627);
            },
            preg_replace_callback("/(\x{FEFB})/u",
            function($word){
                return $this->_ChrW(0x644) . $this->_ChrW(0x627);
            },
            $source));
        }
         private function _GetCode($source, $index){
            if(strlen($source)==0) return 0;
            if($index>5) return $this->_AscW($source);
            for($i=0;$i<33;$i++){
                if($this->_AscW($source) == $this->U[$i][0]){
                    return $this->U[$i][$index];
                }
            }
            return $this->_AscW($source);
        }
        private function _AscW($source){
            $encode = json_encode($source);
            if(preg_match('/^\"\\\u\w+\"$/',$encode)){ 
                return hexdec(json_encode($source));
            }else{
                return ord($source);
            }
        }
        private function _ChrW($number){
            return mb_convert_encoding('&#x'.dechex($number).';', 'UTF-8', 'HTML-ENTITIES');
        }
    }
?> 