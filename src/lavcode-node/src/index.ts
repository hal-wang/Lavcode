import { LambdaStartup } from "@ipare/lambda";
import startup from "./startup";

const app = startup(new LambdaStartup(), "production");
export const main = (event: any, context: any) => app.run(event, context);
