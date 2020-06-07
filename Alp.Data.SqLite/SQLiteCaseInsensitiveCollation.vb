Imports System.Data.SQLite
<SQLiteFunction(FuncType:=FunctionType.Collation, Name:="UTF8CI")>
Public Class SQLiteCaseInsensitiveCollation
    Inherits SQLiteFunction
    Private Shared ReadOnly _cultureInfo As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR")
    Public Overrides Function Compare(ByVal x As String, ByVal y As String) As Integer
        Return String.Compare(x, y, _cultureInfo, System.Globalization.CompareOptions.IgnoreCase)
    End Function
End Class