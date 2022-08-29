import { KeyValuePairEntity } from "../../../entities/key-value-pair.entity";

export class GetKeyValuePairDto {
  id!: string;
  sourceId!: string;
  key!: string;
  value!: string;

  public static fromEntity(entity: KeyValuePairEntity) {
    const result = new GetKeyValuePairDto();
    result.id = entity._id;
    result.sourceId = entity.sourceId;
    result.key = entity.key;
    result.value = entity.value;
    return result;
  }
}
