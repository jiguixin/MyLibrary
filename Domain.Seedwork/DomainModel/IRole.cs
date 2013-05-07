namespace Domain.Seedwork.DomainModel
{
    public interface IRole<TRoleId> : IObject<TRoleId>
    {
    }
    public interface IRole<TRoleId, TRoleState> : IRole<TRoleId>
        where TRoleState : class, IStateObject
    {
    }
}
