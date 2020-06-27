package main
import (
    "fmt"
)
func main() {
	utils := &UyghurCharUtils{}
	source := "سالام go"
	target1 := utils.Basic2Extend(source)	//基本区 转换 扩展区
	target2 := utils.Extend2Basic(target1)	//扩展区 转换 基本区
	target3 := utils.Basic2RExtend(source)	//基本区 转换 反向扩展区
	target4 := utils.RExtend2Basic(target3)	//反向扩展区 转换 基本区
	fmt.Printf("%s\n%s\n%s\n%s\n",target1,target2,target3,target4)
}