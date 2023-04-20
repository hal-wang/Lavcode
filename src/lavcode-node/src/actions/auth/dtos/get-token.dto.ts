import { V } from "@halsp/validator";

export class GetTokenDto {
  @V().Required()
  token!: string;
}
