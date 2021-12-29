module.exports = {
  title: "Lavcode",
  description: "Lavcode 开源密码管理",
  head: [["link", { rel: "icon", href: "/logo.png" }]],
  markdown: {
    lineNumbers: true,
  },
  base: "/",
  serviceWorker: true,
  themeConfig: {
    logo: "/logo.png",
    lastUpdated: "lastUpdate",
    sidebarDepth: 2,
    repo: "hal-wang/Lavcode",
    docsBranch: "main",
    editLinks: true,
    nav: [
      { text: "首页", link: "/" },
      {
        text: "使用帮助",
        items: [
          { text: "快速开始", link: "/usage/" },
          { text: "Svg图标", link: "/usage/svgicon/" },
        ],
      },
      {
        text: "下载",
        ariaLabel: "下载",
        items: [
          {
            text: "Windows 10",
            link: "https://www.microsoft.com/store/apps/9N3N7R34ZJDC",
          },
        ],
      },
      { text: "捐赠", link: "/donate/" },
    ],
    sidebar: {
      "/usage/": ["", "svgicon/"],
      "/pp/": ["en/", "zh/"],
      "/pages/pp/": ["en/", "zh/"], // 已过时，存在是为了兼容 uwp 0.1.0 版本和之前
    },
  },
};
