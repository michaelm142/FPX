Imports Microsoft.Xna.Framework
Imports FPX
Imports System.Xml

Public Class EnemyAI
    Inherits Component

    Private target As GameObject

    Private targetName As String

    Public moveSpeed As Single
    Public turnSpeed As Single
    Public shootRadius As Single
    Public aggroRadius As Single
    Public shootInterval As Single
    Private ShootTimer As Single

    Public Sub Start()
        target = GameObject.Find(targetName)
        ShootTimer = shootInterval
    End Sub

    Public Sub Update(gameTime As GameTime)
        Dim L = target.position - position
        If L.Length() < aggroRadius Then
            Dim targetPose = Matrix.CreateWorld(Vector3.Zero, L.Normalized(), Vector3.Up)
            rotation = Quaternion.Slerp(rotation, targetPose.Rotation, turnSpeed * Time.deltaTime)

            If L.Length() > shootRadius Then
                position += L.Normalized() * moveSpeed * Time.deltaTime
            Else
                shootInterval -= Time.deltaTime
                If shootInterval < 0.0F Then
                    GetComponent(Of FireLaser).Fire(1.0F)
                    shootInterval = ShootTimer
                End If
            End If
        End If

        If GetComponent(Of SpaceShipResources).health <= 0 Then
            Destroy(gameObject)
        End If
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        MyBase.LoadXml(element)

        Dim targetElement = element.SelectSingleNode("Target")
        If Not targetElement Is Nothing Then
            targetName = targetElement.InnerText
        End If

        Dim moveSpeedNode = element.SelectSingleNode("MoveSpeed")
        If Not moveSpeedNode Is Nothing Then
            moveSpeed = Single.Parse(moveSpeedNode.InnerText)
        End If
        Dim shootRadiusNode = element.SelectSingleNode("ShootRadius")
        If Not shootRadiusNode Is Nothing Then
            shootRadius = Single.Parse(shootRadiusNode.InnerText)
        End If
        Dim aggroRadiusNode = element.SelectSingleNode("AggroRadius")
        If Not aggroRadiusNode Is Nothing Then
            aggroRadius = Single.Parse(aggroRadiusNode.InnerText)
        End If
        Dim turnSpeedNode = element.SelectSingleNode("TurnSpeed")
        If Not turnSpeedNode Is Nothing Then
            turnSpeed = Single.Parse(turnSpeedNode.InnerText)
        End If
        Dim shootIntervalNode = element.SelectSingleNode("ShootInterval")
        If Not shootIntervalNode Is Nothing Then
            shootInterval = Single.Parse(shootIntervalNode.InnerText)
        End If
    End Sub
End Class