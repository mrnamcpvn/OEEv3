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
        private readonly IOEE_VNRepository _repoOee_VN;
        private readonly IOEE_MMRepository _repoOee_MM;
        private readonly IOEE_IDRepository _repoOee_ID;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public CommonService(   IMapper mapper, 
                                MapperConfiguration configMapper,
                                IFactoryRepository repoFactory,
                                IShiftRepository repoShift,
                                IActionTimeForOEERepository repoActionTime,
                                IMachineInformationRepository repoMachineInfomation,
                                IMachineTypeRepository repoMachineType,
                                IOEE_VNRepository repoOee_VN,
                                IOEE_MMRepository repoOee_MM,
                                IOEE_IDRepository repoOee_ID
                                ) {
            _repoFactory = repoFactory;
            _repoShift = repoShift;
            _mapper = mapper;
            _configMapper = configMapper;
            _repoActionTime = repoActionTime;
            _repoMachineInfomation = repoMachineInfomation;
            _repoMachineType = repoMachineType;
            _repoOee_VN = repoOee_VN;
            _repoOee_MM = repoOee_MM;
            _repoOee_ID = repoOee_ID;
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

        public async Task<List<MachineInformationModel>> GetListMachineID(string factory, string building, string machine_type)
        {
            var machines = await _repoMachineInfomation.FindAll()
                .Where(x => x.factory_id.Trim() == factory.Trim() &&
                            x.building_id.Trim() == building.Trim() &&
                            x.machine_type.ToString() == machine_type.Trim())
                .Select(x => new MachineInformationModel() {
                    machine_type = x.machine_type,
                    machine_id = x.machine_id,
                    machine_name = x.machine_name
                }).Distinct().ToListAsync();
            return machines;
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
                            id = b.id,
                            machine_type_name = b.machine_type_name
                        }).GroupBy(x => x.id).Select(x => x.First()).ToList();
            return data;
        }

        public async Task<List<M_OEE_Dto>> GetListOEE(string factory)
        {
            var data = new List<M_OEE_Dto>();
            if (factory.Trim() == "CB" || factory.Trim() == "SHC") {
                data = await _repoOee_VN.GetAll().ProjectTo<M_OEE_Dto>(_configMapper).ToListAsync();;
            } else if(factory.Trim() == "SPC") {
                data = await _repoOee_MM.GetAll().ProjectTo<M_OEE_Dto>(_configMapper).ToListAsync();;
            } else if(factory.Trim() == "SYF") {
                data = await _repoOee_ID.GetAll().ProjectTo<M_OEE_Dto>(_configMapper).ToListAsync();;
            } else if(factory.Trim() == "ALL") {
                var data_CB_SHC = await _repoOee_VN.GetAll().ProjectTo<M_OEE_Dto>(_configMapper).ToListAsync();
                var data_SPC = await _repoOee_MM.GetAll().ProjectTo<M_OEE_Dto>(_configMapper).ToListAsync();
                var data_SYF = await _repoOee_ID.GetAll().ProjectTo<M_OEE_Dto>(_configMapper).ToListAsync();
                data.AddRange(data_CB_SHC);
                data.AddRange(data_SPC);
                data.AddRange(data_SYF);
            }
            return data;
        }

        public async Task<List<M_Shift>> GetListShift()
        {
            var data = await _repoShift.FindAll().ToListAsync();
            return data;
        }

        public async Task<List<string>> ListMachineID(string factory, string building)
        {
            var machines = await _repoMachineInfomation.FindAll()
                    .Where(x => x.factory_id.Trim() == factory.Trim() &&
                    x.building_id.Trim() == building.Trim()).Select(x => x.machine_id).Distinct().ToListAsync();
            return machines;
        }

        public async Task<List<string>> ListMachineID(string factory, string building, string machine_type)
        {
            var listMachine = await _repoMachineInfomation.FindAll()
                                    .Where( x => x.factory_id.Trim() == factory.Trim() &&
                                            x.building_id.Trim() == building.Trim() &&
                                            x.machine_type.ToString().Trim() == machine_type.Trim())
                                    .Select(x => x.machine_id.Trim()).ToListAsync();
            return listMachine;
        }
    }
}