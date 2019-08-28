Imports System.Xml
Imports Microsoft.Xna.Framework
Imports ComponentModel

Public Class ColliderTester
    Inherits Component

    Private startColor As Color

    Public Sub Start()
        startColor = GetComponent(Of Material)().DiffuseColor
    End Sub

    Public Sub OnCollisionEnter(other As Collider)
        GetComponent(Of Material)().DiffuseColor = Color.Blue
        Console.WriteLine("Collision entered")
    End Sub

    Public Sub OnCollision(other As Collider)
        GetComponent(Of Material)().DiffuseColor = Color.Orange
        Console.WriteLine("Collision stay")
    End Sub

    Public Sub OnCollisionExit(other As Collider)
        GetComponent(Of Material)().DiffuseColor = startColor
        Console.WriteLine("Collision exit")
    End Sub

    Public Sub LoadXml(node As XmlElement)
        Dim m = Matrix.Identity
    End Sub
End Class
