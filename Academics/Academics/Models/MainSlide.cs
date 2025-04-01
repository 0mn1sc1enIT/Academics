namespace Academics.Models
{
    public class MainSlide
    {
        public string BackgroundImage { get; set; }
        public string Title { get; set; }

        public MainSlide(string backgroundImage, string title) 
        { 
            BackgroundImage = backgroundImage;
            Title = title;
        }
    }
}
