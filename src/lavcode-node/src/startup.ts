import { HttpStartup } from "@halsp/http";
import "@halsp/router";
import "@halsp/swagger";
import "@halsp/inject";
import "@halsp/jwt";
import "@halsp/validator";
import "@halsp/env";
import "@halsp/logger";
import "@halsp/filter";
import { getVersion } from "@halsp/env";
import { AuthFilter } from "./filters/auth.filter";
import { CollectionService } from "./services/collection.service";
import { InjectType } from "@halsp/inject";
import { DbhelperService } from "./services/dbhelper.service";
import { CbappService } from "./services/cbapp.service";

export default <T extends HttpStartup>(startup: T) =>
  startup
    .use(async (ctx, next) => {
      ctx.res.set("version", (await getVersion()) ?? "");
      await next();
    })
    .useEnv()
    .useInject()
    .inject(CollectionService, InjectType.Singleton)
    .inject(DbhelperService, InjectType.Singleton)
    .inject(CbappService, InjectType.Singleton)
    .useConsoleLogger()
    .use(async (ctx, next) => {
      const logger = await ctx.getLogger();
      if (ctx.lambdaEvent) {
        logger.info("event: " + JSON.stringify(ctx.lambdaEvent));
      }
      if (ctx.lambdaContext) {
        logger.info("context: " + JSON.stringify(ctx.lambdaContext));
      }
      await next();
    })
    .useSwagger({
      path: "",
      basePath: process.env.NODE_ENV == "production" ? "v1" : "",
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
      secret: process.env.SECRET_KEY,
    })
    .useRouterParser()
    .useJwtVerify(
      (ctx) => {
        if (!ctx.actionMetadata || ctx.actionMetadata.open) {
          return true;
        }
        return false;
      },
      async (ctx, err) => {
        const logger = await ctx.getLogger();
        logger.error("Token 无效，" + err.message);

        ctx.res.unauthorizedMsg("请注销并重新登录");
      }
    )
    .useGlobalFilter(AuthFilter)
    .useRouter();
