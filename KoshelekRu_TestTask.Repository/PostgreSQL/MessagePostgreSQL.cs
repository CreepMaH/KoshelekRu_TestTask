using Npgsql;
using System.Text.Json;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Repository.PostgreSQL
{
    internal class MessagePostgreSQL : IMessageDBRepository
    {
        private const string CONFIG_FILE_NAME = "dbConfig.json";

        private NpgsqlConnection _connection;
        private string _dbName;
        private string _messagesTableName;

        public void InitConnection()
        {
            var configs = GetConfigs();

            _dbName = configs?["DbName"]
                ?? throw new ArgumentNullException(nameof(_dbName));
            _messagesTableName = configs?["MessagesTableName"]
                ?? throw new ArgumentNullException(nameof(_messagesTableName));

            string connectionString = configs?["ConnectionString"]
                ?? throw new ArgumentNullException(nameof(connectionString));
            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task InitDB()
        {
            if (!await CheckIfMessagesTableExists())
            {
                await CreateDbWithMessagesTable();
            }
        }

        public Task<IEnumerable<Message>> GetByTime(TimeSpan timeInterval)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> Write(Message message)
        {
            string commandText = $"INSERT INTO {_messagesTableName}(IndexNumber, Text, TimeStamp)" +
                $"VALUES ({message.IndexNumber}, {message.Text}, {message.TimeStamp})"; //TODO: Переписать на параметры

            await _connection.OpenAsync();
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
            string json = File.ReadAllText(CONFIG_FILE_NAME);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        private async Task<bool> CheckIfMessagesTableExists()
        {
            string commandText = @$"
                SELECT EXISTS (
                    SELECT 1
                    FROM information_schema.tables 
                    WHERE table_schema = 'public' 
                    AND table_name = {_messagesTableName}
                );";    //TODO: Переписать на параметры

            await _connection.OpenAsync();
            using NpgsqlCommand command = CreateSqlCommand(commandText);
            bool exists = (bool)(await command.ExecuteScalarAsync() ?? false);
            await _connection.CloseAsync();

            return exists;
        }

        private async Task CreateDbWithMessagesTable()
        {
            string commandText = @$"CREATE DATABASE {_dbName};
                CREATE TABLE {_messagesTableName}(
                    Id BIGSERIAL PRIMARY KEY,
                    IndexNumber BIGINT, 
                    Text CHARACTER VARYING(128), 
                    TimeStamp TIMESTAMP)";  //TODO: Переписать на параметры

            await _connection.OpenAsync();
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
