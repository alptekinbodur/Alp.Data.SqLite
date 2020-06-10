# Alp.Data.SqLite
[![License](http://img.shields.io/badge/license-MIT-green.svg)](https://github.com/alptekinbodur/Alp.Data.SqLite/blob/master/LICENSE.txt) [![Nuget](https://img.shields.io/nuget/v/Alp.Data.SqLite)](https://www.nuget.org/packages/Alp.Data.SqLite/) [![Nuget](https://img.shields.io/nuget/dt/Alp.Data.SqLite)](https://www.nuget.org/packages/Alp.Data.SqLite/)

SqLite connection and data works

``Alp.Data.SqLite`` basic usage parametets  
``ExecuteDataset()``
``ExecuteNonQuery()``
``ExecuteReader()``
``ExecuteScalar()``
``FillDataset()``

### Usage Sample
----------------------------------------
```c#
using System;
using System.Data;
public class Program
{
	public static void Main()
	{
		// single line mode
    private readonly Alp.Data.SqLite.Database bag = new Alp.Data.SqLite.Database("Data Source=test.db");
    
    //create table sample
    string strSql = @"CREATE TABLE company(id INTEGER PRIMARY KEY AUTOINCREMENT, status INT, name NVARCHAR(255))";
    bag.ExecuteNonQuery(strSql, CommandType.Text);
    Console.WriteLine("table created");
    
    // insert
    strSql = "INSERT INTO company (status,name) VALUES (0,'Company 1');";
    bag.ExecuteNonQuery(strSql, CommandType.Text);
    Console.WriteLine("company added");
    
    // insert with parameters
    strSql = "INSERT INTO company (status,name) VALUES (@status,@name);";
    strSql += "SELECT last_insert_rowid();";
    var par = new SQLite.SQLiteParameter[1];
    par[0] = new SQLite.SQLiteParameter("@status", DbType.Int32);
    par[0].Value = 0;
    par[1] = new SQLite.SQLiteParameter("@name", DbType.String, 255);
    par[1].Value = "Company 2";
    int iNewID = bag.ExecuteScalar(strSql, CommandType.Text, par);
    Console.WriteLine("New company added. id:" + iNewID);
    
    //update or update with parameters
    strSql = "UPDATE company SET status=@status, name=@name WHERE id=@id;";
    var par = new SQLite.SQLiteParameter[2];
    par(0) = new SQLite.SQLiteParameter("@status", DbType.Int32);
    par(0).Value = 1;
    par(1) = new SQLite.SQLiteParameter("@name", DbType.String, 255);
    par(1).Value = "Company 2 - Updated";
    par(2) = new SQLite.SQLiteParameter("@id", DbType.Int32);
    par(2).Value = 2;
    bag.ExecuteNonQuery(strSql, CommandType.Text, par);
    Console.WriteLine("company updated");
    
    // read data
    strSql = "SELECT id,name FROM company;"; // tables0
    //strSql +="SELECT * FROM AnotherTableOrQuery;" tables1
    Using;
DataSet dt = bag.FillDataset(strSql, CommandType.Text);
foreach (DataRow row in dt.Tables[0].Rows) {
    Console.WriteLine(string.Format("{0}:{1}", row("id").ToString, row("name").ToString));
}
EndUsing;

    // delete
    strSql = "DELETE FROM company WHERE id=1;";
    bag.ExecuteNonQuery(strSql, CommandType.Text);
    Console.WriteLine("1 record deleted");
    
	}
}
```
