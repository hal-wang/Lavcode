import { DtoDescription, DtoRequired } from "@ipare/swagger";
import { IsString } from "class-validator";
import { UpsertIconDto } from "../../icon/dtos/upsert-icon.dto";

export class UpdateFolderDto {
  @DtoRequired()
  @IsString()
  name!: string;

  @DtoDescription("空则不修改图标")
  icon!: UpsertIconDto;
}
