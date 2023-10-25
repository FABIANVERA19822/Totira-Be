
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
                .ForMember(y => y.FrontingOnNSEW, m => m.MapFrom(x => x.Get("comp_pts").EmptyOrValue()))
                .ForMember(y => y.LotDepth, m => m.MapFrom(x => x.Get("depth").NullOrValue()))
                .ForMember(y => y.Drive, m => m.MapFrom(x => x.Get("drive").EmptyOrValue()))
                .ForMember(y => y.UtilitiesHydro, m => m.MapFrom(x => x.Get("elec").EmptyOrValue()))
                .ForMember(y => y.FarmAgriculture, m => m.MapFrom(x => x.Get("farm_agri").EmptyOrValue()))
                .ForMember(y => y.LotFront, m => m.MapFrom(x => x.Get("front_ft").NullOrValue()))
                .ForMember(y => y.GarageSpaces, m => m.MapFrom(x => x.Get("gar_spaces").NullOrValue()))
                .ForMember(y => y.UtilitiesGas, m => m.MapFrom(x => x.Get("gas").EmptyOrValue()))
                .ForMember(y => y.PaymentFrequency, m => m.MapFrom(x => x.Get("pay_freq").EmptyOrValue()))
                .ForMember(y => y.Pool, m => m.MapFrom(x => x.Get("pool").EmptyOrValue()))
                .ForMember(y => y.UtilitiesCable, m => m.MapFrom(x => x.Get("util_cable").EmptyOrValue()))
                .ForMember(y => y.UtilitiesTelephone, m => m.MapFrom(x => x.Get("util_tel").EmptyOrValue()))
                .ForMember(y => y.Water, m => m.MapFrom(x => x.Get("water").EmptyOrValue()))
                .ForMember(y => y.Waterfront, m => m.MapFrom(x => x.Get("waterfront").EmptyOrValue()))
                .ForMember(y => y.WaterSupplyTypes, m => m.MapFrom(x => x.Get("wtr_suptyp").EmptyOrValue()))
                .ForMember(y => y.Acreage, m => m.MapFrom(x => x.Get("acres").EmptyOrValue()))
                .ForMember(y => y.LotIrregularities, m => m.MapFrom(x => x.Get("irreg").EmptyOrValue()))
                .ForMember(y => y.LeaseAgreement, m => m.MapFrom(x => x.Get("lease").EmptyOrValue()))
                .ForMember(y => y.LeaseTerm, m => m.MapFrom(x => x.Get("lease_term").EmptyOrValue()))
                .ForMember(y => y.LegalDescription, m => m.MapFrom(x => x.Get("legal_desc").EmptyOrValue()))
                .ForMember(y => y.LotFrontIncomplete, m => m.MapFrom(x => x.Get("lot_fr_inc").EmptyOrValue()))
                .ForMember(y => y.LotSizeCode, m => m.MapFrom(x => x.Get("lotsz_code").EmptyOrValue()))
                .ForMember(y => y.LeasedTerms, m => m.MapFrom(x => x.Get("lse_terms").EmptyOrValue()))
                .ForMember(y => y.Sewers, m => m.MapFrom(x => x.Get("sewer").EmptyOrValue()))
                .ForMember(y => y.ParcelOfTiedLand, m => m.MapFrom(x => x.Get("potl").EmptyOrValue()))
                .ForMember(y => y.Link, m => m.MapFrom(x => x.Get("link_yn").EmptyOrValue()))
                .ForMember(y => y.Link_Comment, m => m.MapFrom(x => x.Get("link_comment").EmptyOrValue()))
                .ForMember(y => y.OtherStructures, opt => opt.MapFrom(src => new List<string> { src.Get("oth_struc1_out").EmptyOrValue(),
                    src.Get("oth_struc2_out").EmptyOrValue()}));



        }
    }
}
