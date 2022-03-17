Imports System.Reflection
Imports System.Xml.Serialization


Public Class Configuracao

    Public Property FirebirdUser As String
    Public Property FirebirdPassword As String
    Public Property FirebirdDatabase As String
    Public Property FirebirdDatasource As String
    Public Property FirebirdPort As String
    Public Property FirebirdDialect As String

    Public Property SqlserverUser As String
    Public Property SqlserverPassword As String
    Public Property SqlserverDatabase As String
    Public Property SqlserverDatasource As String
    Public Property SqlserverTimeout As String

    Public Sub New()
        Me.FirebirdUser = String.Empty
        Me.FirebirdPassword = String.Empty
        Me.FirebirdDatabase = String.Empty
        Me.FirebirdDatasource = String.Empty
        Me.FirebirdPort = String.Empty
        Me.FirebirdDialect = String.Empty

        Me.SqlserverUser = String.Empty
        Me.SqlserverPassword = String.Empty
        Me.SqlserverDatabase = String.Empty
        Me.SqlserverDatasource = String.Empty
        Me.SqlserverTimeout = String.Empty
    End Sub


    Public Sub Save2(path As String)

        Dim stringWriter = New System.IO.StringWriter
        Dim serializer = New XmlSerializer(Me.GetType())

        Dim stream As New System.IO.FileStream(path, IO.FileMode.OpenOrCreate)
        Dim binWriter As New System.IO.BinaryWriter(stream)

        Dim encrypted As Byte()

        Try
            serializer.Serialize(stringWriter, Me)
            encrypted = AesCryp.Encrypt(stringWriter.ToString, "laga", "laga")
            stream.Write(encrypted, 0, encrypted.Count)

        Catch ex As Exception
            Throw ex
        Finally
            Try
                stream.Flush()
                stream.Close()
                stream.Dispose()
            Catch ex2 As Exception
            End Try
        End Try

    End Sub

    Public Sub Save(path As String)

        Dim text As System.IO.TextWriter
        Dim serializer = New XmlSerializer(Me.GetType())

        Dim stream As New System.IO.FileStream(path, IO.FileMode.OpenOrCreate)
        Dim binWriter As New System.IO.BinaryWriter(stream)

        Dim encrypted As Byte()

        Try
            text = New System.IO.StringWriter(New System.Text.StringBuilder(""))
            serializer.Serialize(text, Me)
            encrypted = AesCryp.Encrypt(text.ToString, "laga", "laga")
            stream.Write(encrypted, 0, encrypted.Count)
        Catch ex As Exception
            Throw ex
        Finally
            Try
                stream.Flush()
                stream.Close()
                stream.Dispose()
            Catch ex2 As Exception
            End Try
        End Try

    End Sub

    Public Sub Load(path As String)

        Dim loadedConf As New Configuracao
        Dim text As System.IO.TextReader

        Dim serializer = New XmlSerializer(Me.GetType())

        Dim stream As New System.IO.FileStream(path, IO.FileMode.Open)
        Dim binReader As New System.IO.BinaryReader(stream)

        Dim encrypted As New List(Of Byte)
        Dim decrypted As String

        Try
            Dim pos, count As Integer

            pos = 0
            count = binReader.BaseStream.Length

            While pos < count
                encrypted.Add(binReader.ReadByte)
                pos += System.Runtime.InteropServices.Marshal.SizeOf((New Byte).GetType)
            End While

            decrypted = AesCryp.Decrypt(encrypted.ToArray, "laga", "laga")

            text = New System.IO.StringReader(decrypted)
            loadedConf = serializer.Deserialize(text)


            For Each item In Me.GetType.GetProperties
                item.SetValue(Me, item.GetValue(loadedConf, Nothing), Nothing)
            Next

        Catch ex As Exception
            Throw ex
        Finally
            Try
                stream.Close()
                stream.Dispose()
            Catch ex2 As Exception
            End Try
        End Try

    End Sub


    Public Function GetFirebirdConnectionString() As String

        Dim ListaParametros As New Dictionary(Of String, String)

        ListaParametros.Add("User", Me.FirebirdUser)
        ListaParametros.Add("Password", Me.FirebirdPassword)
        ListaParametros.Add("Database", Me.FirebirdDatabase)
        ListaParametros.Add("DataSource", Me.FirebirdDatasource)
        ListaParametros.Add("Port", Me.FirebirdPort)
        ListaParametros.Add("Dialect", Me.FirebirdDialect)

        Return ListaParametros.RemoveEmptyValues().JoinWith("=", ";") & ";"

    End Function

    Public Function GetSqlServerConnectionString() As String

        Dim ListaParametros As New Dictionary(Of String, String)

        ListaParametros.Add("Initial Catalog", Me.SqlserverDatabase)
        ListaParametros.Add("Data Source", Me.SqlserverDatasource)
        ListaParametros.Add("User ID", Me.SqlserverUser)
        ListaParametros.Add("Password", Me.SqlserverPassword)
        ListaParametros.Add("Connect Timeout", Me.SqlserverTimeout)
        ListaParametros.Add("Application Name", "Firebird_To_SqlServer")

        Return ListaParametros.RemoveEmptyValues().JoinWith("=", ";") & ";"

    End Function



End Class


