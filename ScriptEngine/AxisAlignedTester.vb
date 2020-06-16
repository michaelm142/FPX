Imports System.Xml
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports FPX

Public Class AxisAlignedTester
    Inherits Component

    Private pointSphere As GameObject

    Private target As GameObject

    Private testSize As Vector3 = Vector3.One


    Public Sub Start()

        pointSphere = ObjectFactory.Create(PrimitiveType.Sphere, Scene.Active)
        pointSphere.transform.localScale = Vector3.One * 0.2F
        pointSphere.GetComponent(Of Material).DiffuseColor = Color.Purple
    End Sub

    Public Sub Update(gameTime As GameTime)
        Dim closestPoint = target.GetComponent(Of BoxCollider).ClosestPoint(Vector3.Zero)

        pointSphere.position = closestPoint
    End Sub

    Public Sub LoadXml(node As XmlElement)
        Dim targetNode = node.SelectSingleNode("Target")
        Dim sizeNode = node.SelectSingleNode("Size")

        If Not targetNode Is Nothing Then
            Dim targetId As UInt32 = UInt32.Parse(targetNode.InnerText)
            target = GameObject.Find(targetId)
        End If

        If Not sizeNode Is Nothing Then
            testSize = LinearAlgebraUtil.Vector3FromXml(sizeNode)
        End If

    End Sub
End Class