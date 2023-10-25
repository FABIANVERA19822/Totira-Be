
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
                .ForMember(y => y.CondoRegistryOffice, m => m.MapFrom(x => x.Get("condo_corp").NullOrValue()))
                .ForMember(y => y.Exposure, m => m.MapFrom(x => x.Get("condo_exp").NullOrValue()))
                .ForMember(y => y.CondoCorp, m => m.MapFrom(x => x.Get("corp_num").NullOrValue()))
                .ForMember(y => y.EnsuiteLaundry, m => m.MapFrom(x => x.Get("ens_lndry").NullOrValue()))
                .ForMember(y => y.GarageParkSpaces, m => m.MapFrom(x => x.Get("gar").NullOrValue()))
                .ForMember(y => y.BuildingInsuranceIncluded, m => m.MapFrom(x => x.Get("insur_bldg").NullOrValue()))
                .ForMember(y => y.ParkingType, m => m.MapFrom(x => x.Get("park_desig").NullOrValue()))

                .ForMember(y => y.ParkingDrive, m => m.MapFrom(x => x.Get("park_fac").NullOrValue()))
             
                .ForMember(y => y.Balcony, m => m.MapFrom(x => x.Get("patio_ter").NullOrValue()))
                .ForMember(y => y.PetsPermitted, m => m.MapFrom(x => x.Get("pets").NullOrValue()))
                .ForMember(y => y.SecurityGuardSystem, m => m.MapFrom(x => x.Get("secgrd_sys").NullOrValue()))
                .ForMember(y => y.SharesPer, m => m.MapFrom(x => x.Get("share_perc").NullOrValue()))
                .ForMember(y => y.Level, m => m.MapFrom(x => x.Get("stories").NullOrValue()))
                .ForMember(y => y.Unit, m => m.MapFrom(x => x.Get("unit_num").NullOrValue()))


                .ForMember(y => y.ParkingSpot, m => m.MapFrom(
                    src => new List<string> { src.Get("park_spc1").NullOrValue(),
                        src.Get("park_spc2").NullOrValue()
                    }))
                .ForMember(y => y.ParkingLegalDescription, m => m.MapFrom(
                    src => new List<string> { src.Get("park_lgl_desc1").NullOrValue(),
                        src.Get("park_lgl_desc2").NullOrValue()
                    }))
                 .ForMember(y => y.ParkingType, m => m.MapFrom(
                    src => new List<string> { src.Get("park_desig").NullOrValue(),
                        src.Get("park_desig_2").NullOrValue()
                    }))
                  
                 .ForMember(y => y.BuildingAmenities, m => m.MapFrom(
                    src => new List<string> { src.Get("bldg_amen1_out").NullOrValue(),
                        src.Get("bldg_amen2_out").NullOrValue(),
                        src.Get("bldg_amen3_out").NullOrValue(),
                        src.Get("bldg_amen4_out").NullOrValue(),
                        src.Get("bldg_amen5_out").NullOrValue(),
                        src.Get("bldg_amen6_out").NullOrValue()
                    }))
                .ForMember(y => y.CondoTaxesIncluded, m => m.MapFrom(x => x.Get("cond_txinc").NullOrValue()))
                .ForMember(y => y.Locker, m => m.MapFrom(x => x.Get("locker").NullOrValue()))
                .ForMember(y => y.LockerNum, m => m.MapFrom(x => x.Get("locker_num").NullOrValue()))
                .ForMember(y => y.Maintenance, m => m.MapFrom(x => x.Get("maint").NullOrValue()))
                .ForMember(y => y.LockerLevel, m => m.MapFrom(x => x.Get("locker_lev_unit").NullOrValue()))
                .ForMember(y => y.LockerUnit, m => m.MapFrom(x => x.Get("locker_unit").NullOrValue()));
        }
    }
}
