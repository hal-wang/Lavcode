import { FolderEntity } from "../../../entities/folder.entity";
import { IconEntity } from "../../../entities/icon.entity";
import { GetIconDto } from "../../../dtos/get-icon.dto";
import { V } from "@halsp/validator";

export class GetFolderDto {
  @V().Required()
  id!: string;
  @V().Required()
  name!: string;
  @V().Required()
  order!: number;
  @V().Required()
  updatedAt!: number;

  icon?: GetIconDto;

  public static fromEntity(entity: FolderEntity, icon?: IconEntity) {
    const result = new GetFolderDto();
    result.id = entity._id;
    result.name = entity.name;
    result.order = entity.order;
    result.updatedAt = entity.updatedAt;
    result.icon = icon ? GetIconDto.fromEntity(icon) : undefined;
    return result;
  }
}
