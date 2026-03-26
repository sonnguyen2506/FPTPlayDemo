using FPTPlay.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FPTPlay.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace FPTPlay.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : BaseAdminController
    {
        private readonly FPTPlayContext _db;

        public DashboardController(FPTPlayContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var weekAgo = today.AddDays(-7);

            var vm = new DashboardVM
            {
                // ===== USER =====
                TotalUsers = await _db.Users.CountAsync(x => !x.IsDeleted),

                TotalLogingUsers = await _db.Users.CountAsync(x => !x.IsDeleted && x.IsActive),

                TotalInactiveUsers = await _db.Users.CountAsync(x => !x.IsDeleted && !x.IsActive),

                TodayRegisterdUser = await _db.Users.CountAsync(x => !x.IsDeleted && x.CreatedAt.Date == today),

                // ===== MOVIES =====
                TotalMovies = await _db.Movies.CountAsync(),

                // ⚡ dùng IsNewRelease (chuẩn hơn DB bạn)
                TotaNewReleasedThisWeek = await _db.Movies.CountAsync(x =>
                    x.IsNewRelease == true ||
                    (x.CreatedDate.Date >= weekAgo)
                ),

                // Top 10 phim nhiều view
                Top10ViewedMovies = await _db.Movies
                    .OrderByDescending(x => x.VideoCount)
                    .Take(10)
                    .CountAsync(),

                TodayCreatedMovies = await _db.Movies.CountAsync(x => x.CreatedDate.Date == today),

                // Tổng lượt xem hôm nay
                TodayViewedCount = await _db.Movies
                    .Where(x => x.CreatedDate.Date == today)
                    .SumAsync(x => (int?)x.VideoCount) ?? 0
            };

            return View(vm);
        }
    }
}