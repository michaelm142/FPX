Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports ComponentModel

Public NotInheritable Class ObjectFactory
    Private Sub New()
    End Sub

    Public Shared Function Create(ByVal type As PrimitiveType, ByVal level As Scene) As GameObject
        If type = PrimitiveType.Box Then
            Dim box = level.Spawn(GetType(MeshRenderer), GetType(Material))
            box.GetComponent(Of MeshRenderer).model = GameCore.content.Load(Of Model)("Models//Cube")
            box.GetComponent(Of MeshRenderer).model.Tag = "Models//Cube"
            Return box
        Else
            Dim sphere = level.Spawn(GetType(MeshRenderer), GetType(Material))
            sphere.GetComponent(Of MeshRenderer).model = GameCore.content.Load(Of Model)("Models//Sphere")
            sphere.GetComponent(Of MeshRenderer).model.Tag = "Models//Sphere"
            Return sphere
        End If

    End Function

End Class

Public Enum PrimitiveType
    Sphere
    Box
End Enum
