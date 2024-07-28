namespace MuratBaloglu.Application.DTOs.Configurations
{
    public class Menu
    {
        //Menu Adı
        public string Name { get; set; }

        //Hangi menüdeysek(Name property sindeki menü adı) o menüye ait attribute ile işaretlenmiş action listesi
        public List<Action> Actions { get; set; } = new List<Action>();
    }
}
