Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports FPX

Public Class RayTester
    Inherits Component

    Private sphere As GameObject
    Private arrow As GameObject

    Private ray As Ray

    Public Sub Start()
        sphere = ObjectFactory.Create(PrimitiveType.Sphere, GameCore.currentLevel)
        sphere.transform.localScale = Vector3.One * 0.1F

        arrow = GameObject.Find("Arrow")
    End Sub

    Public Sub Update(gameTime As GameTime)
        For Each obj In GameCore.currentLevel.Objects
            Dim sphereCollider = obj.GetComponent(Of SphereCollider)
            Dim boxCollider = obj.GetComponent(Of BoxCollider)

            Dim length As Single
            Dim point As Vector3
            Dim mouse As MouseState = Microsoft.Xna.Framework.Input.Mouse.GetState()

            If mouse.LeftButton = ButtonState.Pressed Then
                ray = Camera.Active.ScreenPointToRay(New Vector2(mouse.X, mouse.Y))
                arrow.position = ray.Position
                arrow.rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAt(Vector3.Zero, ray.Direction, Vector3.Cross(ray.Direction, Vector3.Right)))
                Debug.Log("Arrow Position: {0}", arrow.position)
                Debug.Log("Arrow Rotation: {0}", arrow.rotation.GetEulerAngles())
            End If

            If Not sphereCollider Is Nothing Then
                If Physics.IntersectRaySphere(ray, sphereCollider, length, point) Then
                    Debug.Log("Ray intersects object {0}", obj.Name)
                    sphere.position = point
                End If
            End If

            If Not boxCollider Is Nothing Then
                If Physics.IntersectRayBox(ray, boxCollider, length, point) Then
                    Debug.Log("Ray intersects object {0}. Location: {1}", obj.Name, point)
                    sphere.position = point
                End If
            End If
        Next
    End Sub

End Class
