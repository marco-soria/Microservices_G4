﻿namespace Microservices.Web.Models
{
    public class CouponDto
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }

        public double MinimumAmount { get; set; }
    }
}
