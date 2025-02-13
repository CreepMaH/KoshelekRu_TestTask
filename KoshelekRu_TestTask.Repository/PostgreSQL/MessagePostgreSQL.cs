using Npgsql;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Repository.PostgreSQL
{
    internal class MessagePostgreSQL(IAppSettings appSettings) : IMessageDBRepository
    {
        private readonly IAppSettings _appSettings = appSettings;

        private string? _connectionString;
        private string? _dbName;
        private string? _messagesTableName;

        public void LoadAppSettings()
        {
            var settings = _appSettings.GetAppSettings();

            _connectionString = settings.DBConfig?.ConnectionString
                ?? throw new ArgumentNullException(nameof(_connectionString));
            _dbName = settings.DBConfig?.DBName
                ?? throw new ArgumentNullException(nameof(_dbName));
            _messagesTableName = settings.DBConfig.MessagesTableName
                ?? throw new ArgumentNullException(nameof(_messagesTableName));
        }

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

        public async Task<IEnumerable<Message>> GetByTime(TimeSpan timeInterval)
        {
            using NpgsqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            DateTime minTime = DateTime.UtcNow.Subtract(timeInterval);
            string commandText = $@"
                SELECT * FROM {_messagesTableName}
                WHERE TimeStamp > '{minTime}'";
            using NpgsqlCommand command = CreateSqlCommand(commandText, conn);
            using var sqlReader = await command.ExecuteReaderAsync();

            List<Message> messages = [];
            while (await sqlReader.ReadAsync())
            {
                var msg = new Message
                {
                    Id = (ulong)sqlReader.GetInt64(0),
                    IndexNumber = (ulong)sqlReader.GetInt64(1),
                    Text = sqlReader.GetString(2),
                    TimeStamp = sqlReader.GetDateTime(3)
                };
                messages.Add(msg);
            }

            return messages;
        }

        public async Task<OperationResult> Write(Message message)
        {
            using NpgsqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string commandText = @$"INSERT INTO {_messagesTableName}(IndexNumber, Text, TimeStamp)" +
                @$"VALUES ({message.IndexNumber}, '{message.Text}', '{message.TimeStamp}')"; //TODO: Переписать на параметры
            using NpgsqlCommand command = CreateSqlCommand(commandText, conn);
            using var sqlDataReader = await command.ExecuteReaderAsync();

            bool isSuccess = sqlDataReader.RecordsAffected > 0;

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

        private async Task<bool> CheckIfDbExists()
        {
            using NpgsqlConnection conn = new("Host=postgres:5432;Username=koshelek;Password=koshelek.Ru@2025;Database=postgres");
            await conn.OpenAsync();

            string commandText = @$"
                SELECT EXISTS(
                    SELECT 1 
                    FROM pg_database 
                    WHERE datname = '{_dbName}'
                );";    //TODO: Переписать на параметры
            using NpgsqlCommand command = CreateSqlCommand(commandText, conn);
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

        private async Task<bool> CheckIfMessagesTableExists()
        {
            try
            {
                using NpgsqlConnection conn = new(_connectionString);
                await conn.OpenAsync();

                string commandText = @$"
                    SELECT 1 
                    FROM {_messagesTableName};";    //TODO: Переписать на параметры

                using NpgsqlCommand command = CreateSqlCommand(commandText, conn);
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task CreateDb()
        {
            using NpgsqlConnection conn = new("Host=postgres:5432;Username=koshelek;Password=koshelek.Ru@2025;Database=postgres");
            await conn.OpenAsync();

            string commandText = @$"CREATE DATABASE {_dbName};";  //TODO: Переписать на параметры

            using NpgsqlCommand command = CreateSqlCommand(commandText, conn);
            _ = await command.ExecuteNonQueryAsync();
        }

        private async Task CreateMessagesTable()
        {
            using NpgsqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string commandText = $@"
                CREATE TABLE {_messagesTableName}(
                    Id BIGSERIAL PRIMARY KEY,
                    IndexNumber BIGINT, 
                    Text CHARACTER VARYING(128), 
                    TimeStamp TIMESTAMP)";  //TODO: Переписать на параметры

            using NpgsqlCommand command = CreateSqlCommand(commandText, conn);
            _ = await command.ExecuteNonQueryAsync();
        }

        private NpgsqlCommand CreateSqlCommand(string commandText, NpgsqlConnection connection)
        {
            NpgsqlCommand command = new()
            {
                Connection = connection,
                CommandType = System.Data.CommandType.Text,
                CommandText = commandText
            };
            return command;
        }
    }
}
