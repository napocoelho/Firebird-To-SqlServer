Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic
Imports Microsoft.Win32

Module ModuloExtensionMethods


    '************************* EXTENSION METHODS *******************************

    ''' <summary>
    ''' Verifica se valor é nulo ou vazio.
    ''' </summary>
    ''' <returns>Retorna verdadeiro se valor for Null (Nothing) ou Empty.</returns>
    <Extension()>
    Public Function IsEmpty(ByVal SelfObj As String) As Boolean

        Return String.IsNullOrEmpty(SelfObj)
    End Function

    ''' <summary>
    ''' Retorna um valor DEFAULT caso o valor da string seja EMPTY.
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <param name="DefaultValue">Valor padrão a ser retornado</param>
    ''' <returns>Retorna uma string</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function DefaultIfEmpty(ByVal SelfObj As String, ByVal DefaultValue As String) As String


        Return IIf(String.IsNullOrEmpty(SelfObj), DefaultValue, SelfObj)
    End Function

    ''' <summary>
    ''' Retorna um valor DEFAULT caso o valor da string seja EMPTY.
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <param name="DefaultValue">Valor padrão a ser retornado</param>
    ''' <returns>Retorna uma string</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ReturnsDefaultIfEmpty(ByVal SelfObj As String, ByVal DefaultValue As String) As String

        Return IIf(String.IsNullOrEmpty(SelfObj), DefaultValue, SelfObj)
    End Function

    ''' <summary>
    ''' Obtém uma quantidade de caracteres à esquerda.
    ''' </summary>
    ''' <param name="IntCorte">Quantidade de caracteres.</param>
    ''' <returns>Retorna sequência de caracteres indicada.</returns>
    <Extension()>
    Public Function TakeLeft(ByVal SelfObj As String, ByVal IntCorte As Integer) As String

        Return Microsoft.VisualBasic.Left(SelfObj, IntCorte)
    End Function

    ''' <summary>
    ''' Obtém uma quantidade de caracteres à direita.
    ''' </summary>
    ''' <param name="IntCorte">Quantidade de caracteres.</param>
    ''' <returns>Retorna sequência de caracteres indicada.</returns>
    <Extension()>
    Public Function TakeRight(ByVal SelfObj As String, ByVal IntCorte As Integer) As String
        Return Microsoft.VisualBasic.Right(SelfObj, IntCorte)
    End Function

    <Extension()>
    Public Function FormatTo(ByVal SelfObj As String, ByVal ParamArray args() As Object) As String
        Return String.Format(SelfObj, args)
    End Function

    <Extension()>
    Public Function FormatTo(ByVal SelfObj As String, ByVal provider As System.IFormatProvider, ByVal ParamArray args() As Object) As String
        Return String.Format(provider, SelfObj, args)
    End Function

    ''' <summary>
    ''' Adiciona aspas simples à string.
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <returns>Retorna a string com aspas simples</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function Aspa(ByVal SelfObj As String) As String
        Return Chr(39) & SelfObj & Chr(39)
    End Function

    ''' <summary>
    ''' Adiciona aspas duplas à string.
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function Aspas(ByVal SelfObj As String) As String
        Return Chr(34) & SelfObj & Chr(34)
    End Function

    ''' <summary>
    ''' Adiciona colchetes à string.
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <returns>Retorna a string com colchetes</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function Colchetes(ByVal SelfObj As String) As String
        Return "[" & SelfObj & "]"
    End Function

    ''' <summary>
    ''' Adiciona espaços ao redor da string.
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <returns>Retorna a string com espaços nas extremidades</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function Espaços(ByVal SelfObj As String) As String
        Return " " & SelfObj & " "
    End Function


    ''' <summary>
    ''' Adiciona parenteses à string.
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <returns>Retorna a string com parenteses</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function Parenteses(ByVal SelfObj As String) As String
        Return "(" & SelfObj & ")"
    End Function

    ''' <summary>
    ''' Replica a string uma quantidade de vezes determinada por parâmetro
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <param name="times">Quantidade de vezes ao qual será replicado o texto</param>
    ''' <returns>Retorna string replicada</returns>
    <Extension()>
    Public Function Replicate(ByVal SelfObj As String, ByVal times As Integer) As String

        Dim replicated As New System.Text.StringBuilder("")

        For index As Integer = 1 To times
            replicated.Append(SelfObj)
        Next

        Return replicated.ToString
    End Function

    ''' <summary>
    ''' O mesmo que String.Join(separator, array()). Funciona apenas com List(of String).
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <param name="separator"></param>
    ''' <returns>Retorna uma string contendo o conteúdo de cada elemento do array especificado, separados pelo argumento "separator".</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function JoinWith(ByVal SelfObj As List(Of String), ByVal separator As String) As String

        Return String.Join(separator, SelfObj.ToArray)
    End Function

    ''' <summary>
    ''' O mesmo que String.Join(separator, array()). Funciona apenas com Dictionary(of String, String).
    ''' </summary>
    ''' <returns>Retorna uma string contendo o conteúdo de cada elemento do array especificado, separados pelo argumento "separator".</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function JoinWith(ByVal SelfObj As Dictionary(Of String, String), ByVal KeyValueSeparator As String, ByVal PairSeparator As String) As String

        Dim Lista As New List(Of String)

        For Each pair As KeyValuePair(Of String, String) In SelfObj
            Lista.Add(pair.Key & KeyValueSeparator & pair.Value)
        Next

        Return Lista.JoinWith(PairSeparator)
    End Function
   
    <Extension()>
    Public Function RemoveEmptyValues(ByVal SelfObj As Dictionary(Of String, String)) As Dictionary(Of String, String)

        Dim tempList As New Dictionary(Of String, String)

        For Each pair As KeyValuePair(Of String, String) In SelfObj
            tempList.Add(pair.Key, pair.Value)
        Next

        For Each pair As KeyValuePair(Of String, String) In tempList
            If pair.Value.Trim.IsEmpty Then
                SelfObj.Remove(pair.Key)
            End If
        Next

        tempList.Clear()

        Return SelfObj
    End Function

    ''' <summary>
    ''' O mesmo que String.Join(separator, array()).
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <param name="separator"></param>
    ''' <returns>Retorna uma string contendo o conteúdo de cada elemento do array especificado, separados pelo argumento "separator".</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function JoinWith(ByVal SelfObj As String(), ByVal separator As String) As String

        Return String.Join(separator, SelfObj)
    End Function

    ''' <summary>
    ''' Retorna a parte chave (nome) de um "Enum" instanciado.
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <returns>Retorna nome/chave da instância de Enum</returns>
    <Extension()>
    Public Function GetName(ByVal SelfObj As System.Enum) As String

        Return [Enum].GetName(SelfObj.GetType, SelfObj)
    End Function

    ''' <summary>
    ''' Retorna a parte chave (nome) de um "Enum" instanciado.
    ''' </summary>
    ''' <param name="SelfObj"></param>
    ''' <returns>Retorna nome/chave da instância de Enum</returns>
    <Extension()>
    Public Function TakeLeft(ByRef SelfObj As List(Of String), ByVal count As Integer) As List(Of String)

        Dim retorno As New List(Of String)

        For Idx As Integer = 1 To count
            retorno.Add(SelfObj(Idx - 1))
        Next

        Return retorno
    End Function

    '************************* EXTENSION METHODS *******************************

End Module
