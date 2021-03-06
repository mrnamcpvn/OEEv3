export interface Pagination {
    currentPage: number;
    totalPage: number;
    pageSize: number;
    totalCount: number;
}

export class PaginationResult<T> {
    result: T;
    resultC: T;
    machineName: string;
    pagination: Pagination;
}