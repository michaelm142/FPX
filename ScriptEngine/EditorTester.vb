Imports System.Xml
Imports ComponentModel

Public Class EditorTester
    Inherits Component

    Public FloatValue As Single
    Public IntValue As Integer
    Public StringValue As String

    Public Sub LoadXml(node As XmlElement)
        Dim floatNode = node.SelectSingleNode("FloatValue")
        Dim intNode = node.SelectSingleNode("IntValue")
        Dim stringNode = node.SelectSingleNode("StringValue")

        If Not floatNode Is Nothing Then
            FloatValue = Single.Parse(floatNode.InnerText)
        End If

        If Not intNode Is Nothing Then
            IntValue = Integer.Parse(intNode.InnerText)
        End If

        If Not stringNode Is Nothing Then
            StringValue = stringNode.InnerText
        End If
    End Sub

End Class
