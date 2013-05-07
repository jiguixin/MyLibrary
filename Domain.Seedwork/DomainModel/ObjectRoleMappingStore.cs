using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Seedwork.DomainModel
{
    public class ActorRoleMappingItem
    {
        public Type ActorType { get; set; }
        public Type RoleTypeDefinition { get; set; }
    }
    public class ObjectRoleMappingStore
    {
        #region Private Variables

        private static ObjectRoleMappingStore current = new ObjectRoleMappingStore();
        private Dictionary<ActorRoleMappingItem, Type> mappings = new Dictionary<ActorRoleMappingItem, Type>();

        #endregion

        #region Public Properties

        public static ObjectRoleMappingStore Current
        {
            get { return current; }
        }

        #endregion

        #region Public Methods

        public void ResolveObjectRoleTypeMappings(Assembly assembly)
        {
            var allRoleImplementationTypes = assembly.GetTypes().Where(type => type.IsClass && type.GetInterfaces().Any(i => IsRoleDefinition(i)));
            foreach (var roleImplementationType in allRoleImplementationTypes)
            {
                var actorType = GetActorType(roleImplementationType);
                if (actorType == null)
                {
                    actorType = roleImplementationType;
                }
                foreach (var roleTypeDefinition in roleImplementationType.GetInterfaces().Where(i => IsRoleDefinition(i)))
                {
                    if (mappings.Keys.ToList().Exists(item => item.ActorType == actorType && item.RoleTypeDefinition == roleTypeDefinition))
                    {
                        throw new Exception(string.Format("Actor of type '{0}' cannot act with role type '{1}' twice.", actorType.FullName, roleTypeDefinition.FullName));
                    }
                    mappings[new ActorRoleMappingItem { ActorType = actorType, RoleTypeDefinition = roleTypeDefinition }] = roleImplementationType;
                }
            }
        }
        public Type GetActorRoleType(Type actorType, Type roleTypeDefinition)
        {
            return mappings.Where(item => item.Key.ActorType == actorType && item.Key.RoleTypeDefinition == roleTypeDefinition).Select(item => item.Value).SingleOrDefault();
        }

        #endregion

        #region Private Methods

        private bool IsRoleDefinition(Type type)
        {
            if (type.IsInterface)
            {
                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == typeof(IRole<,>) || type.GetGenericTypeDefinition() == typeof(IRole<>))
                    {
                        return false;
                    }
                    else if (type.GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IRole<>)))
                    {
                        return true;
                    }
                }
                else if (type.GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IRole<>)))
                {
                    return true;
                }
            }
            return false;
        }
        private Type GetActorType(Type roleType)
        {
            Type baseType = roleType.BaseType;

            while (baseType != typeof(object) && !(baseType.IsGenericType && (baseType.GetGenericTypeDefinition() == typeof(Role<,>) || baseType.GetGenericTypeDefinition() == typeof(Role<,,>))))
            {
                baseType = baseType.BaseType;
            }

            if (baseType.IsGenericType && (baseType.GetGenericTypeDefinition() == typeof(Role<,>) || baseType.GetGenericTypeDefinition() == typeof(Role<,,>)))
            {
                return baseType.GetGenericArguments()[0];
            }

            return null;
        }

        #endregion
    }
}
