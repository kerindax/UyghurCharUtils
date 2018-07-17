Imports System.Text
Imports System.Text.RegularExpressions

Public Class UyghurCharUtils
    Private U = {{&H626, &HFE8B, &HFE8B, &HFE8C, &HFE8C, 1}, {&H627, &HFE8D, &HFE8D, &HFE8E, &HFE8E, 0}, {&H6D5, &HFEE9, &HFEE9, &HFEEA, &HFEEA, 0}, {&H628, &HFE8F, &HFE91, &HFE92, &HFE90, 1}, {&H67E, &HFB56, &HFB58, &HFB59, &HFB57, 1}, {&H62A, &HFE95, &HFE97, &HFE98, &HFE96, 1}, {&H62C, &HFE9D, &HFE9F, &HFEA0, &HFE9E, 1}, {&H686, &HFB7A, &HFB7C, &HFB7D, &HFB7B, 1}, {&H62E, &HFEA5, &HFEA7, &HFEA8, &HFEA6, 1}, {&H62F, &HFEA9, &HFEA9, &HFEAA, &HFEAA, 0}, {&H631, &HFEAD, &HFEAD, &HFEAE, &HFEAE, 0}, {&H632, &HFEAF, &HFEAF, &HFEB0, &HFEB0, 0}, {&H698, &HFB8A, &HFB8A, &HFB8B, &HFB8B, 0}, {&H633, &HFEB1, &HFEB3, &HFEB4, &HFEB2, 1}, {&H634, &HFEB5, &HFEB7, &HFEB8, &HFEB6, 1}, {&H63A, &HFECD, &HFECF, &HFED0, &HFECE, 1}, {&H641, &HFED1, &HFED3, &HFED4, &HFED2, 1}, {&H642, &HFED5, &HFED7, &HFED8, &HFED6, 1}, {&H643, &HFED9, &HFEDB, &HFEDC, &HFEDA, 1}, {&H6AF, &HFB92, &HFB94, &HFB95, &HFB93, 1}, {&H6AD, &HFBD3, &HFBD5, &HFBD6, &HFBD4, 1}, {&H644, &HFEDD, &HFEDF, &HFEE0, &HFEDE, 1}, {&H645, &HFEE1, &HFEE3, &HFEE4, &HFEE2, 1}, {&H646, &HFEE5, &HFEE7, &HFEE8, &HFEE6, 1}, {&H6BE, &HFBAA, &HFBAC, &HFBAD, &HFBAB, 1}, {&H648, &HFEED, &HFEED, &HFEEE, &HFEEE, 0}, {&H6C7, &HFBD7, &HFBD7, &HFBD8, &HFBD8, 0}, {&H6C6, &HFBD9, &HFBD9, &HFBDA, &HFBDA, 0}, {&H6C8, &HFBDB, &HFBDB, &HFBDC, &HFBDC, 0}, {&H6CB, &HFBDE, &HFBDE, &HFBDF, &HFBDF, 0}, {&H6D0, &HFBE4, &HFBE6, &HFBE7, &HFBE5, 1}, {&H649, &HFEEF, &HFBE8, &HFBE9, &HFEF0, 1}, {&H64A, &HFEF1, &HFEF3, &HFEF4, &HFEF2, 1}}
    Public Sub New()
    End Sub
    '基本区   转换   扩展区
    Public Function Basic2Extend(source As String) As String
        Dim reg1 As New Regex("([\u0600-\u06FF]+)")
        Return reg1.Replace(source,
            Function(word)
                Dim str = word.Value
                Dim returns As String = ""
                Dim target As String = ""
                Dim target2 As String = ""
                Dim ch As String
                Dim p As Integer
                Dim length As Integer = str.Length
                If length > 1 Then
                    target = str.Substring(0, 1)
                    ch = _GetCode(target, 2)
                    returns &= ChrW(ch)
                    For i = 0 To length - 3
                        target = str.Substring(i, 1)
                        target2 = str.Substring(i + 1, 1)
                        p = _GetCode(target, 5)
                        ch = _GetCode(target2, 2 + p)
                        returns &= ChrW(ch)
                    Next
                    target = str.Substring(length - 2, 1)
                    target2 = str.Substring(length - 1, 1)
                    p = _GetCode(target, 5) * 3
                    ch = _GetCode(target2, 1 + p)
                    returns &= ChrW(ch)
                Else
                    ch = _GetCode(str, 1)
                    returns &= ChrW(ch)
                End If
                Return _ExtendLa(returns.Trim())
            End Function)
    End Function
    '基本区  转换   反向扩展区
    Public Function Basic2RExtend(source As String) As String
        Dim ThisText = Basic2Extend(source)
        Dim ReverseString = _ReverseString(ThisText)
        Return _ReverseAscii(ReverseString)
    End Function
    '扩展区   转换   基本区
    Public Function Extend2Basic(source As String) As String
        Dim i, ch
        Dim target = ""
        source = _BasicLa(source)
        For i = 0 To source.Length - 1
            ch = source.Substring(i, 1)
            target += _GetCode(ch, 0)
        Next
        Return target
    End Function
    '反向扩展区   转换   基本区
    Function GetSyllable(Str As String) As List(Of String)
        Str = Trim(Str)
        Dim List As New List(Of String)
        Dim Sozuk As String = "اەوۇۆۈېىئ"

        Dim ch As Char
        Dim Str2 As String = Str
        Dim Check As Boolean = False
        For i = Str.Length - 1 To 0 Step -1
            ch = Str.Substring(i, 1)

            If ch = "ئ" Then
                List.Insert(0, Str2.Substring(i))
                Str2 = Str.Substring(0, i)
                Continue For
            End If

            If Sozuk.Contains(Str.Substring(i, 1)) AndAlso i > 0 AndAlso Not Sozuk.Contains(Str.Substring(i - 1, 1)) Then
                Check = False
                For Each Item As Char In Str.Substring(0, i - 1)
                    If Sozuk.Contains(Item) Then
                        Check = True
                        Exit For
                    End If
                Next

                If Check Then
                    List.Insert(0, Str2.Substring(i - 1))
                    Str2 = Str.Substring(0, i - 1)
                    i -= 1
                Else
                    List.Insert(0, Str2)
                    Exit For
                End If

            End If

            If i = 0 AndAlso Trim(Str2) <> "" Then
                List.Insert(0, Str2)
            End If
        Next

        Return List
    End Function
    Public Function RExtend2Basic(source As String) As String
        Dim target = _ReverseAscii(source)
        target = _ReverseString(target)
        target = Extend2Basic(target)
        Return target
    End Function
    Private Function _ReverseAscii(source As String) As String
        Dim reg1 As New Regex("([^\uFB00-\uFEFF\s]+)")
        Return reg1.Replace(source, Function(word)
                                        Return _ReverseString(word.Value.ToString())
                                    End Function)
    End Function
    Private Function _ReverseString(source As String) As String ' 反转
        Return StrReverse(source)
    End Function
    Private Function _ExtendLa(source As String) As String
        Dim reg1 As New Regex("(\uFEDF\uFE8E)")
        Dim reg2 As New Regex("(\uFEE0\uFE8E)")
        Return reg2.Replace(reg1.Replace(source,
                           Function(word)
                               Return ChrW(&HFEFB)
                           End Function),
                           Function(word)
                               Return ChrW(&HFEFC)
                           End Function)
    End Function
    Private Function _BasicLa(source As String) As String
        Dim reg1 As New Regex("(\uFEFB)")
        Dim reg2 As New Regex("(\uFEFC)")
        Return reg2.Replace(reg1.Replace(source,
                           Function(word)
                               Return ChrW(&HFEDF) & ChrW(&HFE8E)
                           End Function),
                           Function(word)
                               Return ChrW(&HFEE0) & ChrW(&HFE8E)
                           End Function)
    End Function
    Private Function _GetCode(source As String, index As Integer) As Integer
        Dim target As Integer = 0
        If source.Length = 0 Then Return 0
        If index > 5 Then Return AscW(source)
        For i = 0 To 32
            If AscW(source) = U(i, 0) Then
                Return U(i, index)
            End If
        Next
        Return AscW(source)
    End Function
End Class
