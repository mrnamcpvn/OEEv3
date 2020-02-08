export interface ActionTime {
    id: number;
    factory_id: string;
    machine_id: string;
    building_id: string;
    shiftdate?: Date;
    date: string;
    start_time?: Date;
    end_time?: Date;
    duration: string;
    status_id: string;
    status: string;
    shift: string;
}
