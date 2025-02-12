namespace TestTask.Domain.Interfaces
{
    public interface IMessageDBRepositoryBuilder<T> where T : IMessageDBRepository
    {
        Task<T> Build();
    }
}
