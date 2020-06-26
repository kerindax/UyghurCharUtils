package main

import (
//    "fmt"
    "regexp"
)

type UyghurCharUtils struct {}
var (
    U [][]int = [][]int{{0x626, 0xFE8B, 0xFE8B, 0xFE8C, 0xFE8C, 1}, {0x627, 0xFE8D, 0xFE8D, 0xFE8E, 0xFE8E, 0}, {0x6D5, 0xFEE9, 0xFEE9, 0xFEEA, 0xFEEA, 0}, {0x628, 0xFE8F, 0xFE91, 0xFE92, 0xFE90, 1}, {0x67E, 0xFB56, 0xFB58, 0xFB59, 0xFB57, 1}, {0x62A, 0xFE95, 0xFE97, 0xFE98, 0xFE96, 1}, {0x62C, 0xFE9D, 0xFE9F, 0xFEA0, 0xFE9E, 1}, {0x686, 0xFB7A, 0xFB7C, 0xFB7D, 0xFB7B, 1}, {0x62E, 0xFEA5, 0xFEA7, 0xFEA8, 0xFEA6, 1}, {0x62F, 0xFEA9, 0xFEA9, 0xFEAA, 0xFEAA, 0}, {0x631, 0xFEAD, 0xFEAD, 0xFEAE, 0xFEAE, 0}, {0x632, 0xFEAF, 0xFEAF, 0xFEB0, 0xFEB0, 0}, {0x698, 0xFB8A, 0xFB8A, 0xFB8B, 0xFB8B, 0}, {0x633, 0xFEB1, 0xFEB3, 0xFEB4, 0xFEB2, 1}, {0x634, 0xFEB5, 0xFEB7, 0xFEB8, 0xFEB6, 1}, {0x63A, 0xFECD, 0xFECF, 0xFED0, 0xFECE, 1}, {0x641, 0xFED1, 0xFED3, 0xFED4, 0xFED2, 1}, {0x642, 0xFED5, 0xFED7, 0xFED8, 0xFED6, 1}, {0x643, 0xFED9, 0xFEDB, 0xFEDC, 0xFEDA, 1}, {0x6AF, 0xFB92, 0xFB94, 0xFB95, 0xFB93, 1}, {0x6AD, 0xFBD3, 0xFBD5, 0xFBD6, 0xFBD4, 1}, {0x644, 0xFEDD, 0xFEDF, 0xFEE0, 0xFEDE, 1}, {0x645, 0xFEE1, 0xFEE3, 0xFEE4, 0xFEE2, 1}, {0x646, 0xFEE5, 0xFEE7, 0xFEE8, 0xFEE6, 1}, {0x6BE, 0xFBAA, 0xFBAC, 0xFBAD, 0xFBAB, 1}, {0x648, 0xFEED, 0xFEED, 0xFEEE, 0xFEEE, 0}, {0x6C7, 0xFBD7, 0xFBD7, 0xFBD8, 0xFBD8, 0}, {0x6C6, 0xFBD9, 0xFBD9, 0xFBDA, 0xFBDA, 0}, {0x6C8, 0xFBDB, 0xFBDB, 0xFBDC, 0xFBDC, 0}, {0x6CB, 0xFBDE, 0xFBDE, 0xFBDF, 0xFBDF, 0}, {0x6D0, 0xFBE4, 0xFBE6, 0xFBE7, 0xFBE5, 1}, {0x649, 0xFEEF, 0xFBE8, 0xFBE9, 0xFEF0, 1}, {0x64A, 0xFEF1, 0xFEF3, 0xFEF4, 0xFEF2, 1}}
    reg_str string = "([\u0626-\u06d5]+)";
)

/**
* 基本区   转换   扩展区
* @param source
* 要转换的内容
* @return 
* 已转换的内容
*/
func (u UyghurCharUtils) Basic2Extend(source string) string {
    reg := regexp.MustCompile(reg_str)
    ret := reg.ReplaceAllStringFunc(source,func(str string) string{
    returns := ""
    var target,target2 rune
    runes := []rune(str)
    var ch, p int
    length := len(runes)
    if length > 1 {
        target = runes[0]
        ch = u.getCode(target, 2)
        returns += string(rune(ch))
        for i := 0; i < length - 2; i++ {
            target = runes[i]
            target2 = runes[i + 1]
            p = u.getCode(target,5)
            ch = u.getCode(target2,p + 2)
            returns += string(rune(ch))
        }
        target = runes[length - 2]
        target2 = runes[length - 1]
        p = u.getCode(target, 5) * 3
        ch = u.getCode(target2, 1 + p)
        returns += string(rune(ch))
    } else {
        ch = u.getCode(runes[0], 1)
        returns += string(rune(ch))
    }
        return returns
    })
    return ret
}

/**
* 基本区  转换   反向扩展区
* @param source
* 要转换的内容
* @return
* 已转换的内容
*/
func (u UyghurCharUtils) Basic2RExtend(source string) string {
    thisText := u.Basic2Extend(source)
    reverseString := u.reverseString(thisText)
    return u.reverseAscii(reverseString)
}
/**
* 扩展区   转换   基本区
* @param source
* 要转换的内容
* @return 
* 已转换的内容
*/
func (u UyghurCharUtils) Extend2Basic(source string) string {
    var ch rune
    target := ""
    source = u.basicLa(source)
    runes := []rune(source)
    for i := 0; i < len(runes); i++ {
        ch = runes[i]
        target += string(u.getCode(ch, 0))
    }
    return target
}
/**
* 反向扩展区   转换   基本区
* @param source
* 要转换的内容
* @return
* 已转换的内容
*/
func (u UyghurCharUtils) RExtend2Basic(source string) string {
    target := u.reverseAscii(source)
    target = u.reverseString(target)
    target = u.Extend2Basic(target)
    return target
}

func (u UyghurCharUtils) getCode(source rune,index int) int {
    if index > 5 {
        return int(source)
    }
    for i := 0; i < 33; i++ {
        code := int(source)
        if (code == U[i][0] || code == U[i][1] || code == U[i][2] || code == U[i][3] || code == U[i][4]) {
            return U[i][index]
        }
    }
    return int(source)
}

func (u UyghurCharUtils) reverseString(source string) string {
    runes := []rune(source)
    for from, to := 0, len(runes)-1; from < to; from, to = from+1, to-1 {
        runes[from], runes[to] = runes[to], runes[from]
    }
    return string(runes)
}

func (u UyghurCharUtils) reverseAscii(source string) string {
    reg := regexp.MustCompile("([^\uFB00-\uFEFF\\s]+)")
    ret := reg.ReplaceAllStringFunc(source,func(str string) string{
        return u.reverseString(str)
    })
    return ret
}

func (u UyghurCharUtils) basicLa(source string) string {
    reg1 := regexp.MustCompile("(\uFEFB)")
    reg2 := regexp.MustCompile("(\uFEFC)")
    return reg2.ReplaceAllString(reg1.ReplaceAllString(source,"\uFEFB"),"\uFEFC")
}

func (u UyghurCharUtils) extendLa(source string) string {
    reg1 := regexp.MustCompile("(\uFEDF\uFE8E)")
    reg2 := regexp.MustCompile("(\uFEE0\uFE8E)")
    return reg2.ReplaceAllString(reg1.ReplaceAllString(source,"\u0644\u0627"),"\u0644\u0627")
}