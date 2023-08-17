Imports System.Data.OleDb

Public Class Form2
    Private Sub volver_Click(sender As Object, e As EventArgs) Handles volver.Click
        Form1.Show()
        Me.Hide()
    End Sub



    Private _controlador As CerdoController

    Public Sub New()
        InitializeComponent()

        Dim directorioApp As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

        Dim rutaBaseDatos As String = System.IO.Path.Combine(directorioApp, "pigtransport2.accdb")

        Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & rutaBaseDatos & ";Persist Security Info=False"

        _controlador = New CerdoController(connectionString)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        _controlador.Importar(DataGridView1)
        MessageBox.Show("Datos importados correctamente.")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        _controlador.borrar(DataGridView1)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        _controlador.actualizar(DataGridView1)
    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        _controlador.cargar(DataGridView1)
    End Sub
End Class


Public Class modelo
    Public connectionString As String

    Public Sub New(connectionString As String)
        Me.connectionString = connectionString
    End Sub

    Public Sub Importar(dataGridView1 As DataGridView)


        Dim connection As New OleDbConnection(connectionString)

        Try
            connection.Open()

            For Each row As DataGridViewRow In dataGridView1.Rows
                Dim isEmptyRow As Boolean = True

                For Each cell As DataGridViewCell In row.Cells
                    ' Verificar si la celda está en blanco
                    If cell.Value IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(cell.Value.ToString()) Then
                        isEmptyRow = False
                        Exit For
                    End If
                Next

                ' Omitir la fila si está en blanco
                If Not isEmptyRow Then
                    Dim cmd As OleDbCommand = connection.CreateCommand()
                    cmd.CommandText = "INSERT INTO cerdos (ibague, espinal, guamo, melgar, libano, honda, fecha, tiempo, ruta, ganancia, viabilidad) VALUES (@ibague, @espinal, @guamo, @melgar, @libano, @honda, @fecha, @tiempo, @ruta, @ganancia, @viabilidad)"

                    cmd.Parameters.AddWithValue("@ibague", row.Cells(0).Value)
                    cmd.Parameters.AddWithValue("@espinal", row.Cells(1).Value)
                    cmd.Parameters.AddWithValue("@guamo", row.Cells(2).Value)
                    cmd.Parameters.AddWithValue("@melgar", row.Cells(3).Value)
                    cmd.Parameters.AddWithValue("@libano", row.Cells(4).Value)
                    cmd.Parameters.AddWithValue("@honda", row.Cells(5).Value)
                    cmd.Parameters.AddWithValue("@fecha", row.Cells(6).Value)
                    cmd.Parameters.AddWithValue("@tiempo", row.Cells(7).Value)
                    cmd.Parameters.AddWithValue("@ruta", row.Cells(8).Value)
                    cmd.Parameters.AddWithValue("@ganancia", row.Cells(9).Value)
                    cmd.Parameters.AddWithValue("@viabilidad", row.Cells(10).Value)
                    cmd.ExecuteNonQuery()
                End If
            Next

            MessageBox.Show("Datos guardados correctamente.")

        Catch ex As Exception
            MessageBox.Show("Error al guardar los datos: " & ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    Public Sub cargar(dataGridView1 As DataGridView)

        Dim fechaSeleccionada As Date = Form2.datafecha.Value
        Dim dataTable As DataTable = ObtenerDatosPorFecha(fechaSeleccionada)

        dataGridView1.Columns.Clear()
        DataGridView1.DataSource = dataTable


        dataGridView1.Columns(1).HeaderText = "Ibague"
        DataGridView1.Columns(2).HeaderText = "Espinal"
        DataGridView1.Columns(3).HeaderText = "Guamo"
        DataGridView1.Columns(4).HeaderText = "Melgar"
        DataGridView1.Columns(5).HeaderText = "Libano"
        DataGridView1.Columns(6).HeaderText = "Honda"
        DataGridView1.Columns(7).HeaderText = "Fecha"
        DataGridView1.Columns(8).HeaderText = "Tiempo"
        DataGridView1.Columns(9).HeaderText = "Ruta"
        DataGridView1.Columns(10).HeaderText = "Ganancia"
        dataGridView1.Columns(11).HeaderText = "Viabilida"

    End Sub
    Public Function ObtenerDatosPorFecha(fecha As Date) As DataTable
        Dim query As String = "SELECT * FROM cerdos WHERE fecha >= @fechaInicio AND fecha < @fechaFin"


        ' Definir las fechas de inicio y fin del rango deseado
        Dim fechaInicio As Date = fecha.Date
        Dim fechaFin As Date = fecha.Date.AddDays(1)

        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(query, connection)
                command.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                command.Parameters.AddWithValue("@fechaFin", fechaFin)

                Try
                    connection.Open()
                    Dim adapter As New OleDbDataAdapter(command)
                    Dim dataTable As New DataTable()
                    adapter.Fill(dataTable)
                    Return dataTable
                Catch ex As Exception
                    Throw New Exception("Error al cargar los datos: " & ex.Message)
                End Try
            End Using
        End Using
    End Function

    Public Sub borrar(dataGridView1 As DataGridView)
        If dataGridView1.SelectedRows.Count > 0 Then
            Dim id As Integer = CInt(dataGridView1.SelectedRows(0).Cells("ID").Value)
            Dim result As DialogResult = MessageBox.Show("¿Está seguro que desea eliminar la fila con ID " & id & "?", "Confirmar eliminación", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then
                Try

                    Using connection As New OleDbConnection(connectionString)
                        connection.Open()
                        Dim cmd As OleDbCommand = connection.CreateCommand()
                        cmd.CommandText = "DELETE * FROM cerdos WHERE id=@id"
                        cmd.Parameters.AddWithValue("@id", id)
                        cmd.ExecuteNonQuery()
                    End Using
                    MessageBox.Show("Fila eliminada correctamente.")
                Catch ex As Exception
                    MessageBox.Show("Error al eliminar la fila: " & ex.Message)
                End Try
            End If
        Else
            MessageBox.Show("Seleccione una fila para eliminar.")
        End If
    End Sub

    Public Sub actualizar(dataGridView1 As DataGridView)
        Dim ruta As String
        ruta = My.Application.Info.DirectoryPath & "\pigtransport2.accdb"
        Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & (Ruta) & "; Persist Security Info=False"

        If dataGridView1.SelectedRows.Count > 0 Then
            ' Obtener la fila seleccionada
            Dim selectedRow As DataGridViewRow = dataGridView1.SelectedRows(0)

            ' Obtener los valores de las celdas que se pueden editar
            Dim ibagueValue As Integer = CInt(selectedRow.Cells(1).Value)
            Dim espinalValue As Integer = CInt(selectedRow.Cells(2).Value)
            Dim guamoValue As Integer = CInt(selectedRow.Cells(3).Value)
            Dim melgarValue As Integer = CInt(selectedRow.Cells(4).Value)
            Dim libanoValue As Integer = CInt(selectedRow.Cells(5).Value)
            Dim hondaValue As Integer = CInt(selectedRow.Cells(6).Value)

            ' Editar los valores si son mayores a 0
            If ibagueValue > 0 Then
                Dim ibagueInput As String = InputBox("Ingrese el nuevo valor para Ibague:", "Editar Ibague", ibagueValue.ToString())
                If Not String.IsNullOrWhiteSpace(ibagueInput) AndAlso IsNumeric(ibagueInput) AndAlso CInt(ibagueInput) > 0 Then
                    selectedRow.Cells(1).Value = CInt(ibagueInput)
                End If
            End If

            If espinalValue > 0 Then
                Dim espinalInput As String = InputBox("Ingrese el nuevo valor para Espinal:", "Editar Espinal", espinalValue.ToString())
                If Not String.IsNullOrWhiteSpace(espinalInput) AndAlso IsNumeric(espinalInput) AndAlso CInt(espinalInput) > 0 Then
                    selectedRow.Cells(2).Value = CInt(espinalInput)
                End If
            End If

            If guamoValue > 0 Then
                Dim guamoInput As String = InputBox("Ingrese el nuevo valor para Guamo:", "Editar Guamo", guamoValue.ToString())
                If Not String.IsNullOrWhiteSpace(guamoInput) AndAlso IsNumeric(guamoInput) AndAlso CInt(guamoInput) > 0 Then
                    selectedRow.Cells(3).Value = CInt(guamoInput)
                End If
            End If

            If melgarValue > 0 Then
                Dim melgarInput As String = InputBox("Ingrese el nuevo valor para Melgar:", "Editar Melgar", melgarValue.ToString())
                If Not String.IsNullOrWhiteSpace(melgarInput) AndAlso IsNumeric(melgarInput) AndAlso CInt(melgarInput) > 0 Then
                    selectedRow.Cells(4).Value = CInt(melgarInput)
                End If
            End If

            If libanoValue > 0 Then
                Dim libanoInput As String = InputBox("Ingrese el nuevo valor para Libano:", "Editar Libano", libanoValue.ToString())
                If Not String.IsNullOrWhiteSpace(libanoInput) AndAlso IsNumeric(libanoInput) AndAlso CInt(libanoInput) > 0 Then
                    selectedRow.Cells(5).Value = CInt(libanoInput)
                End If
            End If

            If hondaValue > 0 Then
                Dim hondaInput As String = InputBox("Ingrese el nuevo valor para honda:", "Editar Honda", hondaValue.ToString())
                If Not String.IsNullOrWhiteSpace(hondaInput) AndAlso IsNumeric(hondaInput) AndAlso CInt(hondaInput) > 0 Then
                    selectedRow.Cells(6).Value = CInt(hondaInput)
                End If

            End If

            Dim fechaValue As DateTime = Convert.ToDateTime(selectedRow.Cells(7).Value)

            selectedRow.Cells(7).Value = DateTime.Today

            Dim resultado As Integer = Sumar(dataGridView1)



            If resultado > 0 Then
                selectedRow.Cells(11).Value = "viable"
            Else
                selectedRow.Cells(11).Value = "inviable"
            End If

            Try

                Using connection As New OleDbConnection(connectionString)
                    connection.Open()

                    Dim cmd As OleDbCommand = connection.CreateCommand()
                    cmd.CommandText = "UPDATE cerdos SET ibague = @ibague, espinal = @espinal, guamo = @guamo, melgar = @melgar, libano = @libano, honda = @honda, fecha = @fecha ,viabilidad = @viabilidad WHERE id = @id"

                    ' Parámetros para la actualización
                    cmd.Parameters.AddWithValue("@ibague", CInt(selectedRow.Cells(1).Value))
                    cmd.Parameters.AddWithValue("@espinal", CInt(selectedRow.Cells(2).Value))
                    cmd.Parameters.AddWithValue("@guamo", CInt(selectedRow.Cells(3).Value))
                    cmd.Parameters.AddWithValue("@melgar", CInt(selectedRow.Cells(4).Value))
                    cmd.Parameters.AddWithValue("@libano", CInt(selectedRow.Cells(5).Value))
                    cmd.Parameters.AddWithValue("@honda", CInt(selectedRow.Cells(6).Value))
                    cmd.Parameters.AddWithValue("@fecha", fechaValue)
                    cmd.Parameters.AddWithValue("@viabilidad", (selectedRow.Cells(11).Value))
                    cmd.Parameters.AddWithValue("@id", CInt(selectedRow.Cells(0).Value))
                    cmd.ExecuteNonQuery()
                End Using

                MessageBox.Show("Los cambios se han actualizado correctamente en la base de datos.", "Actualización Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error al actualizar los cambios en la base de datos: " & ex.Message, "Error de Actualización", MessageBoxButtons.OK, MessageBoxIcon.Error)


            End Try
        Else
            MessageBox.Show("Seleccione una fila para actualizar.")
        End If
    End Sub

    Public Function Sumar(dataGridView1 As DataGridView) As Integer
        Dim peaje_espinal As Integer = 11500 * 2
        Dim peaje_melgar As Integer = 11700 * 2
        Dim peaje_alvarado As Integer = 10700 * 2
        Dim peaje_cambao As Integer = 10000 * 2
        Dim peaje_girardot As Integer = 10800 * 2
        Dim conductor As Integer = 40000
        Dim combustible_completo As Integer = 70000
        Dim combustible_medio As Integer = 70000 / 2
        Dim combutible_cuarto As Integer = combustible_medio / 2

        Dim selectedRow As DataGridViewRow = dataGridView1.SelectedRows(0)

        Dim ibagueValue As Integer = CInt(selectedRow.Cells(1).Value)
        Dim espinalValue As Integer = CInt(selectedRow.Cells(2).Value)
        Dim guamoValue As Integer = CInt(selectedRow.Cells(3).Value)
        Dim melgarValue As Integer = CInt(selectedRow.Cells(4).Value)
        Dim libanoValue As Integer = CInt(selectedRow.Cells(5).Value)
        Dim hondaValue As Integer = CInt(selectedRow.Cells(6).Value)

        Dim suma As Integer = (Val(ibagueValue * 10000) + Val(melgarValue * 20000) + Val(espinalValue * 15000) + Val(guamoValue * 18000) + Val(libanoValue * 25000) + Val(hondaValue * 25000))
        Dim tiempo As Integer
        Dim resultado As Integer
        If ibagueValue > 0 Then
            resultado = suma - 40000
        ElseIf espinalValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + combutible_cuarto)
        ElseIf guamoValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + combutible_cuarto)
        ElseIf melgarValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + peaje_melgar + combustible_medio)
        ElseIf libanoValue > 0 Then
            resultado = -suma - (conductor + peaje_alvarado + combustible_completo)
        ElseIf hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_cambao + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso espinalValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + combutible_cuarto)
        ElseIf ibagueValue > 0 AndAlso guamoValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + combutible_cuarto)
        ElseIf ibagueValue > 0 AndAlso melgarValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + peaje_melgar + combustible_medio)
        ElseIf espinalValue > 0 AndAlso melgarValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + peaje_melgar + combustible_medio)
        ElseIf guamoValue > 0 AndAlso melgarValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + peaje_melgar + combustible_medio)
        ElseIf espinalValue > 0 AndAlso guamoValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + combutible_cuarto)
        ElseIf ibagueValue > 0 AndAlso espinalValue > 0 AndAlso guamoValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + combutible_cuarto)
        ElseIf ibagueValue > 0 AndAlso espinalValue > 0 AndAlso melgarValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + peaje_melgar + combustible_medio)
        ElseIf guamoValue > 0 AndAlso espinalValue > 0 AndAlso melgarValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + peaje_melgar + combustible_medio)
        ElseIf ibaguevalue > 0 AndAlso guamoValue > 0 AndAlso espinalValue > 0 AndAlso melgarValue > 0 Then
            resultado = suma - (conductor + peaje_espinal + peaje_melgar + combustible_medio)
        ElseIf ibagueValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_cambao + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_cambao + combustible_completo)
        ElseIf hondaValue > 0 AndAlso libanovalue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_cambao + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso hondaValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_cambao + combustible_completo)
        ElseIf espinalValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + combustible_completo)
        ElseIf guamoValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + combustible_completo)
        ElseIf melgarValue > 0 AndAlso Libanovalue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + combustible_completo)
        ElseIf melgarValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_girardot + peaje_espinal + peaje_alvarado + peaje_melgar + combustible_completo)
        ElseIf espinalValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + combustible_completo)
        ElseIf guamoValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + combustible_completo)
        ElseIf melgarValue > 0 AndAlso espinalValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo)
        ElseIf espinalValue > 0 AndAlso guamoValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + combustible_completo)
        ElseIf melgarValue > 0 AndAlso guamoValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo)
        ElseIf melgarValue > 0 AndAlso guamoValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + peaje_cambao + combustible_completo)
        ElseIf melgarValue > 0 AndAlso espinalvalue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo)
        ElseIf espinalValue > 0 AndAlso guamoValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_cambao + peaje_girardot + combustible_completo)
        ElseIf melgarValue > 0 AndAlso espinalvalue > 0 AndAlso hondaValue > 0 AndAlso guamoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo)
        ElseIf melgarValue > 0 AndAlso espinalvalue > 0 AndAlso libanoValue > 0 AndAlso guamoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo)
        ElseIf melgarValue > 0 AndAlso espinalvalue > 0 AndAlso libanoValue > 0 AndAlso guamoValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso espinalValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso guamoValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso melgarValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso melgarValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_girardot + peaje_espinal + peaje_alvarado + peaje_melgar + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso espinalValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso guamoValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso espinalValue > 0 AndAlso melgarValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso espinalValue > 0 AndAlso guamoValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso melgarValue > 0 AndAlso guamoValue > 0 AndAlso libanoValue > 0 Then
            resultado = suma > (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso melgarValue > 0 AndAlso guamoValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + peaje_cambao + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso melgarValue > 0 AndAlso espinalValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_girardot + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso guamoValue > 0 AndAlso espinalValue > 0 AndAlso hondaValue > 0 Then
            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_cambao + peaje_girardot + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso guamoValue > 0 AndAlso espinalValue > 0 AndAlso melgarValue > 0 AndAlso hondaValue > 0 Then

            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso guamoValue > 0 AndAlso espinalValue > 0 AndAlso melgarValue > 0 AndAlso libanoValue > 0 Then

            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo)
        ElseIf ibagueValue > 0 AndAlso guamoValue > 0 AndAlso espinalValue > 0 AndAlso melgarValue > 0 AndAlso libanoValue > 0 AndAlso hondaValue > 0 Then

            resultado = suma - (conductor + peaje_alvarado + peaje_espinal + peaje_melgar + peaje_cambao + peaje_girardot + combustible_completo)

        End If


        Return resultado
    End Function

End Class

Public Class CerdoController
    Private _modelo As modelo

    Public Sub New(connectionString As String)
        _modelo = New modelo(connectionString)

    End Sub

    Public Sub Importar(dataGridView1 As DataGridView)
        _modelo.Importar(dataGridView1)
    End Sub

    Public Sub cargar(dataGridView1 As DataGridView)
        _modelo.cargar(dataGridView1)
    End Sub

    Public Sub borrar(dataGridView1 As DataGridView)
        _modelo.borrar(dataGridView1)
    End Sub

    Public Sub actualizar(dataGridView1 As DataGridView)
        _modelo.actualizar(dataGridView1)
    End Sub
End Class
