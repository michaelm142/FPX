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
    End Sub

    Public Sub LoadXml(node As XmlElement)
        Dim targetNode = node.SelectSingleNode("Target")
        targetId = UInteger.Parse(targetNode.InnerText)
    End Sub

    Public Sub Update(gameTime As GameTime)

    End Sub

End Class