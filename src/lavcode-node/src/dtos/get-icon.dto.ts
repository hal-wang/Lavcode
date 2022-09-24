import { V } from "@ipare/validator";
import { IconEntity } from "../entities/icon.entity";
import { IconType } from "../enums/icon-type";

export class GetIconDto {
  @V().Required()
  id!: string;
  @V().Required()
  iconType!: IconType;
  @V().Required()
  value!: string;

  public static fromEntity(entity: IconEntity) {
    const result = new GetIconDto();
    result.id = entity._id;
    result.iconType = entity.iconType;
    result.value = entity.value;
    return result;
  }
}
