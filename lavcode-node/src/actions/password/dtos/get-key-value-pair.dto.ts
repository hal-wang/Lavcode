import { KeyValuePairEntity } from "../../../entities/key-value-pair.entity";

export class GetKeyValuePairDto {
  id!: string;
  passwordId!: string;
  key!: string;
  value!: string;

  public static fromEntity(entity: KeyValuePairEntity) {
    const result = new GetKeyValuePairDto();
    result.id = entity._id;
    result.passwordId = entity.passwordId;
    result.key = entity.key;
    result.value = entity.value;
    return result;
  }
}
