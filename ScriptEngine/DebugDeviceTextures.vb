Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports FPX.ComponentModel

Public Class DebugDeviceTextures
    Inherits Component

    Public keyboardPrev As KeyboardState

    Public Sub Update(ByVal gameTime As GameTime)
        Dim keys = Keyboard.GetState()
        If keys.IsKeyDown(Input.Keys.Enter) And keyboardPrev.IsKeyUp(Input.Keys.Enter) Then
            GameCore.Graphics.renderer._debug_OutuptGBuffers()
        End If

        keyboardPrev = Keyboard.GetState()
    End Sub

End Class