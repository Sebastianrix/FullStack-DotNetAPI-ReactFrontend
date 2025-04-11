using Microsoft.AspNetCore.Mvc;
using Solnet.Rpc;
using Solnet.Rpc.Models;
using Solnet.Programs;
using Solnet.Wallet;
using Solnet.Wallet.Bip39;
using Solnet.Rpc.Builders;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v3/solana")]
    public class SolanaController : ControllerBase
    {
        // 🪙 Replace these values with your actual info
        private const string TokenMintAddress = "J4Bozua2rBCEU3C5kx8pAvuBgJrviaoLXvA5C9nzG3uP";
        private const string RecipientWalletAddress = "J4Bozua2rBCEU3C5kx8pAvuBgJrviaoLXvA5C9nzG3uP";
        private const string Mnemonic = "other usual sleep agent endless member regret sick obvious network book clean";

        private readonly IRpcClient _rpcClient = ClientFactory.GetClient(Cluster.DevNet);

        [HttpGet("wallet-balance")]
        public async Task<IActionResult> GetWalletBalance()
        {
            var result = await _rpcClient.GetTokenAccountsByOwnerAsync(
                RecipientWalletAddress,
                TokenProgram.ProgramIdKey,
                TokenMintAddress
            );

            if (!result.WasSuccessful || result.Result.Value == null || result.Result.Value.Count == 0)
                return Ok(new { balance = 0 });

            var tokenAmount = result.Result.Value[0].Account.Data.Parsed.Info.TokenAmount;
            return Ok(new { balance = tokenAmount.UiAmountString });
        }

        [HttpPost("mint-10")]
        public async Task<IActionResult> MintTenDriveCoins()
        {
            var wallet = new Wallet(Mnemonic, WordList.English);
            var sender = wallet.GetAccount(0);

            var mintPubKey = new PublicKey(TokenMintAddress);
            var recipientPubKey = new PublicKey(RecipientWalletAddress);

            // Create the associated token account (ATA) if it doesn't exist
            var ata = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(recipientPubKey, mintPubKey);

            // Safe to assume it's already created if it's been used once; otherwise, you'd need to handle creation

            var blockHash = (await _rpcClient.GetLatestBlockHashAsync()).Result.Value.Blockhash;

            var tx = new TransactionBuilder()
                .SetRecentBlockHash(blockHash)
                .SetFeePayer(sender)
                .AddInstruction(TokenProgram.MintTo(
                    mintPubKey,
                    ata,
                    10_000_000, // = 10 tokens (adjust decimals)
                    sender))
                .Build(sender);

            var result = await _rpcClient.SendTransactionAsync(tx);

            return Ok(new
            {
                success = result.WasSuccessful,
                signature = result.Result,
                message = result.WasSuccessful ? "10 DriveCoins minted to wallet 🎉" : "Mint failed ❌"
            });
        }

    }
}
