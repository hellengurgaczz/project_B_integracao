using System;

namespace project_B_integracao.Models
{
    public class Folha
    {
        public int id { get; set; }
        public int mes { get; set; }
        public int ano { get; set; }
        public int horas { get; set; }
        public int valor { get; set; }
        public int bruto { get; set; }
        public int irrf { get; set; }
        public int inss { get; set; }
        public int fgts { get; set; }
        public int liquido { get; set; }
        public string funcionario { get; set; }
        public string createdAt  { get; set; }
        public string  updatedAt  { get; set; }

    }
}