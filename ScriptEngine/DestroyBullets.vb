Imports FPX
Imports Microsoft.Xna.Framework

Public Class DestroyBullets
    Inherits Component

    Public destroyTag As String

    Public damage As Single

    Public Sub OnTriggerEnter(collider As Collider)
        If collider.gameObject.Tag = destroyTag Then
            Destroy(gameObject)

            Dim resources = collider.GetComponent(Of SpaceShipResources)
            If Not resources Is Nothing Then
                resources.health -= damage
            End If
        End If
    End Sub
End Class
