
namespace Totira.Bussiness.PropertiesService.Mappers
{
    using AutoMapper;
    using CrestApps.RetsSdk.Models;
    using Totira.Bussiness.PropertiesService.Domain;

    public class ResidentialProfile: Profile
    {
        public ResidentialProfile()
        {

            CreateMap<SearchResultRow, Residential>()
                .ForMember(y => y.FrontingOnNSEW, m => m.MapFrom(x => x.Get("comp_pts").NullOrValue()))
                .ForMember(y => y.LotDepth, m => m.MapFrom(x => x.Get("depth").NullOrValue()))
                .ForMember(y => y.Drive, m => m.MapFrom(x => x.Get("drive").NullOrValue()))
                .ForMember(y => y.UtilitiesHydro, m => m.MapFrom(x => x.Get("elec").NullOrValue()))
                .ForMember(y => y.FarmAgriculture, m => m.MapFrom(x => x.Get("farm_agri").NullOrValue()))
                .ForMember(y => y.LotFront, m => m.MapFrom(x => x.Get("front_ft").NullOrValue()))
                .ForMember(y => y.GarageSpaces, m => m.MapFrom(x => x.Get("gar_spaces").NullOrValue()))
                .ForMember(y => y.UtilitiesGas, m => m.MapFrom(x => x.Get("gas").NullOrValue()))
                .ForMember(y => y.PaymentFrequency, m => m.MapFrom(x => x.Get("pay_freq").NullOrValue()))
                .ForMember(y => y.Pool, m => m.MapFrom(x => x.Get("pool").NullOrValue()))
                .ForMember(y => y.UtilitiesCable, m => m.MapFrom(x => x.Get("util_cable").NullOrValue()))
                .ForMember(y => y.UtilitiesTelephone, m => m.MapFrom(x => x.Get("util_tel").NullOrValue()))
                .ForMember(y => y.Water, m => m.MapFrom(x => x.Get("water").NullOrValue()))
                .ForMember(y => y.Waterfront, m => m.MapFrom(x => x.Get("waterfront").NullOrValue()))
                .ForMember(y => y.WaterSupplyTypes, m => m.MapFrom(x => x.Get("wtr_suptyp").NullOrValue()))
                .ForMember(y => y.Acreage, m => m.MapFrom(x => x.Get("acres").NullOrValue()))
                .ForMember(y => y.LotIrregularities, m => m.MapFrom(x => x.Get("irreg").NullOrValue()))
                .ForMember(y => y.LeaseAgreement, m => m.MapFrom(x => x.Get("lease").NullOrValue()))
                .ForMember(y => y.LeaseTerm, m => m.MapFrom(x => x.Get("lease_term").NullOrValue()))
                .ForMember(y => y.LegalDescription, m => m.MapFrom(x => x.Get("legal_desc").NullOrValue()))
                .ForMember(y => y.LotFrontIncomplete, m => m.MapFrom(x => x.Get("lot_fr_inc").NullOrValue()))
                .ForMember(y => y.LotSizeCode, m => m.MapFrom(x => x.Get("lotsz_code").NullOrValue()))
                .ForMember(y => y.LeasedTerms, m => m.MapFrom(x => x.Get("lse_terms").NullOrValue()))
                .ForMember(y => y.Sewers, m => m.MapFrom(x => x.Get("sewer").NullOrValue()))
                .ForMember(y => y.ParcelOfTiedLand, m => m.MapFrom(x => x.Get("potl").NullOrValue()))
                .ForMember(y => y.Link, m => m.MapFrom(x => x.Get("link_yn").NullOrValue()))
                .ForMember(y => y.Link_Comment, m => m.MapFrom(x => x.Get("link_comment").NullOrValue()))
                .ForMember(y => y.OtherStructures, opt => opt.MapFrom(src => new List<string> { src.Get("oth_struc1_out").NullOrValue(),
                    src.Get("oth_struc2_out").NullOrValue()}));



        }
    }
}
