Imports System.Xml
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports FPX

Public Class AxisAlignedTester
    Inherits Component

    Private targetUpLine As LineRenderer
    Private targetRightLine As LineRenderer
    Private targetFrontLine As LineRenderer

    Private worldUpLine As LineRenderer
    Private worldRightLine As LineRenderer
    Private worldFrontLine As LineRenderer

    Private testSphere1 As GameObject
    Private testSphere2 As GameObject
    Private testSphere3 As GameObject

    Private pointSphere As GameObject

    Private target As GameObject

    Private testSize As Vector3 = Vector3.One


    Public Sub Start()
        targetUpLine = gameObject.AddComponent(Of LineRenderer)
        targetRightLine = gameObject.AddComponent(Of LineRenderer)
        targetFrontLine = gameObject.AddComponent(Of LineRenderer)
        worldUpLine = gameObject.AddComponent(Of LineRenderer)
        worldRightLine = gameObject.AddComponent(Of LineRenderer)
        worldFrontLine = gameObject.AddComponent(Of LineRenderer)

        targetFrontLine.positions.Add(Vector3.Zero)
        targetFrontLine.positions.Add(Vector3.Forward * 1000)
        worldFrontLine.positions.Add(Vector3.Backward * testSize.Z)
        worldFrontLine.positions.Add(Vector3.Forward * testSize.Z)

        targetRightLine.positions.Add(Vector3.Zero)
        targetRightLine.positions.Add(Vector3.Right * 1000)
        worldRightLine.positions.Add(Vector3.Left * testSize.X)
        worldRightLine.positions.Add(Vector3.Right * testSize.X)

        targetUpLine.positions.Add(Vector3.Zero)
        targetUpLine.positions.Add(Vector3.Up * 1000)
        worldUpLine.positions.Add(Vector3.Down * testSize.Y)
        worldUpLine.positions.Add(Vector3.Up * testSize.Y)

        targetUpLine.useWorldSpace = False
        targetRightLine.useWorldSpace = False
        targetFrontLine.useWorldSpace = False
        worldUpLine.useWorldSpace = False
        worldRightLine.useWorldSpace = False
        worldFrontLine.useWorldSpace = False

        testSphere1 = ObjectFactory.Create(PrimitiveType.Sphere, Scene.Active)
        testSphere1.transform.localScale = Vector3.One * 0.2F
        testSphere1.GetComponent(Of Material).DiffuseColor = Color.Green

        testSphere2 = ObjectFactory.Create(PrimitiveType.Sphere, Scene.Active)
        testSphere2.transform.localScale = Vector3.One * 0.2F
        testSphere2.GetComponent(Of Material).DiffuseColor = Color.Red

        testSphere3 = ObjectFactory.Create(PrimitiveType.Sphere, Scene.Active)
        testSphere3.transform.localScale = Vector3.One * 0.2F
        testSphere3.GetComponent(Of Material).DiffuseColor = Color.Blue

        pointSphere = ObjectFactory.Create(PrimitiveType.Sphere, Scene.Active)
        pointSphere.transform.localScale = Vector3.One * 0.2F
        pointSphere.GetComponent(Of Material).DiffuseColor = Color.Purple
    End Sub

    Public Sub Update(gameTime As GameTime)
        If targetUpLine.material Is Nothing Then
            Return
        End If

        Dim L = target.position
        Dim InvL = -L.Normalized()

        targetUpLine.material.DiffuseColor = Color.Green
        targetRightLine.material.DiffuseColor = Color.DarkRed
        targetFrontLine.material.DiffuseColor = Color.LightBlue

        worldUpLine.material.DiffuseColor = Color.Black * 0.3F
        worldRightLine.material.DiffuseColor = Color.Black * 0.3F
        worldFrontLine.material.DiffuseColor = Color.Black * 0.3F

        worldUpLine.material.blendState = BlendState.AlphaBlend
        worldRightLine.material.blendState = BlendState.AlphaBlend
        worldFrontLine.material.blendState = BlendState.AlphaBlend

        Dim LdotUp = Vector3.Dot(L, Vector3.Up)
        Dim LdotRight = Vector3.Dot(L, Vector3.Right)
        Dim LdotFront = Vector3.Dot(L, Vector3.Forward)

        testSphere1.position = Vector3.Up * LdotUp
        testSphere2.position = Vector3.Right * LdotRight
        testSphere3.position = Vector3.Forward * LdotFront

        Dim tUpDot = Vector3.Dot(target.transform.up, Vector3.Up)
        Dim tRightDot = Vector3.Dot(target.transform.right, Vector3.Right)
        Dim tFrontDot = Vector3.Dot(target.transform.forward, Vector3.Forward)

        Dim projUp = Vector3.Up * tUpDot
        Dim projRight = Vector3.Right * tRightDot
        Dim projFront = Vector3.Forward * tFrontDot

        targetUpLine.positions(0) = testSphere1.position
        targetRightLine.positions(0) = testSphere2.position
        targetFrontLine.positions(0) = testSphere3.position

        targetUpLine.positions(1) = projUp + testSphere1.position
        targetRightLine.positions(1) = projRight + testSphere2.position
        targetFrontLine.positions(1) = projFront + testSphere3.position

        Dim InvLDotUp = Vector3.Dot(InvL, target.transform.up)
        Dim InvLDotRight = Vector3.Dot(InvL, target.transform.right)
        Dim InvLDotFront = Vector3.Dot(InvL, target.transform.forward)

        pointSphere.position = target.position + target.transform.up * InvLDotUp + target.transform.right * InvLDotRight
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