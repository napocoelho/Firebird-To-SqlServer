

Public Class SqlExpressionException
    Inherits Exception

    Public Property Expression As String

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Public Sub New(message As String, expression As String)
        MyBase.New(message)
        Me.Expression = expression
    End Sub

End Class

