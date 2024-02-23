using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillsManagement.Models
{
    public class Skill
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [Range(1, 10, ErrorMessage = "Level must be between 1 and 10")]
        public byte Level { get; set; }

        [JsonIgnore]
        public virtual ICollection<Person> Persons { get; } = new List<Person>();

        public Skill() { }

        public Skill(string name, byte level)
        {
            Name=name;
            Level=level;
        }
    }
}
