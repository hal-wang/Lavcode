import { IconEntity } from "../entities/icon.entity";
import { IconType } from "../enums/icon-type";

export class GetIconDto {
  id!: string;
  iconType!: IconType;
  value!: string;

  public static fromEntity(entity: IconEntity) {
    const result = new GetIconDto();
    result.id = entity._id;
    result.iconType = entity.iconType;
    result.value = entity.value;
    return result;
  }
}
