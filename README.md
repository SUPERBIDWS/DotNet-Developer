## Configurações
Na solution existem dois projetos que deve ser executados juntos, o primeiro Superbid.WebApi, porém antes deve-se configurar a conexão com o mongodb no arquivo appsettings.json, baixar as dependências no nuget e startar a api.
O outro projeto é o projeto console, que também necessita das dependências do nuget. Ao rodar a aplicação console, será solicitado ao usuário que entre com a url da api ex: http://localhost:5050/

## Implementações e Observações
Conforme solicitado no teste, utilizei o mongodb como base de dados bem como entity framework, o EF como todos sabem é um ORM muito utilizado para diminuir a impedância entre o mundo relacional com o mapeamento de dominio da aplicação. Ocorre que o mongodb é um banco de dados nao relacional, e muito flexivel no armazenamento de dados é muito facilmente utilizado principalmente com linguagens dinâminas como python ou mesmo node.js, o EF neste caso engessa a utilização do mongo visto que o mapeamento as entidades com o mongodb mesmo com o driver nativo mongodb, é muito simples ou quase automático.

