using System;

namespace SiteParser.Model
{
    public class Advertisement
    {
        // Адрес объекта
        public string Address { get; set; }   

        // Дата размещения объявления
        public DateTime? PlacingDate { get; set; }

        // Цена за кв. метр 
        public string Price { get; set; }

        // Площадь
        public double? Square { get; set; }

        // Фотографии объекта
        public string Photos { get; set; }
    }
}
