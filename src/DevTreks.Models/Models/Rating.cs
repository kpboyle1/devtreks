using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class Rating
    {
        public int PKId { get; set; }
        public string RatingName { get; set; }
        public float RatingValue { get; set; }
        public int RatingClassId { get; set; }
    }
}
