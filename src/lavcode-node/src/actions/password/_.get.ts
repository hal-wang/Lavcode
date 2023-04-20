import { Inject } from "@halsp/inject";
import { Query } from "@halsp/pipe";
import { Action } from "@halsp/router";
import { V } from "@halsp/validator";
import { GetPasswordDto } from "./dtos/get-password.dto";
import { PasswordService } from "./services/password.service";

@V()
  .Tags("password")
  .Summary("Get passwords")
  .Response(200, [GetPasswordDto])
  .ResponseDescription(200, "success")
  .Security({
    Bearer: [],
  })
export default class extends Action {
  @Inject
  private readonly passwordService!: PasswordService;

  @Query("folderId")
  @V().Description("Empty value for all passwords")
  private readonly folderId!: string;

  async invoke() {
    const passwords = await this.passwordService.getPasswords(
      this.folderId
        ? {
            folderId: this.folderId,
          }
        : undefined
    );
    this.ok(passwords);
  }
}
