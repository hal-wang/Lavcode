<template><div><h1 id="快速开始" tabindex="-1"><a class="header-anchor" href="#快速开始" aria-hidden="true">#</a> 快速开始</h1>
<h2 id="密码管理" tabindex="-1"><a class="header-anchor" href="#密码管理" aria-hidden="true">#</a> 密码管理</h2>
<h3 id="密码数量" tabindex="-1"><a class="header-anchor" href="#密码数量" aria-hidden="true">#</a> 密码数量</h3>
<p>理论上支持无限多的文件夹，每个文件夹支持无限多的密码，每条密码支持无限多的键值内容。</p>
<div class="custom-container tip"><p class="custom-container-title">TIP</p>
<p>如果你的密码数以千计（正常人都没有吧），可以分为多个密码文件存放。</p>
</div>
<h3 id="排序" tabindex="-1"><a class="header-anchor" href="#排序" aria-hidden="true">#</a> 排序</h3>
<p>文件夹、密码都可拖拽进行排序。把最经常用的密码拖到前面来吧！</p>
<h3 id="其他操作" tabindex="-1"><a class="header-anchor" href="#其他操作" aria-hidden="true">#</a> 其他操作</h3>
<p>可将密码单个或批量，移动至另一个文件夹。</p>
<h3 id="图标" tabindex="-1"><a class="header-anchor" href="#图标" aria-hidden="true">#</a> 图标</h3>
<p>每个文件或密码都支持添加图标，可帮助你快速找到目标。</p>
<p>图标不仅支持字体，还支持图片，更支持强大丰富的 svg 路径图。</p>
<p><a href="./svgicon">如何使用 svg 图标？</a></p>
<div class="custom-container tip"><p class="custom-container-title">TIP</p>
<p>建议使用内置字体或 svg 图，在 <a href="https://www.iconfont.cn/collections" target="_blank" rel="noopener noreferrer">iconfont<ExternalLinkIcon/></a> 有海量免费图标。</p>
</div>
<div class="custom-container danger"><p class="custom-container-title">DANGER</p>
<p>非常不建议使用图片，图片会使密码文件变得很臃肿，影响同步速度和操作速度。</p>
</div>
<h3 id="复杂密码生成器" tabindex="-1"><a class="header-anchor" href="#复杂密码生成器" aria-hidden="true">#</a> 复杂密码生成器</h3>
<p>如果你的记忆不太强，应该是每个网站密码都相同，但如果有个网站的密码泄露，别人将可以登录你其他账号。</p>
<p>因此安全的做法是尽可能有多个不同的密码。防止相同密码泄露。</p>
<p>Lavcode 即可以帮你记密码，又可以帮你“创造”密码。</p>
<p>在新建密码记录时，可以生成复杂密码，再使用此密码注册网站。安全又省心。</p>
<div class="custom-container tip"><p class="custom-container-title">TIP</p>
<p>生成密码时可以配置大小写字母、数字、特殊字符</p>
</div>
<h2 id="安全性" tabindex="-1"><a class="header-anchor" href="#安全性" aria-hidden="true">#</a> 安全性</h2>
<p>对于一款密码管理软件来说，安全性是首要的。使用<code v-pre>Lavcode</code>，你管理的密码不用担心被他人获取，你的密码也不会丢失。</p>
<h3 id="开源安全" tabindex="-1"><a class="header-anchor" href="#开源安全" aria-hidden="true">#</a> 开源安全</h3>
<p>Lavcode 是开源软件，所有代码公开透明。因此你无需担心密码有被窃取的危险。</p>
<div class="custom-container tip"><p class="custom-container-title">TIP</p>
<p>源码请访问 <a href="https://github.com/hal-wang/Lavcode" target="_blank" rel="noopener noreferrer">GitHub<ExternalLinkIcon/></a> 或 <a href="https://gitee.com/hal-wang/Lavcode" target="_blank" rel="noopener noreferrer">Gitee<ExternalLinkIcon/></a></p>
</div>
<h3 id="备份安全" tabindex="-1"><a class="header-anchor" href="#备份安全" aria-hidden="true">#</a> 备份安全</h3>
<p>Lavcode 支持以下备份方式：</p>
<ol>
<li>GitHub/Gitee 存储</li>
<li>WebDav 云同步</li>
<li>文件同步</li>
</ol>
<p>详情看下面<a href="#%E5%A4%87%E4%BB%BD">备份</a>部分。</p>
<h3 id="网络安全" tabindex="-1"><a class="header-anchor" href="#网络安全" aria-hidden="true">#</a> 网络安全</h3>
<p>虽然 Lavcode 有联网权限，但只用于以下几处：</p>
<ol>
<li>获取 GitHub 相关的源码、评论、通知信息等时</li>
<li>使用 GitHub/Gitee 存储时</li>
<li>WebDav 同步时（仅与第三方网盘通讯，使用哪个网盘由你自定义）</li>
<li>从应用商店获取应用信息时，如检查更新、评论等</li>
</ol>
<div class="custom-container tip"><p class="custom-container-title">TIP</p>
<p>如果你懂网络监听技术，会发现<code v-pre>Lavcode</code>除以上几条外，完全无任何网络访问行为。</p>
</div>
<h2 id="界面" tabindex="-1"><a class="header-anchor" href="#界面" aria-hidden="true">#</a> 界面</h2>
<p>主题色跟随系统，美观多彩的密码管理有木有！！！</p>
<p>上几张截图</p>
<p><img src="/screenshots/1.png" alt="截图1">
<img src="/screenshots/2.png" alt="截图2">
<img src="/screenshots/3.png" alt="截图3">
<img src="/screenshots/4.png" alt="截图4"></p>
<h2 id="备份" tabindex="-1"><a class="header-anchor" href="#备份" aria-hidden="true">#</a> 备份</h2>
<h3 id="github-gitee-存储" tabindex="-1"><a class="header-anchor" href="#github-gitee-存储" aria-hidden="true">#</a> GitHub/Gitee 存储</h3>
<p>使用 <code v-pre>OAuth2</code> 登录，首次使用 Lavcode 会为你创建一个名为 <code v-pre>LavcodeStorage</code> 的仓库</p>
<h3 id="webdav-云同步" tabindex="-1"><a class="header-anchor" href="#webdav-云同步" aria-hidden="true">#</a> WebDav 云同步</h3>
<p>WebDAV 的 <a href="https://baike.baidu.com/item/WebDAV" target="_blank" rel="noopener noreferrer">百度百科<ExternalLinkIcon/></a> 介绍</p>
<p>可自由选择第三方支持 WebDav 的网盘，同步文件经过你的密码加密，即使把文件发给别人，别人也看不到内容。</p>
<h4 id="可使用什么网盘" tabindex="-1"><a class="header-anchor" href="#可使用什么网盘" aria-hidden="true">#</a> 可使用什么网盘？</h4>
<p>只要是支持 WebDAV 的网盘都支持同步，国内推荐坚果云</p>
<h4 id="如何获取账号" tabindex="-1"><a class="header-anchor" href="#如何获取账号" aria-hidden="true">#</a> 如何获取账号?</h4>
<p>以坚果云为例，在 <a href="https://www.jianguoyun.com/" target="_blank" rel="noopener noreferrer">坚果云官网<ExternalLinkIcon/></a> 注册账号即可</p>
<h4 id="使用费用" tabindex="-1"><a class="header-anchor" href="#使用费用" aria-hidden="true">#</a> 使用费用</h4>
<p>本软件完全免费无广告，无附加收费产品。可能某些网盘会收费，但与本软件无关</p>
<h3 id="文件同步" tabindex="-1"><a class="header-anchor" href="#文件同步" aria-hidden="true">#</a> 文件同步</h3>
<p>不仅可以同步到云，可以把本地文件当作同步介质。导出文件后放在硬盘、U 盘，可随时同步，可随时查看、编辑。</p>
<p>当然，文件也是经过你输入的密码加密的，即使文件被别人弄走，也看不了。</p>
<p>文件不仅支持更新同步，还能直接打开查看和修改。（可以把一个文件当成一个密码箱。）</p>
<div class="custom-container tip"><p class="custom-container-title">进阶操作</p>
<p>你可以把备份文件放到 OneDrive 或其他同步文件夹，每次编辑保存后会自动云同步。</p>
<p>此方法类似于 OneNote 。</p>
</div>
<h3 id="加密方式" tabindex="-1"><a class="header-anchor" href="#加密方式" aria-hidden="true">#</a> 加密方式</h3>
<p>WebDav 云同步和文件同步，备份文件一律使用<code v-pre>AES256</code>加密。</p>
<div class="custom-container tip"><p class="custom-container-title">TIP</p>
<p><code v-pre>AES256</code>也叫<code v-pre>高级加密标准</code>，目前无法破解。</p>
</div>
<h3 id="与云端-或文件-合并" tabindex="-1"><a class="header-anchor" href="#与云端-或文件-合并" aria-hidden="true">#</a> 与云端（或文件）合并</h3>
<p>同步方式不止是傻傻的覆盖，同时也支持智能合并。</p>
<p>你再也不用担心几台电脑，同步来同步去把内容搞丢了。</p>
<p>只能合并大体按以下逻辑进行：</p>
<ol>
<li>删除曾经删除过的记录。不管是在备份介质中删除，还是在本地被删除，合并后该文件夹或密码都不会存在。</li>
<li>找到二者都存在的文件夹或密码，根据最后编辑日期，选择较晚的作为最终结果。</li>
<li>找到二者不同时存在的文件夹或密码，添加至最终结果。</li>
<li>合并后，本地和云端（或文件）都会更新，本地先前的内容会备份至“历史记录”，可随时导出或删除。</li>
</ol>
<h3 id="云端-或文件-覆盖本地" tabindex="-1"><a class="header-anchor" href="#云端-或文件-覆盖本地" aria-hidden="true">#</a> 云端（或文件）覆盖本地</h3>
<p>以云端（或文件）为准，把云端（或文件）内容全部下载本地，不保留原先本地内容。</p>
<p>此方法适合本地无数据，或本地数据不重要的情况。</p>
<h3 id="本地覆盖云端-或文件" tabindex="-1"><a class="header-anchor" href="#本地覆盖云端-或文件" aria-hidden="true">#</a> 本地覆盖云端（或文件）</h3>
<p>以本地为准，把本地内容全部上传云端（或文件），不保留原先云端（或文件）内容。</p>
<p>此方法适合未备份至云端（或文件），或云端（或文件）数据不重要的情况。</p>
<h3 id="同步密码设置" tabindex="-1"><a class="header-anchor" href="#同步密码设置" aria-hidden="true">#</a> 同步密码设置</h3>
<p>同步密码用于加密备份文件</p>
<ul>
<li>如果是云同步，则加密云端文件</li>
<li>如果是文件同步，则加密同步文件</li>
</ul>
<p>密码设置后会缓存在软件中。同步时，或打开同步文件，如果缓存密码正确则无需再次输入密码。缓存密码可以随时修改。</p>
<h3 id="历史记录" tabindex="-1"><a class="header-anchor" href="#历史记录" aria-hidden="true">#</a> 历史记录</h3>
<p>仅记录本地，不记录云端和同步文件。</p>
<p>即如果同步有可能改变本地数据，就会将本地原先的数据加入历史记录。</p>
<p>以下几种情况会产生历史记录：</p>
<ol>
<li>云端（或文件）覆盖本地。</li>
<li>与云端（或文件）合并</li>
</ol>
<p>历史记录可任意导出或删除。导出的历史记录，是一个同步文件，可随时还原。</p>
<h2 id="多窗口" tabindex="-1"><a class="header-anchor" href="#多窗口" aria-hidden="true">#</a> 多窗口</h2>
<p>只能打开一个密码文件怎么能忍？Lavcode 支持同时打开多个文件，即一个文件对应一个应用窗口。</p>
<h3 id="多窗口联动" tabindex="-1"><a class="header-anchor" href="#多窗口联动" aria-hidden="true">#</a> 多窗口联动</h3>
<p>打开多个窗口后，还可以有更多骚操作</p>
<p>比如：有多个密码文件想复制密码怎么办？或把你男(女)朋友的密码复制过来！</p>
<p>简单！直接拖拽就能复制。在多选状态下，还能批量拖拽。</p>
<p>把密码从一个密码文件复制到另一个密码文件，或者从密码文件复制到软件，或从软件复制到密码文件，真是如此简单！</p>
<div class="custom-container tip"><p class="custom-container-title">TIP</p>
<p>当然，文件打开的前提是你有正确的密码</p>
</div>
<h2 id="用命令打开" tabindex="-1"><a class="header-anchor" href="#用命令打开" aria-hidden="true">#</a> 用命令打开</h2>
<p>可用命令快速打开 Lavcode，格式为 <code v-pre>lavcode</code> 或 <code v-pre>lavcode + 存储方式</code></p>
<ul>
<li>快速打开
<ul>
<li><code v-pre>lavcode</code></li>
</ul>
</li>
<li>GitHub
<ul>
<li><code v-pre>lavcode git</code></li>
<li><code v-pre>lavcode github</code></li>
</ul>
</li>
<li>Gitee
<ul>
<li><code v-pre>lavcode gitee</code></li>
<li><code v-pre>lavcode 码云</code></li>
</ul>
</li>
<li>本地
<ul>
<li><code v-pre>lavcode local</code></li>
<li><code v-pre>lavcode sqlite</code></li>
</ul>
</li>
</ul>
<h2 id="结尾" tabindex="-1"><a class="header-anchor" href="#结尾" aria-hidden="true">#</a> 结尾</h2>
<p>喜欢本软件就<a href="https://github.com/hal-wang/Lavcode" target="_blank" rel="noopener noreferrer">给个 Star 吧<ExternalLinkIcon/></a></p>
<p>如果可以的话，当然也希望能收到<a href="/pages/donate">捐赠</a></p>
<p>你的支持是 Lavcode 持续改进的最大动力</p>
</div></template>
