namespace OEE_API.Dtos
{
    public class M_MachineInformation_Dto
    {
        public string factory_id { get; set; }
        public string building_id { get; set; }
        public string machine_id { get; set; }
        public string machine_name { get; set; }
        public string machine_model { get; set; }
        public int machine_type { get; set; }
        public string line_id { get; set; }
        public bool is_active { get; set; }
    }
}