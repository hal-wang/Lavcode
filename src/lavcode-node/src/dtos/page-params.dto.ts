import { V } from "@halsp/validator";

@V().Description("Page params")
export class PageParamsDto {
  @V().Description("Page index").IsNumber().IsOptional()
  readonly page?: number;

  @V().Description("Per page limit").IsNumber().IsOptional()
  readonly limit?: number;
}
