using FPTPlay.Models;

namespace FPTPlay.ViewModels
{
    public class HomeViewModel
    {
        public List<Movie> NewReleases { get; set; } = new();
        public List<Movie> Personalized { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
    }
}
