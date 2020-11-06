Imports System.Xml
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports FPX

Public Class AxisAlignedTester
    Inherits Component

    Private pointSphereA As GameObject
    Private pointSphereB As GameObject

    Private targetId As UInt32

    Private target As GameObject

    Private testSize As Vector3 = Vector3.One


    Public Sub Start()
        pointSphereA = ObjectFactory.Create(PrimitiveType.Sphere, Scene.Active)
        pointSphereA.transform.localScale = Vector3.One * 0.2F
        pointSphereA.GetComponent(Of Material).DiffuseColor = Color.Purple

        pointSphereB = ObjectFactory.Create(PrimitiveType.Sphere, Scene.Active)
        pointSphereB.transform.localScale = Vector3.One * 0.2F
        pointSphereB.GetComponent(Of Material).DiffuseColor = Color.Purple

        target = GameObject.Find(targetId)
    End Sub

    Public Sub Update(gameTime As GameTime)
        Dim closestPointA = GetComponent(Of BoxCollider).ClosestPoint(Vector3.Zero)
        Dim closestPointB = target.GetComponent(Of BoxCollider).ClosestPoint(closestPointA)

        pointSphereA.position = closestPointA
        pointSphereB.position = closestPointB
    End Sub

    Public Overrides Sub LoadXml(node As XmlElement)
        Dim targetNode = node.SelectSingleNode("Target")
        Dim sizeNode = node.SelectSingleNode("Size")

        If Not targetNode Is Nothing Then
            targetId = UInt32.Parse(targetNode.InnerText)
        End If

        If Not sizeNode Is Nothing Then
            testSize = LinearAlgebraUtil.Vector3FromXml(sizeNode)
        End If

    End Sub
End Class