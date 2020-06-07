Imports System.Data.SQLite
Public NotInheritable Class Database
    Public Sub New(connectionString As String)
        Me.ConnectionString = connectionString
    End Sub
    Public Property ConnectionString() As String
    Private Shared Sub AssignParameters(cmd As SQLiteCommand, ByVal cmdParameters() As SQLiteParameter)
        If (cmdParameters Is Nothing) Then Exit Sub
        For Each p As SQLiteParameter In cmdParameters
            cmd.Parameters.Add(p)
        Next
    End Sub
    Private Shared Sub AssignParameters(ByVal cmd As System.Data.SQLite.SQLiteCommand, ByVal parameterValues() As Object)
        If Not (cmd.Parameters.Count - 1 = parameterValues.Length) Then Throw New ApplicationException("Stored procedure's parameters and parameter values does not match.")
        Dim i As Integer
        For Each param As System.Data.SQLite.SQLiteParameter In cmd.Parameters
            If Not (param.Direction = ParameterDirection.Output) AndAlso Not (param.Direction = ParameterDirection.ReturnValue) Then
                param.Value = parameterValues(i)
                i += 1
            End If
        Next
    End Sub
    Public Function ExecuteNonQuery(ByVal cmd As String, ByVal cmdType As CommandType, Optional ByVal parameters() As System.Data.SQLite.SQLiteParameter = Nothing) As Integer
        Dim connection As System.Data.SQLite.SQLiteConnection = Nothing
        Dim transaction As System.Data.SQLite.SQLiteTransaction = Nothing
        Dim command As System.Data.SQLite.SQLiteCommand = Nothing
        Dim res As Integer = -1
        Try
            connection = New System.Data.SQLite.SQLiteConnection(ConnectionString)
            command = New System.Data.SQLite.SQLiteCommand(cmd, connection)
            command.CommandType = cmdType
            Database.AssignParameters(command, parameters)
            connection.Open()
            transaction = connection.BeginTransaction()
            command.Transaction = transaction
            res = command.ExecuteNonQuery()
            transaction.Commit()
        Catch ex As Exception
            If Not (transaction Is Nothing) Then
                transaction.Rollback()
            End If
            Throw New DatabaseException(ex.Message, ex.InnerException)
        Finally
            If Not (connection Is Nothing) AndAlso (connection.State = ConnectionState.Open) Then connection.Close()
            If Not (command Is Nothing) Then command.Dispose()
            If Not (transaction Is Nothing) Then transaction.Dispose()
        End Try
        Return res
    End Function
    Public Function ExecuteNonQuery(ByVal spname As String, ByRef returnValue As Integer, ByVal ParamArray parameterValues() As Object) As Integer
        Dim connection As System.Data.SQLite.SQLiteConnection = Nothing
        Dim transaction As System.Data.SQLite.SQLiteTransaction = Nothing
        Dim command As System.Data.SQLite.SQLiteCommand = Nothing
        Dim res As Integer = -1
        Try
            connection = New System.Data.SQLite.SQLiteConnection(ConnectionString)
            command = New System.Data.SQLite.SQLiteCommand(spname, connection)
            command.CommandType = CommandType.StoredProcedure
            connection.Open()
            'SqlCommandBuilder.DeriveParameters(command)
            Database.AssignParameters(command, parameterValues)
            transaction = connection.BeginTransaction()
            command.Transaction = transaction
            res = command.ExecuteNonQuery()
            returnValue = command.Parameters(0).Value
            transaction.Commit()
        Catch ex As Exception
            If Not (transaction Is Nothing) Then
                transaction.Rollback()
            End If
            Throw New DatabaseException(ex.Message, ex.InnerException)
        Finally
            If Not (connection Is Nothing) AndAlso (connection.State = ConnectionState.Open) Then connection.Close()
            If Not (command Is Nothing) Then command.Dispose()
            If Not (transaction Is Nothing) Then transaction.Dispose()
        End Try
        Return res
    End Function
    Public Function ExecuteScalar(ByVal cmd As String, ByVal cmdType As CommandType, Optional ByVal parameters() As System.Data.SQLite.SQLiteParameter = Nothing) As Object
        Dim connection As System.Data.SQLite.SQLiteConnection = Nothing
        Dim transaction As System.Data.SQLite.SQLiteTransaction = Nothing
        Dim command As System.Data.SQLite.SQLiteCommand = Nothing
        Dim res As Object = Nothing
        Try
            connection = New System.Data.SQLite.SQLiteConnection(ConnectionString)
            command = New System.Data.SQLite.SQLiteCommand(cmd, connection)
            command.CommandType = cmdType
            Database.AssignParameters(command, parameters)
            connection.Open()
            transaction = connection.BeginTransaction()
            command.Transaction = transaction
            res = command.ExecuteScalar()
            transaction.Commit()
        Catch ex As Exception
            If Not (transaction Is Nothing) Then
                transaction.Rollback()
            End If
            Throw New DatabaseException(ex.Message, ex.InnerException)
        Finally
            If Not (connection Is Nothing) AndAlso (connection.State = ConnectionState.Open) Then connection.Close()
            If Not (command Is Nothing) Then command.Dispose()
            If Not (transaction Is Nothing) Then transaction.Dispose()
        End Try
        Return res
    End Function
    Public Function ExecuteScalar(ByVal spname As String, ByRef returnValue As Integer, ByVal ParamArray parameterValues() As Object) As Object
        Dim connection As System.Data.SQLite.SQLiteConnection = Nothing
        Dim transaction As System.Data.SQLite.SQLiteTransaction = Nothing
        Dim command As System.Data.SQLite.SQLiteCommand = Nothing
        Dim res As Object = Nothing
        Try
            connection = New System.Data.SQLite.SQLiteConnection(ConnectionString)
            command = New System.Data.SQLite.SQLiteCommand(spname, connection)
            command.CommandType = CommandType.StoredProcedure
            connection.Open()
            'SqlCommandBuilder.DeriveParameters(command)
            Database.AssignParameters(command, parameterValues)
            transaction = connection.BeginTransaction()
            command.Transaction = transaction
            res = command.ExecuteScalar()
            returnValue = command.Parameters(0).Value
            transaction.Commit()
        Catch ex As Exception
            If Not (transaction Is Nothing) Then
                transaction.Rollback()
            End If
            Throw New DatabaseException(ex.Message, ex.InnerException)
        Finally
            If Not (connection Is Nothing) AndAlso (connection.State = ConnectionState.Open) Then connection.Close()
            If Not (command Is Nothing) Then command.Dispose()
            If Not (transaction Is Nothing) Then transaction.Dispose()
        End Try
        Return res
    End Function
    Public Function ExecuteReader(ByVal cmd As String, ByVal cmdType As CommandType,
                              Optional ByVal parameters() As System.Data.SQLite.SQLiteParameter = Nothing) As IDataReader
        Dim connection As System.Data.SQLite.SQLiteConnection = Nothing
        Dim command As System.Data.SQLite.SQLiteCommand = Nothing
        Dim res As System.Data.SQLite.SQLiteDataReader = Nothing
        Try
            connection = New System.Data.SQLite.SQLiteConnection(ConnectionString)
            command = New System.Data.SQLite.SQLiteCommand(cmd, connection)
            command.CommandType = cmdType
            Database.AssignParameters(command, parameters)
            connection.Open()
            res = command.ExecuteReader(CommandBehavior.CloseConnection)
        Catch ex As Exception
            Throw New DatabaseException(ex.Message, ex.InnerException)
        End Try
        Return CType(res, IDataReader)
    End Function
    Public Function ExecuteReader(ByVal spname As String, ByRef returnValue As Integer, ByVal ParamArray parameterValues() As Object) As IDataReader
        Dim connection As System.Data.SQLite.SQLiteConnection = Nothing
        Dim command As System.Data.SQLite.SQLiteCommand = Nothing
        Dim res As System.Data.SQLite.SQLiteDataReader = Nothing
        Try
            connection = New System.Data.SQLite.SQLiteConnection(ConnectionString)
            command = New System.Data.SQLite.SQLiteCommand(spname, connection)
            command.CommandType = CommandType.StoredProcedure
            connection.Open()
            'SqlCommandBuilder.DeriveParameters(command)
            Database.AssignParameters(command, parameterValues)
            res = command.ExecuteReader(CommandBehavior.CloseConnection)
            returnValue = command.Parameters(0).Value
        Catch ex As Exception
            Throw New DatabaseException(ex.Message, ex.InnerException)
        End Try
        Return CType(res, IDataReader)
    End Function
    Public Function FillDataset(ByVal cmd As String, ByVal cmdType As CommandType,
                            Optional ByVal parameters() As System.Data.SQLite.SQLiteParameter = Nothing) As DataSet
        Dim connection As System.Data.SQLite.SQLiteConnection = Nothing
        Dim command As System.Data.SQLite.SQLiteCommand = Nothing
        Dim sqlda As System.Data.SQLite.SQLiteDataAdapter = Nothing
        Dim res As New DataSet
        Try
            connection = New System.Data.SQLite.SQLiteConnection(ConnectionString)
            command = New System.Data.SQLite.SQLiteCommand(cmd, connection)
            command.CommandType = cmdType
            AssignParameters(command, parameters)
            sqlda = New System.Data.SQLite.SQLiteDataAdapter(command)
            sqlda.Fill(res)
        Catch ex As Exception
            Throw New DatabaseException(ex.Message, ex.InnerException)
        Finally
            If Not (connection Is Nothing) Then connection.Dispose()
            If Not (command Is Nothing) Then command.Dispose()
            If Not (sqlda Is Nothing) Then sqlda.Dispose()
        End Try
        Return res
    End Function
    Public Function ExecuteDataset(ByVal insertCmd As System.Data.SQLite.SQLiteCommand,
                               ByVal updateCmd As System.Data.SQLite.SQLiteCommand,
                               ByVal deleteCmd As System.Data.SQLite.SQLiteCommand,
                               ByVal ds As DataSet, ByVal srcTable As String) As Integer
        Dim connection As System.Data.SQLite.SQLiteConnection = Nothing
        Dim sqlda As System.Data.SQLite.SQLiteDataAdapter = Nothing
        Dim res As Integer = 0
        Try
            connection = New System.Data.SQLite.SQLiteConnection(ConnectionString)
            sqlda = New System.Data.SQLite.SQLiteDataAdapter
            If Not (insertCmd Is Nothing) Then insertCmd.Connection = connection : sqlda.InsertCommand = insertCmd
            If Not (updateCmd Is Nothing) Then updateCmd.Connection = connection : sqlda.UpdateCommand = updateCmd
            If Not (deleteCmd Is Nothing) Then deleteCmd.Connection = connection : sqlda.DeleteCommand = deleteCmd
            res = sqlda.Update(ds, srcTable)
        Catch ex As Exception
            Throw New DatabaseException(ex.Message, ex.InnerException)
        Finally
            If Not (connection Is Nothing) Then connection.Dispose()
            If Not (insertCmd Is Nothing) Then insertCmd.Dispose()
            If Not (updateCmd Is Nothing) Then updateCmd.Dispose()
            If Not (deleteCmd Is Nothing) Then deleteCmd.Dispose()
            If Not (sqlda Is Nothing) Then sqlda.Dispose()
        End Try
        Return res
    End Function
End Class