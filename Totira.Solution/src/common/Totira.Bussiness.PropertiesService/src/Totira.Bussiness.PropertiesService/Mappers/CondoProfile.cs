
namespace Totira.Bussiness.PropertiesService.Mappers
{
    using AutoMapper;
    using CrestApps.RetsSdk.Models;
    using Totira.Bussiness.PropertiesService.Domain;
    public class CondoProfile : Profile
    {
        public CondoProfile() {
            ////Condo

            CreateMap<SearchResultRow, Condo>()
                .ForMember(y => y.CondoRegistryOffice, m => m.MapFrom(x => x.Get("condo_corp").EmptyOrValue()))
                .ForMember(y => y.Exposure, m => m.MapFrom(x => x.Get("condo_exp").EmptyOrValue()))
                .ForMember(y => y.CondoCorp, m => m.MapFrom(x => x.Get("corp_num").NullOrValue()))
                .ForMember(y => y.EnsuiteLaundry, m => m.MapFrom(x => x.Get("ens_lndry").EmptyOrValue()))
                .ForMember(y => y.GarageParkSpaces, m => m.MapFrom(x => x.Get("gar").NullOrValue()))
                .ForMember(y => y.BuildingInsuranceIncluded, m => m.MapFrom(x => x.Get("insur_bldg").EmptyOrValue())) 

                .ForMember(y => y.ParkingDrive, m => m.MapFrom(x => x.Get("park_fac").EmptyOrValue()))
             
                .ForMember(y => y.Balcony, m => m.MapFrom(x => x.Get("patio_ter").EmptyOrValue()))
                .ForMember(y => y.PetsPermitted, m => m.MapFrom(x => x.Get("pets").EmptyOrValue()))
                .ForMember(y => y.SecurityGuardSystem, m => m.MapFrom(x => x.Get("secgrd_sys").EmptyOrValue()))
                .ForMember(y => y.SharesPer, m => m.MapFrom(x => x.Get("share_perc").EmptyOrValue()))
                .ForMember(y => y.Level, m => m.MapFrom(x => x.Get("stories").EmptyOrValue()))
                .ForMember(y => y.Unit, m => m.MapFrom(x => x.Get("unit_num").EmptyOrValue()))


                .ForMember(y => y.ParkingSpot, m => m.MapFrom(
                    src => new List<string> { src.Get("park_spc1").EmptyOrValue(),
                        src.Get("park_spc2").EmptyOrValue()
                    }))
                .ForMember(y => y.ParkingLegalDescription, m => m.MapFrom(
                    src => new List<string> { src.Get("park_lgl_desc1").EmptyOrValue(),
                        src.Get("park_lgl_desc2").EmptyOrValue()
                    }))
                 .ForMember(y => y.ParkingType, m => m.MapFrom(
                    src => new List<string> { src.Get("park_desig").EmptyOrValue(),
                        src.Get("park_desig_2").EmptyOrValue()
                    }))
                  
                 .ForMember(y => y.BuildingAmenities, m => m.MapFrom(
                    src => new List<string> { src.Get("bldg_amen1_out").EmptyOrValue(),
                        src.Get("bldg_amen2_out").EmptyOrValue(),
                        src.Get("bldg_amen3_out").EmptyOrValue(),
                        src.Get("bldg_amen4_out").EmptyOrValue(),
                        src.Get("bldg_amen5_out").EmptyOrValue(),
                        src.Get("bldg_amen6_out").EmptyOrValue()
                    }))
                .ForMember(y => y.CondoTaxesIncluded, m => m.MapFrom(x => x.Get("cond_txinc").EmptyOrValue()))
                .ForMember(y => y.Locker, m => m.MapFrom(x => x.Get("locker").EmptyOrValue()))
                .ForMember(y => y.LockerNum, m => m.MapFrom(x => x.Get("locker_num").EmptyOrValue()))
                .ForMember(y => y.Maintenance, m => m.MapFrom(x => x.Get("maint").NullOrValue()))
                .ForMember(y => y.LockerLevel, m => m.MapFrom(x => x.Get("locker_lev_unit").EmptyOrValue()))
                .ForMember(y => y.LockerUnit, m => m.MapFrom(x => x.Get("locker_unit").EmptyOrValue()));
        }
    }
}
