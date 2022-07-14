using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OnlyOne_Pattern
{
    class Program
    {
        static HttpClient _httpClient;
       
        static async Task Main(string[] args)
        {
            _httpClient = new HttpClient();

            try
            {
                await TestOnlyOneIEnumerable();
                await TestOnlyOneParams();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
            }
            finally
            {
                _httpClient.Dispose();
            }
            
            Console.ReadKey();
        }

        public static async Task TestOnlyOneParams()
        {
            var content = await GenericOnlyOnePattern(
                (ct) => RequestViaCepApi("78280000", ct),
                (ct) => RequestApiCep("78280000", ct),
                (ct) => RequestName("Ewerton", ct)
                );

            Console.WriteLine($"{content}");
        }

        public static async Task TestOnlyOneIEnumerable()
        {
            List<string> responseCodes = new List<string>() { "200", "201", "102", "400", "102" };

            var tasks = responseCodes.Select(name =>
            {
                Func<CancellationToken, Task<string>> func = (ct) => RequestWithReturnAsync(name, ct);
                return func;
            });

            var content = await GenericOnlyOnePattern(tasks.ToArray());

            Console.WriteLine($"\n{content}");
        }
 
        public static async Task<T> GenericOnlyOnePattern<T>(params Func<CancellationToken, Task<T>>[] functions)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            IEnumerable<Task<T>> tasks = functions.Select(function => function(cancellationTokenSource.Token));
            var task = await Task.WhenAny(tasks);
            cancellationTokenSource.Cancel();
            return await task;
        }
        public static async Task<string> RequestName(string name, CancellationToken token)
        {
            var WaitingTime = new Random().NextDouble() * 10 + 1;
            await Task.Delay(TimeSpan.FromSeconds(WaitingTime));
            string message = $"Hello {name}";
            return message;
        }
        private static async Task<string> RequestWithReturnAsync(string code, CancellationToken cancellationToken = default)
        {
            string apiHome = "http://httpstat.us";
            var WaitingTime = new Random().NextDouble() * 10 + 1;
            string uriApi = $"{apiHome}/{code}?sleep={WaitingTime}";
            var response = await _httpClient.GetStringAsync(uriApi);
            return response;
        }
        private static async Task<string> RequestViaCepApi(string cep, CancellationToken cancellationToken = default)
        {
            string uriApi = $"https://viacep.com.br/ws/{cep}/json/";
            var response = await _httpClient.GetStringAsync(uriApi);
            return $"{nameof(RequestViaCepApi)} => {response}";
        }

        private static async Task<string> RequestApiCep(string cep, CancellationToken cancellationToken = default)
        {
            string uriApi = $"https://ws.apicep.com/cep/{cep}.json";
            var response = await _httpClient.GetStringAsync(uriApi);
            return $"{nameof(RequestApiCep)} => {response}";
        }
    }
}
