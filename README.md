InvestimentApi
Descrição
Esta Aplicação é dividida em um micro-serviço e uma api:

O micro-serviço em .NET Core 7 foi desenvolvido para interagir com o WebSocket de Order Book público da Bitstamp, 
mantendo e tratando dados de dois instrumentos financeiros: BTC/USD e ETH/USD.

A aplicação realiza operações de simulação de melhores preços e exibe informações de preço e quantidade periodicamente.

Funcionalidades

Conexão com o WebSocket da Bitstamp:

Conecta-se ao WebSocket API v2 da Bitstamp para obter dados em tempo real do Order Book.
Ingestão e tratamento de dados para os instrumentos BTC/USD e ETH/USD.

Persistência de Dados:
Armazena os dados recebidos em uma base NoSQL ou SQL para consultas posteriores.

Exibição de Dados:
A cada 5 segundos, a aplicação exibe no console (ou uma interface web simples, se implementada):
Maior e menor preço de cada ativo.
Média de preço de cada ativo no momento.
Média de preço acumulada de cada ativo nos últimos 5 segundos.
Média de quantidade acumulada de cada ativo.

API de Simulação de Melhor Preço:
Exponha uma API para calcular o melhor preço baseado na operação solicitada (compra ou venda).
Para uma compra de uma quantidade específica, a API ordena os "asks" em ordem crescente de preço e calcula o valor correspondente.
Para uma venda, a API realiza a mesma operação nos "bids", ordenando-os em ordem decrescente de preço.

O retorno da API inclui:

ID único da cotação.
Coleção de itens utilizados no cálculo.
Quantidade solicitada.
Tipo de operação (compra/venda).
Resultado do cálculo.
Grava o histórico de cálculos no banco de dados.

Desacoplamento da Ingestão de Dados:
A API de simulação não interfere na ingestão de dados em tempo real.

Requisitos
.NET Core 7
Docker (para rodar containers)
Acesso à API WebSocket da Bitstamp
Banco de dados NoSQL ou SQL (configurável)

Configuração

Configuração do WebSocket:
Conecte-se ao WebSocket da Bitstamp usando as informações fornecidas na documentação da Bitstamp.

Configuração do Banco de Dados:
Configure a string de conexão para seu banco de dados no arquivo appsettings.json.

Executar o Serviço:
Compile e execute a aplicação localmente ou utilizando Docker.
Para rodar via Docker, utilize o Dockerfile e docker-compose.yml fornecidos.

Testes
Testes de Unidade e Integração:
Testes automatizados estão configurados para verificar a lógica de negócios e interações com repositórios e serviços.
