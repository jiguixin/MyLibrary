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
    public class CustomerToCustomerDTOMap
        : TypeMapConfigurationBase<Customer, CustomerDto>
    {
        #region Overrides of TypeMapConfigurationBase<Customer,CustomerDto>

        /// <summary>
        /// Excuted action before map source to target.
        /// <remarks>
        /// If you use a framework for mappings you can use this method
        /// for preparing or setup the map.
        /// </remarks>
        /// </summary>
        /// <param name="source">The source to adapt</param>
        protected override void BeforeMap(ref Customer source)
        {
            Mapper.CreateMap<Order, OrderDto>();

            var exp =  Mapper.CreateMap<Customer, CustomerDto>();

            exp.ForMember(dto => dto.FilePath, (map) => map.MapFrom(c => c.Picture.ImgPath));
            
            exp.ForMember(dto => dto.OrderListDto, (map) => map.MapFrom(m=>m.OrderList));
             
        }

        /// <summary>
        /// Executed action after map.
        /// <remarks>
        /// You can use this method for set more sources into adapted object
        /// </remarks>
        /// </summary>
        /// <param name="target">The item adapted </param>
        /// <param name="nestedData">Nested data to use in post filter</param>
        protected override void AfterMap(ref CustomerDto target, params object[] moreSources)
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
        protected override CustomerDto Map(Customer source)
        {
            return Mapper.Map<Customer, CustomerDto>(source);
        }

        #endregion
    }
}
