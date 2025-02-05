// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using Dapper;
using Dapperr;

var configration = new ConfigurationBuilder()
    .AddJsonFile("appSettings.json")
    .Build();

Console.WriteLine(configration.GetSection("constr").Value);

IDbConnection db = new SqlConnection(configration.GetSection("constr").Value);
//execute raw select 
var sql = "SELECT * FROM Wallets";
var wallets  = db.Query<Wallet>(sql);

foreach (var wallet in wallets)
    Console.WriteLine(wallet);

// execute insert statment
var wattetToInsert = new Wallet
{
Holder = "mostafa",
Balance  = 1000m
};
sql = "INSERT INTO Wallets (Holder, Balance VALUES(@Holder,@Balance))";

db.Execute(sql,new {
      Holder = wattetToInsert.Holder
    , Balance = wattetToInsert.Balance
});
// execute insert statment and return id

var wattetToInsert_2 = new Wallet
{
    Holder = "mostafa",
    Balance = 1000m
};
sql = "INSERT INTO Wallets (Holder, Balance )VALUES(@Holder,@Balance) SELECT CAST (SCOPE_IDENTITY()AS INT)";

wattetToInsert_2.Id = db.Query<int>(sql, new
{
    Holder = wattetToInsert.Holder
    ,
    Balance = wattetToInsert.Balance
}).Single();
//execute update statment
var wattetToUpdate = new Wallet
{
    Id = 1,
    Holder = "mostafaaa",
    Balance = 1000m
};
sql = "UPDATE  Wallets SET Holder = @Holder,Balance = @Balance WHERE Id = @Id )";

var parameter = new
{
    Id  = wattetToUpdate.Id
    ,
    Holder = wattetToUpdate.Holder
    ,
    Balance = wattetToUpdate.Balance

};
db.Execute(sql, parameter);

//execute delete statment

sql = "DELETE FROM   Wallets WHERE Id = @Id )";
db.Execute(sql, new { Id = 1 });

// execute multiple query

sql = "SELECT MIN(Balance) FROM Wallets;"
    +"SELECT MAX (Balance)FROM Wallets;";
var multi = db.QueryMultiple(sql);

Console.WriteLine(
    $"Min  = {multi.ReadSingle<decimal>()}"
  + $"Max  = {multi.ReadSingle<decimal>()}"
                  );

Console.WriteLine(
    $"Minn   = {multi.Read<decimal>().Single()}"
  + $"Maxx   = {multi.Read<decimal>().Single()}"
                  );

