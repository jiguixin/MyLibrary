namespace Domain.Seedwork.DomainModel
{
    public abstract class Role<TActor, TRoleId> : Object<TRoleId>, IRole<TRoleId>
        where TActor : class, IObject<TRoleId>
    {
        public Role(TActor actor)
            : base(actor.Id, null)
        {
            this.Actor = actor;
        }

        protected TActor Actor { get; private set; }
    }
    public abstract class Role<TActor, TRoleId, TRoleState> : Object<TRoleId>, IRole<TRoleId, TRoleState>
        where TActor : class, IObject<TRoleId>
        where TRoleState : class, IStateObject
    {
        public Role(TActor actor, TRoleState state)
            : base(actor.Id, state)
        {
            this.Actor = actor;
        }

        protected TActor Actor { get; private set; }
    }
}
