import { HttpContext } from "@ipare/core";
import { AuthorizationFilter } from "@ipare/filter";
import { Inject } from "@ipare/inject";
import "@ipare/jwt";
import { JwtService } from "@ipare/jwt";

export class AuthFilter implements AuthorizationFilter {
  @Inject
  private readonly jwtService!: JwtService;

  onAuthorization(ctx: HttpContext) {
    const isOpen = ctx.actionMetadata.open;
    if (isOpen) return true;

    const payload = this.jwtService.decode({
      json: true,
    });
    if (!payload) return false;

    return payload.from == "lavcode";
  }
}
