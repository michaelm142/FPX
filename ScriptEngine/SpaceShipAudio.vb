Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Audio
Imports FPX
Imports System.Xml

Public Class SpaceShipAudio
    Inherits Component

    Public accelerateClip As SoundEffect
    Public decelerateClip As SoundEffect
    Public ambientClip As SoundEffect

    Public accelerateClipName As String
    Public decelerateClipName As String
    Public ambientClipName As String

    Private speedPrev As Single
    Private threshold As Single

    Private positionPrev As Vector3

    Private movementSource As AudioSource
    Private ambientSource As AudioSource

    Public Sub Start()
        movementSource = gameObject.AddComponent(Of AudioSource)
        ambientSource = gameObject.AddComponent(Of AudioSource)
        ambientSource.Loop = True

        accelerateClip = GameCore.content.Load(Of SoundEffect)(accelerateClipName)
        decelerateClip = GameCore.content.Load(Of SoundEffect)(decelerateClipName)
        ambientClip = GameCore.content.Load(Of SoundEffect)(ambientClipName)

        ambientSource.clip = ambientClip
        ambientSource.Play()

        positionPrev = position
    End Sub

    Public Sub Update(gameTime As GameTime)
        Dim deltaPosition = positionPrev - position
        Dim speed = deltaPosition.Length()

        If speed > threshold And speedPrev < threshold Then
            movementSource.clip = accelerateClip
            movementSource.Play()
        End If
        If speed < threshold And speedPrev > threshold Then
            movementSource.clip = decelerateClip
            movementSource.Play()
        End If

        positionPrev = position
        speedPrev = speed
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        MyBase.LoadXml(element)

        Dim accelerateAttr = If(element.SelectSingleNode("AccelerateClip") IsNot Nothing, element.SelectSingleNode("AccelerateClip").Attributes("Filename"), Nothing)
        Dim decelerateAttr = If(element.SelectSingleNode("DecelerateClip") IsNot Nothing, element.SelectSingleNode("DecelerateClip").Attributes("Filename"), Nothing)
        Dim ambientAttr = If(element.SelectSingleNode("AmbientClip") IsNot Nothing, element.SelectSingleNode("AmbientClip").Attributes("Filename"), Nothing)
        Dim thresholdNode = element.SelectSingleNode("Threshold")

        If Not accelerateAttr Is Nothing Then
            accelerateClipName = accelerateAttr.Value
        End If
        If Not decelerateAttr Is Nothing Then
            decelerateClipName = decelerateAttr.Value
        End If
        If Not ambientAttr Is Nothing Then
            ambientClipName = ambientAttr.Value
        End If
        If Not thresholdNode Is Nothing Then
            threshold = Single.Parse(thresholdNode.InnerText)
        End If
    End Sub
End Class
