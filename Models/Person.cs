using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Text.Json.Serialization;

namespace SkillsManagement.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();

        public Person() { }

        public Person(string name, string displayName)
        {
            Name=name;
            DisplayName=displayName;
        }

        public void AddSkill(Skill skill)
        {
            if (skill == null) throw new ArgumentNullException(nameof(skill));
            Skills.Add(skill);
        }

        public bool RemoveSkill(Skill skill)
        {
            if (skill == null) throw new ArgumentNullException(nameof(skill));
            return Skills.Remove(skill);
        }
    }
}
