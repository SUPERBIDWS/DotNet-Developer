# DotNet-Developer / S4Pay

São dois projetos

  - API.S4Pay c/ MongoDb
  - S4Pay 


### Instalação


| Baixar | URL |
| ------ | ------ |
| MongoDb | https://www.mongodb.com/download-center/community
| Robomongo 3T| https://robomongo.org/download


# Instalar MongoDB

O MongoDB requer uma pasta de dados para armazenar seus arquivos. O local padrão para o diretório de dados MongoDB é ```C:\data\db```. Então, você precisa criar essa pasta usando o Prompt de Comando. Execute a seguinte sequência de comandos:

```
C:\md data
C:\md data\db
```

Se você tem instalar o MongoDB em local diferente então você precisa especificar qualquer caminho alternativo para ```\data\db```, definindo o caminho dbpath no mongod.exe. Para a mesma questão execute os seguintes comandos

No prompt de comando vá para o diretório bin localizado na pasta de instalação do MongoDB. Suponha que a pasta de instalação seja  ```D:\set up\mongodb```


```
D:\cd "set up"
D:\set up>cd mongodb
D:\set up\mongodb>cd bin
D:\set up\mongodb\bin>mongod.exe --dbpath "d:\set up\mongodb\data"
```

Isto irá mostrar à espera de conexões mensagem na saída do console indicando que o processo ```mongod.exe``` está sendo executado com sucesso.


# Criar Database

Para criar um banco de dados basta utilizar o comando ```use database_name```, o comando irá criar um novo banco de dados se caso ele não existir, de outra forma ele irá retornar o banco de dados existente.

Sintaxe:

A sintaxe básica para a utilização do ```USE_DATABASE``` é a seguinte:

```
use DATABASE_NAME
```
Exemplo

Se você quer criar um banco de dados com o nome ```<mydb>```, então a sintaxe seria a seguinte:
```
use mydb
switched to db mydb
```
Para verificar o banco de dados selecionado atualmente usar o comando db
```
> db mydb
```
Se você quiser verificar a sua lista de bancos de dados, em seguida, use o comando show dbs.

```
> show dbs
local 0.78125GB
test 0.23012GB
```

Você deve ter notado que seu banco de dados criado (mydb) não está presente na lista. Para exibir dados é necessário inserir pelo menos um documento para ele.

# Criar Collection

Sintaxe

A sintaxe básica do método ```createCollection ()``` é como se segue:
```
> use test
switched to db test
> db.createCollection("mycollection"){"ok":1}>
```

# Inserir Documento

Sintaxe

A sintaxe básica do comando ```insert()``` é a seguinte:

```> db.COLLECTION_NAME.insert(document)```


Exemplo
```
> db.mycol.insert({
    "_id" : "843adfb8-50e5-41a4-9f00-6fe0cea96dfb",
    "UserId" : "0b7c1c6f-e2db-4a40-b457-4473a98f77db",
    "Pending" : false,
    "Value" : 100000.0,
    "Description" : "Y"
})

```

# Consulta de Documentos
Para consultar os dados de Collection MongoDB, você precisa usar o comando find() do MongoDB.

Sintaxe

A sintaxe básica do comando ```find()``` é como se segue:
```
> db.COLLECTION_NAME.find()
```
O comando find () irá exibir todos os documentos de uma forma não estruturada.

# Instalar RoboMongo
Como conectar um banco mongodb através do aplicativo RoboMongo
* [RoboMongo](<https://king.host/wiki/artigo/como-conectar-um-base-mongodb-atraves-do-aplicativo-robomongo/>)

# API.S4Pay
Após a instalaçao do MongoDb

> Crie um banco chamado ```s4pay``` e uma Collection chamada ```Transaction```.
 * Insira os dados abaixo:
```
/* 1 */
{
    "_id" : "843adfb8-50e5-41a4-9f00-6fe0cea96dfb",
    "UserId" : "0b7c1c6f-e2db-4a40-b457-4473a98f77db",
    "Pending" : false,
    "Value" : 100000.0,
    "Description" : "X"
}

/* 2 */
{
    "_id" : "b0d6e199-a652-41eb-a529-e6ce0266d228",
    "UserId" : "7c5c1c21-4eca-4859-ade0-677b0792e59f",
    "Pending" : false,
    "Value" : 10000.0,
    "Description" : "Y"
}
```

Após criar a base e a collection inicie o projeto ```Api.S4Pay```
logo depois nicie o projeto ```S4Pay```


