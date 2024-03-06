using Microsoft.EntityFrameworkCore;
using VisionetDiskon.Models;

namespace VisionetDiskon.Context
{
	public class VisionetDb : DbContext
	{
		public VisionetDb(DbContextOptions<VisionetDb> options) : base(options)
		{ }

		public DbSet<DiscountTransaction> Transactions { get; set; }
	}	
}
