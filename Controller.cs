using Microsoft.EntityFrameworkCore;
using SkillsManagement;
using SkillsManagement.Models;

namespace SkillsManagement_TestTask
{
    public static class Controller
    {
        // CREATE
        public static async Task<IResult> AddPersonAsync(Person person)
        {
            var errorInfo = HandleUserInput(person);
            if (errorInfo != null) return errorInfo;

            using (var db = new SkillsContext())
            {
                await AssignSkillsToPersonAsync(person, db);
                await db.Persons.AddAsync(person);
                db.SaveChanges();
            }
            return Results.Ok();
        }

        // READ
        public static async Task<IResult> GetPersonsAsync()
        {
            using (var db = new SkillsContext())
            {
                var persons = await db.Persons.Include(p => p.Skills).ToListAsync();
                return Results.Ok(persons);
            }
        }

        public static async Task<IResult> GetPersonAsync(long id)
        {
            using (var db = new SkillsContext())
            {
                var person = await db.Persons.Include(p => p.Skills)
                                .FirstOrDefaultAsync(p => p.Id == id);
                if (person == null) return Results.NotFound();

                return Results.Ok(person);
            }
        }

        //UPDATE
        public static async Task<IResult> UpdatePersonAsync(Person newPerson, long id)
        {
            var errorInfo = HandleUserInput(newPerson);
            if (errorInfo != null) return errorInfo;

            using (var db = new SkillsContext())
            {
                var existingPerson = await db.Persons.Include(p => p.Skills)
                                    .FirstOrDefaultAsync(p => p.Id == id);
                if (existingPerson == null) return Results.NotFound();

                // Assign new skills
                existingPerson.Skills = newPerson.Skills;

                await AssignSkillsToPersonAsync(existingPerson, db);
                db.Persons.Update(existingPerson);
                db.SaveChanges();
            }

            return Results.Ok();
        }

        private static async Task AssignSkillsToPersonAsync(Person person, SkillsContext db)
        {
            var employeeSkills = person.Skills.ToList();
            for (int i = 0; i < employeeSkills.Count; i++)
            {
                var skill = employeeSkills[i];
                var existingSkill = await db.Skills.FirstOrDefaultAsync( s =>
                                     s.Name == skill.Name && s.Level == skill.Level);
                if (existingSkill != null)
                {
                    employeeSkills[i] = existingSkill;
                }
            }
            person.Skills = employeeSkills;
        }

        // DELETE
        public static async Task<IResult> DeletePersonAsync(long id)
        {
            using (var db = new SkillsContext())
            {
                var person = await db.Persons.FindAsync(id);
                if (person == null) return Results.NotFound();

                db.Persons.Remove(person);
                db.SaveChanges();
                return Results.Ok();
            }
        }

        private static IResult HandleUserInput(Person person)
        {
            if (person == null || person.Id != 0)
                return Results.BadRequest("Incorrect person info");
            if (person.Skills == null || person.Skills.Count == 0)
                return Results.BadRequest("Nullable person skills");

            foreach (var skill in person.Skills)
            {
                if (skill.Level < 1 || skill.Level > 10)
                    return Results.BadRequest("Skill level must be between 1 and 10");
            }

            return null!;
        }

        public static bool DbIsEmpty()
        {
            using (var db = new SkillsContext())
            {
                var list = db.Persons.ToList();
                if (list.Count == 0)
                {
                    var list2 = db.Skills.ToList();
                    if (list2.Count == 0)
                        return true;
                }
            }
            return false;
        }

        public static void InitializeDb()
        {
            using (var db = new SkillsContext())
            {
                SeedData.Initialize(db);
            }
        }
    }
}
