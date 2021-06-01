Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports FPX

Public Class CameraSwitch
    Inherits Component

    Public Sub Update(gameTime As GameTime)
        Dim keyboardState = Keyboard.GetState()
        If keyboardState.IsKeyDown(Keys.NumPad1) Then
            Camera.Active.gameObject.Enabled = False
            Camera.Active.gameObject.Enabled = False
            Camera.Active = GameCore.currentScene.Objects.ToList().Find(Function(obj) obj.Name = "Camera 1").GetComponent(Of Camera)
            Camera.Active.gameObject.Enabled = True
            Camera.Active.gameObject.Enabled = True
        End If

        If keyboardState.IsKeyDown(Keys.NumPad2) Then
            Camera.Active.gameObject.Enabled = False
            Camera.Active.gameObject.Visible = False
            Camera.Active = GameCore.currentScene.Objects.ToList.Find(Function(obj) obj.Name = "Camera 2").GetComponent(Of Camera)
            Camera.Active.gameObject.Enabled = True
            Camera.Active.gameObject.Visible = True
        End If

    End Sub
End Class
