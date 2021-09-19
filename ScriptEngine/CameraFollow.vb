Imports Microsoft.Xna.Framework
Imports FPX
Imports System.Xml

Public Class CameraFollow
    Inherits Component

    Public target As Transform

    Public offset As Vector3

    Public moveSpeed As Single = 1.0F
    Public turnSpeed As Single = 1.0F

    Private targetName As String

    Public Sub Start()
        target = GameObject.Find(targetName).transform
    End Sub

    Public Sub Update(gameTime As GameTime)
        Dim targetPosition = Vector3.Transform(offset, target.worldPose)
        Dim L = targetPosition - transform.position
        Dim maxDistance = L.Length()

        transform.position += L.Normalized() * MathHelper.Clamp(moveSpeed, 0.0F, maxDistance)

        Dim dotRight = Vector3.Dot(transform.right, target.forward)
        Dim dotUp = Vector3.Dot(transform.up, target.forward)
        Dim dotForward = Vector3.Dot(transform.up, target.right)

        dotRight = MathHelper.Clamp(dotRight, -1, 1)
        dotUp = MathHelper.Clamp(dotUp, -1, 1)
        dotForward = MathHelper.Clamp(dotForward, -1, 1)

        transform.rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, dotRight) * turnSpeed
        transform.rotation *= Quaternion.CreateFromAxisAngle(Vector3.Right, -dotUp) * turnSpeed
        transform.rotation *= Quaternion.CreateFromAxisAngle(Vector3.Forward, dotForward) * turnSpeed

        If Single.IsNaN(transform.rotation.X) Or Single.IsNaN(transform.rotation.Y) Or Single.IsNaN(transform.rotation.Z) Or Single.IsNaN(transform.rotation.W) Then
            transform.rotation = Quaternion.Identity
        End If
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        MyBase.LoadXml(element)

        Dim offsetNode = element.SelectSingleNode("Offset")
        Dim targetNode = element.SelectSingleNode("Target")
        Dim moveSpeedNode = element.SelectSingleNode("MoveSpeed")
        Dim turnSpeedNode = element.SelectSingleNode("TurnSpeed")

        If Not offsetNode Is Nothing Then
            offset = LinearAlgebraUtil.Vector3FromXml(offsetNode)
        End If
        If Not targetNode Is Nothing Then
            targetName = targetNode.InnerText
        End If
        If Not moveSpeedNode Is Nothing Then
            moveSpeed = Single.Parse(moveSpeedNode.InnerText)
        End If
        If Not turnSpeedNode Is Nothing Then
            turnSpeed = Single.Parse(turnSpeedNode.InnerText)
        End If
    End Sub
End Class