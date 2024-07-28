using MuratBaloglu.Application.Enums;

namespace MuratBaloglu.Application.DTOs.Configurations
{
    public class Action
    {
        //public ActionType ActionType { get; set; }
        public string ActionType { get; set; } //Enum ın string değerini elde etmek için
        public string Definition { get; set; }
        public string HttpType { get; set; }
        public string Code { get; set; } //O anki action ın kendisine özel unik-özgün-tekil kodunu üretmek için
    }
}
