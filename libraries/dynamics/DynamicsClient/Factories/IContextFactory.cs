namespace Libraries.Dynamics.DynamicsClient.Factories
{
    public interface IContextFactory<T>
    {
        T CreateContext();
    }
}