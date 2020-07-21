using AutoMapper;
using OEE_API.Dtos;
using OEE_API.Models;

namespace OEE_API.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<M_ActionTimeForOEE, M_ActionTimeForOEE_Dto>();
            CreateMap<M_DowntimeReason,M_DowntimeReason_Dto>();
            CreateMap< M_DowntimeRecord,M_DowntimeRecord_Dto>();
            CreateMap< M_Factory,M_Factory_Dto>();
            CreateMap<M_MachineInformation,M_MachineInformation_Dto>();
            CreateMap< M_MachineStatus,M_MachineStatus_Dto>();
            CreateMap< M_MachineType,M_MachineType_Dto>();
            CreateMap< M_MaintenanceTime,M_MaintenanceTime_Dto>();
            CreateMap< M_OEE_ID,M_OEE_Dto>();
            CreateMap< M_OEE_MM,M_OEE_Dto>();
            CreateMap< M_OEE_VN,M_OEE_Dto>();
            CreateMap< M_Roles,M_Roles_Dto>();
            CreateMap< M_RoleUser,M_RoleUser_Dto>();
            CreateMap< M_RowIndex,M_RowIndex_Dto>();
            CreateMap< M_Shift,M_Shift_Dto>();
            CreateMap< M_ShiftTimeConfig,M_ShiftTimeConfig_Dto>();
            CreateMap< M_Users,M_Users_Dto>();
            CreateMap< M_Users,UserForDetailDto>();
        }
    }
}