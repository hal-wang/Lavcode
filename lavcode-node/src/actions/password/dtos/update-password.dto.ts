import { V } from "@ipare/validator";
import { UpsertIconDto } from "../../../dtos/upsert-icon.dto";
import { UpsertKeyValuePair } from "./upsert-key-value-pair.dto";

export class UpdatePasswordDto {
  @V().Required()
  folderId!: string;
  @V().Required()
  title!: string;
  @V().Required()
  value!: string;
  @V().Required()
  remark!: string;
  @V().Required()
  order!: string;

  @V().Required()
  icon!: UpsertIconDto;
  @V().Required().Items(UpsertKeyValuePair)
  keyValuePairs!: UpsertKeyValuePair[];
}
