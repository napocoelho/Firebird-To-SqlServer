<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Principal
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.BtnImportar = New System.Windows.Forms.Button()
        Me.grpConfiguração = New System.Windows.Forms.GroupBox()
        Me.grpDest = New System.Windows.Forms.GroupBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.TxtSsUser = New System.Windows.Forms.TextBox()
        Me.TxtSsPassword = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.TxtSsDatabase = New System.Windows.Forms.TextBox()
        Me.TxtSsTimeout = New System.Windows.Forms.TextBox()
        Me.TxtSsDataSource = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.grpSource = New System.Windows.Forms.GroupBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TxtFbUser = New System.Windows.Forms.TextBox()
        Me.TxtFbPassword = New System.Windows.Forms.TextBox()
        Me.TxtFbDatabase = New System.Windows.Forms.TextBox()
        Me.TxtFbDialect = New System.Windows.Forms.TextBox()
        Me.TxtFbPort = New System.Windows.Forms.TextBox()
        Me.TxtFbDataSource = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnTestarConexoes = New System.Windows.Forms.Button()
        Me.grpImportação = New System.Windows.Forms.GroupBox()
        Me.LblTimer = New System.Windows.Forms.Label()
        Me.StatusBar = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.grpProblemas = New System.Windows.Forms.GroupBox()
        Me.GridProblemas = New System.Windows.Forms.DataGridView()
        Me.grpConfiguração.SuspendLayout()
        Me.grpDest.SuspendLayout()
        Me.grpSource.SuspendLayout()
        Me.grpImportação.SuspendLayout()
        Me.StatusBar.SuspendLayout()
        Me.grpProblemas.SuspendLayout()
        CType(Me.GridProblemas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnImportar
        '
        Me.BtnImportar.Location = New System.Drawing.Point(512, 18)
        Me.BtnImportar.Name = "BtnImportar"
        Me.BtnImportar.Size = New System.Drawing.Size(100, 23)
        Me.BtnImportar.TabIndex = 12
        Me.BtnImportar.Text = "Importar"
        Me.BtnImportar.UseVisualStyleBackColor = True
        '
        'grpConfiguração
        '
        Me.grpConfiguração.Controls.Add(Me.grpDest)
        Me.grpConfiguração.Controls.Add(Me.grpSource)
        Me.grpConfiguração.Location = New System.Drawing.Point(12, 12)
        Me.grpConfiguração.Name = "grpConfiguração"
        Me.grpConfiguração.Size = New System.Drawing.Size(618, 218)
        Me.grpConfiguração.TabIndex = 1
        Me.grpConfiguração.TabStop = False
        Me.grpConfiguração.Text = "Configuração"
        '
        'grpDest
        '
        Me.grpDest.Controls.Add(Me.Label20)
        Me.grpDest.Controls.Add(Me.TxtSsUser)
        Me.grpDest.Controls.Add(Me.TxtSsPassword)
        Me.grpDest.Controls.Add(Me.Label19)
        Me.grpDest.Controls.Add(Me.TxtSsDatabase)
        Me.grpDest.Controls.Add(Me.TxtSsTimeout)
        Me.grpDest.Controls.Add(Me.TxtSsDataSource)
        Me.grpDest.Controls.Add(Me.Label7)
        Me.grpDest.Controls.Add(Me.Label9)
        Me.grpDest.Controls.Add(Me.Label10)
        Me.grpDest.Controls.Add(Me.Label11)
        Me.grpDest.Controls.Add(Me.Label12)
        Me.grpDest.Location = New System.Drawing.Point(311, 29)
        Me.grpDest.Name = "grpDest"
        Me.grpDest.Size = New System.Drawing.Size(300, 182)
        Me.grpDest.TabIndex = 4
        Me.grpDest.TabStop = False
        Me.grpDest.Text = "Sql Server (DESTINATION)"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.LightSkyBlue
        Me.Label20.Location = New System.Drawing.Point(127, 127)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(22, 13)
        Me.Label20.TabIndex = 2
        Me.Label20.Text = "? 0"
        '
        'TxtSsUser
        '
        Me.TxtSsUser.Location = New System.Drawing.Point(76, 22)
        Me.TxtSsUser.Name = "TxtSsUser"
        Me.TxtSsUser.Size = New System.Drawing.Size(147, 20)
        Me.TxtSsUser.TabIndex = 6
        '
        'TxtSsPassword
        '
        Me.TxtSsPassword.Location = New System.Drawing.Point(76, 46)
        Me.TxtSsPassword.Name = "TxtSsPassword"
        Me.TxtSsPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TxtSsPassword.Size = New System.Drawing.Size(147, 20)
        Me.TxtSsPassword.TabIndex = 7
        Me.TxtSsPassword.UseSystemPasswordChar = True
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.LightSkyBlue
        Me.Label19.Location = New System.Drawing.Point(227, 101)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(67, 13)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "? DNS ou IP"
        '
        'TxtSsDatabase
        '
        Me.TxtSsDatabase.Location = New System.Drawing.Point(76, 72)
        Me.TxtSsDatabase.Name = "TxtSsDatabase"
        Me.TxtSsDatabase.Size = New System.Drawing.Size(147, 20)
        Me.TxtSsDatabase.TabIndex = 8
        '
        'TxtSsTimeout
        '
        Me.TxtSsTimeout.Location = New System.Drawing.Point(76, 124)
        Me.TxtSsTimeout.Name = "TxtSsTimeout"
        Me.TxtSsTimeout.Size = New System.Drawing.Size(45, 20)
        Me.TxtSsTimeout.TabIndex = 10
        '
        'TxtSsDataSource
        '
        Me.TxtSsDataSource.Location = New System.Drawing.Point(76, 98)
        Me.TxtSsDataSource.Name = "TxtSsDataSource"
        Me.TxtSsDataSource.Size = New System.Drawing.Size(147, 20)
        Me.TxtSsDataSource.TabIndex = 9
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 127)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(45, 13)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Timeout"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 101)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 13)
        Me.Label9.TabIndex = 5
        Me.Label9.Text = "DataSource"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 75)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(53, 13)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Database"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 49)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(53, 13)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Password"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 25)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(29, 13)
        Me.Label12.TabIndex = 9
        Me.Label12.Text = "User"
        '
        'grpSource
        '
        Me.grpSource.Controls.Add(Me.Label18)
        Me.grpSource.Controls.Add(Me.Label17)
        Me.grpSource.Controls.Add(Me.Label16)
        Me.grpSource.Controls.Add(Me.Label15)
        Me.grpSource.Controls.Add(Me.Label14)
        Me.grpSource.Controls.Add(Me.Label13)
        Me.grpSource.Controls.Add(Me.TxtFbUser)
        Me.grpSource.Controls.Add(Me.TxtFbPassword)
        Me.grpSource.Controls.Add(Me.TxtFbDatabase)
        Me.grpSource.Controls.Add(Me.TxtFbDialect)
        Me.grpSource.Controls.Add(Me.TxtFbPort)
        Me.grpSource.Controls.Add(Me.TxtFbDataSource)
        Me.grpSource.Controls.Add(Me.Label6)
        Me.grpSource.Controls.Add(Me.Label5)
        Me.grpSource.Controls.Add(Me.Label4)
        Me.grpSource.Controls.Add(Me.Label3)
        Me.grpSource.Controls.Add(Me.Label2)
        Me.grpSource.Controls.Add(Me.Label1)
        Me.grpSource.Location = New System.Drawing.Point(6, 29)
        Me.grpSource.Name = "grpSource"
        Me.grpSource.Size = New System.Drawing.Size(300, 182)
        Me.grpSource.TabIndex = 2
        Me.grpSource.TabStop = False
        Me.grpSource.Text = "Firebird/Interbase (SOURCE)"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.LightSkyBlue
        Me.Label18.Location = New System.Drawing.Point(140, 156)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(22, 13)
        Me.Label18.TabIndex = 2
        Me.Label18.Text = "? 3"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.LightSkyBlue
        Me.Label17.Location = New System.Drawing.Point(140, 130)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(40, 13)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = "? 3050"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.LightSkyBlue
        Me.Label16.Location = New System.Drawing.Point(225, 104)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(67, 13)
        Me.Label16.TabIndex = 2
        Me.Label16.Text = "? DNS ou IP"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.LightSkyBlue
        Me.Label15.Location = New System.Drawing.Point(225, 78)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(60, 13)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "? c:\db.fdb"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.LightSkyBlue
        Me.Label14.Location = New System.Drawing.Point(225, 52)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(64, 13)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "? masterkey"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.LightSkyBlue
        Me.Label13.Location = New System.Drawing.Point(225, 28)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(59, 13)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "? SYSDBA"
        '
        'TxtFbUser
        '
        Me.TxtFbUser.Location = New System.Drawing.Point(76, 25)
        Me.TxtFbUser.Name = "TxtFbUser"
        Me.TxtFbUser.Size = New System.Drawing.Size(143, 20)
        Me.TxtFbUser.TabIndex = 0
        '
        'TxtFbPassword
        '
        Me.TxtFbPassword.Location = New System.Drawing.Point(76, 49)
        Me.TxtFbPassword.Name = "TxtFbPassword"
        Me.TxtFbPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TxtFbPassword.Size = New System.Drawing.Size(143, 20)
        Me.TxtFbPassword.TabIndex = 1
        Me.TxtFbPassword.UseSystemPasswordChar = True
        '
        'TxtFbDatabase
        '
        Me.TxtFbDatabase.Location = New System.Drawing.Point(76, 75)
        Me.TxtFbDatabase.Name = "TxtFbDatabase"
        Me.TxtFbDatabase.Size = New System.Drawing.Size(143, 20)
        Me.TxtFbDatabase.TabIndex = 2
        '
        'TxtFbDialect
        '
        Me.TxtFbDialect.Location = New System.Drawing.Point(76, 153)
        Me.TxtFbDialect.Name = "TxtFbDialect"
        Me.TxtFbDialect.Size = New System.Drawing.Size(58, 20)
        Me.TxtFbDialect.TabIndex = 5
        '
        'TxtFbPort
        '
        Me.TxtFbPort.Location = New System.Drawing.Point(76, 127)
        Me.TxtFbPort.Name = "TxtFbPort"
        Me.TxtFbPort.Size = New System.Drawing.Size(58, 20)
        Me.TxtFbPort.TabIndex = 4
        '
        'TxtFbDataSource
        '
        Me.TxtFbDataSource.Location = New System.Drawing.Point(76, 101)
        Me.TxtFbDataSource.Name = "TxtFbDataSource"
        Me.TxtFbDataSource.Size = New System.Drawing.Size(143, 20)
        Me.TxtFbDataSource.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 156)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Dialect"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(26, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Port"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 104)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "DataSource"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Database"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Password"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "User"
        '
        'BtnTestarConexoes
        '
        Me.BtnTestarConexoes.Location = New System.Drawing.Point(406, 18)
        Me.BtnTestarConexoes.Name = "BtnTestarConexoes"
        Me.BtnTestarConexoes.Size = New System.Drawing.Size(100, 23)
        Me.BtnTestarConexoes.TabIndex = 11
        Me.BtnTestarConexoes.Text = "Testar conexões"
        Me.BtnTestarConexoes.UseVisualStyleBackColor = True
        '
        'grpImportação
        '
        Me.grpImportação.Controls.Add(Me.LblTimer)
        Me.grpImportação.Controls.Add(Me.BtnTestarConexoes)
        Me.grpImportação.Controls.Add(Me.BtnImportar)
        Me.grpImportação.Location = New System.Drawing.Point(12, 236)
        Me.grpImportação.Name = "grpImportação"
        Me.grpImportação.Size = New System.Drawing.Size(618, 50)
        Me.grpImportação.TabIndex = 2
        Me.grpImportação.TabStop = False
        Me.grpImportação.Text = "Importação"
        '
        'LblTimer
        '
        Me.LblTimer.AutoSize = True
        Me.LblTimer.Location = New System.Drawing.Point(15, 28)
        Me.LblTimer.Name = "LblTimer"
        Me.LblTimer.Size = New System.Drawing.Size(0, 13)
        Me.LblTimer.TabIndex = 14
        '
        'StatusBar
        '
        Me.StatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusBar.Location = New System.Drawing.Point(0, 528)
        Me.StatusBar.Name = "StatusBar"
        Me.StatusBar.Size = New System.Drawing.Size(637, 22)
        Me.StatusBar.TabIndex = 3
        Me.StatusBar.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'grpProblemas
        '
        Me.grpProblemas.Controls.Add(Me.GridProblemas)
        Me.grpProblemas.Location = New System.Drawing.Point(12, 292)
        Me.grpProblemas.Name = "grpProblemas"
        Me.grpProblemas.Size = New System.Drawing.Size(618, 229)
        Me.grpProblemas.TabIndex = 4
        Me.grpProblemas.TabStop = False
        Me.grpProblemas.Text = "Poblemas encontrados"
        '
        'GridProblemas
        '
        Me.GridProblemas.AllowUserToAddRows = False
        Me.GridProblemas.AllowUserToDeleteRows = False
        Me.GridProblemas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridProblemas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridProblemas.Location = New System.Drawing.Point(3, 16)
        Me.GridProblemas.Name = "GridProblemas"
        Me.GridProblemas.ReadOnly = True
        Me.GridProblemas.Size = New System.Drawing.Size(612, 210)
        Me.GridProblemas.TabIndex = 0
        '
        'Principal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(637, 550)
        Me.Controls.Add(Me.grpProblemas)
        Me.Controls.Add(Me.StatusBar)
        Me.Controls.Add(Me.grpImportação)
        Me.Controls.Add(Me.grpConfiguração)
        Me.Name = "Principal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Importar banco"
        Me.grpConfiguração.ResumeLayout(False)
        Me.grpDest.ResumeLayout(False)
        Me.grpDest.PerformLayout()
        Me.grpSource.ResumeLayout(False)
        Me.grpSource.PerformLayout()
        Me.grpImportação.ResumeLayout(False)
        Me.grpImportação.PerformLayout()
        Me.StatusBar.ResumeLayout(False)
        Me.StatusBar.PerformLayout()
        Me.grpProblemas.ResumeLayout(False)
        CType(Me.GridProblemas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnImportar As System.Windows.Forms.Button
    Friend WithEvents grpConfiguração As System.Windows.Forms.GroupBox
    Friend WithEvents grpSource As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxtFbUser As System.Windows.Forms.TextBox
    Friend WithEvents TxtFbPassword As System.Windows.Forms.TextBox
    Friend WithEvents TxtFbDatabase As System.Windows.Forms.TextBox
    Friend WithEvents TxtFbDialect As System.Windows.Forms.TextBox
    Friend WithEvents TxtFbPort As System.Windows.Forms.TextBox
    Friend WithEvents TxtFbDataSource As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents BtnTestarConexoes As System.Windows.Forms.Button
    Friend WithEvents grpDest As System.Windows.Forms.GroupBox
    Friend WithEvents TxtSsUser As System.Windows.Forms.TextBox
    Friend WithEvents TxtSsPassword As System.Windows.Forms.TextBox
    Friend WithEvents TxtSsDatabase As System.Windows.Forms.TextBox
    Friend WithEvents TxtSsTimeout As System.Windows.Forms.TextBox
    Friend WithEvents TxtSsDataSource As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents grpImportação As System.Windows.Forms.GroupBox
    Friend WithEvents StatusBar As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents LblTimer As System.Windows.Forms.Label
    Friend WithEvents grpProblemas As System.Windows.Forms.GroupBox
    Friend WithEvents GridProblemas As System.Windows.Forms.DataGridView

End Class
