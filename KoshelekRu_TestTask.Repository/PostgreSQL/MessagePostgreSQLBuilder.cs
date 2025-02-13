using TestTask.Configuration.Services;
using TestTask.Domain.Interfaces;

namespace TestTask.Repository.PostgreSQL
{
    /// <summary>
    /// Provides methods to build an IMessageDBRepository instance.
    /// </summary>
    public class MessagePostgreSQLBuilder : IMessageDBRepositoryBuilder<IMessageDBRepository>
    {
        public async Task<IMessageDBRepository> Build()
        {
            var appSettings = new AppSettingsService();
            var postgreRepository = new MessagePostgreSQL(appSettings);

            postgreRepository.LoadAppSettings();
            await postgreRepository.InitDB();

            return postgreRepository;
        }
    }
}
