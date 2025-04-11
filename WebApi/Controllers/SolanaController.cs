using Microsoft.AspNetCore.Mvc;
using Solnet.Rpc;
using Solnet.Programs;
using Solnet.Wallet;
using Solnet.Wallet.Bip39;
using Solnet.Rpc.Builders;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v3/solana")]
    public class SolanaController : ControllerBase
    {
        private const string TokenMintAddress = "J4Bozua2rBCEU3C5kx8pAvuBgJrviaoLXvA5C9nzG3uP";
        private const string RecipientWalletAddress = "J4Bozua2rBCEU3C5kx8pAvuBgJrviaoLXvA5C9nzG3uP";
        private const string Mnemonic = "police ball tourist surround tag slogan design crawl atom echo find hope";

        private readonly IRpcClient _rpcClient = ClientFactory.GetClient(Cluster.DevNet);
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string SolanaRpcUrl = "https://api.devnet.solana.com";

        [HttpGet("wallet-balance")]
        public async Task<IActionResult> GetWalletBalance()
        {
            var balance = await GetTokenBalanceManualAsync(RecipientWalletAddress, TokenMintAddress);
            return Ok(new { balance });
        }

        [HttpPost("mint-10")]
        public async Task<IActionResult> MintTenDriveCoins()
        {
            var wallet = new Wallet(Mnemonic, WordList.English);
            var sender = wallet.GetAccount(0);

            var mintPubKey = new PublicKey(TokenMintAddress);
            var recipientPubKey = new PublicKey(RecipientWalletAddress);
            var ata = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(recipientPubKey, mintPubKey);

            var blockHash = (await _rpcClient.GetLatestBlockHashAsync()).Result.Value.Blockhash;

            var tx = new TransactionBuilder()
                .SetRecentBlockHash(blockHash)
                .SetFeePayer(sender)
                .AddInstruction(TokenProgram.MintTo(
                    mintPubKey,
                    ata,
                    10_000_000, // 10 tokens if 6 decimals
                    sender))
                .Build(sender);
            Console.WriteLine("Sender pubkey (from mnemonic): " + sender.PublicKey);
            var result = await _rpcClient.SendTransactionAsync(tx);

            return Ok(new
            {
                success = result.WasSuccessful,
                signature = result.Result,
                message = result.WasSuccessful ? "10 DriveCoins minted to wallet 🎉" : "Mint failed ❌"
            });
        }

        private async Task<decimal> GetTokenBalanceManualAsync(string owner, string mint)
        {
            var requestBody = new
            {
                jsonrpc = "2.0",
                id = 1,
                method = "getTokenAccountsByOwner",
                @params = new object[]
                {
                    owner,
                    new { mint },
                    new { encoding = "jsonParsed" }
                }
            };

            var httpContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync(SolanaRpcUrl, httpContent);

            if (!httpResponse.IsSuccessStatusCode) return 0;

            var json = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<RpcTokenAccountsResponse>(json);

            if (response == null || response.result == null || response.result.value == null || response.result.value.Count == 0)
                return 0;

            var amountStr = response.result.value[0].account.data.parsed.info.tokenAmount.uiAmountString;
            return decimal.Parse(amountStr);
        }

        public class RpcTokenAccountsResponse
        {
            public Result result { get; set; }

            public class Result
            {
                public List<Value> value { get; set; }
            }

            public class Value
            {
                public Account account { get; set; }
            }

            public class Account
            {
                public Data data { get; set; }
            }

            public class Data
            {
                public Parsed parsed { get; set; }
            }

            public class Parsed
            {
                public Info info { get; set; }
            }

            public class Info
            {
                public TokenAmount tokenAmount { get; set; }
            }

            public class TokenAmount
            {
                public string uiAmountString { get; set; }
            }
        }
    }
}
