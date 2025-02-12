using TestTask.Configuration.Services;
using TestTask.Domain.Interfaces;

namespace TestTask.Repository.PostgreSQL
{
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
