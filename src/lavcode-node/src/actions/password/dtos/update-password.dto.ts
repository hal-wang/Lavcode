import { V } from "@halsp/validator";
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
  @V().IsNumber().Required()
  order!: number;

  @V().IsOptional()
  icon!: UpsertIconDto;
  @V().IsOptional().Items(UpsertKeyValuePair)
  keyValuePairs!: UpsertKeyValuePair[];
}
