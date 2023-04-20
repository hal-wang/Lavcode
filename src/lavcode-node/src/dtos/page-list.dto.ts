import { V } from "@halsp/validator";
import { PageParamsDto } from "./page-params.dto";

export class PageListDto<T> extends PageParamsDto {
  @V().IsOptional()
  list!: T[];
  @V().IsNumber().IsOptional()
  total: number | undefined;
}
