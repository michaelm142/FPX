Imports Microsoft.Xna.Framework
Imports FPX
Imports System.Xml

Public Class FireLaser
    Inherits Component

    Private laserEnergyCost As Single = 1.0F
    Private damage As Single

    Private destroyTag As String

    'Direction can be used to reverse the direction of the bullet fired untill the Xna Vector math is fixed
    Public Sub Fire(direction As Single)
        Dim resources = GetComponent(Of SpaceShipResources)()
        If Not resources.recharging Then
            If resources.energy < laserEnergyCost And resources.energy > 0 Then
                resources.recharging = True
                resources.energy = 0
            Else
                resources.energy -= laserEnergyCost
            End If
            Dim bullet = Instantiate(Prefab.Load("Prefabs//LaserBullet.xml"))
            bullet.GetComponent(Of DestroyBullets).destroyTag = destroyTag
            bullet.GetComponent(Of DestroyBullets).damage = damage

            bullet.position = position
            Dim rigidbody = bullet.GetComponent(Of Rigidbody)
            rigidbody.velocity = transform.forward * 5 * direction
            GetComponent(Of AudioSource).Play()
        End If
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        MyBase.LoadXml(element)

        Dim energyCostNode = element.SelectSingleNode("LaserEnergyCost")
        If Not energyCostNode Is Nothing Then
            laserEnergyCost = Single.Parse(energyCostNode.InnerText)
        End If
        Dim damageNode = element.SelectSingleNode("Damage")
        If Not damageNode Is Nothing Then
            damage = Single.Parse(damageNode.InnerText)
        End If
        Dim destroyTagNode = element.SelectSingleNode("DestroyTag")
        If Not destroyTagNode Is Nothing Then
            destroyTag = destroyTagNode.InnerText
        End If
    End Sub
End Class