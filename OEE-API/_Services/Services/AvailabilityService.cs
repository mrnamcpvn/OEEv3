using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OEE_API._Repositories.Interfaces;
using OEE_API._Services.Interfaces;

namespace OEE_API._Services.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IOEE_VNRepository _oEE_VNRepository;
        private readonly IOEE_MMRepository _oEE_MMRepository;
        private readonly ICommonService _commonService;

        private readonly IOEE_IDRepository _oEE_IDRepository;
        public AvailabilityService(IOEE_VNRepository oEE_VNRepository, IOEE_MMRepository oEE_MMRepository,
        IOEE_IDRepository oEE_IDRepository, ICommonService commonService)
        {
            _commonService = commonService;
            _oEE_IDRepository = oEE_IDRepository;
            _oEE_MMRepository = oEE_MMRepository;
            _oEE_VNRepository = oEE_VNRepository;

        }
        public async Task<Dictionary<string, int>> GetListAvailability(string factory, string building, string machine_type,
        string shift, string date, string dateTo)
        {
            Dictionary<string, int> model = new Dictionary<string, int>();

            var data = _oEE_VNRepository.FindAll();
            var SHBdata = _oEE_MMRepository.FindAll();
            var SY2data = _oEE_IDRepository.FindAll();
            if (shift.Trim() == "1")
            {
                data = data.Where(x => x.shift_id == 1);
                SHBdata = SHBdata.Where(x => x.shift_id == 1);
                SY2data = SY2data.Where(x => x.shift_id == 1);
            }
            if (shift.Trim() == "2")
            {
                data = data.Where(x => x.shift_id == 2);
                SHBdata = SHBdata.Where(x => x.shift_id == 2);
                SY2data = SY2data.Where(x => x.shift_id == 2);
            }

            data = data.Where(x => x.shift_day >= Convert.ToDateTime(date).Day
                                && x.shift_month >= Convert.ToDateTime(date).Month
                                && x.shift_year >= Convert.ToDateTime(date).Year
                                && x.shift_day <= Convert.ToDateTime(dateTo).Day
                                && x.shift_month <= Convert.ToDateTime(dateTo).Month
                                && x.shift_year <= Convert.ToDateTime(dateTo).Year);

            SHBdata = SHBdata.Where(x => x.shift_day >= Convert.ToDateTime(date).Day
                                 && x.shift_month >= Convert.ToDateTime(date).Month
                                 && x.shift_year >= Convert.ToDateTime(date).Year
                                 && x.shift_day <= Convert.ToDateTime(dateTo).Day
                                 && x.shift_month <= Convert.ToDateTime(dateTo).Month
                                 && x.shift_year <= Convert.ToDateTime(dateTo).Year);
            SY2data = SY2data.Where(x => x.shift_day >= Convert.ToDateTime(date).Day
                                 && x.shift_month >= Convert.ToDateTime(date).Month
                                 && x.shift_year >= Convert.ToDateTime(date).Year
                                 && x.shift_day <= Convert.ToDateTime(dateTo).Day
                                 && x.shift_month <= Convert.ToDateTime(dateTo).Month
                                 && x.shift_year <= Convert.ToDateTime(dateTo).Year);

            if (factory == "ALL")
            {

                var SHCData = data.Where(x => x.factory_id.Trim() == "SHC").Count() == 0 ? 0 :
                             data.Where(x => x.factory_id.Trim() == "SHC").Sum(z => z.oee_rate) /
                            data.Where(x => x.factory_id.Trim() == "SHC").Count();

                var SHDData = data.Where(x => x.factory_id.Trim() == "CB").Count() == 0 ? 0 :
                data.Where(x => x.factory_id.Trim() == "CB").Sum(z => z.oee_rate) /
                            data.Where(x => x.factory_id.Trim() == "CB").Count();
                var INdata = SY2data.Count() == 0 ? 0 :
                              SY2data.Sum(z => z.oee_rate) /
                               SY2data.Count();
                var MMdata = SHBdata.Count() == 0 ? 0 :
                                SHBdata.Sum(z => z.oee_rate) /
                                 SHBdata.Count();

                model.Add("SHW", (int)SHCData);
                model.Add("SHD", (int)SHDData);
                model.Add("SHB", (int)INdata);
                model.Add("SY2", (int)MMdata);
            }
            else if (factory != "ALL" && building == "ALL")
            {
                var Buildings = await _commonService.GetListBuilding(factory);
                foreach (var itemBuilding in Buildings)
                {
                    if (itemBuilding != null)
                    {
                        decimal value = 0;
                        if (factory == "SHB")
                        {
                            value = SHBdata.Where(x => x.building_id.Trim() == itemBuilding.Trim()).Count() == 0 ? 0 :
                                   SHBdata.Where(x => x.building_id.Trim() == itemBuilding.Trim()).Sum(z => z.oee_rate) /
                                    SHBdata.Where(x => x.building_id.Trim() == itemBuilding.Trim()).Count();
                        }
                        else if (factory == "SY2")
                        {
                            value = SY2data.Where(x => x.building_id.Trim() == itemBuilding.Trim()).Count() == 0 ? 0 :
                                            SY2data.Where(x => x.building_id.Trim() == itemBuilding.Trim()).Sum(z => z.oee_rate) /
                                             SY2data.Where(x => x.building_id.Trim() == itemBuilding.Trim()).Count();
                        }
                        else
                        {
                            value = data.Where(x => x.factory_id.Trim() == factory.Trim()
                           && x.building_id.Trim() == itemBuilding.Trim()).Count() == 0 ? 0 :
                              data.Where(x => x.factory_id.Trim() == factory.Trim()
                             && x.building_id.Trim() == itemBuilding.Trim())
                            .Sum(z => z.oee_rate) /
                            data.Where(x => x.factory_id.Trim() == factory.Trim()
                            && x.building_id.Trim() == itemBuilding.Trim()).Count();
                        }
                        model.Add(itemBuilding, (int)value);
                    }
                }
            }
            else if (building != "ALL" && factory != "ALL")
            {
                var MachineTypes = await _commonService.GetListMachineType(factory, building);
                if (machine_type == "ALL")
                {
                    foreach (var item in MachineTypes)
                    {
                        if (item.machine_type_name != null)
                        {
                            decimal value = 0;
                            if (factory == "SHB")
                            {
                                value = SHBdata.Where(x => x.building_id.Trim() == building.Trim()).Count() == 0 ? 0 :
                                       SHBdata.Where(x => x.building_id.Trim() == building.Trim()).Sum(z => z.oee_rate) /
                                        SHBdata.Where(x => x.building_id.Trim() == building.Trim()).Count();
                            }
                            else if (factory == "SY2")
                            {
                                value = SY2data.Where(x => x.building_id.Trim() == building.Trim()).Count() == 0 ? 0 :
                                                SY2data.Where(x => x.building_id.Trim() == building.Trim()).Sum(z => z.oee_rate) /
                                                 SY2data.Where(x => x.building_id.Trim() == building.Trim()).Count();
                            }
                            else
                            {
                                value = data.Where(x => x.factory_id.Trim() == factory.Trim()
                               && x.building_id.Trim() == building.Trim()).Count() == 0 ? 0 :
                                  data.Where(x => x.factory_id.Trim() == factory.Trim()
                                 && x.building_id.Trim() == building.Trim())
                                .Sum(z => z.oee_rate) /
                                data.Where(x => x.factory_id.Trim() == factory.Trim()
                                && x.building_id.Trim() == building.Trim()).Count();
                            }
                            model.Add(item.machine_id, (int)value);
                        }
                    }

                }
                else
                {
                      decimal value = 0;
                            if (factory == "SHB")
                            {
                                value = SHBdata.Where(x => x.building_id.Trim() == building.Trim() && x.machine_id.Trim()
                                == machine_type.Trim()).Count() == 0 ? 0 :
                                       SHBdata.Where(x => x.building_id.Trim() == building.Trim()   && x.machine_id.Trim()
                                == machine_type.Trim()).Sum(z => z.oee_rate) /
                                        SHBdata.Where(x => x.building_id.Trim() == building.Trim()  && x.machine_id.Trim()
                                == machine_type.Trim()).Count();
                            }
                            else if (factory == "SY2")
                            {
                                value = SY2data.Where(x => x.building_id.Trim() == building.Trim()  && x.machine_id.Trim()
                                == machine_type.Trim()).Count() == 0 ? 0 :
                                                SY2data.Where(x => x.building_id.Trim() == building.Trim() && x.machine_id.Trim()
                                == machine_type.Trim()).Sum(z => z.oee_rate) /
                                                 SY2data.Where(x => x.building_id.Trim() == building.Trim() && x.machine_id.Trim()
                                == machine_type.Trim()).Count();
                            }
                            else
                            {
                                value = data.Where(x => x.factory_id.Trim() == factory.Trim()
                               && x.building_id.Trim() == building.Trim() && x.machine_id.Trim()
                                == machine_type.Trim()).Count() == 0 ? 0 :
                                  data.Where(x => x.factory_id.Trim() == factory.Trim()
                                 && x.building_id.Trim() == building.Trim() && x.machine_id.Trim()
                                == machine_type.Trim())
                                .Sum(z => z.oee_rate) /
                                data.Where(x => x.factory_id.Trim() == factory.Trim()
                                && x.building_id.Trim() == building.Trim() && x.machine_id.Trim()
                                == machine_type.Trim()).Count();
                            }
                            model.Add(machine_type, (int)value);
                }
            }
            return model;
        }
    }
}