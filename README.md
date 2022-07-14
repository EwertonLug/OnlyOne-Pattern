# OnlyOne-Pattern

## A ideia principal do OnlyOne é fazer varis requisições e aguarda a que for mais rápida. Se uma requisição obtiver resposta as outras é canceladas e para isso é usado o CancellationToken.

Teste 2 = Usei a API http://httpstat.us para receber respostas com tempos aleatorios. Com isso criei uma lista de codigos para requisição e vejo qual é o mais rapido.

Teste 2 = Usei duas APIS(https://viacep.com.br e https://ws.apicep.com) diferentes para fazer a msm consulta e testar o OnlyOne com métodos diferentes.
