using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.MAUI.Models
{
    public class Result
    {
        public string Question { get; set; } = string.Empty;
        public string YourAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public bool IsCorrect => YourAnswer == CorrectAnswer;
    }
}
