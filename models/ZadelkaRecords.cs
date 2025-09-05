namespace WebApplication1.models
{
    public class ZadelkaRecords
    {
        // Убедитесь, что имя поля совпадает с именем колонки в БД
        public string series1 { get; set; }
        public string series2 { get; set; }
        public string number { get; set; }
        public string? checkedtabnom { get; set; }
        public DateTime? checkeddatetime { get; set; }

        // Если нужно, можно добавить id, но он не будет использоваться как PK
        public int id { get; set; } // если есть колонка id, но она не PK
    }
}