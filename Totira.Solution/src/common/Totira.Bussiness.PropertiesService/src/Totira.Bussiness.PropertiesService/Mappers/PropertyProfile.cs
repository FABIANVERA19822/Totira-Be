namespace Totira.Bussiness.PropertiesService.Mappers
{
    using AutoMapper;
    using CrestApps.RetsSdk.Models;
    using Microsoft.AspNetCore.Routing;
    using Totira.Bussiness.PropertiesService.Domain;
    using static Totira.Bussiness.PropertiesService.Domain.Property;

    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {

            CreateMap<SearchResultRow, Property>()
                .ForMember(y => y.Id, m => m.MapFrom(x => x.Get("Ml_num").EmptyOrValue()))
                .ForMember(y => y.Condition, m => m.MapFrom(x => x.Get("cond").EmptyOrValue()))

                .ForMember(y => y.OpenHouseDate, m => m.MapFrom(
                  src => new List<DateTime> { Convert.ToDateTime(src.Get("oh_date1").NullOrValue()),
                    Convert.ToDateTime(src.Get("oh_date2").NullOrValue()),
                    Convert.ToDateTime(src.Get("oh_date3").NullOrValue()),
                    Convert.ToDateTime(src.Get("oh_date4").NullOrValue()),
                    Convert.ToDateTime(src.Get("oh_date5").NullOrValue()),
                    Convert.ToDateTime(src.Get("oh_date6").NullOrValue())
                  }))

                  .ForMember(y => y.WashroomsTypePcs, m => m.MapFrom(
                    src => new List<decimal> { Convert.ToDecimal(src.Get("wcloset_p1").NullOrValue()),
                        Convert.ToDecimal(src.Get("wcloset_p2").NullOrValue()),
                        Convert.ToDecimal(src.Get("wcloset_p3").NullOrValue()),
                        Convert.ToDecimal(src.Get("wcloset_p4").NullOrValue()),
                        Convert.ToDecimal(src.Get("wcloset_p5").NullOrValue())
                    }))

                  .ForMember(y => y.WashroomsType, m => m.MapFrom(
                    src => new List<decimal> { Convert.ToDecimal(src.Get("wcloset_t1").NullOrValue()),
                        Convert.ToDecimal(src.Get("wcloset_t2").NullOrValue()),
                        Convert.ToDecimal(src.Get("wcloset_t3").NullOrValue()),
                        Convert.ToDecimal(src.Get("wcloset_t4").NullOrValue()),
                        Convert.ToDecimal(src.Get("wcloset_t5").NullOrValue())
                    }))

                .ForMember(y => y.PropertyFeatures, m => m.MapFrom(
                    src => new List<string> { src.Get("prop_feat1_out").EmptyOrValue(),
                        src.Get("prop_feat2_out").EmptyOrValue(),
                        src.Get("prop_feat3_out").EmptyOrValue(),
                        src.Get("prop_feat4_out").EmptyOrValue(),
                        src.Get("prop_feat5_out").EmptyOrValue(),
                        src.Get("prop_feat6_out").EmptyOrValue()
                    }))

                .ForMember(y => y.Exterior, m => m.MapFrom(
                    src => new List<string> { src.Get("constr1_out").EmptyOrValue(),
                        src.Get("constr2_out").EmptyOrValue()
                    }))



                .ForMember(y => y.WashroomsTypeLevel, m => m.MapFrom(
                    src => new List<string> { src.Get("wcloset_t1lvl").EmptyOrValue(),
                        src.Get("wcloset_t2lvl").EmptyOrValue(),
                        src.Get("wcloset_t3lvl").EmptyOrValue(),
                        src.Get("wcloset_t4lvl").EmptyOrValue(),
                        src.Get("wcloset_t5lvl").EmptyOrValue()
                    }))

                .ForMember(y => y.OpenHouseTo, m => m.MapFrom(
                    src => new List<string> { src.Get("oh_to1").EmptyOrValue(),
                        src.Get("oh_to2").EmptyOrValue(),
                        src.Get("oh_to3").EmptyOrValue(),
                        src.Get("oh_to4").EmptyOrValue(),
                        src.Get("oh_to5").EmptyOrValue(),
                        src.Get("oh_to6").EmptyOrValue()
                    }))
                .ForMember(y => y.OpenHouseFrom, m => m.MapFrom(
                    src => new List<string> { src.Get("oh_from1").EmptyOrValue(),
                        src.Get("oh_from2").EmptyOrValue(),
                        src.Get("oh_from3").EmptyOrValue(),
                        src.Get("oh_from4").EmptyOrValue(),
                        src.Get("oh_from5").EmptyOrValue(),
                        src.Get("oh_from6").EmptyOrValue()
                    }))
                 .ForMember(y => y.PortionPropertyLease, m => m.MapFrom(
                    src => new List<string> { src.Get("portion_property_lease1_out").EmptyOrValue(),
                        src.Get("portion_property_lease2_out").EmptyOrValue(),
                        src.Get("portion_property_lease3_out").EmptyOrValue(),
                        src.Get("portion_property_lease4_out").EmptyOrValue()
                    }))

                 .ForMember(y => y.Basement, m => m.MapFrom(
                    src => new List<string> { src.Get("bsmt1_out").EmptyOrValue(),
                        src.Get("bsmt2_out").EmptyOrValue()
                    }))
                 .ForMember(y => y.Level, m => m.MapFrom(
                    src => new List<string> { src.Get("level1").EmptyOrValue(),
                        src.Get("level2").EmptyOrValue(),
                        src.Get("level3").EmptyOrValue(),
                        src.Get("level4").EmptyOrValue(),
                        src.Get("level5").EmptyOrValue(),
                        src.Get("level6").EmptyOrValue(),
                        src.Get("level7").EmptyOrValue(),
                        src.Get("level8").EmptyOrValue(),
                        src.Get("level9").EmptyOrValue(),
                        src.Get("level10").EmptyOrValue(),
                        src.Get("level11").EmptyOrValue(),
                        src.Get("level12").EmptyOrValue()
                    }))
                  .ForMember(y => y.SpecialDesignation, m => m.MapFrom(
                    src => new List<string> { src.Get("spec_des1_out").EmptyOrValue(),
                        src.Get("spec_des2_out").EmptyOrValue(),
                        src.Get("spec_des3_out").EmptyOrValue(),
                        src.Get("spec_des4_out").EmptyOrValue(),
                        src.Get("spec_des5_out").EmptyOrValue(),
                        src.Get("spec_des6_out").EmptyOrValue()
                    }))

                  .ForMember(y => y.Open_House_Link, m => m.MapFrom(
                    src => new List<string> { src.Get("oh_link1").EmptyOrValue(),
                        src.Get("oh_link2").EmptyOrValue(),
                        src.Get("oh_link3").EmptyOrValue(),
                        src.Get("oh_link4").EmptyOrValue(),
                        src.Get("oh_link5").EmptyOrValue(),
                        src.Get("oh_link6").EmptyOrValue()
                    }))

                  .ForMember(y => y.Open_House_Type, m => m.MapFrom(
                    src => new List<string> { src.Get("oh_type1").EmptyOrValue(),
                        src.Get("oh_type2").EmptyOrValue(),
                        src.Get("oh_type3").EmptyOrValue(),
                        src.Get("oh_type4").EmptyOrValue(),
                        src.Get("oh_type5").EmptyOrValue(),
                        src.Get("oh_type6").EmptyOrValue()
                    }))
                  .ForMember(y => y.AccessToProperty, m => m.MapFrom(
                    src => new List<string> { src.Get("access_prop1").EmptyOrValue(),
                        src.Get("access_prop2").EmptyOrValue()
                    }))

                  .ForMember(y => y.WaterFeatures, m => m.MapFrom(
                    src => new List<string> { src.Get("water_feat1").EmptyOrValue(),
                        src.Get("water_feat2").EmptyOrValue(),
                        src.Get("water_feat3").EmptyOrValue(),
                        src.Get("water_feat4").EmptyOrValue(),
                        src.Get("water_feat5").EmptyOrValue()
                    }))
                  .ForMember(y => y.Shoreline, m => m.MapFrom(
                    src => new List<string> { src.Get("shoreline1").EmptyOrValue(),
                        src.Get("shoreline2").EmptyOrValue()
                    }))

                  .ForMember(y => y.AlternativePower, m => m.MapFrom(
                    src => new List<string> { src.Get("alt_power1").EmptyOrValue(),
                        src.Get("alt_power2").EmptyOrValue()
                    }))
                  .ForMember(y => y.EasementsRestrictions, m => m.MapFrom(
                    src => new List<string> { src.Get("easement_rest1").EmptyOrValue(),
                        src.Get("easement_rest2").EmptyOrValue(),
                        src.Get("easement_rest3").EmptyOrValue(),
                        src.Get("easement_rest4").EmptyOrValue()
                    }))

                  .ForMember(y => y.RuralServices, m => m.MapFrom(
                    src => new List<string> { src.Get("rural_svc1").EmptyOrValue(),
                        src.Get("rural_svc2").EmptyOrValue(),
                        src.Get("rural_svc3").EmptyOrValue(),
                        src.Get("rural_svc4").EmptyOrValue(),
                        src.Get("rural_svc5").EmptyOrValue()
                    }))
                  .ForMember(y => y.WaterfrontAccBldgs, m => m.MapFrom(
                    src => new List<string> { src.Get("water_acc_bldg1").EmptyOrValue(),
                        src.Get("water_acc_bldg2").EmptyOrValue()
                    }))
                  .ForMember(y => y.WaterDeliveryFeatures, m => m.MapFrom(
                    src => new List<string> { src.Get("water_del_feat1").EmptyOrValue(),
                        src.Get("water_del_feat2").EmptyOrValue()
                    }))

                  .ForMember(y => y.Sewage, m => m.MapFrom(
                    src => new List<string> { src.Get("sewage1").EmptyOrValue(),
                        src.Get("sewage2").EmptyOrValue()
                    }))
                .ForMember(y => y.Province, m => m.MapFrom(x => x.Get("county").EmptyOrValue()))
                .ForMember(y => y.DirectionsCrossStreets, m => m.MapFrom(x => x.Get("cross_st").EmptyOrValue()))
                .ForMember(y => y.FamilyRoom, m => m.MapFrom(x => x.Get("den_fr").EmptyOrValue()))
                .ForMember(y => y.DisplayAddressOnInternet, m => m.MapFrom(x => x.Get("disp_addr").EmptyOrValue()))
                .ForMember(y => y.DaysOnMarket, m => m.MapFrom(x => x.Get("dom").NullOrValue()))
                .ForMember(y => y.SuspendedDate, m => m.MapFrom(x => Convert.ToDateTime(x.Get("dt_sus").NullOrValue())))
                .ForMember(y => y.TerminatedDate, m => m.MapFrom(x => Convert.ToDateTime(x.Get("dt_ter").NullOrValue())))
                .ForMember(y => y.Elevator, m => m.MapFrom(x => x.Get("elevator").EmptyOrValue()))
                .ForMember(y => y.Extras, m => m.MapFrom(x => x.Get("extras").EmptyOrValue()))
                .ForMember(y => y.FireplaceStove, m => m.MapFrom(x => x.Get("fpl_num").EmptyOrValue()))
                .ForMember(y => y.HeatSource, m => m.MapFrom(x => x.Get("fuel").EmptyOrValue()))
                .ForMember(y => y.Furnished, m => m.MapFrom(x => x.Get("furnished").EmptyOrValue()))
                .ForMember(y => y.GarageType, m => m.MapFrom(x => x.Get("gar_type").EmptyOrValue()))
                .ForMember(y => y.HeatIncluded, m => m.MapFrom(x => x.Get("heat_inc").EmptyOrValue()))
                .ForMember(y => y.HeatType, m => m.MapFrom(x => x.Get("heating").EmptyOrValue()))
                .ForMember(y => y.HydroIncluded, m => m.MapFrom(x => x.Get("hydro_inc").EmptyOrValue()))
                .ForMember(y => y.ListingEntryDate, m => m.MapFrom(x => Convert.ToDateTime(x.Get("input_date").NullOrValue())))
                .ForMember(y => y.DistributeToInternetPortals, m => m.MapFrom(x => x.Get("internet").EmptyOrValue()))
                .ForMember(y => y.OriginalPrice, m => m.MapFrom(x => x.Get("orig_dol").NullOrValue()))
                .ForMember(y => y.OutofAreaMunicipality, m => m.MapFrom(x => x.Get("outof_area").EmptyOrValue()))
                .ForMember(y => y.ParcelId, m => m.MapFrom(x => x.Get("parcel_id").EmptyOrValue()))
                .ForMember(y => y.ParkCostMo, m => m.MapFrom(x => x.Get("park_chgs").NullOrValue()))
                .ForMember(y => y.ParkingSpaces, m => m.MapFrom(x => x.Get("park_spcs").NullOrValue()))
                .ForMember(y => y.PerListingPrice, m => m.MapFrom(x => x.Get("perc_dif").NullOrValue()))
                .ForMember(y => y.PriorLSC, m => m.MapFrom(x => x.Get("pr_lsc").EmptyOrValue()))
                .ForMember(y => y.ParkingIncluded, m => m.MapFrom(x => x.Get("prkg_inc").EmptyOrValue()))

                .ForMember(y => y.Rooms, m => m.MapFrom(x => x.Get("rms").NullOrValue()))
                .ForMember(y => y.RoomsPlus, m => m.MapFrom(x => x.Get("rooms_plus").NullOrValue()))
                .ForMember(y => y.Uffi, m => m.MapFrom(x => x.Get("uffi").EmptyOrValue()))
                .ForMember(y => y.UnavailableDate, m => m.MapFrom(x => Convert.ToDateTime(x.Get("unavail_dt").NullOrValue())))
                .ForMember(y => y.SellerPropertyInfoStatement, m => m.MapFrom(x => x.Get("vend_pis").EmptyOrValue()))
                .ForMember(y => y.VirtualTourUploadDate, m => m.MapFrom(x => Convert.ToDateTime(x.Get("vtour_updt").NullOrValue())))
                .ForMember(y => y.WaterIncluded, m => m.MapFrom(x => x.Get("water_inc").EmptyOrValue()))

                .ForMember(y => y.ExpiryDate, m => m.MapFrom(x => Convert.ToDateTime(x.Get("xd").NullOrValue())))
                .ForMember(y => y.ExtensionEntryDate, m => m.MapFrom(x => Convert.ToDateTime(x.Get("xdtd").NullOrValue())))
                .ForMember(y => y.TaxYear, m => m.MapFrom(x => x.Get("yr").NullOrValue()))
                .ForMember(y => y.ApproxAge, m => m.MapFrom(x => x.Get("yr_built").EmptyOrValue()))
                .ForMember(y => y.PostalCode, m => m.MapFrom(x => x.Get("zip").EmptyOrValue()))
                .ForMember(y => y.Zoning, m => m.MapFrom(x => x.Get("zoning").EmptyOrValue()))
                .ForMember(y => y.TimestampSql, m => m.MapFrom(x => x.Get("timestamp_sql").NullOrValue()))
                .ForMember(y => y.MunicipalityCode, m => m.MapFrom(x => x.Get("municipality_code").EmptyOrValue()))
                .ForMember(y => y.Area, m => m.MapFrom(x => x.Get("area").EmptyOrValue()))
                .ForMember(y => y.Community, m => m.MapFrom(x => x.Get("community").EmptyOrValue()))
                .ForMember(y => y.CertLevel, m => m.MapFrom(x => x.Get("cert_lvl").EmptyOrValue()))
                .ForMember(y => y.EnergyCertification, m => m.MapFrom(x => x.Get("energy_cert").EmptyOrValue()))
                .ForMember(y => y.PhyHandiEquipped, m => m.MapFrom(x => x.Get("handi_equipped").EmptyOrValue()))


                .ForMember(y => y.AirConditioning, m => m.MapFrom(x => x.Get("a_c").EmptyOrValue()))

                .ForMember(y => y.PortionPropertyLeaseSrch, m => m.MapFrom(x => x.Get("portion_property_lease_srch").EmptyOrValue()))
                .ForMember(y => y.PortionLeaseComments, m => m.MapFrom(x => x.Get("portion_lease_comments").EmptyOrValue()))
                .ForMember(y => y.Assignment, m => m.MapFrom(x => x.Get("assignment").EmptyOrValue()))
                .ForMember(y => y.FractionalOwnership, m => m.MapFrom(x => x.Get("fractional_ownership").EmptyOrValue()))
                .ForMember(y => y.RemarksForClients, m => m.MapFrom(x => x.Get("ad_text").EmptyOrValue()))
                .ForMember(y => y.AddlMonthlyFees, m => m.MapFrom(x => x.Get("addl_mo_fee").NullOrValue()))
                .ForMember(y => y.Address, m => m.MapFrom(x => x.Get("addr").EmptyOrValue()))
                .ForMember(y => y.AllInclusive, m => m.MapFrom(x => x.Get("all_inc").EmptyOrValue()))
                .ForMember(y => y.AptUnit, m => m.MapFrom(x => x.Get("apt_num").EmptyOrValue()))
                .ForMember(y => y.AssessmentYear, m => m.MapFrom(x => x.Get("ass_year").NullOrValue()))
                .ForMember(y => y.Washrooms, m => m.MapFrom(x => x.Get("bath_tot").NullOrValue()))
                .ForMember(y => y.Bedrooms, m => m.MapFrom(x => x.Get("br").NullOrValue()))
                .ForMember(y => y.BedroomsPlus, m => m.MapFrom(x => x.Get("br_plus").NullOrValue()))

                .ForMember(y => y.CableTVIncluded, m => m.MapFrom(x => x.Get("cable").EmptyOrValue()))
                .ForMember(y => y.CacIncluded, m => m.MapFrom(x => x.Get("cac_inc").EmptyOrValue()))
                .ForMember(y => y.SoldDate, m => m.MapFrom(x => Convert.ToDateTime(x.Get("cd").NullOrValue())))
                .ForMember(y => y.CentralVac, m => m.MapFrom(x => x.Get("central_vac").EmptyOrValue()))
                .ForMember(y => y.ConditionalExpiryDate, m => m.MapFrom(x => x.Get("cndsold_xd").NullOrValue()))
                .ForMember(y => y.CommissionCoOpBrokerage, m => m.MapFrom(x => x.Get("com_coopb").EmptyOrValue()))
                .ForMember(y => y.CommonElementsIncluded, m => m.MapFrom(x => x.Get("comel_inc").EmptyOrValue()))
                .ForMember(y => y.KitchensPlus, m => m.MapFrom(x => x.Get("kit_plus").NullOrValue()))
                .ForMember(y => y.LaundryAccess, m => m.MapFrom(x => x.Get("laundry").EmptyOrValue()))
                .ForMember(y => y.LaundryLevel, m => m.MapFrom(x => x.Get("laundry_lev").EmptyOrValue()))
                .ForMember(y => y.ContractDate, m => m.MapFrom(x => Convert.ToDateTime(x.Get("ld").NullOrValue())))

                .ForMember(y => y.ListPrice, m => m.MapFrom(x => x.Get("lp_dol").NullOrValue()))
                .ForMember(y => y.LastStatus, m => m.MapFrom(x => x.Get("lsc").EmptyOrValue()))

                .ForMember(y => y.MapColumn, m => m.MapFrom(x => x.Get("mmap_col").NullOrValue()))
                .ForMember(y => y.Map, m => m.MapFrom(x => x.Get("mmap_page").NullOrValue()))
                .ForMember(y => y.MapRow, m => m.MapFrom(x => x.Get("mmap_row").EmptyOrValue()))
                .ForMember(y => y.Kitchens, m => m.MapFrom(x => x.Get("num_kit").NullOrValue()))
                .ForMember(y => y.PossessionRemarks, m => m.MapFrom(x => x.Get("occ").EmptyOrValue()))
                .ForMember(y => y.PropertyMgmtCo, m => m.MapFrom(x => x.Get("prop_mgmt").EmptyOrValue()))
                .ForMember(y => y.PrivateEntrance, m => m.MapFrom(x => x.Get("pvt_ent").EmptyOrValue()))
                .ForMember(y => y.Retirement, m => m.MapFrom(x => x.Get("retirement").EmptyOrValue()))
                .ForMember(y => y.ListBrokerage, m => m.MapFrom(x => x.Get("rltr").EmptyOrValue()))

                .ForMember(y => y.SaleLease, m => m.MapFrom(x => x.Get("s_r").EmptyOrValue()))
                .ForMember(y => y.SoldPrice, m => m.MapFrom(x => x.Get("sp_dol").NullOrValue()))

                .ForMember(y => y.ApproxSquareFootage, m => m.MapFrom(x => x.Get("sqft").EmptyOrValue()))
                .ForMember(y => y.MinApproxSquareFootage, m => m.MapFrom(x => CalculateMinSqft(x.Get("sqft").EmptyOrValue())))
                .ForMember(y => y.MaxApproxSquareFootage, m => m.MapFrom(x => CalculateMaxSqft(x.Get("sqft").EmptyOrValue())))
                .ForMember(y => y.StreetName, m => m.MapFrom(x => x.Get("st").EmptyOrValue()))
                .ForMember(y => y.StreetDirection, m => m.MapFrom(x => x.Get("st_dir").EmptyOrValue()))
                .ForMember(y => y.Street, m => m.MapFrom(x => x.Get("st_num").EmptyOrValue()))
                .ForMember(y => y.StreetAbbreviation, m => m.MapFrom(x => x.Get("st_sfx").EmptyOrValue()))
                .ForMember(y => y.Status, m => m.MapFrom(x => x.Get("status").EmptyOrValue()))
                .ForMember(y => y.Style, m => m.MapFrom(x => x.Get("style").EmptyOrValue()))
                .ForMember(y => y.Taxes, m => m.MapFrom(x => x.Get("taxes").NullOrValue()))
                .ForMember(y => y.ClosedDate, m => m.MapFrom(x => x.Get("td").NullOrValue()))
                .ForMember(y => y.VirtualTourURL, m => m.MapFrom(x => x.Get("tour_url").EmptyOrValue()))
                .ForMember(y => y.CommunityCode, m => m.MapFrom(x => x.Get("community_code").EmptyOrValue()))
                .ForMember(y => y.AreaCode, m => m.MapFrom(x => x.Get("area_code").EmptyOrValue()))
                .ForMember(y => y.Assessment, m => m.MapFrom(x => x.Get("tv").NullOrValue()))
                .ForMember(y => y.TypeOwnSrch, m => m.MapFrom(x => x.Get("type_own_srch").EmptyOrValue()))
                .ForMember(y => y.TypeOwn1Out, m => m.MapFrom(x => x.Get("type_own1_out").EmptyOrValue()))
                .ForMember(y => y.MunicipalityDistrict, m => m.MapFrom(x => x.Get("municipality_district").EmptyOrValue()))
                .ForMember(y => y.Municipality, m => m.MapFrom(x => x.Get("municipality").EmptyOrValue()))
                .ForMember(y => y.PixUpdtedDt, m => m.MapFrom(x => x.Get("pix_updt").NullOrValue()))

                .ForMember(y => y.OpenHouseUpDtTimestamp, m => m.MapFrom(x => x.Get("oh_dt_stamp").NullOrValue()))
                .ForMember(y => y.GreenPropInfoStatement, m => m.MapFrom(x => x.Get("green_pis").EmptyOrValue()))
                .ForMember(y => y.PossessionDate, m => m.MapFrom(x => Convert.ToDateTime(x.Get("poss_date").NullOrValue())))
                .ForMember(y => y.WaterBodyName, m => m.MapFrom(x => x.Get("water_body").EmptyOrValue()))
                .ForMember(y => y.WaterBodyType, m => m.MapFrom(x => x.Get("water_type").EmptyOrValue()))

                .ForMember(y => y.WaterFrontage, m => m.MapFrom(x => x.Get("water_front").NullOrValue()))

                .ForMember(y => y.ShorelineAllowance, m => m.MapFrom(x => x.Get("shore_allow").EmptyOrValue()))
                .ForMember(y => y.ShorelineExposure, m => m.MapFrom(x => x.Get("shoreline_exp").EmptyOrValue()))

                .ForMember(y => y.TotalParkingSpaces, m => m.MapFrom(x => x.Get("tot_park_spcs").NullOrValue()))



                //Rooms

                .ForMember(y => y.Room1Length, m => m.MapFrom(x => x.Get("rm1_len").EmptyOrValue()))
                .ForMember(y => y.Room2Length, m => m.MapFrom(x => x.Get("rm2_len").EmptyOrValue()))
                .ForMember(y => y.Room3Length, m => m.MapFrom(x => x.Get("rm3_len").EmptyOrValue()))
                .ForMember(y => y.Room4Length, m => m.MapFrom(x => x.Get("rm4_len").EmptyOrValue()))
                .ForMember(y => y.Room5Length, m => m.MapFrom(x => x.Get("rm5_len").EmptyOrValue()))
                .ForMember(y => y.Room6Length, m => m.MapFrom(x => x.Get("rm6_len").EmptyOrValue()))
                .ForMember(y => y.Room7Length, m => m.MapFrom(x => x.Get("rm7_len").EmptyOrValue()))
                .ForMember(y => y.Room8Length, m => m.MapFrom(x => x.Get("rm8_len").EmptyOrValue()))
                .ForMember(y => y.Room9Length, m => m.MapFrom(x => x.Get("rm9_len").EmptyOrValue()))
                .ForMember(y => y.Room10Length, m => m.MapFrom(x => x.Get("rm10_len").EmptyOrValue()))
                .ForMember(y => y.Room11Length, m => m.MapFrom(x => x.Get("rm11_len").EmptyOrValue()))
                .ForMember(y => y.Room12Length, m => m.MapFrom(x => x.Get("rm12_len").EmptyOrValue()))

                .ForMember(y => y.Room1Width, m => m.MapFrom(x => x.Get("rm1_wth").EmptyOrValue()))
                .ForMember(y => y.Room2Width, m => m.MapFrom(x => x.Get("rm2_wth").EmptyOrValue()))
                .ForMember(y => y.Room3Width, m => m.MapFrom(x => x.Get("rm3_wth").EmptyOrValue()))
                .ForMember(y => y.Room4Width, m => m.MapFrom(x => x.Get("rm4_wth").EmptyOrValue()))
                .ForMember(y => y.Room5Width, m => m.MapFrom(x => x.Get("rm5_wth").EmptyOrValue()))
                .ForMember(y => y.Room6Width, m => m.MapFrom(x => x.Get("rm6_wth").EmptyOrValue()))
                .ForMember(y => y.Room7Width, m => m.MapFrom(x => x.Get("rm7_wth").EmptyOrValue()))
                .ForMember(y => y.Room8Width, m => m.MapFrom(x => x.Get("rm8_wth").EmptyOrValue()))
                .ForMember(y => y.Room9Width, m => m.MapFrom(x => x.Get("rm9_wth").EmptyOrValue()))
                .ForMember(y => y.Room10Width, m => m.MapFrom(x => x.Get("rm10_wth").EmptyOrValue()))
                .ForMember(y => y.Room11Width, m => m.MapFrom(x => x.Get("rm11_wth").EmptyOrValue()))
                .ForMember(y => y.Room12Width, m => m.MapFrom(x => x.Get("rm12_wth").EmptyOrValue()))

                .ForMember(y => y.Room1Desc1, m => m.MapFrom(x => x.Get("rm1_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room1Desc2, m => m.MapFrom(x => x.Get("rm1_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room1Desc3, m => m.MapFrom(x => x.Get("rm1_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room2Desc1, m => m.MapFrom(x => x.Get("rm2_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room2Desc2, m => m.MapFrom(x => x.Get("rm2_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room2Desc3, m => m.MapFrom(x => x.Get("rm2_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room3Desc1, m => m.MapFrom(x => x.Get("rm3_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room3Desc2, m => m.MapFrom(x => x.Get("rm3_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room3Desc3, m => m.MapFrom(x => x.Get("rm3_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room4Desc1, m => m.MapFrom(x => x.Get("rm4_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room4Desc2, m => m.MapFrom(x => x.Get("rm4_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room4Desc3, m => m.MapFrom(x => x.Get("rm4_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room5Desc1, m => m.MapFrom(x => x.Get("rm5_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room5Desc2, m => m.MapFrom(x => x.Get("rm5_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room5Desc3, m => m.MapFrom(x => x.Get("rm5_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room6Desc1, m => m.MapFrom(x => x.Get("rm6_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room6Desc2, m => m.MapFrom(x => x.Get("rm6_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room6Desc3, m => m.MapFrom(x => x.Get("rm6_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room7Desc1, m => m.MapFrom(x => x.Get("rm7_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room7Desc2, m => m.MapFrom(x => x.Get("rm7_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room7Desc3, m => m.MapFrom(x => x.Get("rm7_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room8Desc1, m => m.MapFrom(x => x.Get("rm8_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room8Desc2, m => m.MapFrom(x => x.Get("rm8_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room8Desc3, m => m.MapFrom(x => x.Get("rm8_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room9Desc1, m => m.MapFrom(x => x.Get("rm9_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room9Desc2, m => m.MapFrom(x => x.Get("rm9_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room9Desc3, m => m.MapFrom(x => x.Get("rm9_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room10Desc1, m => m.MapFrom(x => x.Get("rm10_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room10Desc2, m => m.MapFrom(x => x.Get("rm10_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room10Desc3, m => m.MapFrom(x => x.Get("rm10_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room11Desc1, m => m.MapFrom(x => x.Get("rm11_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room11Desc2, m => m.MapFrom(x => x.Get("rm11_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room11Desc3, m => m.MapFrom(x => x.Get("rm11_dc3_out").EmptyOrValue()))
                .ForMember(y => y.Room12Desc1, m => m.MapFrom(x => x.Get("rm12_dc1_out").EmptyOrValue()))
                .ForMember(y => y.Room12Desc2, m => m.MapFrom(x => x.Get("rm12_dc2_out").EmptyOrValue()))
                .ForMember(y => y.Room12Desc3, m => m.MapFrom(x => x.Get("rm12_dc3_out").EmptyOrValue()))


                .ForMember(y => y.Room1, m => m.MapFrom(x => x.Get("rm1_out").EmptyOrValue()))
                .ForMember(y => y.Room2, m => m.MapFrom(x => x.Get("rm2_out").EmptyOrValue()))
                .ForMember(y => y.Room3, m => m.MapFrom(x => x.Get("rm3_out").EmptyOrValue()))
                .ForMember(y => y.Room4, m => m.MapFrom(x => x.Get("rm4_out").EmptyOrValue()))
                .ForMember(y => y.Room5, m => m.MapFrom(x => x.Get("rm5_out").EmptyOrValue()))
                .ForMember(y => y.Room6, m => m.MapFrom(x => x.Get("rm6_out").EmptyOrValue()))
                .ForMember(y => y.Room7, m => m.MapFrom(x => x.Get("rm7_out").EmptyOrValue()))
                .ForMember(y => y.Room8, m => m.MapFrom(x => x.Get("rm8_out").EmptyOrValue()))
                .ForMember(y => y.Room9, m => m.MapFrom(x => x.Get("rm9_out").EmptyOrValue()))
                .ForMember(y => y.Room10, m => m.MapFrom(x => x.Get("rm10_out").EmptyOrValue()))
                .ForMember(y => y.Room11, m => m.MapFrom(x => x.Get("rm11_out").EmptyOrValue()))
                .ForMember(y => y.Room12, m => m.MapFrom(x => x.Get("rm12_out").EmptyOrValue()));


        }

        public static double? CalculateMinSqft(string sqft)
        {
            if (!string.IsNullOrEmpty(sqft))
            {
                var value = sqft.Split('-', '+');
                if (value.Length > 0)
                {
                    if (double.TryParse(value[0], out double result))
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        public static double? CalculateMaxSqft(string sqft)
        {
            if (!string.IsNullOrEmpty(sqft))
            {
                var value = sqft.Split('-', '+');
                if (value.Length >= 2)
                {
                    if (value[1] == "+")
                        return double.Parse(value[1]);
                    if (double.TryParse(value[1], out double result))
                        return result;
                }
            }
            return null;
        }
    }
}
