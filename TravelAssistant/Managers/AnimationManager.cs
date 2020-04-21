using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace TravelAssistant.Managers
{
    public class AnimationManager
    {
        /// <summary>
        /// Запуск анимации для PancakeView.
        /// Анимация уменьшение-увеличение.
        /// </summary>
        public static async Task StartScalePancakeView(object sender)
        {
            var s = (sender as PancakeView);
            await s.ScaleTo(0.9, 100);
            await s.ScaleTo(1, 100);
        }
    }
}
