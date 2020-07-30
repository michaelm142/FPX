Imports System.Xml
Imports Microsoft.Xna.Framework
Imports FPX

Public Class ClosestPointFinder
    Inherits Component

    Private target As GameObject
    Private targetId As UInteger

    Private sphere As GameObject

    Public Sub Start()
        target = GameObject.Find(targetId)
        sphere = ObjectFactory.Create(PrimitiveType.Sphere, Scene.Active)
        sphere.transform.localScale = Vector3.One * 0.05
    End Sub

    Public Overrides Sub LoadXml(node As XmlElement)
        Dim targetNode = node.SelectSingleNode("Target")
        targetId = UInteger.Parse(targetNode.InnerText)
    End Sub

    Public Sub Update(gameTime As GameTime)
        sphere.transform.position = GetComponent(Of BoxCollider).ClosestPoint(target.position)
    End Sub

End Class