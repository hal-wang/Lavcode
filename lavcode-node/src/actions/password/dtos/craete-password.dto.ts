import { V } from "@ipare/validator";
import { UpsertIconDto } from "../../../dtos/upsert-icon.dto";
import { UpsertKeyValuePair } from "./upsert-key-value-pair.dto";

export class CreatePasswordDto {
  @V().Required()
  folderId!: string;
  @V().Required()
  title!: string;
  @V().Required()
  value!: string;
  @V().Required()
  remark!: string;

  @V().Required()
  icon!: UpsertIconDto;
  @V().Required().Items(UpsertKeyValuePair)
  keyValuePairs!: UpsertKeyValuePair[];
}
