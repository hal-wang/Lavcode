import { Inject } from "@ipare/inject";
import { Query } from "@ipare/pipe";
import { Action } from "@ipare/router";
import { Logger, LoggerInject } from "@ipare/logger";
import { Open } from "../../decorators/open.decorator";
import { JwtService } from "@ipare/jwt";
import { V } from "@ipare/validator";
import { GetTokenDto } from "./dtos/get-token.dto";

@Open
@V()
  .Tags("auth")
  .Description("Get login token")
  .Response(200, GetTokenDto)
  .ResponseDescription(200, "success")
export default class extends Action {
  @Inject
  private readonly jwtService!: JwtService;
  @LoggerInject()
  private readonly logger!: Logger;

  @V().IsString().IsBase64().Description("Lavcode password")
  @Query("password")
  private readonly password!: string;

  async invoke() {
    if (
      Buffer.from(this.password, "base64").toString("utf-8") !=
      process.env.PASSWORD
    ) {
      this.unauthorizedMsg("密码错误");
      return;
    }

    const token = await this.jwtService.sign(
      {
        from: "lavcode",
      },
      {
        expiresIn: "90d",
      }
    );
    this.logger.info("create token");
    this.ok({
      token: token,
    });
  }
}
