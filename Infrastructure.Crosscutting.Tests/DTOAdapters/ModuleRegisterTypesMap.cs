using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Adapters;
using Infrastructure.Crosscutting.Tests.DTOAdapters.Maps;
using Infrastructure.Crosscutting.Tests.Dtos;
using Infrastructure.Crosscutting.Tests.Model;

namespace Infrastructure.Crosscutting.Tests.DTOAdapters
{
    public class ModuleRegisterTypesMap : RegisterTypesMap
    {
        public ModuleRegisterTypesMap()
        {
            //Customers
            RegisterMap<Customer, CustomerDto>(new CustomerToCustomerDTOMap());
            RegisterMap<CustomerDto,Customer>(new CustomerDTOToCustomerMap());
            RegisterMap<CustomerDto,Address>(new CustomerDTOToAddress());
        }
    }
}
