﻿
//using System;
//using System.Web;
//using System.ServiceModel;
//using System.Runtime.Remoting.Messaging;

//using Microsoft.Practices.Unity;

//using Infrastructure.CrossCutting.IoC.Resources;
 
//namespace Infrastructure.CrossCutting.IoC.Unity.LifetimeManagers
//{
//    /// <summary>
//    /// This is a custom lifetime that preserve  instance on the same
//    /// execution environment. For example, in  a WCF request or ASP.NET request, diferent
//    /// call to resolve method return the same instance
//    /// </summary>
//    sealed class PerExecutionContextLifetimeManager
//        : LifetimeManager
//    {
//        #region Nested

//        /// <summary>
//        /// Custom extension for OperationContext scope
//        /// </summary>
//        class ContainerExtension : IExtension<OperationContext>
//        {
//            #region Members

//            public object Value { get; set; }

//            #endregion

//            #region IExtension<OperationContext> Members

//            public void Attach(OperationContext owner)
//            {

//            }

//            public void Detach(OperationContext owner)
//            {

//            }

//            #endregion
//        }
//        #endregion

//        #region Members

//        Guid _key;

//        #endregion

//        #region Constructor

//        /// <summary>
//        /// Default constructor
//        /// </summary>
//        public PerExecutionContextLifetimeManager() : this(Guid.NewGuid()) { }

//        /// <summary>
//        ///  Constructor
//        /// </summary>
//        /// <param name="key">A key for this lifetimemanager resolver</param>
//        PerExecutionContextLifetimeManager(Guid key)
//        {
//            if (key == Guid.Empty)
//                throw new ArgumentException(Messages.exception_PerExecutionContextLifetimeManagerKeyCannotBeNull);

//            _key = key;
//        }
//        #endregion

//        #region ILifetimeManager Members

//        /// <summary>
//        /// <see cref="M:Microsoft.Practices.Unity.LifetimeManager.GetValue"/>
//        /// </summary>
//        /// <returns><see cref="M:Microsoft.Practices.Unity.LifetimeManager.GetValue"/></returns>
//        public override object GetValue()
//        {
//            object result = null;

//            //Get object depending on  execution environment ( WCF without HttpContext,HttpContext or CallContext)

//            if (OperationContext.Current != null)
//            {
//                //WCF without HttpContext environment
//                ContainerExtension containerExtension = OperationContext.Current.Extensions.Find<ContainerExtension>();
//                if (containerExtension != null)
//                {
//                    result = containerExtension.Value;
//                }
//            }
//            else if (HttpContext.Current != null)
//            {
//                //HttpContext avaiable ( ASP.NET ..)
//                if (HttpContext.Current.Items[_key.ToString()] != null)
//                    result = HttpContext.Current.Items[_key.ToString()];
//            }
//            else
//            {
//                //Not in WCF or ASP.NET Environment, UnitTesting, WinForms, WPF etc.
//                result = CallContext.GetData(_key.ToString());
//            }


//            return result;
//        }
//        /// <summary>
//        /// <see cref="M:Microsoft.Practices.Unity.LifetimeManager.RemoveValue"/>
//        /// </summary>
//        public override void RemoveValue()
//        {
//            if (OperationContext.Current != null)
//            {
//                //WCF without HttpContext environment
//                ContainerExtension containerExtension = OperationContext.Current.Extensions.Find<ContainerExtension>();
//                if (containerExtension != null)
//                    OperationContext.Current.Extensions.Remove(containerExtension);

//            }
//            else if (HttpContext.Current != null)
//            {
//                //HttpContext avaiable ( ASP.NET ..)
//                if (HttpContext.Current.Items[_key.ToString()] != null)
//                    HttpContext.Current.Items[_key.ToString()] = null;
//            }
//            else
//            {
//                //Not in WCF or ASP.NET Environment, UnitTesting, WinForms, WPF etc.
//                CallContext.FreeNamedDataSlot(_key.ToString());
//            }
//        }
//        /// <summary>
//        /// <see cref="M:Microsoft.Practices.Unity.LifetimeManager.SetValue"/>
//        /// </summary>
//        /// <param name="newValue"><see cref="M:Microsoft.Practices.Unity.LifetimeManager.SetValue"/></param>
//        public override void SetValue(object newValue)
//        {

//            if (OperationContext.Current != null)
//            {
//                //WCF without HttpContext environment
//                ContainerExtension containerExtension = OperationContext.Current.Extensions.Find<ContainerExtension>();
//                if (containerExtension == null)
//                {
//                    containerExtension = new ContainerExtension()
//                    {
//                        Value = newValue
//                    };

//                    OperationContext.Current.Extensions.Add(containerExtension);
//                }
//            }
//            else if (HttpContext.Current != null)
//            {
//                //HttpContext avaiable ( ASP.NET ..)
//                if (HttpContext.Current.Items[_key.ToString()] == null)
//                    HttpContext.Current.Items[_key.ToString()] = newValue;
//            }
//            else
//            {
//                //Not in WCF or ASP.NET Environment, UnitTesting, WinForms, WPF etc.
//                CallContext.SetData(_key.ToString(), newValue);
//            }
//        }

//        #endregion
//    }
//}
