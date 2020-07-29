Imports System
Imports System.Text
Imports System.Text.RegularExpressions
'  +----------------------------------------------------------------------
'  | Update: 2020-06-26 00:00
'  +----------------------------------------------------------------------
'  | Author: Kerindax <1482152356@qq.com>
'  +----------------------------------------------------------------------
Namespace Uyghur

    Public Class CharUtils
        ' 双目字对象
        Private Class SpecialObject
            Public Property basic As Object
            Public Property extend As Object
            Public Property link As Object
        End Class

        Private Const BASIC As Integer = 0 '基本区形式  A
        Private Const ALONE As Integer = 1 '单独形式    A
        Private Const HEAD As Integer = 2 '头部形式     A_
        Private Const CENTR As Integer = 3 '中部形式   _A_
        Private Const REAR As Integer = 4 '后部形式    _A

        Private Const convertRang As String = "[\u0622-\u064a\u0675-\u06d5]+"
        Private Const suffixRang As String = "[^\u0627\u062F-\u0632\u0648\u0688-\u0699\u06C0-\u06CB\u06D5]"
        Private Const extendRang As String = "[\ufb50-\ufdff\ufe70-\ufeff]"
        Private Const notExtendRang As String = "[^\ufb50-\ufdff\ufe70-\ufeff\s]+(\s[^\ufb50-\ufdff\ufe70-\ufeff\s]+)*"

        '特助转换区，扩展区反向转换的时候需要替换
        Private Const symbolRang As String = "[\)\(\]\[\}\{\>\<\»\«]"
        Private symbolList As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
            {")", "("},
            {"(", "("},
            {"]", "["},
            {"[", "]"},
            {"}", "{"},
            {"{", "}"},
            {">", "<"},
            {"»", "«"},
            {"«", "»"}
        }
        '数字转换对应的字母
        Public Shared Function fromCharCode(ByVal number As Integer) As String
            Return ChrW(number)
        End Function
        ' 双字母列表
        Private special As ArrayList = New ArrayList() From {
            New SpecialObject() With {
                .basic = New Integer() {&H644, &H627},
                .extend = New Integer() {&HFEFC},
                .link = New Integer() {&HFEE0, &HFE8E}
            },
            New SpecialObject() With {
                .basic = {&H644, &H627},
                .extend = {&HFEFB},
                .link = {&HFEDF, &HFE8E}
            }
        }
        '单字母列表
        Private charCode As Dictionary(Of String, ArrayList) = New Dictionary(Of String, ArrayList)()
        Public Sub New()
            ' 基本码, 单独形式, 头部形式, 中部形式, 后部形式]
            ' [  A  ,     A   ,    A_   ,   _A_  ,   _A   ]
            For Each row In New Integer()() {
                New Integer() {&H626, &HFE8B, &HFE8B, &HFE8C, &HFE8C}, ' 1 --- 00-Hemze
                New Integer() {&H627, &HFE8D, &HFE8D, &HFE8E, &HFE8E}, ' 0 --- 01-a   
                New Integer() {&H6D5, &HFEE9, &HFEE9, &HFEEA, &HFEEA}, ' 0 --- 02-:e  
                New Integer() {&H628, &HFE8F, &HFE91, &HFE92, &HFE90}, ' 1 --- 03-b   
                New Integer() {&H67E, &HFB56, &HFB58, &HFB59, &HFB57}, ' 1 --- 04-p   
                New Integer() {&H62A, &HFE95, &HFE97, &HFE98, &HFE96}, ' 1 --- 05-t   
                New Integer() {&H62C, &HFE9D, &HFE9F, &HFEA0, &HFE9E}, ' 1 --- 06-j   
                New Integer() {&H686, &HFB7A, &HFB7C, &HFB7D, &HFB7B}, ' 1 --- 07-q   
                New Integer() {&H62E, &HFEA5, &HFEA7, &HFEA8, &HFEA6}, ' 1 --- 08-h   
                New Integer() {&H62F, &HFEA9, &HFEA9, &HFEAA, &HFEAA}, ' 0 --- 09-d   
                New Integer() {&H631, &HFEAD, &HFEAD, &HFEAE, &HFEAE}, ' 0 --- 10-r   
                New Integer() {&H632, &HFEAF, &HFEAF, &HFEB0, &HFEB0}, ' 0 --- 11-z   
                New Integer() {&H698, &HFB8A, &HFB8A, &HFB8B, &HFB8B}, ' 0 --- 12-:zh 
                New Integer() {&H633, &HFEB1, &HFEB3, &HFEB4, &HFEB2}, ' 1 --- 13-s   
                New Integer() {&H634, &HFEB5, &HFEB7, &HFEB8, &HFEB6}, ' 1 --- 14-x   
                New Integer() {&H63A, &HFECD, &HFECF, &HFED0, &HFECE}, ' 1 --- 15-:gh 
                New Integer() {&H641, &HFED1, &HFED3, &HFED4, &HFED2}, ' 1 --- 16-f   
                New Integer() {&H642, &HFED5, &HFED7, &HFED8, &HFED6}, ' 1 --- 17-:k  
                New Integer() {&H643, &HFED9, &HFEDB, &HFEDC, &HFEDA}, ' 1 --- 18-k   
                New Integer() {&H6AF, &HFB92, &HFB94, &HFB95, &HFB93}, ' 1 --- 19-g   
                New Integer() {&H6AD, &HFBD3, &HFBD5, &HFBD6, &HFBD4}, ' 1 --- 20-:ng 
                New Integer() {&H644, &HFEDD, &HFEDF, &HFEE0, &HFEDE}, ' 1 --- 21-l   
                New Integer() {&H645, &HFEE1, &HFEE3, &HFEE4, &HFEE2}, ' 1 --- 22-m   
                New Integer() {&H646, &HFEE5, &HFEE7, &HFEE8, &HFEE6}, ' 1 --- 23-n   
                New Integer() {&H6BE, &HFBAA, &HFBAC, &HFBAD, &HFBAB}, ' 1 --- 24-:h  
                New Integer() {&H648, &HFEED, &HFEED, &HFEEE, &HFEEE}, ' 0 --- 25-o   
                New Integer() {&H6C7, &HFBD7, &HFBD7, &HFBD8, &HFBD8}, ' 0 --- 26-u   
                New Integer() {&H6C6, &HFBD9, &HFBD9, &HFBDA, &HFBDA}, ' 0 --- 27-:o  
                New Integer() {&H6C8, &HFBDB, &HFBDB, &HFBDC, &HFBDC}, ' 0 --- 28-v   
                New Integer() {&H6CB, &HFBDE, &HFBDE, &HFBDF, &HFBDF}, ' 0 --- 29-w   
                New Integer() {&H6D0, &HFBE4, &HFBE6, &HFBE7, &HFBE5}, ' 1 --- 30-e   
                New Integer() {&H649, &HFEEF, &HFBE8, &HFBE9, &HFEF0}, ' 1 --- 31-i   
                New Integer() {&H64A, &HFEF1, &HFEF3, &HFEF4, &HFEF2}, ' 1 --- 32-y 
                New Integer() {&H6C5, &HFBE0, &HFBE0, &HFBE1, &HFBE1}, ' 0 --- kz o_
                New Integer() {&H6C9, &HFBE2, &HFBE2, &HFBE3, &HFBE3}, ' 0 --- kz o^
                New Integer() {&H62D, &HFEA1, &HFEA3, &HFEA4, &HFEA2}, ' 1 --- kz h
                New Integer() {&H639, &HFEC9, &HFECB, &HFECC, &HFECA}  ' 1 --- kz c
            }
                Dim list As ArrayList = New ArrayList()

                For Each el In row
                    list.Add(fromCharCode(el))
                Next

                For Each item As String In list

                    If Not charCode.ContainsKey(item) Then
                        charCode.Add(item, list)
                    End If
                Next
            Next
        End Sub
        ''' <summary>
        ''' 基本区   转换   扩展区
        ''' </summary>
        ''' <param name="source">要转换的内容，可以包含混合字符串</param>
        ''' <returns>已转换的内容</returns>
        Public Function Basic2Extend(ByVal source As String) As String
            Return New Regex(convertRang).Replace(source,
                Function(word)
                    Dim returns As String =
                        New Regex("\s(\S)(\S)").Replace( '中部字母-前部有尾
                        New Regex("\s(\S)$").Replace( '最后字母-前部有尾
                        New Regex("\s(\S)\s").Replace( '中部字母-前后有尾
                        New Regex("(\S)(\S)\s").Replace( '中部字母-后部有尾
                        New Regex("^(\S)\s").Replace( '首字母-后部有尾
                        New Regex("(\S)(\S)(\S)").Replace( '中部字母-没有尾
                        New Regex("(\S)(\S)$").Replace( '最后字母-没有尾
                        New Regex("^(\S)(\S)").Replace( '首字母-没有尾
                        New Regex("^(\S)$").Replace( '单字母
                        New Regex(suffixRang).Replace(word.Value,
                        Function(ch)
                            Return ch.Value & "  "
                        End Function).Trim(),
                        Function(ch)
                            Return getChar(ch.Result("$1"), ALONE) '单字母
                        End Function),
                        Function(ch)
                            Return Me.getChar(ch.Result("$1"), ALONE) & ch.Result("$2") '首字母-没有尾
                        End Function),
                        Function(ch)
                            Return ch.Result("$1") & Me.getChar(ch.Result("$2"), ALONE) '最后字母-没有尾
                        End Function),
                        Function(ch)
                            Return ch.Result("$1") & Me.getChar(ch.Result("$2"), ALONE) & ch.Result("$3") '中部字母-没有尾
                        End Function),
                        Function(ch)
                            Return Me.getChar(ch.Result("$1"), HEAD) '首字母-后部有尾
                        End Function),
                        Function(ch)
                            Return ch.Result("$1") & Me.getChar(ch.Result("$2"), HEAD) '中部字母-后部有尾
                        End Function),
                        Function(ch)
                            Return Me.getChar(ch.Result("$1"), CENTR) '中部字母-前后有尾
                        End Function),
                        Function(ch)
                            Return Me.getChar(ch.Result("$1"), REAR) '最后字母-前部有尾
                        End Function),
                        Function(ch)
                            Return Me.getChar(ch.Result("$1"), REAR) & ch.Result("$2") '中部字母-前部有尾
                        End Function)
                    Return Me.extendLa(returns)
                End Function)
        End Function
        ''' <summary>
        ''' 扩展区   转换   基本区
        ''' </summary>
        ''' <param name="source">要转换的内容</param>
        ''' <returns>已转换的内容</returns>
        Public Function Extend2Basic(ByVal source As String) As String
            Return New Regex(extendRang).Replace(Me.basicLa(source), Function(ch) Me.getChar(ch.Value, BASIC))
        End Function
        ''' <summary>
        ''' 基本区  转换   反向扩展区
        ''' </summary>
        ''' <param name="source">要转换的内容</param>
        ''' <returns>已转换的内容</returns>
        Public Function Basic2RExtend(ByVal source As String) As String
            Return Me.reverseAscii(Me.reverseSubject(Me.Basic2Extend(source)))
        End Function
        ''' <summary>
        ''' 反向扩展区   转换   基本区
        ''' </summary>
        ''' <param name="source">要转换的内容</param>
        ''' <returns>已转换的内容</returns>
        Public Function RExtend2Basic(ByVal source As String) As String
            Return Me.Extend2Basic(Me.reverseSubject(Me.reverseAscii(source)))
        End Function
        ''' <summary>
        ''' Ascii区反转
        ''' </summary>
        Private Function reverseAscii(ByVal source As String) As String
            Return New Regex(notExtendRang).Replace(source,
                Function(word)
                    Return New Regex(symbolRang).Replace(StrReverse(word.Value),
                        Function(ch)
                            If symbolList.ContainsKey(ch.Value) Then
                                Return symbolList(ch.Value).ToString()
                            Else
                                Return ch.Value
                            End If
                        End Function)
                End Function)
        End Function
        ''' <summary>
        ''' 对象反转
        ''' </summary>
        Private Function reverseSubject(ByVal str As String) As String
            Return New Regex(".+").Replace(str, Function(subject) StrReverse(subject.Value))
        End Function
        ''' <summary>
        ''' 获取对应字母
        ''' </summary>
        Private Function getChar(ByVal ch As String, ByVal index As Integer) As String
            If charCode.ContainsKey(ch) Then
                Return charCode(ch)(index).ToString()
            Else
                Return ch
            End If
        End Function
        ''' <summary>
        ''' La字母转换扩展区
        ''' </summary>
        Private Function extendLa(ByVal source As String) As String
            For Each item As SpecialObject In Me.special
                source = source.Replace(Me.getString(item.link), Me.getString(item.extend))
            Next
            Return source
        End Function
        ''' <summary>
        ''' La字母转换基本区
        ''' </summary>
        Private Function basicLa(ByVal source As String) As String
            For Each item As SpecialObject In Me.special
                source = source.Replace(Me.getString(item.extend), Me.getString(item.basic))
            Next
            Return source
        End Function
        ''' <summary>
        ''' 双目字母转换字符串
        ''' </summary>
        Public Function getString(value As Integer()) As String
            Dim sb As StringBuilder = New StringBuilder()
            For Each item As Integer In value
                sb.Append(fromCharCode(item))
            Next
            Return sb.ToString()
        End Function
    End Class
End Namespace
