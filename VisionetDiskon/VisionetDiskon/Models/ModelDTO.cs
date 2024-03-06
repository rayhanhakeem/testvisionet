using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Mvc;

namespace VisionetDiskon.Models
{
    public class ModelDTO
    {
        public string CustomerType {  get; set; }

        public int RewardPoints { get; set; }
        public int TotalBelanja { get;set; }

    }
}
