using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication3.Model
{
	public class AuditLog
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string Action { get; set; }
		public string Details { get; set; }
		public DateTime Timestamp { get; set; }
	}

	public class AuditLogService
	{
		private readonly AuthDbContext dbContext;

		public AuditLogService(AuthDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public void LogAudit(string userId, string action, string details)
		{
			var auditLog = new AuditLog
			{
				UserId = userId,
				Action = action,
				Details = details,
				Timestamp = DateTime.UtcNow
			};

			dbContext.AuditLogs.Add(auditLog);
			dbContext.SaveChanges();
		}
	}

	public class AuditController : Controller
	{
		private readonly AuditLogService auditLogService;

		public AuditController(AuditLogService auditLogService)
		{
			this.auditLogService = auditLogService;
		}

		public void LogAction(string action, string desc)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			auditLogService.LogAudit(userId, action, desc);
		}
	}

}
