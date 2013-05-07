namespace Domain.Seedwork.DomainModel
{
    public interface IStateObject
    {
        void ValidateState();
    }
    public abstract class StateObject : IStateObject
    {
        public virtual void ValidateState()
        {
        }
    }

}
