Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports FPX
Imports System.Xml

Public Class SpaceShipResources
    Inherits Component

    Public energy As Single
    Public maxEnergy As Single
    Public energyRechargeRate As Single

    Public health As Single
    Public maxHealth As Single

    Private healthBarWidth As Integer = 250
    Private energyBarWidth As Integer = 225

    Public recharging As Boolean

    Public Sub Update(gameTime As GameTime)
        If energy < maxEnergy Then
            energy += energyRechargeRate * Time.deltaTime
        End If

        energy = MathHelper.Clamp(energy, 0.0F, maxEnergy)
        If recharging And energy = maxEnergy Then
            recharging = False
        End If
    End Sub
    Public Sub DrawUI(spriteBatch As SpriteBatch)
        Dim energyVal = (energy / maxEnergy) * energyBarWidth
        Dim healthVal = (health / maxHealth) * healthBarWidth

        'Draw Health Bar
        spriteBatch.Draw(Material.DefaultTexture, New Rectangle(50, 50, healthBarWidth, 50), Color.Black)
        spriteBatch.Draw(Material.DefaultTexture, New Rectangle(50, 50, healthVal, 50), Color.Red)

        'Draw Energy Bar
        spriteBatch.Draw(Material.DefaultTexture, New Rectangle(50, 100, energyBarWidth, 5), Color.Black)
        spriteBatch.Draw(Material.DefaultTexture, New Rectangle(50, 100, energyVal, 5), If(recharging, Color.Red, Color.Turquoise))
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        MyBase.LoadXml(element)

        Dim energyNode = element.SelectSingleNode("Energy")
        Dim maxEnergyNode = element.SelectSingleNode("MaxEnergy")
        Dim energyRechargeRateNode = element.SelectSingleNode("EnergyRechargeRate")

        Dim healthNode = element.SelectSingleNode("Health")
        Dim maxHealthNode = element.SelectSingleNode("MaxHealth")

        If Not energyNode Is Nothing Then
            energy = Single.Parse(energyNode.InnerText)
        End If
        If Not maxEnergyNode Is Nothing Then
            maxEnergy = Single.Parse(maxEnergyNode.InnerText)
        End If
        If Not energyRechargeRateNode Is Nothing Then
            energyRechargeRate = Single.Parse(energyRechargeRateNode.InnerText)
        End If
        If Not healthNode Is Nothing Then
            health = Single.Parse(healthNode.InnerText)
        End If
        If Not maxHealthNode Is Nothing Then
            maxHealth = Single.Parse(maxHealthNode.InnerText)
        End If
    End Sub
End Class