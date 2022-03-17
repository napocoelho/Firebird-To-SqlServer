Imports DllUtilitaria.Database
Imports System.ServiceProcess



Public Class Principal


    Private Shadows FileName As String = "save.data"
    Public Property Configuracao As Configuracao

    Public Property ProblemasEncontrados As System.ComponentModel.BindingList(Of ProblemaEncontrado)

    Public Sub New()

        'Dim argList As List(Of String) = Environment.GetCommandLineArgs().ToList

        'If argList.Count > 0 Then
        'End If


        Me.ProblemasEncontrados = New System.ComponentModel.BindingList(Of ProblemaEncontrado)


        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'CriarConexoes()
        Configuracao = New Configuracao

        GridProblemas.DataSource = Me.ProblemasEncontrados

        If System.IO.File.Exists(FileName) Then
            Try
                Configuracao.Load(FileName)
            Catch ex As Exception
                System.IO.File.Delete(FileName)
            End Try
        End If

        'Se não existir o arquivo, retorna uma configuração padrão:
        If Not System.IO.File.Exists(FileName) Then
            SetDefaultValues()
        End If

        CopyToControls(Configuracao)

    End Sub

    Private Sub BtnTestarConexoes_Click(sender As System.Object, e As System.EventArgs) Handles BtnTestarConexoes.Click

        TestarConexoes()

    End Sub

    Private Sub BtnImportar_Click(sender As System.Object, e As System.EventArgs) Handles BtnImportar.Click

        Importar()

    End Sub

    Public Sub Importar()

        Dim importador As New Importador
        Dim horaInicial As Date

        horaInicial = Date.Now

        Try

            'CriarConexoes(Configuracao.GetFirebirdConnectionString, Configuracao.GetSqlServerConnectionString)
            CopyToConfiguration(Configuracao)

            Try
                CriarConexoes(Configuracao.GetFirebirdConnectionString, Configuracao.GetSqlServerConnectionString)
                'MsgBox("Conexões testadas com sucesso!", vbInformation, "Testar conexão")
            Catch
                'Tenta ativar o serviço.
                '***  Lembrando que o Firebird pode ser utilizado através de serviço ou aplicativo. 
                '**** Se o aplicativo estiver ativado, o serviço não estará instalado, ou seja, não será encontrado como serviço.
                TryToStartFirebirdService()
                CriarConexoes(Configuracao.GetFirebirdConnectionString, Configuracao.GetSqlServerConnectionString)
            End Try


            importador.Info(Sub(info As ImportadorInfo)

                                ToolStripStatusLabel1.Text = "Importando " & info.Tabela & " | Registros " & info.TotalDeRegistros
                                LblTimer.Text = "Tempo: " & Date.Now.Subtract(horaInicial).ToString
                                Application.DoEvents()

                            End Sub)

            importador.InfoNovoErroEncontrado(Sub(info As ProblemaEncontrado)
                                                  Me.ProblemasEncontrados.Add(info)
                                                  Application.DoEvents()
                                              End Sub)


            importador.Importar()

            LblTimer.Text = "Tempo: " & Date.Now.Subtract(horaInicial).ToString

            MsgBox("Procedimento concluído!", vbInformation, "Conclusão")

        Catch sqlEx As SqlExpressionException
            MsgBox(sqlEx.Message & vbNewLine & sqlEx.Expression, vbExclamation, "Erro")
        Catch ex As Exception
            MsgBox(ex.Message, vbExclamation, "Erro")
        End Try

        ToolStripStatusLabel1.Text = "Pronto!"

    End Sub

    Public Sub TestarConexoes()

        CopyToConfiguration(Configuracao)

        Try
            CriarConexoes(Configuracao.GetFirebirdConnectionString, Configuracao.GetSqlServerConnectionString)
            MsgBox("Conexões testadas com sucesso!", vbInformation, "Testar conexão")
        Catch 'ex As Exception

            'Tenta ativar o serviço:
            '***  Lembrando que o Firebird pode ser utilizado através de serviço ou aplicativo. 
            '**** Se o aplicativo estiver ativado, o serviço não estará instalado, ou seja, não será encontrado como serviço.
            Try
                TryToStartFirebirdService()
                CriarConexoes(Configuracao.GetFirebirdConnectionString, Configuracao.GetSqlServerConnectionString)
                MsgBox("Conexões testadas com sucesso! O serviço estava desativado e foi ativado.", vbInformation, "Testar conexão")
            Catch ex2 As Exception
                MsgBox(ex2.Message, MsgBoxStyle.Exclamation, "Testar conexão")
            End Try

            'MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erro")
        End Try


    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        CopyToConfiguration(Configuracao)
        Configuracao.Save(FileName)
    End Sub

    Private Sub CopyToConfiguration(ByRef config As Configuracao)
        config.FirebirdUser = TxtFbUser.Text
        config.FirebirdPassword = TxtFbPassword.Text
        config.FirebirdDatabase = TxtFbDatabase.Text
        config.FirebirdDatasource = TxtFbDataSource.Text
        config.FirebirdPort = TxtFbPort.Text
        config.FirebirdDialect = TxtFbDialect.Text

        config.SqlserverDatabase = TxtSsDatabase.Text
        config.SqlserverDatasource = TxtSsDataSource.Text
        config.SqlserverUser = TxtSsUser.Text
        config.SqlserverPassword = TxtSsPassword.Text
        config.SqlserverTimeout = TxtSsTimeout.Text
    End Sub

    Private Sub CopyToControls(ByRef config As Configuracao)
        TxtFbUser.Text = config.FirebirdUser
        TxtFbPassword.Text = config.FirebirdPassword
        TxtFbDatabase.Text = config.FirebirdDatabase
        TxtFbDataSource.Text = config.FirebirdDatasource
        TxtFbPort.Text = config.FirebirdPort
        TxtFbDialect.Text = config.FirebirdDialect

        TxtSsDatabase.Text = config.SqlserverDatabase
        TxtSsDataSource.Text = config.SqlserverDatasource
        TxtSsUser.Text = config.SqlserverUser
        TxtSsPassword.Text = config.SqlserverPassword
        TxtSsTimeout.Text = config.SqlserverTimeout
    End Sub

    Private Sub SetDefaultValues()
        Configuracao.FirebirdDatasource = "localhost"
        Configuracao.FirebirdDatabase = "c:\db.fdb"
        Configuracao.FirebirdDialect = 3
        Configuracao.FirebirdUser = "sysdba"
        Configuracao.FirebirdPassword = "masterkey"
        Configuracao.FirebirdPort = 3050

        Configuracao.SqlserverDatabase = "master"
        Configuracao.SqlserverDatasource = "SERVIDOR"
        Configuracao.SqlserverTimeout = 0
        Configuracao.SqlserverUser = "preview"
    End Sub




    ''' <summary>
    ''' Verifica se o serviço existe e, caso existir, tenta ativá-lo.
    ''' O Firebird pode funcionar em 2 modos, sendo mutuamente exclusivos: Aplicativo e Serviço.
    ''' Se o modo Aplicativo estiver ativado, o Serviço pode estar desinstalado, ou seja, não será encontrado na lista de serviços do Windows.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TryToStartFirebirdService()

        '***  Lembrando que o Firebird pode ser utilizado através de serviço ou aplicativo. 
        '**** Se o aplicativo estiver ativado, o serviço não estará instalado, ou seja, não será encontrado como serviço.

        Dim service As ServiceController = Nothing

        'Procurando serviço:
        For Each serviceTemp As ServiceController In ServiceController.GetServices()

            If serviceTemp.ServiceName.Contains("Firebird") Then
                service = serviceTemp
                Exit For
            End If
        Next


        If service Is Nothing Then
            Throw New Exception("Serviço do Firebird não encontrado! Ative o Firebird manualmente.")
            'MsgBox("Serviço do Firebird não encontrado! Ative o Firebird manualmente.", MsgBoxStyle.Exclamation, "Serviço não encontrado")
            Return
        End If


        Try
            If service.Status = ServiceControllerStatus.Paused Or
               service.Status = ServiceControllerStatus.PausePending Or
               service.Status = ServiceControllerStatus.Stopped Or
               service.Status = ServiceControllerStatus.StopPending Then

                service.Start()
            End If
        Catch exc As Exception

            Dim msg As String = "Não foi possível iniciar o serviço do Firebird! Ative-o manualmente." & vbNewLine & _
                "Detalhes do problema: " & vbNewLine & _
                exc.Message

            Throw New Exception(msg)
            'MsgBox(msg, MsgBoxStyle.Exclamation, "Erro ao iniciar serviço")

        End Try

    End Sub

    Private lock As Boolean = False
    
    Private Sub Principal_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

        'Largura:
        grpConfiguração.Width = Me.Width - 35
        grpImportação.Width = Me.Width - 35
        grpProblemas.Width = Me.Width - 35

        BtnImportar.Left = grpImportação.Width - BtnImportar.Width - 5
        BtnTestarConexoes.Left = BtnImportar.Left - BtnTestarConexoes.Width - 5

        'Altura:
        grpProblemas.Height = StatusBar.Top - 5 - grpProblemas.Top



        If Not lock Then
            lock = True
            Dim widthStep As Integer = Math.Abs(((grpDest.Left + grpDest.Width + 5) - grpConfiguração.Width) / 10)
            widthStep = IIf(widthStep = 0, 1, widthStep)


            While (grpDest.Left + grpDest.Width + 5) > grpConfiguração.Width 'Or grpProblemas.Height < 50

                'If (grpDest.Left + grpDest.Width + 5) > grpConfiguração.Width Then
                Me.Width = Me.Width + widthStep
                'End If

            End While

            widthStep = grpConfiguração.Width - (grpDest.Left + grpDest.Width + 5)
            'heightStep = grpConfiguração.Width - (grpDest.Left + grpDest.Width + 5)

            Me.Width = IIf(widthStep > 0, Me.Width - widthStep, Me.Width)

            lock = False
        End If
    End Sub
End Class
