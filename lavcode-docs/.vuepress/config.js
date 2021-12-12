module.exports = {
  title: "Lavcode", // 显示在左上角的网页名称以及首页在浏览器标签显示的title名称
  description: "Lavcode 开源密码管理", // meta 中的描述文字，用于SEO
  head: [
    ["link", { rel: "icon", href: "/logo.png" }], //浏览器的标签栏的网页图标
  ],
  markdown: {
    lineNumbers: true,
  },
  base: "/",
  serviceWorker: true,
  themeConfig: {
    logo: "/logo.png",
    lastUpdated: "lastUpdate", // string | boolean
    sidebarDepth: 2,
    repo: "hal-wang/Lavcode",
    // 可选，默认为 master
    docsBranch: "master",
    // 默认为 true，设置为 false 来禁用
    editLinks: true,
    nav: [
      { text: "首页", link: "/" },
      { text: "捐赠", link: "/donate/" },
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
    ],
    sidebar: {
      "/usage/": ["", "svgicon/"],
      "/pp/": ["en/", "zh/"],
      "/pages/pp/": ["en/", "zh/"], // 已过时，存在是为了兼容 uwp 0.1.0 版本和之前
    },
  },
};
