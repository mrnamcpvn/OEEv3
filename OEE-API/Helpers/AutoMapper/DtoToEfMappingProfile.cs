using AutoMapper;
using OEE_API.Dtos;
using OEE_API.Models;

namespace OEE_API.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<M_ActionTimeForOEE_Dto, M_ActionTimeForOEE>();
            CreateMap<M_DowntimeReason_Dto, M_DowntimeReason>();
            CreateMap<M_DowntimeRecord_Dto, M_DowntimeRecord>();
            CreateMap<M_Factory_Dto, M_Factory>();
            CreateMap<M_MachineInformation_Dto, M_MachineInformation>();
            CreateMap<M_MachineStatus_Dto, M_MachineStatus>();
            CreateMap<M_MachineType_Dto, M_MachineType>();
            CreateMap<M_MaintenanceTime_Dto, M_MaintenanceTime>();
            CreateMap<M_OEE_ID_Dto, M_OEE_ID>();
            CreateMap<M_OEE_MM_Dto, M_OEE_MM>();
            CreateMap<M_OEE_VN_Dto, M_OEE_VN>();
            CreateMap<M_Roles_Dto, M_Roles>();
            CreateMap<M_RoleUser_Dto, M_RoleUser>();
            CreateMap<M_RowIndex_Dto, M_RowIndex>();
            CreateMap<M_Shift_Dto, M_Shift>();
            CreateMap<M_ShiftTimeConfig_Dto, M_ShiftTimeConfig>();
            CreateMap<M_Users_Dto, M_Users>();
        }
    }
}