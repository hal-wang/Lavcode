import { V } from "@ipare/validator";
import { UpsertIconDto } from "../../../dtos/upsert-icon.dto";
import { UpsertKeyValuePair } from "./upsert-key-value-pair.dto";

export class UpdatePasswordDto {
  @V().Required()
  folderId!: string;
  @V().IsOptional()
  title?: string;
  @V().IsOptional()
  value?: string;
  @V().IsOptional()
  remark?: string;
  @V().Required()
  order!: string;

  @V().Required()
  icon!: UpsertIconDto;
  @V().Required().Items(UpsertKeyValuePair)
  keyValuePairs!: UpsertKeyValuePair[];
}
