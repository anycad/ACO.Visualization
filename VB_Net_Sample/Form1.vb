Public Class Form1
    Private renderView As AnyCAD.Presentation.RenderWindow3d

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        renderView = New AnyCAD.Presentation.RenderWindow3d
        renderView.Size = Panel1.ClientSize
        Panel1.Controls.Add(renderView)
    End Sub

    Private Sub Form1_SizeChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.SizeChanged
        If Not IsNothing(renderView) Then
            renderView.Size = Panel1.ClientSize
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim ball = AnyCAD.Platform.GlobalInstance.BrepTools.MakeSphere(AnyCAD.Platform.Vector3.ZERO, 100)
        renderView.ShowGeometry(ball, 100)
        renderView.RequestDraw()
    End Sub
End Class
