Imports Microsoft.Xna.Framework
Imports FPX

Public Class SpaceshipEffects
    Inherits Component

    Public Sub Start()
        For Each t In transform
            If Not t.Name.IndexOf("engineLight") = -1 Then
                Dim light = gameObject.AddComponent(Of Light)
                light.LightType = LightType.Point
            End If
        Next

    End Sub

End Class
