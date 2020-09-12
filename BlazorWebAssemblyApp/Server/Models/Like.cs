namespace BlazorWebAssemblyApp.Server.Models
{
    public class Like
    {
        public int LikerID { get; set; }
        public int LikeeID { get; set; }
        public virtual User Liker { get; set; }
        public virtual User Likee { get; set; }
    }
}