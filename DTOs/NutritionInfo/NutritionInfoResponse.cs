using LemonLime.DTOs.Base;

namespace LemonLime.DTOs.NutritionInfo
{
    public class NutritionInfoResponse : BaseResponse
    {
        public int Calories { get; set; }
        public float Fat { get; set; }
        public float Carbohydrates { get; set; }
        public float Protein { get; set; }
    }
}
