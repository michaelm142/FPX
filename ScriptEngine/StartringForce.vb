Imports System.Xml
Imports Microsoft.Xna.Framework
Imports FPX

Public Class StartingForce
    Inherits Component

    Public startingForce As Vector3
    Public startingTorque As Vector3

    Public Sub Start()
        Dim rigidBody = GetComponent(Of Rigidbody)()
        rigidBody.AddForce(startingForce)
        rigidBody.AddTorque(startingTorque)

        Debug.Log("Object {0} position is {1}", gameObject.Name, position)
    End Sub

    Public Sub LoadXml(node As XmlNode)
        Dim startingForceNode As XmlElement = node.SelectSingleNode("StartingForce")
        Dim startingTorqueNode As XmlElement = node.SelectSingleNode("StartingTorque")

        If Not startingForceNode Is Nothing Then
            startingForce = LinearAlgebraUtil.Vector3FromXml(startingForceNode)
        End If

        If Not startingTorqueNode Is Nothing Then
            startingTorque = LinearAlgebraUtil.Vector3FromXml(startingTorqueNode)
        End If
    End Sub

End Class
