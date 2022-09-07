import { V } from "@ipare/validator";

export class UpsertKeyValuePair {
  @V().Required()
  key!: string;
  @V().Required()
  value!: string;
}
