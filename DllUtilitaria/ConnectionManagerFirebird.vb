
Imports FirebirdSql.Data.FirebirdClient
Imports System.Collections.Generic
Imports System.Threading



Namespace Database

    ''' <summary>
    ''' Gerencia conexões primárias com o banco de dados, encapsulando as classes IDbTransaction, FbConnection e FbDataReader, SqlDataTable.
    ''' Simplifica o uso de transações e ainda cria conexões independentes para cada thread ativa que utilizar ConnectionManager, evitando
    ''' bloqueios e falhas ao fazer dar comandos no banco de dados. Não é necessário criar novas instâncias de ConnectionManager quando estiver 
    ''' em diferentes threads. Além de tudo, os métodos de ConnectionManager são ThreadSafe.
    ''' </summary>
    Public Class ConnectionManagerFirebird


        Private Shared Shadows _selfInstance As ConnectionManagerFirebird = Nothing


        Private Shadows DictConexoesAsync As New Dictionary(Of Thread, FbConnection)
        Private Shadows DictTransacoesAsync As New Dictionary(Of Thread, FbTransaction)
        Private Shadows DictDataReadersAsync As New Dictionary(Of Thread, FbDataReader)
        Private Shadows DictTryCommitLevel As New Dictionary(Of Thread, Integer)    '--> importante não usar esta lista diretamente. Use a property TryCommitLevel, pois foi criada para isso.
        Private Shadows ListBloqueiosAsync As New List(Of Thread)



        Private Shadows ConnectionString As String
        Private Shadows StartingFbCommands As List(Of String)

        'O método [TryCommitTransaction] executará apenas se o atributo [IntTryCommitLevel] igual a ZERO.
        '* Lembrando que o método [CommitTransaction] não considera este atributo.
        'Private Shadows IntTryCommitLevel As Integer = 0


        Private Shadows Property TryCommitLevel As Integer
            Get
                SyncLock Thread.CurrentThread
                    If Not DictTryCommitLevel.ContainsKey(Thread.CurrentThread) Then _
                        DictTryCommitLevel.Add(Thread.CurrentThread, 0)

                    Return DictTryCommitLevel(Thread.CurrentThread)
                End SyncLock
            End Get
            Set(value As Integer)
                SyncLock Thread.CurrentThread
                    If Not DictTryCommitLevel.ContainsKey(Thread.CurrentThread) Then _
                        DictTryCommitLevel.Add(Thread.CurrentThread, 0)

                    DictTryCommitLevel(Thread.CurrentThread) = value
                End SyncLock
            End Set
        End Property


        ''' <summary>
        ''' Construtor de um singleton.
        ''' </summary>    
        Private Sub New(ByVal ConnectionString As String, Optional ByVal StartingFbCommands As List(Of String) = Nothing)

            If StartingFbCommands Is Nothing Then
                StartingFbCommands = New List(Of String)
            End If

            Me.ConnectionString = ConnectionString
            Me.StartingFbCommands = StartingFbCommands

        End Sub

        ''' <summary>
        ''' Inicializa o serviço e retorna uma única instância criada de ConnectionManager.
        ''' Este método segue o conceito de singleton.
        ''' </summary>
        ''' <param name="ConnectionString">String de conexão com o banco de dados (o mesmo usado para criar uma FbConnection).</param>
        ''' <param name="StartingFbCommands">Lista de comandos sql que serão executados ao iniciar a conexão com o banco de dados.</param>
        ''' <returns>Retorna uma instância de ConnectionManager.</returns>
        Public Shared Function CreateInstance(ByVal ConnectionString As String, Optional ByVal StartingFbCommands As List(Of String) = Nothing) As ConnectionManagerFirebird

            SyncLock "lock" '--> um mesmo lock para todas Threads

                If ConnectionManagerFirebird._selfInstance Is Nothing Then

                    ConnectionManagerFirebird._selfInstance = New ConnectionManagerFirebird(ConnectionString, StartingFbCommands)
                    ConnectionManagerFirebird._selfInstance.ExecuteNonQuery("select first 1 rdb$relation_name from rdb$relations")     'testando a conexão

                End If

            End SyncLock

            Return ConnectionManagerFirebird._selfInstance

        End Function

        ''' <summary>
        ''' Retorna uma única instância de ConnectionManager. 
        ''' </summary>
        ''' <returns>Se CreateInstance(...) já tiver sido chamado, retornará ConnectionManager. Caso contrário, NULL.</returns>
        Public Shared Function GetInstance() As ConnectionManagerFirebird

            Return ConnectionManagerFirebird._selfInstance

        End Function

        ''' <summary>
        ''' Cria e configura uma nova conexão (instância de FbConnection) com o banco de dados, através dos argumentos passados.
        ''' </summary>
        ''' <param name="ConnectionString">String de conexão com o banco de dados (o mesmo usado para criar uma FbConnection).</param>
        ''' <param name="StartingFbCommands">Lista de comandos sql que serão executados ao iniciar a conexão com o banco de dados.</param>
        ''' <returns>Retorna uma nova FbConnection</returns>
        Private Shadows Function CreateConnection(ByVal ConnectionString As String, Optional ByVal StartingFbCommands As List(Of String) = Nothing) As FbConnection

            Dim novaConexao As New FirebirdSql.Data.FirebirdClient.FbConnection

            Try
                novaConexao.ConnectionString = ConnectionString

                novaConexao.Open()

                If novaConexao.State = ConnectionState.Open And Not StartingFbCommands Is Nothing Then

                    For Each xSql As String In Me.StartingFbCommands

                        Dim Comando As New FirebirdSql.Data.FirebirdClient.FbCommand
                        Comando.CommandType = CommandType.Text
                        Comando.Connection = novaConexao
                        Comando.CommandText = xSql
                        Comando.CommandTimeout = 0
                        Comando.ExecuteNonQuery()
                        Comando.Dispose()
                    Next

                End If

            Catch Ex As System.Data.SqlClient.SqlException

                novaConexao.Dispose()

                Throw Ex

            End Try

            Return novaConexao

        End Function

        ''' <summary>
        ''' Retorna uma FbConnection distinta para cada Thread ativada.
        ''' Este método é ThreadSafe.
        ''' </summary>
        ''' <returns>Retorna uma FbConnection.</returns>
        Public Function GetCurrentConnection() As FbConnection

            Dim Conexao As FbConnection = Nothing

            SyncLock Thread.CurrentThread

                If Not Me.DictConexoesAsync.ContainsKey(Thread.CurrentThread) Then

                    Conexao = Me.CreateConnection(Me.ConnectionString, Me.StartingFbCommands)

                    Me.DictConexoesAsync.Add(Thread.CurrentThread, Conexao)
                End If

                Conexao = Me.DictConexoesAsync(Thread.CurrentThread)

            End SyncLock

            Return Conexao
        End Function

        ''' <summary>
        ''' Inicia e controla bloqueios de ConnectionManager.
        ''' </summary>
        ''' <remarks>
        ''' Gerencia os bloqueios para que seja criado apenas 1 por thread, tanto 
        ''' para Transactions quanto para DataReaders.
        ''' </remarks>
        Private Sub Bloquear()

            SyncLock Thread.CurrentThread

                If Not ListBloqueiosAsync.Contains(Thread.CurrentThread) Then

                    Monitor.Enter(Thread.CurrentThread)
                    ListBloqueiosAsync.Add(Thread.CurrentThread)
                End If
            End SyncLock
        End Sub

        ''' <summary>
        ''' Controla e finaliza bloqueios de ConnectionManager.
        ''' </summary>
        ''' <remarks>
        ''' Gerencia os bloqueios para que não finalize enquanto
        ''' existir DataReaders e/ou Transactions ativos. O último
        ''' que terminar - Transaction ou DataReader -, finalizará 
        ''' o bloqueio.
        ''' </remarks>
        Private Sub Desbloquear()

            SyncLock Thread.CurrentThread

                If ListBloqueiosAsync.Contains(Thread.CurrentThread) Then

                    If Not Me.HasActiveDataReader And Not Me.HasActiveTransaction Then

                        Monitor.Exit(Thread.CurrentThread)
                        ListBloqueiosAsync.Remove(Thread.CurrentThread)
                    End If
                End If
            End SyncLock
        End Sub


        ''' <summary>
        ''' Inicia uma transação com o banco de dados. 
        ''' Deve ser utilizado em conjunto os seguintes métodos: CommitTransaction() ou RollbackTransaction().
        ''' Este método é ThreadSafe.
        ''' </summary>
        Public Sub BeginTransaction(Optional ByVal IsolationLevelInfo = Nothing)

            '**** IMPORTANTE ****
            Me.Bloquear()

            Dim Transacao As IDbTransaction

            Me.TryCommitLevel = Me.TryCommitLevel + 1

            If Not Me.HasActiveTransaction Then

                If IsolationLevelInfo Is Nothing Then
                    Transacao = GetCurrentConnection.BeginTransaction()
                Else
                    Transacao = GetCurrentConnection.BeginTransaction(IsolationLevelInfo)
                End If

                DictTransacoesAsync.Add(Thread.CurrentThread, Transacao)
            End If

        End Sub

        ''' <summary>
        ''' Envia toda a sequência de comandos para o banco de dados, desde a chamada por BeginTransaction().
        ''' Deve ser utilizado em conjunto com o método BeginTransaction().
        ''' Este método é ThreadSafe.
        ''' </summary>
        Public Sub CommitTransaction()

            If Me.HasActiveTransaction Then


                TryCommitLevel = 0
                DictTransacoesAsync(Thread.CurrentThread).Commit()
                DictTransacoesAsync(Thread.CurrentThread).Dispose()
                DictTransacoesAsync.Remove(Thread.CurrentThread)


                '**** IMPORTANTE ****
                Me.Desbloquear()

            End If

        End Sub

        ''' <summary>
        ''' Executa [CommitTransaction] apenas quando o mesmo número de [BeginTransaction] e [TryCommitTransaction] forem chamados.
        ''' Este método é aconselhado para quando se deseja transferir a responsabilidade de execução da transação para fora
        ''' do escopo referido. Lembrando que [CommitTransaction] fará imediatamente a função proposta.
        ''' </summary>
        Public Sub TryCommitTransaction()

            Me.TryCommitLevel = IIf(Me.TryCommitLevel > 0, Me.TryCommitLevel - 1, 0)

            If Me.TryCommitLevel = 0 Then

                CommitTransaction()
            End If

        End Sub

        ''' <summary>
        ''' Cancela toda a sequência de comandos enviados para o banco de dados, desde a chamada por BeginTransaction().
        ''' Deve ser utilizado em conjunto com o método BeginTransaction().
        ''' Este método é ThreadSafe.
        ''' </summary>
        Public Sub RollbackTransaction()

            If Me.HasActiveTransaction Then

                Dim Transacao As IDbTransaction



                Transacao = DictTransacoesAsync(Thread.CurrentThread)
                'ConnectionManager.GetInstance.RollbackTransaction()
                Transacao.Rollback()
                Transacao.Dispose()
                TryCommitLevel = 0
                DictTransacoesAsync.Remove(Thread.CurrentThread)

                '**** IMPORTANTE ****
                Me.Desbloquear()

            End If

        End Sub



        '' O MÉTODO CLOSE() POSSUI UM BUG, POR ISSO FOI DESATIVADO. 
        '' AO FINALIZAR AS TRANSAÇÕES, CONEXÕES E ETC..., ELE NÃO CONSEGUE FECHAR BLOQUEIOS AINDA.
        '' PRECISA SER CRIADO UMA FORMA DE MAPEAR AS THREADS, POIS AÍ SIM, TERIA COMO LIBERAR TODOS 
        '' OS BLOQUEIOS, LIBERANDO ASSIM CADA THREAD INDIVIDUALMENTE.

        ' ''' <summary>
        ' ''' Fecha/cancela todas as transações, DataReaders, bloqueios e conexões criadas. 
        ' ''' Ao utilizar os métodos de consulta ao banco de dados (ConnectionManager.ExecuteDataReader, ConnectionManager.ExecuteNonQuery, ...),
        ' ''' as conexões serão recriadas automaticamente.
        ' ''' </summary>
        'Public Sub Close()

        '    'Cancelando transações:
        '    For Each xPair As KeyValuePair(Of Thread, IDbTransaction) In DictTransacoesAsync

        '        Try

        '            xPair.Value.Rollback()
        '        Finally
        '            xPair.Value.Dispose()
        '        End Try

        '    Next

        '    'Fechando DataReaders:
        '    For Each xPair As KeyValuePair(Of Thread, FbDataReader) In DictDataReadersAsync

        '        Try
        '            xPair.Value.Close()
        '        Finally
        '            xPair.Value.Dispose()
        '        End Try

        '    Next

        '    'Fechando conexões:
        '    For Each xPair As KeyValuePair(Of Thread, FbConnection) In DictConexoesAsync

        '        Try
        '            xPair.Value.Close()
        '        Finally
        '            xPair.Value.Dispose()
        '        End Try

        '    Next

        '    'Limpando collections:
        '    DictTransacoesAsync.Clear()
        '    DictConexoesAsync.Clear()
        '    DictDataReadersAsync.Clear()

        'End Sub


        '''' <summary>
        '''' Encerra a instância de ConnectionManager.
        '''' </summary>
        Public Sub Dispose()

            'Me.Close()
            _selfInstance = Nothing
            'Me.Dispose()
        End Sub

        ''' <summary>
        ''' Verifica se há alguma transação ativa que não tenha sido finalizada na Thread atual.
        ''' </summary>    
        Public ReadOnly Property HasActiveTransaction() As Boolean
            Get
                SyncLock Thread.CurrentThread
                    Return DictTransacoesAsync.ContainsKey(Thread.CurrentThread)
                End SyncLock
            End Get
        End Property


        ''' <summary>
        ''' Verifica se há algum DataReader que foi retornado e não foi finalizado na Thread atual.
        ''' </summary>    
        Public ReadOnly Property HasActiveDataReader() As Boolean
            Get
                SyncLock Thread.CurrentThread
                    Return Me.DictDataReadersAsync.ContainsKey(Thread.CurrentThread)
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Executa um comando em SQL.
        ''' </summary>
        ''' <param name="StrSql">Comando SQL válido.</param>
        ''' <returns>Retorna o número de registros afetados.</returns>
        Public Function ExecuteNonQuery(ByVal StrSql As String) As Integer

            Dim Comando As New FbCommand

            SyncLock Thread.CurrentThread

                Comando.CommandType = CommandType.Text
                Comando.Connection = Me.GetCurrentConnection()
                Comando.CommandText = StrSql
                Comando.CommandTimeout = 0

                If Me.HasActiveTransaction Then _
                    Comando.Transaction = DictTransacoesAsync(Thread.CurrentThread)

                ExecuteNonQuery = Comando.ExecuteNonQuery()
                Comando.Dispose()

            End SyncLock

        End Function

        ''' <summary>
        ''' Retorna um ou mais registros dado um comando SQL.
        ''' É importante que este método seja utilizado em conjunto com CloseDataReader() e, quando necessário, HasActiveDataReader.
        '''                                                              
        ''' Obs 1.: Aconselha-se o uso de ExecuteDataTable(...) ao invés deste, por questões de escalabilidade;
        ''' Obs 2.: Nenhum outro Comando SQL funcionará enquanto existir um DataReader ativo na conexão.
        ''' </summary>
        ''' <param name="StrSql">Comando SQL válido.</param>
        ''' <returns>O DataReader retornado segura a conexão com o banco de dados.</returns>
        Public Function ExecuteDataReader(ByVal StrSql As String) As FbDataReader

            '**** IMPORTANTE ****
            Me.Bloquear()


            Dim Comando As New FbCommand

            If Me.HasActiveDataReader Then
                Me.CloseDataReader()
            End If

            Comando.CommandType = CommandType.Text
            Comando.Connection = Me.GetCurrentConnection()
            Comando.CommandText = StrSql
            Comando.CommandTimeout = 0

            If Me.HasActiveTransaction Then _
                    Comando.Transaction = DictTransacoesAsync(Thread.CurrentThread)

            ExecuteDataReader = Comando.ExecuteReader()
            'Comando.Dispose()

            Me.DictDataReadersAsync.Add(Thread.CurrentThread, ExecuteDataReader)

        End Function

        ''' <summary>
        ''' Retorna um único valor, dado um comando SQL.
        ''' </summary>
        ''' <param name="StrSql">Comando SQL válido.</param>
        ''' <returns>Possivelmente um objeto contendo: texto, numero, data ou sequencia de bits.</returns>
        Public Function ExecuteScalar(ByVal StrSql As String) As Object

            Dim Comando As New FbCommand

            SyncLock Thread.CurrentThread

                Comando.CommandType = CommandType.Text
                Comando.Connection = Me.GetCurrentConnection()
                Comando.CommandText = StrSql
                Comando.CommandTimeout = 0

                If Me.HasActiveTransaction Then _
                    Comando.Transaction = DictTransacoesAsync(Thread.CurrentThread)

                ExecuteScalar = Comando.ExecuteScalar()
                Comando.Dispose()

            End SyncLock

        End Function


        ''' <summary>
        ''' Fecha o DataReader aberto por ExecuteDataReader(...).
        ''' </summary>
        Public Sub CloseDataReader()

            Try
                If DictDataReadersAsync.ContainsKey(Thread.CurrentThread) Then

                    DictDataReadersAsync(Thread.CurrentThread).Close()
                    DictDataReadersAsync(Thread.CurrentThread) = Nothing
                    DictDataReadersAsync.Remove(Thread.CurrentThread)


                    '**** IMPORTANTE ****
                    Me.Desbloquear()

                End If
            Finally

            End Try

        End Sub


        ''' <summary>
        ''' Retorna um ou mais registros dado um comando SQL
        ''' </summary>
        ''' <param name="StrSql">Comando SQL válido.</param>
        ''' <returns>O DataTable retornado não segura a conexão com o banco de dados.</returns>
        Public Function ExecuteDataTableWithConstraints(ByVal StrSql As String) As System.Data.DataTable
            'Public Function ExecuteDataTable(ByVal StrSql As String) As System.Data.DataTable

            SyncLock Thread.CurrentThread

                Dim DataReader_Aux As FbDataReader
                Dim xTabela As New System.Data.DataTable

                DataReader_Aux = Me.ExecuteDataReader(StrSql)


                If Not DataReader_Aux Is Nothing Then

                    xTabela.Load(DataReader_Aux)
                End If

                Call Me.CloseDataReader()

                Return xTabela

            End SyncLock

        End Function

        ''' <summary>
        ''' Retorna um ou mais registros dado um comando SQL, ignorando qualquer constraint.
        ''' * Obs.: é útil quando o SGBD não respeita as próprias constraints, como unique ou qualquer outro tipo.
        '''         As vezes, uma determinada coluna pode ser unique, mas por alguma falha interna, o SGBD mantém informações duplicadas.
        '''         Este método fará com que o DataTable ignorare qualquer constraint.
        ''' </summary>
        ''' <param name="StrSql">Comando SQL válido.</param>
        ''' <returns>O DataTable retornado não segura a conexão com o banco de dados.</returns>

        Public Function ExecuteDataTable(ByVal StrSql As String) As System.Data.DataTable
            'Public Function ExecuteDataTableIgnoringConstraints(ByVal StrSql As String) As System.Data.DataTable

            SyncLock Thread.CurrentThread

                Dim dtReader As FbDataReader

                dtReader = Me.ExecuteDataReader(StrSql)

                '-------------------------------------------------------------

                Dim dtSchemaTable As New System.Data.DataTable
                Dim dtOutputTable As New System.Data.DataTable
                Dim dtColumn As DataColumn
                Dim dtRow As DataRow

                dtSchemaTable = dtReader.GetSchemaTable()

                For i As Integer = 0 To (dtSchemaTable.Rows.Count - 1)
                    dtColumn = New DataColumn

                    If Not dtOutputTable.Columns.Contains(dtSchemaTable.Rows(i)("ColumnName").ToString()) Then
                        ' ColumnName, ColumnOrdinal, ColumnSize, NumericPrecision, NumericScale, 
                        ' DataType, ProviderType, IsLong, AllowDBNull, IsReadOnly, IsRowVersion, 
                        ' IsUnique, IsKey, IsAutoIncrement, IsAliased, IsExpression, BaseSchemaName, 
                        ' BaseCatalogName, BaseTableName, BaseColumnName

                        dtColumn.ColumnName = dtSchemaTable.Rows(i)("ColumnName").ToString()
                        dtColumn.Unique = False
                        dtColumn.AllowDBNull = True
                        dtColumn.DataType = dtSchemaTable.Rows(i)("DataType")
                        dtColumn.Caption = dtColumn.ColumnName
                        dtOutputTable.Columns.Add(dtColumn)
                    End If
                Next

                While dtReader.Read()
                    dtRow = dtOutputTable.NewRow()

                    For i As Integer = 0 To (dtOutputTable.Columns.Count - 1)
                        dtRow(i) = dtReader.GetValue(i)
                    Next

                    dtOutputTable.Rows.Add(dtRow)
                End While

                Call Me.CloseDataReader()

                Return dtOutputTable

            End SyncLock

        End Function

        ''' <summary>
        ''' Retorna o índice gerado para o último comando INSERT.
        ''' </summary>
        ''' <returns>0 (ZERO) se nenhum índice for encontrado.</returns>
        Public ReadOnly Property LastIdentity As Integer

            Get
                Dim Obj As Object

                Obj = ConnectionManagerFirebird.GetInstance.ExecuteScalar("SELECT @@IDENTITY")

                If IsDBNull(Obj) Then _
                    Return 0

                Return CInt(Obj)
            End Get
        End Property


    End Class


End Namespace