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
    Public Visible As Boolean = True

    Private uiOffset As Vector2

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
        If Not Visible Then
            Return
        End If

        Dim energyVal = (energy / maxEnergy) * energyBarWidth
        Dim healthVal = (health / maxHealth) * healthBarWidth

        'Draw Health Bar
        spriteBatch.Draw(Material.DefaultTexture, New Rectangle(50 + uiOffset.X, 50 + uiOffset.Y, healthBarWidth, 50), Color.Black)
        spriteBatch.Draw(Material.DefaultTexture, New Rectangle(50 + uiOffset.X, 50 + uiOffset.Y, healthVal, 50), Color.Red)

        'Draw Energy Bar
        spriteBatch.Draw(Material.DefaultTexture, New Rectangle(50 + uiOffset.X, 100 + uiOffset.Y, energyBarWidth, 5), Color.Black)
        spriteBatch.Draw(Material.DefaultTexture, New Rectangle(50 + uiOffset.X, 100 + uiOffset.Y, energyVal, 5), If(recharging, Color.Red, Color.Turquoise))
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        MyBase.LoadXml(element)

        Dim energyNode = element.SelectSingleNode("Energy")
        Dim maxEnergyNode = element.SelectSingleNode("MaxEnergy")
        Dim energyRechargeRateNode = element.SelectSingleNode("EnergyRechargeRate")
        Dim visibleNode = element.SelectSingleNode("Visible")

        Dim healthNode = element.SelectSingleNode("Health")
        Dim maxHealthNode = element.SelectSingleNode("MaxHealth")

        Dim uiOffsetNode = element.SelectSingleNode("UIOffset")

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
        If Not visibleNode Is Nothing Then
            Visible = Boolean.Parse(visibleNode.InnerText)
        End If
        If Not uiOffsetNode Is Nothing Then
            uiOffset = LinearAlgebraUtil.Vector3FromXml(uiOffsetNode).ToVector2()
        End If
    End Sub

End Class