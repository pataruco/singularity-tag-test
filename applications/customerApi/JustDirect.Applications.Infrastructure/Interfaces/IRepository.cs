namespace JustDirect.Applications.Infrastructure.Interfaces;

public interface IRepository<T>
{
    IList<T> Get();

    T? GetById(Guid id);

    void Update(T entity);
}