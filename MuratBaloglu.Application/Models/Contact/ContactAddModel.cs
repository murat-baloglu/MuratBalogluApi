namespace MuratBaloglu.Application.Models.Contact
{
    public class ContactAddModel
    {
        public string? Mobile { get; set; } //Cep Telefonu
        public string? FixedPhoneOne { get; set; } //Sabit Telefon
        public string? FixedPhoneOneExtension { get; set; } //Dahili Numara
        public string? FixedPhoneTwo { get; set; }
        public string? FixedPhoneTwoExtension { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? GoogleMap { get; set; }
    }
}
