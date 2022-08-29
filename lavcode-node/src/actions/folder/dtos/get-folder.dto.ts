import { FolderEntity } from "../../../entities/folder.entity";
import { IconEntity } from "../../../entities/icon.entity";
import { GetIconDto } from "../../../dtos/get-icon.dto";

export class GetFolderDto {
  id!: string;
  name!: string;
  order!: number;
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
