module.exports = {
    title: 'Lavcode', // 显示在左上角的网页名称以及首页在浏览器标签显示的title名称
    description: 'Lavcode 开源密码管理', // meta 中的描述文字，用于SEO
    head: [
        ['link', { rel: 'icon', href: '/logo.png' }],  //浏览器的标签栏的网页图标
    ],
    markdown: {
        lineNumbers: true
    },
    base: '/',
    serviceWorker: true,
    themeConfig: {
        logo: '/logo.png',
        sidebarDepth: 2,
        lastUpdated: 'lastUpdate', // string | boolean
        nav: [
            { text: '首页', link: '/' },
            { text: '介绍', link: '/pages/start/' },
            { text: '捐赠', link: '/pages/donate' },
            {
                text: '下载',
                ariaLabel: '下载',
                items: [
                    { text: 'Windows 10', link: 'https://www.microsoft.com/store/apps/9N3N7R34ZJDC' }
                ]
            },
            {
                text: '使用帮助',
                ariaLabel: '使用帮助',
                items: [
                    { text: '简单使用', link: '/pages/usage/start/' },
                    { text: '云同步', link: '/pages/usage/sync/' },
                    { text: 'Svg图标', link: '/pages/usage/svgicon/' },
                ]
            },
            {
                text: '源码说明',
                ariaLabel: '源码说明',
                items: [
                    { text: '开始运行', link: '/pages/dev/start/' },
                    { text: '页面整体布局', link: '/pages/dev/part/' },
                    { text: '模块化组件', link: '/pages/dev/components/' },
                    { text: '自定义控件', link: '/pages/dev/controls/' },
                    { text: '页面', link: '/pages/dev/pages/' }
                ]
            },
            { text: 'GitHub', link: 'https://github.com/hbrwang/Lavcode' },
        ],
        sidebar: {
            '/pages/start/': [
                ''
            ],
            '/pages/pp/zh/': [
                ''
            ],
            '/pages/pp/en/': [
                ''
            ],
            '/pages/usage/': [
                'start/',
                'sync/',
                'svgicon/'
            ],
            '/pages/dev/': [
                'start/',
                'part/',
                'components/',
                'controls/',
                'pages/',
            ]
        }
    }
}