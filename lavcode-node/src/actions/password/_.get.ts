import { Inject } from "@ipare/inject";
import { Query } from "@ipare/pipe";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiSecurity,
  ApiTags,
  DtoDescription,
} from "@ipare/swagger";
import { PasswordService } from "./services/password.service";

@ApiTags("password")
@ApiDescription("Get passwords")
@ApiResponses({
  "200": {
    description: "success",
  },
})
@ApiSecurity({
  Bearer: [],
})
export default class extends Action {
  @Inject
  private readonly passwordService!: PasswordService;

  @Query("folderId")
  @DtoDescription("Empty value for all passwords")
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
