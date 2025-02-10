using TestTask.Domain.Interfaces;

namespace TestTask.Repository.PostgreSQL
{
    public class MessagePostgreSQLBuilder : IMessageDBRepositoryBuilder<IMessageDBRepository>
    {
        public async Task<IMessageDBRepository> Build()
        {
            var postgreRepository = new MessagePostgreSQL();
            postgreRepository.InitConnection();
            await postgreRepository.InitDB();

            return postgreRepository;
        }
    }
}
