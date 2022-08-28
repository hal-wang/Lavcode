import { DtoDescription } from "@ipare/swagger";

@DtoDescription("Page params")
export class PageParamsDto {
  @DtoDescription("Page index")
  readonly page?: number;

  @DtoDescription("Per page limit")
  readonly limit?: number;
}
