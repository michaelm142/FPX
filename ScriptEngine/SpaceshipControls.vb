Imports System.Xml
Imports Microsoft.Xna.Framework
Imports FPX

Public Class SpaceshipControls
    Inherits Component

    Public MoveSpeed As Single
    Public TurnSpeed As Single

    Private rotX As Single
    Private rotY As Single
    Private fireAxisPrevious As Single

    Public Sub Update(gameTime As GameTime)
        Dim horizontal = Input.GetAxis("Horizontal")
        Dim vertical = Input.GetAxis("Vertical")
        Dim pitch = Input.GetAxis("Pitch")
        Dim yaw = Input.GetAxis("Yaw")
        Dim fireAxis = Input.GetAxis("Fire")

        Dim force = transform.right * horizontal * MoveSpeed + transform.forward * vertical * MoveSpeed

        rotX += pitch * TurnSpeed * Time.deltaTime
        rotY += yaw * TurnSpeed * Time.deltaTime
        rotX = MathHelper.Clamp(rotX, CType(-Math.PI, Single) / 2.0F, CType(Math.PI, Single) / 2.0F)
        Dim rigidbody = GetComponent(Of Rigidbody)()
        'rigidbody.angularVelocity = torque
        transform.rotation = Quaternion.CreateFromYawPitchRoll(rotY, rotX, 0.0F)
        rigidbody.AddForce(force)

        If fireAxis = 0 And fireAxisPrevious > 0 Then
            GetComponent(Of FireLaser).Fire(-1.0)
        End If

        If GetComponent(Of SpaceShipResources)().health <= 0.0F Then
            Destroy(gameObject)
        End If

        fireAxisPrevious = fireAxis

        transform.Find("Box001").localRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(180.0F))
    End Sub


End Class