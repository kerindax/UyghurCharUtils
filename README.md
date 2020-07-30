# UyghurCharUtils

### 项目介绍
##### 维吾尔语，哈萨克语，柯尔克孜语基本区和扩展区转换函数类库
##### v1版本已完成的语言：javascript，php，csharp，vb.net，mysql，java，golang
##### v1版本地址：https://gitee.com/kerindax/UyghurCharUtils/tree/v1.x/
##### v2版本项目地址：https://gitee.com/kerindax/UyghurCharUtils

### 贡献者
Kerindax，Sherer，Bulut
### 使用说明

1. Basic2Extend(source){}       基本区 转换 扩展区
2. Extend2Basic(source){}       扩展区 转换 基本区
3. Basic2RExtend(source){}      基本区 转换 反向扩展区
4. RExtend2Basic(source){}      反向扩展区 转换 基本区

##### 克隆仓库
`git clone git@gitee.com:kerindax/UyghurCharUtils.git`

##### 安装依赖
`npm install uyghur-char-utils`

##### CDN 引入
```html
<script src="https://unpkg.com/uyghur-char-utils"></script>
```

### 例子

- nodejs

```js
  const UyghurCharUtils = require("uyghur-char-utils");
  var utils = new UyghurCharUtils();
  var source = "سالام JS";

  var target1 = utils.Basic2Extend(source); //基本区 转换 扩展区
  var target2 = utils.Extend2Basic(target1); //扩展区 转换 基本区

  var target3 = utils.Basic2RExtend(source); //基本区 转换 反向扩展区
  var target4 = utils.RExtend2Basic(target3); //反向扩展区 转换 基本区

  console.log(target1);
  console.log(target2);

  console.log(target3);
  console.log(target4);
```
- browser

```html
<!DOCTYPE html>
<html>
    <head>
        <title>Hello JS</title>
    </head>
    <body>
        <script
        type="text/javascript"
        src="https://unpkg.com/uyghur-char-utils"
        charset="utf-8"
        ></script>
        <script>
        var utils = new UyghurCharUtils();
        var source = "سالام JS";

        var target1 = utils.Basic2Extend(source); //基本区 转换 扩展区
        var target2 = utils.Extend2Basic(target1); //扩展区 转换 基本区

        var target3 = utils.Basic2RExtend(source); //基本区 转换 反向扩展区
        var target4 = utils.RExtend2Basic(target3); //反向扩展区 转换 基本区

        console.log(target1);
        console.log(target2);

        console.log(target3);
        console.log(target4);
        </script>
    </body>
</html>
```
- php
```php
	header("Content-type: text/html; charset=utf-8");
	require_once "UyghurCharUtils.php";
  $utils = new UyghurCharUtils();
  $source = "سالام PHP";

  $target1 = $utils->Basic2Extend($source);//基本区 转换 扩展区
  $target2 = $utils->Extend2Basic($target1);//扩展区 转换 基本区

  $target3 = $utils->Basic2RExtend($source);//基本区 转换 反向扩展区
  $target4 = $utils->RExtend2Basic($target3);//反向扩展区 转换 基本区
	
	echo $target1."<br/>";
	echo $target2."<br/>";

	echo $target3."<br/>";
	echo $target4."<br/>";
```
- c#
```c#
static void Main(string[] args)
{
  Uyghur.CharUtils utils = new Uyghur.CharUtils();
  string source = "سالام C#";
  string target1 = utils.Basic2Extend(source);//基本区 转换 扩展区
  string target2 = utils.Extend2Basic(target1);//扩展区 转换 基本区

  string target3 = utils.Basic2RExtend(source);//基本区 转换 反向扩展区
  string target4 = utils.RExtend2Basic(target3);//反向扩展区 转换 基本区

  MessageBox.Show(target1 + "\n" + target2 + "\n" + target3 + "\n" + target4);
}
```
- vb.net
```vb
Sub Main()
    Dim utils As New Uyghur.CharUtils
    Dim source As String = "سالام VB.NET"

    Dim target1 As String = utils.Basic2Extend(source) '基本区 转换 扩展区
    Dim target2 As String = utils.Extend2Basic(target1) '扩展区 转换 基本区

    Dim target3 As String = utils.Basic2RExtend(source) '基本区 转换 反向扩展区
    Dim target4 As String = utils.RExtend2Basic(target3) '反向扩展区 转换 基本区
    MsgBox(target1 + vbCrLf + target2 + vbCrLf + target3 + vbCrLf + target4)
End Sub
```
- java
```java
public class demo {
    public static void main(String[] args) {
        UyghurCharUtils utils = new UyghurCharUtils();
        String source = "سالام Java";

        String target1 = utils.Basic2Extend(source);//基本区 转换 扩展区
        String target2 = utils.Extend2Basic(target1);//基本区 转换 扩展区

        String target3 = utils.Basic2RExtend(source);//基本区 转换 扩展区
        String target4 = utils.RExtend2Basic(target3);//基本区 转换 扩展区
        System.out.println(target1 + "\n" + target2 + "\n" + target3 + "\n" + target4);
    }
}
```