Imports Microsoft.Xna.Framework
Imports FPX.ComponentModel

Public Class MatrixTester
    Inherits Component

    Public Sub Start()
        Dim m = Matrix.CreateScale(2.3F, 0.1F, -1.25F) * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(30), MathHelper.ToRadians(75), MathHelper.ToRadians(15)) * Matrix.CreateTranslation(New Vector3(-15, 3, 24))
        Console.WriteLine("Rotation and translation matrix:")
        WriteMatrix(m)
        Console.WriteLine()


        m = LinearAlgebraUtil.SetRowColumn(m, 1, 3, 5.0F)
        Console.WriteLine("Set <1, 3> to 5")
        WriteMatrix(m)
        Console.WriteLine()
        m = LinearAlgebraUtil.SetRowColumn(m, 3, 1, 2.0)
        Console.WriteLine("Set <3, 1> to 2")
        WriteMatrix(m)
        Console.WriteLine()

        Console.WriteLine("Value at <1, 1> is:{0}", m.GetRowColumn(1, 1))
        Console.WriteLine("Value at <2, 2> is:{0}", m.GetRowColumn(2, 2))
        Console.WriteLine("Value at <3, 1> is:{0}", m.GetRowColumn(3, 1))

        Console.WriteLine("Set row 0 to <0, 0, 1>")
        m = LinearAlgebraUtil.SetMatrixRow(m, 0, Vector3.Forward.ToVector4())
        Console.WriteLine(m.GetRow(0).ToString())
    End Sub

    Private Sub WriteMatrix(m As Matrix)
        For y = 1 To 4
            Console.Write("|")
            For x = 1 To 4
                Console.Write("{0}, ", Math.Round(m.GetRowColumn(x, y), 2))
            Next
            Console.Write("|")
            Console.WriteLine()
        Next
    End Sub
End Class
