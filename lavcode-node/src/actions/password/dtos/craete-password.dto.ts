import { UpsertIconDto } from "../../../dtos/upsert-icon.dto";
import { UpsertKeyValuePair } from "./upsert-key-value-pair.dto";

export class CreatePasswordDto {
  folderId!: string;
  title!: string;
  value!: string;
  remark!: string;

  icon!: UpsertIconDto;
  keyValuePairs!: UpsertKeyValuePair[];
}
