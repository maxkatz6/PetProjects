<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
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

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DirectXPanel1 = New TDF.WinFormsPanel.DirectXPanel()
        Me.SuspendLayout()
        '
        'DirectXPanel1
        '
        Me.DirectXPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.DirectXPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DirectXPanel1.Location = New System.Drawing.Point(0, 0)
        Me.DirectXPanel1.Name = "DirectXPanel1"
        Me.DirectXPanel1.Size = New System.Drawing.Size(951, 503)
        Me.DirectXPanel1.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(951, 503)
        Me.Controls.Add(Me.DirectXPanel1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DirectXPanel1 As TDF.WinFormsPanel.DirectXPanel

End Class
