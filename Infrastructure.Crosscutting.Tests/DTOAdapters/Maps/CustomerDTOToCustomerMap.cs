using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Infrastructure.Crosscutting.Adapters;
using Infrastructure.Crosscutting.Tests.Dtos;
using Infrastructure.Crosscutting.Tests.Model;

namespace Infrastructure.Crosscutting.Tests.DTOAdapters.Maps
{
    public class CustomerDTOToCustomerMap
        : TypeMapConfigurationBase<CustomerDto,Customer>
    { 
        #region Overrides of TypeMapConfigurationBase<CustomerDto,Customer>

        /// <summary>
        /// Excuted action before map source to target.
        /// <remarks>
        /// If you use a framework for mappings you can use this method
        /// for preparing or setup the map.
        /// </remarks>
        /// </summary>
        /// <param name="source">The source to adapt</param>
        protected override void BeforeMap(ref CustomerDto source)
        {
            Mapper.CreateMap<OrderDto, Order>();

            var exp = Mapper.CreateMap<CustomerDto, Customer>(); 
            
            exp.ForMember(cus => cus.CusAddress/*()*/, (map) => map.MapFrom(dto=>new Address(){AddressLine1 = dto.CusAddressAddressLine1,AddressLine2 = dto.CusAddressAddressLine2,City = dto.CusAddressCity,ZipCode = dto.CusAddressZipCode}));
            exp.ForMember(cus => cus.Picture, (map) => map.MapFrom(dto => new Picture() {ImgPath = dto.FilePath}));

            exp.ForMember(cus => cus.OrderList, (map) => map.MapFrom(dto => dto.OrderListDto));

            //该方法主要用来检查还有那些规则没有写完。
            Mapper.AssertConfigurationIsValid();
             
        }

        /// <summary>
        /// Executed action after map.
        /// <remarks>
        /// You can use this method for set more sources into adapted object
        /// </remarks>
        /// </summary>
        /// <param name="target">The item adapted </param>
        /// <param name="nestedData">Nested data to use in post filter</param>
        protected override void AfterMap(ref Customer target, params object[] moreSources)
        {

        }

        /// <summary>
        /// Map a source to a target
        /// </summary>
        /// <remarks>
        /// If you use a framework, use this method for setup or resolve mapping.
        /// <example>Automapper.Map{TSource,KTarget}</example>
        /// </remarks>
        /// <param name="source">The source to map</param>
        /// <returns>A new instance of <typeparamref name="TTarget"/></returns>
        protected override Customer Map(CustomerDto source)
        {
            return Mapper.Map<CustomerDto, Customer>(source);
        }

        #endregion
    }
}
