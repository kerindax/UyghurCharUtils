Module demo
    '  +----------------------------------------------------------------------
    '  | Update: 2020-01-17 13:54
    '  +----------------------------------------------------------------------
    '  | Author: Kerindax <1482152356@qq.com>
    '  +----------------------------------------------------------------------
    Sub Main()
        Dim utils As New UyghurCharUtils
        Dim source As String = "سالام VB.NET"

        Dim target1 As String = utils.Basic2Extend(source) '基本区 转换 扩展区
        Dim target2 As String = utils.Extend2Basic(target1) '扩展区 转换 基本区

        Dim target3 As String = utils.Basic2RExtend(source) '基本区 转换 反向扩展区
        Dim target4 As String = utils.RExtend2Basic(target3) '反向扩展区 转换 基本区
        MsgBox(target1 + vbCrLf + target2 + vbCrLf + target3 + vbCrLf + target4)
    End Sub

End Module
