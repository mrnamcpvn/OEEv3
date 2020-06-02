using AutoMapper;
using OEE_API.Application.ViewModels;
using OEE_API.Models.SHW_SHD;

namespace OEE_API.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile() {
            CreateMap<Cell_OEE, BuildingViewModel>();
            CreateMap<ActionTime, ChartReason>();
            CreateMap<OeeReport_Today, OeeReport_test>();
        }
    }
}