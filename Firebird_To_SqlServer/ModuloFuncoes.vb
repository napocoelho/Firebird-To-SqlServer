Imports DllUtilitaria.Database

Module ModuloFuncoes

    Public ConnectionFb As ConnectionManagerFirebird
    Public ConnectionSql As ConnectionManagerSqlServer



    Public Sub CriarConexoes(connectionStringFirebird As String, connectionStringSqlServer As String)

        Dim StrConexao As String = String.Empty
        Dim CommandsList As System.Collections.Generic.List(Of String) = Nothing


        '-------------- Configurando Firebird --------------


        '---> ConnectionString exemplo: "User=SYSDBA;Password=masterkey;Database=d:\teste\cadastro.gdb;DataSource=localhost;Port=3050;Dialect=3;"
        'StrConexao = "User=SYSDBA;Password=masterkey;Database=" & Application.StartupPath & "\importacao.gdb;DataSource=localhost;Port=3050;Dialect=3;"
        'StrConexao = "User=SYSDBA;Password=masterkey;Database=c:\testes\DBGOVVAL.GDB;DataSource=localhost;Port=3050;Dialect=3;"

        Try
            If Not ConnectionManagerFirebird.GetInstance Is Nothing Then
                ConnectionManagerFirebird.GetInstance.Dispose()
            End If

            ConnectionFb = ConnectionManagerFirebird.CreateInstance(connectionStringFirebird)
        Catch ex As Exception
            Throw New Exception("Firebird exception: " & ex.Message, ex)
        End Try

        





        '-------------- Configurando SqlServer --------------


        Try
            CommandsList = New System.Collections.Generic.List(Of String)
            CommandsList.Add("SET LANGUAGE 'Português (Brasil)'")
            CommandsList.Add("SET LOCK_TIMEOUT 5000")

            'StrConexao = "Initial Catalog=IMPORTACAO_GOVALPAN;" & _
            '             "Data Source=SERVIDOR;" & _
            '             "User ID=preview;" & _
            '             "Password=919985;" & _
            '             "Connect Timeout=0;" & _
            '             "Application Name='ImportacaoGovalpan'"

            If Not ConnectionManagerSqlServer.GetInstance Is Nothing Then
                ConnectionManagerSqlServer.GetInstance.Dispose()
            End If

            ConnectionSql = ConnectionManagerSqlServer.CreateInstance(connectionStringSqlServer)
        Catch ex As Exception
            Throw New Exception("SqlServer exception: " & ex.Message, ex)
        End Try
    End Sub

End Module
