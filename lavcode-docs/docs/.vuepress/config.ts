import { defaultTheme, defineUserConfig } from "vuepress";
import * as path from "path";

export default defineUserConfig({
  lang: "zh-CN",
  title: "Lavcode",
  description: "Lavcode 开源密码管理",
  head: [["link", { rel: "icon", href: "/logo.png" }]],
  base: "/",
  theme: defaultTheme({
    docsRepo: "https://github.com/hal-wang/Lavcode",
    docsBranch: "main",
    docsDir: "docs",
    editLinkPattern: ":repo/edit/:branch/:path",
    editLink: true,
    editLinkText: "编辑此页",
    logo: "/logo.png",
    home: "/index.md",
    sidebarDepth: 2,
    backToHome: "返回主页",
    notFound: ["你访问的页面不存在"],
    navbar: [
      { text: "首页", link: "/index.md" },
      {
        text: "使用帮助",
        children: [
          { text: "快速开始", link: "/usage/start.md" },
          { text: "Svg图标", link: "/usage/svgicon.md" },
        ],
      },
      {
        text: "下载",
        ariaLabel: "下载",
        children: [
          {
            text: "Windows 10",
            link: "https://www.microsoft.com/store/apps/9N3N7R34ZJDC",
          },
        ],
      },
      { text: "捐赠", link: "/donate.md" },
    ],
    sidebar: {
      "/usage/": ["start.md", "svgicon.md"],
      "/pp/": ["en.md", "zh.md"],
    },
  }),
});
