
namespace TARge25Shop.Core.Domain
{
    public class Kindergarten
    {
        public Guid? Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string ChildrenCount { get; set; } = string.Empty;
        public int KindergartenName { get; set; }
        public int TeacherName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
