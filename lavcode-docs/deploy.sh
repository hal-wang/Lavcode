cd .vuepress/dist/
git init
git add -A
git commit -m deploy
git remote add origin https://github.com/hal-wang/lavcode-docs.git
git push origin master -f