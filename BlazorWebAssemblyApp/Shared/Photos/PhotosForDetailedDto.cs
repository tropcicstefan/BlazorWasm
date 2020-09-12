using System;

namespace BlazorWebAssemblyApp.Shared
{
    public class PhotosForDetailedDto
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
    }
}