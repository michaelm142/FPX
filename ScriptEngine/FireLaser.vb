Imports Microsoft.Xna.Framework
Imports FPX

Public Class FireLaser
    Inherits Component

    Private fireAxisPrevious As Single
    Public Sub Update(gameTime As GameTime)
        Dim fireAxis = Input.GetAxis("Fire")
        If fireAxis = 0 And fireAxisPrevious > 0 Then
            Dim bullet = Instantiate(Prefab.Load("Prefabs//LaserBullet.xml"))
            bullet.position = position
            Dim rigidbody = bullet.GetComponent(Of Rigidbody)
            rigidbody.velocity = -transform.forward * 500
            GetComponent(Of AudioSource).Play()
        End If


        fireAxisPrevious = fireAxis
    End Sub
End Class