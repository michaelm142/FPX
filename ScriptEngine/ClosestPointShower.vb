Imports Microsoft.Xna.Framework
Imports FPX

Public Class ClosestPointShower
    Inherits Component

    Private sphere As GameObject

    Private targetName As String
    Public target As GameObject

    Public Sub Start()
        sphere = ObjectFactory.Create(PrimitiveType.Sphere, GameCore.currentLevel)
        sphere.transform.localScale = Vector3.One * 0.25F
        sphere.GetComponent(Of Material).DiffuseColor = Color.Purple
    End Sub

    Public Sub Update(ByVal gametime As GameTime)
        If target Is Nothing Then
            target = GameObject.Find(targetName)
        End If
        sphere.position = GetComponent(Of Collider).ClosestPoint(target.GetComponent(Of Collider).Location)
    End Sub

    Public Overrides Sub LoadXml(ByVal node As Xml.XmlElement)
        targetName = node.Attributes("Target").Value
    End Sub
End Class
