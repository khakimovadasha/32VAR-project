using System.Collections.Generic;

namespace MY_PROJ32VAR.Models
{
    public class FractionTask
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public TaskType Type { get; set; }
        public List<string>? VisualVariants { get; set; } 
        public List<int>? CorrectAnswers { get; set; } // Индексы правильных вариантов
    }
} 