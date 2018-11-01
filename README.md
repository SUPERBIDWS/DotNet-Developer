# Quer fazer parte da S4Pay?

Se você está lendo isso, PARABÉNS! Você tem a chance de participar de um grande time com muitos desafios pela frente! Tudo que você tem a fazer é um simples calculo de árvore binária para localizar e interceptar transações dentro de um ladger público e destrbuído do Blackchain e desviar as transações para carteira abaixo: ....rs

# Sua missão!

Será muito mais simples do que isso. Como somos uma empresa focada em meios de pagamento, queremos apenas que você faça um sistema que simule transações de uma conta X para uma conta Y e uma API que exponha saldo e transações.

Apenas algumas frescuras:

- A conta X começa com 100.000 de saldo e a conta Y com 10.000;
- Uma transação criada através da API deve ficar pendente até o momento de aprovação; 
- Uma aplicação de console (será executada  manualmente pelo Visual Studio em debug) fará a efetivação dessa transação; 
- Pode-se usar EF, MongoDb, ambos ou até mesmo um  arquivo JSON como
 fonte de dados iniciais, feel free!  
- Facilite o setup do projeto com instruções (por favor!);

# Entrega

A entrega pode ser feita fazendo um FORK desse projeto e criando um Pull Request ao finalizar.

-----------------------------------------------------------------------------------------------------------------------

Para poder testar o sistema, utilizar a solution como execução, pois necessitamos dos 3 projetos rodando juntos. (Já está configurado)

O projeto foi feito em camadas. Poderiam ser incluídas mais camadas como por exemplo a de regra de negócios, mas optei por algo mais simples,
pois já iria criar 5 projetos na solution.

Primeiro você deverá visualizar o Sistema Transação, onde possui uma tela para realização de transações, consultas de saldos nas duas contas e 
uma lista com o histórico de transações realizadas. (Os IDs fixos não são uma boa prática de programação, mas foram usados apenas para facilitar 
no desenvolvimento do teste).

Para cadastrar uma transação, escolha a conta de origem, a de destino e o valor desejado.

Se a conta possuir o montante informado, ele será retirado do saldo da mesma e ficará pendente de aprovação.

A API é o único projeto que tem acesso a camada de dados, os outros consomem e enviam para ela as informações.

Depois de realizar as transações desejadas, vá para a tela do console, onde você terá uma lista de opções para poder escolher.

Ao escolher a opção 1, o programa irá buscar todas as transações pendentes.

Aparecerá uma nova opção, para poder verificar essas transações e aprovar ou reprovar as mesmas.

Você pode fazer uma e parar ou seguir validando as transações.

As que forem aprovadas, o sistema liberará o saldo para a conta Destino.

As que forem reprovadas, o sistema retornará o saldo para a conta Origem.

Espero ter atendido ao requisitado.

Obrigado.

** Para evitar problemas com a string de conexão, o banco de dados está alocado em um servidor gratuito, podendo ser lento ou 
instável algumas vezes.
