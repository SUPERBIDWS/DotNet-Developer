** Passo a passo configurações **

-verificar o nome da instancia do sqlexpress instalada e alterar no web.config do projeto Web.API
-abrir package manager console
-selecionar na opção "Default project" o valor "SuperBid.WebAPI"
->enable-migrations
-substituir o código do SuperBid.WebAPI.Migrations.Configuration.cs do github pelo SuperBid.WebAPI.Migrations.Configuration.cs gerado pelo migrations(para popular o banco com a carga inicial)
->add-migration Initial
->update-database
-iniciar o projeto WebAPI e verificar a porta
-alterar a porta no app.config do console.application
-selecionar o projeto de console.application para iniciar primeiro e debugar
