using System;
using System.Linq;
using System.Reflection;

namespace Domain.Seedwork.DomainModel
{
    public interface IObject<TObjectId>
    {
        TObjectId Id { get; }
        /// <summary>
        /// 使之扮演某种角色
        /// </summary>
        /// <typeparam name="TRole"></typeparam>
        /// <returns></returns>
        TRole ActAs<TRole>() where TRole : class;
        TRole ActAs<TRole>(IStateObject roleState) where TRole : class;
    }

    public abstract class Object<TObjectId> : IObject<TObjectId>
    {
        public Object(TObjectId id, IStateObject state)
        {
            this.Id = id;
            if (state != null)
            {
                state.ValidateState();
                AutoUpdateStatusFromState(state);
            }
        }

        public TObjectId Id { get; private set; }

        public TRole ActAs<TRole>() where TRole : class
        {
            return ActAs<TRole>(null);
        }
        public TRole ActAs<TRole>(IStateObject roleState) where TRole : class
        {
            object role = null;
            var roleTypeDefinition = typeof(TRole);

            if (!roleTypeDefinition.IsInterface)
            {
                throw new NotSupportedException("Role type must be an interface.");
            }

            //If the actor already implement the role type definition, then return it directly.
            if (roleTypeDefinition.IsAssignableFrom(this.GetType()))
            {
                return this as TRole;
            }

            var roleType = ObjectRoleMappingStore.Current.GetActorRoleType(this.GetType(), roleTypeDefinition);
            if (roleType == null)
            {
                throw new NotSupportedException(string.Format("Object of type '{0}' cannot act role of type '{1}'", this.GetType().FullName, roleTypeDefinition.FullName));
            }

            //Try to get the role instance from data persistence.
            var roleInterfaceType = roleType.GetInterfaces().Where(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IRole<>)).FirstOrDefault();
            var roleIdType = roleInterfaceType.GetGenericArguments()[0];
            var getRoleEventType = typeof(GetObjectEvent<,>).MakeGenericType(roleType, roleIdType);
            IDomainEvent getRoleEvent = Activator.CreateInstance(getRoleEventType, this.Id) as IDomainEvent;
            EventPublisher.Publish(getRoleEvent);
            role = getRoleEvent.Results.FirstOrDefault(obj => roleType.IsAssignableFrom(obj.GetType())) as TRole;

            //If the role instance not found on the data persistence, then instantiate it.
            if (role == null)
            {
                var constructor = roleType.GetConstructors().Where(
                    c =>
                        c.GetParameters().Count() == 2
                        && c.GetParameters()[0].ParameterType == this.GetType()
                        && typeof(IStateObject).IsAssignableFrom(c.GetParameters()[1].ParameterType)
                    ).SingleOrDefault();

                if (constructor != null)
                {
                    role = constructor.Invoke(new object[] { this, roleState }) as TRole;
                    Repository.Add(role);
                }
                else
                {
                    constructor = roleType.GetConstructors().Where(
                        c =>
                            c.GetParameters().Count() == 1
                            && c.GetParameters()[0].ParameterType == this.GetType()
                        ).SingleOrDefault();
                    if (constructor != null)
                    {
                        role = constructor.Invoke(new object[] { this }) as TRole;
                        Repository.Add(role);
                    }
                    else
                    {
                        throw new NotSupportedException(string.Format("No available constructor found on role type '{0}'", roleType.FullName));
                    }
                }
            }

            return role as TRole;
        }

        #region Private Methods

        private void AutoUpdateStatusFromState(IStateObject state)
        {
            var autoFromStateProperties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(propertyInfo => propertyInfo.GetCustomAttributes(typeof(MannualAttribute), false).Count() == 0);
            foreach (var autoFromStateProperty in autoFromStateProperties)
            {
                autoFromStateProperty.SetValue(this, state.GetType().GetProperty(autoFromStateProperty.Name).GetValue(state, null), null);
            }
        }

        #endregion
    }
}
