import { V } from "@halsp/validator";

export class UpsertKeyValuePair {
  @V().Required()
  key!: string;
  @V().Required()
  value!: string;
}
