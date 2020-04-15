Imports System.Xml
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports FPX

Public Class LineRenderer
    Inherits Component

    Public useWorldSpace As Boolean = True

    Public positions As List(Of Vector3) = New List(Of Vector3)

    Public vertecies As List(Of VertexPositionColor) = New List(Of VertexPositionColor)()

    Public effect As BasicEffect

    Public Sub Start()
        For i = 0 To positions.Count - 1
            vertecies.Add(New VertexPositionColor(positions(i), GetComponent(Of Material).DiffuseColor))
        Next

        effect = New BasicEffect(GameCore.graphicsDevice)
    End Sub

    Public Sub Draw(gametime As GameTime)
        effect.View = Camera.Active.ViewMatrix
        effect.Projection = Camera.Active.ProjectionMatrix
        If Not useWorldSpace Then
            effect.World = transform.worldPose
        End If

        effect.DiffuseColor = GetComponent(Of Material).DiffuseColor.ToVector3()
        effect.SpecularColor = GetComponent(Of Material).SpecularColor.ToVector3()
        effect.CurrentTechnique.Passes(0).Apply()

        GameCore.graphicsDevice.DrawUserPrimitives(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineList, vertecies.ToArray(), 0, vertecies.Count / 2)
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        Dim positionsNode = element.SelectSingleNode("Positions")
        Dim useWorldSpaceNode = element.SelectSingleNode("UseWorldSpace")

        If Not positionsNode Is Nothing Then
            For Each node In positionsNode.ChildNodes.Cast(Of XmlElement)
                Dim pos = LinearAlgebraUtil.Vector3FromXml(node)
                positions.Add(pos)
            Next
        End If

        If Not useWorldSpaceNode Is Nothing Then
            useWorldSpace = Boolean.Parse(useWorldSpaceNode.InnerText)
        End If
    End Sub

End Class
