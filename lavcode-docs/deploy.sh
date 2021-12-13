cd .vuepress/dist/
git init
git add -A
git commit -m deploy
git remote add origin git@github.com:hal-wang/lavcode-docs.git
git push origin main -f