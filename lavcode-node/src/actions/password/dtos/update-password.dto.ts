import { UpsertIconDto } from "../../../dtos/upsert-icon.dto";
import { UpsertKeyValuePair } from "./upsert-key-value-pair.dto";

export class UpdatePasswordDto {
  folderId!: string;
  title!: string;
  value!: string;
  remark!: string;
  order!: string;

  icon!: UpsertIconDto;
  keyValuePairs!: UpsertKeyValuePair[];
}
