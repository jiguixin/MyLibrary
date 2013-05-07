using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Infrastructure.Data.Ef.Test.UnitOfWork;

namespace Infrastructure.Data.Ef.Test.Initializers
{
    public class AssemblyTestsInitialize
    {
        /// <summary>
        /// In this beta, the unit of work initializer is
        /// </summary>
        /// <param name="context"></param> 
        public static void RebuildUnitOfWork()
        {
            //Set default initializer for MainBCUnitOfWork 
            Database.SetInitializer<MainBCUnitOfWork>(new MainBCUnitOfWorkInitializer()); 
        }
    }
}
