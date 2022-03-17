


Public Class Importador

    Private InfoAction As Action(Of ImportadorInfo) = Nothing
    Private InfoNovoErroEncontradoAction As Action(Of ProblemaEncontrado) = Nothing

    Public Sub New()

    End Sub

    Private Sub PrepararBanco()

        Dim StrSql As String = String.Empty
        'Dim TblTables As DataTable

        Try
            StrSql = "DROP VIEW [VIEW_SCHEMA_ALL_COLUMNS]"
            ConnectionSql.ExecuteNonQuery(StrSql)
        Catch ex As Exception
        End Try

        Try
            StrSql = "" & _
                "CREATE VIEW [VIEW_SCHEMA_ALL_COLUMNS] AS " & _
                "SELECT SCHEMA_NAME(UID)             AS SCHEMA_NAME , " & _
                "       sysobjects.ID                AS TABLE_ID    , " & _
                "       sysobjects.name              AS TABLE_NAME  , " & _
                "       COLUMNS_1.name               AS COLUMN_NAME , " & _
                "       COLUMNS_1.COLID              AS COLUMN_ORDER, " & _
                "       TYPE_NAME(COLUMNS_1.xtype)   AS TYPE_NAME   , " & _
                "       Collation                                   , " & _
                "       CollationId                                 , " & _
                "       Prec AS PRECISION                           , " & _
                "       COLUMNS_1.Scale                             , " & _
                "       IsNullable                                  , " & _
                "       IsComputed                                  , " & _
                "       Is_RowGuidCol AS IsRowGuidCol               , " & _
                "       Is_Identity   AS IsIdentity " & _
                "FROM   sysobjects " & _
                "       INNER JOIN syscolumns AS COLUMNS_1 " & _
                "       ON     COLUMNS_1.Id = sysobjects.id " & _
                "       INNER JOIN sys.columns AS COLUMNS_2 " & _
                "       ON     COLUMNS_2.object_id = COLUMNS_1.id " & _
                "       AND    COLUMNS_2.column_id = COLUMNS_1.colid " & _
                "WHERE  sysobjects.XTYPE           = 'U'"

            ConnectionSql.ExecuteNonQuery(StrSql)
        Catch ex As Exception
            Throw New Exception("Não foi possível criar a view [VIEW_SCHEMA_ALL_COLUMNS]!" & vbNewLine & "Mensagem: " & ex.Message)
        End Try



        'Construindo tabelas:
        'StrSql = "SELECT DISTINCT RDB$RELATION_NAME AS RELATION FROM RDB$Relations AS TABLES WHERE RDB$SYSTEM_FLAG = 0 AND RDB$VIEW_BLR IS NULL ORDER BY RDB$RELATION_NAME"
        'TblTables = ConnectionFb.ExecuteDataTable(StrSql)


        'Construindo tabelas:
        For Each tabela As String In GetNomeDasTabelasDoFirebird()
            Try
                DestruirTabela(tabela)
                ConstruirTabela(tabela)
            Catch ex As Exception
                Throw ex
            End Try
        Next



        'For Each Row As DataRow In TblTables.Rows
        '    Dim tabela As String = Row("Relation").ToString.Trim

        '    Try
        '        DestruirTabela(tabela)
        '        ConstruirTabela(tabela)
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'Next


    End Sub

    Private Function GetNomeDasTabelasDoFirebird() As List(Of String)
        Dim StrSql As String
        Dim TblTables As DataTable
        Dim ListaDeTabelas As New List(Of String)

        StrSql = "SELECT DISTINCT RDB$RELATION_NAME AS RELATION FROM RDB$Relations AS TABLES WHERE RDB$SYSTEM_FLAG = 0 AND RDB$VIEW_BLR IS NULL ORDER BY RDB$RELATION_NAME"
        TblTables = ConnectionFb.ExecuteDataTable(StrSql)

        For Each xRow As DataRow In TblTables.Rows
            Dim nomeDeTabela As String = xRow("RELATION").ToString.Trim

            If Not String.IsNullOrEmpty(nomeDeTabela) Then
                ListaDeTabelas.Add(nomeDeTabela)
            End If
        Next

        Return ListaDeTabelas
    End Function

    Private Sub DestruirTabela(ByVal StrNomeTabela As String)
        Try
            ConnectionSql.ExecuteNonQuery("DROP TABLE " & StrNomeTabela.Trim.Colchetes)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ConstruirTabela(ByVal StrNomeTabela As String)

        Dim StrSql As String
        Dim TblFbSchema As DataTable = Nothing
        Dim FieldsList As New List(Of String)

        StrNomeTabela = StrNomeTabela.Trim

        StrSql = "" & _
                "SELECT   RDB$Relation_Name              AS TableName , " & _
                "         RELATION_FIELDS.RDB$Field_Name AS FieldName , " & _
                "         RDB$Type_Name                  AS TypeName  , " & _
                "         RDB$Field_Length               AS FieldLen  , " & _
                "         RDB$Field_Scale                AS FieldScale, " & _
                "         RDB$Character_Length           AS CharLen " & _
                "FROM     RDB$Relation_Fields            AS RELATION_FIELDS " & _
                "         INNER JOIN RDB$Fields          AS FIELDS " & _
                "         ON       RELATION_FIELDS.RDB$FIELD_SOURCE = FIELDS.RDB$FIELD_NAME " & _
                "         INNER JOIN RDB$Types AS TYPES " & _
                "         ON       FIELDS.RDB$FIELD_TYPE    = TYPES.RDB$TYPE " & _
                "WHERE    RELATION_FIELDS.RDB$RELATION_NAME =  " & StrNomeTabela.Aspa & _
                "AND      TYPES.RDB$FIELD_NAME              = 'RDB$FIELD_TYPE' " & _
                "ORDER BY RELATION_FIELDS.RDB$FIELD_POSITION"

        TblFbSchema = ConnectionFb.ExecuteDataTable(StrSql)

        'Criando configuração de campos:
        For Each Row As DataRow In TblFbSchema.Rows

            Dim fieldName, typeName As String
            Dim scale, fieldLen As Integer
            Dim fieldDeclaration As String = String.Empty

            fieldName = Row("FieldName").ToString.Trim
            scale = Math.Abs(CInt(Row("FieldScale")))
            fieldLen = CInt(Row("FieldLen"))
            typeName = Row("TypeName").Trim


            If {"FLOAT", "DOUBLE"}.Contains(typeName) Then

                fieldDeclaration = "REAL" '"DECIMAL ( 33, 11 )"

            ElseIf {"LONG", "SHORT", "INT64"}.Contains(typeName) Then

                If scale > 0 Then

                    fieldDeclaration = "DECIMAL ( 30, " & scale & " )"

                Else

                    If fieldLen <= 2 Then
                        fieldDeclaration = "SMALLINT"
                    ElseIf fieldLen <= 4 Then
                        fieldDeclaration = "INT"
                    Else    ' 8 BITs
                        fieldDeclaration = "BIGINT"
                    End If

                End If

            ElseIf {"TEXT", "VARYING", "CSTRING", "QUAD"}.Contains(typeName) And fieldLen <= 8000 Then

                fieldDeclaration = "VARCHAR ( " & fieldLen & " )"

            ElseIf {"TEXT", "VARYING", "CSTRING", "QUAD"}.Contains(typeName) And fieldLen > 8000 Then

                fieldDeclaration = "TEXT"

            ElseIf {"TIMESTAMP", "DATE", "TIME"}.Contains(typeName) Then

                fieldDeclaration = "DATETIME"

            ElseIf {"BLOB"}.Contains(typeName) Then
                fieldDeclaration = "TEXT"
            Else

                Throw New Exception("Tipo não encontrado!")
            End If


            FieldsList.Add(fieldName.Colchetes & Space(1) & fieldDeclaration)
        Next


        'Montando script SQL para criar tabela:
        StrSql = "CREATE TABLE " & StrNomeTabela & " (" & _
                    vbNewLine & vbTab & _
                    FieldsList.JoinWith(", " & vbNewLine & vbTab) & _
                    vbNewLine & " )"


        'Clipboard.Clear()
        'Clipboard.SetText(StrSql)

        'Criando tabela:
        ConnectionSql.ExecuteNonQuery(StrSql)

    End Sub




    Private Sub ImportarTabela(ByVal StrNomeTabela As String)

        Dim TblSchema As DataTable
        Dim TblFbSource As DataTable
        Dim StrSql As String


        Dim ListaParametros As New List(Of String)
        Dim ListaTuplas As New List(Of List(Of String))

        Dim StrColumnName, StrTypeName, StrValor As String
        Dim IsNullable As Boolean


        StrValor = String.Empty

        'Obtendo schema da tabela do SqlServer:
        StrSql = "select * from [VIEW_SCHEMA_ALL_COLUMNS] where [TABLE_NAME] = " & StrNomeTabela.Aspa & " order by COLUMN_ORDER"
        TblSchema = ConnectionSql.ExecuteDataTable(StrSql)

        'Preparando informação para ser inserida no SqlServer:
        For Each Row As DataRow In TblSchema.Rows
            ListaParametros.Add(Row("COLUMN_NAME").ToString.Colchetes)
        Next

        'SqlInsertFixo = "INSERT INTO " & StrNomeTabela.Colchetes & " " & ListaParametros.JoinWith(", ").Espaços.Parenteses & " SELECT "
        'SqlInsertFixo = "INSERT INTO " & StrNomeTabela.Colchetes & " " & ListaParametros.JoinWith(", ").Espaços.Parenteses


        'Obtendo registros da tabela do Firebird:
        StrSql = "select * from " & StrNomeTabela
        TblFbSource = ConnectionFb.ExecuteDataTable(StrSql)




        'Preparando 2a parte (VALUES) do insert:
        For Each RowValores As DataRow In TblFbSource.Rows

            Dim ListaRegistros As New List(Of String)

            'Obtendo os valores de cada registro (cada linha):
            For Each RowSchema As DataRow In TblSchema.Rows

                StrColumnName = RowSchema("COLUMN_NAME").ToString
                StrTypeName = RowSchema("TYPE_NAME").ToString.ToUpper
                IsNullable = RowSchema("IsNullable")


                StrValor = RowValores(StrColumnName).ToString

                'If StrColumnName = "PEDIDO" Then
                '    If StrValor = "98033966" Then
                '        Stop
                '    End If
                'End If

                'Try
                '    StrValor = RowValores(StrColumnName).ToString
                'Catch ex As Exception
                '    ListaParametros.Remove(StrColumnName.Colchetes)
                '    SqlInsertFixo = "INSERT INTO " & StrNomeTabela.Colchetes & " " & ListaParametros.JoinWith(", ").Espaços.Parenteses & " SELECT "
                '    Continue For
                'End Try


                If {"CHAR", "NCHAR", "VARCHAR", "NVARCHAR", "TEXT", "NTEXT", "VARBINARY", "DATE", "DATETIME", "SMALLDATETIME"}.Contains(StrTypeName) Then

                    If IsNullable And StrValor.IsEmpty Then
                        StrValor = "Null"
                    Else
                        StrValor = StrValor.Replace("'", "''").Aspa
                    End If

                Else    'INT,DECIMAL,SMALLINT

                    If Not Decimal.TryParse(StrValor, New Decimal) Then
                        StrValor = 0
                    End If


                    If IsNullable And StrValor.IsEmpty Then
                        StrValor = "Null"
                    Else
                        StrValor = StrValor.Replace(",", ".")
                    End If

                End If

                'ListaRegistros.Add(StrValor & " AS " & StrColumnName.Colchetes)
                ListaRegistros.Add(StrValor)

            Next

            'Construindo inserts:
            'ListaTuplas.Add(SqlInsertFixo & ListaRegistros.JoinWith(", "))
            ListaTuplas.Add(ListaRegistros)
        Next



        'Executando inserts:
        Try
            Dim CachedInsert As New SqlServerCachedInsert(StrNomeTabela.Trim.Colchetes, 100, ListaParametros)



            ConnectionSql.ExecuteNonQuery("delete from " & StrNomeTabela.Trim.Colchetes)

            ConnectionSql.BeginTransaction()

            For Each tuple As List(Of String) In ListaTuplas
                CachedInsert.ExecuteTuple(tuple)
            Next

            CachedInsert.Flush()

            ConnectionSql.CommitTransaction()

        Catch ex As Exception
            ConnectionSql.RollbackTransaction()
            Throw New SqlExpressionException(ex.Message, StrSql)
        End Try

    End Sub

    Private Sub Importar_e_RecuperarTabela(ByVal StrNomeTabela As String)

        Dim TblSchema As DataTable
        Dim TblFbSource As DataTable
        Dim StrSql As String


        Dim ListaParametros As New List(Of String)
        Dim ListaTuplas As New List(Of List(Of String))

        Dim StrColumnName, StrTypeName, StrValor As String
        Dim IsNullable As Boolean


        StrValor = String.Empty

        'Obtendo schema da tabela do SqlServer:
        StrSql = "select * from [VIEW_SCHEMA_ALL_COLUMNS] where [TABLE_NAME] = " & StrNomeTabela.Aspa & " order by COLUMN_ORDER"
        TblSchema = ConnectionSql.ExecuteDataTable(StrSql)

        'Preparando informação para ser inserida no SqlServer:
        For Each Row As DataRow In TblSchema.Rows
            ListaParametros.Add(Row("COLUMN_NAME").ToString.Colchetes)
        Next

        'SqlInsertFixo = "INSERT INTO " & StrNomeTabela.Colchetes & " " & ListaParametros.JoinWith(", ").Espaços.Parenteses & " SELECT "
        'SqlInsertFixo = "INSERT INTO " & StrNomeTabela.Colchetes & " " & ListaParametros.JoinWith(", ").Espaços.Parenteses


        'Obtendo registros da tabela do Firebird:
        StrSql = "select * from " & StrNomeTabela
        'TblFbSource = ConnectionFb.ExecuteDataTable(StrSql)



        Dim StrFields As String

        Try
            StrFields = String.Join(", ", ListaParametros.Skip(2).Take(106))
            StrFields = StrFields.Replace("[", "").Replace("]", "")


            StrSql = "select " & StrFields & " from " & StrNomeTabela
            StrSql = "select PRO_COD_BARRAS from " & StrNomeTabela

            TblFbSource = ConnectionFb.ExecuteDataTable(StrSql)



            Stop
        Catch ex As Exception
            Dim teste As String
            teste = ex.Message
        End Try





        'Preparando 2a parte (VALUES) do insert:
        For Each RowValores As DataRow In TblFbSource.Rows

            Dim ListaRegistros As New List(Of String)

            'Obtendo os valores de cada registro (cada linha):
            For Each RowSchema As DataRow In TblSchema.Rows

                StrColumnName = RowSchema("COLUMN_NAME").ToString
                StrTypeName = RowSchema("TYPE_NAME").ToString.ToUpper
                IsNullable = RowSchema("IsNullable")


                StrValor = RowValores(StrColumnName).ToString

                'If StrColumnName = "PEDIDO" Then
                '    If StrValor = "98033966" Then
                '        Stop
                '    End If
                'End If

                'Try
                '    StrValor = RowValores(StrColumnName).ToString
                'Catch ex As Exception
                '    ListaParametros.Remove(StrColumnName.Colchetes)
                '    SqlInsertFixo = "INSERT INTO " & StrNomeTabela.Colchetes & " " & ListaParametros.JoinWith(", ").Espaços.Parenteses & " SELECT "
                '    Continue For
                'End Try


                If {"CHAR", "NCHAR", "VARCHAR", "NVARCHAR", "TEXT", "NTEXT", "VARBINARY", "DATE", "DATETIME", "SMALLDATETIME"}.Contains(StrTypeName) Then

                    If IsNullable And StrValor.IsEmpty Then
                        StrValor = "Null"
                    Else
                        StrValor = StrValor.Replace("'", "''").Aspa
                    End If

                Else    'INT,DECIMAL,SMALLINT

                    If Not Decimal.TryParse(StrValor, New Decimal) Then
                        StrValor = 0
                    End If


                    If IsNullable And StrValor.IsEmpty Then
                        StrValor = "Null"
                    Else
                        StrValor = StrValor.Replace(",", ".")
                    End If

                End If

                'ListaRegistros.Add(StrValor & " AS " & StrColumnName.Colchetes)
                ListaRegistros.Add(StrValor)

            Next

            'Construindo inserts:
            'ListaTuplas.Add(SqlInsertFixo & ListaRegistros.JoinWith(", "))
            ListaTuplas.Add(ListaRegistros)
        Next



        'Executando inserts:
        Try
            Dim CachedInsert As New SqlServerCachedInsert(StrNomeTabela.Trim.Colchetes, 100, ListaParametros)



            ConnectionSql.ExecuteNonQuery("delete from " & StrNomeTabela.Trim.Colchetes)

            ConnectionSql.BeginTransaction()

            For Each tuple As List(Of String) In ListaTuplas
                CachedInsert.ExecuteTuple(tuple)
            Next

            CachedInsert.Flush()

            ConnectionSql.CommitTransaction()

        Catch ex As Exception
            ConnectionSql.RollbackTransaction()
            Throw New SqlExpressionException(ex.Message, StrSql)
        End Try

    End Sub

    Public Sub Importar()

        Dim TblSchema As DataTable
        Dim StrSql As String
        Dim ListaTabelasDoFirebird As List(Of String)
        Dim ListaTabelasComProblemas As New System.Collections.Generic.List(Of ProblemaEncontrado)


        'Criando views:
        Me.PrepararBanco()

        'Obtendo schema da tabela do SqlServer:
        StrSql = "select DISTINCT [TABLE_NAME] from [VIEW_SCHEMA_ALL_COLUMNS] order by [TABLE_NAME]"
        TblSchema = ConnectionSql.ExecuteDataTable(StrSql)
        ListaTabelasDoFirebird = GetNomeDasTabelasDoFirebird()


        ' where TABLE_NAME = 'ITENSPEDIDO'


        For Each Row As DataRow In TblSchema.Rows
            Dim info As New ImportadorInfo
            Dim TabelaDoSqlServer As String

            TabelaDoSqlServer = Row("TABLE_NAME").ToString.Trim()

            'Verifica se contém o nome da tabela nos 2 bancos de dados:
            If ListaTabelasDoFirebird.Contains(TabelaDoSqlServer) Then

                Try
                    info.Tabela = TabelaDoSqlServer
                    info.TotalDeRegistros = ConnectionFb.ExecuteScalar("SELECT COUNT(*) FROM " & info.Tabela)

                    If Not InfoAction Is Nothing Then _
                        InfoAction.DynamicInvoke(info)

                    Me.ImportarTabela(info.Tabela)
                Catch ex As Exception
                    Dim problema As New ProblemaEncontrado
                    problema.Tabela = TabelaDoSqlServer
                    problema.Mensagem = ex.Message

                    If Not InfoNovoErroEncontradoAction Is Nothing Then _
                        InfoNovoErroEncontradoAction.DynamicInvoke(problema)
                End Try


            End If
        Next

    End Sub

    Public Sub Info(action As Action(Of ImportadorInfo))
        InfoAction = action
    End Sub

    Public Sub InfoNovoErroEncontrado(action As Action(Of ProblemaEncontrado))
        InfoNovoErroEncontradoAction = action
    End Sub



