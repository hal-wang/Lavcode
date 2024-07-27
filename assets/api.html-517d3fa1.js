import{_ as t,M as d,p as c,q as r,R as e,t as a,N as s,a1 as o}from"./framework-fb0bc66b.js";const l="/assets/mng-855b61b1.png",i="/assets/edit-c988b659.png",p="/assets/add-a00ea3c2.png",h="/assets/url-97cf8796.png",u="/assets/login-e889d9ea.png",v="/assets/secrets-b806b21c.png",_={},b=o('<h1 id="云接口" tabindex="-1"><a class="header-anchor" href="#云接口" aria-hidden="true">#</a> 云接口</h1><blockquote><p><strong>不想看介绍的，就直接转到后面 “部署接口” 部分</strong></p></blockquote><p>Lavcode 支持通过接口的方式存取数据</p><p>在选择 “数据存储位置” 界面，选择云接口</p><p>输入接口地址和密码即可使用</p><h2 id="后端接口方案" tabindex="-1"><a class="header-anchor" href="#后端接口方案" aria-hidden="true">#</a> 后端接口方案</h2><p>Lavcode 没有提供直接用的接口，因为大多数人都不愿意将密码存储到别人那里，因此你需要自行部署</p><p>源码提供两种方案实现后端接口。对于复杂需求，也可以仿照这两种方案实现自定义方案</p><ol><li>方案 1</li></ol>',9),g={href:"https://www.cloudbase.net/",target:"_blank",rel:"noopener noreferrer"},m={href:"https://ipare.org",target:"_blank",rel:"noopener noreferrer"},f=o('<p>源码目录 <code>src/lavcode-node</code></p><p>此方案使用比较简单，适合零开发经验的用户，稍微有些开发经验就更好了</p><ol start="2"><li>方案 2</li></ol><p>.NET6 + Asp + Sql Server</p><p>源码目录 <code>Lavcode.Asp</code></p><p>此方案使用起来稍复杂，需要一定的开发基础</p><h2 id="部署接口" tabindex="-1"><a class="header-anchor" href="#部署接口" aria-hidden="true">#</a> 部署接口</h2><p>此部分同样适用于零开发基础的普通用户，每个步骤都有图片示例</p><p>使用源码提供的 <code>src/lavcode-node</code> 快速部署接口至腾讯云 CloudBase</p><p>按以下步骤进行</p><ol><li>一键部署</li></ol>',11),E={href:"https://console.cloud.tencent.com/tcb/env/index?action=CreateAndDeployCloudBaseProject&appUrl=https%3A%2F%2Fgithub.com%2Fhal-wang%2FLavcode&branch=main&workDir=src/lavcode-node",target:"_blank",rel:"noopener noreferrer"},S=e("img",{src:"https://main.qcloudimg.com/raw/67f5a389f1ac6f3b4d04c7256438e44f.svg",alt:""},null,-1),k=o('<ol start="2"><li>设置环境变量</li></ol><p>设置环境变量步骤如下</p><p>(1) 在腾讯云 CloudBase 选择 “我的应用”，点击 <code>Lavcode</code> 右侧的 “管理”</p><p><img src="'+l+'" alt="管理"></p><p>(2) 点击编辑按钮，以打开编辑开关</p><p><img src="'+i+'" alt="编辑环境变量"></p><p>(3) 点击 “新建环境变量” 按钮，添加环境变量 <code>SECRET_KEY</code>，值作为云接口密码，然后点 “保存并部署”，等待部署完毕。请尽量设置复杂密码并保管好。</p><p><img src="'+p+'" alt="添加环境变量"></p><ol start="3"><li>查看域名并连接</li></ol><p>(1) 点击左侧菜单 “环境” -&gt; “访问服务”，查看默认域名。也可以添加自定义域名（如果了解域名，建议添加，不加也没影响，<em>等于废话</em>）</p><p><img src="'+h+'" alt="查看域名"></p><p>(2) 在 Lavcode 客户端，存储方式选择云接口，地址为 <code>https://域名/v1</code>，密码为前面的环境变量设置的密码 <code>SECRET_KEY</code></p><p>如 <code>https://env-yourenvid-123456789.ap-shanghai.app.tcloudbase.com/v1</code></p><p><img src="'+u+'" alt="登录"></p><h2 id="lavcode-node-介绍" tabindex="-1"><a class="header-anchor" href="#lavcode-node-介绍" aria-hidden="true">#</a> lavcode-node 介绍</h2>',15),x={href:"https://ipare.org",target:"_blank",rel:"noopener noreferrer"},C=e("code",null,"swagger",-1),T={href:"https://ipare.org",target:"_blank",rel:"noopener noreferrer"},N=e("code",null,"swagger",-1),y=o(`<h3 id="本地运行" tabindex="-1"><a class="header-anchor" href="#本地运行" aria-hidden="true">#</a> 本地运行</h3><p>你也可以选择本地运行 <code>lavcode-node</code></p><p>fork 并 clone 项目后，在 <code>src/lavcode-node</code> 目录下新增 <code>.env.local</code> 文件，内如如下</p><div class="language-text line-numbers-mode" data-ext="text"><pre class="language-text"><code>SECRET_KEY=云接口密码
ENV_ID=CloudBase 环境Id
TENCENT_SECRET_KEY=腾讯云 SecretKey
TENCENT_SECRET_ID=腾讯云 SecretId
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div>`,4),L=e("code",null,"SecretKey",-1),q=e("code",null,"SecretId",-1),w={href:"https://cloud.tencent.com/developer/article/1385239",target:"_blank",rel:"noopener noreferrer"},I=o(`<p>先运行命令</p><div class="language-bash line-numbers-mode" data-ext="sh"><pre class="language-bash"><code><span class="token function">npm</span> <span class="token function">install</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div></div></div><p>然后用 vscode 打开 <code>src/lavcode-node</code> 目录，按 F5 即可启动调试</p><p>或执行命令</p><div class="language-bash line-numbers-mode" data-ext="sh"><pre class="language-bash"><code><span class="token function">npm</span> start
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div></div></div><h3 id="部署" tabindex="-1"><a class="header-anchor" href="#部署" aria-hidden="true">#</a> 部署</h3><p>可以本地部署，也可以使用 <code>GitHub Actions</code> 部署</p><h4 id="本地部署" tabindex="-1"><a class="header-anchor" href="#本地部署" aria-hidden="true">#</a> 本地部署</h4><p>与前面 <code>本地运行</code> 部分创建了 <code>.env.local</code> 文件类似，但只用确保包含下面内容</p><div class="language-text line-numbers-mode" data-ext="text"><pre class="language-text"><code>SECRET_KEY=云接口密码
ENV_ID=CloudBase 环境Id
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div></div></div><p>然后在 <code>src/lavcode-node</code> 目录，运行下面命令</p><div class="language-bash line-numbers-mode" data-ext="sh"><pre class="language-bash"><code><span class="token function">npm</span> <span class="token function">install</span>

npx tcb login
npx tcb framework deploy <span class="token parameter variable">--mode</span> <span class="token builtin class-name">local</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="使用-github-actions-部署" tabindex="-1"><a class="header-anchor" href="#使用-github-actions-部署" aria-hidden="true">#</a> 使用 <code>GitHub Actions</code> 部署</h4><ol><li>设置 GitHub Secrets</li></ol><p>Fork 仓库后，设置 GitHub Secrets，添加如下 Secrets</p><ul><li>NODE_ENV_ID: CloudBase 环境 Id</li><li>NODE_SECRET_KEY: 云接口密码</li><li>TENCENT_SECRET_ID: 腾讯云 SecretId</li><li>TENCENT_SECRET_KEY: 腾讯云 SecretKey</li></ul><p><img src="`+v+'" alt="Secrets"></p>',17),K=e("code",null,"SecretKey",-1),A=e("code",null,"SecretId",-1),R={href:"https://cloud.tencent.com/developer/article/1385239",target:"_blank",rel:"noopener noreferrer"},D=o(`<ol start="2"><li>修改脚本</li></ol><p>修改源码文件 <code>.github/workflows/publish-node.yml</code></p><p>删除此行：<code>if: github.repository == &#39;hal-wang/Lavcode&#39;</code></p><ol start="3"><li>提交代码</li></ol><p>上面操作完成后，每次提交代码，都会自动部署</p><h2 id="lavcode-asp-介绍" tabindex="-1"><a class="header-anchor" href="#lavcode-asp-介绍" aria-hidden="true">#</a> Lavcode.Asp 介绍</h2><p>Lavcode.Asp 用 C# 语言开发，使用了 Asp.Net Core 框架，按 Restful 规范实现接口</p><p>数据库使用 Sql Server + EF，可以很方便的切换数据库，如 MySQL 或 Sqlite</p><p>接口的默认页面是 <code>swagger</code>，你可以通过修改源码的方式隐藏 <code>swagger</code></p><h3 id="本地运行-1" tabindex="-1"><a class="header-anchor" href="#本地运行-1" aria-hidden="true">#</a> 本地运行</h3><p>在 <code>Lavcode.Asp</code> 文件夹中，创建文件 <code>appsettings.local.json</code>，内容如</p><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token property">&quot;SecretKey&quot;</span><span class="token operator">:</span> <span class="token string">&quot;your_secret_key&quot;</span><span class="token punctuation">,</span>
  <span class="token property">&quot;ConnectionStrings&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;MSSQL&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Server=127.0.0.1;Database=Lavcode;uid=sa;Password=H;Encrypt=True;TrustServerCertificate=True;&quot;</span>
  <span class="token punctuation">}</span>
<span class="token punctuation">}</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><ul><li><code>SecretKey</code> 为云接口密码</li><li><code>ConnectionStrings.MSSQL</code> 为数据库连接字符串，一般修改地址和端口即可</li></ul><p>无需手动创建数据库，数据库会在首次运行时自动创建</p><p>使用 vs 2022 或更新版本打开 <code>Lavcode.sln</code>，启动项目选择 <code>Lavcode.Asp</code>，按下 F5 即可开始调试</p><h3 id="部署-1" tabindex="-1"><a class="header-anchor" href="#部署-1" aria-hidden="true">#</a> 部署</h3><p>与其他 Asp.NET Core 项目部署方式相同</p><p>可以部署到 <code>IIS</code> / <code>Docker</code>/ <code>Apache</code> 等</p><p>部署时别忘记修改 <code>appsettings.json</code> 文件中的 <code>SecretKey</code> 和 <code>ConnectionStrings.MSSQL</code></p><h2 id="安全性" tabindex="-1"><a class="header-anchor" href="#安全性" aria-hidden="true">#</a> 安全性</h2>`,20),j={href:"https://jwt.io/",target:"_blank",rel:"noopener noreferrer"},B=e("h2",{id:"开发接口",tabindex:"-1"},[e("a",{class:"header-anchor",href:"#开发接口","aria-hidden":"true"},"#"),a(" 开发接口")],-1),F=e("p",null,"你也可以参考源码提供的两种后端方案，自己开发后端接口，能更灵活的更换语言框架和数据库",-1);function V(Y,H){const n=d("ExternalLinkIcon");return c(),r("div",null,[b,e("p",null,[e("a",g,[a("cloudbase"),s(n)]),a(" + nodejs + "),e("a",m,[a("ipare"),s(n)])]),f,e("p",null,[e("a",E,[S,s(n)])]),k,e("p",null,[a("lavcode-node 使用了 nodejs 框架 "),e("a",x,[a("ipare"),s(n)]),a("，按 Restful 规范实现接口")]),e("p",null,[a("接口的默认页面是 "),C,a("，同样是由 "),e("a",T,[a("ipare"),s(n)]),a(" 自动生成，你可以通过修改源码的方式隐藏 "),N]),y,e("p",null,[L,a(" 和 "),q,a(" 的获取参考 "),e("a",w,[a("https://cloud.tencent.com/developer/article/1385239"),s(n)])]),I,e("p",null,[K,a(" 和 "),A,a(" 的获取参考 "),e("a",R,[a("https://cloud.tencent.com/developer/article/1385239"),s(n)])]),D,e("p",null,[a("云接口使用 "),e("a",j,[a("jwt"),s(n)]),a(" 来保证接口不会被非法调用")]),B,F])}const G=t(_,[["render",V],["__file","api.html.vue"]]);export{G as default};