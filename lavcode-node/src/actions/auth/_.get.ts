import { Inject } from "@ipare/inject";
import { Header, Query } from "@ipare/pipe";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiTags,
  DtoDescription,
  DtoLengthRange,
  DtoRequired,
} from "@ipare/swagger";
import { Logger, LoggerInject } from "@ipare/logger";
import { IsString, IsNumberString, IsBase64 } from "class-validator";
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
  @DtoRequired()
  @DtoDescription("Lavcode password")
  @Query("password")
  private readonly password!: string;

  async invoke() {
    console.log(
      "password",
      process.env.PASSWORD,
      this.password,
      Buffer.from(this.password, "base64").toString("utf-8")
    );
    if (
      Buffer.from(this.password, "base64").toString("utf-8") !=
      process.env.PASSWORD
    ) {
      this.unauthorizedMsg("密码错误");
      return;
    }

    const token = await this.jwtService.sign({
      from: "lavcode",
    });
    this.logger.info("create token");
    this.ok({
      token: token,
    });
  }
}
