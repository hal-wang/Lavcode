import { V } from "@halsp/validator";
import { UpsertIconDto } from "../../../dtos/upsert-icon.dto";

export class UpdateFolderDto {
  @V().Required().IsString()
  name!: string;

  @V().Required().IsNumber()
  order!: number;

  @V().IsOptional().Description("空则不修改图标")
  icon!: UpsertIconDto;
}
