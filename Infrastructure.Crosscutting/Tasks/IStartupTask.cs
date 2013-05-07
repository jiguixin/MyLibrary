namespace Infrastructure.Crosscutting.Tasks 
{
    public interface IStartupTask 
    {
        void Execute();

        int Order { get; }
    }
}
