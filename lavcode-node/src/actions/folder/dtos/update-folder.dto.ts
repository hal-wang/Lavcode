import { DtoDescription, DtoRequired } from "@ipare/swagger";
import { IsNumber, IsString } from "class-validator";
import { UpsertIconDto } from "../../../dtos/upsert-icon.dto";

export class UpdateFolderDto {
  @DtoRequired()
  @IsString()
  name!: string;

  @DtoRequired()
  @IsNumber()
  order!: number;

  @DtoDescription("空则不修改图标")
  icon!: UpsertIconDto;
}
