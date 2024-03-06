using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisionetDiskon.Context;
using VisionetDiskon.Models;

namespace VisionetDiskon.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DiscountTransactionController : Controller
	{
        private readonly VisionetDb _context;

        public DiscountTransactionController(VisionetDb context)
        {
            _context = context;
        }

		[HttpPost]
		public async Task<ActionResult> CreateTransaction(string tipeCustomer, int pointReward, int totalBelanja)
		{
            var diskon = 0m;
			if (tipeCustomer == "platinum")
            {
                if (pointReward >= 100 && pointReward <= 300) diskon= totalBelanja * 0.5m + 35;
                else if (pointReward >= 301 && pointReward <= 500) diskon= totalBelanja * 0.5m + 50;
                else if (pointReward > 500) diskon= totalBelanja * 0.5m + 68;
            }
            else if (tipeCustomer == "gold")
            {
                if (pointReward >= 100 && pointReward <= 300) diskon= totalBelanja * 0.25m + 25;
                else if (pointReward >= 301 && pointReward <= 500) diskon= totalBelanja * 0.25m + 34;
                else if (pointReward > 500) diskon= totalBelanja * 0.25m + 52;
            }
            else if (tipeCustomer == "silver")
            {
                if (pointReward >= 100 && pointReward <= 300) diskon= totalBelanja * 0.1m + 12;
				else if (pointReward >= 301 && pointReward <= 500) diskon = totalBelanja * 0.1m + 27;
                else if (pointReward > 500) diskon= totalBelanja * 0.1m + 39;
            }
            else
            {
                return BadRequest("Tipe Customer hanya ada 'platinum', 'gold', dan 'silver' !");
            }

			DiscountTransaction dt = new DiscountTransaction();

            var checkId = await _context.Transactions.AnyAsync(x => x.TransactionId.Substring(9) == "00001");

            if (!checkId)
            {
				var firstTransactionId = 1;
				dt.TransactionId = $"{DateTime.Now.ToString("yyyyMMdd")}_{firstTransactionId.ToString("D5")}";
            }
            else
            {
                var checkMaxTransactionId = await _context.Transactions.MaxAsync(x => x.TransactionId.Substring(9));
                int.TryParse(checkMaxTransactionId, out var maxId);
                maxId++;
				dt.TransactionId = $"{DateTime.Now.ToString("yyyyMMdd")}_{maxId:D5}";
			}

            dt.CustomerType = tipeCustomer;
            dt.RewardPoints = pointReward;
            dt.Discount = diskon;
            dt.TotalBelanja = totalBelanja;

            dt.TotalBayar = totalBelanja - diskon;

            _context.Transactions.Add(dt);
            await _context.SaveChangesAsync();

            return Ok(new { NewTransaction = dt});
		}

        [HttpGet]
        public async Task<ActionResult> GetListTransaction()
        {
            var getList = await _context.Transactions.ToListAsync();
            if (getList.Count < 0) { return BadRequest("Tidak ada transaksi"); }

            return Ok(getList);
        }
	}
}
