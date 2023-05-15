using DeliveryAgreagatorApplication.Notifications.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Notifications.DAL
{
	public class NotificationDbContext: DbContext
	{
		public DbSet<NotificationDbModel> Notifications { get; set; }
		public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }
	}
}
