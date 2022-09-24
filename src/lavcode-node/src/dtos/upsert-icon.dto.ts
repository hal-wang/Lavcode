import { V } from "@ipare/validator";
import { IconType } from "../enums/icon-type";

export class UpsertIconDto {
  @V().Required()
  iconType!: IconType;

  @V().Required()
  value!: string;
}
