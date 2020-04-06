# O que é CopaFilmes?
O projeto CopaFilmes implementa uma API utilizando .NET Core 3, consiste numa ideia de criar uma competição de filmes (como se fosse uma copa do mundo), em que há chaves que classificam os filmes mais bem avaliados a cada disputa, no final, possibilita verificar o vencedor e o vice.

# Como funciona a competição?
São selecionados 8 filmes de um lista pré-cadastrada de filmes, com isso, as chaves são montados pelo seguinte processo:
- Os filmes são ordenados por ordem alfabetica;
- A primeira fase é montada entre os extremos subsequentes.
    - Ex.: São selecionados os filmes "H", "G", "F", "E", "D", "C", "B" e "A", eles serão ordenados, resultando em "A", "B", "C", "D", "E", "F", "G" e "H", em seguida as chaves serão:
    - A x H
    - B x G
    - C x F
    - D x E
- As próximas fases é definida pelas chaves montadas de forma sequencial, logo, o vencedor da chave 1, enfrenta o vencedor da chave 2 e assim por diante.
- Critério de desempate: caso dois filmes empatem na pontuação, o vencedor da rodada será aquele que vier alfabeticamente primeiro. 

# Pré-requisitos
Microsoft .NET Core SDK 3.0+

# Como executar?

Há duas maneiras de executar o projeto.

## Utilizando Visual Studio
- Clone o repositório na sua máquina
- Abra a solução (arquivo CopaFilmes.sln) pelo Visual Studio 2017+
- Execute o projeto API e pronto.

Sua API estará rodando com a porta HTTP 60000 e a porta HTTPS 44300.

## Utilizando dotnet command line
- Abra um terminal/cmd na pasta do projeto API: CopaFilmes/CopaFilmes.API
- Execute a seguinte linha de comando: 
    `dotnet run CopaFilmes.API.csproj`
- Dê as permissões necessárias caso seja solicitado e pronto.

Sua API também estará rodando com a porta HTTP 60000 e a porta HTTPS 44300.

# Quais os endpoints?
Você pode testar a API utilizando o Postman baixando a coleção pelo link: https://www.getpostman.com/collections/17ae6476017fce20290a

Configure a variável "urlApi" para o endereço que estará executando a API (normalmente, http://localhost:60000 ou https://localhost:44300)
