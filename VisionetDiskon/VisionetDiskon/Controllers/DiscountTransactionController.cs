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
		public async Task<ActionResult> CreateTransaction([FromBody] ModelDTO model)
		{
            var diskon = 0m;
			if (model.CustomerType == "platinum")
            {
                if (model.RewardPoints >= 100 && model.RewardPoints <= 300) diskon= model.TotalBelanja * 0.5m + 35;
                else if (model.RewardPoints >= 301 && model.RewardPoints <= 500) diskon= model.TotalBelanja * 0.5m + 50;
                else if (model.RewardPoints > 500) diskon= model.TotalBelanja * 0.5m + 68;
            }
            else if (model.CustomerType == "gold")
            {
                if (model.RewardPoints >= 100 && model.RewardPoints <= 300) diskon= model.TotalBelanja * 0.25m + 25;
                else if (model.RewardPoints >= 301 && model.RewardPoints <= 500) diskon= model.TotalBelanja * 0.25m + 34;
                else if (model.RewardPoints > 500) diskon= model.TotalBelanja * 0.25m + 52;
            }
            else if (model.CustomerType == "silver")
            {
                if (model.RewardPoints >= 100 && model.RewardPoints <= 300) diskon= model.TotalBelanja * 0.1m + 12;
				else if (model.RewardPoints >= 301 && model.RewardPoints <= 500) diskon = model.TotalBelanja * 0.1m + 27;
                else if (model.RewardPoints > 500) diskon= model.TotalBelanja * 0.1m + 39;
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

            dt.CustomerType = model.CustomerType;
            dt.RewardPoints = model.RewardPoints;
            dt.Discount = diskon;
            dt.TotalBelanja = model.TotalBelanja;

            dt.TotalBayar = model.TotalBelanja - diskon;

            _context.Transactions.Add(dt);
            await _context.SaveChangesAsync();

            return Ok(new { NewTransaction = dt});
		}

        [HttpGet]
        public async Task<ActionResult> GetListTransaction([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var totalRecords = await _context.Transactions.CountAsync();
            var transactions = await _context.Transactions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                //.Select(v=> new ModelDTO { })
                .ToListAsync();
            if (totalRecords <= 0) { return BadRequest("Tidak ada transaksi"); }

            return Ok(new PaginatedResponse<DiscountTransaction>(transactions, totalRecords, pageNumber,pageSize));
        }
	}

    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginatedResponse(List<T> data, int total, int pageNumber, int pageSize)
        {
            Data = data;
            Total = total;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
