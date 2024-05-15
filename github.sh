#!/bin/sh

#Altere as variáveis abaixo de acordo com a sua necessidade
#na variável repositório, prefira a url do repositório baseada em https para nao termos que configurar SSH
export name='Daniel Manoel'
export email="daniel.silva1398@etec.sp.gov.br"
export repositorio="https://github.com/danielmbs123/abracepets-api.git";

#Daqui pra baixo mexa somente se for realmente necessário ou se souber o que está fazendo :P

export branch_name="tcc-api-$(date +%d-%m-%Y %H:%M:%S)"

#add linux, mac and windows folders to gitignore
echo "" >> .gitignore

git init
git remote add origin $repositorio
git checkout -b $branch_name

git config --global user.name $name
git config --global user.email $email

git add .gitignore
git commit -m "added ignored files" .gitignore

git add .

git commit -am "branch com o código da aula do dia $(date)"

git push origin $branch_name