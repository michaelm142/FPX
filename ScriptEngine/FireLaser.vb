Imports Microsoft.Xna.Framework
Imports FPX
Imports System.Xml

Public Class FireLaser
    Inherits Component

    Private laserEnergyCost As Single = 1.0F
    Private fireAxisPrevious As Single

    Public Sub Update(gameTime As GameTime)
        Dim fireAxis = Input.GetAxis("Fire")
        Dim resources = GetComponent(Of SpaceShipResources)()

        If Not resources.recharging And fireAxis = 0 And fireAxisPrevious > 0 Then
            If resources.energy < laserEnergyCost And resources.energy > 0 Then
                resources.recharging = True
                resources.energy = 0
            Else
                resources.energy -= laserEnergyCost
            End If
            Dim bullet = Instantiate(Prefab.Load("Prefabs//LaserBullet.xml"))
            bullet.position = position
            Dim rigidbody = bullet.GetComponent(Of Rigidbody)
            rigidbody.velocity = -transform.forward * 500
            GetComponent(Of AudioSource).Play()
        End If


        fireAxisPrevious = fireAxis
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        MyBase.LoadXml(element)

        Dim energyCostNode = element.SelectSingleNode("LaserEnergyCost")
        If Not energyCostNode Is Nothing Then
            laserEnergyCost = Single.Parse(energyCostNode.InnerText)
        End If
    End Sub
End Class