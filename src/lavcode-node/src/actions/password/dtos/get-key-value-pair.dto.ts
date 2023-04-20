import { V } from "@halsp/validator";
import { KeyValuePairEntity } from "../../../entities/key-value-pair.entity";

export class GetKeyValuePairDto {
  @V().Required()
  id!: string;
  @V().Required()
  passwordId!: string;
  @V().Required()
  key!: string;
  @V().Required()
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
