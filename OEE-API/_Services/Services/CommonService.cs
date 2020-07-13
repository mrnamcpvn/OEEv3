using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OEE_API._Repositories.Interfaces;
using OEE_API._Services.Interfaces;
using OEE_API.Dtos;
using OEE_API.Models;
using OEE_API.ViewModels;

namespace OEE_API._Services.Services
{
    public class CommonService : ICommonService
    {
        private readonly IFactoryRepository _repoFactory;
        private readonly IShiftRepository _repoShift;
        private readonly IActionTimeForOEERepository _repoActionTime;
        private readonly IMachineInformationRepository _repoMachineInfomation;
        private readonly IMachineTypeRepository _repoMachineType;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public CommonService(   IMapper mapper, 
                                MapperConfiguration configMapper,
                                IFactoryRepository repoFactory,
                                IShiftRepository repoShift,
                                IActionTimeForOEERepository repoActionTime,
                                IMachineInformationRepository repoMachineInfomation,
                                IMachineTypeRepository repoMachineType
                                ) {
            _repoFactory = repoFactory;
            _repoShift = repoShift;
            _mapper = mapper;
            _configMapper = configMapper;
            _repoActionTime = repoActionTime;
            _repoMachineInfomation = repoMachineInfomation;
            _repoMachineType = repoMachineType;
        }

        public async Task<List<string>> GetListBuilding(string factory)
        {
            var data = await _repoActionTime.FindAll()
                .Where(x => x.factory_id.Trim() == factory.Trim())
                .GroupBy(x => x.building_id.Trim()).Select(x => x.Key).ToListAsync();
            return data;
        }

        public async Task<List<M_Factory>> GetListFactory()
        {
            var data = await _repoFactory.FindAll()
            .ToListAsync();
            return data;
        }

        public async Task<List<MachineViewModel>> GetListMachineType(string factory, string building)
        {
            var machines = await _repoMachineInfomation.GetAll()
                    .Where(x => x.factory_id.Trim() == factory.Trim() &&
                            building == "ALL"? 1 == 1 : x.building_id.Trim() == building.Trim())
                            .ToListAsync();
            var machineType = await _repoMachineType.GetAll().ToListAsync();
            var data =  (from a in machines 
                        join b in machineType
                        on a.machine_type equals b.id
                        select new MachineViewModel() {
                            machine_id = a.machine_id,
                            machine_name = a.machine_name,
                            machine_model = a.machine_model,
                            machine_type = a.machine_type, 
                            machine_type_name = b.machine_type_name
                        }).GroupBy(x => x.machine_type).Select(x => x.First()).ToList();
            return data;
        }

        public async Task<List<M_Shift>> GetListShift()
        {
            var data = await _repoShift.FindAll().ToListAsync();
            return data;
        }
    }
}