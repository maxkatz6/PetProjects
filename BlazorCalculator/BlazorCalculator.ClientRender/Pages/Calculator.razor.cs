using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCalculator.ClientRender.Pages
{
    public partial class Calculator : ComponentBase
    {
        [Parameter]
        public double NumberA { get; set; }

        [Parameter]
        public double NumberB { get; set; }

        [Parameter]
        public double Result { get; set; }

        [Parameter]
        public CalcOperation Operation { get; set; }

        protected void Recalculate()
        {
            Result = Operation switch
            {
                CalcOperation.Add => NumberA + NumberB,
                CalcOperation.Substract => NumberA - NumberB,
                _ => double.NaN
            };
        }

        public enum CalcOperation
        { 
            Add,
            Substract
        }
    }
}
