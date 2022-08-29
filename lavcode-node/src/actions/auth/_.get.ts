import { Inject } from "@ipare/inject";
import { Query } from "@ipare/pipe";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiTags,
  DtoDescription,
  DtoRequired,
} from "@ipare/swagger";
import { Logger, LoggerInject } from "@ipare/logger";
import { IsString, IsBase64 } from "class-validator";
import { Open } from "../../decorators/open.decorator";
import { JwtService } from "@ipare/jwt";

@Open
@ApiTags("auth")
@ApiDescription("Get login token")
@ApiResponses({
  "200": {
    description: "success",
  },
})
export default class extends Action {
  @Inject
  private readonly jwtService!: JwtService;
  @LoggerInject()
  private readonly logger!: Logger;

  @IsString()
  @IsBase64()
  @DtoDescription("Lavcode password")
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
