using System.Data;

using DotNetCore.CAP;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace CapTestApp;

public record EventPublisher(ICapPublisher Publisher)
{
    public async Task PublishAsync(CancellationTokenSource cts)
    {
        while (!cts.IsCancellationRequested)
        {
            await using var connection = new SqlConnection(Constants.ConnectionString);
            //using var transaction = connection.BeginTransaction(this.Publisher, autoCommit: false);
            await connection.OpenAsync();
            await using var transaction = connection.BeginTransaction();

            var capTransaction = ActivatorUtilities.CreateInstance<SqlServerCapTransaction>(this.Publisher.ServiceProvider);
            capTransaction.DbTransaction = transaction;
            this.Publisher.Transaction.Value = capTransaction;

            var command = ((IDbConnection)connection).CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandText = "select getdate()";

            //using (var reader = command.ExecuteReader())
            //{
            //    reader.Read();
            //}
            await using (var reader = await ((SqlCommand)command).ExecuteReaderAsync())
            {
                await reader.ReadAsync();
            }

            Console.WriteLine($"Publisher has transaction: {this.Publisher.Transaction?.Value != null}");

            await this.Publisher.PublishAsync("sample.console.showtime", DateTime.Now, cancellationToken: cts.Token);

            transaction.Commit();

            await Task.Delay(3000, cts.Token);
        }
    }
}
