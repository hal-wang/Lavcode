import { defaultTheme, defineUserConfig } from "vuepress";

const usagePages = [
  {
    text: "新手指南",
    children: ["start.md", "password.md", "problem.md"],
  },
  {
    text: "基础",
    children: ["safety.md", "sync.md", "window.md", "svgicon.md"],
  },
  {
    text: "进阶",
    children: ["command.md", "contribute.md", "api.md"],
  },
];

const navbarUsagePages = usagePages.map((item) =>
  Object.assign({}, item, {
    children: item.children.map((item) => `/usage/${item}`),
  })
);

export default defineUserConfig({
  lang: "zh-CN",
  title: "Lavcode",
  description: "Lavcode 开源密码管理",
  head: [["link", { rel: "icon", href: "/logo.png" }]],
  base: "/",
  theme: defaultTheme({
    repo: "https://github.com/hal-wang/Lavcode",
    docsRepo: "hal-wang/Lavcode",
    docsBranch: "main",
    docsDir: "docs",
    editLinkPattern: ":repo/edit/:branch/lavcode-docs/:path",
    editLink: true,
    editLinkText: "编辑此页",
    logo: "/logo.png",
    home: "/index.md",
    sidebarDepth: 2,
    backToHome: "返回主页",
    notFound: ["你访问的页面不存在"],
    navbar: [
      { text: "首页", link: "/index.md" },
      ...navbarUsagePages,
      {
        text: "下载",
        ariaLabel: "下载",
        children: [
          {
            text: "Windows 10/11",
            link: "https://www.microsoft.com/store/apps/9N3N7R34ZJDC",
          },
        ],
      },
      { text: "捐赠", link: "/donate.md" },
    ],
    sidebar: {
      "/usage/": usagePages,
      "/pp/": ["en.md", "zh.md"],
    },
  }),
});
