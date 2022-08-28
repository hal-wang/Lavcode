import { PageParamsDto } from "./page-params.dto";

export class PageListDto<T> extends PageParamsDto {
  list!: T[];
  total: number | undefined;
}
