import { DtoRequired } from "@ipare/swagger";
import { IsNotEmpty } from "class-validator";
import { IconType } from "../enums/icon-type";

export class UpsertIconDto {
  @DtoRequired()
  @IsNotEmpty()
  iconType!: IconType;

  @DtoRequired()
  @IsNotEmpty()
  value!: string;
}