End Class


Public Class ImportadorInfo

    Public Property Tabela As String
    Public Property TotalDeRegistros As Integer
    Public Property ProblemasEncontrados

End Class


Public Class ProblemaEncontrado
    Public Property Tabela As String
    Public Property Mensagem As String
End Class

Public Class SqlServerCachedInsert

    Private Shadows _Params As List(Of String)
    Private Shadows _CurrentTuples As Queue(Of List(Of String))


    Public Property CacheSize As Integer
    Public Property Table As String

    Public Property Params As List(Of String)
        Get
            Return Me._Params
        End Get

        Private Set(value As List(Of String))
            Me._Params = value
        End Set
    End Property

    Public Property CurrentTuples As Queue(Of List(Of String))
        Get
            Return Me._CurrentTuples
        End Get

        Private Set(value As Queue(Of List(Of String)))
            Me._CurrentTuples = value
        End Set
    End Property



    Public Sub New(ByVal tableName As String, ByVal cacheSize As Integer)
        Me.CacheSize = cacheSize
        Me.Params = New List(Of String)
        Me.CurrentTuples = New Queue(Of List(Of String))
        Me.Table = tableName
    End Sub

    Public Sub New(ByVal tableName As String, ByVal cacheSize As Integer, ByRef params As List(Of String))
        Me.Table = tableName
        Me.CacheSize = cacheSize
        Me.Params = params
        Me.CurrentTuples = New Queue(Of List(Of String))
    End Sub




    Public Sub SetParameters(ByRef params() As String)
        Me.SetParameters(params.ToList)
    End Sub

    Public Sub SetParameters(ByVal params As String)
        Dim newList As New List(Of String)

        For Each param As String In params.Split({","}, StringSplitOptions.None).ToList()
            newList.Add(param.Trim)
        Next

        Me.SetParameters(newList)
    End Sub

    Public Sub SetParameters(ByRef params As List(Of String))

        Dim newList As New List(Of String)

        params.ForEach(Sub(item)
                           newList.Add(item.ToString)
                       End Sub)

        Me.Params = newList
    End Sub





    Public Sub ExecuteTuple(ByRef tuple() As String)
        Me.ExecuteTuple(tuple.ToList)
    End Sub

    Public Sub ExecuteTuple(ByVal tuple As String)
        Dim newList As New List(Of String)

        For Each value As String In tuple.Split({","}, StringSplitOptions.None).ToList()
            newList.Add(value.Trim)
        Next

        Me.ExecuteTuple(newList)
    End Sub

    Public Sub ExecuteTuple(ByRef tuple As List(Of String))

        Dim newList As New List(Of String)

        tuple.ForEach(Sub(item)
                          newList.Add(item.ToString)
                      End Sub)

        Me.CurrentTuples.Enqueue(newList)
        AnalizeAndExecute()
    End Sub

    Private Shadows Sub AnalizeAndExecute()
        If CurrentTuples.Count >= CacheSize Then
            Flush()
        End If
    End Sub

    Public Sub Flush()

        Dim queries As New List(Of String)
        Dim strSql As String = String.Empty
        Dim strParams As String = String.Empty
        Dim strTuples As String = String.Empty

        If CurrentTuples.Count > 0 Then

            strParams = Me.Params.JoinWith(", ")

            While CurrentTuples.Count > 0
                Dim tuple As List(Of String)
                tuple = CurrentTuples.Dequeue

                queries.Add(tuple.JoinWith(", ").Espaços.Parenteses)
            End While

            strTuples = queries.JoinWith(", ")

            strSql = "INSERT INTO {0} ( {1} ) VALUES {2}".FormatTo(Me.Table, strParams, strTuples)

            Try
                DllUtilitaria.Database.ConnectionManagerSqlServer.GetInstance.ExecuteNonQuery(strSql)
                
            Catch ex As Exception
                Clipboard.Clear()
                Clipboard.SetText(strSql)
                Dim xxx As String = ex.Message
            End Try

            
        End If

    End Sub

End Class


