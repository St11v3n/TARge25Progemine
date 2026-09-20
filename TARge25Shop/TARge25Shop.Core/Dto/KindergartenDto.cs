using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25Shop.Core.Dto
{
    public class KindergartenDto
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
