Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports FPX

Public Class DebugDeviceTextures
    Inherits Component

    Public keyboardPrev As KeyboardState

    Public Sub Update(ByVal gameTime As GameTime)
        Dim keys = Keyboard.GetState()
        If keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) And keyboardPrev.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Enter) Then
            GameCore.Graphics.renderer._debug_OutuptGBuffers()
        End If

        keyboardPrev = Keyboard.GetState()
    End Sub

End Class