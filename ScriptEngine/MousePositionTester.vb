Imports Microsoft.Xna.Framework
Imports FPX

Public Class MousePositionTester
    Inherits Component

    Public Sub Update(gameTime As GameTime)
        transform.position = Input.mousePosition.ToVector3()
    End Sub

End Class