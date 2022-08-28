import { Startup } from "@ipare/core";
import "@ipare/router";
import "@ipare/swagger";
import "@ipare/inject";
import "@ipare/jwt";
import "@ipare/validator";
import "@ipare/env";
import "@ipare/logger";
import "@ipare/filter";
import { getVersion } from "@ipare/env";
import { AuthFilter } from "./filters/auth.filter";
import { CollectionService } from "./services/collection.service";
import { InjectType } from "@ipare/inject";
import { DbhelperService } from "./services/dbhelper.service";
import { CbappService } from "./services/cbapp.service";

export default <T extends Startup>(startup: T, mode: string) =>
  startup
    .useVersion()
    .useEnv({ mode })
    .useInject()
    .inject(CollectionService, InjectType.Singleton)
    .inject(DbhelperService, InjectType.Singleton)
    .inject(CbappService, InjectType.Singleton)
    .useConsoleLogger()
    .use(async (ctx, next) => {
      const logger = await ctx.getLogger();
      logger.info("event: " + JSON.stringify(ctx.lambdaEvent));
      logger.info("context: " + JSON.stringify(ctx.lambdaContext));
      await next();
    })
    .useSwagger({
      builder: async (builder) =>
        builder
          .addInfo({
            title: "Lavcode",
            description: "Lavcode Api",
            version: (await getVersion()) ?? "",
            license: {
              name: "MIT",
            },
            contact: {
              email: "support@hal.wang",
            },
          })
          .addServer({
            url: "/",
          })
          .addOpenApiVersion("3.0.0")
          .addSecurityScheme("Bearer", {
            type: "http",
            description:
              'JWT Authorization header using the Bearer scheme. Example: "Authorization: Bearer {token}"',
            scheme: "Bearer",
            bearerFormat: "JWT",
          }),
    })
    .useValidator()
    .useJwt({
      secret: process.env.PASSWORD,
    })
    .useRouterParser()
    .useJwtVerify(
      (ctx) => {
        if (!ctx.actionMetadata || ctx.actionMetadata.open) {
          return true;
        }
        return false;
      },
      (ctx, err) => {
        if (!ctx.jwtToken) {
          ctx.unauthorizedMsg("Please login");
        } else {
          ctx.unauthorizedMsg(err.message);
        }
      }
    )
    .useGlobalFilter(AuthFilter)
    .useRouter();
