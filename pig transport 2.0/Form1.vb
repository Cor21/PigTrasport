Public Class Form1   ' vista

    Inherits Form

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim controller As New Controller()
        controller.Proceso(txtIbague.Text, txtmelgar.Text, Txtespinal.Text, Txtguamo.Text, txtlibano.Text, Txthonda.Text)
        controller.almacenar()
    End Sub

    Public Sub ActualizarResultados(combinaciones As Dictionary(Of String, Combinacion))

        For Each combinacion In combinaciones
            Console.WriteLine("Combinación: " & combinacion.Key)
            Console.WriteLine("Total Valor: " & combinacion.Value.TotalValor)
            Console.WriteLine("Tiempo: " & combinacion.Value.Tiempo)
            Console.WriteLine("Ruta: " & combinacion.Value.Ruta)
            Console.WriteLine()
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim controller As New Controller()
        controller.borrar()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form2.Show()
        Me.Hide()
    End Sub
End Class



Public Class Combinacion   ' modelo
    Public Property TotalValor As Integer
    Public Property Tiempo As Integer
    Public Property Ruta As String

    Public Sub New(totalValor As Integer, tiempo As Integer, ruta As String)
        Me.TotalValor = totalValor
        Me.Tiempo = tiempo
        Me.Ruta = ruta
    End Sub
End Class





