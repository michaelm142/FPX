Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports FPX
Imports System.Xml

Public Class SpaceShipResources
    Inherits Component

    Public Energy As Single
    Public MaxEnergy As Single
    Public EnergyRechargeRate As Single

    Public Health As Single
    Public MaxHealth As Single

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

End Class