using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Adapters;
using Infrastructure.Crosscutting.Tests.Dtos;
using Infrastructure.Crosscutting.Tests.Model;
using AutoMapper;

namespace Infrastructure.Crosscutting.Tests.DTOAdapters.Maps
{
    public class CustomerDTOToAddress: TypeMapConfigurationBase<CustomerDto,Address>
    {
        #region Overrides of TypeMapConfigurationBase<CustomerDto,Address>

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
            var exp = Mapper.CreateMap<CustomerDto, Address>();
            exp.ForMember(ads => ads.AddressLine1, dto => dto.MapFrom(d => d.CusAddressAddressLine1));
            exp.ForMember(ads => ads.AddressLine2, dto => dto.MapFrom(d => d.CusAddressAddressLine2));
            exp.ForMember(ads => ads.City, dto => dto.MapFrom(d => d.CusAddressCity));

            //exp.ForMember(ads => ads.ZipCode, dto => dto.MapFrom(d => d.CusAddressZipCode)); 
            exp.ForMember(ads => ads.ZipCode, dto => dto.Ignore()); //如果对于不想某属性有值，我们可以通过Ignore来忽略他，这样在调用AssertConfigurationIsValid时也不会报错.
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
        protected override void AfterMap(ref Address target, params object[] moreSources)
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
        protected override Address Map(CustomerDto source)
        { 
            return Mapper.Map<CustomerDto, Address>(source);
        }

        #endregion
    }
}
