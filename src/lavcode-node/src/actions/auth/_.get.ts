import { ILogger } from "@halsp/core";
import { Inject } from "@halsp/inject";
import { Query } from "@halsp/pipe";
import { Action } from "@halsp/router";
import { Logger } from "@halsp/logger";
import { Open } from "../../decorators/open.decorator";
import { JwtService } from "@halsp/jwt";
import { V } from "@halsp/validator";
import { GetTokenDto } from "./dtos/get-token.dto";

@Open
@V()
  .Tags("auth")
  .Summary("Get login token")
  .Response(200, GetTokenDto)
  .ResponseDescription(200, "success")
export default class extends Action {
  @Inject
  private readonly jwtService!: JwtService;

  @V().IsString().IsBase64().Description("Lavcode password")
  @Query("password")
  private readonly password!: string;

  async invoke() {
    if (
      Buffer.from(this.password, "base64").toString("utf-8") !=
      process.env.SECRET_KEY
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
