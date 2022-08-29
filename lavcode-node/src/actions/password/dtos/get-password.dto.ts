import { GetIconDto } from "../../../dtos/get-icon.dto";
import { IconEntity } from "../../../entities/icon.entity";
import { KeyValuePairEntity } from "../../../entities/key-value-pair.entity";
import { PasswordEntity } from "../../../entities/password-entity";
import { GetKeyValuePairDto } from "./get-key-value-pair.dto";

export class GetPasswordDto {
  id!: string;
  folderId!: string;
  title!: string;
  value!: string;
  remark!: string;
  order!: string;
  updatedAt!: number;

  icon?: GetIconDto;
  keyValuePairs?: GetKeyValuePairDto[];

  public static fromEntity(
    entity: PasswordEntity,
    icon?: IconEntity,
    keyValuePairs?: KeyValuePairEntity[]
  ) {
    const result = new GetPasswordDto();
    result.id = entity._id;
    result.folderId = entity.folderId;
    result.title = entity.title;
    result.value = entity.value;
    result.remark = entity.remark;
    result.order = entity.order;
    result.updatedAt = entity.updatedAt;
    result.icon = icon ? GetIconDto.fromEntity(icon) : undefined;
    result.keyValuePairs = keyValuePairs?.map((item) =>
      GetKeyValuePairDto.fromEntity(item)
    );
    return result;
  }
}
