import { V } from "@ipare/validator";

export class GetTokenDto {
  @V().Required()
  token!: string;
}
