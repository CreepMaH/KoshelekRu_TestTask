using Npgsql;
using System.Text.Json;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Repository.PostgreSQL
{
    internal class MessagePostgreSQL : IMessageDBRepository
    {
        private readonly string _configFileName = "";//Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Repository", "dbConfig.json");

        private NpgsqlConnection? _connection;
        private string? _dbName = "koshelekdb";
        private string? _messagesTableName = "messages";

        public async Task InitDB()
        {
            if (!await CheckIfDbExists())
            {
                await CreateDb();
            }
            if (!await CheckIfMessagesTableExists())
            {
                await CreateMessagesTable();
            }
        }

        public void InitConnection()
        {
            _connection = new NpgsqlConnection("Host=postgres:5432;Username=koshelek;Password=koshelek.Ru@2025;Database=koshelekdb");
            return;

            var configs = GetConfigs();

            _dbName = configs?["DbName"]
                ?? throw new ArgumentNullException(nameof(_dbName));
            _messagesTableName = configs?["MessagesTableName"]
                ?? throw new ArgumentNullException(nameof(_messagesTableName));

            string connectionString = configs?["ConnectionString"]
                ?? throw new ArgumentNullException(nameof(connectionString));
            _connection = new NpgsqlConnection(connectionString);
        }

        public Task<IEnumerable<Message>> GetByTime(TimeSpan timeInterval)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> Write(Message message)
        {
            string commandText = @$"INSERT INTO {_messagesTableName}(IndexNumber, Text, TimeStamp)" +
                @$"VALUES ({message.IndexNumber}, '{message.Text}', {message.TimeStamp})"; //TODO: Переписать на параметры

            await _connection!.OpenAsync();
            using NpgsqlCommand command = CreateSqlCommand(commandText);
            using var sqlDataReader = await command.ExecuteReaderAsync();

            bool isSuccess = sqlDataReader.RecordsAffected > 0;
            await _connection.CloseAsync();

            return new OperationResult
            {
                IsSuccess = isSuccess,
                Message = $"The command has been executed with {sqlDataReader.RecordsAffected} rows affected"
            };
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        private Dictionary<string, string>? GetConfigs()
        {
            string json = File.ReadAllText(_configFileName);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        private async Task<bool> CheckIfDbExists()
        {
            _connection = new NpgsqlConnection("Host=postgres:5432;Username=koshelek;Password=koshelek.Ru@2025;Database=postgres");

            string commandText = @$"
                SELECT EXISTS(
                    SELECT 1 
                    FROM pg_database 
                    WHERE datname = '{_dbName}'
                );";    //TODO: Переписать на параметры

            try
            {
                await _connection!.OpenAsync();

                using NpgsqlCommand command = CreateSqlCommand(commandText);
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    bool dbExists = reader.GetBoolean(0);
                    return dbExists;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private async Task<bool> CheckIfMessagesTableExists()
        {
            _connection = new NpgsqlConnection("Host=postgres:5432;Username=koshelek;Password=koshelek.Ru@2025;Database=koshelekdb");

            string commandText = @$"
                SELECT 1 
                FROM {_messagesTableName};";    //TODO: Переписать на параметры

            try
            {
                await _connection!.OpenAsync();
                using NpgsqlCommand command = CreateSqlCommand(commandText);
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private async Task CreateDb()
        {
            _connection = new NpgsqlConnection("Host=postgres:5432;Username=koshelek;Password=koshelek.Ru@2025;Database=postgres");

            string commandText = @$"CREATE DATABASE {_dbName};";  //TODO: Переписать на параметры

            await _connection!.OpenAsync();
            using NpgsqlCommand command = CreateSqlCommand(commandText);
            _ = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        private async Task CreateMessagesTable()
        {
            _connection = new NpgsqlConnection("Host=postgres:5432;Username=koshelek;Password=koshelek.Ru@2025;Database=koshelekdb");

            string commandText = $@"
                CREATE TABLE {_messagesTableName}(
                    Id BIGSERIAL PRIMARY KEY,
                    IndexNumber BIGINT, 
                    Text CHARACTER VARYING(128), 
                    TimeStamp TIMESTAMP)";  //TODO: Переписать на параметры

            await _connection!.OpenAsync();
            using NpgsqlCommand command = CreateSqlCommand(commandText);
            _ = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        private NpgsqlCommand CreateSqlCommand(string commandText)
        {
            NpgsqlCommand command = new()
            {
                Connection = _connection,
                CommandType = System.Data.CommandType.Text,
                CommandText = commandText
            };
            return command;
        }
    }
}
