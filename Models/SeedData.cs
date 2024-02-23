namespace SkillsManagement.Models
{
    public class SeedData
    {
        public static void Initialize(SkillsContext db)
        {
            var skills = new Skill[3]
            {
                new Skill("C#", 8),
                new Skill(".NET", 6),
                new Skill("API", 4),
            };
            var employees = new Person[2]
            {
                new Person("Ivanov Ivan", "Ivanov I."),
                new Person("Petrov Petor", "Petrov P."),
            };

            employees[0].AddSkill(skills[0]);
            employees[1].AddSkill(skills[1]);
            employees[1].AddSkill(skills[2]);

            db.Persons.AddRange(employees);
            db.Skills.AddRange(skills);
            db.SaveChanges();
        }

    }
}
