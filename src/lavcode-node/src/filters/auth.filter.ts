import { Context } from "@halsp/core";
import { AuthorizationFilter } from "@halsp/filter";
import { Inject } from "@halsp/inject";
import { JwtService } from "@halsp/jwt";

export class AuthFilter implements AuthorizationFilter {
  @Inject
  private readonly jwtService!: JwtService;

  onAuthorization(ctx: Context) {
    const isOpen = ctx.actionMetadata.open;
    if (isOpen) return true;

    const payload = this.jwtService.decode({
      json: true,
    });
    if (!payload) return false;

    return payload.from == "lavcode";
  }
}