Public Class Controller   ' Controlador

    Private peaje_espinal As Integer = 11500 * 2
    Private peaje_melgar As Integer = 11700 * 2
    Private peaje_alvarado As Integer = 10700 * 2
    Private peaje_cambao As Integer = 10000 * 2
    Private peaje_girardot As Integer = 10800 * 2
    Private conductor As Integer = 40000
    Private combustible_completo As Integer = 70000
    Private combustible_medio As Integer = 70000 / 2
    Private combutible_cuarto As Integer = combustible_medio / 2

    Public Sub Proceso(ibague As String, melgar As String, espinal As String, guamo As String, libano As String, honda As String)

        Dim ibagueValue As Integer = Integer.Parse(ibague)
        Dim melgarValue As Integer = Integer.Parse(melgar)
        Dim espinalValue As Integer = Integer.Parse(espinal)
        Dim guamoValue As Integer = Integer.Parse(guamo)
        Dim libanoValue As Integer = Integer.Parse(libano)
        Dim hondaValue As Integer = Integer.Parse(honda)

        Dim resultado As Integer = Sumar(ibagueValue, melgarValue, espinalValue, guamoValue, libanoValue, hondaValue)
        Dim limite As Integer = ibagueValue + melgarValue + espinalValue + guamoValue + libanoValue + hondaValue

        If limite < 25 Then

            Dim combinaciones As New Dictionary(Of String, Combinacion)()

            combinaciones.Add("ibague", New Combinacion(resultado - (conductor), 1, "no tiene"))
            combinaciones.Add("espinal", New Combinacion(resultado - (conductor + peaje_espinal + combutible_cuarto), 1 + 2, "Sur"))
            combinaciones.Add("guamo", New Combinacion(resultado - (conductor + peaje_espinal + combutible_cuarto), 1 + 1 + 2, "Sur"))
            combinaciones.Add("melgar", New Combinacion(resultado - (conductor + peaje_espinal + peaje_melgar + combustible_medio), 1 + 3, "Sur"))
            combinaciones.Add("libano", New Combinacion(resultado - (conductor + peaje_alvarado + combustible_completo), 1 + 4, "Norte"))
            combinaciones.Add("honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_cambao + combustible_completo), 1 + 4, "Norte"))
            combinaciones.Add("ibague,espinal", New Combinacion(resultado - (conductor + peaje_espinal + combutible_cuarto), 1 + 1 + 2, "sur"))
            combinaciones.Add("ibague,guamo", New Combinacion(resultado - (conductor + peaje_espinal + combutible_cuarto), 1 + 1 + 2, "sur"))
            combinaciones.Add("ibague,melgar", New Combinacion(resultado - (conductor + peaje_espinal + peaje_melgar + combustible_medio), 1 + 1 + 4, "sur"))
            combinaciones.Add("melgar,espinal", New Combinacion(resultado - (conductor + peaje_espinal + peaje_melgar + combustible_medio), 1 + 1 + 4, "sur"))
            combinaciones.Add("melgar,guamo", New Combinacion(resultado - (conductor + peaje_espinal + peaje_melgar + combustible_medio), 1 + 1 + 4, "sur"))
            combinaciones.Add("espinal,guamo", New Combinacion(resultado - (conductor + peaje_espinal + combutible_cuarto), 1 + 1 + 2, "sur"))
            combinaciones.Add("ibague,espinal,guamo", New Combinacion(resultado - (conductor + peaje_espinal + combutible_cuarto), 1 + 1 + 1 + 2, "sur"))
            combinaciones.Add("ibague,melgar,espinal", New Combinacion(resultado - (conductor + peaje_espinal + peaje_melgar + combustible_medio), 1 + 1 + 1 + 4, "sur"))
            combinaciones.Add("ibague,melgar,guamo", New Combinacion(resultado - (conductor + peaje_espinal + peaje_melgar + combustible_medio), 1 + 1 + 1 + 4, "sur"))
            combinaciones.Add("melgar,espinal,guamo", New Combinacion(resultado - (conductor + peaje_espinal + peaje_melgar + combustible_medio), 1 + 1 + 1 + 1 + 4, "sur"))
            combinaciones.Add("ibague,melgar,espinal,guamo", New Combinacion(resultado - (conductor + peaje_espinal + peaje_melgar + combustible_medio), 1 + 1 + 1 + 1 + 4, "sur"))
            combinaciones.Add("ibague,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_cambao + combustible_completo), 1 + 1 + 4, "Norte"))
            combinaciones.Add("ibague,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_cambao + combustible_completo), 1 + 1 + 4, "Norte"))
            combinaciones.Add("honda,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_cambao + combustible_completo), 1 + 1 + 2 + 1 + 2, "Norte"))
            combinaciones.Add("ibague,libano,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_cambao + combustible_completo), 1 + 1 + 4, "Norte"))
            combinaciones.Add("espinal,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + combustible_completo), 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("guamo,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + combustible_completo), 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("melgar,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + combustible_completo), 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("melgar,honda", New Combinacion(resultado - (conductor + peaje_girardot + peaje_espinal + peaje_alvarado + peaje_melgar + combustible_completo), 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("espinal,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + combustible_completo), 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("guamo,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + combustible_completo), 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("melgar,espinal,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("espinal,guamo,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("melgar,guamo,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("melgar,guamo,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + peaje_cambao + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("melgar,espinal,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("espinal,guamo,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_cambao + peaje_girardot + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("melgar,espinal,guamo,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo), 1 + 1 + 1 + 1 + 1 + 2 + 6, "Norte-Sur"))
            combinaciones.Add("melgar,espinal,guamo,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo), 1 + 1 + 1 + 1 + 2 + 6, "Norte-Sur"))
            combinaciones.Add("melgar,espinal,guamo,libano,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo), 1 + 1 + 1 + 1 + 2 + 6, "Norte-Sur"))
            combinaciones.Add("ibague,espinal,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,guamo,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,melgar,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,melgar,honda", New Combinacion(resultado - (conductor + peaje_girardot + peaje_espinal + peaje_alvarado + peaje_melgar + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,espinal,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,guamo,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + combustible_completo), 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,melgar,espinal,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo), 1 + 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,espinal,guamo,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + combustible_completo), 1 + 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,melgar,guamo,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo), 1 + 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,melgar,guamo,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + peaje_cambao + combustible_completo), 1 + 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,melgar,espinal,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo), 1 + 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,espinal,guamo,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_cambao + peaje_girardot + combustible_completo), 1 + 1 + 1 + 1 + 2 + 4, "Norte-Sur"))
            combinaciones.Add("ibague,melgar,espinal,guamo,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo), 1 + 1 + 1 + 1 + 1 + 2 + 6, "Norte-Sur"))
            combinaciones.Add("ibague,melgar,espinal,guamo,libano", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo), 1 + 1 + 1 + 1 + 1 + 2 + 6, "Norte-Sur"))
            combinaciones.Add("ibague,melgar,espinal,guamo,libano,honda", New Combinacion(resultado - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo), 1 + 1 + 1 + 1 + 1 + 1 + 2 + 6, "Norte-Sur"))

            Dim combinacionActual As New List(Of String)()

            If ibague > 0 Then combinacionActual.Add("ibague")
            If melgar > 0 Then combinacionActual.Add("melgar")
            If espinal > 0 Then combinacionActual.Add("espinal")
            If guamo > 0 Then combinacionActual.Add("guamo")
            If libano > 0 Then combinacionActual.Add("libano")
            If honda > 0 Then combinacionActual.Add("honda")

            Dim combinacionClave As String = String.Join(",", combinacionActual)


            If combinaciones.ContainsKey(combinacionClave) Then


                Dim combinacion As Combinacion = combinaciones(combinacionClave)
                Dim totalValor As Integer = combinacion.TotalValor
                Dim tiempo As Integer = combinacion.Tiempo
                Dim ruta As String = combinacion.Ruta

                If totalValor > 0 And tiempo < 12 Then
                    Form1.Txtganancia.Text = totalValor.ToString()
                    Form1.Txt_tiempo.Text = tiempo.ToString()
                    Form1.Txt_gastos.Text = ruta.ToString()
                    Form1.txtviablidad.Text = "viable"
                    Form1.txtviablidad.BackColor = Color.Green
                Else
                    Form1.Txtganancia.Text = totalValor.ToString()
                    Form1.Txt_tiempo.Text = tiempo.ToString()
                    Form1.Txt_gastos.Text = ruta.ToString()
                    Form1.txtviablidad.Text = "inviable"
                    Form1.txtviablidad.BackColor = Color.Red
                End If


            Else
                Form1.Txtganancia.Text = "Otras combinaciones"
            End If

            Dim vista As New Form1()
            vista.ActualizarResultados(combinaciones)
        Else
            MsgBox("Error: No se puede acceder a más de 25 cerdos")
        End If
    End Sub

    Function almacenar()


        Dim almacenados(10) As String


        Dim newRow As DataGridViewRow = New DataGridViewRow()
        newRow.CreateCells(Form2.DataGridView1)
        newRow.Cells(0).Value = Form1.txtIbague.Text
        newRow.Cells(1).Value = Form1.Txtespinal.Text
        newRow.Cells(2).Value = Form1.Txtguamo.Text
        newRow.Cells(3).Value = Form1.txtmelgar.Text
        newRow.Cells(4).Value = Form1.txtlibano.Text
        newRow.Cells(5).Value = Form1.Txthonda.Text
        newRow.Cells(6).Value = DateTime.Today
        newRow.Cells(7).Value = Form1.Txt_tiempo.Text
        newRow.Cells(8).Value = Form1.Txt_gastos.Text
        newRow.Cells(9).Value = Form1.Txtganancia.Text
        newRow.Cells(10).Value = Form1.txtviablidad.Text
        Form2.DataGridView1.Rows.Add(newRow)

        Return almacenados

    End Function
    Public Sub borrar()
        Form1.txtIbague.Text = "0"
        Form1.Txtespinal.Text = "0"
        Form1.Txtguamo.Text = "0"
        Form1.txtmelgar.Text = "0"
        Form1.txtlibano.Text = "0"
        Form1.Txthonda.Text = "0"
        Form1.Txt_tiempo.Clear()
        Form1.Txtganancia.Clear()
        Form1.txtviablidad.Clear()
        Form1.Txt_gastos.Clear()
        Form1.txtviablidad.BackColor = Color.White
    End Sub

    Public Function Sumar(ByVal ParamArray valores() As Integer) As Integer

        Dim ibagueValue As Integer
        Dim melgarValue As Integer
        Dim espinalValue As Integer
        Dim guamoValue As Integer
        Dim libanoValue As Integer
        Dim hondaValue As Integer

        Integer.TryParse(Form1.txtIbague.Text, ibagueValue)
        Integer.TryParse(Form1.txtmelgar.Text, melgarValue)
        Integer.TryParse(Form1.Txtespinal.Text, espinalValue)
        Integer.TryParse(Form1.Txtguamo.Text, guamoValue)
        Integer.TryParse(Form1.txtlibano.Text, libanoValue)
        Integer.TryParse(Form1.Txthonda.Text, hondaValue)

        Dim suma As Integer = Val(ibagueValue * 10000) + Val(melgarValue * 20000) + Val(espinalValue * 15000) + Val(guamoValue * 18000) + Val(libanoValue * 25000) + Val(hondaValue * 25000)

        Return suma
    End Function


End Class

