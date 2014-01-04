Imports TDF.Core
Imports TDF.Inputs
Imports TDF.Graphics.Data
Imports TDF.Graphics.Render
Imports SharpDX
Imports TDF.Graphics.Cameras
Imports TDF.Graphics.Models

Public Class Form1
    ReadOnly _model As DxModel
    ReadOnly _freeCamera As FreeCamera

    Public Sub New()
        InitializeComponent()

        _freeCamera = New FreeCamera(New Vector3(0, 25, -150), 0, 0, True)
        DirectX11.Initialize(DirectXPanel1.Handle, _freeCamera, True)

        _model = GeometryGenerator.GenereateModel(Of TextureVertex)(GeometryGenerator.CreateGrid(1000, 1000, 20, 20), New Texture("dragonTexture.dds"))

    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        DirectXPanel1.Run(AddressOf UpdateT, AddressOf DrawT)
    End Sub

    Public Sub UpdateT()
        Input.SetMouseCoord(WinAPI.GetCurPos())

        _freeCamera.RotateWithMouse(Input.MouseState)

        If Input.IsKeyDown(Key.W) Then
            _freeCamera.DirectionMove(Vector3.ForwardLH)
        ElseIf Input.IsKeyDown(Key.S) Then
            _freeCamera.DirectionMove(Vector3.BackwardLH)
        ElseIf Input.IsKeyDown(Key.A) Then
            _freeCamera.DirectionMove(Vector3.Left)
        ElseIf Input.IsKeyDown(Key.D) Then
            _freeCamera.DirectionMove(Vector3.Right)
        End If

        _freeCamera.Update()
    End Sub

    Public Sub DrawT()
        _model.Render()
    End Sub
End Class
