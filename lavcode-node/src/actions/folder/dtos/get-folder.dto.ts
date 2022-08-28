import { FolderEntity } from "../../../entities/folder.entity";
import { IconEntity } from "../../../entities/icon.entity";
import { GetIconDto } from "../../icon/dtos/get-icon.dto";

export class GetFolderDto {
  id!: string;
  name!: string;
  order!: number;
  lastEditTime!: number;

  icon!: GetIconDto;

  public static fromEntity(entity: FolderEntity, icon: IconEntity) {
    const result = new GetFolderDto();
    result.id = entity._id;
    result.name = entity.name;
    result.order = entity.order;
    result.lastEditTime = entity.lastEditTime;
    result.icon = icon ? GetIconDto.fromEntity(icon) : new GetIconDto();
    return result;
  }
}
