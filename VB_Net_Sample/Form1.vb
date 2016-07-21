Public Class Form1
    Private renderView As AnyCAD.Presentation.RenderWindow3d

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        'Initialize the 3D control
        renderView = New AnyCAD.Presentation.RenderWindow3d
        renderView.Size = Panel1.ClientSize
        Panel1.Controls.Add(renderView)

        'Register Selection Event Handler
        AddHandler AnyCAD.Platform.GlobalInstance.EventListener.OnSelectElementEvent, AddressOf Selection_Changed
    End Sub

    Private Sub Form1_SizeChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.SizeChanged
        If Not IsNothing(renderView) Then
            renderView.Size = Panel1.ClientSize
        End If
    End Sub
    'Create the shape
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim ball = AnyCAD.Platform.GlobalInstance.BrepTools.MakeSphere(AnyCAD.Platform.Vector3.ZERO, 100)
        renderView.ShowGeometry(ball, 100)
        renderView.RequestDraw()
    End Sub

    ' Selection Event Handler
    Private Sub Selection_Changed(args As AnyCAD.Platform.SelectionChangeArgs)
        MsgBox(args.GetIds().ToString())
    End Sub

End Class
