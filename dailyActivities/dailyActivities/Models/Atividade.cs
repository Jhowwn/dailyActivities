using System;
using SQLite;

namespace dailyActivities.Models
{
    public class Atividade
    {
        [primarykey, AutoIncreament]
        public int id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public double? Peso { get; set; }
        public string Observacoes { get; set; }
    }
}
