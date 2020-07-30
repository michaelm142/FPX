Imports System.Xml
Imports Microsoft.Xna.Framework
Imports FPX

Public Class PusdodistanceTester
    Inherits Component

    Private IdA As String
    Private IdB As String

    Private TargetA As GameObject
    Private TargetB As GameObject

    Private TargetAPosPrev As Vector3
    Private TargetBPosPrev As Vector3

    Public Sub LoadXml(node As XmlElement)
        Dim nodeA = node.SelectSingleNode("IDA")
        Dim nodeB = node.SelectSingleNode("IDB")

        If Not nodeA Is Nothing Then
            IdA = nodeA.InnerText
        End If

        If Not nodeB Is Nothing Then
            IdB = nodeB.InnerText
        End If
    End Sub

    Public Sub Start()
        TargetA = GameObject.Find(IdA)
        TargetB = GameObject.Find(IdB)

        Debug.Log("Target A is {0}; Target B is {1}", TargetA.Name, TargetB.Name)
    End Sub

    Public Sub Update(deltaTime As GameTime)
        Dim boxA = TargetA.GetComponent(Of BoxCollider)
        Dim boxB = TargetB.GetComponent(Of BoxCollider)

        Dim bodyA = boxA.GetComponent(Of Rigidbody)
        Dim bodyB = boxB.GetComponent(Of Rigidbody)

        Dim boxA_dotRight = Vector3.Dot(boxA.transform.right * boxA.size.X, Vector3.Right)
        Dim boxA_dotUp = Vector3.Dot(boxA.transform.up * boxA.size.Y, Vector3.Up)
        Dim boxA_dotFront = Vector3.Dot(boxA.transform.forward * boxA.size.Z, Vector3.Forward)

        Dim boxB_dotRight = Vector3.Dot(boxB.transform.right * boxA.size.X, Vector3.Right)
        Dim boxB_dotUp = Vector3.Dot(boxB.transform.up * boxB.size.Y, Vector3.Up)
        Dim boxB_dotFront = Vector3.Dot(boxB.transform.forward * boxB.size.Z, Vector3.Forward)

        Dim boxA_maxRight = boxA.Location + Vector3.Right * boxA_dotRight
        Dim boxA_minRight = boxA.Location - Vector3.Right * boxA_dotRight

        Dim boxA_maxUp = boxA.Location + Vector3.Up * boxA_dotUp
        Dim boxA_minUp = boxA.Location - Vector3.Up * boxA_dotUp

        Dim boxA_maxFront = boxA.Location + Vector3.Forward * boxA_dotFront
        Dim boxA_minFront = boxA.Location - Vector3.Forward * boxA_dotFront

        Dim boxB_maxRight = boxB.Location + Vector3.Right * boxA_dotRight
        Dim boxB_minRight = boxB.Location - Vector3.Right * boxA_dotRight

        Dim boxB_maxUp = boxB.Location + Vector3.Up * boxA_dotUp
        Dim boxB_minUp = boxB.Location - Vector3.Up * boxA_dotUp

        Dim boxB_maxFront = boxB.Location + Vector3.Forward * boxA_dotFront
        Dim boxB_minFront = boxB.Location - Vector3.Forward * boxA_dotFront

        Debug.Log("Psudodistance X:{0} Y:{1}", Psudodistance(boxA_minRight.X, boxA_maxRight.X, boxB_minRight.X, boxB_maxRight.X, bodyA.velocity.X, bodyB.velocity.X, Time.deltaTime),
                                                Psudodistance(boxA_minUp.Y, boxA_maxUp.Y, boxB_minUp.Y, boxB_maxUp.Y, bodyA.velocity.Y, bodyB.velocity.Y, Time.deltaTime))
        If Psudodistance(boxA_minRight.X, boxA_maxRight.X, boxB_minRight.X, boxB_maxRight.X, bodyA.velocity.X, bodyB.velocity.X, Time.deltaTime) < 0 Then 'boxA.size.X + boxB.size.X Then
            Debug.Log("Contact Dected")
        End If

    End Sub

    Private Function Psudodistance(p0 As Single, p1 As Single, q0 As Single, q1 As Single, u As Single, v As Single, t As Single) As Single
        'Return ((p1 - p0) * 0.5 + (q1 - q0) * 0.5) - ((p0 + p1) * 0.5 - (q0 + q1) * 0.5)
        Return (u - v) * (u - v) * (t * t) + 2 * (u - v) * ((p0 - p1) / 2.0F - (q1 + q0) / 2.0F) * t + (p0 - q1) * (p1 - q0)
    End Function


End Class