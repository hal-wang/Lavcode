cd docs/.vuepress/dist/
git init -b gh-pages
git add -A
git commit -m deploy
git remote add origin git@github.com:hal-wang/Lavcode.git
git push origin gh-pages -f