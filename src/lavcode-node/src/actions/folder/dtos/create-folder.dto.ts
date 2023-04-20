import { V } from "@halsp/validator";
import { UpsertIconDto } from "../../../dtos/upsert-icon.dto";

export class CreateFolderDto {
  @V().Required().IsString()
  name!: string;

  @V().Required()
  icon!: UpsertIconDto;
}
