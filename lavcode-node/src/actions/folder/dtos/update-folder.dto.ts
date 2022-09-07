import { V } from "@ipare/validator";
import { UpsertIconDto } from "../../../dtos/upsert-icon.dto";

export class UpdateFolderDto {
  @V().Required().IsString()
  name!: string;

  @V().Required().IsNumber()
  order!: number;

  @V().Description("空则不修改图标")
  icon!: UpsertIconDto;
}
