namespace FPTPlay.Admin.Models
{
    public class DashboardVM
    {
        //Thống kê user
        public int TotalUsers { get; set; } = 1;               
        public int TotalLogingUsers { get; set; } = 0; //Tổng số user có thể đăng nhập
        public int TotalInactiveUsers { get; set; } = 0; //Tổng số user bị khóa
        

        //Thống kê theo phim
        public int TotalMovies { get; set; } = 1; //Tổng số phim
        public int TotaNewReleasedThisWeek { get; set; } = 1; //Tổng số phim mới phát hành tuần này
        public int Top10ViewedMovies { get; set; } = 1; //Top 10 phim được xem nhiều nhất       

        //Thống kê hôm nay
        public int TodayRegisterdUser { get; set; } = 0; //Tổng số user đăng ký hôm nay

        public int TodayViewedCount { get; set; } = 0; //Tổng số lượt xem hôm nay

        public int TodayCreatedMovies { get; set; } = 0;  //Tổng số phim được up hôm nay
    }
}
