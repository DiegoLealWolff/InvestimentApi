using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Client;
using Bitstamp.Client.Websocket.Communicator;
using Bitstamp.Client.Websocket.Requests;
using Bitstamp.Client.Websocket.Responses.Books;
using InvestimentApi.Models;
using InvestimentApi.Services;

namespace InvestimentApi.HostedServices
{
    public class OrderBookBackgroundService : BackgroundService
    {
        private readonly ILogger<OrderBookBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private BitstampWebsocketClient _client = null!;
        private Timer _timer = null!;

        public OrderBookBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<OrderBookBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Inicializar o cliente WebSocket da Bitstamp
            var url = new Uri("wss://ws.bitstamp.net");
            var exitEvent = new ManualResetEvent(false);

            // Iniciar um timer que a cada 5 segundos irá calcular os preços
            _timer = new Timer(CalculatePrices, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));  

            // Aqui fica o loop de execução principal (WebSocket)
            while (!stoppingToken.IsCancellationRequested)
            {
                using var communicator = new BitstampWebsocketCommunicator(url);
                _client = new BitstampWebsocketClient(communicator);
                _client.Streams.OrderBookStream.Subscribe(HandleOrderBookMessage);

                await communicator.Start();

                // Assinar os canais para BTC/USD e ETH/USD
                _client.Send(new SubscribeRequest("btcusd", Channel.OrderBook));
                _client.Send(new SubscribeRequest("ethusd", Channel.OrderBook));

                // Manter a conexão ativa enquanto o serviço não for cancelado
                exitEvent.WaitOne();
            }
        }

        public async void HandleOrderBookMessage(OrderBookResponse book)
        {
            if (book == null || book.Data == null)
            {
                _logger.LogWarning("Received empty book data.");
                return;
            }

            // Determinar o par de moedas
            var assetPair = book.Channel.Contains("btcusd") ? "BTC/USD" : "ETH/USD";

            // Persistir as ordens no banco de dados
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var orderBookService = scope.ServiceProvider.GetRequiredService<IOrderBookService>();

                foreach (var ask in book.Data.Asks)
                {
                    await orderBookService.AddOrderAsync(new OrderDto
                    {
                        AssetPair = assetPair,
                        OrderType = OrderType.Sell,
                        Price = Convert.ToDecimal(ask.Price),
                        Quantity = Convert.ToDecimal(ask.Amount)
                    });
                }

                foreach (var bid in book.Data.Bids)
                {
                    await orderBookService.AddOrderAsync(new OrderDto
                    {
                        AssetPair = assetPair,
                        OrderType = OrderType.Buy,
                        Price = Convert.ToDecimal(bid.Price),
                        Quantity = Convert.ToDecimal(bid.Amount)
                    });
                }
            }
        }

        public void CalculatePrices(object? state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var orderBookService = scope.ServiceProvider.GetRequiredService<IOrderBookService>();

                // Obter as ordens dos últimos 5 segundos para BTC/USD
                var fiveSecondsAgo = DateTime.Now.AddSeconds(-5);

                // Buscar ordens de BTC/USD e ETH/USD nos últimos 5 segundos
                var btcOrders = orderBookService.GetAsync("BTC/USD", fiveSecondsAgo).Result;
                var ethOrders = orderBookService.GetAsync("ETH/USD", fiveSecondsAgo).Result;

                // Cálculos para BTC/USD
                if (btcOrders.Any())
                {
                    //Maior preço de cada ativo naquele momento
                    var maxBtcPrice = btcOrders.Max(o => o.Price);
                    //Menor preço de cada ativo naquele momento
                    var minBtcPrice = btcOrders.Min(o => o.Price);
                    //Média de preço de cada ativo naquele momento
                    var avgBtcPrice = btcOrders.Average(o => o.Price);
                    //Média de preço acumulada (ponderada pela quantidade)
                    var totalBtcQuantity = btcOrders.Sum(o => o.Quantity);
                    var weightedAvgBtcPrice = totalBtcQuantity > 0
                        ? btcOrders.Sum(o => o.Price * o.Quantity) / totalBtcQuantity
                        : 0;
                    //Média de quantidade acumulada
                    var avgBtcQuantity = btcOrders.Average(o => o.Quantity);

                    Console.WriteLine($"BTC/USD - Maior Preço: {maxBtcPrice}, Menor Preço: {minBtcPrice}, Média de Preço: {avgBtcPrice}" +
                                      $", Média de Preço Acumulada: {weightedAvgBtcPrice}, " +
                                      $", Média de Quantidade Acumulada: {avgBtcQuantity}");
                }

                // Cálculos para ETH/USD
                if (ethOrders.Any())
                {
                    //Maior preço de cada ativo naquele momento
                    var maxEthPrice = ethOrders.Max(o => o.Price);
                    //Menor preço de cada ativo naquele momento
                    var minEthPrice = ethOrders.Min(o => o.Price);
                    //Média de preço de cada ativo naquele momento
                    var avgEthPrice = ethOrders.Average(o => o.Price);
                    //Média de preço acumulada (ponderada pela quantidade)
                    var totalEthQuantity = ethOrders.Sum(o => o.Quantity);
                    var weightedAvgEthPrice = totalEthQuantity > 0
                        ? ethOrders.Sum(o => o.Price * o.Quantity) / totalEthQuantity
                        : 0;
                    //Média de quantidade acumulada
                    var avgEthQuantity = ethOrders.Average(o => o.Quantity);

                    Console.WriteLine($"ETH/USD - Maior Preço: {maxEthPrice}, Menor Preço: {minEthPrice}, Média de Preço: {avgEthPrice}" +
                                      $", Média de Preço Acumulada: {weightedAvgEthPrice}, " +
                                      $", Média de Quantidade Acumulada: {avgEthQuantity}");
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(cancellationToken);
        }
    }
}